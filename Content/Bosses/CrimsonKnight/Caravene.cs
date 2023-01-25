using ExoriumMod.Core;
using ExoriumMod.Helpers;
using Microsoft.Xna.Framework;
using System;
using ExoriumMod.Content.Dusts;
using ExoriumMod.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Audio;

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    class Caravene : ModNPC
    {
        public override string Texture => AssetDirectory.CrimsonKnight + Name + "_Hitbox";
        public override string BossHeadTexture => AssetDirectory.CrimsonKnight + Name + "_Head_Boss";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crimson Knight");
            Main.npcFrameCount[NPC.type] = 6;

            //Always draw so visuals don't fail while offscreen
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;

            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.OnFire,
                    BuffID.OnFire3,
                }
            };
            NPCID.Sets.DebuffImmunitySets[Type] = debuffData;

            NPCID.Sets.TrailCacheLength[Type] = 7;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 6666;
            NPC.damage = 29;
            NPC.defense = 11;
            NPC.knockBackResist = 0f;
            NPC.width = 140;
            NPC.height = 240;
            NPC.value = Item.buyPrice(0, 7, 7, 7);
            NPC.npcSlots = 30f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.NPCHit4;
            //NPC.DeathSound = SoundID.NPCDeath52;
            NPC.timeLeft = NPC.activeTime * 30;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.alpha = 255;
            if (!Main.dedServ)
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Sounds/Music/knight");
            //bossBag = ItemType<ShadowmancerBag>();
        }

        //May want to make teleport next to player not damage when teleporting

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.75 * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 0.7);
        }

        private bool left = false;

        private int frameX = 0;

        private int loopCounter = 0;

        //Action trackers
        private bool teleIndicator = false;
        private bool parry = false;
        private float parryFireballTimer = 0;
        private int parryRetaliate = 0;
        private int parryDamaged = 0;
        private static float Parry_Durration = 240;
        private bool shieldDown = false;
        private bool dashIndicator = false;

        //Misc Trackers
        private bool endFlameSpawn = false;
        private float shieldScale = 0f;
        private float timeReachedPortal = 0f;
        private Vector2 playerPlaceholder = Vector2.Zero;
        private bool altBeamType = false;
        private bool noContactDamage = false;
        private bool showPortals = false;
        private float portalSize = 0;
        private float portalLoop = 0;
        private Vector2 bladeSpawnOrigin = Vector2.Zero;
        private int bladeSpawnQuadrant = 0;
        private int bladeSpawnCount = 0;
        private float auraAlpha = 0;


        //Phase trackers
        private bool introAnimation = true;
        private int introTicker = 9999;
        private int introTickerMax = 0;
        private float introPortalSize = 0;

        private bool exitAnimation = false;
        private float exitTicker = 0;
        private float exitPortalSize = 0;

        private int phase = 1;
        private bool phaseTransition = false;
        private int transitionCounter = 0;

        //Portal/Arena Locations
        private static Vector2 topL = ExoriumWorld.FallenTowerRect.TopLeft();
        private static Vector2 topR = ExoriumWorld.FallenTowerRect.TopRight();
        private static Vector2 Arena_Top_Left = topL + new Vector2(250, 400);
        private static Vector2 Arena_Middle_Left = topL + new Vector2(250, 720); //20 tiles to next location so + 320
        private static Vector2 Arena_Bottom_Left = topL + new Vector2(250, 1040);
        private static Vector2 Arena_Top_Right = topR + new Vector2(-250, 400);
        private static Vector2 Arena_Middle_Right = topR + new Vector2(-250, 720);
        private static Vector2 Arena_Bottom_Right = topR + new Vector2(-250, 1040);
        private static List<Vector2> Arena_Portals = new List<Vector2>() { Arena_Top_Left, Arena_Middle_Left, Arena_Bottom_Left, Arena_Top_Right, Arena_Middle_Right, Arena_Bottom_Right };
        private static float Arena_Left = topL.X + 250;
        private static float Arena_Right = topR.X - 250;
        private static List<Vector2> Current_Portals = new List<Vector2>() { Vector2.Zero, Vector2.Zero};
        private static float maxTeleportHeight = ExoriumWorld.FallenTowerRect.Top + 160 + 240;
        private static float minTeleportX = ExoriumWorld.FallenTowerRect.Left + 80;
        private static float maxTeleportX = ExoriumWorld.FallenTowerRect.Right - 80;

        //Actions
        //0 - jump
        //1 - dash
        //2 - Teleport next to player
        //3 - Send down flame ring
        //4 - parry
        //5 - Galacta knight lol
        //6 - swords come down
        //7 - sword beams
        //8 - hop down
        //9 - flame breath
        //10 - portal dash
        //11 - Burning Sphere
        //12 - enrage
        public float Action
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        private float wait = 60;

        private float actionTimer;

        public override void AI()
        {
            //Damage calculations
            int damage = NPC.damage / (Main.expertMode == true ? 4 : 2);

            //Reset vars
            parry = false;
            Vector2 swordTip = new Vector2(NPC.Center.X + (left ? 65 : -65), NPC.Center.Y - NPC.height - 75);

            #region Targeting  
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.TargetClosest(true);
            }

            Player player = Main.player[NPC.target];
            if (!player.active || player.dead && Main.netMode != NetmodeID.MultiplayerClient || !player.getRect().Intersects(ExoriumWorld.FallenTowerRect)) //Also stop targeting if outside of arena
            {
                NPC.TargetClosest(true);
                NPC.netUpdate = true;
                player = Main.player[NPC.target];
                if (!player.active || player.dead || (NPC.position - player.position).Length() > 6000 || !player.getRect().Intersects(ExoriumWorld.FallenTowerRect))
                {
                    if (!player.active || player.dead)//Player died
                    {
                        //A valiant effort
                    }
                    else//Player ran away
                    {
                        //Coward...
                    }
                    exitAnimation = true;
                }
            }
            #endregion

            #region Action Choosing
            //Loop counter
            loopCounter++;
            if (loopCounter >= 20)
                loopCounter = 0;

            //Override normal action
            if (introAnimation)
            {
                IntroAI();
                return;
            }
            else if (phaseTransition)
            {
                PhaseTransition();
                return;
            }
            else if (exitAnimation)
            {
                ExitAI();
                return;
            }
            else if (wait > 0) //What to do while waiting
            {
                NPC.noGravity = false;

                //Hit the ground before starting next action
                if (NPC.velocity.Y == 0)
                {
                    NPC.velocity = Vector2.Zero;
                    wait--;
                }

                //Change animation when attack starts
                if (wait == 0)
                {
                    switch (Action)
                    {
                        case 0:
                            frameX = 9;
                            break;
                        case 1:
                            endFlameSpawn = false;
                            frameX = 10;
                            break;
                        case 2:
                            frameX = 3;
                            break;
                        case 3:
                            frameX = 1;
                            break;
                        case 4:
                            frameX = 7;
                            break;
                        case 5:
                            frameX = 5;
                            break;
                        case 6:
                            frameX = 5;
                            break;
                        case 7:
                            frameX = 3;
                            break;
                        case 8:
                            frameX = 9;
                            break;
                        case 9:
                            frameX = 0;
                            break;
                        case 10:
                            endFlameSpawn = false;
                            frameX = 0;
                            break;
                        case 11:
                            frameX = 5;
                            break;
                        case 12:
                            frameX = 5;
                            break;
                    }

                    NPC.frameCounter = 0;
                }
                return;
            }

            //Choose facing direction
            if (actionTimer == 0)
            {
                if ((NPC.Center - player.Center).X > 0)
                    left = false;
                else
                    left = true;
            }

            //Phase check
            if (phase == 1 && NPC.life < NPC.lifeMax/2)
            {
                phase = 2;
                phaseTransition = true;
            }
            #endregion

            #region Actions
            //Action to make
            switch (Action)
            {
                case 0:
                    NPC.noGravity = false;
                    if (actionTimer == 0)
                    {
                        float xDiff = player.Center.X - NPC.Center.X;
                        NPC.velocity = new Vector2(xDiff / 150, -16);
                    }
                    else
                    {
                        if (NPC.velocity.Y == 0f)
                        {
                            NPC.velocity.X *= 0.8f;
                            frameX = 0;
                        }

                        if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
                        {
                            NPC.velocity.X = 0f;
                        }
                    }

                    if (NPC.velocity == Vector2.Zero)
                    {
                        ChooseAttack();
                    }
                    break;
                case 1:
                    if (actionTimer <= 60)
                        dashIndicator = true;
                    else if (actionTimer < 150)
                    {
                        dashIndicator = false;
                        NPC.velocity = new Vector2(20, 0) * (left ? 1 : -1);

                        //Flame trail
                        Vector2 swordPoint = NPC.Bottom + new Vector2(left ? NPC.width * 1.5f : -NPC.width * 1.5f, -14);
                        if (Main.tile[swordPoint.ToTileCoordinates().X, swordPoint.ToTileCoordinates().Y].WallType != WallType<Walls.StructureWalls.FallenTowerWalls.CharredObsidianWall>())
                            endFlameSpawn = true;
                        if ((phase == 2 || Main.masterMode) && !endFlameSpawn)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), swordPoint, Vector2.Zero, ProjectileType<FlameTrail>(), damage, 0);
                            if (Main.rand.NextBool(2))
                            {
                                if (left)
                                {
                                    Dust.NewDust(NPC.Bottom + new Vector2(NPC.width * 1.5f, 0), 0, 0, DustID.SolarFlare, Main.rand.Next(-5, -4), 3);
                                    Dust.NewDust(NPC.Bottom + new Vector2(NPC.width * 1.5f, 0), 0, 0, DustID.SolarFlare, Main.rand.Next(-7, -6), -7);
                                }
                                else
                                {
                                    Dust.NewDust(NPC.Bottom + new Vector2(-NPC.width * 1.5f, 0), 0, 0, DustID.SolarFlare, Main.rand.Next(4, 5), 3);
                                    Dust.NewDust(NPC.Bottom + new Vector2(-NPC.width * 1.5f, 0), 0, 0, DustID.SolarFlare, Main.rand.Next(6, 7), -7);
                                }
                            }
                        }

                        //Push out of wall
                        Vector2 sideCheck = left ? NPC.Right : NPC.Left;
                        if (Main.tile[sideCheck.ToTileCoordinates().X, sideCheck.ToTileCoordinates().Y].HasTile)
                        {
                            int counter = 0; //Limit loops to 200 just in case
                            while (Main.tile[sideCheck.ToTileCoordinates().X, sideCheck.ToTileCoordinates().Y].HasTile && counter < 200)
                            {
                                NPC.position.X += (left? -2: 2);
                                sideCheck = left ? NPC.Right : NPC.Left;
                                counter++;
                            }
                        }
                    }
                    else if (actionTimer == 150)
                    {
                        NPC.velocity = Vector2.Zero;
                    }
                    else if (actionTimer == 155 && Main.expertMode)
                    {
                        frameX = 3;

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                Vector2 vel = new Vector2(0, -14);
                                vel = vel.RotatedBy(MathHelper.ToRadians(-30 + (15 * i)));
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Bottom + new Vector2(left ? NPC.width * 1.5f : -NPC.width * 1.5f, 0), vel, ProjectileType<CaraveneFireball>(), damage, 2, Main.myPlayer, 0, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                            }
                        }
                    }
                    if (actionTimer >= 180)
                    {
                        ChooseAttack();
                    }
                    break;
                case 2:
                    if (actionTimer == 0)
                        teleIndicator = true;
                    else if (actionTimer == 90)
                    {
                        Vector2 offset = new Vector2(!left ? NPC.width : -NPC.width, -NPC.height/2.4f);

                        //Find point to dash to
                        //Adjust to not leave arena
                        Vector2 coordinatesOfDash = player.Center + offset;
                        if (coordinatesOfDash.Y < maxTeleportHeight - NPC.height / 2)
                            coordinatesOfDash.Y = maxTeleportHeight - NPC.height / 2;
                        if (coordinatesOfDash.X < minTeleportX + NPC.width / 2)
                            coordinatesOfDash.X = minTeleportX + NPC.width / 2;
                        if (coordinatesOfDash.X > maxTeleportX - NPC.width / 2)
                            coordinatesOfDash.X = maxTeleportX - NPC.width / 2;

                        Vector2 dash = coordinatesOfDash - NPC.Center;

                        NPC.velocity = dash / 4;
                        NPC.noGravity = true;
                        NPC.noTileCollide = true;
                        teleIndicator = false;
                        noContactDamage = true;
                    }
                    else if (actionTimer == 94)
                    {
                        NPC.velocity = Vector2.Zero;
                        noContactDamage = false;
                    }
                    else if (actionTimer == 140)
                    {
                        NPC.noGravity = false;
                        NPC.noTileCollide = false;
                        frameX = 1;
                        NPC.frameCounter = 0;
                    }
                    else if (actionTimer >= 160)
                    {
                        ChooseMovement();
                    }
                    break;
                case 3:
                    if (actionTimer == 5)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (int i = 0; i < 12; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center + new Vector2(0, -900), Vector2.Zero, ProjectileType<FireballRing>(), damage, 1, Main.myPlayer, (MathHelper.Pi / 6) * i, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                            }
                        }

                        //SOUND explosion or swing sound

                        // dust telegraph
                        for (int i = 0; i < 100; i++)
                        {
                            Vector2 pos = player.Center;
                            pos.Y -= 900;
                            pos.X -= 500;
                            Dust.NewDust(pos, 1000, 1, DustID.SolarFlare, 0, Main.rand.NextFloat(20));
                        }
                    }
                    else if (actionTimer == 60)
                    {
                        ChooseMovement();
                    }
                    break;
                case 4:
                    if (actionTimer == 0)
                    {
                        parryDamaged = 0;
                        parryDamaged = 0;
                    }
                    else if (actionTimer > 60 && actionTimer < Parry_Durration)
                        parry = true;
                    else if (actionTimer == Parry_Durration)
                    {
                        parry = false;
                        shieldDown = true;
                        NPC.frameCounter = 0;
                        bool sound = false;

                        //Use same formula as draw to create projectiles

                        for (int i = 0; i < parryRetaliate; i++)
                        {
                            Vector2 offset = new Vector2(0, 200);
                            offset = offset.RotatedBy(MathHelper.ToRadians((360 / parryRetaliate) * i));
                            offset = offset.RotatedBy(Main.GameUpdateCount * .0001);
                            Vector2 toPlayer = player.Center - (NPC.Center + offset);
                            toPlayer.Normalize();
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + offset, toPlayer * 18, ProjectileType<backupFireball>(), damage * 2, 3, Main.myPlayer, player.whoAmI);
                            }
                            SoundEngine.PlaySound(SoundID.Item20, NPC.Center + offset);
                        }

                        //Reset Trackers
                        parryRetaliate = 0;
                        parryDamaged = 0;
                    }
                    else if (actionTimer > Parry_Durration + 40)
                    {
                        ChooseMovement();
                    }
                    break;
                case 5:
                    if (actionTimer == 0)
                    {
                        //Only accept negative y quadrants in phase 2
                        do
                        {
                            bladeSpawnQuadrant = Main.rand.Next(1, 5);
                        }
                        while (phase != 2 && (bladeSpawnQuadrant == 2 || bladeSpawnQuadrant == 3));

                        if (bladeSpawnQuadrant == 1 || bladeSpawnQuadrant == 4)
                        {
                            bladeSpawnOrigin = new Vector2(ExoriumWorld.FallenTowerRect.Center.X, ExoriumWorld.FallenTowerRect.Bottom);
                        }
                        else
                        {
                            bladeSpawnOrigin = new Vector2(ExoriumWorld.FallenTowerRect.Center.X, ExoriumWorld.FallenTowerRect.Top);
                        }

                        Vector2 offset = new Vector2(1, -1);
                        offset *= 1800;
                        offset = offset.RotatedBy(MathHelper.ToRadians(90 * (bladeSpawnQuadrant - 1)));

                        bladeSpawnOrigin += offset;

                        bladeSpawnCount = Main.expertMode ? 20 : 15;
                        if (Main.masterMode)
                            bladeSpawnCount -= 5;
                        if (phase == 2)
                            bladeSpawnCount -= 5;
                    }
                    else if (actionTimer > 30 && actionTimer % bladeSpawnCount == 0 && actionTimer < 360)
                    {
                        Vector2 spawnPoint = bladeSpawnOrigin;
                        spawnPoint.X += Main.rand.NextFloat(-(ExoriumWorld.FallenTowerRect.Width / 2) + 80, ExoriumWorld.FallenTowerRect.Width / 2 - 80);

                        Vector2 direction = new Vector2(-1, 1);
                        direction *= 10;
                        direction = direction.RotatedBy(MathHelper.ToRadians(90 * (bladeSpawnQuadrant - 1)));

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPoint, direction, ProjectileType<ReboundingSword>(), damage, 1, Main.myPlayer, 60, (bladeSpawnQuadrant == 1 || bladeSpawnQuadrant == 4)? 1f : 0f);
                    }
                    if (actionTimer >= 420)
                    {
                        ChooseFollowup();
                    }

                    if (actionTimer < 60 && actionTimer > 12) //Indicators
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient && actionTimer % 6 == 0)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), swordTip, new Vector2(0, -30).RotatedByRandom(MathHelper.Pi/16), ProjectileType<indicatorRainSword>(), damage, 1, Main.myPlayer, 60, (bladeSpawnQuadrant == 1 || bladeSpawnQuadrant == 4) ? 1f : 0f);
                            SoundEngine.PlaySound(SoundID.Item100, swordTip);
                        }
                    }
                    break;
                case 6:
                    if (actionTimer == 0)
                    {
                        altBeamType = false;
                        if (Main.rand.NextBool(2) && Main.masterMode)
                            altBeamType = true;
                        playerPlaceholder = player.Center;
                    }
                    if ((actionTimer % 10 == 0) && Main.netMode != NetmodeID.MultiplayerClient )
                    {
                        if (phase == 2)
                        {
                            if (altBeamType)
                            {
                                if (actionTimer % 20 == 0)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center + new Vector2(Main.rand.NextFloat(-800, 800), -400), Vector2.Zero, ProjectileType<CaraveneBladeProj>(), (int)(damage * 1.5f), 1, Main.myPlayer, 0, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                                }

                                if (actionTimer < 70)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPlaceholder + new Vector2(-700, 50 * (actionTimer / 10)), Vector2.Zero, ProjectileType<CaraveneBladeProjHorizontal>(), (int)(damage * 1.5f), 1, Main.myPlayer, 0, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPlaceholder + new Vector2(700, 50 * (actionTimer / 10)), Vector2.Zero, ProjectileType<CaraveneBladeProjHorizontal>(), (int)(damage * 1.5f), 1, Main.myPlayer, 1, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPlaceholder + new Vector2(-700, -50 * (actionTimer / 10)), Vector2.Zero, ProjectileType<CaraveneBladeProjHorizontal>(), (int)(damage * 1.5f), 1, Main.myPlayer, 0, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPlaceholder + new Vector2(700, -50 * (actionTimer / 10)), Vector2.Zero, ProjectileType<CaraveneBladeProjHorizontal>(), (int)(damage * 1.5f), 1, Main.myPlayer, 1, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                                }
                            }
                            else
                            {
                                if (actionTimer % 20 == 0)
                                {
                                    if (Main.rand.NextBool(2))
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center + new Vector2(-700, Main.rand.NextFloat(-600, 600)), Vector2.Zero, ProjectileType<CaraveneBladeProjHorizontal>(), (int)(damage * 1.5f), 1, Main.myPlayer, 0, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                                    else
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center + new Vector2(700, Main.rand.NextFloat(-600, 600)), Vector2.Zero, ProjectileType<CaraveneBladeProjHorizontal>(), (int)(damage * 1.5f), 1, Main.myPlayer, 1, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                                }

                                //create more projectiles in phase two, done this way rather than changing the first if statement so that the increased projectiles are fired in pairs so the rhthym of the attack is kept consistant
                                if (actionTimer < 130)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPlaceholder + new Vector2(50 * (actionTimer / 10), -400), Vector2.Zero, ProjectileType<CaraveneBladeProj>(), (int)(damage * 1.5f), 1, Main.myPlayer, 0, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPlaceholder + new Vector2(-50 * (actionTimer / 10), -400), Vector2.Zero, ProjectileType<CaraveneBladeProj>(), (int)(damage * 1.5f), 1, Main.myPlayer, 0, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                                }
                            }

                        }
                        else
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPlaceholder + new Vector2((Main.expertMode ? 300 : 400) * (actionTimer / 10), -400), Vector2.Zero, ProjectileType<CaraveneBladeProj>(), (int)(damage * 1.5f), 1, Main.myPlayer, 0, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPlaceholder + new Vector2((Main.expertMode ? -300 : -400) * (actionTimer / 10), -400), Vector2.Zero, ProjectileType<CaraveneBladeProj>(), (int)(damage * 1.5f), 1, Main.myPlayer, 0, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                        }
                    }
                    if (actionTimer >= 150)
                    {
                        ChooseFollowup();
                    }
                    break;
                case 7:
                    if (Main.netMode != NetmodeID.MultiplayerClient && actionTimer == 0)
                    {
                        if (!Main.expertMode && phase != 2)
                        {
                            Vector2 vel = new Vector2(left ? 1 : -1, 0);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + (vel * 150), vel * .01f, ProjectileType<FlametoungeBeam>(), damage, 2, Main.myPlayer, 60);
                        }
                        else
                        {
                            //Check elevation level in arena
                            if (NPC.Center.Y < topL.Y + 544)
                            {
                                for (int i = 0; i < 7; i += ((phase == 2 && Main.expertMode) ? 1 : 2))
                                {
                                    Vector2 vel = new Vector2(left ? 1 : -1, 0);
                                    vel = vel.RotatedBy(MathHelper.ToRadians(10 * i * (left ? 1 : -1)));
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + (vel * 150), vel * .01f, ProjectileType<FlametoungeBeam>(), damage, 2, Main.myPlayer, 60);
                                }
                            }
                            else if (NPC.Center.Y < topL.Y + 864)
                            {
                                for (int i = 0; i < 7; i += ((phase == 2 && Main.expertMode) ? 1 : 2))
                                {
                                    Vector2 vel = new Vector2(left ? 1 : -1, 0);
                                    vel = vel.RotatedBy(MathHelper.ToRadians(35 * (left ? -1 : 1)));
                                    if (phase != 2) // center the swords in phase 1
                                        vel = vel.RotatedBy(MathHelper.ToRadians(5 * (left ? 1 : -1)));
                                    vel = vel.RotatedBy(MathHelper.ToRadians(10 * i * (left ? 1 : -1)));
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + (vel * 150), vel * .01f, ProjectileType<FlametoungeBeam>(), damage, 2, Main.myPlayer, 60);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < 7; i += ((phase == 2 && Main.expertMode) ? 1 : 2))
                                {
                                    Vector2 vel = new Vector2(left ? 1 : -1, 0);
                                    vel = vel.RotatedBy(MathHelper.ToRadians(70 * (left ? -1 : 1)));
                                    if (phase != 2) // allign the swords in phase 1
                                        vel = vel.RotatedBy(MathHelper.ToRadians(10 * (left ? 1 : -1)));
                                    vel = vel.RotatedBy(MathHelper.ToRadians(10 * i * (left ? 1 : -1)));
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + (vel * 150), vel * .01f, ProjectileType<FlametoungeBeam>(), damage, 2, Main.myPlayer, 60);
                                }
                            }
                        }
                    }
                    if (actionTimer >= 40)
                    {
                        ChooseFollowup();
                    }
                    break;
                case 8:
                    if (NPC.Bottom.Y > Main.player[NPC.target].Bottom.Y + 20 && actionTimer > 60 && actionTimer < 100)
                    {
                        actionTimer = 100;
                    }
                    if (NPC.Bottom.Y < Arena_Bottom_Left.Y)
                    {
                        actionTimer = 120;
                    }
                    if (actionTimer == 0)
                    {
                        NPC.velocity = new Vector2(0, -9);
                        NPC.noGravity = false;
                        NPC.noTileCollide = true;
                    }
                    else if (actionTimer > 120)
                    {
                        NPC.noTileCollide = false;
                    }
                    if (NPC.velocity == Vector2.Zero)
                    {
                        frameX = 0;
                        ChooseAttack();
                    }
                    break;
                case 9:
                    NPC.velocity = Vector2.Zero;
                    if (actionTimer == 0)
                    {
                        frameX = 5;
                    }
                    else if (actionTimer == 30)
                    {
                        //Flames shoot from arena walls, if they come in contact with each other they explode into flame\
                        Rectangle arena = ExoriumWorld.FallenTowerRect;

                        //place along top, right, bottom, left
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int counter = 0;

                            //Top and bottom
                            for (int i = (-arena.Width / 2) + 160; i < (arena.Width / 2) - 160; i += Main.rand.Next(40,280))
                            {
                                if (Main.rand.NextBool(4))
                                {
                                    int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), swordTip, Vector2.Zero, ProjectileType<GridFire>(), damage, 2, Main.myPlayer, 1, i);
                                }
                                if (Main.rand.NextBool(4))
                                {
                                    int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), swordTip, Vector2.Zero, ProjectileType<GridFire>(), damage, 2, Main.myPlayer, 3, i);
                                }
                                
                                counter += 5;
                            }

                            counter = 0;

                            //Left and right
                            for (int i = (-arena.Height / 2) + 160; i < (arena.Height / 2) - 160; i += Main.rand.Next(40, 280))
                            {
                                if (Main.rand.NextBool(5))
                                {
                                    int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), swordTip, Vector2.Zero, ProjectileType<GridFire>(), damage, 2, Main.myPlayer, 2, i);
                                }
                                if (Main.rand.NextBool(5))
                                {
                                    int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), swordTip, Vector2.Zero, ProjectileType<GridFire>(), damage, 2, Main.myPlayer, 4, i);
                                }

                                counter += 5;
                            }
                        }
                        SoundEngine.PlaySound(SoundID.Item45, swordTip);
                    }
                    else if (actionTimer > 180)
                    {
                        ChooseFollowup();
                    }
                    break;
                case 10:
                    if (actionTimer == 0)
                    {
                        //Set up portal locations
                        Current_Portals.Clear();

                        int portal = -1;
                        //Check elevation level in arena
                        if (NPC.Center.Y < topL.Y + 544)
                        {
                            portal = 0;
                        }
                        else if (NPC.Center.Y < topL.Y + 864)
                        {
                            portal = 1;
                        }
                        else
                        {
                            portal = 2;
                        }
                        if (left)
                            portal += 3;

                        Current_Portals.Add(Arena_Portals[portal]);

                        for (int i = 0; i < (Main.masterMode? 3: 1); i++) //Add extra portals in master mode
                        {
                            Vector2 randPortal = Vector2.Zero;
                            do
                            {
                                randPortal = Arena_Portals[Main.rand.Next(Arena_Portals.Count)];
                            }
                            while (Current_Portals.Contains(randPortal));
                            Current_Portals.Add(randPortal);
                        }

                        showPortals = true;
                    }
                    if (actionTimer <= 60)
                        dashIndicator = true;
                    else if (actionTimer <= 600 && timeReachedPortal == 0)
                    {
                        dashIndicator = false;
                        NPC.velocity = new Vector2(20, 0) * (left ? 1 : -1);

                        //Flame trail
                        Vector2 swordPoint = NPC.Bottom + new Vector2(left ? NPC.width * 1.5f : -NPC.width * 1.5f, -14);
                        if (Main.tile[swordPoint.ToTileCoordinates().X, swordPoint.ToTileCoordinates().Y].WallType != WallType<Walls.StructureWalls.FallenTowerWalls.CharredObsidianWall>())
                            endFlameSpawn = true;
                        if (phase == 2 && !endFlameSpawn)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), swordPoint, Vector2.Zero, ProjectileType<FlameTrail>(), damage, 0);
                            if (Main.rand.NextBool(2))
                            {
                                if (left)
                                {
                                    Dust.NewDust(NPC.Bottom + new Vector2(NPC.width * 1.5f, 0), 0, 0, DustID.SolarFlare, Main.rand.Next(-5, -4), 3);
                                    Dust.NewDust(NPC.Bottom + new Vector2(NPC.width * 1.5f, 0), 0, 0, DustID.SolarFlare, Main.rand.Next(-7, -6), -7);
                                }
                                else
                                {
                                    Dust.NewDust(NPC.Bottom + new Vector2(-NPC.width * 1.5f, 0), 0, 0, DustID.SolarFlare, Main.rand.Next(4, 5), 3);
                                    Dust.NewDust(NPC.Bottom + new Vector2(-NPC.width * 1.5f, 0), 0, 0, DustID.SolarFlare, Main.rand.Next(6, 7), -7);
                                }
                            }
                        }

                        //Push out of wall
                        Vector2 sideCheck = left ? NPC.Right : NPC.Left;
                        if (Main.tile[sideCheck.ToTileCoordinates().X, sideCheck.ToTileCoordinates().Y].HasTile)
                        {
                            int counter = 0; //Limit loops to 200 just in case
                            while (Main.tile[sideCheck.ToTileCoordinates().X, sideCheck.ToTileCoordinates().Y].HasTile && counter < 200)
                            {
                                NPC.position.X += (left ? -2 : 2);
                                sideCheck = left ? NPC.Right : NPC.Left;
                                counter++;
                            }
                        }

                        //Mark time and teleport when portal is touched
                        if ((NPC.Center.X < Arena_Left && !left) ||
                            NPC.Center.X > Arena_Right && left)
                        {
                            timeReachedPortal = actionTimer + 1;
                            endFlameSpawn = false;

                            //Teleport to random portal
                            NPC.Center = Current_Portals[1];
                            NPC.position.Y += 8;
                            if (NPC.Center.X < Arena_Top_Left.X + 800) //Arbitrary point chosen here to find what side of the arena teleported to
                                left = true;
                            else
                                left = false;
                        }
                    }
                    else if (actionTimer < timeReachedPortal + 30)
                    {
                        noContactDamage = true;
                        NPC.velocity = Vector2.Zero;
                        NPC.alpha = 255;
                    }
                    else if (actionTimer < timeReachedPortal + 120) //Move out of next portal for 2 seconds
                    {
                        if (NPC.alpha > 255)
                            NPC.alpha -= 15;
                        else
                            showPortals = false;

                        noContactDamage = false;
                        NPC.velocity = new Vector2(20, 0) * (left ? 1 : -1);

                        //Flame trail
                        Vector2 swordPoint = NPC.Bottom + new Vector2(left ? NPC.width * 1.5f : -NPC.width * 1.5f, -14);
                        if (Main.tile[swordPoint.ToTileCoordinates().X, swordPoint.ToTileCoordinates().Y].WallType != WallType<Walls.StructureWalls.FallenTowerWalls.CharredObsidianWall>())
                            endFlameSpawn = true;
                        if (phase == 2 && !endFlameSpawn)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), swordPoint, Vector2.Zero, ProjectileType<FlameTrail>(), damage, 0);
                            if (Main.rand.NextBool(2))
                            {
                                if (left)
                                {
                                    Dust.NewDust(NPC.Bottom + new Vector2(NPC.width * 1.5f, 0), 0, 0, DustID.SolarFlare, Main.rand.Next(-5, -4), 3);
                                    Dust.NewDust(NPC.Bottom + new Vector2(NPC.width * 1.5f, 0), 0, 0, DustID.SolarFlare, Main.rand.Next(-7, -6), -7);
                                }
                                else
                                {
                                    Dust.NewDust(NPC.Bottom + new Vector2(-NPC.width * 1.5f, 0), 0, 0, DustID.SolarFlare, Main.rand.Next(4, 5), 3);
                                    Dust.NewDust(NPC.Bottom + new Vector2(-NPC.width * 1.5f, 0), 0, 0, DustID.SolarFlare, Main.rand.Next(6, 7), -7);
                                }
                            }
                        }

                        //Push out of wall
                        Vector2 sideCheck = left ? NPC.Right : NPC.Left;
                        if (Main.tile[sideCheck.ToTileCoordinates().X, sideCheck.ToTileCoordinates().Y].HasTile)
                        {
                            int counter = 0; //Limit loops to 200 just in case
                            while (Main.tile[sideCheck.ToTileCoordinates().X, sideCheck.ToTileCoordinates().Y].HasTile && counter < 200)
                            {
                                NPC.position.X += (left ? -2 : 2);
                                sideCheck = left ? NPC.Right : NPC.Left;
                                counter++;
                            }
                        }
                    }
                    else if (actionTimer == timeReachedPortal + 120)
                    {
                        NPC.velocity = Vector2.Zero;
                    }
                    else if (actionTimer == timeReachedPortal + 125)
                    {
                        frameX = 3;

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                Vector2 vel = new Vector2(0, -14);
                                vel = vel.RotatedBy(MathHelper.ToRadians(-30 + (15 * i)));
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Bottom + new Vector2(left ? NPC.width * 1.5f : -NPC.width * 1.5f, 0), vel, ProjectileType<CaraveneFireball>(), damage, 2, Main.myPlayer, 0, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                            }
                        }
                    }
                    else if (actionTimer >= timeReachedPortal + 150)
                    {
                        timeReachedPortal = 0;

                        ChooseAttack();
                    }
                    break;
                case 11:
                    NPC.velocity = Vector2.Zero;
                    NPC.noGravity = false;
                    if (actionTimer == 60 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + (left ? 65 : -65), NPC.Center.Y - NPC.height - 75), Vector2.Zero, ProjectileType<FlamingSphere>(), damage * 2, 1, Main.myPlayer, NPC.target);
                    }
                    if (actionTimer == 60)
                    {
                        SoundEngine.PlaySound(SoundID.Item45, swordTip);
                    }
                    else if (actionTimer > 180)
                    {
                        ChooseMovement();
                    }
                    break;
                case 12:
                    bool playSound = false;
                    if (actionTimer == 30 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 toPlayer = player.Center - swordTip;
                        playSound = true;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), swordTip, toPlayer, ProjectileType<backupFireball>(), damage, 3, Main.myPlayer);
                    }
                    else if (actionTimer == 40 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 toPlayer = player.Center - swordTip;
                        playSound = true;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), swordTip, toPlayer.RotatedBy(MathHelper.ToRadians(20)), ProjectileType<backupFireball>(), damage, 3, Main.myPlayer);
                    }
                    else if (actionTimer == 50 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 toPlayer = player.Center - swordTip;
                        playSound = true;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), swordTip, toPlayer.RotatedBy(MathHelper.ToRadians(-20)), ProjectileType<backupFireball>(), damage, 3, Main.myPlayer);
                    }
                    else if (actionTimer == 80)
                    {
                        frameX = 1;
                    }
                    else if (actionTimer > 90)
                    {
                        ChooseMovement();
                    }
                    if (playSound)
                    {
                        SoundEngine.PlaySound(SoundID.Item100, swordTip);
                    }
                    break;
            }

            actionTimer++;
#endregion
        }

        #region Action Helper Methods
        private void ChooseMovement()
        {
            if (Main.rand.NextBool(2))
            {
                //Check elevation level in arena
                if (NPC.Center.Y < topL.Y + 544)
                {
                    Action = 8;
                    wait = 20;
                }
                else if (NPC.Center.Y < topL.Y + 864)
                {
                    if (Main.rand.NextBool(2))
                    {
                        Action = 8;
                        wait = 20;
                    }
                    else
                    {
                        Action = 0;
                        wait = 10;
                    }
                }
                else
                {
                    Action = 0;
                    wait = 10;
                }
            }
            else
            {
                if (phase == 2 && Main.rand.NextBool(3))
                {
                    Action = 10;
                }
                else
                    Action = 1;
                wait = 90;
            }

            actionTimer = -1;
        }

        private void ChooseAttack()
        {
            if (Main.rand.NextBool(4) && phase == 2 && Main.expertMode)
            {
                Action = 9;
                wait = 30;
            }
            else  if (Main.rand.NextBool(3))
            {
                Action = 5;
                wait = 30;
            }
            else if (Main.rand.NextBool(2))
            {
                Action = 7;
                wait = 20;
            }
            else
            {
                Action = 6;
                wait = 90;
            }

            actionTimer = -1;
        }
        
        private void ChooseFollowup()
        {
            if (Main.rand.NextBool(4) && phase == 2)
            {
                Action = 11;

                //Use flaming sphere if there is no flaming sphere
                foreach (Projectile p in Main.projectile)
                {
                    if (p.type == ProjectileType<FlamingSphere>())
                        Action = 12;
                }

                wait = 90;
            }
            else if (Main.rand.NextBool(3))
            {
                Action = 4;
                wait = 5;
            }
            else if (Main.rand.NextBool(2))
            {
                Action = 3;
                wait = 20;
            }
            else
            {
                Action = 2;
                wait = 5;
            }

            actionTimer = -1;
        }
        #endregion

        public override void FindFrame(int frameHeight)
        {
            //Increment speed changed by column
            switch (frameX)
            {
                case 0:
                case 2:
                case 4:
                case 6:
                case 8:
                case 10: 
                    NPC.frameCounter += 2;
                    break;

                case 1:
                case 3:
                    if (NPC.frameCounter == 0)
                    {
                        SoundEngine.PlaySound(SoundID.Item1, NPC.Center);
                    }
                    else if (NPC.frameCounter == 20)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2((NPC.width * (left? 1: -1)), 80), Vector2.Zero, ProjectileType<SwordHitbox>(), NPC.damage, 7, Main.myPlayer);
                    }
                    NPC.frameCounter += 5;
                    break;
                case 7:
                    NPC.frameCounter += 5;
                    break;

                case 5:
                case 9:
                    NPC.frameCounter += 3;
                    break;
            }

            //Change column of animation used after a loop
            if (NPC.frameCounter >= 60)
            {
                NPC.frameCounter = 0;

                switch (frameX)
                {
                    case 1:
                    case 3:
                    case 5:
                    case 9:
                        frameX--;
                        break;
                    case 6:
                        if (shieldDown)
                            frameX++;
                        break;
                    case 7:
                        if (shieldDown)
                        {
                            frameX = 0;
                            shieldDown = false;
                        }
                        else
                            frameX--;
                        break;
                }
            }
        }

        //Play intro animation
        private void IntroAI()
        {
            if (introTicker == 9999) //Set ticker based on past fights
            {
                introTicker = 480;
                introTickerMax = introTicker;
            }

            introTicker--;
            if (introTicker <= 0)
                introAnimation = false;
            
            foreach (Player player in Main.player)
            {
                //Set each player's screen target if not set
                if ((player.Center - NPC.Center).Length() < 3000 && player.GetModPlayer<ExoriumPlayer>().ScreenMoveTarget == Vector2.Zero)
                {
                    player.GetModPlayer<ExoriumPlayer>().ScreenMoveTarget = NPC.Center;
                    player.GetModPlayer<ExoriumPlayer>().ScreenMoveTime = introTicker;
                }
            }

            //TODO: This changes based on past fights
            if (introTicker == introTickerMax - 180) //3 seconds after
            {
                //That's a nice trinket you got there
            }
            else if (introTicker == introTickerMax - 240)
            {
                //Hope you don't mind if I take it off your hands
            }
        }

        //Play phase transition
        private void PhaseTransition()
        {
            if (transitionCounter >= 260)
            {
                auraAlpha -= 9;
                if (auraAlpha < 0)
                    auraAlpha = 0;
            }
            else
            {
                auraAlpha += 9;
                if (auraAlpha > 255)
                    auraAlpha = 255;
            }

            if (transitionCounter == 0)
            {
                //Reset Trackers
                NPC.velocity = Vector2.Zero;
                frameX = 0;
                NPC.frameCounter = 0;
                dashIndicator = false;
                teleIndicator = false;
                parry = false;
                parryDamaged = 0;
                parryRetaliate = 0;
                parryFireballTimer = 0;
                shieldDown = false;
                NPC.noTileCollide = false;
                NPC.noGravity = false;
                noContactDamage = false;

                RemoveProjectiles();
            }
            else if (transitionCounter == 60)
            {
                foreach (Player player in Main.player)
                {
                    //Set each player's screen target if not set
                    if ((player.Center - NPC.Center).Length() < 9000 && player.GetModPlayer<ExoriumPlayer>().ScreenMoveTarget == Vector2.Zero)
                    {
                        player.GetModPlayer<ExoriumPlayer>().ScreenMoveTarget = NPC.Center;
                        player.GetModPlayer<ExoriumPlayer>().ScreenMoveTime = 250;
                    }
                }
            }
            else if (transitionCounter == 90)
            {
                frameX = 5;
            }
            else if (transitionCounter > 120 && transitionCounter < 210)
            {
                for (int i = 0; i < 20; i++)
                {
                    Vector2 rad = new Vector2(0, Main.rand.NextFloat(30));
                    Vector2 shootPoint = rad.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi));
                    Dust dust = Dust.NewDustPerfect(NPC.Center, DustID.SolarFlare, shootPoint, 1, default, 1 + Main.rand.NextFloat(-.5f, .5f));
                    dust.noGravity = true;
                    dust.color = new Color(184, 58, 24);
                }
            }
            else if (transitionCounter == 210)
            {
                frameX = 1;
            }
            else if (transitionCounter == 290)
            {
                phaseTransition = false;

                ChooseMovement();
            }
            transitionCounter++;
        }

        //Play exit animation
        private void ExitAI()
        {
            exitTicker++;

            if (exitTicker > 210)
            {
                NPC.active = false;
            }
        }

        //Kill all boss projectiles so no cheap hits when camera is moved
        private void RemoveProjectiles()
        {
            foreach (Projectile p in Main.projectile)
            {
                if (p.type == ProjectileType<FlameTrail>() ||
                    p.type == ProjectileType<CaraveneBladeProj>() ||
                    p.type == ProjectileType<CaraveneBladeProjHorizontal>() ||
                    p.type == ProjectileType<CaraveneFireball>() ||
                    p.type == ProjectileType<GridFire>() ||
                    p.type == ProjectileType<gridCollision>() ||
                    p.type == ProjectileType<gridShot>() ||
                    p.type == ProjectileType<FlamingSphere>() ||
                    p.type == ProjectileType<FireballRing>() || 
                    p.type == ProjectileType<ReboundingSword>() ||
                    p.type == ProjectileType<backupFireball>() ||
                    p.type == ProjectileType<ReboundingSword>() ||
                    p.type == ProjectileType<FlametoungeBeam>())
                {
                    p.timeLeft = 1;
                    p.Kill();
                }
            }
        }

        public override void OnKill()
        {
            RemoveProjectiles();
            if (Main.netMode != NetmodeID.MultiplayerClient)
                NPC.NewNPCDirect(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Bottom.Y, NPCType<CaraveneBattleIntermission>(), default, default, 300);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.CrimsonKnight + Name).Value;

            int ySourceHeight = (int)(NPC.frameCounter / 10) * 442;
            int xSourceHeight = (int)(frameX * 412);

            //Fire Aura
            var fire = Filters.Scene["ExoriumMod:FireAura"].GetShader().Shader;
            fire.Parameters["noiseTexture"].SetValue(Request<Texture2D>(AssetDirectory.ShaderMap + "FlamingSphere").Value);
            fire.Parameters["gradientTexture"].SetValue(Request<Texture2D>(AssetDirectory.ShaderMap + "basicGradient").Value);
            fire.Parameters["uTime"].SetValue(Main.GameUpdateCount * 0.02f);

            if (true)
            {
                Texture2D auraTex = Request<Texture2D>(AssetDirectory.CrimsonKnight + "Aura").Value;
                Color alpha = new Color(255, 255, 255, auraAlpha);

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.NonPremultiplied, default, default, default, fire, Main.GameViewMatrix.ZoomMatrix);

                spriteBatch.Draw(auraTex, NPC.Center - screenPos + new Vector2(0, -150), null, Color.White, 0, auraTex.Size() / 2, 1.8f, 0, 0);

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
            }

            //ShieldDown needs frames to loop backwards
            if (frameX == 7 && shieldDown)
            {
                ySourceHeight = (5 - (int)(NPC.frameCounter / 10)) * 442;
            }

            Color alphaColor;

            if (NPC.alpha != 0)
                alphaColor = new Color(drawColor.R, drawColor.G, drawColor.B, NPC.alpha);
            else
                alphaColor = drawColor;

            if (introTicker > introTickerMax - 90 || exitTicker > 90) //Cut early for now as there was wierdness with the alpha values of the intro
                return false;

            if (!left)
            {
                //Afterimage for dash
                if ((Action == 1 && actionTimer > 60 && actionTimer < 150) || (Action == 10 && actionTimer > 60 && (timeReachedPortal == 0 || actionTimer < timeReachedPortal + 120)) || (Action == 2 && actionTimer >= 90 && actionTimer <= 94))
                {
                    for (int k = NPC.oldPos.Length - 1; k >= 0; k--)
                    {
                        Vector2 pos = NPC.oldPos[k];

                        spriteBatch.Draw(tex,
                        new Rectangle((int)(pos.X - 221) - (int)(screenPos.X), (int)(pos.Y - 200) - (int)(screenPos.Y), 412, 442),
                        new Rectangle(xSourceHeight, ySourceHeight, 412, 442),
                        Color.Red * ((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length),
                        NPC.rotation,
                        Vector2.Zero,
                        SpriteEffects.None,
                        0);
                    }
                }

                spriteBatch.Draw(tex,
                    new Rectangle((int)(NPC.position.X - 221) - (int)(screenPos.X), (int)(NPC.position.Y - 200) - (int)(screenPos.Y), 412, 442),
                    new Rectangle(xSourceHeight, ySourceHeight, 412, 442),
                    alphaColor,
                    NPC.rotation,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0);
            }
            else
            {
                if ((Action == 1 && actionTimer > 60 && actionTimer < 150) || (Action == 10 && actionTimer > 60 && (timeReachedPortal == 0 || actionTimer < timeReachedPortal + 120)) || (Action == 2 && actionTimer >= 90 && actionTimer <= 100))
                {
                    for (int k = NPC.oldPos.Length - 1; k >= 0; k--)
                    {
                        Vector2 pos = NPC.oldPos[k];

                        spriteBatch.Draw(tex,
                            new Rectangle((int)(pos.X - 51) - (int)(screenPos.X), (int)(pos.Y - 205) - (int)(screenPos.Y), 412, 442),
                            new Rectangle(xSourceHeight, ySourceHeight, 412, 442),
                            Color.Red * ((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length),
                            NPC.rotation,
                            Vector2.Zero,
                            SpriteEffects.FlipHorizontally,
                            0);
                    }
                }

                spriteBatch.Draw(tex,
                    new Rectangle((int)(NPC.position.X - 51) - (int)(screenPos.X), (int)(NPC.position.Y - 205) - (int)(screenPos.Y), 412, 442),
                    new Rectangle(xSourceHeight, ySourceHeight, 412, 442),
                    alphaColor,
                    NPC.rotation,
                    Vector2.Zero,
                    SpriteEffects.FlipHorizontally,
                    0);
            }

            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (teleIndicator)
            {
                Texture2D texTele = Request<Texture2D>(AssetDirectory.CrimsonKnight + "TeleportIndicator").Value;

                spriteBatch.Draw(texTele, (Main.player[NPC.target].Bottom + new Vector2(!left ? texTele.Width : -texTele.Width, -texTele.Height / 2)) - screenPos, null, Color.Lerp(new Color(0, 0, 0, 0), new Color(255, 255, 255, 255), (float)(-1 * (loopCounter - 30)) / 30f), 0, new Vector2(texTele.Width, texTele.Height) / 2, 1 + (0.02f * loopCounter), !left ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            }

            if (dashIndicator)
            {
                Texture2D texDash = Request<Texture2D>(AssetDirectory.CrimsonKnight + "DashIndicator").Value;
                spriteBatch.Draw(texDash, NPC.Center + (new Vector2(left ? 30 : -30, 0) * actionTimer) - screenPos, null, new Color(255, 255, 255, 0), 0, new Vector2(texDash.Width, texDash.Height) / 2, 1, left ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            }
                
            if (frameX == 6 || (frameX == 7 && !shieldDown)) //if shield is up or going up
            {
                if (shieldScale < 1)
                    shieldScale += .04f;
            }
            else if (shieldScale > 0)
            {
                shieldScale -= .04f;
            }

            if (shieldScale > 0)
            {
                Texture2D texShield = Request<Texture2D>(AssetDirectory.CrimsonKnight + "ShieldIndicator").Value;
                spriteBatch.Draw(texShield, NPC.Center - screenPos, null, new Color(100, 0, 0, 0), 0, texShield.Size() / 2, shieldScale, SpriteEffects.None, 0);
            }

            if (showPortals && portalSize < 1)
                portalSize += .02f;
            else if (!showPortals && portalSize > 0)
                portalSize -= .02f;

            if (portalSize > 0)
            {
                var portal = Filters.Scene["ExoriumMod:VioletPortal"].GetShader().Shader;
                portal.Parameters["sampleTexture2"].SetValue(Request<Texture2D>(AssetDirectory.ShaderMap + "PortalMap").Value);
                portal.Parameters["uTime"].SetValue(Main.GameUpdateCount * 0.02f);
                portal.Parameters["uProgress"].SetValue(Main.GameUpdateCount * .003f);

                Texture2D texPortal = Request<Texture2D>(AssetDirectory.ShaderMap + "Portal").Value;
                foreach (Vector2 p in Current_Portals)
                {
                    spriteBatch.End();
                    spriteBatch.Begin(default, BlendState.NonPremultiplied, default, default, default, portal, Main.GameViewMatrix.ZoomMatrix);

                    spriteBatch.Draw(texPortal, p - screenPos, null, new Color(255, 255, 255, 0), Main.GameUpdateCount * .01f, texPortal.Size() / 2, 2.5f * portalSize, SpriteEffects.None, 0);

                    spriteBatch.End();
                    spriteBatch.Begin(default, BlendState.Additive, SamplerState.PointWrap, default, default, default, Main.GameViewMatrix.ZoomMatrix);
                }
            }

            //Drawing for the parry attack
            if (parry && parryFireballTimer < 1)
                parryFireballTimer += .02f;
            else if (!parry && parryFireballTimer > 0)
                parryFireballTimer -= .02f;

            if (parryFireballTimer > 0)
            {
                Texture2D texFireball = Request<Texture2D>(AssetDirectory.CrimsonKnight + "CaraveneFireball").Value;
                for (int i = 0; i < parryRetaliate; i++)
                {
                    Vector2 offset = new Vector2(0, 200);
                    offset = offset.RotatedBy(MathHelper.ToRadians((360 / parryRetaliate) * i));
                    offset = offset.RotatedBy(Main.GameUpdateCount * .02);
                    spriteBatch.Draw(texFireball, NPC.Center + offset - screenPos, null, Color.White, Main.GameUpdateCount * .1f, texFireball.Size() / 2, parryFireballTimer, SpriteEffects.None, 0);
                }
            }

            //Portal for the intro
            if (introTicker > introTickerMax - 180)
            {
                var portal = Filters.Scene["ExoriumMod:VioletPortal"].GetShader().Shader;
                portal.Parameters["sampleTexture2"].SetValue(Request<Texture2D>(AssetDirectory.ShaderMap + "PortalMap").Value);
                portal.Parameters["uTime"].SetValue(Main.GameUpdateCount * 0.02f);
                portal.Parameters["uProgress"].SetValue(Main.GameUpdateCount * .003f);

                Texture2D texPortal = Request<Texture2D>(AssetDirectory.ShaderMap + "Portal").Value;
                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.NonPremultiplied, default, default, default, portal, Main.GameViewMatrix.ZoomMatrix);

                if (introTicker > introTickerMax - 60 && introPortalSize < 1)
                    introPortalSize += .02f;
                else if (introTicker < introTickerMax - 120 && introPortalSize > 0)
                    introPortalSize -= .02f;

                if (introPortalSize > 0)
                    spriteBatch.Draw(texPortal, NPC.Center - screenPos, null, new Color(255, 255, 255, 0), Main.GameUpdateCount * .01f, texPortal.Size() / 2, 3f * introPortalSize, SpriteEffects.None, 0);

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.Additive, SamplerState.PointWrap, default, default, default, Main.GameViewMatrix.ZoomMatrix);
            }

            //Portal for despawn
            if (exitTicker > 60)
            {
                var portal = Filters.Scene["ExoriumMod:VioletPortal"].GetShader().Shader;
                portal.Parameters["sampleTexture2"].SetValue(Request<Texture2D>(AssetDirectory.ShaderMap + "PortalMap").Value);
                portal.Parameters["uTime"].SetValue(Main.GameUpdateCount * 0.02f);
                portal.Parameters["uProgress"].SetValue(Main.GameUpdateCount * .003f);

                Texture2D texPortal = Request<Texture2D>(AssetDirectory.ShaderMap + "Portal").Value;
                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.NonPremultiplied, default, default, default, portal, Main.GameViewMatrix.ZoomMatrix);

                if (exitTicker > 60 && exitTicker < 120 && exitPortalSize < 1)
                    exitPortalSize += .02f;
                else if (exitTicker > 130 && exitPortalSize > 0)
                    exitPortalSize -= .02f;

                if (exitPortalSize > 0)
                    spriteBatch.Draw(texPortal, NPC.Center - screenPos, null, new Color(255, 255, 255, 0), Main.GameUpdateCount * .01f, texPortal.Size() / 2, 3f * exitPortalSize, SpriteEffects.None, 0);

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.Additive, SamplerState.PointWrap, default, default, default, Main.GameViewMatrix.ZoomMatrix);
            }

            base.PostDraw(spriteBatch, screenPos, drawColor);
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (introAnimation || phaseTransition || noContactDamage)
                return false;
            return base.CanHitPlayer(target, ref cooldownSlot);
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (introAnimation || phaseTransition)
                return false;
            return base.CanBeHitByProjectile(projectile);
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            if (introAnimation || phaseTransition)
                return false;
            return base.CanBeHitByItem(player, item);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return !noContactDamage;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }

        public override bool? CanFallThroughPlatforms()
        {
            if (Main.tile[NPC.Bottom.ToTileCoordinates().X, NPC.Bottom.ToTileCoordinates().Y].HasTile ||
                Main.tile[NPC.Bottom.ToTileCoordinates().X, NPC.Bottom.ToTileCoordinates().Y - 1].HasTile ||
                Main.tile[NPC.Bottom.ToTileCoordinates().X, NPC.Bottom.ToTileCoordinates().Y - 2].HasTile ||
                Main.tile[NPC.Bottom.ToTileCoordinates().X, NPC.Bottom.ToTileCoordinates().Y + 1].HasTile)
                return false;
            return true;
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            if (parry)
            {
                parryDamaged += damage;
                if (parryDamaged > 20 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    parryDamaged = 0;
                    parryRetaliate++;
                }
                NPC.life += damage;
            }
            base.OnHitByItem(player, item, damage, knockback, crit);
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            if (parry)
            {
                parryDamaged += damage;
                if (parryDamaged > 20 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    parryDamaged = 0;
                    parryRetaliate++;
                }
                NPC.life += damage;
            }
            base.OnHitByProjectile(projectile, damage, knockback, crit);
        }
    }
}   
