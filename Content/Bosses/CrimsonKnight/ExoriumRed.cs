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
using Terraria.GameContent.UI.Elements;
using ExoriumMod.Core.Utilities;
using ExoriumMod.Content.Bosses.Shadowmancer;
using Terraria.GameContent.ItemDropRules;

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    [AutoloadBossHead]
    class ExoriumRed : ModNPC
    {
        public override string Texture => AssetDirectory.CrimsonKnight + "Caravene" + "_Hitbox";
        public override string BossHeadTexture => AssetDirectory.CrimsonKnight + "Caravene" + "_Head_Boss";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crimson Knight");
            Main.npcFrameCount[NPC.type] = 6;

            //Always draw so visuals don't fail while offscreen
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;

            NPCID.Sets.TrailCacheLength[Type] = 7;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.TeleportationImmune[Type] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 6666;
            NPC.damage = 61;
            NPC.defense = 36;
            NPC.knockBackResist = 0f;
            NPC.width = 140;
            NPC.height = 240;
            NPC.value = Item.buyPrice(0, 25, 0, 0);
            NPC.npcSlots = 30f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.NPCHit4;
            //NPC.DeathSound = SoundID.NPCDeath52;
            NPC.timeLeft = NPC.activeTime * 30;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffType<Inferno>()] = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.alpha = 0;
            if (!Main.dedServ)
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Sounds/Music/ExoriumRed");
            //bossBag = ItemType<ShadowmancerBag>();
        }

        //May want to make teleport next to player not damage when teleporting

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.75 * balance);
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
        private Vector2 bladeSpawnOriginQ1 = Vector2.Zero;
        private Vector2 bladeSpawnOriginQ2 = Vector2.Zero;
        private Vector2 bladeSpawnOriginQ3 = Vector2.Zero;
        private Vector2 bladeSpawnOriginQ4 = Vector2.Zero;
        private int bladeSpawnSide = 0;
        private int bladeSpawnCount = 0;
        private float auraAlpha = 0;
        private bool[] teleportLocations = new bool[6]{ false, false, false, false, false, false };
        Vector2 trueTeleportLocation = Vector2.Zero;


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

        private bool deathAnimation = false;
        private float deathTimer = 0;

        private float[] deathLightLengths = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private float[] deathLightAngles = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        //Portal/Arena Locations
        private static Vector2 topL = Core.Systems.WorldDataSystem.FallenTowerRect.TopLeft();
        private static Vector2 topR = Core.Systems.WorldDataSystem.FallenTowerRect.TopRight();
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
        private static float maxTeleportHeight = Core.Systems.WorldDataSystem.FallenTowerRect.Top + 160 + 240;
        private static float minTeleportX = Core.Systems.WorldDataSystem.FallenTowerRect.Left + 80;
        private static float maxTeleportX = Core.Systems.WorldDataSystem.FallenTowerRect.Right - 80;

        //Actions
        //0 - jump                              -Unchanged?
        //1 - dash                              -Done
        //2 - Teleport next to player           -Done
        //3 - Rift                              -Done
        //4 - parry                             -Done
        //5 - Galacta knight lol                -Done
        //6 - swords shower                     -Done
        //7 - sword beams                       -Done
        //8 - hop down                          -Unchanged?
        //9 - Laser Pinwheel                    -Unfinished - TODO: Make unable to be used too close to walls or ceiling
        //10 - portal dash                      -Done
        //11 - Burning Sphere                   -Done
        //12 - enrage                           -Unchanged?
        public float Action
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        private float wait = 60;

        private float actionTimer;

        public float DeathTimer
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }

        public override void AI()
        {
            if (DeathTimer > 0) //override AI with death animation in this case
            {
                DeathAnimation();
                return;
            }

            //Damage calculations
            int damage = NPC.damage / (Main.expertMode == true ? 2 : 1);

            //Reset vars
            parry = false;
            Vector2 swordTip = new Vector2(NPC.Center.X + (left ? 65 : -65), NPC.Center.Y - NPC.height - 75);

            #region Targeting  
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.TargetClosest(true);
            }

            Player player = Main.player[NPC.target];
            if (!player.active || player.dead && Main.netMode != NetmodeID.MultiplayerClient || !player.getRect().Intersects(Core.Systems.WorldDataSystem.FallenTowerRect)) //Also stop targeting if outside of arena
            {
                NPC.TargetClosest(true);
                NPC.netUpdate = true;
                player = Main.player[NPC.target];
                if (!player.active || player.dead || (NPC.position - player.position).Length() > 6000 || !player.getRect().Intersects(Core.Systems.WorldDataSystem.FallenTowerRect))
                {
                    if (actionTimer == 0) //Only leave if action done
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
                        if (!endFlameSpawn)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), swordPoint, Vector2.Zero, ProjectileType<FlameTrail>(), damage, 0);

                                if (actionTimer % 20 == 0)
                                {
                                    SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, swordPoint);
                                    if (phase == 2 || Main.masterMode)
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), swordPoint, Vector2.Zero, ProjectileType<LargeFlamePillar>(), damage, 0);
                                    else
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), swordPoint, Vector2.Zero, ProjectileType<FlamePillar>(), damage, 0);
                                }
                            }
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
                    {
                        teleIndicator = true;
                        int spots = 2;
                        if (phase == 2 || Main.expertMode)
                            spots++;
                        if (phase == 2 && Main.expertMode)
                            spots++;
                        if (phase == 2 && Main.masterMode)
                            spots++;

                        for (int i = 0; i < 6; i++)
                            teleportLocations[i] = false;

                        for (int i = 0; i < spots; i++) //Choose sports for teleports
                        {
                            int choose = 0;
                            do
                            {
                                choose = Main.rand.Next(6);
                            }
                            while (teleportLocations[choose] != false); //repeat if that spot was already chosen
                            teleportLocations[choose] = true;
                        }

                        NPC.noGravity = true;
                        NPC.noTileCollide = true;
                        noContactDamage = true;
                    }
                    else if (actionTimer == 150)
                    {
                        //Choose the teleport spot
                        int teleportLocal = 0;
                        do
                        {
                            teleportLocal = Main.rand.Next(6);
                        }
                        while (teleportLocations[teleportLocal] == false); //repeat if that spot was not chosen

                        //Choose offest based on teleport location
                        Texture2D texTele = Request<Texture2D>(AssetDirectory.CrimsonKnight + "TeleportIndicator", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                        Vector2 location;
                        for (int i = 0; i < 6; i++)//loop through locations and spawn clone or move based on index
                        {
                            if (teleportLocations[i])
                            {
                                location = new Vector2(player.Center.X + (i<3? -NPC.width : NPC.width), Arena_Top_Left.Y + (i%3 == 0? 20 : i%3 == 1? 344:652));//really wish I didn't have to use static numbers for this but can't think of a way to avoid it
                                if (location.X < minTeleportX + NPC.width / 2)
                                    location.X = minTeleportX + NPC.width / 2;
                                if (location.X > maxTeleportX - NPC.width / 2)
                                    location.X = maxTeleportX - NPC.width / 2;
                                if (teleportLocal == i)
                                    trueTeleportLocation = location;
                                else
                                    CreateCloneProjectile(location, i, damage);
                            }
                        }

                        NPC.velocity = Vector2.Zero;
                        noContactDamage = false;

                        NPC.Center = trueTeleportLocation;
                        NPC.alpha = 0;
                        if ((NPC.Center - player.Center).X > 0)
                            left = false;
                        else
                            left = true;
                        teleIndicator = false;
                    }
                    else if (actionTimer == 180)
                    {
                        NPC.noGravity = false;
                        NPC.noTileCollide = false;
                        frameX = 1;
                        NPC.frameCounter = 0;
                    }
                    else if (actionTimer >= 210)
                    {
                        ChooseMovement();
                    }

                    //control alpha
                    if (actionTimer < 80 && NPC.alpha < 255)
                    {
                        NPC.alpha += 5;
                        if (NPC.alpha > 255) NPC.alpha = 255;//attempt to stop red flash
                    }
                    break;
                case 3:
                    if (actionTimer == 5)
                    {
                        //Swingup Effect
                        frameX = 3;
                    }
                    else if (actionTimer == 30)
                    {
                        frameX = 1;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(206 * (left ? 1 : -1), 0), Vector2.Zero, ProjectileType<InfernalRift>(), damage, 1, Main.myPlayer, topL.Y, Core.Systems.WorldDataSystem.FallenTowerRect.BottomLeft().Y);
                    }
                    else if (actionTimer > 240)
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
                    else if (Main.netMode != NetmodeID.MultiplayerClient && actionTimer == 90)
                    {
                        for (int i = 0; i < 15; i++)
                        {
                            Vector2 trajectory = new Vector2(0, 1);
                            trajectory = trajectory.RotatedBy((MathHelper.TwoPi / 15) * i);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + trajectory * 200, trajectory * .01f, ProjectileType<FlametoungeBeam>(), damage, 1, Main.myPlayer, 90);
                        }
                    }
                    else if (Main.netMode != NetmodeID.MultiplayerClient && actionTimer == 150 && phase == 2)
                    {
                        for (int i = 0; i < 15; i++)
                        {
                            Vector2 trajectory = new Vector2(0, 1);
                            trajectory = trajectory.RotatedBy((MathHelper.TwoPi / 15) * i + (MathHelper.TwoPi / 30));
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + trajectory * 200, trajectory * .01f, ProjectileType<FlametoungeBeam>(), damage, 1, Main.myPlayer, 90);
                        }
                    }
                    if (actionTimer > 60 && actionTimer < Parry_Durration)
                    {
                        NPC.HitSound = SoundID.Item150;
                        parry = true;
                    }
                    else if (actionTimer == Parry_Durration)
                    {
                        parry = false;
                        NPC.HitSound = SoundID.NPCHit4;
                        shieldDown = true;
                        NPC.frameCounter = 0;
                        bool sound = false;

                        //Use same formula as draw to create projectiles

                        for (int i = 0; i < parryRetaliate; i++)
                        {
                            Vector2 offset = new Vector2(0, 200);
                            offset = offset.RotatedBy(MathHelper.ToRadians((360 / parryRetaliate) * i));
                            offset = offset.RotatedBy(Main.GameUpdateCount * .02);
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
                        //Choose a side, 1 is top 2 is bottom
                        bladeSpawnSide = Main.rand.Next(1, 3);

                        //Grab spawn origins for each quadrant (same as how original boss does it)
                        bladeSpawnOriginQ1 = new Vector2(Core.Systems.WorldDataSystem.FallenTowerRect.Center.X, Core.Systems.WorldDataSystem.FallenTowerRect.Bottom) + (new Vector2(1, -1) * 1800);
                        bladeSpawnOriginQ2 = new Vector2(Core.Systems.WorldDataSystem.FallenTowerRect.Center.X, Core.Systems.WorldDataSystem.FallenTowerRect.Top) + (new Vector2(1, 1) * 1800);
                        bladeSpawnOriginQ3 = new Vector2(Core.Systems.WorldDataSystem.FallenTowerRect.Center.X, Core.Systems.WorldDataSystem.FallenTowerRect.Top) + (new Vector2(-1, 1) * 1800);
                        bladeSpawnOriginQ4 = new Vector2(Core.Systems.WorldDataSystem.FallenTowerRect.Center.X, Core.Systems.WorldDataSystem.FallenTowerRect.Bottom) + (new Vector2(-1, -1) * 1800);

                        bladeSpawnCount = Main.expertMode ? 20 : 25;
                        if (Main.masterMode)
                            bladeSpawnCount -= 5;
                        if (phase == 2)
                            bladeSpawnCount += 10; //reduce since double are being created
                    }
                    else if (actionTimer > 30 && actionTimer % bladeSpawnCount == 0 && actionTimer < 360)
                    {
                        if (bladeSpawnSide == 1 || phase == 2) //Spawn blades in quadrants 1 & 4
                        {
                            Vector2 spawnPointQ1 = bladeSpawnOriginQ1;
                            spawnPointQ1.X += Main.rand.NextFloat(-(Core.Systems.WorldDataSystem.FallenTowerRect.Width / 2) + 80, Core.Systems.WorldDataSystem.FallenTowerRect.Width / 2 - 80);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPointQ1, new Vector2(-10, 10)/*This is what is would calculate as for this quadrant for the first boss*/, ProjectileType<ReboundingSword>(), damage, 1, Main.myPlayer, 60, 1f);

                            Vector2 spawnPointQ4 = bladeSpawnOriginQ4;
                            spawnPointQ4.X += Main.rand.NextFloat(-(Core.Systems.WorldDataSystem.FallenTowerRect.Width / 2) + 80, Core.Systems.WorldDataSystem.FallenTowerRect.Width / 2 - 80);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPointQ4, new Vector2(10, 10)/*This is what is would calculate as for this quadrant for the first boss*/, ProjectileType<ReboundingSword>(), damage, 1, Main.myPlayer, 60, 1f);
                        }

                        if (bladeSpawnSide == 2 || phase == 2)
                        {
                            Vector2 spawnPointQ2 = bladeSpawnOriginQ2;
                            spawnPointQ2.X += Main.rand.NextFloat(-(Core.Systems.WorldDataSystem.FallenTowerRect.Width / 2) + 80, Core.Systems.WorldDataSystem.FallenTowerRect.Width / 2 - 80);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPointQ2, new Vector2(-10, -10)/*This is what is would calculate as for this quadrant for the first boss*/, ProjectileType<ReboundingSword>(), damage, 1, Main.myPlayer, 60, 0f);

                            Vector2 spawnPointQ3 = bladeSpawnOriginQ3;
                            spawnPointQ3.X += Main.rand.NextFloat(-(Core.Systems.WorldDataSystem.FallenTowerRect.Width / 2) + 80, Core.Systems.WorldDataSystem.FallenTowerRect.Width / 2 - 80);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPointQ3, new Vector2(10, -10)/*This is what is would calculate as for this quadrant for the first boss*/, ProjectileType<ReboundingSword>(), damage, 1, Main.myPlayer, 60, 0f);
                        }
                    }
                    if (actionTimer >= 361)
                    {
                        ChooseFollowup();
                    }

                    if (actionTimer < 60 && actionTimer > 12) //Indicators
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient && actionTimer % 6 == 0)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), swordTip, new Vector2(0, -30).RotatedByRandom(MathHelper.Pi/16), ProjectileType<indicatorRainSword>(), damage, 1, Main.myPlayer, 60/*, (bladeSpawnQuadrant == 1 || bladeSpawnQuadrant == 4) ? 1f : 0f*/);
                            SoundEngine.PlaySound(SoundID.Item100, swordTip);
                        }
                    }
                    break;
                case 6:
                    if (actionTimer == 0)
                    {
                        altBeamType = false;
                        if (Main.rand.NextBool())
                            altBeamType = true;
                        playerPlaceholder = player.Center;
                    }
                    if ((actionTimer % 5 == 0) && Main.netMode != NetmodeID.MultiplayerClient )
                    {
                        Vector2 dummy = new Vector2(0, 1);
                        if (phase == 2 && Main.expertMode && !Main.masterMode) //Makes projectiles start at the sides which is harder to read
                            dummy = new Vector2(1, 0);
                        if (altBeamType)
                        {
                            if (actionTimer <= 140)
                            {
                                if (actionTimer % 10 == 0)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPlaceholder + dummy.RotatedBy((MathHelper.PiOver2 / 14f) * (actionTimer / 10f)) * 700, dummy.RotatedBy((MathHelper.PiOver2 / 14) * (actionTimer / 10f)) * -.01f, ProjectileType<FlametoungeBeam>(), (int)(damage * 1.5f), 1, Main.myPlayer, 60, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPlaceholder - dummy.RotatedBy((MathHelper.PiOver2 / 14f) * (actionTimer / 10f)) * 700, dummy.RotatedBy((MathHelper.PiOver2 / 14) * (actionTimer / 10f)) * .01f, ProjectileType<FlametoungeBeam>(), (int)(damage * 1.5f), 1, Main.myPlayer, 60, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                                }

                                if (Main.masterMode || phase == 2 && actionTimer % 10 == 0)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPlaceholder + dummy.RotatedBy(((MathHelper.PiOver2 / 14) * (actionTimer / 10f)) + MathHelper.PiOver2) * 700, dummy.RotatedBy(((MathHelper.PiOver2 / 14) * (actionTimer / 10f)) + MathHelper.PiOver2) * -.01f, ProjectileType<FlametoungeBeam>(), (int)(damage * 1.5f), 1, Main.myPlayer, 60, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPlaceholder - dummy.RotatedBy(((MathHelper.PiOver2 / 14) * (actionTimer / 10f)) + MathHelper.PiOver2) * 700, dummy.RotatedBy(((MathHelper.PiOver2 / 14) * (actionTimer / 10f)) + MathHelper.PiOver2) * .01f, ProjectileType<FlametoungeBeam>(), (int)(damage * 1.5f), 1, Main.myPlayer, 60, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                                }
                                else if (phase == 1 && Main.expertMode && actionTimer % 35 == 0)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPlaceholder + dummy.RotatedBy(((MathHelper.PiOver2 / 14) * (actionTimer / 10f)) + MathHelper.PiOver2) * 700, dummy.RotatedBy(((MathHelper.PiOver2 / 14) * (actionTimer / 10f)) + MathHelper.PiOver2) * -.01f, ProjectileType<FlametoungeBeam>(), (int)(damage * 1.5f), 1, Main.myPlayer, 60, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPlaceholder - dummy.RotatedBy(((MathHelper.PiOver2 / 14) * (actionTimer / 10f)) + MathHelper.PiOver2) * 700, dummy.RotatedBy(((MathHelper.PiOver2 / 14) * (actionTimer / 10f)) + MathHelper.PiOver2) * .01f, ProjectileType<FlametoungeBeam>(), (int)(damage * 1.5f), 1, Main.myPlayer, 60, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                                }
                            }
                        }
                        else
                        {
                            if (actionTimer <= 140) //Rotations are negative to reverse sides
                            {
                                if (actionTimer % 10 == 0)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPlaceholder + dummy.RotatedBy(-(MathHelper.PiOver2 / 14f) * (actionTimer / 10f)) * 700, dummy.RotatedBy(-(MathHelper.PiOver2 / 14) * (actionTimer / 10f)) * -.01f, ProjectileType<FlametoungeBeam>(), (int)(damage * 1.5f), 1, Main.myPlayer, 60, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPlaceholder - dummy.RotatedBy(-(MathHelper.PiOver2 / 14f) * (actionTimer / 10f)) * 700, dummy.RotatedBy(-(MathHelper.PiOver2 / 14) * (actionTimer / 10f)) * .01f, ProjectileType<FlametoungeBeam>(), (int)(damage * 1.5f), 1, Main.myPlayer, 60, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                                }

                                if (Main.masterMode || phase == 2 && actionTimer % 10 == 0)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPlaceholder + dummy.RotatedBy(-((MathHelper.PiOver2 / 14) * (actionTimer / 10f)) + MathHelper.PiOver2) * 700, dummy.RotatedBy((-(MathHelper.PiOver2 / 14) * (actionTimer / 10f)) + MathHelper.PiOver2) * -.01f, ProjectileType<FlametoungeBeam>(), (int)(damage * 1.5f), 1, Main.myPlayer, 60, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPlaceholder - dummy.RotatedBy(-((MathHelper.PiOver2 / 14) * (actionTimer / 10f)) + MathHelper.PiOver2) * 700, dummy.RotatedBy((-(MathHelper.PiOver2 / 14) * (actionTimer / 10f)) + MathHelper.PiOver2) * .01f, ProjectileType<FlametoungeBeam>(), (int)(damage * 1.5f), 1, Main.myPlayer, 60, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                                }
                                else if (phase == 1 && Main.expertMode && actionTimer % 35 == 0)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPlaceholder + dummy.RotatedBy(-((MathHelper.PiOver2 / 14) * (actionTimer / 10f)) + MathHelper.PiOver2) * 700, dummy.RotatedBy((-(MathHelper.PiOver2 / 14) * (actionTimer / 10f)) + MathHelper.PiOver2) * -.01f, ProjectileType<FlametoungeBeam>(), (int)(damage * 1.5f), 1, Main.myPlayer, 60, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPlaceholder - dummy.RotatedBy(-((MathHelper.PiOver2 / 14) * (actionTimer / 10f)) + MathHelper.PiOver2) * 700, dummy.RotatedBy((-(MathHelper.PiOver2 / 14) * (actionTimer / 10f)) + MathHelper.PiOver2) * .01f, ProjectileType<FlametoungeBeam>(), (int)(damage * 1.5f), 1, Main.myPlayer, 60, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                                }
                            }
                        }
                    }
                    if (actionTimer >= 170)
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
                    if (actionTimer == 20 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2((NPC.width * (left? 1:-1)), 0), Vector2.Zero, ProjectileType<CrimsonSlash>(), damage, 4, Main.myPlayer, player.whoAmI, Main.rand.NextFloat(MathHelper.Pi * 2));
                    }
                    if (actionTimer >= 40)
                    {
                        ChooseFollowup();
                    }
                    break;
                case 8:
                    if (NPC.Bottom.Y > Main.player[NPC.target].Bottom.Y + 32 && actionTimer > 50 && actionTimer < 110)
                    {
                        actionTimer = 110;
                    }
                    if (NPC.Bottom.Y > Arena_Bottom_Left.Y - 32)
                    {
                        actionTimer = 121;
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
                        Vector2 unit = Vector2.UnitX;
                        bool turnLeft = false;
                        if (Main.expertMode && Main.rand.NextBool())//Chance for diagonal beams in expert
                            unit = unit.RotatedBy(MathHelper.PiOver4);
                        if (Main.rand.NextBool())
                            turnLeft = true;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), swordTip, unit.RotatedBy(MathHelper.PiOver2 * i), ProjectileType<InfernoBeam>(), damage * 2, 5, -1, 0, turnLeft ? 1 : 0);
                                if ((phase == 2 && Main.expertMode) || Main.masterMode)//Double beams
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), swordTip, unit.RotatedBy(MathHelper.PiOver2 * i + MathHelper.PiOver4), ProjectileType<InfernoBeam>(), damage * 2, 5, -1, 0, turnLeft ? 1 : 0);
                                }
                            }
                        }
                    }
                    else if (actionTimer >= 150 && actionTimer % 50 == 0 && Main.netMode != NetmodeID.MultiplayerClient && Main.expertMode)
                    {
                        Vector2 trajectory = player.Center - swordTip;
                        trajectory.Normalize();
                        if (Main.expertMode && (actionTimer%100 == 0 || Main.masterMode))
                        {
                            for (int i = 0; i < 8; i++)
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), swordTip, trajectory.RotatedBy((MathHelper.TwoPi / 8) * i) * .01f, ProjectileType<FlametoungeBeam>(), damage, 1, Main.myPlayer, 90);
                        }

                        bool counterClockwise = actionTimer % 100 == 0;
                        for (int i = 0; i < 4; i++)
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), swordTip, Vector2.UnitX.RotatedBy((MathHelper.TwoPi / 4) * i), ProjectileType<RotatingFireball>(), damage, 2, Main.myPlayer, counterClockwise? 1 : 0, phase == 2 ? 1 : 0);
                    }
                    else if (actionTimer > 510)
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
                        if (!endFlameSpawn)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), swordPoint, Vector2.Zero, ProjectileType<FlameTrail>(), damage, 0);

                                if (actionTimer % 20 == 0)
                                {
                                    SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, swordPoint);
                                    if (phase == 2 || Main.masterMode)
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), swordPoint, Vector2.Zero, ProjectileType<LargeFlamePillar>(), damage, 0);
                                    else
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), swordPoint, Vector2.Zero, ProjectileType<FlamePillar>(), damage, 0);
                                }
                            }
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
                    else if (actionTimer < timeReachedPortal + 120) //Move out of next portal after 2 seconds
                    {
                        if (NPC.alpha > 0)
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
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), swordPoint, Vector2.Zero, ProjectileType<FlameTrail>(), damage, 0);

                                if (actionTimer % 20 == 0)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), swordPoint, Vector2.Zero, ProjectileType<FlamePillar>(), damage, 0);
                                }
                            }
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
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + (left ? 65 : -65), NPC.Center.Y - NPC.height - 75), Vector2.Zero, ProjectileType<LargeFlamingSphere>(), damage * 2, 1, Main.myPlayer, NPC.target);
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
                    if (actionTimer == 5)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (int i = 0; i < 28; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center + new Vector2(0, -900), Vector2.Zero, ProjectileType<FireballRing>(), damage, 1, Main.myPlayer, (MathHelper.Pi / 14) * i, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0, 2000);
                            }
                        }

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
                else if (NPC.Center.Y < topL.Y + 865)
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
                if (phase == 2 && Main.rand.NextBool())
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
            if (phase == 2 && Main.rand.NextBool(4))
            {
                Action = 9;
                wait = 30;
            }
            else if (Main.rand.NextBool(3))
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
                wait = 90;
            }
            else if (Main.rand.NextBool(3)) //Less common in phase 1
            {
                if (phase == 1)
                {
                    Action = 12;
                    wait = 10;
                }
                else
                {
                    Action = 4;
                    wait = 5;
                }
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

        private void CreateCloneProjectile(Vector2 location, int index, int damage)
        {
            //Projectile
            if (Main.netMode != NetmodeID.MultiplayerClient)
                Projectile.NewProjectile(NPC.GetSource_FromAI(), location, Vector2.Zero, ProjectileType<CaraveneClone>(), damage, 2, Main.myPlayer, index < 3 ? 0 : 1);
            /*
            for (int i = 0; i < 50; i++)
            {
                Vector2 rad = new Vector2(0, Main.rand.NextFloat(30));
                Vector2 shootPoint = rad.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi));
                Dust dust = Dust.NewDustPerfect(location, DustID.SolarFlare, shootPoint, 1, default, 1 + Main.rand.NextFloat(-.5f, .5f));
                dust.noGravity = true;
                dust.color = new Color(184, 58, 24);
            }*/
        }
        #endregion

        public override void FindFrame(int frameHeight)
        {
            if (deathAnimation) return;
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
                    else if (NPC.frameCounter == 20 && Action == 2 && actionTimer > 150)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2((left ? NPC.width : -NPC.width), 0), Vector2.Zero, ProjectileType<SwordHitbox>(), (NPC.damage / (Main.expertMode == true ? 4 : 2)) * 2, 7, Main.myPlayer);
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
            if (introTicker == 9999) //Boss Card
            {
                introTicker = 250;
                introTickerMax = introTicker;

                if (Main.netMode != NetmodeID.Server && !Filters.Scene["ExoriumMod:CaraveneTitle"].IsActive())
                {
                    Texture2D heatMap = Request<Texture2D>(AssetDirectory.ShaderMap + "HeatDistortMap", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                    Texture2D text = Request<Texture2D>(AssetDirectory.Effect + "CaraveneIntroCard", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

                    Filters.Scene.Activate("ExoriumMod:CaraveneTitle", NPC.Center).GetShader().UseImage(text).UseImage(heatMap, 1).UseTargetPosition(NPC.Center).UseIntensity(introTicker).UseProgress(Main.GameUpdateCount * 0.0015f);
                }
            }
            Main.musicFade[Main.curMusic] = 1f; //Want volume to be at max for a bit..
            //TODO: This is a TEST line
            //Action = 0;
            introTicker--;
            if (introTicker <= 0)
            {
                introAnimation = false;
                ChooseMovement();
            }

            if (Main.netMode != NetmodeID.Server && Filters.Scene["ExoriumMod:CaraveneTitle"].IsActive())
            {
                Filters.Scene["ExoriumMod:CaraveneTitle"].GetShader().UseTargetPosition(NPC.Center).UseIntensity(introTicker - (introTickerMax - 150)/*Time until shader end*/).UseProgress(Main.GameUpdateCount * 0.01f); //Make use game time for stoppin while paused
            }
            if (introTicker == introTickerMax - 150)
            {
                if (Main.netMode != NetmodeID.Server && Filters.Scene["ExoriumMod:CaraveneTitle"].IsActive())
                {
                    Filters.Scene["ExoriumMod:CaraveneTitle"].Deactivate();
                }
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
                NPC.alpha = 0;

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
            else if (transitionCounter == 120)
            {
                SoundEngine.PlaySound(SoundID.Roar, NPC.position);
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

        private void DeathAnimation()
        {
            if (DeathTimer == 1) //First frame sets
            {
                NPC.boss = false; //This might be bad??? Using it to stop music
                Music = -1;
                for (int i = 0; i < deathLightAngles.Length; i++) //Set light angles randomly
                {
                    bool done = false;
                    float angle = 0;

                    while (!done) //loop unil an angle that is different enough from the others is chosen
                    {
                        done = true;
                        angle = Main.rand.NextFloat(MathHelper.TwoPi);

                        foreach(float rad in deathLightAngles)
                        {
                            if (rad != 0 && Math.Abs(rad - angle) < .15f) { done = false; }
                        }
                    }
                    deathLightAngles[i] = angle;
                }

                foreach (Player player in Main.player)
                {
                    //Set each player's screen target if not set
                    if ((player.Center - NPC.Center).Length() < 3000 && player.GetModPlayer<ExoriumPlayer>().ScreenMoveTarget == Vector2.Zero)
                    {
                        player.GetModPlayer<ExoriumPlayer>().ScreenMoveTarget = NPC.Center;
                        player.GetModPlayer<ExoriumPlayer>().ScreenMoveTime = 500; //Extra second over death animation time
                    }
                }
            }
            if (DeathTimer % 8 == 0) //Set ticker based on past fights
            {
                Vector2 blastPos = new Vector2(NPC.position.X + Main.rand.NextFloat(NPC.width), NPC.position.Y + Main.rand.NextFloat(NPC.height));
                int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), blastPos.X, blastPos.Y, 0, 0, 612, 0, 0, Main.myPlayer, 1, 1);
                SoundEngine.PlaySound(SoundID.Item14, Main.projectile[proj].position);
                Main.projectile[proj].hostile = false;
            }
            if (DeathTimer >= 60)
            {
                deathLightLengths[0] += 5;
                deathLightLengths[1] += 5;
                deathLightLengths[2] += 5;
                deathLightAngles[0] += .003f;
                deathLightAngles[1] += .003f;
                deathLightAngles[2] += .003f;
                if (DeathTimer >= 150)
                {
                    deathLightLengths[3] += 5;
                    deathLightLengths[4] += 5;
                    deathLightLengths[5] += 5;
                    deathLightAngles[3] += .003f;
                    deathLightAngles[4] += .003f;
                    deathLightAngles[5] += .003f;
                    if (DeathTimer >= 240) 
                    {
                        deathLightLengths[6] += 5;
                        deathLightLengths[7] += 5;
                        deathLightLengths[8] += 5;
                        deathLightAngles[6] += .003f;
                        deathLightAngles[7] += .003f;
                        deathLightAngles[8] += .003f;
                    }
                }
            }

            DeathTimer++;
            if (DeathTimer == 380)
            {
                NPC.life = 0;
                NPC.HitEffect(0, 0);
                NPC.checkDead(); // This will trigger ModNPC.CheckDead the second time, causing the real death.
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
                    p.type == ProjectileType<CrimsonSlash>() ||
                    p.type == ProjectileType<CrimsonSlashProjectile>() ||
                    p.type == ProjectileType<LargeFlamingSphere>() ||
                    p.type == ProjectileType<FlamePillar>() ||
                    p.type == ProjectileType<LargeFlamePillar>() ||
                    p.type == ProjectileType<InfernoBeam>() ||
                    p.type == ProjectileType<InfernalRift>() ||
                    p.type == ProjectileType<RiftSpirit>() ||
                    p.type == ProjectileType<RotatingFireball>())
                {
                    p.timeLeft = 1;
                    p.Kill();
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.CrimsonKnight + "Caravene").Value;

            int ySourceHeight = (int)(NPC.frameCounter / 10) * 442;
            int xSourceHeight = (int)(frameX * 412);

            //Fire Aura
            var fire = Filters.Scene["ExoriumMod:FireAura"].GetShader().Shader;
            fire.Parameters["noiseTexture"].SetValue(Request<Texture2D>(AssetDirectory.ShaderMap + "FlamingSphere").Value);
            fire.Parameters["gradientTexture"].SetValue(Request<Texture2D>(AssetDirectory.ShaderMap + "basicGradient").Value);
            fire.Parameters["uTime"].SetValue(Main.GameUpdateCount * 0.02f);
            fire.Parameters["uOpacity"].SetValue(NPC.alpha);

            if (true)
            {
                Texture2D auraTex = Request<Texture2D>(AssetDirectory.CrimsonKnight + "Aura").Value;
                Color alpha = new Color(255, 255, 255, NPC.alpha);

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.NonPremultiplied, default, default, default, fire, Main.GameViewMatrix.ZoomMatrix);

                spriteBatch.Draw(auraTex, NPC.Center - screenPos + new Vector2(0, -150), null, alpha, 0, auraTex.Size() / 2, 1.8f, 0, 0);

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
                alphaColor = Color.Lerp(new Color(0, 0, 0, 0), drawColor, (float)(-1 * (NPC.alpha - 255)) / 255f);
            else
                alphaColor = drawColor;

            if (introTicker > introTickerMax - 90 || exitTicker > 90) //Cut early for now as there was wierdness with the alpha values of the intro
                return false;

            if (!left)
            {
                //Afterimage for dash
                if ((Action == 1 && actionTimer > 60 && actionTimer < 150) || (Action == 10 && actionTimer > 60 && (timeReachedPortal == 0 || actionTimer < timeReachedPortal + 120)))
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
                if ((Action == 1 && actionTimer > 60 && actionTimer < 150) || (Action == 10 && actionTimer > 60 && (timeReachedPortal == 0 || actionTimer < timeReachedPortal + 120)))
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
            if (DeathTimer > 0) //Catch into this block so that other draws stop for death animation
            {
                Color lightWhite = new Color(120, 0, 0, 200);
                spriteBatch.End();
                ShapeBatch.Begin(spriteBatch.GraphicsDevice);
                ShapeBatch.Triangle(NPC.Center - screenPos, NPC.Center + (Vector2.UnitY * deathLightLengths[0]).RotatedBy(deathLightAngles[0] + MathHelper.PiOver4 / 5) - screenPos, NPC.Center + (Vector2.UnitY * deathLightLengths[0]).RotatedBy(deathLightAngles[0] - MathHelper.PiOver4 / 5) - screenPos, lightWhite, Color.Transparent, Color.Transparent);
                ShapeBatch.Triangle(NPC.Center - screenPos, NPC.Center + (Vector2.UnitY * deathLightLengths[1]).RotatedBy(deathLightAngles[1] + MathHelper.PiOver4 / 5) - screenPos, NPC.Center + (Vector2.UnitY * deathLightLengths[1]).RotatedBy(deathLightAngles[1] - MathHelper.PiOver4 / 5) - screenPos, lightWhite, Color.Transparent, Color.Transparent);
                ShapeBatch.Triangle(NPC.Center - screenPos, NPC.Center + (Vector2.UnitY * deathLightLengths[2]).RotatedBy(deathLightAngles[2] + MathHelper.PiOver4 / 5) - screenPos, NPC.Center + (Vector2.UnitY * deathLightLengths[2]).RotatedBy(deathLightAngles[2] - MathHelper.PiOver4 / 5) - screenPos, lightWhite, Color.Transparent, Color.Transparent);
                ShapeBatch.Triangle(NPC.Center - screenPos, NPC.Center + (Vector2.UnitY * deathLightLengths[3]).RotatedBy(deathLightAngles[3] + MathHelper.PiOver4 / 5) - screenPos, NPC.Center + (Vector2.UnitY * deathLightLengths[3]).RotatedBy(deathLightAngles[3] - MathHelper.PiOver4 / 5) - screenPos, lightWhite, Color.Transparent, Color.Transparent);
                ShapeBatch.Triangle(NPC.Center - screenPos, NPC.Center + (Vector2.UnitY * deathLightLengths[4]).RotatedBy(deathLightAngles[4] + MathHelper.PiOver4 / 5) - screenPos, NPC.Center + (Vector2.UnitY * deathLightLengths[4]).RotatedBy(deathLightAngles[4] - MathHelper.PiOver4 / 5) - screenPos, lightWhite, Color.Transparent, Color.Transparent);
                ShapeBatch.Triangle(NPC.Center - screenPos, NPC.Center + (Vector2.UnitY * deathLightLengths[5]).RotatedBy(deathLightAngles[5] + MathHelper.PiOver4 / 5) - screenPos, NPC.Center + (Vector2.UnitY * deathLightLengths[5]).RotatedBy(deathLightAngles[5] - MathHelper.PiOver4 / 5) - screenPos, lightWhite, Color.Transparent, Color.Transparent);
                ShapeBatch.Triangle(NPC.Center - screenPos, NPC.Center + (Vector2.UnitY * deathLightLengths[6]).RotatedBy(deathLightAngles[6] + MathHelper.PiOver4 / 5) - screenPos, NPC.Center + (Vector2.UnitY * deathLightLengths[6]).RotatedBy(deathLightAngles[6] - MathHelper.PiOver4 / 5) - screenPos, lightWhite, Color.Transparent, Color.Transparent);
                ShapeBatch.Triangle(NPC.Center - screenPos, NPC.Center + (Vector2.UnitY * deathLightLengths[7]).RotatedBy(deathLightAngles[7] + MathHelper.PiOver4 / 5) - screenPos, NPC.Center + (Vector2.UnitY * deathLightLengths[7]).RotatedBy(deathLightAngles[7] - MathHelper.PiOver4 / 5) - screenPos, lightWhite, Color.Transparent, Color.Transparent);
                ShapeBatch.Triangle(NPC.Center - screenPos, NPC.Center + (Vector2.UnitY * deathLightLengths[8]).RotatedBy(deathLightAngles[8] + MathHelper.PiOver4 / 5) - screenPos, NPC.Center + (Vector2.UnitY * deathLightLengths[8]).RotatedBy(deathLightAngles[8] - MathHelper.PiOver4 / 5) - screenPos, lightWhite, Color.Transparent, Color.Transparent);
                ShapeBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
                return;
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
            if (exitTicker > 0) //Cut out all other draw calls if despawning
                return;

            if (teleIndicator)
            {
                Texture2D texTele = Request<Texture2D>(AssetDirectory.CrimsonKnight + "TeleportIndicator").Value;
                Player p = Main.player[NPC.target];
                for (int i = 0; i < 6; i++)
                {
                    if (teleportLocations[i])
                    {
                        //adjust to not be out of arena
                        Vector2 offset = new Vector2(i<3? -texTele.Width : texTele.Width, Arena_Top_Left.Y + (i%3==0? -40 : i%3==1? 280 : 596) - p.Center.Y);
                        Vector2 coordinatesOfDash = p.Center + offset;
                        if (coordinatesOfDash.X < minTeleportX + NPC.width / 2)
                            coordinatesOfDash.X = minTeleportX + NPC.width / 2;
                        if (coordinatesOfDash.X > maxTeleportX - NPC.width / 2)
                            coordinatesOfDash.X = maxTeleportX - NPC.width / 2;
                        spriteBatch.Draw(texTele, coordinatesOfDash - screenPos, null, Color.Lerp(new Color(0, 0, 0, 0), new Color(255, 255, 255, 255), (float)(-1 * (loopCounter - 30)) / 30f), 0, new Vector2(texTele.Width, texTele.Height) / 2, 1 + (0.02f * loopCounter), i<3? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                    }
                }

                //spriteBatch.Draw(texTele, (Main.player[NPC.target].Bottom + new Vector2(!left ? texTele.Width : -texTele.Width, -texTele.Height / 2)) - screenPos, null, Color.Lerp(new Color(0, 0, 0, 0), new Color(255, 255, 255, 255), (float)(-1 * (loopCounter - 30)) / 30f), 0, new Vector2(texTele.Width, texTele.Height) / 2, 1 + (0.02f * loopCounter), !left ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            }

            if (dashIndicator)
            {
                Texture2D texDash = Request<Texture2D>(AssetDirectory.CrimsonKnight + "DashIndicator").Value;
                //allign with player X
                float difference = Main.player[NPC.target].Center.X - NPC.Center.X;
                spriteBatch.Draw(texDash, NPC.Center + (new Vector2(left ? Math.Max(200, difference) : Math.Min(-200, difference), 0)) - screenPos, null, new Color(255, 255, 255, 0), 0, new Vector2(texDash.Width, texDash.Height) / 2, 1 + (0.04f * loopCounter), left ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
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
            if (introAnimation || phaseTransition || noContactDamage)
                return false;
            return base.CanBeHitByProjectile(projectile);
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            if (introAnimation || phaseTransition || noContactDamage)
                return false;
            return base.CanBeHitByItem(player, item);
        }

        public override bool CanHitNPC(NPC target)/* tModPorter Suggestion: Return true instead of null */
        {
            return !noContactDamage;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
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

        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (parry)
            {
                parryDamaged += item.damage;
                if (parryDamaged > 20 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    parryDamaged = 0;
                    parryRetaliate++;
                }
                modifiers.SetMaxDamage(1);
            }
            base.ModifyHitByItem(player, item, ref modifiers);
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (parry)
            {
                parryDamaged += projectile.damage;
                if (parryDamaged > 20 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    parryDamaged = 0;
                    parryRetaliate++;
                }
                modifiers.SetMaxDamage(1);
            }
            base.ModifyHitByProjectile(projectile, ref modifiers);
        }

        public override bool CheckDead()
        {
            if (DeathTimer == 0)
            {
                DeathTimer = 1;
                NPC.damage = 0;
                NPC.life = NPC.lifeMax;
                NPC.dontTakeDamage = true;
                NPC.netUpdate = true;
                RemoveProjectiles();
                return false;
            }
            return true;
        }

        public override void OnKill()
        {
            SoundEngine.PlaySound(SoundID.NPCDeath10, NPC.Center);
            int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, 0, 0, 612, 0, 0, Main.myPlayer, 1, 10);
            SoundEngine.PlaySound(SoundID.Item14, Main.projectile[proj].position);
            Main.projectile[proj].hostile = false;

            //gores
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, (-Vector2.UnitY * (Main.rand.NextFloat(3) + .5f)).RotatedByRandom(MathHelper.PiOver2), Mod.Find<ModGore>(Name + "_gore1").Type, NPC.scale);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, (-Vector2.UnitY * (Main.rand.NextFloat(3) + .5f)).RotatedByRandom(MathHelper.PiOver2), Mod.Find<ModGore>(Name + "_gore2").Type, NPC.scale);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, (-Vector2.UnitY * (Main.rand.NextFloat(3) + .5f)).RotatedByRandom(MathHelper.PiOver2), Mod.Find<ModGore>(Name + "_gore3").Type, NPC.scale);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, (-Vector2.UnitY * (Main.rand.NextFloat(3) + .5f)).RotatedByRandom(MathHelper.PiOver2), Mod.Find<ModGore>(Name + "_gore4").Type, NPC.scale);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, (-Vector2.UnitY * (Main.rand.NextFloat(3) + .5f)).RotatedByRandom(MathHelper.PiOver2), Mod.Find<ModGore>(Name + "_gore5").Type, NPC.scale);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, (-Vector2.UnitY * (Main.rand.NextFloat(3) + .5f)).RotatedByRandom(MathHelper.PiOver2), Mod.Find<ModGore>(Name + "_gore6").Type, NPC.scale);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, (-Vector2.UnitY * (Main.rand.NextFloat(3) + .5f)).RotatedByRandom(MathHelper.PiOver2), Mod.Find<ModGore>(Name + "_gore7").Type, NPC.scale);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, (-Vector2.UnitY * (Main.rand.NextFloat(3) + .5f)).RotatedByRandom(MathHelper.PiOver2), Mod.Find<ModGore>(Name + "_gore8").Type, NPC.scale);
            base.OnKill();
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.NotScalingWithLuck(ItemType<Items.Weapons.Magic.BurningSphere>()));
            npcLoot.Add(ItemDropRule.NotScalingWithLuck(ItemType<Items.Weapons.Ranger.MagmaMortar>()));
            npcLoot.Add(ItemDropRule.NotScalingWithLuck(ItemType<Items.Weapons.Summoner.Whips.FlameTongue>()));
            npcLoot.Add(ItemDropRule.NotScalingWithLuck(ItemType<Items.Weapons.Melee.InfernalSledge>()));
            npcLoot.Add(ItemDropRule.NotScalingWithLuck(ItemType<Items.Weapons.Melee.FlameTongueGreatsword>()));
            LeadingConditionRule expertRule = new LeadingConditionRule(new Conditions.IsExpert());
            expertRule.OnSuccess(ItemDropRule.NotScalingWithLuck(ItemType<Items.Accessories.CrimsonCrest>()));
            npcLoot.Add(expertRule);
        }
    }
}   
