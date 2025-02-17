using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Core;
using ExoriumMod.Content.Dusts;
using ExoriumMod.Helpers;
using ReLogic.Content;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using ExoriumMod.Content.Biomes;

namespace ExoriumMod.Content.Bosses.BlightedSlime
{
    [AutoloadBossHead]
    class BlightedSlime : ModNPC
    {
        public override string Texture => AssetDirectory.BlightedSlime + Name;
        public override string BossHeadTexture => AssetDirectory.BlightedSlime + Name + "_Head_Boss";

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Scale = .5f,
                PortraitPositionXOverride = 0f,
                PortraitPositionYOverride = 46f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);

            NPCID.Sets.BossBestiaryPriority.Add(Type);

            // Add this in for bosses that have a summon item, requires corresponding code in the item (See MinionBossSummonItem.cs)
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            // Automatically group with other bosses
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 4400;
            NPC.damage = 49;
            NPC.defense = 16;
            NPC.knockBackResist = 0f;
            NPC.width = 174;
            NPC.height = 120;
            NPC.value = Item.buyPrice(0, 4, 0, 0);
            NPC.npcSlots = 15f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.alpha = 15;
            NPC.scale = 2f;
            NPC.timeLeft = NPC.activeTime * 30;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<DeadlandBiome>().Type };
            if (!Main.dedServ)
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Sounds/Music/SlimyGrime");
        }
         
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.7 * balance);
            NPC.damage = (int)(NPC.damage * 0.8f);
        }

        bool flag3 = false;
        bool flag6 = false;
        bool flag7 = false;
        float timer = 0;

        public override void AI()
        {
            //npc.ai[0] = move delay
            //npc.ai[1] = action to make
            //mpc.ai[2] = timer to teleport based on player being too far
            //npc.aiAction = play animation
            int damage = NPC.damage / (Main.expertMode == true ? 4 : 2);
            float num1 = 1f;
            bool flag1 = false; //npc.ai[1] will increase when flag1 is false later on
            bool flag2 = false; //allows boss to become invisible?
            NPC.aiAction = 0;
            if (NPC.localAI[3] == 0f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.ai[0] = -100f;
                NPC.localAI[3] = 1f;
                NPC.TargetClosest(true);
                NPC.netUpdate = true;
            }
            if (Main.player[NPC.target].dead)
            {
                NPC.TargetClosest(true);
                if (Main.player[NPC.target].dead)
                {
                    NPC.timeLeft = 0;
                    if (Main.player[NPC.target].Center.X < NPC.Center.X)
                    {
                        NPC.direction = 1;
                    }
                    else
                    {
                        NPC.direction = -1;
                    }
                }
            }
            if (NPC.ai[1] == 8f)
            {
                flag1 = true;
                NPC.aiAction = 1;
                ref float reference = ref NPC.ai[0];
                reference += 1f;
                num1 = MathHelper.Clamp((60f - NPC.ai[0]) / 60f, 0f, 1f);
                num1 = 0.5f + num1 * 0.5f;
                if (NPC.ai[0] >= 60f)
                {
                    flag2 = true;
                }
                if (NPC.ai[0] >= 60f && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.Bottom = new Vector2(NPC.localAI[1], NPC.localAI[2]);
                    NPC.ai[1] = 9f;
                    NPC.ai[0] = 0f;
                    NPC.netUpdate = true;
                }
                if (Main.netMode == NetmodeID.MultiplayerClient && NPC.ai[0] >= 120f)
                {
                    NPC.ai[1] = 9f;
                    NPC.ai[0] = 0f;
                }
                if (!flag2)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        //Dust
                        int dust0 = Dust.NewDust(NPC.position + Vector2.UnitX * -20f, NPC.width + 40, NPC.height, DustType<BlightDust>(), 0, 0, 150, default(Color), 2f);
                    }
                }
            }
            else if (NPC.ai[1] == 9f)
            {
                flag1 = true;
                NPC.aiAction = 0;
                ref float reference = ref NPC.ai[0];
                reference += 1f;
                num1 = MathHelper.Clamp(NPC.ai[0] / 30f, 0f, 1f);
                num1 = 0.5f + num1 * 0.5f;
                if (NPC.ai[0] >= 30f && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.ai[1] = 1f;
                    NPC.ai[0] = 0f;
                    NPC.netUpdate = true;
                    NPC.TargetClosest(true);
                }
                if (Main.netMode == NetmodeID.MultiplayerClient && NPC.ai[0] >= 60f)
                {
                    NPC.ai[1] = 1f;
                    NPC.ai[0] = 0f;
                    NPC.TargetClosest(true);
                }
                for (int i = 0; i < 10; i++)
                {
                    //dust
                    int dust1 = Dust.NewDust(NPC.position + Vector2.UnitX * -20f, NPC.width + 40, NPC.height, DustType<BlightDust>(), 0, 0, 150, default(Color), 2f);
                }
            }
            NPC.dontTakeDamage = (NPC.hide = flag2);
            if (NPC.velocity.Y == 0f)
            {
                NPC.velocity.X = NPC.velocity.X * 0.8f;
                if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
                {
                    NPC.velocity.X = 0f;
                }
                if (!flag1) // actions cooldown faster at lower hp
                {
                    ref float reference = ref NPC.ai[0];
                    reference += 2f;
                    if ((double)NPC.life < (double)NPC.lifeMax * 0.8)
                    {
                        NPC.ai[0] += 1f;
                    }
                    if ((double)NPC.life < (double)NPC.lifeMax * 0.6)
                    {
                        NPC.ai[0] += 1f;
                    }
                    if ((double)NPC.life < (double)NPC.lifeMax * 0.4)
                    {
                        NPC.ai[0] += 1f;
                    }
                    if ((double)NPC.life < (double)NPC.lifeMax * 0.2)
                    {
                        NPC.ai[0] += 2f;
                    }
                    if ((double)NPC.life < (double)NPC.lifeMax * 0.1)
                    {
                        NPC.ai[0] += 3f;
                    }
                    if (flag3 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 vector3 = new Vector2(NPC.position.X + (float)(NPC.width / 2), NPC.position.Y);
                        float num17 = (float)(NPC.position.X + (float)(NPC.width / 2) + vector3.X);
                        float num18 = -(float)(NPC.position.X + (float)(NPC.width / 2) + vector3.X);
                        float num19 = (float)(NPC.position.Y - vector3.Y);
                        float num20 = (float)Math.Sqrt((double)(num17 * num17 + num19 * num19));
                        float num21 = (float)Math.Sqrt((double)(num18 * num18 + num19 * num19));
                        num20 = 5 / num20;
                        num21 = 5 / num21;
                        num17 *= num20;
                        num18 *= num21;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 2f * num17, num20, ProjectileType<SpikePlacer>(), damage, 0, Main.myPlayer, 0, 0);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 2f * num18, num21, ProjectileType<SpikePlacer>(), damage, 0, Main.myPlayer, 0, 0);
                        SoundEngine.PlaySound(SoundID.Item14, NPC.position);
                        //Dust
                        for (int i = 0; i < 100; i++)
                        {
                            //Dust
                            int dustStomp = Dust.NewDust(NPC.position + Vector2.UnitX * -20f, NPC.width + 40, NPC.height, DustType<BlightDust>(), Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-40, -10), 150, default(Color), Main.rand.NextFloat(1.5f, 3));
                        }
                        flag3 = false;
                    }
                    if (NPC.ai[0] >= 0f)
                    {
                        NPC.netUpdate = true;
                        NPC.TargetClosest(true);
                        if (NPC.ai[1] == 7f)
                        {
                            if ((double)NPC.life < (double)NPC.lifeMax * .70 || Main.expertMode)
                            {
                                flag3 = true;
                            }
                            flag6 = true;
                            timer = 0;
                            NPC.ai[0] = -180f;
                            NPC.ai[1] = 0f;
                        }
                        else if (NPC.ai[1] == 6f)
                        {
                            NPC.velocity.Y = -10f;
                            NPC.velocity.X = NPC.velocity.X + 7f * (float)NPC.direction;
                            NPC.ai[0] = -100f;
                            NPC.ai[1] += 1f;
                        }
                        else if (NPC.ai[1] == 5f)
                        {
                            NPC.velocity.Y = -9f;
                            NPC.velocity.X = NPC.velocity.X + 5f * (float)NPC.direction;
                            NPC.ai[0] = -100f;
                            NPC.ai[1] += 1f;
                        }
                        else if (NPC.ai[1] == 4f)
                        {
                            NPC.velocity.Y = -18;
                            NPC.velocity.X = 0;
                            if ((double)NPC.life < (double)NPC.lifeMax * 0.85 || Main.expertMode && Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                for (int i = 1; i < Main.rand.Next(10, 17) + 10 * (1 - (int)(NPC.life/NPC.lifeMax)); i++)
                                {
                                    Vector2 vector1 = new Vector2(NPC.position.X + (float)(NPC.width / 2) + (float)(Main.rand.Next(20) * NPC.direction), NPC.position.Y + (float)NPC.height * 0.8f);
                                    float num14 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector1.X + (float)Main.rand.Next(-80, 81);
                                    float num15 = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - vector1.Y + (float)Main.rand.Next(-40, 41);
                                    float num16 = (float)Math.Sqrt((double)(num14 * num14 + num15 * num15));
                                    num16 = 5 / num16;
                                    num14 *= num16;
                                    num15 *= num16;
                                    num14 += (float)Main.rand.Next(-50, 51)/10f;
                                    num15 -= 12;
                                    num15 += (float)Main.rand.Next(-10, 11)/10f;
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, num14, num15, ProjectileType<BlightSlimeShot>(), damage, 1, Main.myPlayer, 0, 0);
                                    NPC.noTileCollide = true;
                                }
                            }
                            int dust2 = Dust.NewDust(NPC.position + Vector2.UnitX * -20f, NPC.width + 60, NPC.height, DustType<BlightDust>(), 0, 0, 150, default(Color), 2f);
                            NPC.ai[0] = -180;
                            NPC.ai[1] += 1f;
                        }
                        else if (NPC.ai[1] == 3f)
                        {
                            NPC.velocity.Y = -13f;
                            NPC.velocity.X = NPC.velocity.X + 5.5f * (float)NPC.direction;
                            NPC.ai[0] = -160f;
                            NPC.ai[1] += 1f;
                        }
                        else if (NPC.ai[1] == 2f)
                        {
                            if (NPC.life < NPC.lifeMax * .25 || (NPC.life < NPC.lifeMax * .5 && Main.expertMode))
                            {
                                if (!flag7)
                                    timer = 0;
                                flag7 = true;
                            }
                            else
                            {
                                NPC.velocity.Y = -5.5f;
                                NPC.velocity.X = NPC.velocity.X + 7f * (float)NPC.direction;
                                NPC.ai[0] = -100f;
                                NPC.ai[1] += 1f;
                            }
                        }
                        else
                        {
                            NPC.velocity.Y = -9f;
                            NPC.velocity.X = NPC.velocity.X + 5f * (float)NPC.direction;
                            NPC.ai[0] = -120f;
                            NPC.ai[1] += 1f;
                        }
                    }
                    else if (NPC.ai[0] >= -30f)
                    {
                        NPC.aiAction = 1;
                    }
                }
            }
            else if (NPC.target < 255)
            {
                if ((NPC.direction == 1 && NPC.velocity.X < 3f) || (NPC.direction == -1 && NPC.velocity.X > -3f))
                {
                    if ((NPC.direction == -1 && (double)NPC.velocity.X < 0.1) || (NPC.direction == 1 && (double)NPC.velocity.X > -0.1))
                    {
                        if (NPC.ai[1] != 4 && NPC.ai[1] != 7)
                        {
                            NPC.velocity.X = NPC.velocity.X + 0.2f * (float)NPC.direction;
                        }
                        //dust
                        int dust2 = Dust.NewDust(NPC.position + Vector2.UnitX * -20f, NPC.width + 60, NPC.height, DustType<BlightDust>(), 0, 0, 150, default(Color), 2f);
                        if (NPC.life > 0)
                        {
                            float num11 = 1;
                            num11 = num11 * 0.5f + 1.25f;
                            num11 *= num1;
                            if (num11 != NPC.scale)
                            {
                                NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
                                NPC.position.Y = NPC.position.Y + (float)NPC.height;
                                NPC.scale = num11;
                                NPC.width = (int)(98f * NPC.scale);
                                NPC.height = (int)(92f * NPC.scale);
                                NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
                                NPC.position.Y = NPC.position.Y - (float)NPC.height;
                            }
                        }
                    }
                    NPC.velocity.X = NPC.velocity.X * 0.93f;
                    //Dust
                    int dust = Dust.NewDust(NPC.position + Vector2.UnitX * -20f, NPC.width + 40, NPC.height, DustType<BlightDust>(), 0, 0, 150, default(Color), 2f);
                    if (NPC.life > 0)
                    {
                        float num11 = 1;
                        num11 = num11 * 0.5f + 1.25f;
                        num11 *= num1;
                        if (num11 != NPC.scale)
                        {
                            NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
                            NPC.position.Y = NPC.position.Y + (float)NPC.height;
                            NPC.scale = num11;
                            NPC.width = (int)(98f * NPC.scale);
                            NPC.height = (int)(92f * NPC.scale);
                            NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
                            NPC.position.Y = NPC.position.Y - (float)NPC.height;
                        }
                    }
                }
            }
            int dust4 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<BlightDust>(), NPC.velocity.X, NPC.velocity.Y, 255, default(Color), NPC.scale * 1.2f);
            Main.dust[dust4].noGravity = true;
            Dust dust46 = Main.dust[dust4];
            dust46.velocity *= 0.5f;
            if (NPC.life > 0)
            {
                float num11 = 1;
                num11 = num11 * 0.5f + 1.25f;
                num11 *= num1;
                if (num1 != NPC.scale)
                {
                    NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
                    NPC.position.Y = NPC.position.Y + (float)NPC.height;
                    NPC.scale = num11;
                    NPC.width = (int)(98f * NPC.scale);
                    NPC.height = (int)(92f * NPC.scale);
                    NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
                    NPC.position.Y = NPC.position.Y - (float)NPC.height;
                }
            }
            if (NPC.ai[1] == 5 || NPC.ai[1] == 0)
            {
                NPC.velocity.X = 0f;
            }
            if (flag6 && NPC.Bottom.Y > Main.player[NPC.target].position.Y - 700 && timer < 600)
            {
                NPC.alpha += 2;
                NPC.velocity.Y = -7f;
                timer++;
                NPC.noTileCollide = true;
                NPC.noGravity = true;
            }
            else if (flag6)
            {
                flag6 = false;
                NPC.position.X = Main.player[NPC.target].position.X - NPC.width * 0.5f;
                NPC.velocity.Y = 14f;
                NPC.netUpdate = true;
            }
            if (NPC.Bottom.Y > Main.player[NPC.target].Center.Y && !flag6) //Make tilecollide true again if jump into air is done
            {
                NPC.noTileCollide = false;
                NPC.noGravity = false;
            }

            if (flag7)
            {
                for (int i = 0; i< 3; i++) //Dust
                {
                    int dust2 = Dust.NewDust(NPC.position + Vector2.UnitX * -20f, NPC.width + 60, NPC.height, DustType<BlightDust>(), 0, 0, 150, default(Color), 2f);
                }
                timer++;
                NPC.aiAction = 1;
                if (timer % 15 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 up = new Vector2(0, -30);
                    Vector2 altUp = up.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-15, 15)));
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, altUp.X, altUp.Y, ProjectileType<BlightedThorn>(), damage, 1.5f, Main.myPlayer, 0, 0);
                }
                if (timer > 180)
                {
                    NPC.ai[1] += 1f;
                    NPC.ai[0] = -110;
                    flag7 = false;
                    NPC.aiAction = 0;
                }
            }

            if (NPC.alpha > 30)
            {
                NPC.alpha -= 2;
            }
            if (!Main.player[NPC.target].dead && NPC.ai[2] >= 500f && NPC.ai[1] < 8f && NPC.velocity.Y == 0f) // teleport when player far
            {
                NPC.ai[2] = 0f;
                NPC.ai[0] = 0f;
                NPC.ai[1] = 8f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.TargetClosest(false);
                    Point pointNPC = NPC.Center.ToTileCoordinates();
                    Point pointPlayer = Main.player[NPC.target].Center.ToTileCoordinates();
                    Vector2 vector2 = Main.player[NPC.target].Center - NPC.Center;
                    int num2 = 10;
                    int num3 = 0;
                    int num4 = 7;
                    int num5 = 0;
                    bool flag4 = false;
                    if (vector2.Length() > 2000f)
                    {
                        flag4 = true;
                        num5 = 100;
                    }
                    while (!flag4 && num5 < 100)
                    {
                        int num6 = num5;
                        num5 = num6 + 1;
                        int num7 = Main.rand.Next(pointPlayer.X - num2, pointPlayer.X + num2 + 1);
                        int num8 = Main.rand.Next(pointPlayer.Y - num2, pointPlayer.Y + 1);
                        if ((num8 < pointPlayer.Y - num4 || num8 > pointPlayer.Y + num4 || num7 < pointPlayer.X - num4 || num7 > pointPlayer.X + num4) && (num8 < pointNPC.Y - num3 || num8 > pointNPC.Y + num3 || num7 < pointNPC.X - num3 || num7 > pointNPC.X + num3) && !Main.tile[num7, num8].HasUnactuatedTile)
                        {
                            int num9 = num8;
                            int num10 = 0; // Shifts teleport y position if the chosen tile is active
                            if (Main.tile[num7, num9].HasUnactuatedTile && Main.tileSolid[Main.tile[num7, num9].TileType] /*&& !Main.tileSolidTop[Main.tile[num7, num9].type]*/)
                            {
                                num10 = 1;
                            }
                            else
                            {
                                while (num10 < 150 && num9 + num10 < Main.maxTilesY)
                                {
                                    int num3247 = num9 + num10;
                                    if (Main.tile[num7, num3247].HasUnactuatedTile && Main.tileSolid[Main.tile[num7, num3247].TileType] /*&& !Main.tileSolidTop[Main.tile[num7, num3247].type]*/)
                                    {
                                        num6 = num10;
                                        num10 = num6 - 1;
                                        break;
                                    }
                                    num6 = num10;
                                    num10 = num6 + 1;
                                }
                            }
                            num8 += num10;
                            bool flag5 = true;
                            if (flag5 && (Main.tile[num7, num8].LiquidType == LiquidID.Lava))
                            {
                                flag5 = false;
                            }
                            if (flag5 && !Collision.CanHitLine(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0))
                            {
                                flag5 = false;
                            }
                            if (flag5)
                            {
                                NPC.localAI[1] = (float)(num7 * 16 + 8);
                                NPC.localAI[2] = (float)(num8 * 16 + 16);
                                break;
                            }
                        }
                    }
                    if (num5 >= 100)
                    {
                        Vector2 bottom = Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)].Bottom;
                        NPC.localAI[1] = bottom.X;
                        NPC.localAI[2] = bottom.Y;
                    }
                }
            }
            if (!Collision.CanHitLine(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0)) // prevents collision teleports
            {
                ref float reference = ref NPC.ai[2];
                reference += 1f;
            }
            if (Math.Abs(NPC.Top.Y - Main.player[NPC.target].Bottom.Y) > 500f) // prevents teleports that are too far
            {
                ref float reference = ref NPC.ai[2];
                reference += 1f;
            }
        }

        public override void FindFrame(int num)
        {
            int num12 = 0;
            if (NPC.aiAction == 0)
            {
                num12 = ((!(NPC.velocity.Y < 0f)) ? ((!(NPC.velocity.Y > 0f)) ? ((NPC.velocity.X != 0f) ? 1 : 0) : 3) : 2);
            }
            else if (NPC.aiAction == 1)
            {
                num12 = 4;
            }
            if (NPC.velocity.Y != 0f)
            {
                if (NPC.frame.Y < num * 4)
                {
                    NPC.frame.Y = num * 4;
                    NPC.frameCounter = 0.0;
                }
                if ((NPC.frameCounter += 1.0) >= 4.0)
                {
                    NPC.frame.Y = num * 5;
                }
            }
            else
            {
                if (NPC.frame.Y >= num * 5)
                {
                    NPC.frame.Y = num * 4;
                    NPC.frameCounter = 0.0;
                }
                NPC.frameCounter += 1.0;
                if (num12 > 0)
                {
                    NPC.frameCounter += 1.0;
                }
                if (num12 == 4)
                {
                    NPC.frameCounter += 1.0;
                }
                if (NPC.frameCounter >= 8.0)
                {
                    NPC.frame.Y = NPC.frame.Y + num;
                    NPC.frameCounter = 0.0;
                    if (NPC.frame.Y >= num * 4)
                    {
                        NPC.frame.Y = 0;
                    }
                }
            }
        }

        public override bool PreKill()
        {
            //Update
            if (!Core.Systems.DownedBossSystem.downedBlightslime)
            {
                Core.Systems.DownedBossSystem.downedBlightslime = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                }
            }
            //dust
            DustHelper.DustCircle(NPC.Center, DustType<BlightDust>(), 28, 150, 2.5f, 1.5f, 0, 0, default, true);
            return true;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "The Blighted Slime";
            potionType = ItemID.LesserHealingPotion;
        }

        public override void OnKill()
        {
            //dust
            DustHelper.DustCircle(NPC.Center, DustType<BlightDust>(), 28, 150, 2.5f, 1.5f, 0, 0, default, true);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ItemType<BlightedSlimeBag>()));

            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

            notExpertRule.OnSuccess(ItemDropRule.Common(ItemType<Items.Materials.TaintedGel>(), 1, 50, 66));
            notExpertRule.OnSuccess(ItemDropRule.Common(ItemType<Items.Materials.Metals.BlightedOre>(), 1, 80, 120));

            npcLoot.Add(notExpertRule);
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
            new FlavorTextBestiaryInfoElement("A mass formed of an ooze-like substance quite similar to the metal it coagulated into upon defeat. It shares the same desire to devour as others of its ilk."),
            new BestiaryPortraitBackgroundProviderPreferenceInfoElement(ModContent.GetInstance<DeadlandBiome>().ModBiomeBestiaryInfoElement)
            });
        }
    }
}
