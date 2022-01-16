using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Core;
using ExoriumMod.Content.Dusts;

namespace ExoriumMod.Content.Bosses.BlightedSlime
{
    [AutoloadBossHead]
    class BlightedSlime : ModNPC
    {
        public override string Texture => AssetDirectory.BlightedSlime + Name;
        public override string BossHeadTexture => AssetDirectory.BlightedSlime + Name + "_Head";

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 4000;
            npc.damage = 49;
            npc.defense = 16;
            npc.knockBackResist = 0f;
            npc.width = 174;
            npc.height = 120;
            npc.value = Item.buyPrice(0, 4, 0, 0);
            npc.npcSlots = 15f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.alpha = 15;
            npc.scale = 2f;
            npc.timeLeft = NPC.activeTime * 30;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/SlimyGrime");
            bossBag = ItemType<BlightedSlimeBag>();
        }
         
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.7 * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.8f);
        }

        bool flag3 = false;
        bool flag6 = false;
        float timer = 0;

        public override void AI()
        {
            //npc.ai[0] = move delay
            //npc.ai[1] = action to make
            //mpc.ai[2] = timer to teleport based on player being too far
            //npc.aiAction = play animation
            int damage = npc.damage / (Main.expertMode == true ? 4 : 2);
            float num1 = 1f;
            bool flag1 = false; //npc.ai[1] will increase when flag1 is false later on
            bool flag2 = false; //allows boss to become invisible?
            npc.aiAction = 0;
            if (npc.localAI[3] == 0f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.ai[0] = -100f;
                npc.localAI[3] = 1f;
                npc.TargetClosest(true);
                npc.netUpdate = true;
            }
            if (Main.player[npc.target].dead)
            {
                npc.TargetClosest(true);
                if (Main.player[npc.target].dead)
                {
                    npc.timeLeft = 0;
                    if (Main.player[npc.target].Center.X < npc.Center.X)
                    {
                        npc.direction = 1;
                    }
                    else
                    {
                        npc.direction = -1;
                    }
                }
            }
            if (npc.ai[1] == 8f)
            {
                flag1 = true;
                npc.aiAction = 1;
                ref float reference = ref npc.ai[0];
                reference += 1f;
                num1 = MathHelper.Clamp((60f - npc.ai[0]) / 60f, 0f, 1f);
                num1 = 0.5f + num1 * 0.5f;
                if (npc.ai[0] >= 60f)
                {
                    flag2 = true;
                }
                if (npc.ai[0] >= 60f && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.Bottom = new Vector2(npc.localAI[1], npc.localAI[2]);
                    npc.ai[1] = 9f;
                    npc.ai[0] = 0f;
                    npc.netUpdate = true;
                }
                if (Main.netMode == NetmodeID.MultiplayerClient && npc.ai[0] >= 120f)
                {
                    npc.ai[1] = 9f;
                    npc.ai[0] = 0f;
                }
                if (!flag2)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        //Dust
                        int dust0 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, DustType<BlightDust>(), 0, 0, 150, default(Color), 2f);
                    }
                }
            }
            else if (npc.ai[1] == 9f)
            {
                flag1 = true;
                npc.aiAction = 0;
                ref float reference = ref npc.ai[0];
                reference += 1f;
                num1 = MathHelper.Clamp(npc.ai[0] / 30f, 0f, 1f);
                num1 = 0.5f + num1 * 0.5f;
                if (npc.ai[0] >= 30f && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.ai[1] = 1f;
                    npc.ai[0] = 0f;
                    npc.netUpdate = true;
                    npc.TargetClosest(true);
                }
                if (Main.netMode == NetmodeID.MultiplayerClient && npc.ai[0] >= 60f)
                {
                    npc.ai[1] = 1f;
                    npc.ai[0] = 0f;
                    npc.TargetClosest(true);
                }
                for (int i = 0; i < 10; i++)
                {
                    //dust
                    int dust1 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, DustType<BlightDust>(), 0, 0, 150, default(Color), 2f);
                }
            }
            npc.dontTakeDamage = (npc.hide = flag2);
            if (npc.velocity.Y == 0f)
            {
                npc.velocity.X = npc.velocity.X * 0.8f;
                if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
                {
                    npc.velocity.X = 0f;
                }
                if (!flag1) // actions cooldown faster at lower hp
                {
                    ref float reference = ref npc.ai[0];
                    reference += 2f;
                    if ((double)npc.life < (double)npc.lifeMax * 0.8)
                    {
                        npc.ai[0] += 1f;
                    }
                    if ((double)npc.life < (double)npc.lifeMax * 0.6)
                    {
                        npc.ai[0] += 1f;
                    }
                    if ((double)npc.life < (double)npc.lifeMax * 0.4)
                    {
                        npc.ai[0] += 1f;
                    }
                    if ((double)npc.life < (double)npc.lifeMax * 0.2)
                    {
                        npc.ai[0] += 2f;
                    }
                    if ((double)npc.life < (double)npc.lifeMax * 0.1)
                    {
                        npc.ai[0] += 2f;
                    }
                    if (flag3)
                    {
                        Vector2 vector3 = new Vector2(npc.position.X + (float)(npc.width / 2), npc.position.Y);
                        float num17 = (float)(npc.position.X + (float)(npc.width / 2) + vector3.X);
                        float num18 = -(float)(npc.position.X + (float)(npc.width / 2) + vector3.X);
                        float num19 = (float)(npc.position.Y - vector3.Y);
                        float num20 = (float)Math.Sqrt((double)(num17 * num17 + num19 * num19));
                        float num21 = (float)Math.Sqrt((double)(num18 * num18 + num19 * num19));
                        num20 = 5 / num20;
                        num21 = 5 / num21;
                        num17 *= num20;
                        num18 *= num21;
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 2f * num17, num20, ProjectileType<SpikePlacer>(), damage, 0, Main.myPlayer, 0, 0);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 2f * num18, num21, ProjectileType<SpikePlacer>(), damage, 0, Main.myPlayer, 0, 0);
                        Main.PlaySound(SoundID.Item14, npc.position);
                        //Dust
                        for (int i = 0; i < 100; i++)
                        {
                            //Dust
                            int dustStomp = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, DustType<BlightDust>(), Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-40, -10), 150, default(Color), Main.rand.NextFloat(1.5f, 3));
                        }
                        flag3 = false;
                    }
                    if (npc.ai[0] >= 0f)
                    {
                        npc.netUpdate = true;
                        npc.TargetClosest(true);
                        if (npc.ai[1] == 7f)
                        {
                            if ((double)npc.life < (double)npc.lifeMax * .70 || Main.expertMode)
                            {
                                flag3 = true;
                            }
                            if (Main.expertMode && (npc.life < npc.lifeMax * 0.35))
                            {
                                for (int i = 1; i < Main.rand.Next(10, 17); i++)
                                {
                                    Vector2 vector1 = new Vector2(npc.position.X + (float)(npc.width / 2) + (float)(Main.rand.Next(20) * npc.direction), npc.position.Y + (float)npc.height * 0.8f);
                                    float num14 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector1.X + (float)Main.rand.Next(-80, 81);
                                    float num15 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector1.Y + (float)Main.rand.Next(-40, 41);
                                    float num16 = (float)Math.Sqrt((double)(num14 * num14 + num15 * num15));
                                    num16 = 5 / num16;
                                    num14 *= num16;
                                    num15 *= num16;
                                    num14 += (float)Main.rand.Next(-50, 51)/10f;
                                    num15 -= 12;
                                    num15 += (float)Main.rand.Next(-30, 31)/10f;
                                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, num14, num15, ProjectileType<BlightSlimeShot>(), damage, 1, Main.myPlayer, 0, 0);
                                }
                            }
                            flag6 = true;
                            timer = 0;
                            npc.ai[0] = -200f;
                            npc.ai[1] = 0f;
                        }
                        else if (npc.ai[1] == 6f)
                        {
                            npc.velocity.Y = -10f;
                            npc.velocity.X = npc.velocity.X + 7f * (float)npc.direction;
                            npc.ai[0] = -120f;
                            npc.ai[1] += 1f;
                        }
                        else if (npc.ai[1] == 5f)
                        {
                            npc.velocity.Y = -9f;
                            npc.velocity.X = npc.velocity.X + 5f * (float)npc.direction;
                            npc.ai[0] = -120f;
                            npc.ai[1] += 1f;
                        }
                        else if (npc.ai[1] == 4f)
                        {
                            npc.velocity.Y = -18;
                            npc.velocity.X = 0;
                            if ((double)npc.life < (double)npc.lifeMax * 0.5 && Main.expertMode)
                            {
                                flag3 = true;
                            }
                            if ((double)npc.life < (double)npc.lifeMax * 0.75 || Main.expertMode)
                            {
                                for (int i = 1; i < Main.rand.Next(10, 17); i++)
                                {
                                    Vector2 vector1 = new Vector2(npc.position.X + (float)(npc.width / 2) + (float)(Main.rand.Next(20) * npc.direction), npc.position.Y + (float)npc.height * 0.8f);
                                    float num14 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector1.X + (float)Main.rand.Next(-80, 81);
                                    float num15 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector1.Y + (float)Main.rand.Next(-40, 41);
                                    float num16 = (float)Math.Sqrt((double)(num14 * num14 + num15 * num15));
                                    num16 = 5 / num16;
                                    num14 *= num16;
                                    num15 *= num16;
                                    num14 += (float)Main.rand.Next(-50, 51)/10f;
                                    num15 -= 12;
                                    num15 += (float)Main.rand.Next(-10, 11)/10f;
                                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, num14, num15, ProjectileType<BlightSlimeShot>(), damage, 1, Main.myPlayer, 0, 0);
                                    npc.noTileCollide = true;
                                }
                            }
                            int dust2 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 60, npc.height, DustType<BlightDust>(), 0, 0, 150, default(Color), 2f);
                            npc.ai[0] = -200;
                            npc.ai[1] += 1f;
                        }
                        else if (npc.ai[1] == 3f)
                        {
                            npc.velocity.Y = -13f;
                            npc.velocity.X = npc.velocity.X + 5.5f * (float)npc.direction;
                            npc.ai[0] = -200f;
                            npc.ai[1] += 1f;
                        }
                        else if (npc.ai[1] == 2f)
                        {
                            npc.velocity.Y = -5.5f;
                            npc.velocity.X = npc.velocity.X + 7f * (float)npc.direction;
                            npc.ai[0] = -120f;
                            npc.ai[1] += 1f;
                        }
                        else
                        {
                            npc.velocity.Y = -9f;
                            npc.velocity.X = npc.velocity.X + 5f * (float)npc.direction;
                            npc.ai[0] = -120f;
                            npc.ai[1] += 1f;
                        }
                    }
                    else if (npc.ai[0] >= -30f)
                    {
                        npc.aiAction = 1;
                    }
                }
            }
            else if (npc.target < 255)
            {
                if ((npc.direction == 1 && npc.velocity.X < 3f) || (npc.direction == -1 && npc.velocity.X > -3f))
                {
                    if ((npc.direction == -1 && (double)npc.velocity.X < 0.1) || (npc.direction == 1 && (double)npc.velocity.X > -0.1))
                    {
                        if (npc.ai[1] != 4 && npc.ai[1] != 7)
                        {
                            npc.velocity.X = npc.velocity.X + 0.2f * (float)npc.direction;
                        }
                        //dust
                        int dust2 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 60, npc.height, DustType<BlightDust>(), 0, 0, 150, default(Color), 2f);
                        if (npc.life > 0)
                        {
                            float num11 = (float)npc.life / (float)npc.lifeMax;
                            num11 = num11 * 0.5f + 1.25f;
                            num11 *= num1;
                            if (num11 != npc.scale)
                            {
                                npc.position.X = npc.position.X + (float)(npc.width / 2);
                                npc.position.Y = npc.position.Y + (float)npc.height;
                                npc.scale = num11;
                                npc.width = (int)(98f * npc.scale);
                                npc.height = (int)(92f * npc.scale);
                                npc.position.X = npc.position.X - (float)(npc.width / 2);
                                npc.position.Y = npc.position.Y - (float)npc.height;
                            }
                        }
                    }
                    npc.velocity.X = npc.velocity.X * 0.93f;
                    //Dust
                    int dust = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, DustType<BlightDust>(), 0, 0, 150, default(Color), 2f);
                    if (npc.life > 0)
                    {
                        float num11 = (float)npc.life / (float)npc.lifeMax;
                        num11 = num11 * 0.5f + 1.25f;
                        num11 *= num1;
                        if (num11 != npc.scale)
                        {
                            npc.position.X = npc.position.X + (float)(npc.width / 2);
                            npc.position.Y = npc.position.Y + (float)npc.height;
                            npc.scale = num11;
                            npc.width = (int)(98f * npc.scale);
                            npc.height = (int)(92f * npc.scale);
                            npc.position.X = npc.position.X - (float)(npc.width / 2);
                            npc.position.Y = npc.position.Y - (float)npc.height;
                        }
                    }
                }
            }
            int dust4 = Dust.NewDust(npc.position, npc.width, npc.height, DustType<BlightDust>(), npc.velocity.X, npc.velocity.Y, 255, default(Color), npc.scale * 1.2f);
            Main.dust[dust4].noGravity = true;
            Dust dust46 = Main.dust[dust4];
            dust46.velocity *= 0.5f;
            if (npc.life > 0)
            {
                float num11 = (float)npc.life / (float)npc.lifeMax;
                num11 = num11 * 0.5f + 1.25f;
                num11 *= num1;
                if (num1 != npc.scale)
                {
                    npc.position.X = npc.position.X + (float)(npc.width / 2);
                    npc.position.Y = npc.position.Y + (float)npc.height;
                    npc.scale = num11;
                    npc.width = (int)(98f * npc.scale);
                    npc.height = (int)(92f * npc.scale);
                    npc.position.X = npc.position.X - (float)(npc.width / 2);
                    npc.position.Y = npc.position.Y - (float)npc.height;
                }
            }
            if (npc.ai[1] == 5 || npc.ai[1] == 0)
            {
                npc.velocity.X = 0f;
            }
            if (flag6 && npc.Bottom.Y > Main.player[npc.target].position.Y - 700 && timer < 600)
            {
                npc.alpha += 2;
                npc.velocity.Y = -7f;
                timer++;
                npc.noTileCollide = true;
            }
            else if (flag6)
            {
                flag6 = false;
                npc.position.X = Main.player[npc.target].position.X - npc.width * 0.5f;
                npc.velocity.Y = 15f;
                npc.netUpdate = true;
            }
            if (npc.Bottom.Y > Main.player[npc.target].Center.Y) //Make tilecollide true again if jump into air is done
                npc.noTileCollide = false;
            if (npc.alpha > 30)
            {
                npc.alpha -= 2;
            }
            if (!Main.player[npc.target].dead && npc.ai[2] >= 500f && npc.ai[1] < 8f && npc.velocity.Y == 0f) // teleport when player far
            {
                npc.ai[2] = 0f;
                npc.ai[0] = 0f;
                npc.ai[1] = 8f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.TargetClosest(false);
                    Point pointNPC = npc.Center.ToTileCoordinates();
                    Point pointPlayer = Main.player[npc.target].Center.ToTileCoordinates();
                    Vector2 vector2 = Main.player[npc.target].Center - npc.Center;
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
                        if ((num8 < pointPlayer.Y - num4 || num8 > pointPlayer.Y + num4 || num7 < pointPlayer.X - num4 || num7 > pointPlayer.X + num4) && (num8 < pointNPC.Y - num3 || num8 > pointNPC.Y + num3 || num7 < pointNPC.X - num3 || num7 > pointNPC.X + num3) && !Main.tile[num7, num8].nactive())
                        {
                            int num9 = num8;
                            int num10 = 0; // Shifts teleport y position if the chosen tile is active
                            if (Main.tile[num7, num9].nactive() && Main.tileSolid[Main.tile[num7, num9].type] /*&& !Main.tileSolidTop[Main.tile[num7, num9].type]*/)
                            {
                                num10 = 1;
                            }
                            else
                            {
                                while (num10 < 150 && num9 + num10 < Main.maxTilesY)
                                {
                                    int num3247 = num9 + num10;
                                    if (Main.tile[num7, num3247].nactive() && Main.tileSolid[Main.tile[num7, num3247].type] /*&& !Main.tileSolidTop[Main.tile[num7, num3247].type]*/)
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
                            if (flag5 && Main.tile[num7, num8].lava())
                            {
                                flag5 = false;
                            }
                            if (flag5 && !Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
                            {
                                flag5 = false;
                            }
                            if (flag5)
                            {
                                npc.localAI[1] = (float)(num7 * 16 + 8);
                                npc.localAI[2] = (float)(num8 * 16 + 16);
                                break;
                            }
                        }
                    }
                    if (num5 >= 100)
                    {
                        Vector2 bottom = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].Bottom;
                        npc.localAI[1] = bottom.X;
                        npc.localAI[2] = bottom.Y;
                    }
                }
            }
            if (!Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0)) // prevents collision teleports
            {
                ref float reference = ref npc.ai[2];
                reference += 1f;
            }
            if (Math.Abs(npc.Top.Y - Main.player[npc.target].Bottom.Y) > 500f) // prevents teleports that are too far
            {
                ref float reference = ref npc.ai[2];
                reference += 1f;
            }
        }

        public override void FindFrame(int num)
        {
            int num12 = 0;
            if (npc.aiAction == 0)
            {
                num12 = ((!(npc.velocity.Y < 0f)) ? ((!(npc.velocity.Y > 0f)) ? ((npc.velocity.X != 0f) ? 1 : 0) : 3) : 2);
            }
            else if (npc.aiAction == 1)
            {
                num12 = 4;
            }
            if (npc.velocity.Y != 0f)
            {
                if (npc.frame.Y < num * 4)
                {
                    npc.frame.Y = num * 4;
                    npc.frameCounter = 0.0;
                }
                if ((npc.frameCounter += 1.0) >= 4.0)
                {
                    npc.frame.Y = num * 5;
                }
            }
            else
            {
                if (npc.frame.Y >= num * 5)
                {
                    npc.frame.Y = num * 4;
                    npc.frameCounter = 0.0;
                }
                npc.frameCounter += 1.0;
                if (num12 > 0)
                {
                    npc.frameCounter += 1.0;
                }
                if (num12 == 4)
                {
                    npc.frameCounter += 1.0;
                }
                if (npc.frameCounter >= 8.0)
                {
                    npc.frame.Y = npc.frame.Y + num;
                    npc.frameCounter = 0.0;
                    if (npc.frame.Y >= num * 4)
                    {
                        npc.frame.Y = 0;
                    }
                }
            }
        }

        public override bool PreNPCLoot()
        {
            //Update
            if (!ExoriumWorld.downedBlightslime)
            {
                ExoriumWorld.downedBlightslime = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                }
            }
            //dust
            for (int i = 0; i < 150; i++)
            {
                Vector2 dustPos = new Vector2(0, Main.rand.NextFloat(28));
                Vector2 perturbedDustPos = dustPos.RotatedBy(MathHelper.ToRadians(Main.rand.Next(0, 361)));
                int deathDust = Dust.NewDust(npc.Center + perturbedDustPos, 0, 0, DustType<BlightDust>(), 0, 0, 0, default, Main.rand.NextFloat(1, 4));
                Main.dust[deathDust].velocity = (Main.dust[deathDust].position - npc.Center);
            }

            return true;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "The Blighted Slime";
            potionType = ItemID.LesserHealingPotion;
        }

        public override void NPCLoot()
        {
            //Loot
            if (Main.expertMode)
            {
                npc.DropBossBags();
                return;
            }
            else
            {
                Item.NewItem(npc.getRect(), ItemType<Items.Materials.TaintedGel>(), Main.rand.Next(50, 66));
                Item.NewItem(npc.getRect(), ItemType<Items.Materials.Metals.BlightedOre>(), Main.rand.Next(80, 121));
            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return true;
        }
    }
}
