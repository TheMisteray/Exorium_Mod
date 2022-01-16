using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Microsoft.Xna.Framework;

namespace ExoriumMod.Content.NPCs.Enemies
{
    class WightArcher : ModNPC
    {
        public override string Texture => AssetDirectory.EnemyNPC + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wight Archer");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.SkeletonArcher];
        }

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.SkeletonArcher);
            npc.damage = 17;
            npc.defense = 3;
            npc.lifeMax = 60;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.value = 160f;
            npc.knockBackResist = 0.55f;
            npc.buffImmune[BuffID.Confused] = false;
            npc.aiStyle = -1;
            animationType = NPCID.SkeletonArcher;
        }

        public override void AI()
        {
            int damage = npc.damage / (Main.expertMode == true ? 4 : 2);
            Vector2 vector41;
            if (npc.type == 466)
            {
                int num311 = 200;
                if (npc.ai[2] == 0f)
                {
                    npc.alpha = num311;
                    npc.TargetClosest(true);
                    if (!Main.player[npc.target].dead)
                    {
                        vector41 = Main.player[npc.target].Center - npc.Center;
                        if (vector41.Length() < 170f)
                        {
                            npc.ai[2] = -16f;
                        }
                    }
                    if (npc.velocity.X == 0f && !(npc.velocity.Y < 0f) && !(npc.velocity.Y > 2f) && !npc.justHit)
                    {
                        return;
                    }
                    npc.ai[2] = -16f;
                    return;
                }
                if (npc.ai[2] < 0f)
                {
                    if (npc.alpha > 0)
                    {
                        npc.alpha -= num311 / 16;
                        if (npc.alpha < 0)
                        {
                            npc.alpha = 0;
                        }
                    }
                    npc.ai[2] += 1f;
                    if (npc.ai[2] == 0f)
                    {
                        npc.ai[2] = 1f;
                        npc.velocity.X = (float)(npc.direction * 2);
                    }
                    return;
                }
                npc.alpha = 0;
            }
            if (npc.type == 166)
            {
                if (Main.netMode != 1 && Main.rand.Next(240) == 0)
                {
                    npc.ai[2] = (float)Main.rand.Next(-480, -60);
                    npc.netUpdate = true;
                }
                if (npc.ai[2] < 0f)
                {
                    npc.TargetClosest(true);
                    if (npc.justHit)
                    {
                        npc.ai[2] = 0f;
                    }
                    if (Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
                    {
                        npc.ai[2] = 0f;
                    }
                }
                if (npc.ai[2] < 0f)
                {
                    npc.velocity.X = npc.velocity.X * 0.9f;
                    if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
                    {
                        npc.velocity.X = 0f;
                    }
                    npc.ai[2] += 1f;
                    if (npc.ai[2] == 0f)
                    {
                        npc.velocity.X = (float)npc.direction * 0.1f;
                    }
                    return;
                }
            }
            if (npc.type == 461)
            {
                if (npc.wet)
                {
                    npc.knockBackResist = 0f;
                    npc.ai[3] = -0.10101f;
                    npc.noGravity = true;
                    Vector2 center4 = npc.Center;
                    npc.width = 34;
                    npc.height = 24;
                    npc.position.X = center4.X - (float)(npc.width / 2);
                    npc.position.Y = center4.Y - (float)(npc.height / 2);
                    npc.TargetClosest(true);
                    if (npc.collideX)
                    {
                        npc.velocity.X = 0f - npc.oldVelocity.X;
                    }
                    if (npc.velocity.X < 0f)
                    {
                        npc.direction = -1;
                    }
                    if (npc.velocity.X > 0f)
                    {
                        npc.direction = 1;
                    }
                    if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].Center, 1, 1))
                    {
                        Vector2 value3 = Main.player[npc.target].Center - npc.Center;
                        value3.Normalize();
                        value3 *= 5f;
                        npc.velocity = (npc.velocity * 19f + value3) / 20f;
                    }
                    else
                    {
                        float num374 = 5f;
                        if (npc.velocity.Y > 0f)
                        {
                            num374 = 3f;
                        }
                        if (npc.velocity.Y < 0f)
                        {
                            num374 = 8f;
                        }
                        Vector2 value2 = new Vector2((float)npc.direction, -1f);
                        value2.Normalize();
                        value2 *= num374;
                        if (num374 < 5f)
                        {
                            npc.velocity = (npc.velocity * 24f + value2) / 25f;
                        }
                        else
                        {
                            npc.velocity = (npc.velocity * 9f + value2) / 10f;
                        }
                    }
                    return;
                }
                npc.knockBackResist = 0.4f * Main.knockBackMultiplier;
                npc.noGravity = false;
                Vector2 center5 = npc.Center;
                npc.width = 18;
                npc.height = 40;
                npc.position.X = center5.X - (float)(npc.width / 2);
                npc.position.Y = center5.Y - (float)(npc.height / 2);
                if (npc.ai[3] == -0.10101f)
                {
                    npc.ai[3] = 0f;
                    float num383 = npc.velocity.Length();
                    num383 *= 2f;
                    if (num383 > 10f)
                    {
                        num383 = 10f;
                    }
                    npc.velocity.Normalize();
                    npc.velocity *= num383;
                    if (npc.velocity.X < 0f)
                    {
                        npc.direction = -1;
                    }
                    if (npc.velocity.X > 0f)
                    {
                        npc.direction = 1;
                    }
                    npc.spriteDirection = npc.direction;
                }
            }
            if (npc.type == 379 || npc.type == 380)
            {
                if (npc.ai[3] < 0f)
                {
                    npc.damage = 0;
                    npc.velocity.X = npc.velocity.X * 0.93f;
                    if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
                    {
                        npc.velocity.X = 0f;
                    }
                    int num387 = (int)(0f - npc.ai[3] - 1f);
                    int num395 = Math.Sign(Main.npc[num387].Center.X - npc.Center.X);
                    if (num395 != npc.direction)
                    {
                        npc.velocity.X = 0f;
                        npc.direction = num395;
                        npc.netUpdate = true;
                    }
                    if (npc.justHit && Main.netMode != 1 && Main.npc[num387].localAI[0] == 0f)
                    {
                        Main.npc[num387].localAI[0] = 1f;
                    }
                    if (npc.ai[0] < 1000f)
                    {
                        npc.ai[0] = 1000f;
                    }
                    if ((npc.ai[0] += 1f) >= 1300f)
                    {
                        npc.ai[0] = 1000f;
                        npc.netUpdate = true;
                    }
                    return;
                }
                if (npc.ai[0] >= 1000f)
                {
                    npc.ai[0] = 0f;
                }
                npc.damage = npc.defDamage;
            }
            if (npc.type == 383 && npc.ai[2] == 0f && npc.localAI[0] == 0f && Main.netMode != 1)
            {
                int num403 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, 384, npc.whoAmI, 0f, 0f, 0f, 0f, 255);
                npc.ai[2] = (float)(num403 + 1);
                npc.localAI[0] = -1f;
                npc.netUpdate = true;
                Main.npc[num403].ai[0] = (float)npc.whoAmI;
                Main.npc[num403].netUpdate = true;
            }
            if (npc.type == 383)
            {
                int num415 = (int)npc.ai[2] - 1;
                if (num415 != -1 && Main.npc[num415].active && Main.npc[num415].type == 384)
                {
                    npc.dontTakeDamage = true;
                }
                else
                {
                    npc.dontTakeDamage = false;
                    npc.ai[2] = 0f;
                    if (npc.localAI[0] == -1f)
                    {
                        npc.localAI[0] = 180f;
                    }
                    if (npc.localAI[0] > 0f)
                    {
                        npc.localAI[0] -= 1f;
                    }
                }
            }
            if (npc.type == 482)
            {
                int num414 = 300;
                int num413 = 120;
                npc.dontTakeDamage = false;
                if (npc.ai[2] < 0f)
                {
                    npc.dontTakeDamage = true;
                    npc.ai[2] += 1f;
                    npc.velocity.X = npc.velocity.X * 0.9f;
                    if ((double)Math.Abs(npc.velocity.X) < 0.001)
                    {
                        npc.velocity.X = 0.001f * (float)npc.direction;
                    }
                    if (Math.Abs(npc.velocity.Y) > 1f)
                    {
                        npc.ai[2] += 10f;
                    }
                    if (npc.ai[2] >= 0f)
                    {
                        npc.netUpdate = true;
                        npc.velocity.X = npc.velocity.X + (float)npc.direction * 0.3f;
                    }
                    return;
                }
                if (npc.ai[2] < (float)num414)
                {
                    if (npc.justHit)
                    {
                        npc.ai[2] += 15f;
                    }
                    npc.ai[2] += 1f;
                }
                else if (npc.velocity.Y == 0f)
                {
                    npc.ai[2] = 0f - (float)num413;
                    npc.netUpdate = true;
                }
            }
            int num410;
            int num408;
            if (npc.type == 480)
            {
                int num412 = 180;
                int num411 = 300;
                num410 = 180;
                int num409 = 60;
                num408 = 20;
                if (npc.life < npc.lifeMax / 3)
                {
                    num412 = 120;
                    num411 = 240;
                    num410 = 240;
                    num409 = 90;
                }
                if (npc.ai[2] > 0f)
                {
                    npc.ai[2] -= 1f;
                }
                else if (npc.ai[2] == 0f)
                {
                    if (Main.player[npc.target].Center.X < npc.Center.X && npc.direction < 0)
                    {
                        goto IL_0b35;
                    }
                    if (Main.player[npc.target].Center.X > npc.Center.X && npc.direction > 0)
                    {
                        goto IL_0b35;
                    }
                }
                else
                {
                    if (npc.ai[2] < 0f && npc.ai[2] < 0f - (float)num410)
                    {
                        npc.velocity.X = npc.velocity.X * 0.9f;
                        if (npc.velocity.Y < -2f || npc.velocity.Y > 4f || npc.justHit)
                        {
                            npc.ai[2] = (float)num412;
                        }
                        else
                        {
                            npc.ai[2] += 1f;
                            if (npc.ai[2] == 0f)
                            {
                                npc.ai[2] = (float)num411;
                            }
                        }
                        float num407 = npc.ai[2] + (float)num410 + (float)num408;
                        if (num407 == 1f)
                        {
                            Main.PlaySound(4, (int)npc.position.X, (int)npc.position.Y, 17, 1f, 0f);
                        }
                        if (num407 < (float)num408)
                        {
                            Vector2 vector40 = npc.Top + new Vector2((float)(npc.spriteDirection * 6), 6f);
                            float scaleFactor11 = MathHelper.Lerp(20f, 30f, (num407 * 3f + 50f) / 182f);
                            Main.rand.NextFloat();
                            for (float num406 = 0f; num406 < 2f; num406 += 1f)
                            {
                                Vector2 vector39 = Vector2.UnitY.RotatedByRandom(6.2831854820251465) * (Main.rand.NextFloat() * 0.5f + 0.5f);
                                Dust obj = Main.dust[Dust.NewDust(vector40, 0, 0, 228, 0f, 0f, 0, default(Color), 1f)];
                                obj.position = vector40 + vector39 * scaleFactor11;
                                obj.noGravity = true;
                                obj.velocity = vector39 * 2f;
                                obj.scale = 0.5f + Main.rand.NextFloat() * 0.5f;
                            }
                        }
                        Lighting.AddLight(npc.Center, 0.9f, 0.75f, 0.1f);
                        return;
                    }
                    if (npc.ai[2] < 0f && npc.ai[2] >= 0f - (float)num410)
                    {
                        Lighting.AddLight(npc.Center, 0.9f, 0.75f, 0.1f);
                        npc.velocity.X = npc.velocity.X * 0.9f;
                        if (npc.velocity.Y < -2f || npc.velocity.Y > 4f || npc.justHit)
                        {
                            npc.ai[2] = (float)num412;
                        }
                        else
                        {
                            npc.ai[2] += 1f;
                            if (npc.ai[2] == 0f)
                            {
                                npc.ai[2] = (float)num411;
                            }
                        }
                        float num405 = npc.ai[2] + (float)num410;
                        if (num405 < 180f && (Main.rand.Next(3) == 0 || npc.ai[2] % 3f == 0f))
                        {
                            Vector2 vector38 = npc.Top + new Vector2((float)(npc.spriteDirection * 10), 10f);
                            float scaleFactor10 = MathHelper.Lerp(20f, 30f, (num405 * 3f + 50f) / 182f);
                            Main.rand.NextFloat();
                            for (float num404 = 0f; num404 < 1f; num404 += 1f)
                            {
                                Vector2 vector37 = Vector2.UnitY.RotatedByRandom(6.2831854820251465) * (Main.rand.NextFloat() * 0.5f + 0.5f);
                                Dust obj2 = Main.dust[Dust.NewDust(vector38, 0, 0, 228, 0f, 0f, 0, default(Color), 1f)];
                                obj2.position = vector38 + vector37 * scaleFactor10;
                                obj2.noGravity = true;
                                obj2.velocity = vector37 * 4f;
                                obj2.scale = 0.5f + Main.rand.NextFloat();
                            }
                        }
                        if (Main.netMode != 2)
                        {
                            Player player = Main.player[Main.myPlayer];
                            int myPlayer = Main.myPlayer;
                            if (!player.dead && player.active && player.FindBuffIndex(156) == -1)
                            {
                                Vector2 vector22 = player.Center - npc.Center;
                                if (vector22.Length() < 700f)
                                {
                                    bool flag54 = vector22.Length() < 30f;
                                    if (!flag54)
                                    {
                                        float x = 0.7853982f.ToRotationVector2().X;
                                        Vector2 vector36 = Vector2.Normalize(vector22);
                                        if (vector36.X > x || vector36.X < 0f - x)
                                        {
                                            flag54 = true;
                                        }
                                    }
                                    if ((((player.Center.X < npc.Center.X && npc.direction < 0 && player.direction > 0) || (player.Center.X > npc.Center.X && npc.direction > 0 && player.direction < 0)) & flag54) && (Collision.CanHitLine(npc.Center, 1, 1, player.Center, 1, 1) || Collision.CanHitLine(npc.Center - Vector2.UnitY * 16f, 1, 1, player.Center, 1, 1) || Collision.CanHitLine(npc.Center + Vector2.UnitY * 8f, 1, 1, player.Center, 1, 1)))
                                    {
                                        player.AddBuff(156, num409 + (int)npc.ai[2] * -1, true);
                                    }
                                }
                            }
                        }
                        return;
                    }
                }
            }
            goto IL_119a;
        IL_119a:
            if (npc.type == 471)
            {
                if (npc.ai[3] < 0f)
                {
                    npc.knockBackResist = 0f;
                    npc.defense = (int)((double)npc.defDefense * 1.1);
                    npc.noGravity = true;
                    npc.noTileCollide = true;
                    if (npc.velocity.X < 0f)
                    {
                        npc.direction = -1;
                    }
                    else if (npc.velocity.X > 0f)
                    {
                        npc.direction = 1;
                    }
                    npc.rotation = npc.velocity.X * 0.1f;
                    if (Main.netMode != 1)
                    {
                        npc.localAI[3] += 1f;
                        if (npc.localAI[3] > (float)Main.rand.Next(20, 180))
                        {
                            npc.localAI[3] = 0f;
                            Vector2 value11 = npc.Center;
                            value11 += npc.velocity;
                            NPC.NewNPC((int)value11.X, (int)value11.Y, 30, 0, 0f, 0f, 0f, 0f, 255);
                        }
                    }
                }
                else
                {
                    npc.localAI[3] = 0f;
                    npc.knockBackResist = 0.35f * Main.knockBackMultiplier;
                    npc.rotation *= 0.9f;
                    npc.defense = npc.defDefense;
                    npc.noGravity = false;
                    npc.noTileCollide = false;
                }
                if (npc.ai[3] == 1f)
                {
                    npc.knockBackResist = 0f;
                    npc.defense += 10;
                }
                if (npc.ai[3] == -1f)
                {
                    npc.TargetClosest(true);
                    float num402 = 8f;
                    float num401 = 40f;
                    Vector2 value4 = Main.player[npc.target].Center - npc.Center;
                    float num400 = value4.Length();
                    num402 += num400 / 200f;
                    value4.Normalize();
                    value4 *= num402;
                    npc.velocity = (npc.velocity * (num401 - 1f) + value4) / num401;
                    if (num400 < 500f && !Collision.SolidCollision(npc.position, npc.width, npc.height))
                    {
                        npc.ai[3] = 0f;
                        npc.ai[2] = 0f;
                    }
                    return;
                }
                if (npc.ai[3] == -2f)
                {
                    npc.velocity.Y = npc.velocity.Y - 0.2f;
                    if (npc.velocity.Y < -10f)
                    {
                        npc.velocity.Y = -10f;
                    }
                    if (Main.player[npc.target].Center.Y - npc.Center.Y > 200f)
                    {
                        npc.TargetClosest(true);
                        npc.ai[3] = -3f;
                        if (Main.player[npc.target].Center.X > npc.Center.X)
                        {
                            npc.ai[2] = 1f;
                        }
                        else
                        {
                            npc.ai[2] = -1f;
                        }
                    }
                    npc.velocity.X = npc.velocity.X * 0.99f;
                    return;
                }
                if (npc.ai[3] == -3f)
                {
                    if (npc.direction == 0)
                    {
                        npc.TargetClosest(true);
                    }
                    if (npc.ai[2] == 0f)
                    {
                        npc.ai[2] = (float)npc.direction;
                    }
                    npc.velocity.Y = npc.velocity.Y * 0.9f;
                    npc.velocity.X = npc.velocity.X + npc.ai[2] * 0.3f;
                    if (npc.velocity.X > 10f)
                    {
                        npc.velocity.X = 10f;
                    }
                    if (npc.velocity.X < -10f)
                    {
                        npc.velocity.X = -10f;
                    }
                    float num398 = Main.player[npc.target].Center.X - npc.Center.X;
                    if (npc.ai[2] < 0f && num398 > 300f)
                    {
                        goto IL_1614;
                    }
                    if (npc.ai[2] > 0f && num398 < -300f)
                    {
                        goto IL_1614;
                    }
                    if (Math.Abs(num398) > 800f)
                    {
                        npc.ai[3] = -1f;
                        npc.ai[2] = 0f;
                    }
                    return;
                }
                if (npc.ai[3] == -4f)
                {
                    npc.ai[2] += 1f;
                    npc.velocity.Y = npc.velocity.Y + 0.1f;
                    if (npc.velocity.Length() > 4f)
                    {
                        npc.velocity *= 0.9f;
                    }
                    int num397 = (int)npc.Center.X / 16;
                    int num396 = (int)(npc.position.Y + (float)npc.height + 12f) / 16;
                    bool flag53 = false;
                    for (int n = num397 - 1; n <= num397 + 1; n++)
                    {
                        if (Main.tile[n, num396] == null)
                        {
                            Tile[,] tile = Main.tile;
                            int num416 = num397;
                            int num417 = num396;
                            Tile tile2 = new Tile();
                            tile[num416, num417] = tile2;
                        }
                        if (Main.tile[n, num396].active() && Main.tileSolid[Main.tile[n, num396].type])
                        {
                            flag53 = true;
                        }
                    }
                    if (flag53 && !Collision.SolidCollision(npc.position, npc.width, npc.height))
                    {
                        npc.ai[3] = 0f;
                        npc.ai[2] = 0f;
                    }
                    else if (npc.ai[2] > 300f || npc.Center.Y > Main.player[npc.target].Center.Y + 200f)
                    {
                        npc.ai[3] = -1f;
                        npc.ai[2] = 0f;
                    }
                }
                else
                {
                    if (npc.ai[3] == 1f)
                    {
                        Vector2 center3 = npc.Center;
                        center3.Y -= 70f;
                        npc.velocity.X = npc.velocity.X * 0.8f;
                        npc.ai[2] += 1f;
                        if (npc.ai[2] == 60f)
                        {
                            if (Main.netMode != 1)
                            {
                                NPC.NewNPC((int)center3.X, (int)center3.Y + 18, 472, 0, 0f, 0f, 0f, 0f, 255);
                            }
                        }
                        else if (npc.ai[2] >= 90f)
                        {
                            npc.ai[3] = -2f;
                            npc.ai[2] = 0f;
                        }
                        for (int m = 0; m < 2; m++)
                        {
                            Vector2 value12 = center3;
                            Vector2 value5 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                            value5.Normalize();
                            value5 *= (float)Main.rand.Next(0, 100) * 0.1f;
                            Vector2 position3 = value12 + value5;
                            value5.Normalize();
                            value5 *= (float)Main.rand.Next(50, 90) * 0.1f;
                            int num394 = Dust.NewDust(position3, 1, 1, 27, 0f, 0f, 0, default(Color), 1f);
                            Main.dust[num394].velocity = -value5 * 0.3f;
                            Main.dust[num394].alpha = 100;
                            if (Main.rand.Next(2) == 0)
                            {
                                Main.dust[num394].noGravity = true;
                                Main.dust[num394].scale += 0.3f;
                            }
                        }
                        return;
                    }
                    npc.ai[2] += 1f;
                    int num393 = 10;
                    if (npc.velocity.Y == 0f && NPC.CountNPCS(472) < num393)
                    {
                        if (npc.ai[2] >= 180f)
                        {
                            npc.ai[2] = 0f;
                            npc.ai[3] = 1f;
                        }
                    }
                    else
                    {
                        if (NPC.CountNPCS(472) >= num393)
                        {
                            npc.ai[2] += 1f;
                        }
                        if (npc.ai[2] >= 360f)
                        {
                            npc.ai[2] = 0f;
                            npc.ai[3] = -2f;
                            npc.velocity.Y = npc.velocity.Y - 3f;
                        }
                    }
                    if (npc.target >= 0 && !Main.player[npc.target].dead)
                    {
                        vector41 = Main.player[npc.target].Center - npc.Center;
                        if (vector41.Length() > 800f)
                        {
                            npc.ai[3] = -1f;
                            npc.ai[2] = 0f;
                        }
                    }
                }
                if (Main.player[npc.target].dead)
                {
                    npc.TargetClosest(true);
                    if (Main.player[npc.target].dead && npc.timeLeft > 1)
                    {
                        npc.timeLeft = 1;
                    }
                }
            }
            if (npc.type == 419)
            {
                npc.reflectingProjectiles = false;
                npc.takenDamageMultiplier = 1f;
                int num392 = 6;
                int num391 = 10;
                float scaleFactor9 = 16f;
                if (npc.ai[2] > 0f)
                {
                    npc.ai[2] -= 1f;
                }
                if (npc.ai[2] == 0f)
                {
                    if (Main.player[npc.target].Center.X < npc.Center.X && npc.direction < 0)
                    {
                        goto IL_1c13;
                    }
                    if (Main.player[npc.target].Center.X > npc.Center.X && npc.direction > 0)
                    {
                        goto IL_1c13;
                    }
                }
                else
                {
                    if (npc.ai[2] < 0f && npc.ai[2] > 0f - (float)num392)
                    {
                        npc.ai[2] -= 1f;
                        npc.velocity.X = npc.velocity.X * 0.9f;
                        return;
                    }
                    if (npc.ai[2] == 0f - (float)num392)
                    {
                        npc.ai[2] -= 1f;
                        npc.TargetClosest(true);
                        Vector2 vec = npc.DirectionTo(Main.player[npc.target].Top + new Vector2(0f, -30f));
                        if (vec.HasNaNs())
                        {
                            vec = Vector2.Normalize(new Vector2((float)npc.spriteDirection, -1f));
                        }
                        npc.velocity = vec * scaleFactor9;
                        npc.netUpdate = true;
                        return;
                    }
                    if (npc.ai[2] < 0f - (float)num392)
                    {
                        npc.ai[2] -= 1f;
                        if (npc.velocity.Y == 0f)
                        {
                            npc.ai[2] = 60f;
                        }
                        else if (npc.ai[2] < 0f - (float)num392 - (float)num391)
                        {
                            npc.velocity.Y = npc.velocity.Y + 0.15f;
                            if (npc.velocity.Y > 24f)
                            {
                                npc.velocity.Y = 24f;
                            }
                        }
                        npc.reflectingProjectiles = true;
                        npc.takenDamageMultiplier = 3f;
                        if (npc.justHit)
                        {
                            npc.ai[2] = 60f;
                            npc.netUpdate = true;
                        }
                        return;
                    }
                }
            }
            goto IL_1e03;
        IL_2d75:
            bool flag49;
            bool flag48;
            if (!flag49 & flag48)
            {
                if (npc.velocity.Y == 0f)
                {
                    if (npc.velocity.X > 0f && npc.direction < 0)
                    {
                        goto IL_2dca;
                    }
                    if (npc.velocity.X < 0f && npc.direction > 0)
                    {
                        goto IL_2dca;
                    }
                }
                goto IL_2dcc;
            }
            goto IL_2e95;
        IL_1614:
            npc.ai[3] = -4f;
            npc.ai[2] = 0f;
            return;
        IL_8f61:
            if (npc.type == 386)
            {
                if (npc.confused)
                {
                    npc.ai[2] = -60f;
                }
                else
                {
                    if (npc.ai[2] < 60f)
                    {
                        npc.ai[2] += 1f;
                    }
                    if (npc.ai[2] > 0f && NPC.CountNPCS(387) >= 4 * NPC.CountNPCS(386))
                    {
                        npc.ai[2] = 0f;
                    }
                    if (npc.justHit)
                    {
                        npc.ai[2] = -30f;
                    }
                    if (npc.ai[2] == 30f)
                    {
                        int num263 = (int)npc.position.X / 16;
                        int num262 = (int)npc.position.Y / 16;
                        int num261 = (int)npc.position.X / 16;
                        int num260 = (int)npc.position.Y / 16;
                        int num259 = 5;
                        int num258 = 0;
                        bool flag41 = false;
                        int num257 = 2;
                        int num256 = 0;
                        while (!flag41 && num258 < 100)
                        {
                            num258++;
                            int num255 = Main.rand.Next(num263 - num259, num263 + num259);
                            for (int num254 = Main.rand.Next(num262 - num259, num262 + num259); num254 < num262 + num259; num254++)
                            {
                                if ((num254 < num262 - num257 || num254 > num262 + num257 || num255 < num263 - num257 || num255 > num263 + num257) && (num254 < num260 - num256 || num254 > num260 + num256 || num255 < num261 - num256 || num255 > num261 + num256) && Main.tile[num255, num254].nactive())
                                {
                                    bool flag40 = true;
                                    if (Main.tile[num255, num254 - 1].lava())
                                    {
                                        flag40 = false;
                                    }
                                    if (flag40 && Main.tileSolid[Main.tile[num255, num254].type] && !Collision.SolidTiles(num255 - 1, num255 + 1, num254 - 4, num254 - 1))
                                    {
                                        int num253 = NPC.NewNPC(num255 * 16 - npc.width / 2, num254 * 16, 387, 0, 0f, 0f, 0f, 0f, 255);
                                        Main.npc[num253].position.Y = (float)(num254 * 16 - Main.npc[num253].height);
                                        flag41 = true;
                                        npc.netUpdate = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (npc.ai[2] == 60f)
                    {
                        npc.ai[2] = -120f;
                    }
                }
            }
            if (npc.type == 389)
            {
                if (npc.confused)
                {
                    npc.ai[2] = -60f;
                }
                else
                {
                    if (npc.ai[2] < 20f)
                    {
                        npc.ai[2] += 1f;
                    }
                    if (npc.justHit)
                    {
                        npc.ai[2] = -30f;
                    }
                    if (npc.ai[2] == 20f && Main.netMode != 1)
                    {
                        npc.ai[2] = (float)(-10 + Main.rand.Next(3) * -10);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 8f, (float)(npc.direction * 6), 0f, ProjectileID.WoodenArrowHostile, damage, 1f, Main.myPlayer, 0f, 0f);
                    }
                }
            }
            if (false && npc.type != 111 && npc.type != 206 && npc.type != 214 && npc.type != 215 && npc.type != 216 && npc.type != 290 && npc.type != 291 && npc.type != 292 && npc.type != 293 && npc.type != 350 && npc.type != 379 && npc.type != 380 && npc.type != 381 && npc.type != 382 && (npc.type < 449 || npc.type > 452) && npc.type != 468 && npc.type != 481 && npc.type != 411 && npc.type != 409 && (npc.type < 498 || npc.type > 506) && npc.type != 424 && npc.type != 426 && npc.type != 520)
            {
                goto IL_aba2;
            }
            bool flag39 = npc.type == 381 || npc.type == 382 || npc.type == 520;
            bool flag38 = npc.type == 426;
            bool flag37 = true;
            int num252 = -1;
            int num251 = -1;
            if (npc.type == 411)
            {
                flag39 = true;
                num252 = 90;
                num251 = 90;
                if (npc.ai[1] <= 150f)
                {
                    flag37 = false;
                }
            }
            if (npc.confused)
            {
                npc.ai[2] = 0f;
                goto IL_aba2;
            }
            if (npc.ai[1] > 0f)
            {
                npc.ai[1] -= 1f;
            }
            if (npc.justHit)
            {
                npc.ai[1] = 30f;
                npc.ai[2] = 0f;
            }
            int num250 = 70;
            if (npc.type == 379 || npc.type == 380)
            {
                num250 = 80;
            }
            if (npc.type == 381 || npc.type == 382)
            {
                num250 = 80;
            }
            if (npc.type == 520)
            {
                num250 = 15;
            }
            if (npc.type == 350)
            {
                num250 = 110;
            }
            if (npc.type == 291)
            {
                num250 = 200;
            }
            if (npc.type == 292)
            {
                num250 = 120;
            }
            if (npc.type == 293)
            {
                num250 = 90;
            }
            if (npc.type == 111)
            {
                num250 = 180;
            }
            if (npc.type == 206)
            {
                num250 = 50;
            }
            if (npc.type == 481)
            {
                num250 = 100;
            }
            if (npc.type == 214)
            {
                num250 = 40;
            }
            if (npc.type == 215)
            {
                num250 = 80;
            }
            if (npc.type == 290)
            {
                num250 = 30;
            }
            if (npc.type == 411)
            {
                num250 = 300;
            }
            if (npc.type == 409)
            {
                num250 = 60;
            }
            if (npc.type == 424)
            {
                num250 = 180;
            }
            if (npc.type == 426)
            {
                num250 = 60;
            }
            bool flag36 = false;
            if (npc.type == 216)
            {
                if (npc.localAI[2] >= 20f)
                {
                    flag36 = true;
                }
                num250 = ((!flag36) ? 8 : 60);
            }
            int num249 = num250 / 2;
            if (npc.type == 424)
            {
                num249 = num250 - 1;
            }
            if (npc.type == 426)
            {
                num249 = num250 - 1;
            }
            if (npc.ai[2] > 0f)
            {
                if (flag37)
                {
                    npc.TargetClosest(true);
                }
                if (npc.ai[1] == (float)num249)
                {
                    if (npc.type == 216)
                    {
                        npc.localAI[2] += 1f;
                    }
                    float num248 = 11f;
                    if (npc.type == 111)
                    {
                        num248 = 9f;
                    }
                    if (npc.type == 206)
                    {
                        num248 = 7f;
                    }
                    if (npc.type == 290)
                    {
                        num248 = 9f;
                    }
                    if (npc.type == 293)
                    {
                        num248 = 4f;
                    }
                    if (npc.type == 214)
                    {
                        num248 = 14f;
                    }
                    if (npc.type == 215)
                    {
                        num248 = 16f;
                    }
                    if (npc.type == 382)
                    {
                        num248 = 7f;
                    }
                    if (npc.type == 520)
                    {
                        num248 = 8f;
                    }
                    if (npc.type == 409)
                    {
                        num248 = 4f;
                    }
                    if (npc.type >= 449 && npc.type <= 452)
                    {
                        num248 = 7f;
                    }
                    if (npc.type == 481)
                    {
                        num248 = 8f;
                    }
                    if (npc.type == 468)
                    {
                        num248 = 7.5f;
                    }
                    if (npc.type == 411)
                    {
                        num248 = 1f;
                    }
                    if (npc.type >= 498 && npc.type <= 506)
                    {
                        num248 = 7f;
                    }
                    Vector2 value9 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                    if (npc.type == 481)
                    {
                        value9.Y -= 14f;
                    }
                    if (npc.type == 206)
                    {
                        value9.Y -= 10f;
                    }
                    if (npc.type == 290)
                    {
                        value9.Y -= 10f;
                    }
                    if (npc.type == 381 || npc.type == 382)
                    {
                        value9.Y += 6f;
                    }
                    if (npc.type == 520)
                    {
                        value9.Y = npc.position.Y + 20f;
                    }
                    if (npc.type >= 498 && npc.type <= 506)
                    {
                        value9.Y -= 8f;
                    }
                    if (npc.type == 426)
                    {
                        value9 += new Vector2((float)(npc.spriteDirection * 2), -12f);
                    }
                    float num247 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - value9.X;
                    float num246 = Math.Abs(num247) * 0.1f;
                    if (npc.type == 291 || npc.type == 292)
                    {
                        num246 = 0f;
                    }
                    if (npc.type == 215)
                    {
                        num246 = Math.Abs(num247) * 0.08f;
                    }
                    if (npc.type == 214 || (npc.type == 216 && !flag36))
                    {
                        num246 = 0f;
                    }
                    if (npc.type == 381 || npc.type == 382 || npc.type == 520)
                    {
                        num246 = 0f;
                    }
                    if (npc.type >= 449 && npc.type <= 452)
                    {
                        num246 = Math.Abs(num247) * (float)Main.rand.Next(10, 50) * 0.01f;
                    }
                    if (npc.type == 468)
                    {
                        num246 = Math.Abs(num247) * (float)Main.rand.Next(10, 50) * 0.01f;
                    }
                    if (npc.type == 481)
                    {
                        num246 = Math.Abs(num247) * (float)Main.rand.Next(-10, 11) * 0.0035f;
                    }
                    if (npc.type >= 498 && npc.type <= 506)
                    {
                        num246 = Math.Abs(num247) * (float)Main.rand.Next(1, 11) * 0.0025f;
                    }
                    float num245 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - value9.Y - num246;
                    if (npc.type == 291)
                    {
                        num247 += (float)Main.rand.Next(-40, 41) * 0.2f;
                        num245 += (float)Main.rand.Next(-40, 41) * 0.2f;
                    }
                    else if (npc.type == 381 || npc.type == 382 || npc.type == 520)
                    {
                        num247 += (float)Main.rand.Next(-100, 101) * 0.4f;
                        num245 += (float)Main.rand.Next(-100, 101) * 0.4f;
                        num247 *= (float)Main.rand.Next(85, 116) * 0.01f;
                        num245 *= (float)Main.rand.Next(85, 116) * 0.01f;
                        if (npc.type == 520)
                        {
                            num247 += (float)Main.rand.Next(-100, 101) * 0.6f;
                            num245 += (float)Main.rand.Next(-100, 101) * 0.6f;
                            num247 *= (float)Main.rand.Next(85, 116) * 0.015f;
                            num245 *= (float)Main.rand.Next(85, 116) * 0.015f;
                        }
                    }
                    else if (npc.type == 481)
                    {
                        num247 += (float)Main.rand.Next(-40, 41) * 0.4f;
                        num245 += (float)Main.rand.Next(-40, 41) * 0.4f;
                    }
                    else if (npc.type >= 498 && npc.type <= 506)
                    {
                        num247 += (float)Main.rand.Next(-40, 41) * 0.3f;
                        num245 += (float)Main.rand.Next(-40, 41) * 0.3f;
                    }
                    else if (npc.type != 292)
                    {
                        num247 += (float)Main.rand.Next(-40, 41);
                        num245 += (float)Main.rand.Next(-40, 41);
                    }
                    float num240 = (float)Math.Sqrt((double)(num247 * num247 + num245 * num245));
                    npc.netUpdate = true;
                    num240 = num248 / num240;
                    num247 *= num240;
                    num245 *= num240;
                    int num236 = 35;
                    int num235 = 82;
                    if (npc.type == 111)
                    {
                        num236 = 11;
                    }
                    if (npc.type == 206)
                    {
                        num236 = 37;
                    }
                    if (npc.type == 379 || npc.type == 380)
                    {
                        num236 = 40;
                    }
                    if (npc.type == 350)
                    {
                        num236 = 45;
                    }
                    if (npc.type == 468)
                    {
                        num236 = 50;
                    }
                    if (npc.type == 111)
                    {
                        num235 = 81;
                    }
                    if (npc.type == 379 || npc.type == 380)
                    {
                        num235 = 81;
                    }
                    if (npc.type == 381)
                    {
                        num235 = 436;
                        num236 = 24;
                    }
                    if (npc.type == 382)
                    {
                        num235 = 438;
                        num236 = 30;
                    }
                    if (npc.type == 520)
                    {
                        num235 = 592;
                        num236 = 35;
                    }
                    if (npc.type >= 449 && npc.type <= 452)
                    {
                        num235 = 471;
                        num236 = 20;
                    }
                    if (npc.type >= 498 && npc.type <= 506)
                    {
                        num235 = 572;
                        num236 = 14;
                    }
                    if (npc.type == 481)
                    {
                        num235 = 508;
                        num236 = 18;
                    }
                    if (npc.type == 206)
                    {
                        num235 = 177;
                    }
                    if (npc.type == 468)
                    {
                        num235 = 501;
                    }
                    if (npc.type == 411)
                    {
                        num235 = 537;
                        num236 = (Main.expertMode ? 45 : 60);
                    }
                    if (npc.type == 424)
                    {
                        num235 = 573;
                        num236 = (Main.expertMode ? 45 : 60);
                    }
                    if (npc.type == 426)
                    {
                        num235 = 581;
                        num236 = (Main.expertMode ? 45 : 60);
                    }
                    if (npc.type == 291)
                    {
                        num235 = 302;
                        num236 = 100;
                    }
                    if (npc.type == 290)
                    {
                        num235 = 300;
                        num236 = 60;
                    }
                    if (npc.type == 293)
                    {
                        num235 = 303;
                        num236 = 60;
                    }
                    if (npc.type == 214)
                    {
                        num235 = 180;
                        num236 = 25;
                    }
                    if (npc.type == 215)
                    {
                        num235 = 82;
                        num236 = 40;
                    }
                    if (npc.type == 292)
                    {
                        num236 = 50;
                        num235 = 180;
                    }
                    if (npc.type == 216)
                    {
                        num235 = 180;
                        num236 = 30;
                        if (flag36)
                        {
                            num236 = 100;
                            num235 = 240;
                            npc.localAI[2] = 0f;
                        }
                    }
                    value9.X += num247;
                    value9.Y += num245;
                    if (Main.expertMode && npc.type == 290)
                    {
                        num236 = (int)((double)num236 * 0.75);
                    }
                    if (Main.expertMode && npc.type >= 381 && npc.type <= 392)
                    {
                        num236 = (int)((double)num236 * 0.8);
                    }
                    if (Main.netMode != 1)
                    {
                        if (npc.type == 292)
                        {
                            for (int num234 = 0; num234 < 4; num234++)
                            {
                                num247 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - value9.X;
                                num245 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - value9.Y;
                                num240 = (float)Math.Sqrt((double)(num247 * num247 + num245 * num245));
                                num240 = 12f / num240;
                                num247 += (float)Main.rand.Next(-40, 41);
                                num245 += (float)Main.rand.Next(-40, 41);
                                num247 *= num240;
                                num245 *= num240;
                                Projectile.NewProjectile(value9.X, value9.Y, num247, num245, ProjectileID.WoodenArrowHostile, damage, 0f, Main.myPlayer, 0f, 0f);
                            }
                        }
                        else if (npc.type == 411)
                        {
                            Projectile.NewProjectile(value9.X, value9.Y, num247, num245, ProjectileID.WoodenArrowHostile, damage, 0f, Main.myPlayer, 0f, (float)npc.whoAmI);
                        }
                        else if (npc.type == 424)
                        {
                            for (int num227 = 0; num227 < 4; num227++)
                            {
                                Projectile.NewProjectile(npc.Center.X - (float)(npc.spriteDirection * 4), npc.Center.Y + 6f, (float)(-3 + 2 * num227) * 0.15f, (0f - (float)Main.rand.Next(0, 3)) * 0.2f - 0.1f, ProjectileID.WoodenArrowHostile, damage, 0f, Main.myPlayer, 0f, (float)npc.whoAmI);
                            }
                        }
                        else if (npc.type == 409)
                        {
                            int num226 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, 410, npc.whoAmI, 0f, 0f, 0f, 0f, 255);
                            Main.npc[num226].velocity = new Vector2(num247, -6f + num245);
                        }
                        else
                        {
                            Projectile.NewProjectile(value9.X, value9.Y, num247, num245, ProjectileID.WoodenArrowHostile, damage, 0f, Main.myPlayer, 0f, 0f);
                        }
                    }
                    if (Math.Abs(num245) > Math.Abs(num247) * 2f)
                    {
                        if (num245 > 0f)
                        {
                            npc.ai[2] = 1f;
                        }
                        else
                        {
                            npc.ai[2] = 5f;
                        }
                    }
                    else if (Math.Abs(num247) > Math.Abs(num245) * 2f)
                    {
                        npc.ai[2] = 3f;
                    }
                    else if (num245 > 0f)
                    {
                        npc.ai[2] = 2f;
                    }
                    else
                    {
                        npc.ai[2] = 4f;
                    }
                }
                if (npc.velocity.Y != 0f && !flag38)
                {
                    goto IL_a3fa;
                }
                if (npc.ai[1] <= 0f)
                {
                    goto IL_a3fa;
                }
                if (!flag39 || (num252 != -1 && npc.ai[1] >= (float)num252 && npc.ai[1] < (float)(num252 + num251) && (!flag38 || npc.velocity.Y == 0f)))
                {
                    npc.velocity.X = npc.velocity.X * 0.9f;
                    npc.spriteDirection = npc.direction;
                }
            }
            goto IL_a47a;
        IL_2e95:
            if (npc.type == 463 && Main.netMode != 1)
            {
                if (npc.localAI[3] > 0f)
                {
                    npc.localAI[3] -= 1f;
                }
                if (npc.justHit && npc.localAI[3] <= 0f && Main.rand.Next(3) == 0)
                {
                    npc.localAI[3] = 30f;
                    int num370 = Main.rand.Next(3, 6);
                    int[] array = new int[num370];
                    int num369 = 0;
                    for (int num368 = 0; num368 < 255; num368++)
                    {
                        if (Main.player[num368].active && !Main.player[num368].dead && Collision.CanHitLine(npc.position, npc.width, npc.height, Main.player[num368].position, Main.player[num368].width, Main.player[num368].height))
                        {
                            array[num369] = num368;
                            num369++;
                            if (num369 == num370)
                            {
                                break;
                            }
                        }
                    }
                    if (num369 > 1)
                    {
                        for (int num367 = 0; num367 < 100; num367++)
                        {
                            int num366 = Main.rand.Next(num369);
                            int num365;
                            for (num365 = num366; num365 == num366; num365 = Main.rand.Next(num369))
                            {
                            }
                            int num364 = array[num366];
                            array[num366] = array[num365];
                            array[num365] = num364;
                        }
                    }
                    Vector2 vector24 = new Vector2(-1f, -1f);
                    for (int num363 = 0; num363 < num369; num363++)
                    {
                        Vector2 value6 = Main.npc[array[num363]].Center - npc.Center;
                        value6.Normalize();
                        vector24 += value6;
                    }
                    vector24.Normalize();
                    for (int num362 = 0; num362 < num370; num362++)
                    {
                        float scaleFactor8 = (float)Main.rand.Next(8, 13);
                        Vector2 value7 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                        value7.Normalize();
                        if (num369 > 0)
                        {
                            value7 += vector24;
                            value7.Normalize();
                        }
                        value7 *= scaleFactor8;
                        if (num369 > 0)
                        {
                            num369--;
                            value7 = Main.player[array[num369]].Center - npc.Center;
                            value7.Normalize();
                            value7 *= scaleFactor8;
                        }
                        Projectile.NewProjectile(npc.Center.X, npc.position.Y + (float)(npc.width / 4), value7.X, value7.Y, ProjectileID.WoodenArrowHostile, damage, 1f, 255, 0f, 0f);
                    }
                }
            }
            if (npc.type == 469)
            {
                npc.knockBackResist = 0.45f * Main.knockBackMultiplier;
                if (npc.ai[2] == 1f)
                {
                    npc.knockBackResist = 0f;
                }
                bool flag47 = false;
                int num361 = (int)npc.Center.X / 16;
                int num360 = (int)npc.Center.Y / 16;
                for (int num359 = num361 - 1; num359 <= num361 + 1; num359++)
                {
                    int num358 = num360 - 1;
                    while (num358 <= num360 + 1)
                    {
                        if (Main.tile[num359, num358] == null || Main.tile[num359, num358].wall <= 0)
                        {
                            num358++;
                            continue;
                        }
                        flag47 = true;
                        break;
                    }
                    if (flag47)
                    {
                        break;
                    }
                }
                if (npc.ai[2] == 0f & flag47)
                {
                    if (npc.velocity.Y == 0f)
                    {
                        npc.velocity.Y = -4.6f;
                        npc.velocity.X = npc.velocity.X * 1.3f;
                    }
                    else if (npc.velocity.Y > 0f)
                    {
                        npc.ai[2] = 1f;
                    }
                }
                if (flag47 && npc.ai[2] == 1f && Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
                {
                    Vector2 value8 = Main.player[npc.target].Center - npc.Center;
                    float num357 = value8.Length();
                    value8.Normalize();
                    value8 *= 4.5f + num357 / 300f;
                    npc.velocity = (npc.velocity * 29f + value8) / 30f;
                    npc.noGravity = true;
                    npc.ai[2] = 1f;
                    return;
                }
                npc.noGravity = false;
                npc.ai[2] = 0f;
            }
            if (npc.type == 462 && npc.velocity.Y == 0f)
            {
                vector41 = Main.player[npc.target].Center - npc.Center;
                if (vector41.Length() < 150f && Math.Abs(npc.velocity.X) > 3f)
                {
                    if (npc.velocity.X < 0f && npc.Center.X > Main.player[npc.target].Center.X)
                    {
                        goto IL_3498;
                    }
                    if (npc.velocity.X > 0f && npc.Center.X < Main.player[npc.target].Center.X)
                    {
                        goto IL_3498;
                    }
                }
            }
            goto IL_362f;
        IL_1e03:
            if (npc.type == 415)
            {
                int num390 = 42;
                int num389 = 18;
                if (npc.justHit)
                {
                    npc.ai[2] = 120f;
                    npc.netUpdate = true;
                }
                if (npc.ai[2] > 0f)
                {
                    npc.ai[2] -= 1f;
                }
                if (npc.ai[2] == 0f)
                {
                    int num388 = 0;
                    for (int l = 0; l < 200; l++)
                    {
                        if (Main.npc[l].active && Main.npc[l].type == 516)
                        {
                            num388++;
                        }
                    }
                    if (num388 > 6)
                    {
                        npc.ai[2] = 90f;
                    }
                    else
                    {
                        if (Main.player[npc.target].Center.X < npc.Center.X && npc.direction < 0)
                        {
                            goto IL_1f22;
                        }
                        if (Main.player[npc.target].Center.X > npc.Center.X && npc.direction > 0)
                        {
                            goto IL_1f22;
                        }
                    }
                }
                else if (npc.ai[2] < 0f && npc.ai[2] > 0f - (float)num390)
                {
                    npc.ai[2] -= 1f;
                    if (npc.ai[2] == 0f - (float)num390)
                    {
                        npc.ai[2] = (float)(180 + 30 * Main.rand.Next(10));
                    }
                    npc.velocity.X = npc.velocity.X * 0.8f;
                    if (npc.ai[2] != 0f - (float)num389 && npc.ai[2] != 0f - (float)num389 - 8f && npc.ai[2] != 0f - (float)num389 - 16f)
                    {
                        return;
                    }
                    for (int k = 0; k < 20; k++)
                    {
                        Vector2 vector35 = npc.Center + Vector2.UnitX * (float)npc.spriteDirection * 40f;
                        Dust obj3 = Main.dust[Dust.NewDust(vector35, 0, 0, 259, 0f, 0f, 0, default(Color), 1f)];
                        Vector2 vector34 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                        obj3.position = vector35 + vector34 * 4f;
                        obj3.velocity = vector34 * 2f + Vector2.UnitX * Main.rand.NextFloat() * (float)npc.spriteDirection * 3f;
                        obj3.scale = 0.3f + vector34.X * (0f - (float)npc.spriteDirection);
                        obj3.fadeIn = 0.7f;
                        obj3.noGravity = true;
                    }
                    if (npc.velocity.X > -0.5f && npc.velocity.X < 0.5f)
                    {
                        npc.velocity.X = 0f;
                    }
                    if (Main.netMode != 1)
                    {
                        NPC.NewNPC((int)npc.Center.X + npc.spriteDirection * 45, (int)npc.Center.Y + 8, 516, 0, 0f, 0f, 0f, 0f, npc.target);
                    }
                    return;
                }
            }
            goto IL_21b6;
        IL_bdce:
            if ((npc.type == 31 || npc.type == 294 || npc.type == 295 || npc.type == 296 || npc.type == 47 || npc.type == 77 || npc.type == 104 || npc.type == 168 || npc.type == 196 || npc.type == 385 || npc.type == 389 || npc.type == 464 || npc.type == 470 || (npc.type >= 524 && npc.type <= 527)) && npc.velocity.Y == 0f && Math.Abs(npc.position.X + (float)(npc.width / 2) - (Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2))) < 100f && Math.Abs(npc.position.Y + (float)(npc.height / 2) - (Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2))) < 50f)
            {
                if (npc.direction > 0 && npc.velocity.X >= 1f)
                {
                    goto IL_bf88;
                }
                if (npc.direction < 0 && npc.velocity.X <= -1f)
                {
                    goto IL_bf88;
                }
            }
            goto IL_bfff;
        IL_afd9:
            int num205;
            int num206;
            int num204;
            Vector2 position2;
            if ((!Main.tile[num205, num204 - 1].nactive() || !Main.tileSolid[Main.tile[num205, num204 - 1].type] || Main.tileSolidTop[Main.tile[num205, num204 - 1].type] || (Main.tile[num205, num204 - 1].halfBrick() && (!Main.tile[num205, num204 - 4].nactive() || !Main.tileSolid[Main.tile[num205, num204 - 4].type] || Main.tileSolidTop[Main.tile[num205, num204 - 4].type]))) && (!Main.tile[num205, num204 - 2].nactive() || !Main.tileSolid[Main.tile[num205, num204 - 2].type] || Main.tileSolidTop[Main.tile[num205, num204 - 2].type]) && (!Main.tile[num205, num204 - 3].nactive() || !Main.tileSolid[Main.tile[num205, num204 - 3].type] || Main.tileSolidTop[Main.tile[num205, num204 - 3].type]) && (!Main.tile[num205 - num206, num204 - 3].nactive() || !Main.tileSolid[Main.tile[num205 - num206, num204 - 3].type]))
            {
                float num203 = (float)(num204 * 16);
                if (Main.tile[num205, num204].halfBrick())
                {
                    num203 += 8f;
                }
                if (Main.tile[num205, num204 - 1].halfBrick())
                {
                    num203 -= 8f;
                }
                if (num203 < position2.Y + (float)npc.height)
                {
                    float num202 = position2.Y + (float)npc.height - num203;
                    float num201 = 16.1f;
                    if (npc.type == 163 || npc.type == 164 || npc.type == 236 || npc.type == 239 || npc.type == 530)
                    {
                        num201 += 8f;
                    }
                    if (num202 <= num201)
                    {
                        npc.gfxOffY += npc.position.Y + (float)npc.height - num203;
                        npc.position.Y = num203 - (float)npc.height;
                        if (num202 < 9f)
                        {
                            npc.stepSpeed = 1f;
                        }
                        else
                        {
                            npc.stepSpeed = 2f;
                        }
                    }
                }
            }
            goto IL_b2a6;
        IL_362f:
            int num382;
            if (npc.ai[3] < (float)num382 && (Main.eclipse || !Main.dayTime || (double)npc.position.Y > Main.worldSurface * 16.0 || (Main.invasionType == 1 && (npc.type == 343 || npc.type == 350)) || (Main.invasionType == 1 && (npc.type == 26 || npc.type == 27 || npc.type == 28 || npc.type == 111 || npc.type == 471)) || npc.type == 73 || (Main.invasionType == 3 && npc.type >= 212 && npc.type <= 216) || (Main.invasionType == 4 && (npc.type == 381 || npc.type == 382 || npc.type == 383 || npc.type == 385 || npc.type == 386 || npc.type == 389 || npc.type == 391 || npc.type == 520)) || npc.type == 31 || npc.type == 294 || npc.type == 295 || npc.type == 296 || npc.type == 47 || npc.type == 67 || npc.type == 77 || npc.type == 78 || npc.type == 79 || npc.type == 80 || npc.type == 110 || true || npc.type == 120 || npc.type == 168 || npc.type == 181 || npc.type == 185 || npc.type == 198 || npc.type == 199 || npc.type == 206 || npc.type == 217 || npc.type == 218 || npc.type == 219 || npc.type == 220 || npc.type == 239 || npc.type == 243 || npc.type == 254 || npc.type == 255 || npc.type == 257 || npc.type == 258 || npc.type == 291 || npc.type == 292 || npc.type == 293 || npc.type == 379 || npc.type == 380 || npc.type == 464 || npc.type == 470 || npc.type == 424 || (npc.type == 411 && (npc.ai[1] >= 180f || npc.ai[1] < 90f)) || npc.type == 409 || npc.type == 425 || npc.type == 429 || npc.type == 427 || npc.type == 428 || npc.type == 508 || npc.type == 415 || npc.type == 419 || (npc.type >= 524 && npc.type <= 527) || npc.type == 528 || npc.type == 529 || npc.type == 530 || npc.type == 532))
            {
                if ((npc.type == 3 || npc.type == 331 || npc.type == 332 || npc.type == 21 || (npc.type >= 449 && npc.type <= 452) || npc.type == 31 || npc.type == 294 || npc.type == 295 || npc.type == 296 || npc.type == 77 || npc.type == 110 || true || npc.type == 132 || npc.type == 167 || npc.type == 161 || npc.type == 162 || npc.type == 186 || npc.type == 187 || npc.type == 188 || npc.type == 189 || npc.type == 197 || npc.type == 200 || npc.type == 201 || npc.type == 202 || npc.type == 203 || npc.type == 223 || npc.type == 291 || npc.type == 292 || npc.type == 293 || npc.type == 320 || npc.type == 321 || npc.type == 319 || npc.type == 481) && Main.rand.Next(1000) == 0)
                {
                    Main.PlaySound(14, (int)npc.position.X, (int)npc.position.Y, 1, 1f, 0f);
                }
                if (npc.type == 489 && Main.rand.Next(800) == 0)
                {
                    Main.PlaySound(14, (int)npc.position.X, (int)npc.position.Y, npc.type, 1f, 0f);
                }
                if ((npc.type == 78 || npc.type == 79 || npc.type == 80) && Main.rand.Next(500) == 0)
                {
                    Main.PlaySound(26, (int)npc.position.X, (int)npc.position.Y, 1, 1f, 0f);
                }
                if (npc.type == 159 && Main.rand.Next(500) == 0)
                {
                    Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 7, 1f, 0f);
                }
                if (npc.type == 162 && Main.rand.Next(500) == 0)
                {
                    Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 6, 1f, 0f);
                }
                if (npc.type == 181 && Main.rand.Next(500) == 0)
                {
                    Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 8, 1f, 0f);
                }
                if (npc.type >= 269 && npc.type <= 280 && Main.rand.Next(1000) == 0)
                {
                    Main.PlaySound(14, (int)npc.position.X, (int)npc.position.Y, 1, 1f, 0f);
                }
                npc.TargetClosest(true);
            }
            else if (npc.ai[2] <= 0f || (false && npc.type != 111 && npc.type != 206 && npc.type != 216 && npc.type != 214 && npc.type != 215 && npc.type != 291 && npc.type != 292 && npc.type != 293 && npc.type != 350 && npc.type != 381 && npc.type != 382 && npc.type != 383 && npc.type != 385 && npc.type != 386 && npc.type != 389 && npc.type != 391 && npc.type != 469 && npc.type != 166 && npc.type != 466 && npc.type != 471 && npc.type != 411 && npc.type != 409 && npc.type != 424 && npc.type != 425 && npc.type != 426 && npc.type != 415 && npc.type != 419 && npc.type != 520))
            {
                if (Main.dayTime && (double)(npc.position.Y / 16f) < Main.worldSurface && npc.timeLeft > 10)
                {
                    npc.timeLeft = 10;
                }
                if (npc.velocity.X == 0f)
                {
                    if (npc.velocity.Y == 0f)
                    {
                        npc.ai[0] += 1f;
                        if (npc.ai[0] >= 2f)
                        {
                            npc.direction *= -1;
                            npc.spriteDirection = npc.direction;
                            npc.ai[0] = 0f;
                        }
                    }
                }
                else
                {
                    npc.ai[0] = 0f;
                }
                if (npc.direction == 0)
                {
                    npc.direction = 1;
                }
            }
            if (npc.type != 159 && npc.type != 349)
            {
                if (npc.type == 199)
                {
                    if (npc.velocity.X < -4f || npc.velocity.X > 4f)
                    {
                        if (npc.velocity.Y == 0f)
                        {
                            npc.velocity *= 0.8f;
                        }
                    }
                    else if (npc.velocity.X < 4f && npc.direction == 1)
                    {
                        if (npc.velocity.Y == 0f && npc.velocity.X < 0f)
                        {
                            npc.velocity.X = npc.velocity.X * 0.8f;
                        }
                        npc.velocity.X = npc.velocity.X + 0.1f;
                        if (npc.velocity.X > 4f)
                        {
                            npc.velocity.X = 4f;
                        }
                    }
                    else if (npc.velocity.X > -4f && npc.direction == -1)
                    {
                        if (npc.velocity.Y == 0f && npc.velocity.X > 0f)
                        {
                            npc.velocity.X = npc.velocity.X * 0.8f;
                        }
                        npc.velocity.X = npc.velocity.X - 0.1f;
                        if (npc.velocity.X < -4f)
                        {
                            npc.velocity.X = -4f;
                        }
                    }
                }
                else if (npc.type == 120 || npc.type == 166 || npc.type == 213 || npc.type == 258 || npc.type == 528 || npc.type == 529)
                {
                    if (npc.velocity.X < -3f || npc.velocity.X > 3f)
                    {
                        if (npc.velocity.Y == 0f)
                        {
                            npc.velocity *= 0.8f;
                        }
                    }
                    else if (npc.velocity.X < 3f && npc.direction == 1)
                    {
                        if (npc.velocity.Y == 0f && npc.velocity.X < 0f)
                        {
                            npc.velocity.X = npc.velocity.X * 0.99f;
                        }
                        npc.velocity.X = npc.velocity.X + 0.07f;
                        if (npc.velocity.X > 3f)
                        {
                            npc.velocity.X = 3f;
                        }
                    }
                    else if (npc.velocity.X > -3f && npc.direction == -1)
                    {
                        if (npc.velocity.Y == 0f && npc.velocity.X > 0f)
                        {
                            npc.velocity.X = npc.velocity.X * 0.99f;
                        }
                        npc.velocity.X = npc.velocity.X - 0.07f;
                        if (npc.velocity.X < -3f)
                        {
                            npc.velocity.X = -3f;
                        }
                    }
                }
                else if (npc.type == 461 || npc.type == 27 || npc.type == 77 || npc.type == 104 || npc.type == 163 || npc.type == 162 || npc.type == 196 || npc.type == 197 || npc.type == 212 || npc.type == 257 || npc.type == 326 || npc.type == 343 || npc.type == 348 || npc.type == 351 || (npc.type >= 524 && npc.type <= 527) || npc.type == 530)
                {
                    if (npc.velocity.X < -2f || npc.velocity.X > 2f)
                    {
                        if (npc.velocity.Y == 0f)
                        {
                            npc.velocity *= 0.8f;
                        }
                    }
                    else if (npc.velocity.X < 2f && npc.direction == 1)
                    {
                        npc.velocity.X = npc.velocity.X + 0.07f;
                        if (npc.velocity.X > 2f)
                        {
                            npc.velocity.X = 2f;
                        }
                    }
                    else if (npc.velocity.X > -2f && npc.direction == -1)
                    {
                        npc.velocity.X = npc.velocity.X - 0.07f;
                        if (npc.velocity.X < -2f)
                        {
                            npc.velocity.X = -2f;
                        }
                    }
                }
                else if (npc.type == 109)
                {
                    if (npc.velocity.X < -2f || npc.velocity.X > 2f)
                    {
                        if (npc.velocity.Y == 0f)
                        {
                            npc.velocity *= 0.8f;
                        }
                    }
                    else if (npc.velocity.X < 2f && npc.direction == 1)
                    {
                        npc.velocity.X = npc.velocity.X + 0.04f;
                        if (npc.velocity.X > 2f)
                        {
                            npc.velocity.X = 2f;
                        }
                    }
                    else if (npc.velocity.X > -2f && npc.direction == -1)
                    {
                        npc.velocity.X = npc.velocity.X - 0.04f;
                        if (npc.velocity.X < -2f)
                        {
                            npc.velocity.X = -2f;
                        }
                    }
                }
                else if (npc.type == 21 || npc.type == 26 || npc.type == 31 || npc.type == 294 || npc.type == 295 || npc.type == 296 || npc.type == 47 || npc.type == 73 || npc.type == 140 || npc.type == 164 || npc.type == 239 || npc.type == 167 || npc.type == 168 || npc.type == 185 || npc.type == 198 || npc.type == 201 || npc.type == 202 || npc.type == 203 || npc.type == 217 || npc.type == 218 || npc.type == 219 || npc.type == 226 || npc.type == 181 || npc.type == 254 || npc.type == 338 || npc.type == 339 || npc.type == 340 || npc.type == 342 || npc.type == 385 || npc.type == 389 || npc.type == 462 || npc.type == 463 || npc.type == 466 || npc.type == 464 || npc.type == 469 || npc.type == 470 || npc.type == 480 || npc.type == 482 || npc.type == 425 || npc.type == 429)
                {
                    float num324 = 1.5f;
                    if (npc.type == 294)
                    {
                        num324 = 2f;
                    }
                    else if (npc.type == 295)
                    {
                        num324 = 1.75f;
                    }
                    else if (npc.type == 296)
                    {
                        num324 = 1.25f;
                    }
                    else if (npc.type == 201)
                    {
                        num324 = 1.1f;
                    }
                    else if (npc.type == 202)
                    {
                        num324 = 0.9f;
                    }
                    else if (npc.type == 203)
                    {
                        num324 = 1.2f;
                    }
                    else if (npc.type == 338)
                    {
                        num324 = 1.75f;
                    }
                    else if (npc.type == 339)
                    {
                        num324 = 1.25f;
                    }
                    else if (npc.type == 340)
                    {
                        num324 = 2f;
                    }
                    else if (npc.type == 385)
                    {
                        num324 = 1.8f;
                    }
                    else if (npc.type == 389)
                    {
                        num324 = 2.25f;
                    }
                    else if (npc.type == 462)
                    {
                        num324 = 4f;
                    }
                    else if (npc.type == 463)
                    {
                        num324 = 0.75f;
                    }
                    else if (npc.type == 466)
                    {
                        num324 = 3.75f;
                    }
                    else if (npc.type == 469)
                    {
                        num324 = 3.25f;
                    }
                    else if (npc.type == 480)
                    {
                        num324 = 1.5f + (1f - (float)npc.life / (float)npc.lifeMax) * 2f;
                    }
                    else if (npc.type == 425)
                    {
                        num324 = 6f;
                    }
                    else if (npc.type == 429)
                    {
                        num324 = 4f;
                    }
                    if (npc.type == 21 || npc.type == 201 || npc.type == 202 || npc.type == 203 || npc.type == 342)
                    {
                        num324 *= 1f + (1f - npc.scale);
                    }
                    if (npc.velocity.X < 0f - num324 || npc.velocity.X > num324)
                    {
                        if (npc.velocity.Y == 0f)
                        {
                            npc.velocity *= 0.8f;
                        }
                    }
                    else if (npc.velocity.X < num324 && npc.direction == 1)
                    {
                        if (npc.type == 466 && npc.velocity.X < -2f)
                        {
                            npc.velocity.X = npc.velocity.X * 0.9f;
                        }
                        npc.velocity.X = npc.velocity.X + 0.07f;
                        if (npc.velocity.X > num324)
                        {
                            npc.velocity.X = num324;
                        }
                    }
                    else if (npc.velocity.X > 0f - num324 && npc.direction == -1)
                    {
                        if (npc.type == 466 && npc.velocity.X > 2f)
                        {
                            npc.velocity.X = npc.velocity.X * 0.9f;
                        }
                        npc.velocity.X = npc.velocity.X - 0.07f;
                        if (npc.velocity.X < 0f - num324)
                        {
                            npc.velocity.X = 0f - num324;
                        }
                    }
                    if (npc.velocity.Y == 0f && npc.type == 462)
                    {
                        if (npc.direction > 0 && npc.velocity.X < 0f)
                        {
                            goto IL_509d;
                        }
                        if (npc.direction < 0 && npc.velocity.X > 0f)
                        {
                            goto IL_509d;
                        }
                    }
                }
                else if (npc.type >= 269 && npc.type <= 280)
                {
                    float num356 = 1.5f;
                    if (npc.type == 269)
                    {
                        num356 = 2f;
                    }
                    if (npc.type == 270)
                    {
                        num356 = 1f;
                    }
                    if (npc.type == 271)
                    {
                        num356 = 1.5f;
                    }
                    if (npc.type == 272)
                    {
                        num356 = 3f;
                    }
                    if (npc.type == 273)
                    {
                        num356 = 1.25f;
                    }
                    if (npc.type == 274)
                    {
                        num356 = 3f;
                    }
                    if (npc.type == 275)
                    {
                        num356 = 3.25f;
                    }
                    if (npc.type == 276)
                    {
                        num356 = 2f;
                    }
                    if (npc.type == 277)
                    {
                        num356 = 2.75f;
                    }
                    if (npc.type == 278)
                    {
                        num356 = 1.8f;
                    }
                    if (npc.type == 279)
                    {
                        num356 = 1.3f;
                    }
                    if (npc.type == 280)
                    {
                        num356 = 2.5f;
                    }
                    num356 *= 1f + (1f - npc.scale);
                    if (npc.velocity.X < 0f - num356 || npc.velocity.X > num356)
                    {
                        if (npc.velocity.Y == 0f)
                        {
                            npc.velocity *= 0.8f;
                        }
                    }
                    else if (npc.velocity.X < num356 && npc.direction == 1)
                    {
                        npc.velocity.X = npc.velocity.X + 0.07f;
                        if (npc.velocity.X > num356)
                        {
                            npc.velocity.X = num356;
                        }
                    }
                    else if (npc.velocity.X > 0f - num356 && npc.direction == -1)
                    {
                        npc.velocity.X = npc.velocity.X - 0.07f;
                        if (npc.velocity.X < 0f - num356)
                        {
                            npc.velocity.X = 0f - num356;
                        }
                    }
                }
                else if (npc.type >= 305 && npc.type <= 314)
                {
                    float num354 = 1.5f;
                    if (npc.type == 305 || npc.type == 310)
                    {
                        num354 = 2f;
                    }
                    if (npc.type == 306 || npc.type == 311)
                    {
                        num354 = 1.25f;
                    }
                    if (npc.type == 307 || npc.type == 312)
                    {
                        num354 = 2.25f;
                    }
                    if (npc.type == 308 || npc.type == 313)
                    {
                        num354 = 1.5f;
                    }
                    if (npc.type == 309 || npc.type == 314)
                    {
                        num354 = 1f;
                    }
                    if (npc.type < 310)
                    {
                        if (npc.velocity.Y == 0f)
                        {
                            npc.velocity.X = npc.velocity.X * 0.85f;
                            if ((double)npc.velocity.X > -0.3 && (double)npc.velocity.X < 0.3)
                            {
                                npc.velocity.Y = -7f;
                                npc.velocity.X = num354 * (float)npc.direction;
                            }
                        }
                        else if (npc.spriteDirection == npc.direction)
                        {
                            npc.velocity.X = (npc.velocity.X * 10f + num354 * (float)npc.direction) / 11f;
                        }
                    }
                    else if (npc.velocity.X < 0f - num354 || npc.velocity.X > num354)
                    {
                        if (npc.velocity.Y == 0f)
                        {
                            npc.velocity *= 0.8f;
                        }
                    }
                    else if (npc.velocity.X < num354 && npc.direction == 1)
                    {
                        npc.velocity.X = npc.velocity.X + 0.07f;
                        if (npc.velocity.X > num354)
                        {
                            npc.velocity.X = num354;
                        }
                    }
                    else if (npc.velocity.X > 0f - num354 && npc.direction == -1)
                    {
                        npc.velocity.X = npc.velocity.X - 0.07f;
                        if (npc.velocity.X < 0f - num354)
                        {
                            npc.velocity.X = 0f - num354;
                        }
                    }
                }
                else if (npc.type == 67 || npc.type == 220 || npc.type == 428)
                {
                    if (npc.velocity.X < -0.5f || npc.velocity.X > 0.5f)
                    {
                        if (npc.velocity.Y == 0f)
                        {
                            npc.velocity *= 0.7f;
                        }
                    }
                    else if (npc.velocity.X < 0.5f && npc.direction == 1)
                    {
                        npc.velocity.X = npc.velocity.X + 0.03f;
                        if (npc.velocity.X > 0.5f)
                        {
                            npc.velocity.X = 0.5f;
                        }
                    }
                    else if (npc.velocity.X > -0.5f && npc.direction == -1)
                    {
                        npc.velocity.X = npc.velocity.X - 0.03f;
                        if (npc.velocity.X < -0.5f)
                        {
                            npc.velocity.X = -0.5f;
                        }
                    }
                }
                else if (npc.type == 78 || npc.type == 79 || npc.type == 80)
                {
                    float num326 = 1f;
                    float num325 = 0.05f;
                    if (npc.life < npc.lifeMax / 2)
                    {
                        num326 = 2f;
                        num325 = 0.1f;
                    }
                    if (npc.type == 79)
                    {
                        num326 *= 1.5f;
                    }
                    if (npc.velocity.X < 0f - num326 || npc.velocity.X > num326)
                    {
                        if (npc.velocity.Y == 0f)
                        {
                            npc.velocity *= 0.7f;
                        }
                    }
                    else if (npc.velocity.X < num326 && npc.direction == 1)
                    {
                        npc.velocity.X = npc.velocity.X + num325;
                        if (npc.velocity.X > num326)
                        {
                            npc.velocity.X = num326;
                        }
                    }
                    else if (npc.velocity.X > 0f - num326 && npc.direction == -1)
                    {
                        npc.velocity.X = npc.velocity.X - num325;
                        if (npc.velocity.X < 0f - num326)
                        {
                            npc.velocity.X = 0f - num326;
                        }
                    }
                }
                else if (npc.type == 287)
                {
                    float num353 = 5f;
                    float num352 = 0.2f;
                    if (npc.velocity.X < 0f - num353 || npc.velocity.X > num353)
                    {
                        if (npc.velocity.Y == 0f)
                        {
                            npc.velocity *= 0.7f;
                        }
                    }
                    else if (npc.velocity.X < num353 && npc.direction == 1)
                    {
                        npc.velocity.X = npc.velocity.X + num352;
                        if (npc.velocity.X > num353)
                        {
                            npc.velocity.X = num353;
                        }
                    }
                    else if (npc.velocity.X > 0f - num353 && npc.direction == -1)
                    {
                        npc.velocity.X = npc.velocity.X - num352;
                        if (npc.velocity.X < 0f - num353)
                        {
                            npc.velocity.X = 0f - num353;
                        }
                    }
                }
                else if (npc.type == 243)
                {
                    float num351 = 1f;
                    float num350 = 0.07f;
                    num351 += (1f - (float)npc.life / (float)npc.lifeMax) * 1.5f;
                    num350 += (1f - (float)npc.life / (float)npc.lifeMax) * 0.15f;
                    if (npc.velocity.X < 0f - num351 || npc.velocity.X > num351)
                    {
                        if (npc.velocity.Y == 0f)
                        {
                            npc.velocity *= 0.7f;
                        }
                    }
                    else if (npc.velocity.X < num351 && npc.direction == 1)
                    {
                        npc.velocity.X = npc.velocity.X + num350;
                        if (npc.velocity.X > num351)
                        {
                            npc.velocity.X = num351;
                        }
                    }
                    else if (npc.velocity.X > 0f - num351 && npc.direction == -1)
                    {
                        npc.velocity.X = npc.velocity.X - num350;
                        if (npc.velocity.X < 0f - num351)
                        {
                            npc.velocity.X = 0f - num351;
                        }
                    }
                }
                else if (npc.type == 251)
                {
                    float num347 = 1f;
                    float num346 = 0.08f;
                    num347 += (1f - (float)npc.life / (float)npc.lifeMax) * 2f;
                    num346 += (1f - (float)npc.life / (float)npc.lifeMax) * 0.2f;
                    if (npc.velocity.X < 0f - num347 || npc.velocity.X > num347)
                    {
                        if (npc.velocity.Y == 0f)
                        {
                            npc.velocity *= 0.7f;
                        }
                    }
                    else if (npc.velocity.X < num347 && npc.direction == 1)
                    {
                        npc.velocity.X = npc.velocity.X + num346;
                        if (npc.velocity.X > num347)
                        {
                            npc.velocity.X = num347;
                        }
                    }
                    else if (npc.velocity.X > 0f - num347 && npc.direction == -1)
                    {
                        npc.velocity.X = npc.velocity.X - num346;
                        if (npc.velocity.X < 0f - num347)
                        {
                            npc.velocity.X = 0f - num347;
                        }
                    }
                }
                else if (npc.type == 386)
                {
                    if (npc.ai[2] > 0f)
                    {
                        if (npc.velocity.Y == 0f)
                        {
                            npc.velocity.X = npc.velocity.X * 0.8f;
                        }
                    }
                    else
                    {
                        float num343 = 0.15f;
                        float num342 = 1.5f;
                        if (npc.velocity.X < 0f - num342 || npc.velocity.X > num342)
                        {
                            if (npc.velocity.Y == 0f)
                            {
                                npc.velocity *= 0.7f;
                            }
                        }
                        else if (npc.velocity.X < num342 && npc.direction == 1)
                        {
                            npc.velocity.X = npc.velocity.X + num343;
                            if (npc.velocity.X > num342)
                            {
                                npc.velocity.X = num342;
                            }
                        }
                        else if (npc.velocity.X > 0f - num342 && npc.direction == -1)
                        {
                            npc.velocity.X = npc.velocity.X - num343;
                            if (npc.velocity.X < 0f - num342)
                            {
                                npc.velocity.X = 0f - num342;
                            }
                        }
                    }
                }
                else if (npc.type == 460)
                {
                    float num341 = 3f;
                    float num340 = 0.1f;
                    if (Math.Abs(npc.velocity.X) > 2f)
                    {
                        num340 *= 0.8f;
                    }
                    if ((double)Math.Abs(npc.velocity.X) > 2.5)
                    {
                        num340 *= 0.8f;
                    }
                    if (Math.Abs(npc.velocity.X) > 3f)
                    {
                        num340 *= 0.8f;
                    }
                    if ((double)Math.Abs(npc.velocity.X) > 3.5)
                    {
                        num340 *= 0.8f;
                    }
                    if (Math.Abs(npc.velocity.X) > 4f)
                    {
                        num340 *= 0.8f;
                    }
                    if ((double)Math.Abs(npc.velocity.X) > 4.5)
                    {
                        num340 *= 0.8f;
                    }
                    if (Math.Abs(npc.velocity.X) > 5f)
                    {
                        num340 *= 0.8f;
                    }
                    if ((double)Math.Abs(npc.velocity.X) > 5.5)
                    {
                        num340 *= 0.8f;
                    }
                    num341 += (1f - (float)npc.life / (float)npc.lifeMax) * 3f;
                    if (npc.velocity.X < 0f - num341 || npc.velocity.X > num341)
                    {
                        if (npc.velocity.Y == 0f)
                        {
                            npc.velocity *= 0.7f;
                        }
                    }
                    else if (npc.velocity.X < num341 && npc.direction == 1)
                    {
                        if (npc.velocity.X < 0f)
                        {
                            npc.velocity.X = npc.velocity.X * 0.93f;
                        }
                        npc.velocity.X = npc.velocity.X + num340;
                        if (npc.velocity.X > num341)
                        {
                            npc.velocity.X = num341;
                        }
                    }
                    else if (npc.velocity.X > 0f - num341 && npc.direction == -1)
                    {
                        if (npc.velocity.X > 0f)
                        {
                            npc.velocity.X = npc.velocity.X * 0.93f;
                        }
                        npc.velocity.X = npc.velocity.X - num340;
                        if (npc.velocity.X < 0f - num341)
                        {
                            npc.velocity.X = 0f - num341;
                        }
                    }
                }
                else if (npc.type == 391 || npc.type == 427 || npc.type == 415 || npc.type == 419 || npc.type == 518 || npc.type == 532)
                {
                    float num328 = 5f;
                    float num327 = 0.25f;
                    float scaleFactor7 = 0.7f;
                    if (npc.type == 427)
                    {
                        num328 = 6f;
                        num327 = 0.2f;
                        scaleFactor7 = 0.8f;
                    }
                    else if (npc.type == 415)
                    {
                        num328 = 4f;
                        num327 = 0.1f;
                        scaleFactor7 = 0.95f;
                    }
                    else if (npc.type == 419)
                    {
                        num328 = 6f;
                        num327 = 0.15f;
                        scaleFactor7 = 0.85f;
                    }
                    else if (npc.type == 518)
                    {
                        num328 = 5f;
                        num327 = 0.1f;
                        scaleFactor7 = 0.95f;
                    }
                    else if (npc.type == 532)
                    {
                        num328 = 5f;
                        num327 = 0.15f;
                        scaleFactor7 = 0.98f;
                    }
                    if (npc.velocity.X < 0f - num328 || npc.velocity.X > num328)
                    {
                        if (npc.velocity.Y == 0f)
                        {
                            npc.velocity *= scaleFactor7;
                        }
                    }
                    else if (npc.velocity.X < num328 && npc.direction == 1)
                    {
                        npc.velocity.X = npc.velocity.X + num327;
                        if (npc.velocity.X > num328)
                        {
                            npc.velocity.X = num328;
                        }
                    }
                    else if (npc.velocity.X > 0f - num328 && npc.direction == -1)
                    {
                        npc.velocity.X = npc.velocity.X - num327;
                        if (npc.velocity.X < 0f - num328)
                        {
                            npc.velocity.X = 0f - num328;
                        }
                    }
                }
                else
                {
                    if ((npc.type < 430 || npc.type > 436) && npc.type != 494 && npc.type != 495)
                    {
                        if (false && npc.type != 111 && npc.type != 206 && npc.type != 214 && npc.type != 215 && npc.type != 216 && npc.type != 290 && npc.type != 291 && npc.type != 292 && npc.type != 293 && npc.type != 350 && npc.type != 379 && npc.type != 380 && npc.type != 381 && npc.type != 382 && (npc.type < 449 || npc.type > 452) && npc.type != 468 && npc.type != 481 && npc.type != 411 && npc.type != 409 && (npc.type < 498 || npc.type > 506) && npc.type != 424 && npc.type != 426 && npc.type != 520)
                        {
                            float num335 = 1f;
                            if (npc.type == 186)
                            {
                                num335 = 1.1f;
                            }
                            if (npc.type == 187)
                            {
                                num335 = 0.9f;
                            }
                            if (npc.type == 188)
                            {
                                num335 = 1.2f;
                            }
                            if (npc.type == 189)
                            {
                                num335 = 0.8f;
                            }
                            if (npc.type == 132)
                            {
                                num335 = 0.95f;
                            }
                            if (npc.type == 200)
                            {
                                num335 = 0.87f;
                            }
                            if (npc.type == 223)
                            {
                                num335 = 1.05f;
                            }
                            if (npc.type == 489)
                            {
                                vector41 = Main.player[npc.target].Center - npc.Center;
                                float num334 = vector41.Length();
                                num334 *= 0.0025f;
                                if ((double)num334 > 1.5)
                                {
                                    num334 = 1.5f;
                                }
                                num335 = ((!Main.expertMode) ? (2.5f - num334) : (3f - num334));
                                num335 *= 0.8f;
                            }
                            if (npc.type == 489 || npc.type == 3 || npc.type == 132 || npc.type == 186 || npc.type == 187 || npc.type == 188 || npc.type == 189 || npc.type == 200 || npc.type == 223 || npc.type == 331 || npc.type == 332)
                            {
                                num335 *= 1f + (1f - npc.scale);
                            }
                            if (npc.velocity.X < 0f - num335 || npc.velocity.X > num335)
                            {
                                if (npc.velocity.Y == 0f)
                                {
                                    npc.velocity *= 0.8f;
                                }
                            }
                            else if (npc.velocity.X < num335 && npc.direction == 1)
                            {
                                npc.velocity.X = npc.velocity.X + 0.07f;
                                if (npc.velocity.X > num335)
                                {
                                    npc.velocity.X = num335;
                                }
                            }
                            else if (npc.velocity.X > 0f - num335 && npc.direction == -1)
                            {
                                npc.velocity.X = npc.velocity.X - 0.07f;
                                if (npc.velocity.X < 0f - num335)
                                {
                                    npc.velocity.X = 0f - num335;
                                }
                            }
                        }
                        goto IL_6bf9;
                    }
                    if (npc.ai[2] == 0f)
                    {
                        npc.damage = npc.defDamage;
                        float num331 = 1f;
                        num331 *= 1f + (1f - npc.scale);
                        if (npc.velocity.X < 0f - num331 || npc.velocity.X > num331)
                        {
                            if (npc.velocity.Y == 0f)
                            {
                                npc.velocity *= 0.8f;
                            }
                        }
                        else if (npc.velocity.X < num331 && npc.direction == 1)
                        {
                            npc.velocity.X = npc.velocity.X + 0.07f;
                            if (npc.velocity.X > num331)
                            {
                                npc.velocity.X = num331;
                            }
                        }
                        else if (npc.velocity.X > 0f - num331 && npc.direction == -1)
                        {
                            npc.velocity.X = npc.velocity.X - 0.07f;
                            if (npc.velocity.X < 0f - num331)
                            {
                                npc.velocity.X = 0f - num331;
                            }
                        }
                        if (npc.velocity.Y == 0f && (!Main.dayTime || (double)npc.position.Y > Main.worldSurface * 16.0) && !Main.player[npc.target].dead)
                        {
                            Vector2 vector25 = npc.Center - Main.player[npc.target].Center;
                            int num329 = 50;
                            if (npc.type >= 494 && npc.type <= 495)
                            {
                                num329 = 42;
                            }
                            if (vector25.Length() < (float)num329 && Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
                            {
                                npc.velocity.X = npc.velocity.X * 0.7f;
                                npc.ai[2] = 1f;
                            }
                        }
                    }
                    else
                    {
                        npc.damage = (int)((double)npc.defDamage * 1.5);
                        npc.ai[3] = 1f;
                        npc.velocity.X = npc.velocity.X * 0.9f;
                        if ((double)Math.Abs(npc.velocity.X) < 0.1)
                        {
                            npc.velocity.X = 0f;
                        }
                        npc.ai[2] += 1f;
                        if (npc.ai[2] >= 20f || npc.velocity.Y != 0f || (Main.dayTime && (double)npc.position.Y < Main.worldSurface * 16.0))
                        {
                            npc.ai[2] = 0f;
                        }
                    }
                }
                goto IL_6bf9;
            }
            if (npc.type == 159)
            {
                if (npc.velocity.X > 0f && npc.direction < 0)
                {
                    goto IL_41b9;
                }
                if (npc.velocity.X < 0f && npc.direction > 0)
                {
                    goto IL_41b9;
                }
            }
            goto IL_41d5;
        IL_1f22:
            if (Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
            {
                npc.ai[2] = -1f;
                npc.netUpdate = true;
                npc.TargetClosest(true);
            }
            goto IL_21b6;
        IL_0b35:
            if (npc.velocity.Y == 0f && npc.Distance(Main.player[npc.target].Center) < 900f && Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
            {
                npc.ai[2] = 0f - (float)num410 - (float)num408;
                npc.netUpdate = true;
            }
            goto IL_119a;
        IL_b2a6:
            bool flag33;
            int num200;
            int num199;
            bool flag50;
            if (flag33)
            {
                num200 = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
                num199 = (int)((npc.position.Y + (float)npc.height - 15f) / 16f);
                if (npc.type == 109 || npc.type == 163 || npc.type == 164 || npc.type == 199 || npc.type == 236 || npc.type == 239 || npc.type == 257 || npc.type == 258 || npc.type == 290 || npc.type == 391 || npc.type == 425 || npc.type == 427 || npc.type == 426 || npc.type == 508 || npc.type == 415 || npc.type == 530 || npc.type == 532)
                {
                    num200 = (int)((npc.position.X + (float)(npc.width / 2) + (float)((npc.width / 2 + 16) * npc.direction)) / 16f);
                }
                if (Main.tile[num200, num199] == null)
                {
                    Tile[,] tile3 = Main.tile;
                    int num418 = num200;
                    int num419 = num199;
                    Tile tile4 = new Tile();
                    tile3[num418, num419] = tile4;
                }
                if (Main.tile[num200, num199 - 1] == null)
                {
                    Tile[,] tile5 = Main.tile;
                    int num420 = num200;
                    int num421 = num199 - 1;
                    Tile tile6 = new Tile();
                    tile5[num420, num421] = tile6;
                }
                if (Main.tile[num200, num199 - 2] == null)
                {
                    Tile[,] tile7 = Main.tile;
                    int num422 = num200;
                    int num423 = num199 - 2;
                    Tile tile8 = new Tile();
                    tile7[num422, num423] = tile8;
                }
                if (Main.tile[num200, num199 - 3] == null)
                {
                    Tile[,] tile9 = Main.tile;
                    int num424 = num200;
                    int num425 = num199 - 3;
                    Tile tile10 = new Tile();
                    tile9[num424, num425] = tile10;
                }
                if (Main.tile[num200, num199 + 1] == null)
                {
                    Tile[,] tile11 = Main.tile;
                    int num426 = num200;
                    int num427 = num199 + 1;
                    Tile tile12 = new Tile();
                    tile11[num426, num427] = tile12;
                }
                if (Main.tile[num200 + npc.direction, num199 - 1] == null)
                {
                    Tile[,] tile13 = Main.tile;
                    int num428 = num200 + npc.direction;
                    int num429 = num199 - 1;
                    Tile tile14 = new Tile();
                    tile13[num428, num429] = tile14;
                }
                if (Main.tile[num200 + npc.direction, num199 + 1] == null)
                {
                    Tile[,] tile15 = Main.tile;
                    int num430 = num200 + npc.direction;
                    int num431 = num199 + 1;
                    Tile tile16 = new Tile();
                    tile15[num430, num431] = tile16;
                }
                if (Main.tile[num200 - npc.direction, num199 + 1] == null)
                {
                    Tile[,] tile17 = Main.tile;
                    int num432 = num200 - npc.direction;
                    int num433 = num199 + 1;
                    Tile tile18 = new Tile();
                    tile17[num432, num433] = tile18;
                }
                Main.tile[num200, num199 + 1].halfBrick();
                if ((Main.tile[num200, num199 - 1].nactive() && (TileLoader.IsClosedDoor(Main.tile[num200, num199 - 1]) || Main.tile[num200, num199 - 1].type == 388)) & flag50)
                {
                    npc.ai[2] += 1f;
                    npc.ai[3] = 0f;
                    if (npc.ai[2] >= 60f)
                    {
                        if (!Main.bloodMoon && (npc.type == 3 || npc.type == 331 || npc.type == 332 || npc.type == 132 || npc.type == 161 || npc.type == 186 || npc.type == 187 || npc.type == 188 || npc.type == 189 || npc.type == 200 || npc.type == 223 || npc.type == 320 || npc.type == 321 || npc.type == 319))
                        {
                            npc.ai[1] = 0f;
                        }
                        npc.velocity.X = 0.5f * (0f - (float)npc.direction);
                        int num198 = 5;
                        if (Main.tile[num200, num199 - 1].type == 388)
                        {
                            num198 = 2;
                        }
                        npc.ai[1] += (float)num198;
                        if (npc.type == 27)
                        {
                            npc.ai[1] += 1f;
                        }
                        if (npc.type == 31 || npc.type == 294 || npc.type == 295 || npc.type == 296)
                        {
                            npc.ai[1] += 6f;
                        }
                        npc.ai[2] = 0f;
                        bool flag32 = false;
                        if (npc.ai[1] >= 10f)
                        {
                            flag32 = true;
                            npc.ai[1] = 10f;
                        }
                        if (npc.type == 460)
                        {
                            flag32 = true;
                        }
                        WorldGen.KillTile(num200, num199 - 1, true, false, false);
                        if (((Main.netMode != 1 || !flag32) & flag32) && Main.netMode != 1)
                        {
                            if (npc.type == 26)
                            {
                                WorldGen.KillTile(num200, num199 - 1, false, false, false);
                                if (Main.netMode == 2)
                                {
                                    NetMessage.SendData(17, -1, -1, null, 0, (float)num200, (float)(num199 - 1), 0f, 0, 0, 0);
                                }
                            }
                            else
                            {
                                if (TileLoader.OpenDoorID(Main.tile[num200, num199 - 1]) >= 0)
                                {
                                    bool flag31 = WorldGen.OpenDoor(num200, num199 - 1, npc.direction);
                                    if (!flag31)
                                    {
                                        npc.ai[3] = (float)num382;
                                        npc.netUpdate = true;
                                    }
                                    if (Main.netMode == 2 & flag31)
                                    {
                                        NetMessage.SendData(19, -1, -1, null, 0, (float)num200, (float)(num199 - 1), (float)npc.direction, 0, 0, 0);
                                    }
                                }
                                if (Main.tile[num200, num199 - 1].type == 388)
                                {
                                    bool flag30 = WorldGen.ShiftTallGate(num200, num199 - 1, false);
                                    if (!flag30)
                                    {
                                        npc.ai[3] = (float)num382;
                                        npc.netUpdate = true;
                                    }
                                    if (Main.netMode == 2 & flag30)
                                    {
                                        NetMessage.SendData(19, -1, -1, null, 4, (float)num200, (float)(num199 - 1), 0f, 0, 0, 0);
                                    }
                                }
                            }
                        }
                    }
                    goto IL_c227;
                }
                int num197 = npc.spriteDirection;
                if (npc.type == 425)
                {
                    num197 *= -1;
                }
                if (npc.velocity.X < 0f && num197 == -1)
                {
                    goto IL_bac7;
                }
                if (npc.velocity.X > 0f && num197 == 1)
                {
                    goto IL_bac7;
                }
                goto IL_bdce;
            }
            if (flag50)
            {
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
            }
            goto IL_c227;
        IL_3498:
            npc.velocity.X = npc.velocity.X * 1.75f;
            npc.velocity.Y = npc.velocity.Y - 4.5f;
            if (npc.Center.Y - Main.player[npc.target].Center.Y > 20f)
            {
                npc.velocity.Y = npc.velocity.Y - 0.5f;
            }
            if (npc.Center.Y - Main.player[npc.target].Center.Y > 40f)
            {
                npc.velocity.Y = npc.velocity.Y - 1f;
            }
            if (npc.Center.Y - Main.player[npc.target].Center.Y > 80f)
            {
                npc.velocity.Y = npc.velocity.Y - 1.5f;
            }
            if (npc.Center.Y - Main.player[npc.target].Center.Y > 100f)
            {
                npc.velocity.Y = npc.velocity.Y - 1.5f;
            }
            if (Math.Abs(npc.velocity.X) > 7f)
            {
                if (npc.velocity.X < 0f)
                {
                    npc.velocity.X = -7f;
                }
                else
                {
                    npc.velocity.X = 7f;
                }
            }
            goto IL_362f;
        IL_8a60:
            if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
            {
                Vector2 vector31 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + 20f);
                vector31.X += (float)(10 * npc.direction);
                float num286 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector31.X;
                float num285 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector31.Y;
                num286 += (float)Main.rand.Next(-40, 41);
                num285 += (float)Main.rand.Next(-40, 41);
                float num282 = (float)Math.Sqrt((double)(num286 * num286 + num285 * num285));
                npc.netUpdate = true;
                num282 = 15f / num282;
                num286 *= num282;
                num285 *= num282;
                int num278 = 32;
                int num277 = 257;
                vector31.X += num286 * 3f;
                vector31.Y += num285 * 3f;
                Projectile.NewProjectile(vector31.X, vector31.Y, num286, num285, ProjectileID.WoodenArrowHostile, damage, 0f, Main.myPlayer, 0f, 0f);
                npc.ai[2] = 0f;
            }
            goto IL_8c30;
        IL_8c30:
            if (npc.type == 251)
            {
                if (npc.justHit)
                {
                    npc.ai[2] -= (float)Main.rand.Next(30);
                }
                if (npc.ai[2] < 0f)
                {
                    npc.ai[2] = 0f;
                }
                if (npc.confused)
                {
                    npc.ai[2] = 0f;
                }
                npc.ai[2] += 1f;
                float num276 = (float)Main.rand.Next(60, 1800);
                num276 *= (float)npc.life / (float)npc.lifeMax;
                num276 += 15f;
                if (Main.netMode != 1 && npc.ai[2] >= num276 && npc.velocity.Y == 0f && !Main.player[npc.target].dead && !Main.player[npc.target].frozen)
                {
                    if (npc.direction > 0 && npc.Center.X < Main.player[npc.target].Center.X)
                    {
                        goto IL_8d96;
                    }
                    if (npc.direction < 0 && npc.Center.X > Main.player[npc.target].Center.X)
                    {
                        goto IL_8d96;
                    }
                }
            }
            goto IL_8f61;
        IL_2dca:
            bool flag51 = true;
            goto IL_2dcc;
        IL_a47a:
            if (npc.type == 468 && !Main.eclipse)
            {
                flag39 = true;
            }
            else if ((npc.ai[2] <= 0f | flag39) && (npc.velocity.Y == 0f | flag38) && npc.ai[1] <= 0f && !Main.player[npc.target].dead)
            {
                bool flag35 = Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height);
                if (npc.type == 520)
                {
                    flag35 = Collision.CanHitLine(npc.Top + new Vector2(0f, 20f), 0, 0, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height);
                }
                if (Main.player[npc.target].stealth == 0f && Main.player[npc.target].itemAnimation == 0)
                {
                    flag35 = false;
                }
                if (flag35)
                {
                    float num225 = 10f;
                    Vector2 vector23 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                    float num224 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector23.X;
                    float num223 = Math.Abs(num224) * 0.1f;
                    float num222 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector23.Y - num223;
                    num224 += (float)Main.rand.Next(-40, 41);
                    num222 += (float)Main.rand.Next(-40, 41);
                    float num219 = (float)Math.Sqrt((double)(num224 * num224 + num222 * num222));
                    float num218 = 700f;
                    if (npc.type == 214)
                    {
                        num218 = 550f;
                    }
                    if (npc.type == 215)
                    {
                        num218 = 800f;
                    }
                    if (npc.type >= 498 && npc.type <= 506)
                    {
                        num218 = 190f;
                    }
                    if (npc.type >= 449 && npc.type <= 452)
                    {
                        num218 = 200f;
                    }
                    if (npc.type == 481)
                    {
                        num218 = 400f;
                    }
                    if (npc.type == 468)
                    {
                        num218 = 400f;
                    }
                    if (num219 < num218)
                    {
                        npc.netUpdate = true;
                        npc.velocity.X = npc.velocity.X * 0.5f;
                        num219 = num225 / num219;
                        num224 *= num219;
                        num222 *= num219;
                        npc.ai[2] = 3f;
                        npc.ai[1] = (float)num250;
                        if (Math.Abs(num222) > Math.Abs(num224) * 2f)
                        {
                            if (num222 > 0f)
                            {
                                npc.ai[2] = 1f;
                            }
                            else
                            {
                                npc.ai[2] = 5f;
                            }
                        }
                        else if (Math.Abs(num224) > Math.Abs(num222) * 2f)
                        {
                            npc.ai[2] = 3f;
                        }
                        else if (num222 > 0f)
                        {
                            npc.ai[2] = 2f;
                        }
                        else
                        {
                            npc.ai[2] = 4f;
                        }
                    }
                }
            }
            if (npc.ai[2] <= 0f || (flag39 && (num252 == -1 || npc.ai[1] < (float)num252 || npc.ai[1] >= (float)(num252 + num251))))
            {
                float num214 = 1f;
                float num213 = 0.07f;
                float scaleFactor6 = 0.8f;
                if (npc.type == 214)
                {
                    num214 = 2f;
                    num213 = 0.09f;
                }
                else if (npc.type == 215)
                {
                    num214 = 1.5f;
                    num213 = 0.08f;
                }
                else if (npc.type == 381 || npc.type == 382)
                {
                    num214 = 2f;
                    num213 = 0.5f;
                }
                else if (npc.type == 520)
                {
                    num214 = 4f;
                    num213 = 1f;
                    scaleFactor6 = 0.7f;
                }
                else if (npc.type == 411)
                {
                    num214 = 2f;
                    num213 = 0.5f;
                }
                else if (npc.type == 409)
                {
                    num214 = 2f;
                    num213 = 0.5f;
                }
                bool flag34 = false;
                if ((npc.type == 381 || npc.type == 382) && Vector2.Distance(npc.Center, Main.player[npc.target].Center) < 300f && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
                {
                    flag34 = true;
                    npc.ai[3] = 0f;
                }
                if (npc.type == 520 && Vector2.Distance(npc.Center, Main.player[npc.target].Center) < 400f && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
                {
                    flag34 = true;
                    npc.ai[3] = 0f;
                }
                if ((npc.velocity.X < 0f - num214 || npc.velocity.X > num214) | flag34)
                {
                    if (npc.velocity.Y == 0f)
                    {
                        npc.velocity *= scaleFactor6;
                    }
                }
                else if (npc.velocity.X < num214 && npc.direction == 1)
                {
                    npc.velocity.X = npc.velocity.X + num213;
                    if (npc.velocity.X > num214)
                    {
                        npc.velocity.X = num214;
                    }
                }
                else if (npc.velocity.X > 0f - num214 && npc.direction == -1)
                {
                    npc.velocity.X = npc.velocity.X - num213;
                    if (npc.velocity.X < 0f - num214)
                    {
                        npc.velocity.X = 0f - num214;
                    }
                }
            }
            if (npc.type == 520)
            {
                npc.localAI[2] += 1f;
                if (npc.localAI[2] >= 6f)
                {
                    npc.localAI[2] = 0f;
                    npc.localAI[3] = Main.player[npc.target].DirectionFrom(npc.Top + new Vector2(0f, 20f)).ToRotation();
                }
            }
            goto IL_aba2;
        IL_c15a:
            if (npc.type == 287 && npc.velocity.Y < 0f)
            {
                npc.velocity.X = npc.velocity.X * 1.2f;
                npc.velocity.Y = npc.velocity.Y * 1.1f;
            }
            if (npc.type == 460 && npc.velocity.Y < 0f)
            {
                npc.velocity.X = npc.velocity.X * 1.3f;
                npc.velocity.Y = npc.velocity.Y * 1.1f;
            }
            goto IL_c227;
        IL_a3fa:
            npc.ai[2] = 0f;
            npc.ai[1] = 0f;
            goto IL_a47a;
        IL_1c13:
            if (Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
            {
                npc.ai[2] = -1f;
                npc.netUpdate = true;
                npc.TargetClosest(true);
            }
            goto IL_1e03;
        IL_8d96:
            if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
            {
                Vector2 vector32 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + 12f);
                vector32.X += (float)(6 * npc.direction);
                float num273 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector32.X;
                float num272 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector32.Y;
                num273 += (float)Main.rand.Next(-40, 41);
                num272 += (float)Main.rand.Next(-30, 0);
                float num269 = (float)Math.Sqrt((double)(num273 * num273 + num272 * num272));
                npc.netUpdate = true;
                num269 = 15f / num269;
                num273 *= num269;
                num272 *= num269;
                int num265 = 30;
                int num264 = 83;
                vector32.X += num273 * 3f;
                vector32.Y += num272 * 3f;
                Projectile.NewProjectile(vector32.X, vector32.Y, num273, num272, ProjectileID.WoodenArrowHostile, damage, 0f, Main.myPlayer, 0f, 0f);
                npc.ai[2] = 0f;
            }
            goto IL_8f61;
        IL_41d5:
            if (npc.velocity.X < -6f || npc.velocity.X > 6f)
            {
                if (npc.velocity.Y == 0f)
                {
                    npc.velocity *= 0.8f;
                }
            }
            else if (npc.velocity.X < 6f && npc.direction == 1)
            {
                if (npc.velocity.Y == 0f && npc.velocity.X < 0f)
                {
                    npc.velocity.X = npc.velocity.X * 0.99f;
                }
                npc.velocity.X = npc.velocity.X + 0.07f;
                if (npc.velocity.X > 6f)
                {
                    npc.velocity.X = 6f;
                }
            }
            else if (npc.velocity.X > -6f && npc.direction == -1)
            {
                if (npc.velocity.Y == 0f && npc.velocity.X > 0f)
                {
                    npc.velocity.X = npc.velocity.X * 0.99f;
                }
                npc.velocity.X = npc.velocity.X - 0.07f;
                if (npc.velocity.X < -6f)
                {
                    npc.velocity.X = -6f;
                }
            }
            goto IL_6bf9;
        IL_c227:
            if (Main.netMode != 1 && npc.type == 120 && npc.ai[3] >= (float)num382)
            {
                int num196 = (int)Main.player[npc.target].position.X / 16;
                int num195 = (int)Main.player[npc.target].position.Y / 16;
                int num194 = (int)npc.position.X / 16;
                int num193 = (int)npc.position.Y / 16;
                int num192 = 20;
                int num191 = 0;
                bool flag29 = false;
                if (Math.Abs(npc.position.X - Main.player[npc.target].position.X) + Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000f)
                {
                    num191 = 100;
                    flag29 = true;
                }
                while (!flag29 && num191 < 100)
                {
                    num191++;
                    int num190 = Main.rand.Next(num196 - num192, num196 + num192);
                    for (int num189 = Main.rand.Next(num195 - num192, num195 + num192); num189 < num195 + num192; num189++)
                    {
                        if ((num189 < num195 - 4 || num189 > num195 + 4 || num190 < num196 - 4 || num190 > num196 + 4) && (num189 < num193 - 1 || num189 > num193 + 1 || num190 < num194 - 1 || num190 > num194 + 1) && Main.tile[num190, num189].nactive())
                        {
                            bool flag28 = true;
                            if (npc.type == 32 && Main.tile[num190, num189 - 1].wall == 0)
                            {
                                flag28 = false;
                            }
                            else if (Main.tile[num190, num189 - 1].lava())
                            {
                                flag28 = false;
                            }
                            if (flag28 && Main.tileSolid[Main.tile[num190, num189].type] && !Collision.SolidTiles(num190 - 1, num190 + 1, num189 - 4, num189 - 1))
                            {
                                npc.position.X = (float)(num190 * 16 - npc.width / 2);
                                npc.position.Y = (float)(num189 * 16 - npc.height);
                                npc.netUpdate = true;
                                npc.ai[3] = -120f;
                            }
                        }
                    }
                }
            }
            return;
        IL_bac7:
            if (npc.height >= 32 && Main.tile[num200, num199 - 2].nactive() && Main.tileSolid[Main.tile[num200, num199 - 2].type])
            {
                if (Main.tile[num200, num199 - 3].nactive() && Main.tileSolid[Main.tile[num200, num199 - 3].type])
                {
                    npc.velocity.Y = -8f;
                    npc.netUpdate = true;
                }
                else
                {
                    npc.velocity.Y = -7f;
                    npc.netUpdate = true;
                }
            }
            else if (Main.tile[num200, num199 - 1].nactive() && Main.tileSolid[Main.tile[num200, num199 - 1].type])
            {
                npc.velocity.Y = -6f;
                npc.netUpdate = true;
            }
            else if (npc.position.Y + (float)npc.height - (float)(num199 * 16) > 20f && Main.tile[num200, num199].nactive() && !Main.tile[num200, num199].topSlope() && Main.tileSolid[Main.tile[num200, num199].type])
            {
                npc.velocity.Y = -5f;
                npc.netUpdate = true;
            }
            else if (npc.directionY < 0 && npc.type != 67 && (!Main.tile[num200, num199 + 1].nactive() || !Main.tileSolid[Main.tile[num200, num199 + 1].type]) && (!Main.tile[num200 + npc.direction, num199 + 1].nactive() || !Main.tileSolid[Main.tile[num200 + npc.direction, num199 + 1].type]))
            {
                npc.velocity.Y = -8f;
                npc.velocity.X = npc.velocity.X * 1.5f;
                npc.netUpdate = true;
            }
            else if (flag50)
            {
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
            }
            bool flag52;
            if ((npc.velocity.Y == 0f & flag52) && npc.ai[3] == 1f)
            {
                npc.velocity.Y = -5f;
            }
            goto IL_bdce;
        IL_c12f:
            npc.velocity.X = (float)(8 * npc.direction);
            npc.velocity.Y = -4f;
            npc.netUpdate = true;
            goto IL_c15a;
        IL_2dcc:
            if ((npc.position.X == npc.oldPosition.X || npc.ai[3] >= (float)num382) | flag51)
            {
                npc.ai[3] += 1f;
            }
            else if ((double)Math.Abs(npc.velocity.X) > 0.9 && npc.ai[3] > 0f)
            {
                npc.ai[3] -= 1f;
            }
            if (npc.ai[3] > (float)(num382 * 10))
            {
                npc.ai[3] = 0f;
            }
            if (npc.justHit)
            {
                npc.ai[3] = 0f;
            }
            if (npc.ai[3] == (float)num382)
            {
                npc.netUpdate = true;
            }
            goto IL_2e95;
        IL_6bf9:
            if (npc.type >= 277 && npc.type <= 280)
            {
                Lighting.AddLight((int)npc.Center.X / 16, (int)npc.Center.Y / 16, 0.2f, 0.1f, 0f);
            }
            else if (npc.type == 520)
            {
                Lighting.AddLight(npc.Top + new Vector2(0f, 20f), 0.3f, 0.3f, 0.7f);
            }
            else if (npc.type == 525)
            {
                Vector3 rgb5 = new Vector3(0.7f, 1f, 0.2f) * 0.5f;
                Lighting.AddLight(npc.Top + new Vector2(0f, 15f), rgb5);
            }
            else if (npc.type == 526)
            {
                Vector3 rgb4 = new Vector3(1f, 1f, 0.5f) * 0.4f;
                Lighting.AddLight(npc.Top + new Vector2(0f, 15f), rgb4);
            }
            else if (npc.type == 527)
            {
                Vector3 rgb3 = new Vector3(0.6f, 0.3f, 1f) * 0.4f;
                Lighting.AddLight(npc.Top + new Vector2(0f, 15f), rgb3);
            }
            else if (npc.type == 415)
            {
                npc.hide = false;
                int num323 = 0;
                while (num323 < 200)
                {
                    if (!Main.npc[num323].active || Main.npc[num323].type != 416 || Main.npc[num323].ai[0] != (float)npc.whoAmI)
                    {
                        num323++;
                        continue;
                    }
                    npc.hide = true;
                    break;
                }
            }
            else if (npc.type == 258)
            {
                if (npc.velocity.Y != 0f)
                {
                    npc.TargetClosest(true);
                    npc.spriteDirection = npc.direction;
                    if (Main.player[npc.target].Center.X < npc.position.X && npc.velocity.X > 0f)
                    {
                        npc.velocity.X = npc.velocity.X * 0.95f;
                    }
                    else if (Main.player[npc.target].Center.X > npc.position.X + (float)npc.width && npc.velocity.X < 0f)
                    {
                        npc.velocity.X = npc.velocity.X * 0.95f;
                    }
                    if (Main.player[npc.target].Center.X < npc.position.X && npc.velocity.X > -5f)
                    {
                        npc.velocity.X = npc.velocity.X - 0.1f;
                    }
                    else if (Main.player[npc.target].Center.X > npc.position.X + (float)npc.width && npc.velocity.X < 5f)
                    {
                        npc.velocity.X = npc.velocity.X + 0.1f;
                    }
                }
                else if (Main.player[npc.target].Center.Y + 50f < npc.position.Y && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                {
                    npc.velocity.Y = -7f;
                }
            }
            else if (npc.type == 425)
            {
                if (npc.velocity.Y == 0f)
                {
                    npc.ai[2] = 0f;
                }
                if (npc.velocity.Y != 0f && npc.ai[2] == 1f)
                {
                    npc.TargetClosest(true);
                    npc.spriteDirection = -npc.direction;
                    if (Collision.CanHit(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
                    {
                        float num322 = Main.player[npc.target].Center.X - (float)(npc.direction * 400) - npc.Center.X;
                        float num321 = Main.player[npc.target].Bottom.Y - npc.Bottom.Y;
                        if (num322 < 0f && npc.velocity.X > 0f)
                        {
                            npc.velocity.X = npc.velocity.X * 0.9f;
                        }
                        else if (num322 > 0f && npc.velocity.X < 0f)
                        {
                            npc.velocity.X = npc.velocity.X * 0.9f;
                        }
                        if (num322 < 0f && npc.velocity.X > -5f)
                        {
                            npc.velocity.X = npc.velocity.X - 0.1f;
                        }
                        else if (num322 > 0f && npc.velocity.X < 5f)
                        {
                            npc.velocity.X = npc.velocity.X + 0.1f;
                        }
                        if (npc.velocity.X > 6f)
                        {
                            npc.velocity.X = 6f;
                        }
                        if (npc.velocity.X < -6f)
                        {
                            npc.velocity.X = -6f;
                        }
                        if (num321 < -20f && npc.velocity.Y > 0f)
                        {
                            npc.velocity.Y = npc.velocity.Y * 0.8f;
                        }
                        else if (num321 > 20f && npc.velocity.Y < 0f)
                        {
                            npc.velocity.Y = npc.velocity.Y * 0.8f;
                        }
                        if (num321 < -20f && npc.velocity.Y > -5f)
                        {
                            npc.velocity.Y = npc.velocity.Y - 0.3f;
                        }
                        else if (num321 > 20f && npc.velocity.Y < 5f)
                        {
                            npc.velocity.Y = npc.velocity.Y + 0.3f;
                        }
                    }
                    if (Main.rand.Next(3) == 0)
                    {
                        Vector2 position = npc.Center + new Vector2((float)(npc.direction * -14), -8f) - Vector2.One * 4f;
                        Vector2 velocity = new Vector2((float)(npc.direction * -6), 12f) * 0.2f + Utils.RandomVector2(Main.rand, -1f, 1f) * 0.1f;
                        Dust obj4 = Main.dust[Dust.NewDust(position, 8, 8, 229, velocity.X, velocity.Y, 100, Color.Transparent, 1f + Main.rand.NextFloat() * 0.5f)];
                        obj4.noGravity = true;
                        obj4.velocity = velocity;
                        obj4.customData = npc;
                    }
                    for (int num320 = 0; num320 < 200; num320++)
                    {
                        if (num320 != npc.whoAmI && Main.npc[num320].active && Main.npc[num320].type == npc.type && Math.Abs(npc.position.X - Main.npc[num320].position.X) + Math.Abs(npc.position.Y - Main.npc[num320].position.Y) < (float)npc.width)
                        {
                            if (npc.position.X < Main.npc[num320].position.X)
                            {
                                npc.velocity.X = npc.velocity.X - 0.05f;
                            }
                            else
                            {
                                npc.velocity.X = npc.velocity.X + 0.05f;
                            }
                            if (npc.position.Y < Main.npc[num320].position.Y)
                            {
                                npc.velocity.Y = npc.velocity.Y - 0.05f;
                            }
                            else
                            {
                                npc.velocity.Y = npc.velocity.Y + 0.05f;
                            }
                        }
                    }
                }
                else if (Main.player[npc.target].Center.Y + 100f < npc.position.Y && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                {
                    npc.velocity.Y = -5f;
                    npc.ai[2] = 1f;
                }
                if (Main.netMode != 1)
                {
                    npc.localAI[2] += 1f;
                    if (npc.localAI[2] >= (float)(360 + Main.rand.Next(360)) && npc.Distance(Main.player[npc.target].Center) < 400f && Math.Abs(npc.DirectionTo(Main.player[npc.target].Center).Y) < 0.5f && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
                    {
                        npc.localAI[2] = 0f;
                        Vector2 vector29 = npc.Center + new Vector2((float)(npc.direction * 30), 2f);
                        Vector2 vector28 = npc.DirectionTo(Main.player[npc.target].Center) * 7f;
                        if (vector28.HasNaNs())
                        {
                            vector28 = new Vector2((float)(npc.direction * 8), 0f);
                        }
                        int num319 = Main.expertMode ? 50 : 75;
                        for (int num318 = 0; num318 < 4; num318++)
                        {
                            Vector2 vector27 = vector28 + Utils.RandomVector2(Main.rand, -0.8f, 0.8f);
                            Projectile.NewProjectile(vector29.X, vector29.Y, vector27.X, vector27.Y, ProjectileID.WoodenArrowHostile, damage, 1f, Main.myPlayer, 0f, 0f);
                        }
                    }
                }
            }
            else if (npc.type == 427)
            {
                if (npc.velocity.Y == 0f)
                {
                    npc.ai[2] = 0f;
                    npc.rotation = 0f;
                }
                else
                {
                    npc.rotation = npc.velocity.X * 0.1f;
                }
                if (npc.velocity.Y != 0f && npc.ai[2] == 1f)
                {
                    npc.TargetClosest(true);
                    npc.spriteDirection = -npc.direction;
                    if (Collision.CanHit(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
                    {
                        float num317 = Main.player[npc.target].Center.X - npc.Center.X;
                        float num316 = Main.player[npc.target].Center.Y - npc.Center.Y;
                        if (num317 < 0f && npc.velocity.X > 0f)
                        {
                            npc.velocity.X = npc.velocity.X * 0.98f;
                        }
                        else if (num317 > 0f && npc.velocity.X < 0f)
                        {
                            npc.velocity.X = npc.velocity.X * 0.98f;
                        }
                        if (num317 < -20f && npc.velocity.X > -6f)
                        {
                            npc.velocity.X = npc.velocity.X - 0.015f;
                        }
                        else if (num317 > 20f && npc.velocity.X < 6f)
                        {
                            npc.velocity.X = npc.velocity.X + 0.015f;
                        }
                        if (npc.velocity.X > 6f)
                        {
                            npc.velocity.X = 6f;
                        }
                        if (npc.velocity.X < -6f)
                        {
                            npc.velocity.X = -6f;
                        }
                        if (num316 < -20f && npc.velocity.Y > 0f)
                        {
                            npc.velocity.Y = npc.velocity.Y * 0.98f;
                        }
                        else if (num316 > 20f && npc.velocity.Y < 0f)
                        {
                            npc.velocity.Y = npc.velocity.Y * 0.98f;
                        }
                        if (num316 < -20f && npc.velocity.Y > -6f)
                        {
                            npc.velocity.Y = npc.velocity.Y - 0.15f;
                        }
                        else if (num316 > 20f && npc.velocity.Y < 6f)
                        {
                            npc.velocity.Y = npc.velocity.Y + 0.15f;
                        }
                    }
                    for (int num315 = 0; num315 < 200; num315++)
                    {
                        if (num315 != npc.whoAmI && Main.npc[num315].active && Main.npc[num315].type == npc.type && Math.Abs(npc.position.X - Main.npc[num315].position.X) + Math.Abs(npc.position.Y - Main.npc[num315].position.Y) < (float)npc.width)
                        {
                            if (npc.position.X < Main.npc[num315].position.X)
                            {
                                npc.velocity.X = npc.velocity.X - 0.05f;
                            }
                            else
                            {
                                npc.velocity.X = npc.velocity.X + 0.05f;
                            }
                            if (npc.position.Y < Main.npc[num315].position.Y)
                            {
                                npc.velocity.Y = npc.velocity.Y - 0.05f;
                            }
                            else
                            {
                                npc.velocity.Y = npc.velocity.Y + 0.05f;
                            }
                        }
                    }
                }
                else if (Main.player[npc.target].Center.Y + 100f < npc.position.Y && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                {
                    npc.velocity.Y = -5f;
                    npc.ai[2] = 1f;
                }
            }
            else if (npc.type == 426)
            {
                if (npc.ai[1] > 0f && npc.velocity.Y > 0f)
                {
                    npc.velocity.Y = npc.velocity.Y * 0.85f;
                    if (npc.velocity.Y == 0f)
                    {
                        npc.velocity.Y = -0.4f;
                    }
                }
                if (npc.velocity.Y != 0f)
                {
                    npc.TargetClosest(true);
                    npc.spriteDirection = npc.direction;
                    if (Collision.CanHit(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
                    {
                        float num314 = Main.player[npc.target].Center.X - (float)(npc.direction * 300) - npc.Center.X;
                        if (num314 < 40f && npc.velocity.X > 0f)
                        {
                            npc.velocity.X = npc.velocity.X * 0.98f;
                        }
                        else if (num314 > 40f && npc.velocity.X < 0f)
                        {
                            npc.velocity.X = npc.velocity.X * 0.98f;
                        }
                        if (num314 < 40f && npc.velocity.X > -5f)
                        {
                            npc.velocity.X = npc.velocity.X - 0.2f;
                        }
                        else if (num314 > 40f && npc.velocity.X < 5f)
                        {
                            npc.velocity.X = npc.velocity.X + 0.2f;
                        }
                        if (npc.velocity.X > 6f)
                        {
                            npc.velocity.X = 6f;
                        }
                        if (npc.velocity.X < -6f)
                        {
                            npc.velocity.X = -6f;
                        }
                    }
                }
                else if (Main.player[npc.target].Center.Y + 100f < npc.position.Y && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                {
                    npc.velocity.Y = -6f;
                }
                for (int num313 = 0; num313 < 200; num313++)
                {
                    if (num313 != npc.whoAmI && Main.npc[num313].active && Main.npc[num313].type == npc.type && Math.Abs(npc.position.X - Main.npc[num313].position.X) + Math.Abs(npc.position.Y - Main.npc[num313].position.Y) < (float)npc.width)
                    {
                        if (npc.position.X < Main.npc[num313].position.X)
                        {
                            npc.velocity.X = npc.velocity.X - 0.1f;
                        }
                        else
                        {
                            npc.velocity.X = npc.velocity.X + 0.1f;
                        }
                        if (npc.position.Y < Main.npc[num313].position.Y)
                        {
                            npc.velocity.Y = npc.velocity.Y - 0.1f;
                        }
                        else
                        {
                            npc.velocity.Y = npc.velocity.Y + 0.1f;
                        }
                    }
                }
                if (Main.rand.Next(6) == 0 && npc.ai[1] <= 20f)
                {
                    Dust obj5 = Main.dust[Dust.NewDust(npc.Center + new Vector2((float)((npc.spriteDirection == 1) ? 8 : (-20)), -20f), 8, 8, 229, npc.velocity.X, npc.velocity.Y, 100, default(Color), 1f)];
                    obj5.velocity = obj5.velocity / 4f + npc.velocity / 2f;
                    obj5.scale = 0.6f;
                    obj5.noLight = true;
                }
                if (npc.ai[1] >= 57f)
                {
                    int num312 = Utils.SelectRandom(Main.rand, 161, 229);
                    Dust obj6 = Main.dust[Dust.NewDust(npc.Center + new Vector2((float)((npc.spriteDirection == 1) ? 8 : (-20)), -20f), 8, 8, num312, npc.velocity.X, npc.velocity.Y, 100, default(Color), 1f)];
                    obj6.velocity = obj6.velocity / 4f + npc.DirectionTo(Main.player[npc.target].Top);
                    obj6.scale = 1.2f;
                    obj6.noLight = true;
                }
                if (Main.rand.Next(6) == 0)
                {
                    Dust dust9 = Main.dust[Dust.NewDust(npc.Center, 2, 2, 229, 0f, 0f, 0, default(Color), 1f)];
                    dust9.position = npc.Center + new Vector2((float)((npc.spriteDirection == 1) ? 26 : (-26)), 24f);
                    dust9.velocity.X = 0f;
                    if (dust9.velocity.Y < 0f)
                    {
                        dust9.velocity.Y = 0f;
                    }
                    dust9.noGravity = true;
                    dust9.scale = 1f;
                    dust9.noLight = true;
                }
            }
            else if (npc.type == 185)
            {
                if (npc.velocity.Y == 0f)
                {
                    npc.rotation = 0f;
                    npc.localAI[0] = 0f;
                }
                else if (npc.localAI[0] == 1f)
                {
                    npc.rotation += npc.velocity.X * 0.05f;
                }
            }
            else if (npc.type == 428)
            {
                if (npc.velocity.Y == 0f)
                {
                    npc.rotation = 0f;
                }
                else
                {
                    npc.rotation += npc.velocity.X * 0.08f;
                }
            }
            if (npc.type == 159 && Main.netMode != 1)
            {
                Vector2 vector26 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                float num434 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector26.X;
                float num310 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector26.Y;
                if ((float)Math.Sqrt((double)(num434 * num434 + num310 * num310)) > 300f)
                {
                    npc.Transform(158);
                }
            }
            if (npc.type == 164 && Main.netMode != 1 && npc.velocity.Y == 0f)
            {
                int num309 = (int)npc.Center.X / 16;
                int num308 = (int)npc.Center.Y / 16;
                bool flag46 = false;
                for (int num307 = num309 - 1; num307 <= num309 + 1; num307++)
                {
                    for (int num306 = num308 - 1; num306 <= num308 + 1; num306++)
                    {
                        if (Main.tile[num307, num306].wall > 0)
                        {
                            flag46 = true;
                        }
                    }
                }
                if (flag46)
                {
                    npc.Transform(165);
                }
            }
            if (npc.type == 239 && Main.netMode != 1 && npc.velocity.Y == 0f)
            {
                int num305 = (int)npc.Center.X / 16;
                int num304 = (int)npc.Center.Y / 16;
                bool flag45 = false;
                for (int num303 = num305 - 1; num303 <= num305 + 1; num303++)
                {
                    for (int num302 = num304 - 1; num302 <= num304 + 1; num302++)
                    {
                        if (Main.tile[num303, num302].wall > 0)
                        {
                            flag45 = true;
                        }
                    }
                }
                if (flag45)
                {
                    npc.Transform(240);
                }
            }
            if (npc.type == 530 && Main.netMode != 1 && npc.velocity.Y == 0f)
            {
                int num301 = (int)npc.Center.X / 16;
                int num300 = (int)npc.Center.Y / 16;
                bool flag44 = false;
                for (int num299 = num301 - 1; num299 <= num301 + 1; num299++)
                {
                    for (int num298 = num300 - 1; num298 <= num300 + 1; num298++)
                    {
                        if (Main.tile[num299, num298].wall > 0)
                        {
                            flag44 = true;
                        }
                    }
                }
                if (flag44)
                {
                    npc.Transform(531);
                }
            }
            if (Main.netMode != 1 && Main.expertMode && npc.target >= 0 && (npc.type == 163 || npc.type == 238) && Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
            {
                npc.localAI[0] += 1f;
                if (npc.justHit)
                {
                    npc.localAI[0] -= (float)Main.rand.Next(20, 60);
                    if (npc.localAI[0] < 0f)
                    {
                        npc.localAI[0] = 0f;
                    }
                }
                if (npc.localAI[0] > (float)Main.rand.Next(180, 900))
                {
                    npc.localAI[0] = 0f;
                    Vector2 vector30 = Main.player[npc.target].Center - npc.Center;
                    vector30.Normalize();
                    vector30 *= 8f;
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, vector30.X, vector30.Y, ProjectileID.WoodenArrowHostile, damage, 0f, Main.myPlayer, 0f, 0f);
                }
            }
            if (npc.type == 163 && Main.netMode != 1 && npc.velocity.Y == 0f)
            {
                int num297 = (int)npc.Center.X / 16;
                int num296 = (int)npc.Center.Y / 16;
                bool flag43 = false;
                for (int num295 = num297 - 1; num295 <= num297 + 1; num295++)
                {
                    for (int num294 = num296 - 1; num294 <= num296 + 1; num294++)
                    {
                        if (Main.tile[num295, num294].wall > 0)
                        {
                            flag43 = true;
                        }
                    }
                }
                if (flag43)
                {
                    npc.Transform(238);
                }
            }
            if (npc.type == 236 && Main.netMode != 1 && npc.velocity.Y == 0f)
            {
                int num293 = (int)npc.Center.X / 16;
                int num292 = (int)npc.Center.Y / 16;
                bool flag42 = false;
                for (int num291 = num293 - 1; num291 <= num293 + 1; num291++)
                {
                    for (int num290 = num292 - 1; num290 <= num292 + 1; num290++)
                    {
                        if (Main.tile[num291, num290].wall > 0)
                        {
                            flag42 = true;
                        }
                    }
                }
                if (flag42)
                {
                    npc.Transform(237);
                }
            }
            if (npc.type == 243)
            {
                if (npc.justHit && Main.rand.Next(3) == 0)
                {
                    npc.ai[2] -= (float)Main.rand.Next(30);
                }
                if (npc.ai[2] < 0f)
                {
                    npc.ai[2] = 0f;
                }
                if (npc.confused)
                {
                    npc.ai[2] = 0f;
                }
                npc.ai[2] += 1f;
                float num289 = (float)Main.rand.Next(30, 900);
                num289 *= (float)npc.life / (float)npc.lifeMax;
                num289 += 30f;
                if (Main.netMode != 1 && npc.ai[2] >= num289 && npc.velocity.Y == 0f && !Main.player[npc.target].dead && !Main.player[npc.target].frozen)
                {
                    if (npc.direction > 0 && npc.Center.X < Main.player[npc.target].Center.X)
                    {
                        goto IL_8a60;
                    }
                    if (npc.direction < 0 && npc.Center.X > Main.player[npc.target].Center.X)
                    {
                        goto IL_8a60;
                    }
                }
            }
            goto IL_8c30;
        IL_21b6:
            if (npc.type == 428)
            {
                npc.localAI[0] += 1f;
                if (npc.localAI[0] >= 300f)
                {
                    int num435 = (int)npc.Center.X / 16 - 1;
                    int num386 = (int)npc.Center.Y / 16 - 1;
                    if (!Collision.SolidTiles(num435, num435 + 2, num386, num386 + 1) && Main.netMode != 1)
                    {
                        npc.Transform(427);
                        npc.life = npc.lifeMax;
                        npc.localAI[0] = 0f;
                        return;
                    }
                }
                int maxValue3 = (!(npc.localAI[0] < 60f)) ? ((!(npc.localAI[0] < 120f)) ? ((!(npc.localAI[0] < 180f)) ? ((!(npc.localAI[0] < 240f)) ? ((!(npc.localAI[0] < 300f)) ? 1 : 1) : 2) : 4) : 8) : 16;
                if (Main.rand.Next(maxValue3) == 0)
                {
                    Dust dust10 = Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, 229, 0f, 0f, 0, default(Color), 1f)];
                    dust10.noGravity = true;
                    dust10.scale = 1f;
                    dust10.noLight = true;
                    dust10.velocity = npc.DirectionFrom(dust10.position) * dust10.velocity.Length();
                    Dust dust11 = dust10;
                    dust11.position -= dust10.velocity * 5f;
                    Dust expr_23FC_cp_0 = dust10;
                    expr_23FC_cp_0.position.X = expr_23FC_cp_0.position.X + (float)(npc.direction * 6);
                    Dust expr_2418_cp_0 = dust10;
                    expr_2418_cp_0.position.Y = expr_2418_cp_0.position.Y + 4f;
                }
            }
            if (npc.type == 427)
            {
                npc.localAI[0] += 1f;
                npc.localAI[0] += Math.Abs(npc.velocity.X) / 2f;
                if (npc.localAI[0] >= 1200f && Main.netMode != 1)
                {
                    int num436 = (int)npc.Center.X / 16 - 2;
                    int num384 = (int)npc.Center.Y / 16 - 3;
                    if (!Collision.SolidTiles(num436, num436 + 4, num384, num384 + 4))
                    {
                        npc.Transform(426);
                        npc.life = npc.lifeMax;
                        npc.localAI[0] = 0f;
                        return;
                    }
                }
                int maxValue2 = (!(npc.localAI[0] < 360f)) ? ((!(npc.localAI[0] < 720f)) ? ((!(npc.localAI[0] < 1080f)) ? ((!(npc.localAI[0] < 1440f)) ? ((!(npc.localAI[0] < 1800f)) ? 1 : 1) : 2) : 6) : 16) : 32;
                if (Main.rand.Next(maxValue2) == 0)
                {
                    Dust obj7 = Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, 229, 0f, 0f, 0, default(Color), 1f)];
                    obj7.noGravity = true;
                    obj7.scale = 1f;
                    obj7.noLight = true;
                }
            }
            flag52 = false;
            if (npc.velocity.X == 0f)
            {
                flag52 = true;
            }
            if (npc.justHit)
            {
                flag52 = false;
            }
            if (Main.netMode != 1 && npc.type == 198 && (double)npc.life <= (double)npc.lifeMax * 0.55)
            {
                npc.Transform(199);
            }
            if (Main.netMode != 1 && npc.type == 348 && (double)npc.life <= (double)npc.lifeMax * 0.55)
            {
                npc.Transform(349);
            }
            num382 = 60;
            if (npc.type == 120)
            {
                num382 = 180;
                if (npc.ai[3] == -120f)
                {
                    npc.velocity *= 0f;
                    npc.ai[3] = 0f;
                    Main.PlaySound(SoundID.Item8, npc.position);
                    Vector2 vector33 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                    float num381 = npc.oldPos[2].X + (float)npc.width * 0.5f - vector33.X;
                    float num380 = npc.oldPos[2].Y + (float)npc.height * 0.5f - vector33.Y;
                    float num379 = (float)Math.Sqrt((double)(num381 * num381 + num380 * num380));
                    num379 = 2f / num379;
                    num381 *= num379;
                    num380 *= num379;
                    for (int j = 0; j < 20; j++)
                    {
                        int num373 = Dust.NewDust(npc.position, npc.width, npc.height, 71, num381, num380, 200, default(Color), 2f);
                        Main.dust[num373].noGravity = true;
                        Dust expr_27D4_cp_0 = Main.dust[num373];
                        expr_27D4_cp_0.velocity.X = expr_27D4_cp_0.velocity.X * 2f;
                    }
                    for (int i = 0; i < 20; i++)
                    {
                        int num375 = Dust.NewDust(npc.oldPos[2], npc.width, npc.height, 71, 0f - num381, 0f - num380, 200, default(Color), 2f);
                        Main.dust[num375].noGravity = true;
                        Dust expr_2855_cp_0 = Main.dust[num375];
                        expr_2855_cp_0.velocity.X = expr_2855_cp_0.velocity.X * 2f;
                    }
                }
            }
            flag51 = false;
            flag50 = true;
            if (npc.type == 343 || npc.type == 47 || npc.type == 67 || npc.type == 109 || npc.type == 110 || true || true || npc.type == 111 || npc.type == 120 || npc.type == 163 || npc.type == 164 || npc.type == 239 || npc.type == 168 || npc.type == 199 || npc.type == 206 || npc.type == 214 || npc.type == 215 || npc.type == 216 || npc.type == 217 || npc.type == 218 || npc.type == 219 || npc.type == 220 || npc.type == 226 || npc.type == 243 || npc.type == 251 || npc.type == 257 || npc.type == 258 || npc.type == 290 || npc.type == 291 || npc.type == 292 || npc.type == 293 || npc.type == 305 || npc.type == 306 || npc.type == 307 || npc.type == 308 || npc.type == 309 || npc.type == 348 || npc.type == 349 || npc.type == 350 || npc.type == 351 || npc.type == 379 || (npc.type >= 430 && npc.type <= 436) || npc.type == 380 || npc.type == 381 || npc.type == 382 || npc.type == 383 || npc.type == 386 || npc.type == 391 || (npc.type >= 449 && npc.type <= 452) || npc.type == 466 || npc.type == 464 || npc.type == 166 || npc.type == 469 || npc.type == 468 || npc.type == 471 || npc.type == 470 || npc.type == 480 || npc.type == 481 || npc.type == 482 || npc.type == 411 || npc.type == 424 || npc.type == 409 || (npc.type >= 494 && npc.type <= 506) || npc.type == 425 || npc.type == 427 || npc.type == 426 || npc.type == 428 || npc.type == 508 || npc.type == 415 || npc.type == 419 || npc.type == 520 || (npc.type >= 524 && npc.type <= 527) || npc.type == 528 || npc.type == 529 || npc.type == 530 || npc.type == 532)
            {
                flag50 = false;
            }
            flag49 = false;
            int num372 = npc.type;
            if (num372 == 425 || num372 == 471)
            {
                flag49 = true;
            }
            flag48 = true;
            num372 = npc.type;
            if (num372 <= 350)
            {
                if (num372 <= 206)
                {
                    if ((uint)(num372 - 110) > 1u && num372 != 206)
                    {
                        goto IL_2d75;
                    }
                }
                else if ((uint)(num372 - 214) > 2u && (uint)(num372 - 291) > 2u && num372 != 350)
                {
                    goto IL_2d75;
                }
            }
            else if (num372 <= 426)
            {
                if ((uint)(num372 - 379) > 3u)
                {
                    switch (num372)
                    {
                        case 409:
                        case 411:
                        case 424:
                        case 426:
                            break;
                        default:
                            goto IL_2d75;
                    }
                }
            }
            else if (num372 != 466 && (uint)(num372 - 498) > 8u && num372 != 520)
            {
                goto IL_2d75;
            }
            if (npc.ai[2] > 0f)
            {
                flag48 = false;
            }
            goto IL_2d75;
        IL_bfff:
            if (npc.type == 120 && npc.velocity.Y < 0f)
            {
                npc.velocity.Y = npc.velocity.Y * 1.1f;
            }
            if (npc.type == 287 && npc.velocity.Y == 0f && Math.Abs(npc.position.X + (float)(npc.width / 2) - (Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2))) < 150f && Math.Abs(npc.position.Y + (float)(npc.height / 2) - (Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2))) < 50f)
            {
                if (npc.direction > 0 && npc.velocity.X >= 1f)
                {
                    goto IL_c12f;
                }
                if (npc.direction < 0 && npc.velocity.X <= -1f)
                {
                    goto IL_c12f;
                }
            }
            goto IL_c15a;
        IL_bf88:
            npc.velocity.X = npc.velocity.X * 2f;
            if (npc.velocity.X > 3f)
            {
                npc.velocity.X = 3f;
            }
            if (npc.velocity.X < -3f)
            {
                npc.velocity.X = -3f;
            }
            npc.velocity.Y = -4f;
            npc.netUpdate = true;
            goto IL_bfff;
        IL_509d:
            npc.velocity.X = npc.velocity.X * 0.9f;
            goto IL_6bf9;
        IL_aba2:
            if (npc.type == 109 && Main.netMode != 1 && !Main.player[npc.target].dead)
            {
                if (npc.justHit)
                {
                    npc.ai[2] = 0f;
                }
                npc.ai[2] += 1f;
                if (npc.ai[2] > 450f)
                {
                    Vector2 vector21 = new Vector2(npc.position.X + (float)npc.width * 0.5f - (float)(npc.direction * 24), npc.position.Y + 4f);
                    int num212 = 3 * npc.direction;
                    int num211 = -5;
                    int num210 = Projectile.NewProjectile(vector21.X, vector21.Y, (float)num212, (float)num211, ProjectileID.WoodenArrowHostile, damage, 0f, Main.myPlayer, 0f, 0f);
                    Main.projectile[num210].timeLeft = 300;
                    npc.ai[2] = 0f;
                }
            }
            flag33 = false;
            if (npc.velocity.Y == 0f)
            {
                int num209 = (int)(npc.position.Y + (float)npc.height + 7f) / 16;
                int num437 = (int)npc.position.X / 16;
                int num208 = (int)(npc.position.X + (float)npc.width) / 16;
                for (int num207 = num437; num207 <= num208; num207++)
                {
                    if (Main.tile[num207, num209] == null)
                    {
                        return;
                    }
                    if (Main.tile[num207, num209].nactive() && Main.tileSolid[Main.tile[num207, num209].type])
                    {
                        flag33 = true;
                        break;
                    }
                }
            }
            if (npc.type == 428)
            {
                flag33 = false;
            }
            if (npc.velocity.Y >= 0f)
            {
                num206 = 0;
                if (npc.velocity.X < 0f)
                {
                    num206 = -1;
                }
                if (npc.velocity.X > 0f)
                {
                    num206 = 1;
                }
                position2 = npc.position;
                position2.X += npc.velocity.X;
                num205 = (int)((position2.X + (float)(npc.width / 2) + (float)((npc.width / 2 + 1) * num206)) / 16f);
                num204 = (int)((position2.Y + (float)npc.height - 1f) / 16f);
                if (Main.tile[num205, num204] == null)
                {
                    Tile[,] tile19 = Main.tile;
                    int num438 = num205;
                    int num439 = num204;
                    Tile tile20 = new Tile();
                    tile19[num438, num439] = tile20;
                }
                if (Main.tile[num205, num204 - 1] == null)
                {
                    Tile[,] tile21 = Main.tile;
                    int num440 = num205;
                    int num441 = num204 - 1;
                    Tile tile22 = new Tile();
                    tile21[num440, num441] = tile22;
                }
                if (Main.tile[num205, num204 - 2] == null)
                {
                    Tile[,] tile23 = Main.tile;
                    int num442 = num205;
                    int num443 = num204 - 2;
                    Tile tile24 = new Tile();
                    tile23[num442, num443] = tile24;
                }
                if (Main.tile[num205, num204 - 3] == null)
                {
                    Tile[,] tile25 = Main.tile;
                    int num444 = num205;
                    int num445 = num204 - 3;
                    Tile tile26 = new Tile();
                    tile25[num444, num445] = tile26;
                }
                if (Main.tile[num205, num204 + 1] == null)
                {
                    Tile[,] tile27 = Main.tile;
                    int num446 = num205;
                    int num447 = num204 + 1;
                    Tile tile28 = new Tile();
                    tile27[num446, num447] = tile28;
                }
                if (Main.tile[num205 - num206, num204 - 3] == null)
                {
                    Tile[,] tile29 = Main.tile;
                    int num448 = num205 - num206;
                    int num449 = num204 - 3;
                    Tile tile30 = new Tile();
                    tile29[num448, num449] = tile30;
                }
                if ((float)(num205 * 16) < position2.X + (float)npc.width && (float)(num205 * 16 + 16) > position2.X)
                {
                    if (Main.tile[num205, num204].nactive() && !Main.tile[num205, num204].topSlope() && !Main.tile[num205, num204 - 1].topSlope() && Main.tileSolid[Main.tile[num205, num204].type] && !Main.tileSolidTop[Main.tile[num205, num204].type])
                    {
                        goto IL_afd9;
                    }
                    if (Main.tile[num205, num204 - 1].halfBrick() && Main.tile[num205, num204 - 1].nactive())
                    {
                        goto IL_afd9;
                    }
                }
            }
            goto IL_b2a6;
        IL_41b9:
            npc.velocity.X = npc.velocity.X * 0.95f;
            goto IL_41d5;
        }

        public override void NPCLoot()
        {
            if (Main.rand.NextBool(40))
                Item.NewItem(npc.getRect(), ItemType<Items.Accessories.WightQuiver>());
            if (Main.rand.NextBool(3))
                Item.NewItem(npc.getRect(), ItemType<Items.Materials.WightBone>());
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.player.GetModPlayer<ExoriumPlayer>().ZoneDeadlands ? .1f : 0f;
        }
    }
}