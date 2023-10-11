using ExoriumMod.Core;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;

namespace ExoriumMod.Content.NPCs.Enemies
{
    class DuneCreeper : ModNPC
    {
        public override string Texture => AssetDirectory.EnemyNPC + Name;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dune Creeper");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.WallCreeper];
        }

        public override void SetDefaults()
        {
            NPC.width = 70;
            NPC.height = 30;
            NPC.damage = 26;
            NPC.defense = 12;
            NPC.lifeMax = 180;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.value = 1600f;
            NPC.knockBackResist = .4f;
            NPC.aiStyle = -1;
            NPC.buffImmune[BuffID.Confused] = false;
            AnimationType = NPCID.WallCreeper;
        }

        public override void AI()
        {
            Vector2 vector41;
            if (NPC.type == 466)
            {
                int num311 = 200;
                if (NPC.ai[2] == 0f)
                {
                    NPC.alpha = num311;
                    NPC.TargetClosest(true);
                    if (!Main.player[NPC.target].dead)
                    {
                        vector41 = Main.player[NPC.target].Center - NPC.Center;
                        if (vector41.Length() < 170f)
                        {
                            NPC.ai[2] = -16f;
                        }
                    }
                    if (NPC.velocity.X == 0f && !(NPC.velocity.Y < 0f) && !(NPC.velocity.Y > 2f) && !NPC.justHit)
                    {
                        return;
                    }
                    NPC.ai[2] = -16f;
                    return;
                }
                if (NPC.ai[2] < 0f)
                {
                    if (NPC.alpha > 0)
                    {
                        NPC.alpha -= num311 / 16;
                        if (NPC.alpha < 0)
                        {
                            NPC.alpha = 0;
                        }
                    }
                    NPC.ai[2] += 1f;
                    if (NPC.ai[2] == 0f)
                    {
                        NPC.ai[2] = 1f;
                        NPC.velocity.X = (float)(NPC.direction * 2);
                    }
                    return;
                }
                NPC.alpha = 0;
            }
            if (NPC.type == 166)
            {
                if (Main.netMode != 1 && Main.rand.Next(240) == 0)
                {
                    NPC.ai[2] = (float)Main.rand.Next(-480, -60);
                    NPC.netUpdate = true;
                }
                if (NPC.ai[2] < 0f)
                {
                    NPC.TargetClosest(true);
                    if (NPC.justHit)
                    {
                        NPC.ai[2] = 0f;
                    }
                    if (Collision.CanHit(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1))
                    {
                        NPC.ai[2] = 0f;
                    }
                }
                if (NPC.ai[2] < 0f)
                {
                    NPC.velocity.X = NPC.velocity.X * 0.9f;
                    if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
                    {
                        NPC.velocity.X = 0f;
                    }
                    NPC.ai[2] += 1f;
                    if (NPC.ai[2] == 0f)
                    {
                        NPC.velocity.X = (float)NPC.direction * 0.1f;
                    }
                    return;
                }
            }
            if (NPC.type == 461)
            {
                if (NPC.wet)
                {
                    NPC.knockBackResist = 0f;
                    NPC.ai[3] = -0.10101f;
                    NPC.noGravity = true;
                    Vector2 center4 = NPC.Center;
                    NPC.width = 34;
                    NPC.height = 24;
                    NPC.position.X = center4.X - (float)(NPC.width / 2);
                    NPC.position.Y = center4.Y - (float)(NPC.height / 2);
                    NPC.TargetClosest(true);
                    if (NPC.collideX)
                    {
                        NPC.velocity.X = 0f - NPC.oldVelocity.X;
                    }
                    if (NPC.velocity.X < 0f)
                    {
                        NPC.direction = -1;
                    }
                    if (NPC.velocity.X > 0f)
                    {
                        NPC.direction = 1;
                    }
                    if (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].Center, 1, 1))
                    {
                        Vector2 value3 = Main.player[NPC.target].Center - NPC.Center;
                        value3.Normalize();
                        value3 *= 5f;
                        NPC.velocity = (NPC.velocity * 19f + value3) / 20f;
                    }
                    else
                    {
                        float num374 = 5f;
                        if (NPC.velocity.Y > 0f)
                        {
                            num374 = 3f;
                        }
                        if (NPC.velocity.Y < 0f)
                        {
                            num374 = 8f;
                        }
                        Vector2 value2 = new Vector2((float)NPC.direction, -1f);
                        value2.Normalize();
                        value2 *= num374;
                        if (num374 < 5f)
                        {
                            NPC.velocity = (NPC.velocity * 24f + value2) / 25f;
                        }
                        else
                        {
                            NPC.velocity = (NPC.velocity * 9f + value2) / 10f;
                        }
                    }
                    return;
                }
                NPC.noGravity = false;
                Vector2 center5 = NPC.Center;
                NPC.width = 18;
                NPC.height = 40;
                NPC.position.X = center5.X - (float)(NPC.width / 2);
                NPC.position.Y = center5.Y - (float)(NPC.height / 2);
                if (NPC.ai[3] == -0.10101f)
                {
                    NPC.ai[3] = 0f;
                    float num383 = NPC.velocity.Length();
                    num383 *= 2f;
                    if (num383 > 10f)
                    {
                        num383 = 10f;
                    }
                    NPC.velocity.Normalize();
                    NPC.velocity *= num383;
                    if (NPC.velocity.X < 0f)
                    {
                        NPC.direction = -1;
                    }
                    if (NPC.velocity.X > 0f)
                    {
                        NPC.direction = 1;
                    }
                    NPC.spriteDirection = NPC.direction;
                }
            }
            if (NPC.type == 379 || NPC.type == 380)
            {
                if (NPC.ai[3] < 0f)
                {
                    NPC.damage = 0;
                    NPC.velocity.X = NPC.velocity.X * 0.93f;
                    if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
                    {
                        NPC.velocity.X = 0f;
                    }
                    int num387 = (int)(0f - NPC.ai[3] - 1f);
                    int num395 = Math.Sign(Main.npc[num387].Center.X - NPC.Center.X);
                    if (num395 != NPC.direction)
                    {
                        NPC.velocity.X = 0f;
                        NPC.direction = num395;
                        NPC.netUpdate = true;
                    }
                    if (NPC.justHit && Main.netMode != 1 && Main.npc[num387].localAI[0] == 0f)
                    {
                        Main.npc[num387].localAI[0] = 1f;
                    }
                    if (NPC.ai[0] < 1000f)
                    {
                        NPC.ai[0] = 1000f;
                    }
                    if ((NPC.ai[0] += 1f) >= 1300f)
                    {
                        NPC.ai[0] = 1000f;
                        NPC.netUpdate = true;
                    }
                    return;
                }
                if (NPC.ai[0] >= 1000f)
                {
                    NPC.ai[0] = 0f;
                }
                NPC.damage = NPC.defDamage;
            }
            if (NPC.type == 383)
            {
                int num415 = (int)NPC.ai[2] - 1;
                if (num415 != -1 && Main.npc[num415].active && Main.npc[num415].type == 384)
                {
                    NPC.dontTakeDamage = true;
                }
                else
                {
                    NPC.dontTakeDamage = false;
                    NPC.ai[2] = 0f;
                    if (NPC.localAI[0] == -1f)
                    {
                        NPC.localAI[0] = 180f;
                    }
                    if (NPC.localAI[0] > 0f)
                    {
                        NPC.localAI[0] -= 1f;
                    }
                }
            }
            if (NPC.type == 482)
            {
                int num414 = 300;
                int num413 = 120;
                NPC.dontTakeDamage = false;
                if (NPC.ai[2] < 0f)
                {
                    NPC.dontTakeDamage = true;
                    NPC.ai[2] += 1f;
                    NPC.velocity.X = NPC.velocity.X * 0.9f;
                    if ((double)Math.Abs(NPC.velocity.X) < 0.001)
                    {
                        NPC.velocity.X = 0.001f * (float)NPC.direction;
                    }
                    if (Math.Abs(NPC.velocity.Y) > 1f)
                    {
                        NPC.ai[2] += 10f;
                    }
                    if (NPC.ai[2] >= 0f)
                    {
                        NPC.netUpdate = true;
                        NPC.velocity.X = NPC.velocity.X + (float)NPC.direction * 0.3f;
                    }
                    return;
                }
                if (NPC.ai[2] < (float)num414)
                {
                    if (NPC.justHit)
                    {
                        NPC.ai[2] += 15f;
                    }
                    NPC.ai[2] += 1f;
                }
                else if (NPC.velocity.Y == 0f)
                {
                    NPC.ai[2] = 0f - (float)num413;
                    NPC.netUpdate = true;
                }
            }
            int num410;
            int num408;
            goto IL_119a;
            IL_119a:
            goto IL_1e03;
        IL_2d75:
            bool flag49;
            bool flag48;
            if (!flag49 & flag48)
            {
                if (NPC.velocity.Y == 0f)
                {
                    if (NPC.velocity.X > 0f && NPC.direction < 0)
                    {
                        goto IL_2dca;
                    }
                    if (NPC.velocity.X < 0f && NPC.direction > 0)
                    {
                        goto IL_2dca;
                    }
                }
                goto IL_2dcc;
            }
            goto IL_2e95;
        IL_1614:
            NPC.ai[3] = -4f;
            NPC.ai[2] = 0f;
            return;
        IL_8f61:
            if (NPC.type != 110 && NPC.type != 111 && NPC.type != 206 && NPC.type != 214 && NPC.type != 215 && NPC.type != 216 && NPC.type != 290 && NPC.type != 291 && NPC.type != 292 && NPC.type != 293 && NPC.type != 350 && NPC.type != 379 && NPC.type != 380 && NPC.type != 381 && NPC.type != 382 && (NPC.type < 449 || NPC.type > 452) && NPC.type != 468 && NPC.type != 481 && NPC.type != 411 && NPC.type != 409 && (NPC.type < 498 || NPC.type > 506) && NPC.type != 424 && NPC.type != 426 && NPC.type != 520)
            {
                goto IL_aba2;
            }
            bool flag39 = NPC.type == 381 || NPC.type == 382 || NPC.type == 520;
            bool flag38 = NPC.type == 426;
            bool flag37 = true;
            int num252 = -1;
            int num251 = -1;
            if (NPC.type == 411)
            {
                flag39 = true;
                num252 = 90;
                num251 = 90;
                if (NPC.ai[1] <= 150f)
                {
                    flag37 = false;
                }
            }
            if (NPC.confused)
            {
                NPC.ai[2] = 0f;
                goto IL_aba2;
            }
            if (NPC.ai[1] > 0f)
            {
                NPC.ai[1] -= 1f;
            }
            if (NPC.justHit)
            {
                NPC.ai[1] = 30f;
                NPC.ai[2] = 0f;
            }
            int num250 = 70;
            if (NPC.type == 379 || NPC.type == 380)
            {
                num250 = 80;
            }
            if (NPC.type == 381 || NPC.type == 382)
            {
                num250 = 80;
            }
            if (NPC.type == 520)
            {
                num250 = 15;
            }
            if (NPC.type == 350)
            {
                num250 = 110;
            }
            if (NPC.type == 291)
            {
                num250 = 200;
            }
            if (NPC.type == 292)
            {
                num250 = 120;
            }
            if (NPC.type == 293)
            {
                num250 = 90;
            }
            if (NPC.type == 111)
            {
                num250 = 180;
            }
            if (NPC.type == 206)
            {
                num250 = 50;
            }
            if (NPC.type == 481)
            {
                num250 = 100;
            }
            if (NPC.type == 214)
            {
                num250 = 40;
            }
            if (NPC.type == 215)
            {
                num250 = 80;
            }
            if (NPC.type == 290)
            {
                num250 = 30;
            }
            if (NPC.type == 411)
            {
                num250 = 300;
            }
            if (NPC.type == 409)
            {
                num250 = 60;
            }
            if (NPC.type == 424)
            {
                num250 = 180;
            }
            if (NPC.type == 426)
            {
                num250 = 60;
            }
            bool flag36 = false;
            if (NPC.type == 216)
            {
                if (NPC.localAI[2] >= 20f)
                {
                    flag36 = true;
                }
                num250 = ((!flag36) ? 8 : 60);
            }
            int num249 = num250 / 2;
            if (NPC.type == 424)
            {
                num249 = num250 - 1;
            }
            if (NPC.type == 426)
            {
                num249 = num250 - 1;
            }
            if (NPC.ai[2] > 0f)
            {
                if (flag37)
                {
                    NPC.TargetClosest(true);
                }
                if (NPC.ai[1] == (float)num249)
                {
                    if (NPC.type == 216)
                    {
                        NPC.localAI[2] += 1f;
                    }
                    float num248 = 11f;
                    if (NPC.type == 111)
                    {
                        num248 = 9f;
                    }
                    if (NPC.type == 206)
                    {
                        num248 = 7f;
                    }
                    if (NPC.type == 290)
                    {
                        num248 = 9f;
                    }
                    if (NPC.type == 293)
                    {
                        num248 = 4f;
                    }
                    if (NPC.type == 214)
                    {
                        num248 = 14f;
                    }
                    if (NPC.type == 215)
                    {
                        num248 = 16f;
                    }
                    if (NPC.type == 382)
                    {
                        num248 = 7f;
                    }
                    if (NPC.type == 520)
                    {
                        num248 = 8f;
                    }
                    if (NPC.type == 409)
                    {
                        num248 = 4f;
                    }
                    if (NPC.type >= 449 && NPC.type <= 452)
                    {
                        num248 = 7f;
                    }
                    if (NPC.type == 481)
                    {
                        num248 = 8f;
                    }
                    if (NPC.type == 468)
                    {
                        num248 = 7.5f;
                    }
                    if (NPC.type == 411)
                    {
                        num248 = 1f;
                    }
                    if (NPC.type >= 498 && NPC.type <= 506)
                    {
                        num248 = 7f;
                    }
                    Vector2 value9 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                    if (NPC.type == 481)
                    {
                        value9.Y -= 14f;
                    }
                    if (NPC.type == 206)
                    {
                        value9.Y -= 10f;
                    }
                    if (NPC.type == 290)
                    {
                        value9.Y -= 10f;
                    }
                    if (NPC.type == 381 || NPC.type == 382)
                    {
                        value9.Y += 6f;
                    }
                    if (NPC.type == 520)
                    {
                        value9.Y = NPC.position.Y + 20f;
                    }
                    if (NPC.type >= 498 && NPC.type <= 506)
                    {
                        value9.Y -= 8f;
                    }
                    if (NPC.type == 426)
                    {
                        value9 += new Vector2((float)(NPC.spriteDirection * 2), -12f);
                    }
                    float num247 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - value9.X;
                    float num246 = Math.Abs(num247) * 0.1f;
                    if (NPC.type == 291 || NPC.type == 292)
                    {
                        num246 = 0f;
                    }
                    if (NPC.type == 215)
                    {
                        num246 = Math.Abs(num247) * 0.08f;
                    }
                    if (NPC.type == 214 || (NPC.type == 216 && !flag36))
                    {
                        num246 = 0f;
                    }
                    if (NPC.type == 381 || NPC.type == 382 || NPC.type == 520)
                    {
                        num246 = 0f;
                    }
                    if (NPC.type >= 449 && NPC.type <= 452)
                    {
                        num246 = Math.Abs(num247) * (float)Main.rand.Next(10, 50) * 0.01f;
                    }
                    if (NPC.type == 468)
                    {
                        num246 = Math.Abs(num247) * (float)Main.rand.Next(10, 50) * 0.01f;
                    }
                    if (NPC.type == 481)
                    {
                        num246 = Math.Abs(num247) * (float)Main.rand.Next(-10, 11) * 0.0035f;
                    }
                    if (NPC.type >= 498 && NPC.type <= 506)
                    {
                        num246 = Math.Abs(num247) * (float)Main.rand.Next(1, 11) * 0.0025f;
                    }
                    float num245 = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - value9.Y - num246;
                    if (NPC.type == 291)
                    {
                        num247 += (float)Main.rand.Next(-40, 41) * 0.2f;
                        num245 += (float)Main.rand.Next(-40, 41) * 0.2f;
                    }
                    else if (NPC.type == 381 || NPC.type == 382 || NPC.type == 520)
                    {
                        num247 += (float)Main.rand.Next(-100, 101) * 0.4f;
                        num245 += (float)Main.rand.Next(-100, 101) * 0.4f;
                        num247 *= (float)Main.rand.Next(85, 116) * 0.01f;
                        num245 *= (float)Main.rand.Next(85, 116) * 0.01f;
                        if (NPC.type == 520)
                        {
                            num247 += (float)Main.rand.Next(-100, 101) * 0.6f;
                            num245 += (float)Main.rand.Next(-100, 101) * 0.6f;
                            num247 *= (float)Main.rand.Next(85, 116) * 0.015f;
                            num245 *= (float)Main.rand.Next(85, 116) * 0.015f;
                        }
                    }
                    else if (NPC.type == 481)
                    {
                        num247 += (float)Main.rand.Next(-40, 41) * 0.4f;
                        num245 += (float)Main.rand.Next(-40, 41) * 0.4f;
                    }
                    else if (NPC.type >= 498 && NPC.type <= 506)
                    {
                        num247 += (float)Main.rand.Next(-40, 41) * 0.3f;
                        num245 += (float)Main.rand.Next(-40, 41) * 0.3f;
                    }
                    else if (NPC.type != 292)
                    {
                        num247 += (float)Main.rand.Next(-40, 41);
                        num245 += (float)Main.rand.Next(-40, 41);
                    }
                    float num240 = (float)Math.Sqrt((double)(num247 * num247 + num245 * num245));
                    NPC.netUpdate = true;
                    num240 = num248 / num240;
                    num247 *= num240;
                    num245 *= num240;
                    int num236 = 35;
                    int num235 = 82;
                    if (NPC.type == 111)
                    {
                        num236 = 11;
                    }
                    if (NPC.type == 206)
                    {
                        num236 = 37;
                    }
                    if (NPC.type == 379 || NPC.type == 380)
                    {
                        num236 = 40;
                    }
                    if (NPC.type == 350)
                    {
                        num236 = 45;
                    }
                    if (NPC.type == 468)
                    {
                        num236 = 50;
                    }
                    if (NPC.type == 111)
                    {
                        num235 = 81;
                    }
                    if (NPC.type == 379 || NPC.type == 380)
                    {
                        num235 = 81;
                    }
                    if (NPC.type == 381)
                    {
                        num235 = 436;
                        num236 = 24;
                    }
                    if (NPC.type == 382)
                    {
                        num235 = 438;
                        num236 = 30;
                    }
                    if (NPC.type == 520)
                    {
                        num235 = 592;
                        num236 = 35;
                    }
                    if (NPC.type >= 449 && NPC.type <= 452)
                    {
                        num235 = 471;
                        num236 = 20;
                    }
                    if (NPC.type >= 498 && NPC.type <= 506)
                    {
                        num235 = 572;
                        num236 = 14;
                    }
                    if (NPC.type == 481)
                    {
                        num235 = 508;
                        num236 = 18;
                    }
                    if (NPC.type == 206)
                    {
                        num235 = 177;
                    }
                    if (NPC.type == 468)
                    {
                        num235 = 501;
                    }
                    if (NPC.type == 411)
                    {
                        num235 = 537;
                        num236 = (Main.expertMode ? 45 : 60);
                    }
                    if (NPC.type == 424)
                    {
                        num235 = 573;
                        num236 = (Main.expertMode ? 45 : 60);
                    }
                    if (NPC.type == 426)
                    {
                        num235 = 581;
                        num236 = (Main.expertMode ? 45 : 60);
                    }
                    if (NPC.type == 291)
                    {
                        num235 = 302;
                        num236 = 100;
                    }
                    if (NPC.type == 290)
                    {
                        num235 = 300;
                        num236 = 60;
                    }
                    if (NPC.type == 293)
                    {
                        num235 = 303;
                        num236 = 60;
                    }
                    if (NPC.type == 214)
                    {
                        num235 = 180;
                        num236 = 25;
                    }
                    if (NPC.type == 215)
                    {
                        num235 = 82;
                        num236 = 40;
                    }
                    if (NPC.type == 292)
                    {
                        num236 = 50;
                        num235 = 180;
                    }
                    if (NPC.type == 216)
                    {
                        num235 = 180;
                        num236 = 30;
                        if (flag36)
                        {
                            num236 = 100;
                            num235 = 240;
                            NPC.localAI[2] = 0f;
                        }
                    }
                    value9.X += num247;
                    value9.Y += num245;
                    if (Main.expertMode && NPC.type == 290)
                    {
                        num236 = (int)((double)num236 * 0.75);
                    }
                    if (Main.expertMode && NPC.type >= 381 && NPC.type <= 392)
                    {
                        num236 = (int)((double)num236 * 0.8);
                    }
                    if (Main.netMode != 1)
                    {
                        if (NPC.type == 292)
                        {
                            for (int num234 = 0; num234 < 4; num234++)
                            {
                                num247 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - value9.X;
                                num245 = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - value9.Y;
                                num240 = (float)Math.Sqrt((double)(num247 * num247 + num245 * num245));
                                num240 = 12f / num240;
                                num247 += (float)Main.rand.Next(-40, 41);
                                num245 += (float)Main.rand.Next(-40, 41);
                                num247 *= num240;
                                num245 *= num240;
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), value9.X, value9.Y, num247, num245, num235, num236, 0f, Main.myPlayer, 0f, 0f);
                            }
                        }
                        else if (NPC.type == 411)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), value9.X, value9.Y, num247, num245, num235, num236, 0f, Main.myPlayer, 0f, (float)NPC.whoAmI);
                        }
                        else if (NPC.type == 424)
                        {
                            for (int num227 = 0; num227 < 4; num227++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X - (float)(NPC.spriteDirection * 4), NPC.Center.Y + 6f, (float)(-3 + 2 * num227) * 0.15f, (0f - (float)Main.rand.Next(0, 3)) * 0.2f - 0.1f, num235, num236, 0f, Main.myPlayer, 0f, (float)NPC.whoAmI);
                            }
                        }
                        else
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), value9.X, value9.Y, num247, num245, num235, num236, 0f, Main.myPlayer, 0f, 0f);
                        }
                    }
                    if (Math.Abs(num245) > Math.Abs(num247) * 2f)
                    {
                        if (num245 > 0f)
                        {
                            NPC.ai[2] = 1f;
                        }
                        else
                        {
                            NPC.ai[2] = 5f;
                        }
                    }
                    else if (Math.Abs(num247) > Math.Abs(num245) * 2f)
                    {
                        NPC.ai[2] = 3f;
                    }
                    else if (num245 > 0f)
                    {
                        NPC.ai[2] = 2f;
                    }
                    else
                    {
                        NPC.ai[2] = 4f;
                    }
                }
                if (NPC.velocity.Y != 0f && !flag38)
                {
                    goto IL_a3fa;
                }
                if (NPC.ai[1] <= 0f)
                {
                    goto IL_a3fa;
                }
                if (!flag39 || (num252 != -1 && NPC.ai[1] >= (float)num252 && NPC.ai[1] < (float)(num252 + num251) && (!flag38 || NPC.velocity.Y == 0f)))
                {
                    NPC.velocity.X = NPC.velocity.X * 0.9f;
                    NPC.spriteDirection = NPC.direction;
                }
            }
            goto IL_a47a;
        IL_2e95:
            if (NPC.type == 463 && Main.netMode != 1)
            {
                if (NPC.localAI[3] > 0f)
                {
                    NPC.localAI[3] -= 1f;
                }
                if (NPC.justHit && NPC.localAI[3] <= 0f && Main.rand.Next(3) == 0)
                {
                    NPC.localAI[3] = 30f;
                    int num370 = Main.rand.Next(3, 6);
                    int[] array = new int[num370];
                    int num369 = 0;
                    for (int num368 = 0; num368 < 255; num368++)
                    {
                        if (Main.player[num368].active && !Main.player[num368].dead && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, Main.player[num368].position, Main.player[num368].width, Main.player[num368].height))
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
                        Vector2 value6 = Main.npc[array[num363]].Center - NPC.Center;
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
                            value7 = Main.player[array[num369]].Center - NPC.Center;
                            value7.Normalize();
                            value7 *= scaleFactor8;
                        }
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.position.Y + (float)(NPC.width / 4), value7.X, value7.Y, 498, (int)((double)NPC.damage * 0.15), 1f, 255, 0f, 0f);
                    }
                }
            }
            if (NPC.type == 462 && NPC.velocity.Y == 0f)
            {
                vector41 = Main.player[NPC.target].Center - NPC.Center;
                if (vector41.Length() < 150f && Math.Abs(NPC.velocity.X) > 3f)
                {
                    if (NPC.velocity.X < 0f && NPC.Center.X > Main.player[NPC.target].Center.X)
                    {
                        goto IL_3498;
                    }
                    if (NPC.velocity.X > 0f && NPC.Center.X < Main.player[NPC.target].Center.X)
                    {
                        goto IL_3498;
                    }
                }
            }
            goto IL_362f;
        IL_1e03:
            goto IL_21b6;
        IL_bdce:
            if ((NPC.type == 31 || NPC.type == 294 || NPC.type == 295 || NPC.type == 296 || NPC.type == 47 || NPC.type == 77 || NPC.type == 104 || NPC.type == 168 || NPC.type == 196 || NPC.type == 385 || NPC.type == 389 || NPC.type == 464 || NPC.type == 470 || (NPC.type >= 524 && NPC.type <= 527)) && NPC.velocity.Y == 0f && Math.Abs(NPC.position.X + (float)(NPC.width / 2) - (Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2))) < 100f && Math.Abs(NPC.position.Y + (float)(NPC.height / 2) - (Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2))) < 50f)
            {
                if (NPC.direction > 0 && NPC.velocity.X >= 1f)
                {
                    goto IL_bf88;
                }
                if (NPC.direction < 0 && NPC.velocity.X <= -1f)
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
            if ((!Main.tile[num205, num204 - 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num205, num204 - 1].TileType] || Main.tileSolidTop[Main.tile[num205, num204 - 1].TileType] || (Main.tile[num205, num204 - 1].IsHalfBlock && (!Main.tile[num205, num204 - 4].HasUnactuatedTile || !Main.tileSolid[Main.tile[num205, num204 - 4].TileType] || Main.tileSolidTop[Main.tile[num205, num204 - 4].TileType]))) && (!Main.tile[num205, num204 - 2].HasUnactuatedTile || !Main.tileSolid[Main.tile[num205, num204 - 2].TileType] || Main.tileSolidTop[Main.tile[num205, num204 - 2].TileType]) && (!Main.tile[num205, num204 - 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num205, num204 - 3].TileType] || Main.tileSolidTop[Main.tile[num205, num204 - 3].TileType]) && (!Main.tile[num205 - num206, num204 - 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num205 - num206, num204 - 3].TileType]))
            {
                float num203 = (float)(num204 * 16);
                if (Main.tile[num205, num204].IsHalfBlock)
                {
                    num203 += 8f;
                }
                if (Main.tile[num205, num204 - 1].IsHalfBlock)
                {
                    num203 -= 8f;
                }
                if (num203 < position2.Y + (float)NPC.height)
                {
                    float num202 = position2.Y + (float)NPC.height - num203;
                    float num201 = 16.1f;
                    if (NPC.type == 163 || NPC.type == 164 || true || NPC.type == 236 || NPC.type == 239 || NPC.type == 530)
                    {
                        num201 += 8f;
                    }
                    if (num202 <= num201)
                    {
                        NPC.gfxOffY += NPC.position.Y + (float)NPC.height - num203;
                        NPC.position.Y = num203 - (float)NPC.height;
                        if (num202 < 9f)
                        {
                            NPC.stepSpeed = 1f;
                        }
                        else
                        {
                            NPC.stepSpeed = 2f;
                        }
                    }
                }
            }
            goto IL_b2a6;
        IL_362f:
            int num382;
            if (NPC.ai[3] < (float)num382 && (Main.eclipse || !Main.dayTime || (double)NPC.position.Y > Main.worldSurface * 16.0 || (Main.invasionType == 1 && (NPC.type == 343 || NPC.type == 350)) || (Main.invasionType == 1 && (NPC.type == 26 || NPC.type == 27 || NPC.type == 28 || NPC.type == 111 || NPC.type == 471)) || NPC.type == 73 || (Main.invasionType == 3 && NPC.type >= 212 && NPC.type <= 216) || (Main.invasionType == 4 && (NPC.type == 381 || NPC.type == 382 || NPC.type == 383 || NPC.type == 385 || NPC.type == 386 || NPC.type == 389 || NPC.type == 391 || NPC.type == 520)) || NPC.type == 31 || NPC.type == 294 || NPC.type == 295 || NPC.type == 296 || NPC.type == 47 || NPC.type == 67 || NPC.type == 77 || NPC.type == 78 || NPC.type == 79 || NPC.type == 80 || NPC.type == 110 || NPC.type == 120 || NPC.type == 168 || NPC.type == 181 || NPC.type == 185 || NPC.type == 198 || NPC.type == 199 || NPC.type == 206 || NPC.type == 217 || NPC.type == 218 || NPC.type == 219 || NPC.type == 220 || NPC.type == 239 || NPC.type == 243 || NPC.type == 254 || NPC.type == 255 || NPC.type == 257 || NPC.type == 258 || NPC.type == 291 || NPC.type == 292 || NPC.type == 293 || NPC.type == 379 || NPC.type == 380 || NPC.type == 464 || NPC.type == 470 || NPC.type == 424 || (NPC.type == 411 && (NPC.ai[1] >= 180f || NPC.ai[1] < 90f)) || NPC.type == 409 || NPC.type == 425 || NPC.type == 429 || NPC.type == 427 || NPC.type == 428 || NPC.type == 508 || NPC.type == 415 || NPC.type == 419 || (NPC.type >= 524 && NPC.type <= 527) || NPC.type == 528 || NPC.type == 529 || NPC.type == 530 || NPC.type == 532))
            {
                /*
                if ((NPC.type == 3 || NPC.type == 331 || NPC.type == 332 || NPC.type == 21 || (NPC.type >= 449 && NPC.type <= 452) || NPC.type == 31 || NPC.type == 294 || NPC.type == 295 || NPC.type == 296 || NPC.type == 77 || NPC.type == 110 || NPC.type == 132 || NPC.type == 167 || NPC.type == 161 || NPC.type == 162 || NPC.type == 186 || NPC.type == 187 || NPC.type == 188 || NPC.type == 189 || NPC.type == 197 || NPC.type == 200 || NPC.type == 201 || NPC.type == 202 || NPC.type == 203 || NPC.type == 223 || NPC.type == 291 || NPC.type == 292 || NPC.type == 293 || NPC.type == 320 || NPC.type == 321 || NPC.type == 319 || NPC.type == 481) && Main.rand.Next(1000) == 0)
                {
                    SoundEngine.PlaySound(14, (int)NPC.position.X, (int)NPC.position.Y, 1, 1f, 0f);
                }
                if (NPC.type == 489 && Main.rand.Next(800) == 0)
                {
                    SoundEngine.PlaySound(14, (int)NPC.position.X, (int)NPC.position.Y, NPC.type, 1f, 0f);
                }
                if ((NPC.type == 78 || NPC.type == 79 || NPC.type == 80) && Main.rand.Next(500) == 0)
                {
                    SoundEngine.PlaySound(26, (int)NPC.position.X, (int)NPC.position.Y, 1, 1f, 0f);
                }
                if (NPC.type == 159 && Main.rand.Next(500) == 0)
                {
                    SoundEngine.PlaySound(29, (int)NPC.position.X, (int)NPC.position.Y, 7, 1f, 0f);
                }
                if (NPC.type == 162 && Main.rand.Next(500) == 0)
                {
                    SoundEngine.PlaySound(29, (int)NPC.position.X, (int)NPC.position.Y, 6, 1f, 0f);
                }
                if (NPC.type == 181 && Main.rand.Next(500) == 0)
                {
                    SoundEngine.PlaySound(29, (int)NPC.position.X, (int)NPC.position.Y, 8, 1f, 0f);
                }
                if (NPC.type >= 269 && NPC.type <= 280 && Main.rand.Next(1000) == 0)
                {
                    SoundEngine.PlaySound(14, (int)NPC.position.X, (int)NPC.position.Y, 1, 1f, 0f);
                }
                */
                NPC.TargetClosest(true);
            }
            else if (NPC.ai[2] <= 0f || (NPC.type != 110 && NPC.type != 111 && NPC.type != 206 && NPC.type != 216 && NPC.type != 214 && NPC.type != 215 && NPC.type != 291 && NPC.type != 292 && NPC.type != 293 && NPC.type != 350 && NPC.type != 381 && NPC.type != 382 && NPC.type != 383 && NPC.type != 385 && NPC.type != 386 && NPC.type != 389 && NPC.type != 391 && NPC.type != 469 && NPC.type != 166 && NPC.type != 466 && NPC.type != 471 && NPC.type != 411 && NPC.type != 409 && NPC.type != 424 && NPC.type != 425 && NPC.type != 426 && NPC.type != 415 && NPC.type != 419 && NPC.type != 520))
            {
                if (Main.dayTime && (double)(NPC.position.Y / 16f) < Main.worldSurface && NPC.timeLeft > 10)
                {
                    NPC.timeLeft = 10;
                }
                if (NPC.velocity.X == 0f)
                {
                    if (NPC.velocity.Y == 0f)
                    {
                        NPC.ai[0] += 1f;
                        if (NPC.ai[0] >= 2f)
                        {
                            NPC.direction *= -1;
                            NPC.spriteDirection = NPC.direction;
                            NPC.ai[0] = 0f;
                        }
                    }
                }
                else
                {
                    NPC.ai[0] = 0f;
                }
                if (NPC.direction == 0)
                {
                    NPC.direction = 1;
                }
            }
            if (NPC.type != 159 && NPC.type != 349)
            {
                if (NPC.type == 199)
                {
                    if (NPC.velocity.X < -4f || NPC.velocity.X > 4f)
                    {
                        if (NPC.velocity.Y == 0f)
                        {
                            NPC.velocity *= 0.8f;
                        }
                    }
                    else if (NPC.velocity.X < 4f && NPC.direction == 1)
                    {
                        if (NPC.velocity.Y == 0f && NPC.velocity.X < 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X * 0.8f;
                        }
                        NPC.velocity.X = NPC.velocity.X + 0.1f;
                        if (NPC.velocity.X > 4f)
                        {
                            NPC.velocity.X = 4f;
                        }
                    }
                    else if (NPC.velocity.X > -4f && NPC.direction == -1)
                    {
                        if (NPC.velocity.Y == 0f && NPC.velocity.X > 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X * 0.8f;
                        }
                        NPC.velocity.X = NPC.velocity.X - 0.1f;
                        if (NPC.velocity.X < -4f)
                        {
                            NPC.velocity.X = -4f;
                        }
                    }
                }
                else if (NPC.type == 120 || NPC.type == 166 || NPC.type == 213 || NPC.type == 258 || NPC.type == 528 || NPC.type == 529)
                {
                    if (NPC.velocity.X < -3f || NPC.velocity.X > 3f)
                    {
                        if (NPC.velocity.Y == 0f)
                        {
                            NPC.velocity *= 0.8f;
                        }
                    }
                    else if (NPC.velocity.X < 3f && NPC.direction == 1)
                    {
                        if (NPC.velocity.Y == 0f && NPC.velocity.X < 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X * 0.99f;
                        }
                        NPC.velocity.X = NPC.velocity.X + 0.07f;
                        if (NPC.velocity.X > 3f)
                        {
                            NPC.velocity.X = 3f;
                        }
                    }
                    else if (NPC.velocity.X > -3f && NPC.direction == -1)
                    {
                        if (NPC.velocity.Y == 0f && NPC.velocity.X > 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X * 0.99f;
                        }
                        NPC.velocity.X = NPC.velocity.X - 0.07f;
                        if (NPC.velocity.X < -3f)
                        {
                            NPC.velocity.X = -3f;
                        }
                    }
                }
                else if (NPC.type == 461 || NPC.type == 27 || NPC.type == 77 || NPC.type == 104 || NPC.type == 163 || NPC.type == 162 || NPC.type == 196 || NPC.type == 197 || NPC.type == 212 || NPC.type == 257 || NPC.type == 326 || NPC.type == 343 || NPC.type == 348 || NPC.type == 351 || (NPC.type >= 524 && NPC.type <= 527) || NPC.type == 530)
                {
                    if (NPC.velocity.X < -2f || NPC.velocity.X > 2f)
                    {
                        if (NPC.velocity.Y == 0f)
                        {
                            NPC.velocity *= 0.8f;
                        }
                    }
                    else if (NPC.velocity.X < 2f && NPC.direction == 1)
                    {
                        NPC.velocity.X = NPC.velocity.X + 0.07f;
                        if (NPC.velocity.X > 2f)
                        {
                            NPC.velocity.X = 2f;
                        }
                    }
                    else if (NPC.velocity.X > -2f && NPC.direction == -1)
                    {
                        NPC.velocity.X = NPC.velocity.X - 0.07f;
                        if (NPC.velocity.X < -2f)
                        {
                            NPC.velocity.X = -2f;
                        }
                    }
                }
                else if (NPC.type == 109)
                {
                    if (NPC.velocity.X < -2f || NPC.velocity.X > 2f)
                    {
                        if (NPC.velocity.Y == 0f)
                        {
                            NPC.velocity *= 0.8f;
                        }
                    }
                    else if (NPC.velocity.X < 2f && NPC.direction == 1)
                    {
                        NPC.velocity.X = NPC.velocity.X + 0.04f;
                        if (NPC.velocity.X > 2f)
                        {
                            NPC.velocity.X = 2f;
                        }
                    }
                    else if (NPC.velocity.X > -2f && NPC.direction == -1)
                    {
                        NPC.velocity.X = NPC.velocity.X - 0.04f;
                        if (NPC.velocity.X < -2f)
                        {
                            NPC.velocity.X = -2f;
                        }
                    }
                }
                else if (NPC.type == 21 || NPC.type == 26 || NPC.type == 31 || NPC.type == 294 || NPC.type == 295 || NPC.type == 296 || NPC.type == 47 || NPC.type == 73 || NPC.type == 140 || NPC.type == 164 || true || NPC.type == 239 || NPC.type == 167 || NPC.type == 168 || NPC.type == 185 || NPC.type == 198 || NPC.type == 201 || NPC.type == 202 || NPC.type == 203 || NPC.type == 217 || NPC.type == 218 || NPC.type == 219 || NPC.type == 226 || NPC.type == 181 || NPC.type == 254 || NPC.type == 338 || NPC.type == 339 || NPC.type == 340 || NPC.type == 342 || NPC.type == 385 || NPC.type == 389 || NPC.type == 462 || NPC.type == 463 || NPC.type == 466 || NPC.type == 464 || NPC.type == 469 || NPC.type == 470 || NPC.type == 480 || NPC.type == 482 || NPC.type == 425 || NPC.type == 429)
                {
                    float num324 = 1.5f;
                    if (NPC.type == 294)
                    {
                        num324 = 2f;
                    }
                    else if (NPC.type == 295)
                    {
                        num324 = 1.75f;
                    }
                    else if (NPC.type == 296)
                    {
                        num324 = 1.25f;
                    }
                    else if (NPC.type == 201)
                    {
                        num324 = 1.1f;
                    }
                    else if (NPC.type == 202)
                    {
                        num324 = 0.9f;
                    }
                    else if (NPC.type == 203)
                    {
                        num324 = 1.2f;
                    }
                    else if (NPC.type == 338)
                    {
                        num324 = 1.75f;
                    }
                    else if (NPC.type == 339)
                    {
                        num324 = 1.25f;
                    }
                    else if (NPC.type == 340)
                    {
                        num324 = 2f;
                    }
                    else if (NPC.type == 385)
                    {
                        num324 = 1.8f;
                    }
                    else if (NPC.type == 389)
                    {
                        num324 = 2.25f;
                    }
                    else if (NPC.type == 462)
                    {
                        num324 = 4f;
                    }
                    else if (NPC.type == 463)
                    {
                        num324 = 0.75f;
                    }
                    else if (NPC.type == 466)
                    {
                        num324 = 3.75f;
                    }
                    else if (NPC.type == 469)
                    {
                        num324 = 3.25f;
                    }
                    else if (NPC.type == 480)
                    {
                        num324 = 1.5f + (1f - (float)NPC.life / (float)NPC.lifeMax) * 2f;
                    }
                    else if (NPC.type == 425)
                    {
                        num324 = 6f;
                    }
                    else if (NPC.type == 429)
                    {
                        num324 = 4f;
                    }
                    if (NPC.type == 21 || NPC.type == 201 || NPC.type == 202 || NPC.type == 203 || NPC.type == 342)
                    {
                        num324 *= 1f + (1f - NPC.scale);
                    }
                    if (NPC.velocity.X < 0f - num324 || NPC.velocity.X > num324)
                    {
                        if (NPC.velocity.Y == 0f)
                        {
                            NPC.velocity *= 0.8f;
                        }
                    }
                    else if (NPC.velocity.X < num324 && NPC.direction == 1)
                    {
                        if (NPC.type == 466 && NPC.velocity.X < -2f)
                        {
                            NPC.velocity.X = NPC.velocity.X * 0.9f;
                        }
                        NPC.velocity.X = NPC.velocity.X + 0.07f;
                        if (NPC.velocity.X > num324)
                        {
                            NPC.velocity.X = num324;
                        }
                    }
                    else if (NPC.velocity.X > 0f - num324 && NPC.direction == -1)
                    {
                        if (NPC.type == 466 && NPC.velocity.X > 2f)
                        {
                            NPC.velocity.X = NPC.velocity.X * 0.9f;
                        }
                        NPC.velocity.X = NPC.velocity.X - 0.07f;
                        if (NPC.velocity.X < 0f - num324)
                        {
                            NPC.velocity.X = 0f - num324;
                        }
                    }
                    if (NPC.velocity.Y == 0f && NPC.type == 462)
                    {
                        if (NPC.direction > 0 && NPC.velocity.X < 0f)
                        {
                            goto IL_509d;
                        }
                        if (NPC.direction < 0 && NPC.velocity.X > 0f)
                        {
                            goto IL_509d;
                        }
                    }
                }
                else if (NPC.type >= 269 && NPC.type <= 280)
                {
                    float num356 = 1.5f;
                    if (NPC.type == 269)
                    {
                        num356 = 2f;
                    }
                    if (NPC.type == 270)
                    {
                        num356 = 1f;
                    }
                    if (NPC.type == 271)
                    {
                        num356 = 1.5f;
                    }
                    if (NPC.type == 272)
                    {
                        num356 = 3f;
                    }
                    if (NPC.type == 273)
                    {
                        num356 = 1.25f;
                    }
                    if (NPC.type == 274)
                    {
                        num356 = 3f;
                    }
                    if (NPC.type == 275)
                    {
                        num356 = 3.25f;
                    }
                    if (NPC.type == 276)
                    {
                        num356 = 2f;
                    }
                    if (NPC.type == 277)
                    {
                        num356 = 2.75f;
                    }
                    if (NPC.type == 278)
                    {
                        num356 = 1.8f;
                    }
                    if (NPC.type == 279)
                    {
                        num356 = 1.3f;
                    }
                    if (NPC.type == 280)
                    {
                        num356 = 2.5f;
                    }
                    num356 *= 1f + (1f - NPC.scale);
                    if (NPC.velocity.X < 0f - num356 || NPC.velocity.X > num356)
                    {
                        if (NPC.velocity.Y == 0f)
                        {
                            NPC.velocity *= 0.8f;
                        }
                    }
                    else if (NPC.velocity.X < num356 && NPC.direction == 1)
                    {
                        NPC.velocity.X = NPC.velocity.X + 0.07f;
                        if (NPC.velocity.X > num356)
                        {
                            NPC.velocity.X = num356;
                        }
                    }
                    else if (NPC.velocity.X > 0f - num356 && NPC.direction == -1)
                    {
                        NPC.velocity.X = NPC.velocity.X - 0.07f;
                        if (NPC.velocity.X < 0f - num356)
                        {
                            NPC.velocity.X = 0f - num356;
                        }
                    }
                }
                else if (NPC.type >= 305 && NPC.type <= 314)
                {
                    float num354 = 1.5f;
                    if (NPC.type == 305 || NPC.type == 310)
                    {
                        num354 = 2f;
                    }
                    if (NPC.type == 306 || NPC.type == 311)
                    {
                        num354 = 1.25f;
                    }
                    if (NPC.type == 307 || NPC.type == 312)
                    {
                        num354 = 2.25f;
                    }
                    if (NPC.type == 308 || NPC.type == 313)
                    {
                        num354 = 1.5f;
                    }
                    if (NPC.type == 309 || NPC.type == 314)
                    {
                        num354 = 1f;
                    }
                    if (NPC.type < 310)
                    {
                        if (NPC.velocity.Y == 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X * 0.85f;
                            if ((double)NPC.velocity.X > -0.3 && (double)NPC.velocity.X < 0.3)
                            {
                                NPC.velocity.Y = -7f;
                                NPC.velocity.X = num354 * (float)NPC.direction;
                            }
                        }
                        else if (NPC.spriteDirection == NPC.direction)
                        {
                            NPC.velocity.X = (NPC.velocity.X * 10f + num354 * (float)NPC.direction) / 11f;
                        }
                    }
                    else if (NPC.velocity.X < 0f - num354 || NPC.velocity.X > num354)
                    {
                        if (NPC.velocity.Y == 0f)
                        {
                            NPC.velocity *= 0.8f;
                        }
                    }
                    else if (NPC.velocity.X < num354 && NPC.direction == 1)
                    {
                        NPC.velocity.X = NPC.velocity.X + 0.07f;
                        if (NPC.velocity.X > num354)
                        {
                            NPC.velocity.X = num354;
                        }
                    }
                    else if (NPC.velocity.X > 0f - num354 && NPC.direction == -1)
                    {
                        NPC.velocity.X = NPC.velocity.X - 0.07f;
                        if (NPC.velocity.X < 0f - num354)
                        {
                            NPC.velocity.X = 0f - num354;
                        }
                    }
                }
                else if (NPC.type == 67 || NPC.type == 220 || NPC.type == 428)
                {
                    if (NPC.velocity.X < -0.5f || NPC.velocity.X > 0.5f)
                    {
                        if (NPC.velocity.Y == 0f)
                        {
                            NPC.velocity *= 0.7f;
                        }
                    }
                    else if (NPC.velocity.X < 0.5f && NPC.direction == 1)
                    {
                        NPC.velocity.X = NPC.velocity.X + 0.03f;
                        if (NPC.velocity.X > 0.5f)
                        {
                            NPC.velocity.X = 0.5f;
                        }
                    }
                    else if (NPC.velocity.X > -0.5f && NPC.direction == -1)
                    {
                        NPC.velocity.X = NPC.velocity.X - 0.03f;
                        if (NPC.velocity.X < -0.5f)
                        {
                            NPC.velocity.X = -0.5f;
                        }
                    }
                }
                else if (NPC.type == 78 || NPC.type == 79 || NPC.type == 80)
                {
                    float num326 = 1f;
                    float num325 = 0.05f;
                    if (NPC.life < NPC.lifeMax / 2)
                    {
                        num326 = 2f;
                        num325 = 0.1f;
                    }
                    if (NPC.type == 79)
                    {
                        num326 *= 1.5f;
                    }
                    if (NPC.velocity.X < 0f - num326 || NPC.velocity.X > num326)
                    {
                        if (NPC.velocity.Y == 0f)
                        {
                            NPC.velocity *= 0.7f;
                        }
                    }
                    else if (NPC.velocity.X < num326 && NPC.direction == 1)
                    {
                        NPC.velocity.X = NPC.velocity.X + num325;
                        if (NPC.velocity.X > num326)
                        {
                            NPC.velocity.X = num326;
                        }
                    }
                    else if (NPC.velocity.X > 0f - num326 && NPC.direction == -1)
                    {
                        NPC.velocity.X = NPC.velocity.X - num325;
                        if (NPC.velocity.X < 0f - num326)
                        {
                            NPC.velocity.X = 0f - num326;
                        }
                    }
                }
                else if (NPC.type == 287)
                {
                    float num353 = 5f;
                    float num352 = 0.2f;
                    if (NPC.velocity.X < 0f - num353 || NPC.velocity.X > num353)
                    {
                        if (NPC.velocity.Y == 0f)
                        {
                            NPC.velocity *= 0.7f;
                        }
                    }
                    else if (NPC.velocity.X < num353 && NPC.direction == 1)
                    {
                        NPC.velocity.X = NPC.velocity.X + num352;
                        if (NPC.velocity.X > num353)
                        {
                            NPC.velocity.X = num353;
                        }
                    }
                    else if (NPC.velocity.X > 0f - num353 && NPC.direction == -1)
                    {
                        NPC.velocity.X = NPC.velocity.X - num352;
                        if (NPC.velocity.X < 0f - num353)
                        {
                            NPC.velocity.X = 0f - num353;
                        }
                    }
                }
                else if (NPC.type == 243)
                {
                    float num351 = 1f;
                    float num350 = 0.07f;
                    num351 += (1f - (float)NPC.life / (float)NPC.lifeMax) * 1.5f;
                    num350 += (1f - (float)NPC.life / (float)NPC.lifeMax) * 0.15f;
                    if (NPC.velocity.X < 0f - num351 || NPC.velocity.X > num351)
                    {
                        if (NPC.velocity.Y == 0f)
                        {
                            NPC.velocity *= 0.7f;
                        }
                    }
                    else if (NPC.velocity.X < num351 && NPC.direction == 1)
                    {
                        NPC.velocity.X = NPC.velocity.X + num350;
                        if (NPC.velocity.X > num351)
                        {
                            NPC.velocity.X = num351;
                        }
                    }
                    else if (NPC.velocity.X > 0f - num351 && NPC.direction == -1)
                    {
                        NPC.velocity.X = NPC.velocity.X - num350;
                        if (NPC.velocity.X < 0f - num351)
                        {
                            NPC.velocity.X = 0f - num351;
                        }
                    }
                }
                else if (NPC.type == 251)
                {
                    float num347 = 1f;
                    float num346 = 0.08f;
                    num347 += (1f - (float)NPC.life / (float)NPC.lifeMax) * 2f;
                    num346 += (1f - (float)NPC.life / (float)NPC.lifeMax) * 0.2f;
                    if (NPC.velocity.X < 0f - num347 || NPC.velocity.X > num347)
                    {
                        if (NPC.velocity.Y == 0f)
                        {
                            NPC.velocity *= 0.7f;
                        }
                    }
                    else if (NPC.velocity.X < num347 && NPC.direction == 1)
                    {
                        NPC.velocity.X = NPC.velocity.X + num346;
                        if (NPC.velocity.X > num347)
                        {
                            NPC.velocity.X = num347;
                        }
                    }
                    else if (NPC.velocity.X > 0f - num347 && NPC.direction == -1)
                    {
                        NPC.velocity.X = NPC.velocity.X - num346;
                        if (NPC.velocity.X < 0f - num347)
                        {
                            NPC.velocity.X = 0f - num347;
                        }
                    }
                }
                else if (NPC.type == 386)
                {
                    if (NPC.ai[2] > 0f)
                    {
                        if (NPC.velocity.Y == 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X * 0.8f;
                        }
                    }
                    else
                    {
                        float num343 = 0.15f;
                        float num342 = 1.5f;
                        if (NPC.velocity.X < 0f - num342 || NPC.velocity.X > num342)
                        {
                            if (NPC.velocity.Y == 0f)
                            {
                                NPC.velocity *= 0.7f;
                            }
                        }
                        else if (NPC.velocity.X < num342 && NPC.direction == 1)
                        {
                            NPC.velocity.X = NPC.velocity.X + num343;
                            if (NPC.velocity.X > num342)
                            {
                                NPC.velocity.X = num342;
                            }
                        }
                        else if (NPC.velocity.X > 0f - num342 && NPC.direction == -1)
                        {
                            NPC.velocity.X = NPC.velocity.X - num343;
                            if (NPC.velocity.X < 0f - num342)
                            {
                                NPC.velocity.X = 0f - num342;
                            }
                        }
                    }
                }
                else if (NPC.type == 460)
                {
                    float num341 = 3f;
                    float num340 = 0.1f;
                    if (Math.Abs(NPC.velocity.X) > 2f)
                    {
                        num340 *= 0.8f;
                    }
                    if ((double)Math.Abs(NPC.velocity.X) > 2.5)
                    {
                        num340 *= 0.8f;
                    }
                    if (Math.Abs(NPC.velocity.X) > 3f)
                    {
                        num340 *= 0.8f;
                    }
                    if ((double)Math.Abs(NPC.velocity.X) > 3.5)
                    {
                        num340 *= 0.8f;
                    }
                    if (Math.Abs(NPC.velocity.X) > 4f)
                    {
                        num340 *= 0.8f;
                    }
                    if ((double)Math.Abs(NPC.velocity.X) > 4.5)
                    {
                        num340 *= 0.8f;
                    }
                    if (Math.Abs(NPC.velocity.X) > 5f)
                    {
                        num340 *= 0.8f;
                    }
                    if ((double)Math.Abs(NPC.velocity.X) > 5.5)
                    {
                        num340 *= 0.8f;
                    }
                    num341 += (1f - (float)NPC.life / (float)NPC.lifeMax) * 3f;
                    if (NPC.velocity.X < 0f - num341 || NPC.velocity.X > num341)
                    {
                        if (NPC.velocity.Y == 0f)
                        {
                            NPC.velocity *= 0.7f;
                        }
                    }
                    else if (NPC.velocity.X < num341 && NPC.direction == 1)
                    {
                        if (NPC.velocity.X < 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X * 0.93f;
                        }
                        NPC.velocity.X = NPC.velocity.X + num340;
                        if (NPC.velocity.X > num341)
                        {
                            NPC.velocity.X = num341;
                        }
                    }
                    else if (NPC.velocity.X > 0f - num341 && NPC.direction == -1)
                    {
                        if (NPC.velocity.X > 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X * 0.93f;
                        }
                        NPC.velocity.X = NPC.velocity.X - num340;
                        if (NPC.velocity.X < 0f - num341)
                        {
                            NPC.velocity.X = 0f - num341;
                        }
                    }
                }
                else if (NPC.type == 508)
                {
                    float num338 = 2.5f;
                    float num337 = 40f;
                    float num336 = Math.Abs(NPC.velocity.X);
                    if (num336 > 2.75f)
                    {
                        num338 = 3.5f;
                        num337 += 80f;
                    }
                    else if ((double)num336 > 2.25)
                    {
                        num338 = 3f;
                        num337 += 60f;
                    }
                    if ((double)Math.Abs(NPC.velocity.Y) < 0.5)
                    {
                        if (NPC.velocity.X > 0f && NPC.direction < 0)
                        {
                            NPC.velocity *= 0.9f;
                        }
                        if (NPC.velocity.X < 0f && NPC.direction > 0)
                        {
                            NPC.velocity *= 0.9f;
                        }
                    }
                    if (Math.Abs(NPC.velocity.Y) > 0.3f)
                    {
                        num337 *= 3f;
                    }
                    if (NPC.velocity.X <= 0f && NPC.direction < 0)
                    {
                        NPC.velocity.X = (NPC.velocity.X * num337 - num338) / (num337 + 1f);
                    }
                    else if (NPC.velocity.X >= 0f && NPC.direction > 0)
                    {
                        NPC.velocity.X = (NPC.velocity.X * num337 + num338) / (num337 + 1f);
                    }
                    else if (Math.Abs(NPC.Center.X - Main.player[NPC.target].Center.X) > 20f && Math.Abs(NPC.velocity.Y) <= 0.3f)
                    {
                        NPC.velocity.X = NPC.velocity.X * 0.99f;
                        NPC.velocity.X = NPC.velocity.X + (float)NPC.direction * 0.025f;
                    }
                }
                else if (NPC.type == 391 || NPC.type == 427 || NPC.type == 415 || NPC.type == 419 || NPC.type == 518 || NPC.type == 532)
                {
                    float num328 = 5f;
                    float num327 = 0.25f;
                    float scaleFactor7 = 0.7f;
                    if (NPC.type == 427)
                    {
                        num328 = 6f;
                        num327 = 0.2f;
                        scaleFactor7 = 0.8f;
                    }
                    else if (NPC.type == 415)
                    {
                        num328 = 4f;
                        num327 = 0.1f;
                        scaleFactor7 = 0.95f;
                    }
                    else if (NPC.type == 419)
                    {
                        num328 = 6f;
                        num327 = 0.15f;
                        scaleFactor7 = 0.85f;
                    }
                    else if (NPC.type == 518)
                    {
                        num328 = 5f;
                        num327 = 0.1f;
                        scaleFactor7 = 0.95f;
                    }
                    else if (NPC.type == 532)
                    {
                        num328 = 5f;
                        num327 = 0.15f;
                        scaleFactor7 = 0.98f;
                    }
                    if (NPC.velocity.X < 0f - num328 || NPC.velocity.X > num328)
                    {
                        if (NPC.velocity.Y == 0f)
                        {
                            NPC.velocity *= scaleFactor7;
                        }
                    }
                    else if (NPC.velocity.X < num328 && NPC.direction == 1)
                    {
                        NPC.velocity.X = NPC.velocity.X + num327;
                        if (NPC.velocity.X > num328)
                        {
                            NPC.velocity.X = num328;
                        }
                    }
                    else if (NPC.velocity.X > 0f - num328 && NPC.direction == -1)
                    {
                        NPC.velocity.X = NPC.velocity.X - num327;
                        if (NPC.velocity.X < 0f - num328)
                        {
                            NPC.velocity.X = 0f - num328;
                        }
                    }
                }
                else
                {
                    if ((NPC.type < 430 || NPC.type > 436) && NPC.type != 494 && NPC.type != 495)
                    {
                        if (NPC.type != 110 && NPC.type != 111 && NPC.type != 206 && NPC.type != 214 && NPC.type != 215 && NPC.type != 216 && NPC.type != 290 && NPC.type != 291 && NPC.type != 292 && NPC.type != 293 && NPC.type != 350 && NPC.type != 379 && NPC.type != 380 && NPC.type != 381 && NPC.type != 382 && (NPC.type < 449 || NPC.type > 452) && NPC.type != 468 && NPC.type != 481 && NPC.type != 411 && NPC.type != 409 && (NPC.type < 498 || NPC.type > 506) && NPC.type != 424 && NPC.type != 426 && NPC.type != 520)
                        {
                            float num335 = 1f;
                            if (NPC.type == 186)
                            {
                                num335 = 1.1f;
                            }
                            if (NPC.type == 187)
                            {
                                num335 = 0.9f;
                            }
                            if (NPC.type == 188)
                            {
                                num335 = 1.2f;
                            }
                            if (NPC.type == 189)
                            {
                                num335 = 0.8f;
                            }
                            if (NPC.type == 132)
                            {
                                num335 = 0.95f;
                            }
                            if (NPC.type == 200)
                            {
                                num335 = 0.87f;
                            }
                            if (NPC.type == 223)
                            {
                                num335 = 1.05f;
                            }
                            if (NPC.type == 489)
                            {
                                vector41 = Main.player[NPC.target].Center - NPC.Center;
                                float num334 = vector41.Length();
                                num334 *= 0.0025f;
                                if ((double)num334 > 1.5)
                                {
                                    num334 = 1.5f;
                                }
                                num335 = ((!Main.expertMode) ? (2.5f - num334) : (3f - num334));
                                num335 *= 0.8f;
                            }
                            if (NPC.type == 489 || NPC.type == 3 || NPC.type == 132 || NPC.type == 186 || NPC.type == 187 || NPC.type == 188 || NPC.type == 189 || NPC.type == 200 || NPC.type == 223 || NPC.type == 331 || NPC.type == 332)
                            {
                                num335 *= 1f + (1f - NPC.scale);
                            }
                            if (NPC.velocity.X < 0f - num335 || NPC.velocity.X > num335)
                            {
                                if (NPC.velocity.Y == 0f)
                                {
                                    NPC.velocity *= 0.8f;
                                }
                            }
                            else if (NPC.velocity.X < num335 && NPC.direction == 1)
                            {
                                NPC.velocity.X = NPC.velocity.X + 0.07f;
                                if (NPC.velocity.X > num335)
                                {
                                    NPC.velocity.X = num335;
                                }
                            }
                            else if (NPC.velocity.X > 0f - num335 && NPC.direction == -1)
                            {
                                NPC.velocity.X = NPC.velocity.X - 0.07f;
                                if (NPC.velocity.X < 0f - num335)
                                {
                                    NPC.velocity.X = 0f - num335;
                                }
                            }
                        }
                        goto IL_6bf9;
                    }
                    if (NPC.ai[2] == 0f)
                    {
                        NPC.damage = NPC.defDamage;
                        float num331 = 1f;
                        num331 *= 1f + (1f - NPC.scale);
                        if (NPC.velocity.X < 0f - num331 || NPC.velocity.X > num331)
                        {
                            if (NPC.velocity.Y == 0f)
                            {
                                NPC.velocity *= 0.8f;
                            }
                        }
                        else if (NPC.velocity.X < num331 && NPC.direction == 1)
                        {
                            NPC.velocity.X = NPC.velocity.X + 0.07f;
                            if (NPC.velocity.X > num331)
                            {
                                NPC.velocity.X = num331;
                            }
                        }
                        else if (NPC.velocity.X > 0f - num331 && NPC.direction == -1)
                        {
                            NPC.velocity.X = NPC.velocity.X - 0.07f;
                            if (NPC.velocity.X < 0f - num331)
                            {
                                NPC.velocity.X = 0f - num331;
                            }
                        }
                        if (NPC.velocity.Y == 0f && (!Main.dayTime || (double)NPC.position.Y > Main.worldSurface * 16.0) && !Main.player[NPC.target].dead)
                        {
                            Vector2 vector25 = NPC.Center - Main.player[NPC.target].Center;
                            int num329 = 50;
                            if (NPC.type >= 494 && NPC.type <= 495)
                            {
                                num329 = 42;
                            }
                            if (vector25.Length() < (float)num329 && Collision.CanHit(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1))
                            {
                                NPC.velocity.X = NPC.velocity.X * 0.7f;
                                NPC.ai[2] = 1f;
                            }
                        }
                    }
                    else
                    {
                        NPC.damage = (int)((double)NPC.defDamage * 1.5);
                        NPC.ai[3] = 1f;
                        NPC.velocity.X = NPC.velocity.X * 0.9f;
                        if ((double)Math.Abs(NPC.velocity.X) < 0.1)
                        {
                            NPC.velocity.X = 0f;
                        }
                        NPC.ai[2] += 1f;
                        if (NPC.ai[2] >= 20f || NPC.velocity.Y != 0f || (Main.dayTime && (double)NPC.position.Y < Main.worldSurface * 16.0))
                        {
                            NPC.ai[2] = 0f;
                        }
                    }
                }
                goto IL_6bf9;
            }
            if (NPC.type == 159)
            {
                if (NPC.velocity.X > 0f && NPC.direction < 0)
                {
                    goto IL_41b9;
                }
                if (NPC.velocity.X < 0f && NPC.direction > 0)
                {
                    goto IL_41b9;
                }
            }
            goto IL_41d5;
        IL_1f22:
            if (Collision.CanHit(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1))
            {
                NPC.ai[2] = -1f;
                NPC.netUpdate = true;
                NPC.TargetClosest(true);
            }
            goto IL_21b6;
        IL_0b35:
            if (NPC.velocity.Y == 0f && NPC.Distance(Main.player[NPC.target].Center) < 900f && Collision.CanHit(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1))
            {
                NPC.ai[2] = 0f - (float)num410 - (float)num408;
                NPC.netUpdate = true;
            }
            goto IL_119a;
        IL_b2a6:
            bool flag33;
            int num200;
            int num199;
            bool flag50;
            if (flag33)
            {
                num200 = (int)((NPC.position.X + (float)(NPC.width / 2) + (float)(15 * NPC.direction)) / 16f);
                num199 = (int)((NPC.position.Y + (float)NPC.height - 15f) / 16f);
                if (NPC.type == 109 || NPC.type == 163 || NPC.type == 164 || true || NPC.type == 199 || NPC.type == 236 || NPC.type == 239 || NPC.type == 257 || NPC.type == 258 || NPC.type == 290 || NPC.type == 391 || NPC.type == 425 || NPC.type == 427 || NPC.type == 426 || NPC.type == 508 || NPC.type == 415 || NPC.type == 530 || NPC.type == 532)
                {
                    num200 = (int)((NPC.position.X + (float)(NPC.width / 2) + (float)((NPC.width / 2 + 16) * NPC.direction)) / 16f);
                }
                /*
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
                if (Main.tile[num200 + NPC.direction, num199 - 1] == null)
                {
                    Tile[,] tile13 = Main.tile;
                    int num428 = num200 + NPC.direction;
                    int num429 = num199 - 1;
                    Tile tile14 = new Tile();
                    tile13[num428, num429] = tile14;
                }
                if (Main.tile[num200 + NPC.direction, num199 + 1] == null)
                {
                    Tile[,] tile15 = Main.tile;
                    int num430 = num200 + NPC.direction;
                    int num431 = num199 + 1;
                    Tile tile16 = new Tile();
                    tile15[num430, num431] = tile16;
                }
                if (Main.tile[num200 - NPC.direction, num199 + 1] == null)
                {
                    Tile[,] tile17 = Main.tile;
                    int num432 = num200 - NPC.direction;
                    int num433 = num199 + 1;
                    Tile tile18 = new Tile();
                    tile17[num432, num433] = tile18;
                }
                Main.tile[num200, num199 + 1].IsHalfBlock;
                */
                if ((Main.tile[num200, num199 - 1].HasUnactuatedTile && (TileLoader.IsClosedDoor(Main.tile[num200, num199 - 1]) || Main.tile[num200, num199 - 1].TileType == 388)) & flag50)
                {
                    NPC.ai[2] += 1f;
                    NPC.ai[3] = 0f;
                    if (NPC.ai[2] >= 60f)
                    {
                        if (!Main.bloodMoon && (NPC.type == 3 || NPC.type == 331 || NPC.type == 332 || NPC.type == 132 || NPC.type == 161 || NPC.type == 186 || NPC.type == 187 || NPC.type == 188 || NPC.type == 189 || NPC.type == 200 || NPC.type == 223 || NPC.type == 320 || NPC.type == 321 || NPC.type == 319))
                        {
                            NPC.ai[1] = 0f;
                        }
                        NPC.velocity.X = 0.5f * (0f - (float)NPC.direction);
                        int num198 = 5;
                        if (Main.tile[num200, num199 - 1].TileType == 388)
                        {
                            num198 = 2;
                        }
                        NPC.ai[1] += (float)num198;
                        if (NPC.type == 27)
                        {
                            NPC.ai[1] += 1f;
                        }
                        if (NPC.type == 31 || NPC.type == 294 || NPC.type == 295 || NPC.type == 296)
                        {
                            NPC.ai[1] += 6f;
                        }
                        NPC.ai[2] = 0f;
                        bool flag32 = false;
                        if (NPC.ai[1] >= 10f)
                        {
                            flag32 = true;
                            NPC.ai[1] = 10f;
                        }
                        if (NPC.type == 460)
                        {
                            flag32 = true;
                        }
                        WorldGen.KillTile(num200, num199 - 1, true, false, false);
                        if (((Main.netMode != 1 || !flag32) & flag32) && Main.netMode != 1)
                        {
                            if (NPC.type == 26)
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
                                    bool flag31 = WorldGen.OpenDoor(num200, num199 - 1, NPC.direction);
                                    if (!flag31)
                                    {
                                        NPC.ai[3] = (float)num382;
                                        NPC.netUpdate = true;
                                    }
                                    if (Main.netMode == 2 & flag31)
                                    {
                                        NetMessage.SendData(19, -1, -1, null, 0, (float)num200, (float)(num199 - 1), (float)NPC.direction, 0, 0, 0);
                                    }
                                }
                                if (Main.tile[num200, num199 - 1].TileType == 388)
                                {
                                    bool flag30 = WorldGen.ShiftTallGate(num200, num199 - 1, false);
                                    if (!flag30)
                                    {
                                        NPC.ai[3] = (float)num382;
                                        NPC.netUpdate = true;
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
                int num197 = NPC.spriteDirection;
                if (NPC.type == 425)
                {
                    num197 *= -1;
                }
                if (NPC.velocity.X < 0f && num197 == -1)
                {
                    goto IL_bac7;
                }
                if (NPC.velocity.X > 0f && num197 == 1)
                {
                    goto IL_bac7;
                }
                goto IL_bdce;
            }
            if (flag50)
            {
                NPC.ai[1] = 0f;
                NPC.ai[2] = 0f;
            }
            goto IL_c227;
        IL_3498:
            NPC.velocity.X = NPC.velocity.X * 1.75f;
            NPC.velocity.Y = NPC.velocity.Y - 4.5f;
            if (NPC.Center.Y - Main.player[NPC.target].Center.Y > 20f)
            {
                NPC.velocity.Y = NPC.velocity.Y - 0.5f;
            }
            if (NPC.Center.Y - Main.player[NPC.target].Center.Y > 40f)
            {
                NPC.velocity.Y = NPC.velocity.Y - 1f;
            }
            if (NPC.Center.Y - Main.player[NPC.target].Center.Y > 80f)
            {
                NPC.velocity.Y = NPC.velocity.Y - 1.5f;
            }
            if (NPC.Center.Y - Main.player[NPC.target].Center.Y > 100f)
            {
                NPC.velocity.Y = NPC.velocity.Y - 1.5f;
            }
            if (Math.Abs(NPC.velocity.X) > 7f)
            {
                if (NPC.velocity.X < 0f)
                {
                    NPC.velocity.X = -7f;
                }
                else
                {
                    NPC.velocity.X = 7f;
                }
            }
            goto IL_362f;
        IL_8a60:
            if (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
            {
                Vector2 vector31 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + 20f);
                vector31.X += (float)(10 * NPC.direction);
                float num286 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector31.X;
                float num285 = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - vector31.Y;
                num286 += (float)Main.rand.Next(-40, 41);
                num285 += (float)Main.rand.Next(-40, 41);
                float num282 = (float)Math.Sqrt((double)(num286 * num286 + num285 * num285));
                NPC.netUpdate = true;
                num282 = 15f / num282;
                num286 *= num282;
                num285 *= num282;
                int num278 = 32;
                int num277 = 257;
                vector31.X += num286 * 3f;
                vector31.Y += num285 * 3f;
                Projectile.NewProjectile(NPC.GetSource_FromAI(), vector31.X, vector31.Y, num286, num285, num277, num278, 0f, Main.myPlayer, 0f, 0f);
                NPC.ai[2] = 0f;
            }
            goto IL_8c30;
        IL_8c30:
            if (NPC.type == 251)
            {
                if (NPC.justHit)
                {
                    NPC.ai[2] -= (float)Main.rand.Next(30);
                }
                if (NPC.ai[2] < 0f)
                {
                    NPC.ai[2] = 0f;
                }
                if (NPC.confused)
                {
                    NPC.ai[2] = 0f;
                }
                NPC.ai[2] += 1f;
                float num276 = (float)Main.rand.Next(60, 1800);
                num276 *= (float)NPC.life / (float)NPC.lifeMax;
                num276 += 15f;
                if (Main.netMode != 1 && NPC.ai[2] >= num276 && NPC.velocity.Y == 0f && !Main.player[NPC.target].dead && !Main.player[NPC.target].frozen)
                {
                    if (NPC.direction > 0 && NPC.Center.X < Main.player[NPC.target].Center.X)
                    {
                        goto IL_8d96;
                    }
                    if (NPC.direction < 0 && NPC.Center.X > Main.player[NPC.target].Center.X)
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
            if (NPC.type == 468 && !Main.eclipse)
            {
                flag39 = true;
            }
            else if ((NPC.ai[2] <= 0f | flag39) && (NPC.velocity.Y == 0f | flag38) && NPC.ai[1] <= 0f && !Main.player[NPC.target].dead)
            {
                bool flag35 = Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height);
                if (NPC.type == 520)
                {
                    flag35 = Collision.CanHitLine(NPC.Top + new Vector2(0f, 20f), 0, 0, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height);
                }
                if (Main.player[NPC.target].stealth == 0f && Main.player[NPC.target].itemAnimation == 0)
                {
                    flag35 = false;
                }
                if (flag35)
                {
                    float num225 = 10f;
                    Vector2 vector23 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                    float num224 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector23.X;
                    float num223 = Math.Abs(num224) * 0.1f;
                    float num222 = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - vector23.Y - num223;
                    num224 += (float)Main.rand.Next(-40, 41);
                    num222 += (float)Main.rand.Next(-40, 41);
                    float num219 = (float)Math.Sqrt((double)(num224 * num224 + num222 * num222));
                    float num218 = 700f;
                    if (NPC.type == 214)
                    {
                        num218 = 550f;
                    }
                    if (NPC.type == 215)
                    {
                        num218 = 800f;
                    }
                    if (NPC.type >= 498 && NPC.type <= 506)
                    {
                        num218 = 190f;
                    }
                    if (NPC.type >= 449 && NPC.type <= 452)
                    {
                        num218 = 200f;
                    }
                    if (NPC.type == 481)
                    {
                        num218 = 400f;
                    }
                    if (NPC.type == 468)
                    {
                        num218 = 400f;
                    }
                    if (num219 < num218)
                    {
                        NPC.netUpdate = true;
                        NPC.velocity.X = NPC.velocity.X * 0.5f;
                        num219 = num225 / num219;
                        num224 *= num219;
                        num222 *= num219;
                        NPC.ai[2] = 3f;
                        NPC.ai[1] = (float)num250;
                        if (Math.Abs(num222) > Math.Abs(num224) * 2f)
                        {
                            if (num222 > 0f)
                            {
                                NPC.ai[2] = 1f;
                            }
                            else
                            {
                                NPC.ai[2] = 5f;
                            }
                        }
                        else if (Math.Abs(num224) > Math.Abs(num222) * 2f)
                        {
                            NPC.ai[2] = 3f;
                        }
                        else if (num222 > 0f)
                        {
                            NPC.ai[2] = 2f;
                        }
                        else
                        {
                            NPC.ai[2] = 4f;
                        }
                    }
                }
            }
            if (NPC.ai[2] <= 0f || (flag39 && (num252 == -1 || NPC.ai[1] < (float)num252 || NPC.ai[1] >= (float)(num252 + num251))))
            {
                float num214 = 1f;
                float num213 = 0.07f;
                float scaleFactor6 = 0.8f;
                if (NPC.type == 214)
                {
                    num214 = 2f;
                    num213 = 0.09f;
                }
                else if (NPC.type == 215)
                {
                    num214 = 1.5f;
                    num213 = 0.08f;
                }
                else if (NPC.type == 381 || NPC.type == 382)
                {
                    num214 = 2f;
                    num213 = 0.5f;
                }
                else if (NPC.type == 520)
                {
                    num214 = 4f;
                    num213 = 1f;
                    scaleFactor6 = 0.7f;
                }
                else if (NPC.type == 411)
                {
                    num214 = 2f;
                    num213 = 0.5f;
                }
                else if (NPC.type == 409)
                {
                    num214 = 2f;
                    num213 = 0.5f;
                }
                bool flag34 = false;
                if ((NPC.type == 381 || NPC.type == 382) && Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) < 300f && Collision.CanHitLine(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0))
                {
                    flag34 = true;
                    NPC.ai[3] = 0f;
                }
                if (NPC.type == 520 && Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) < 400f && Collision.CanHitLine(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0))
                {
                    flag34 = true;
                    NPC.ai[3] = 0f;
                }
                if ((NPC.velocity.X < 0f - num214 || NPC.velocity.X > num214) | flag34)
                {
                    if (NPC.velocity.Y == 0f)
                    {
                        NPC.velocity *= scaleFactor6;
                    }
                }
                else if (NPC.velocity.X < num214 && NPC.direction == 1)
                {
                    NPC.velocity.X = NPC.velocity.X + num213;
                    if (NPC.velocity.X > num214)
                    {
                        NPC.velocity.X = num214;
                    }
                }
                else if (NPC.velocity.X > 0f - num214 && NPC.direction == -1)
                {
                    NPC.velocity.X = NPC.velocity.X - num213;
                    if (NPC.velocity.X < 0f - num214)
                    {
                        NPC.velocity.X = 0f - num214;
                    }
                }
            }
            if (NPC.type == 520)
            {
                NPC.localAI[2] += 1f;
                if (NPC.localAI[2] >= 6f)
                {
                    NPC.localAI[2] = 0f;
                    NPC.localAI[3] = Main.player[NPC.target].DirectionFrom(NPC.Top + new Vector2(0f, 20f)).ToRotation();
                }
            }
            goto IL_aba2;
        IL_c15a:
            if (NPC.type == 287 && NPC.velocity.Y < 0f)
            {
                NPC.velocity.X = NPC.velocity.X * 1.2f;
                NPC.velocity.Y = NPC.velocity.Y * 1.1f;
            }
            if (NPC.type == 460 && NPC.velocity.Y < 0f)
            {
                NPC.velocity.X = NPC.velocity.X * 1.3f;
                NPC.velocity.Y = NPC.velocity.Y * 1.1f;
            }
            goto IL_c227;
        IL_a3fa:
            NPC.ai[2] = 0f;
            NPC.ai[1] = 0f;
            goto IL_a47a;
        IL_1c13:
            if (Collision.CanHit(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1))
            {
                NPC.ai[2] = -1f;
                NPC.netUpdate = true;
                NPC.TargetClosest(true);
            }
            goto IL_1e03;
        IL_8d96:
            if (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
            {
                Vector2 vector32 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + 12f);
                vector32.X += (float)(6 * NPC.direction);
                float num273 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector32.X;
                float num272 = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - vector32.Y;
                num273 += (float)Main.rand.Next(-40, 41);
                num272 += (float)Main.rand.Next(-30, 0);
                float num269 = (float)Math.Sqrt((double)(num273 * num273 + num272 * num272));
                NPC.netUpdate = true;
                num269 = 15f / num269;
                num273 *= num269;
                num272 *= num269;
                int num265 = 30;
                int num264 = 83;
                vector32.X += num273 * 3f;
                vector32.Y += num272 * 3f;
                Projectile.NewProjectile(NPC.GetSource_FromAI(), vector32.X, vector32.Y, num273, num272, num264, num265, 0f, Main.myPlayer, 0f, 0f);
                NPC.ai[2] = 0f;
            }
            goto IL_8f61;
        IL_41d5:
            if (NPC.velocity.X < -6f || NPC.velocity.X > 6f)
            {
                if (NPC.velocity.Y == 0f)
                {
                    NPC.velocity *= 0.8f;
                }
            }
            else if (NPC.velocity.X < 6f && NPC.direction == 1)
            {
                if (NPC.velocity.Y == 0f && NPC.velocity.X < 0f)
                {
                    NPC.velocity.X = NPC.velocity.X * 0.99f;
                }
                NPC.velocity.X = NPC.velocity.X + 0.07f;
                if (NPC.velocity.X > 6f)
                {
                    NPC.velocity.X = 6f;
                }
            }
            else if (NPC.velocity.X > -6f && NPC.direction == -1)
            {
                if (NPC.velocity.Y == 0f && NPC.velocity.X > 0f)
                {
                    NPC.velocity.X = NPC.velocity.X * 0.99f;
                }
                NPC.velocity.X = NPC.velocity.X - 0.07f;
                if (NPC.velocity.X < -6f)
                {
                    NPC.velocity.X = -6f;
                }
            }
            goto IL_6bf9;
        IL_c227:
            if (Main.netMode != 1 && NPC.type == 120 && NPC.ai[3] >= (float)num382)
            {
                int num196 = (int)Main.player[NPC.target].position.X / 16;
                int num195 = (int)Main.player[NPC.target].position.Y / 16;
                int num194 = (int)NPC.position.X / 16;
                int num193 = (int)NPC.position.Y / 16;
                int num192 = 20;
                int num191 = 0;
                bool flag29 = false;
                if (Math.Abs(NPC.position.X - Main.player[NPC.target].position.X) + Math.Abs(NPC.position.Y - Main.player[NPC.target].position.Y) > 2000f)
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
                        if ((num189 < num195 - 4 || num189 > num195 + 4 || num190 < num196 - 4 || num190 > num196 + 4) && (num189 < num193 - 1 || num189 > num193 + 1 || num190 < num194 - 1 || num190 > num194 + 1) && Main.tile[num190, num189].HasUnactuatedTile)
                        {
                            bool flag28 = true;
                            if (NPC.type == 32 && Main.tile[num190, num189 - 1].WallType == 0)
                            {
                                flag28 = false;
                            }
                            else if ((Main.tile[num190, num189 - 1].LiquidType == LiquidID.Lava))
                            {
                                flag28 = false;
                            }
                            if (flag28 && Main.tileSolid[Main.tile[num190, num189].TileType] && !Collision.SolidTiles(num190 - 1, num190 + 1, num189 - 4, num189 - 1))
                            {
                                NPC.position.X = (float)(num190 * 16 - NPC.width / 2);
                                NPC.position.Y = (float)(num189 * 16 - NPC.height);
                                NPC.netUpdate = true;
                                NPC.ai[3] = -120f;
                            }
                        }
                    }
                }
            }
            return;
        IL_bac7:
            if (NPC.height >= 32 && Main.tile[num200, num199 - 2].HasUnactuatedTile && Main.tileSolid[Main.tile[num200, num199 - 2].TileType])
            {
                if (Main.tile[num200, num199 - 3].HasUnactuatedTile && Main.tileSolid[Main.tile[num200, num199 - 3].TileType])
                {
                    NPC.velocity.Y = -8f;
                    NPC.netUpdate = true;
                }
                else
                {
                    NPC.velocity.Y = -7f;
                    NPC.netUpdate = true;
                }
            }
            else if (Main.tile[num200, num199 - 1].HasUnactuatedTile && Main.tileSolid[Main.tile[num200, num199 - 1].TileType])
            {
                NPC.velocity.Y = -6f;
                NPC.netUpdate = true;
            }
            else if (NPC.position.Y + (float)NPC.height - (float)(num199 * 16) > 20f && Main.tile[num200, num199].HasUnactuatedTile && !Main.tile[num200, num199].TopSlope && Main.tileSolid[Main.tile[num200, num199].TileType])
            {
                NPC.velocity.Y = -5f;
                NPC.netUpdate = true;
            }
            else if (NPC.directionY < 0 && NPC.type != 67 && (!Main.tile[num200, num199 + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num200, num199 + 1].TileType]) && (!Main.tile[num200 + NPC.direction, num199 + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num200 + NPC.direction, num199 + 1].TileType]))
            {
                NPC.velocity.Y = -8f;
                NPC.velocity.X = NPC.velocity.X * 1.5f;
                NPC.netUpdate = true;
            }
            else if (flag50)
            {
                NPC.ai[1] = 0f;
                NPC.ai[2] = 0f;
            }
            bool flag52;
            if ((NPC.velocity.Y == 0f & flag52) && NPC.ai[3] == 1f)
            {
                NPC.velocity.Y = -5f;
            }
            goto IL_bdce;
        IL_c12f:
            NPC.velocity.X = (float)(8 * NPC.direction);
            NPC.velocity.Y = -4f;
            NPC.netUpdate = true;
            goto IL_c15a;
        IL_2dcc:
            if ((NPC.position.X == NPC.oldPosition.X || NPC.ai[3] >= (float)num382) | flag51)
            {
                NPC.ai[3] += 1f;
            }
            else if ((double)Math.Abs(NPC.velocity.X) > 0.9 && NPC.ai[3] > 0f)
            {
                NPC.ai[3] -= 1f;
            }
            if (NPC.ai[3] > (float)(num382 * 10))
            {
                NPC.ai[3] = 0f;
            }
            if (NPC.justHit)
            {
                NPC.ai[3] = 0f;
            }
            if (NPC.ai[3] == (float)num382)
            {
                NPC.netUpdate = true;
            }
            goto IL_2e95;
        IL_6bf9:
            if (NPC.type >= 277 && NPC.type <= 280)
            {
                Lighting.AddLight((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16, 0.2f, 0.1f, 0f);
            }
            else if (NPC.type == 520)
            {
                Lighting.AddLight(NPC.Top + new Vector2(0f, 20f), 0.3f, 0.3f, 0.7f);
            }
            else if (NPC.type == 525)
            {
                Vector3 rgb5 = new Vector3(0.7f, 1f, 0.2f) * 0.5f;
                Lighting.AddLight(NPC.Top + new Vector2(0f, 15f), rgb5);
            }
            else if (NPC.type == 526)
            {
                Vector3 rgb4 = new Vector3(1f, 1f, 0.5f) * 0.4f;
                Lighting.AddLight(NPC.Top + new Vector2(0f, 15f), rgb4);
            }
            else if (NPC.type == 527)
            {
                Vector3 rgb3 = new Vector3(0.6f, 0.3f, 1f) * 0.4f;
                Lighting.AddLight(NPC.Top + new Vector2(0f, 15f), rgb3);
            }
            else if (NPC.type == 415)
            {
                NPC.hide = false;
                int num323 = 0;
                while (num323 < 200)
                {
                    if (!Main.npc[num323].active || Main.npc[num323].type != 416 || Main.npc[num323].ai[0] != (float)NPC.whoAmI)
                    {
                        num323++;
                        continue;
                    }
                    NPC.hide = true;
                    break;
                }
            }
            else if (NPC.type == 258)
            {
                if (NPC.velocity.Y != 0f)
                {
                    NPC.TargetClosest(true);
                    NPC.spriteDirection = NPC.direction;
                    if (Main.player[NPC.target].Center.X < NPC.position.X && NPC.velocity.X > 0f)
                    {
                        NPC.velocity.X = NPC.velocity.X * 0.95f;
                    }
                    else if (Main.player[NPC.target].Center.X > NPC.position.X + (float)NPC.width && NPC.velocity.X < 0f)
                    {
                        NPC.velocity.X = NPC.velocity.X * 0.95f;
                    }
                    if (Main.player[NPC.target].Center.X < NPC.position.X && NPC.velocity.X > -5f)
                    {
                        NPC.velocity.X = NPC.velocity.X - 0.1f;
                    }
                    else if (Main.player[NPC.target].Center.X > NPC.position.X + (float)NPC.width && NPC.velocity.X < 5f)
                    {
                        NPC.velocity.X = NPC.velocity.X + 0.1f;
                    }
                }
                else if (Main.player[NPC.target].Center.Y + 50f < NPC.position.Y && Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
                {
                    NPC.velocity.Y = -7f;
                }
            }
            else if (NPC.type == 425)
            {
                if (NPC.velocity.Y == 0f)
                {
                    NPC.ai[2] = 0f;
                }
                if (NPC.velocity.Y != 0f && NPC.ai[2] == 1f)
                {
                    NPC.TargetClosest(true);
                    NPC.spriteDirection = -NPC.direction;
                    if (Collision.CanHit(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0))
                    {
                        float num322 = Main.player[NPC.target].Center.X - (float)(NPC.direction * 400) - NPC.Center.X;
                        float num321 = Main.player[NPC.target].Bottom.Y - NPC.Bottom.Y;
                        if (num322 < 0f && NPC.velocity.X > 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X * 0.9f;
                        }
                        else if (num322 > 0f && NPC.velocity.X < 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X * 0.9f;
                        }
                        if (num322 < 0f && NPC.velocity.X > -5f)
                        {
                            NPC.velocity.X = NPC.velocity.X - 0.1f;
                        }
                        else if (num322 > 0f && NPC.velocity.X < 5f)
                        {
                            NPC.velocity.X = NPC.velocity.X + 0.1f;
                        }
                        if (NPC.velocity.X > 6f)
                        {
                            NPC.velocity.X = 6f;
                        }
                        if (NPC.velocity.X < -6f)
                        {
                            NPC.velocity.X = -6f;
                        }
                        if (num321 < -20f && NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y = NPC.velocity.Y * 0.8f;
                        }
                        else if (num321 > 20f && NPC.velocity.Y < 0f)
                        {
                            NPC.velocity.Y = NPC.velocity.Y * 0.8f;
                        }
                        if (num321 < -20f && NPC.velocity.Y > -5f)
                        {
                            NPC.velocity.Y = NPC.velocity.Y - 0.3f;
                        }
                        else if (num321 > 20f && NPC.velocity.Y < 5f)
                        {
                            NPC.velocity.Y = NPC.velocity.Y + 0.3f;
                        }
                    }
                    if (Main.rand.Next(3) == 0)
                    {
                        Vector2 position = NPC.Center + new Vector2((float)(NPC.direction * -14), -8f) - Vector2.One * 4f;
                        Vector2 velocity = new Vector2((float)(NPC.direction * -6), 12f) * 0.2f + Utils.RandomVector2(Main.rand, -1f, 1f) * 0.1f;
                        Dust obj4 = Main.dust[Dust.NewDust(position, 8, 8, 229, velocity.X, velocity.Y, 100, Color.Transparent, 1f + Main.rand.NextFloat() * 0.5f)];
                        obj4.noGravity = true;
                        obj4.velocity = velocity;
                        obj4.customData = NPC;
                    }
                    for (int num320 = 0; num320 < 200; num320++)
                    {
                        if (num320 != NPC.whoAmI && Main.npc[num320].active && Main.npc[num320].type == NPC.type && Math.Abs(NPC.position.X - Main.npc[num320].position.X) + Math.Abs(NPC.position.Y - Main.npc[num320].position.Y) < (float)NPC.width)
                        {
                            if (NPC.position.X < Main.npc[num320].position.X)
                            {
                                NPC.velocity.X = NPC.velocity.X - 0.05f;
                            }
                            else
                            {
                                NPC.velocity.X = NPC.velocity.X + 0.05f;
                            }
                            if (NPC.position.Y < Main.npc[num320].position.Y)
                            {
                                NPC.velocity.Y = NPC.velocity.Y - 0.05f;
                            }
                            else
                            {
                                NPC.velocity.Y = NPC.velocity.Y + 0.05f;
                            }
                        }
                    }
                }
                else if (Main.player[NPC.target].Center.Y + 100f < NPC.position.Y && Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
                {
                    NPC.velocity.Y = -5f;
                    NPC.ai[2] = 1f;
                }
                if (Main.netMode != 1)
                {
                    NPC.localAI[2] += 1f;
                    if (NPC.localAI[2] >= (float)(360 + Main.rand.Next(360)) && NPC.Distance(Main.player[NPC.target].Center) < 400f && Math.Abs(NPC.DirectionTo(Main.player[NPC.target].Center).Y) < 0.5f && Collision.CanHitLine(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0))
                    {
                        NPC.localAI[2] = 0f;
                        Vector2 vector29 = NPC.Center + new Vector2((float)(NPC.direction * 30), 2f);
                        Vector2 vector28 = NPC.DirectionTo(Main.player[NPC.target].Center) * 7f;
                        if (vector28.HasNaNs())
                        {
                            vector28 = new Vector2((float)(NPC.direction * 8), 0f);
                        }
                        int num319 = Main.expertMode ? 50 : 75;
                        for (int num318 = 0; num318 < 4; num318++)
                        {
                            Vector2 vector27 = vector28 + Utils.RandomVector2(Main.rand, -0.8f, 0.8f);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), vector29.X, vector29.Y, vector27.X, vector27.Y, 577, num319, 1f, Main.myPlayer, 0f, 0f);
                        }
                    }
                }
            }
            else if (NPC.type == 427)
            {
                if (NPC.velocity.Y == 0f)
                {
                    NPC.ai[2] = 0f;
                    NPC.rotation = 0f;
                }
                else
                {
                    NPC.rotation = NPC.velocity.X * 0.1f;
                }
                if (NPC.velocity.Y != 0f && NPC.ai[2] == 1f)
                {
                    NPC.TargetClosest(true);
                    NPC.spriteDirection = -NPC.direction;
                    if (Collision.CanHit(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0))
                    {
                        float num317 = Main.player[NPC.target].Center.X - NPC.Center.X;
                        float num316 = Main.player[NPC.target].Center.Y - NPC.Center.Y;
                        if (num317 < 0f && NPC.velocity.X > 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X * 0.98f;
                        }
                        else if (num317 > 0f && NPC.velocity.X < 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X * 0.98f;
                        }
                        if (num317 < -20f && NPC.velocity.X > -6f)
                        {
                            NPC.velocity.X = NPC.velocity.X - 0.015f;
                        }
                        else if (num317 > 20f && NPC.velocity.X < 6f)
                        {
                            NPC.velocity.X = NPC.velocity.X + 0.015f;
                        }
                        if (NPC.velocity.X > 6f)
                        {
                            NPC.velocity.X = 6f;
                        }
                        if (NPC.velocity.X < -6f)
                        {
                            NPC.velocity.X = -6f;
                        }
                        if (num316 < -20f && NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y = NPC.velocity.Y * 0.98f;
                        }
                        else if (num316 > 20f && NPC.velocity.Y < 0f)
                        {
                            NPC.velocity.Y = NPC.velocity.Y * 0.98f;
                        }
                        if (num316 < -20f && NPC.velocity.Y > -6f)
                        {
                            NPC.velocity.Y = NPC.velocity.Y - 0.15f;
                        }
                        else if (num316 > 20f && NPC.velocity.Y < 6f)
                        {
                            NPC.velocity.Y = NPC.velocity.Y + 0.15f;
                        }
                    }
                    for (int num315 = 0; num315 < 200; num315++)
                    {
                        if (num315 != NPC.whoAmI && Main.npc[num315].active && Main.npc[num315].type == NPC.type && Math.Abs(NPC.position.X - Main.npc[num315].position.X) + Math.Abs(NPC.position.Y - Main.npc[num315].position.Y) < (float)NPC.width)
                        {
                            if (NPC.position.X < Main.npc[num315].position.X)
                            {
                                NPC.velocity.X = NPC.velocity.X - 0.05f;
                            }
                            else
                            {
                                NPC.velocity.X = NPC.velocity.X + 0.05f;
                            }
                            if (NPC.position.Y < Main.npc[num315].position.Y)
                            {
                                NPC.velocity.Y = NPC.velocity.Y - 0.05f;
                            }
                            else
                            {
                                NPC.velocity.Y = NPC.velocity.Y + 0.05f;
                            }
                        }
                    }
                }
                else if (Main.player[NPC.target].Center.Y + 100f < NPC.position.Y && Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
                {
                    NPC.velocity.Y = -5f;
                    NPC.ai[2] = 1f;
                }
            }
            else if (NPC.type == 426)
            {
                if (NPC.ai[1] > 0f && NPC.velocity.Y > 0f)
                {
                    NPC.velocity.Y = NPC.velocity.Y * 0.85f;
                    if (NPC.velocity.Y == 0f)
                    {
                        NPC.velocity.Y = -0.4f;
                    }
                }
                if (NPC.velocity.Y != 0f)
                {
                    NPC.TargetClosest(true);
                    NPC.spriteDirection = NPC.direction;
                    if (Collision.CanHit(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0))
                    {
                        float num314 = Main.player[NPC.target].Center.X - (float)(NPC.direction * 300) - NPC.Center.X;
                        if (num314 < 40f && NPC.velocity.X > 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X * 0.98f;
                        }
                        else if (num314 > 40f && NPC.velocity.X < 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X * 0.98f;
                        }
                        if (num314 < 40f && NPC.velocity.X > -5f)
                        {
                            NPC.velocity.X = NPC.velocity.X - 0.2f;
                        }
                        else if (num314 > 40f && NPC.velocity.X < 5f)
                        {
                            NPC.velocity.X = NPC.velocity.X + 0.2f;
                        }
                        if (NPC.velocity.X > 6f)
                        {
                            NPC.velocity.X = 6f;
                        }
                        if (NPC.velocity.X < -6f)
                        {
                            NPC.velocity.X = -6f;
                        }
                    }
                }
                else if (Main.player[NPC.target].Center.Y + 100f < NPC.position.Y && Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
                {
                    NPC.velocity.Y = -6f;
                }
                for (int num313 = 0; num313 < 200; num313++)
                {
                    if (num313 != NPC.whoAmI && Main.npc[num313].active && Main.npc[num313].type == NPC.type && Math.Abs(NPC.position.X - Main.npc[num313].position.X) + Math.Abs(NPC.position.Y - Main.npc[num313].position.Y) < (float)NPC.width)
                    {
                        if (NPC.position.X < Main.npc[num313].position.X)
                        {
                            NPC.velocity.X = NPC.velocity.X - 0.1f;
                        }
                        else
                        {
                            NPC.velocity.X = NPC.velocity.X + 0.1f;
                        }
                        if (NPC.position.Y < Main.npc[num313].position.Y)
                        {
                            NPC.velocity.Y = NPC.velocity.Y - 0.1f;
                        }
                        else
                        {
                            NPC.velocity.Y = NPC.velocity.Y + 0.1f;
                        }
                    }
                }
                if (Main.rand.Next(6) == 0 && NPC.ai[1] <= 20f)
                {
                    Dust obj5 = Main.dust[Dust.NewDust(NPC.Center + new Vector2((float)((NPC.spriteDirection == 1) ? 8 : (-20)), -20f), 8, 8, 229, NPC.velocity.X, NPC.velocity.Y, 100, default(Color), 1f)];
                    obj5.velocity = obj5.velocity / 4f + NPC.velocity / 2f;
                    obj5.scale = 0.6f;
                    obj5.noLight = true;
                }
                if (NPC.ai[1] >= 57f)
                {
                    int num312 = Utils.SelectRandom(Main.rand, 161, 229);
                    Dust obj6 = Main.dust[Dust.NewDust(NPC.Center + new Vector2((float)((NPC.spriteDirection == 1) ? 8 : (-20)), -20f), 8, 8, num312, NPC.velocity.X, NPC.velocity.Y, 100, default(Color), 1f)];
                    obj6.velocity = obj6.velocity / 4f + NPC.DirectionTo(Main.player[NPC.target].Top);
                    obj6.scale = 1.2f;
                    obj6.noLight = true;
                }
                if (Main.rand.Next(6) == 0)
                {
                    Dust dust9 = Main.dust[Dust.NewDust(NPC.Center, 2, 2, 229, 0f, 0f, 0, default(Color), 1f)];
                    dust9.position = NPC.Center + new Vector2((float)((NPC.spriteDirection == 1) ? 26 : (-26)), 24f);
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
            else if (NPC.type == 185)
            {
                if (NPC.velocity.Y == 0f)
                {
                    NPC.rotation = 0f;
                    NPC.localAI[0] = 0f;
                }
                else if (NPC.localAI[0] == 1f)
                {
                    NPC.rotation += NPC.velocity.X * 0.05f;
                }
            }
            else if (NPC.type == 428)
            {
                if (NPC.velocity.Y == 0f)
                {
                    NPC.rotation = 0f;
                }
                else
                {
                    NPC.rotation += NPC.velocity.X * 0.08f;
                }
            }
            if (NPC.type == 159 && Main.netMode != 1)
            {
                Vector2 vector26 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float num434 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector26.X;
                float num310 = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - vector26.Y;
                if ((float)Math.Sqrt((double)(num434 * num434 + num310 * num310)) > 300f)
                {
                    NPC.Transform(158);
                }
            }
            if (NPC.type == 164 || true && Main.netMode != 1 && NPC.velocity.Y == 0f)
            {
                int num309 = (int)NPC.Center.X / 16;
                int num308 = (int)NPC.Center.Y / 16;
                bool flag46 = false;
                for (int num307 = num309 - 1; num307 <= num309 + 1; num307++)
                {
                    for (int num306 = num308 - 1; num306 <= num308 + 1; num306++)
                    {
                        if (Main.tile[num307, num306].WallType > 0)
                        {
                            flag46 = true;
                        }
                    }
                }
                if (flag46)
                {
                    NPC.Transform(NPCType<DuneCreeperWall>());
                }
            }
            if (NPC.type == 239 && Main.netMode != 1 && NPC.velocity.Y == 0f)
            {
                int num305 = (int)NPC.Center.X / 16;
                int num304 = (int)NPC.Center.Y / 16;
                bool flag45 = false;
                for (int num303 = num305 - 1; num303 <= num305 + 1; num303++)
                {
                    for (int num302 = num304 - 1; num302 <= num304 + 1; num302++)
                    {
                        if (Main.tile[num303, num302].WallType > 0)
                        {
                            flag45 = true;
                        }
                    }
                }
                if (flag45)
                {
                    NPC.Transform(240);
                }
            }
            if (NPC.type == 530 && Main.netMode != 1 && NPC.velocity.Y == 0f)
            {
                int num301 = (int)NPC.Center.X / 16;
                int num300 = (int)NPC.Center.Y / 16;
                bool flag44 = false;
                for (int num299 = num301 - 1; num299 <= num301 + 1; num299++)
                {
                    for (int num298 = num300 - 1; num298 <= num300 + 1; num298++)
                    {
                        if (Main.tile[num299, num298].WallType > 0)
                        {
                            flag44 = true;
                        }
                    }
                }
                if (flag44)
                {
                    NPC.Transform(531);
                }
            }
            if (Main.netMode != 1 && Main.expertMode && NPC.target >= 0 && (NPC.type == 163 || NPC.type == 238) && Collision.CanHit(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1))
            {
                NPC.localAI[0] += 1f;
                if (NPC.justHit)
                {
                    NPC.localAI[0] -= (float)Main.rand.Next(20, 60);
                    if (NPC.localAI[0] < 0f)
                    {
                        NPC.localAI[0] = 0f;
                    }
                }
                if (NPC.localAI[0] > (float)Main.rand.Next(180, 900))
                {
                    NPC.localAI[0] = 0f;
                    Vector2 vector30 = Main.player[NPC.target].Center - NPC.Center;
                    vector30.Normalize();
                    vector30 *= 8f;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, vector30.X, vector30.Y, 472, 18, 0f, Main.myPlayer, 0f, 0f);
                }
            }
            if (NPC.type == 163 && Main.netMode != 1 && NPC.velocity.Y == 0f)
            {
                int num297 = (int)NPC.Center.X / 16;
                int num296 = (int)NPC.Center.Y / 16;
                bool flag43 = false;
                for (int num295 = num297 - 1; num295 <= num297 + 1; num295++)
                {
                    for (int num294 = num296 - 1; num294 <= num296 + 1; num294++)
                    {
                        if (Main.tile[num295, num294].WallType > 0)
                        {
                            flag43 = true;
                        }
                    }
                }
                if (flag43)
                {
                    NPC.Transform(238);
                }
            }
            if (NPC.type == 236 && Main.netMode != 1 && NPC.velocity.Y == 0f)
            {
                int num293 = (int)NPC.Center.X / 16;
                int num292 = (int)NPC.Center.Y / 16;
                bool flag42 = false;
                for (int num291 = num293 - 1; num291 <= num293 + 1; num291++)
                {
                    for (int num290 = num292 - 1; num290 <= num292 + 1; num290++)
                    {
                        if (Main.tile[num291, num290].WallType > 0)
                        {
                            flag42 = true;
                        }
                    }
                }
                if (flag42)
                {
                    NPC.Transform(237);
                }
            }
            if (NPC.type == 243)
            {
                if (NPC.justHit && Main.rand.Next(3) == 0)
                {
                    NPC.ai[2] -= (float)Main.rand.Next(30);
                }
                if (NPC.ai[2] < 0f)
                {
                    NPC.ai[2] = 0f;
                }
                if (NPC.confused)
                {
                    NPC.ai[2] = 0f;
                }
                NPC.ai[2] += 1f;
                float num289 = (float)Main.rand.Next(30, 900);
                num289 *= (float)NPC.life / (float)NPC.lifeMax;
                num289 += 30f;
                if (Main.netMode != 1 && NPC.ai[2] >= num289 && NPC.velocity.Y == 0f && !Main.player[NPC.target].dead && !Main.player[NPC.target].frozen)
                {
                    if (NPC.direction > 0 && NPC.Center.X < Main.player[NPC.target].Center.X)
                    {
                        goto IL_8a60;
                    }
                    if (NPC.direction < 0 && NPC.Center.X > Main.player[NPC.target].Center.X)
                    {
                        goto IL_8a60;
                    }
                }
            }
            goto IL_8c30;
        IL_21b6:
            if (NPC.type == 428)
            {
                NPC.localAI[0] += 1f;
                if (NPC.localAI[0] >= 300f)
                {
                    int num435 = (int)NPC.Center.X / 16 - 1;
                    int num386 = (int)NPC.Center.Y / 16 - 1;
                    if (!Collision.SolidTiles(num435, num435 + 2, num386, num386 + 1) && Main.netMode != 1)
                    {
                        NPC.Transform(427);
                        NPC.life = NPC.lifeMax;
                        NPC.localAI[0] = 0f;
                        return;
                    }
                }
                int maxValue3 = (!(NPC.localAI[0] < 60f)) ? ((!(NPC.localAI[0] < 120f)) ? ((!(NPC.localAI[0] < 180f)) ? ((!(NPC.localAI[0] < 240f)) ? ((!(NPC.localAI[0] < 300f)) ? 1 : 1) : 2) : 4) : 8) : 16;
                if (Main.rand.Next(maxValue3) == 0)
                {
                    Dust dust10 = Main.dust[Dust.NewDust(NPC.position, NPC.width, NPC.height, 229, 0f, 0f, 0, default(Color), 1f)];
                    dust10.noGravity = true;
                    dust10.scale = 1f;
                    dust10.noLight = true;
                    dust10.velocity = NPC.DirectionFrom(dust10.position) * dust10.velocity.Length();
                    Dust dust11 = dust10;
                    dust11.position -= dust10.velocity * 5f;
                    Dust expr_23FC_cp_0 = dust10;
                    expr_23FC_cp_0.position.X = expr_23FC_cp_0.position.X + (float)(NPC.direction * 6);
                    Dust expr_2418_cp_0 = dust10;
                    expr_2418_cp_0.position.Y = expr_2418_cp_0.position.Y + 4f;
                }
            }
            if (NPC.type == 427)
            {
                NPC.localAI[0] += 1f;
                NPC.localAI[0] += Math.Abs(NPC.velocity.X) / 2f;
                if (NPC.localAI[0] >= 1200f && Main.netMode != 1)
                {
                    int num436 = (int)NPC.Center.X / 16 - 2;
                    int num384 = (int)NPC.Center.Y / 16 - 3;
                    if (!Collision.SolidTiles(num436, num436 + 4, num384, num384 + 4))
                    {
                        NPC.Transform(426);
                        NPC.life = NPC.lifeMax;
                        NPC.localAI[0] = 0f;
                        return;
                    }
                }
                int maxValue2 = (!(NPC.localAI[0] < 360f)) ? ((!(NPC.localAI[0] < 720f)) ? ((!(NPC.localAI[0] < 1080f)) ? ((!(NPC.localAI[0] < 1440f)) ? ((!(NPC.localAI[0] < 1800f)) ? 1 : 1) : 2) : 6) : 16) : 32;
                if (Main.rand.Next(maxValue2) == 0)
                {
                    Dust obj7 = Main.dust[Dust.NewDust(NPC.position, NPC.width, NPC.height, 229, 0f, 0f, 0, default(Color), 1f)];
                    obj7.noGravity = true;
                    obj7.scale = 1f;
                    obj7.noLight = true;
                }
            }
            flag52 = false;
            if (NPC.velocity.X == 0f)
            {
                flag52 = true;
            }
            if (NPC.justHit)
            {
                flag52 = false;
            }
            if (Main.netMode != 1 && NPC.type == 198 && (double)NPC.life <= (double)NPC.lifeMax * 0.55)
            {
                NPC.Transform(199);
            }
            if (Main.netMode != 1 && NPC.type == 348 && (double)NPC.life <= (double)NPC.lifeMax * 0.55)
            {
                NPC.Transform(349);
            }
            num382 = 60;
            if (NPC.type == 120)
            {
                num382 = 180;
                if (NPC.ai[3] == -120f)
                {
                    NPC.velocity *= 0f;
                    NPC.ai[3] = 0f;
                    SoundEngine.PlaySound(SoundID.Item8, NPC.position);
                    Vector2 vector33 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                    float num381 = NPC.oldPos[2].X + (float)NPC.width * 0.5f - vector33.X;
                    float num380 = NPC.oldPos[2].Y + (float)NPC.height * 0.5f - vector33.Y;
                    float num379 = (float)Math.Sqrt((double)(num381 * num381 + num380 * num380));
                    num379 = 2f / num379;
                    num381 *= num379;
                    num380 *= num379;
                    for (int j = 0; j < 20; j++)
                    {
                        int num373 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 71, num381, num380, 200, default(Color), 2f);
                        Main.dust[num373].noGravity = true;
                        Dust expr_27D4_cp_0 = Main.dust[num373];
                        expr_27D4_cp_0.velocity.X = expr_27D4_cp_0.velocity.X * 2f;
                    }
                    for (int i = 0; i < 20; i++)
                    {
                        int num375 = Dust.NewDust(NPC.oldPos[2], NPC.width, NPC.height, 71, 0f - num381, 0f - num380, 200, default(Color), 2f);
                        Main.dust[num375].noGravity = true;
                        Dust expr_2855_cp_0 = Main.dust[num375];
                        expr_2855_cp_0.velocity.X = expr_2855_cp_0.velocity.X * 2f;
                    }
                }
            }
            flag51 = false;
            flag50 = true;
            if (NPC.type == 343 || NPC.type == 47 || NPC.type == 67 || NPC.type == 109 || NPC.type == 110 || NPC.type == 111 || NPC.type == 120 || NPC.type == 163 || NPC.type == 164 || true || NPC.type == 239 || NPC.type == 168 || NPC.type == 199 || NPC.type == 206 || NPC.type == 214 || NPC.type == 215 || NPC.type == 216 || NPC.type == 217 || NPC.type == 218 || NPC.type == 219 || NPC.type == 220 || NPC.type == 226 || NPC.type == 243 || NPC.type == 251 || NPC.type == 257 || NPC.type == 258 || NPC.type == 290 || NPC.type == 291 || NPC.type == 292 || NPC.type == 293 || NPC.type == 305 || NPC.type == 306 || NPC.type == 307 || NPC.type == 308 || NPC.type == 309 || NPC.type == 348 || NPC.type == 349 || NPC.type == 350 || NPC.type == 351 || NPC.type == 379 || (NPC.type >= 430 && NPC.type <= 436) || NPC.type == 380 || NPC.type == 381 || NPC.type == 382 || NPC.type == 383 || NPC.type == 386 || NPC.type == 391 || (NPC.type >= 449 && NPC.type <= 452) || NPC.type == 466 || NPC.type == 464 || NPC.type == 166 || NPC.type == 469 || NPC.type == 468 || NPC.type == 471 || NPC.type == 470 || NPC.type == 480 || NPC.type == 481 || NPC.type == 482 || NPC.type == 411 || NPC.type == 424 || NPC.type == 409 || (NPC.type >= 494 && NPC.type <= 506) || NPC.type == 425 || NPC.type == 427 || NPC.type == 426 || NPC.type == 428 || NPC.type == 508 || NPC.type == 415 || NPC.type == 419 || NPC.type == 520 || (NPC.type >= 524 && NPC.type <= 527) || NPC.type == 528 || NPC.type == 529 || NPC.type == 530 || NPC.type == 532)
            {
                flag50 = false;
            }
            flag49 = false;
            int num372 = NPC.type;
            if (num372 == 425 || num372 == 471)
            {
                flag49 = true;
            }
            flag48 = true;
            num372 = NPC.type;
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
            if (NPC.ai[2] > 0f)
            {
                flag48 = false;
            }
            goto IL_2d75;
        IL_bfff:
            if (NPC.type == 120 && NPC.velocity.Y < 0f)
            {
                NPC.velocity.Y = NPC.velocity.Y * 1.1f;
            }
            if (NPC.type == 287 && NPC.velocity.Y == 0f && Math.Abs(NPC.position.X + (float)(NPC.width / 2) - (Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2))) < 150f && Math.Abs(NPC.position.Y + (float)(NPC.height / 2) - (Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2))) < 50f)
            {
                if (NPC.direction > 0 && NPC.velocity.X >= 1f)
                {
                    goto IL_c12f;
                }
                if (NPC.direction < 0 && NPC.velocity.X <= -1f)
                {
                    goto IL_c12f;
                }
            }
            goto IL_c15a;
        IL_bf88:
            NPC.velocity.X = NPC.velocity.X * 2f;
            if (NPC.velocity.X > 3f)
            {
                NPC.velocity.X = 3f;
            }
            if (NPC.velocity.X < -3f)
            {
                NPC.velocity.X = -3f;
            }
            NPC.velocity.Y = -4f;
            NPC.netUpdate = true;
            goto IL_bfff;
        IL_509d:
            NPC.velocity.X = NPC.velocity.X * 0.9f;
            goto IL_6bf9;
        IL_aba2:
            if (NPC.type == 109 && Main.netMode != 1 && !Main.player[NPC.target].dead)
            {
                if (NPC.justHit)
                {
                    NPC.ai[2] = 0f;
                }
                NPC.ai[2] += 1f;
                if (NPC.ai[2] > 450f)
                {
                    Vector2 vector21 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f - (float)(NPC.direction * 24), NPC.position.Y + 4f);
                    int num212 = 3 * NPC.direction;
                    int num211 = -5;
                    int num210 = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector21.X, vector21.Y, (float)num212, (float)num211, 75, 0, 0f, Main.myPlayer, 0f, 0f);
                    Main.projectile[num210].timeLeft = 300;
                    NPC.ai[2] = 0f;
                }
            }
            flag33 = false;
            if (NPC.velocity.Y == 0f)
            {
                int num209 = (int)(NPC.position.Y + (float)NPC.height + 7f) / 16;
                int num437 = (int)NPC.position.X / 16;
                int num208 = (int)(NPC.position.X + (float)NPC.width) / 16;
                for (int num207 = num437; num207 <= num208; num207++)
                {
                    if (Main.tile[num207, num209] == null)
                    {
                        return;
                    }
                    if (Main.tile[num207, num209].HasUnactuatedTile && Main.tileSolid[Main.tile[num207, num209].TileType])
                    {
                        flag33 = true;
                        break;
                    }
                }
            }
            if (NPC.type == 428)
            {
                flag33 = false;
            }
            if (NPC.velocity.Y >= 0f)
            {
                num206 = 0;
                if (NPC.velocity.X < 0f)
                {
                    num206 = -1;
                }
                if (NPC.velocity.X > 0f)
                {
                    num206 = 1;
                }
                position2 = NPC.position;
                position2.X += NPC.velocity.X;
                num205 = (int)((position2.X + (float)(NPC.width / 2) + (float)((NPC.width / 2 + 1) * num206)) / 16f);
                num204 = (int)((position2.Y + (float)NPC.height - 1f) / 16f);
                /*
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
                */
                if ((float)(num205 * 16) < position2.X + (float)NPC.width && (float)(num205 * 16 + 16) > position2.X)
                {
                    if (Main.tile[num205, num204].HasUnactuatedTile && !Main.tile[num205, num204].TopSlope && !Main.tile[num205, num204 - 1].TopSlope && Main.tileSolid[Main.tile[num205, num204].TileType] && !Main.tileSolidTop[Main.tile[num205, num204].TileType])
                    {
                        goto IL_afd9;
                    }
                    if (Main.tile[num205, num204 - 1].IsHalfBlock && Main.tile[num205, num204 - 1].HasUnactuatedTile)
                    {
                        goto IL_afd9;
                    }
                }
            }
            goto IL_b2a6;
        IL_41b9:
            NPC.velocity.X = NPC.velocity.X * 0.95f;
            goto IL_41d5;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<Items.Materials.Metals.DuneStone>(), 1, 7, 13));

            base.ModifyNPCLoot(npcLoot);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.DesertCave.Chance * .04f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
                new FlavorTextBestiaryInfoElement("")
            });
        }
    }

    class DuneCreeperWall : ModNPC
    {
        public override string Texture => AssetDirectory.EnemyNPC + Name;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dune Creeper");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.WallCreeperWall];
        }

        public override void SetDefaults()
        {
            NPC.width = 62;
            NPC.height = 62;
            NPC.damage = 26;
            NPC.defense = 12;
            NPC.lifeMax = 180;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.value = 1600f;
            NPC.knockBackResist = .4f;
            NPC.aiStyle = -1;
            NPC.buffImmune[BuffID.Confused] = false;
            NPC.noGravity = true;
            AnimationType = NPCID.WallCreeperWall;
        }

        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead)
            {
                NPC.TargetClosest(true);
            }
            float num2745 = 2f;
            float num2744 = 0.08f;
            Vector2 vector266 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
            float num2743 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2);
            float num2742 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2);
            num2743 = (float)((int)(num2743 / 8f) * 8);
            num2742 = (float)((int)(num2742 / 8f) * 8);
            vector266.X = (float)((int)(vector266.X / 8f) * 8);
            vector266.Y = (float)((int)(vector266.Y / 8f) * 8);
            num2743 -= vector266.X;
            num2742 -= vector266.Y;
            float num2737 = (float)Math.Sqrt((double)(num2743 * num2743 + num2742 * num2742));
            if (num2737 == 0f)
            {
                num2743 = NPC.velocity.X;
                num2742 = NPC.velocity.Y;
            }
            else
            {
                num2737 = num2745 / num2737;
                num2743 *= num2737;
                num2742 *= num2737;
            }
            if (Main.player[NPC.target].dead)
            {
                num2743 = (float)NPC.direction * num2745 / 2f;
                num2742 = (0f - num2745) / 2f;
            }
            NPC.spriteDirection = -1;
            if (!Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
            {
                ref float reference = ref NPC.ai[0];
                reference += 1f;
                if (NPC.ai[0] > 0f)
                {
                    NPC.velocity.Y = NPC.velocity.Y + 0.023f;
                }
                else
                {
                    NPC.velocity.Y = NPC.velocity.Y - 0.023f;
                }
                if (NPC.ai[0] < -100f || NPC.ai[0] > 100f)
                {
                    NPC.velocity.X = NPC.velocity.X + 0.023f;
                }
                else
                {
                    NPC.velocity.X = NPC.velocity.X - 0.023f;
                }
                if (NPC.ai[0] > 200f)
                {
                    NPC.ai[0] = -200f;
                }
                NPC.velocity.X = NPC.velocity.X + num2743 * 0.007f;
                NPC.velocity.Y = NPC.velocity.Y + num2742 * 0.007f;
                NPC.rotation = (float)Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X);
                if ((double)NPC.velocity.X > 1.5)
                {
                    NPC.velocity.X = NPC.velocity.X * 0.9f;
                }
                if ((double)NPC.velocity.X < -1.5)
                {
                    NPC.velocity.X = NPC.velocity.X * 0.9f;
                }
                if ((double)NPC.velocity.Y > 1.5)
                {
                    NPC.velocity.Y = NPC.velocity.Y * 0.9f;
                }
                if ((double)NPC.velocity.Y < -1.5)
                {
                    NPC.velocity.Y = NPC.velocity.Y * 0.9f;
                }
                if (NPC.velocity.X > 3f)
                {
                    NPC.velocity.X = 3f;
                }
                if (NPC.velocity.X < -3f)
                {
                    NPC.velocity.X = -3f;
                }
                if (NPC.velocity.Y > 3f)
                {
                    NPC.velocity.Y = 3f;
                }
                if (NPC.velocity.Y < -3f)
                {
                    NPC.velocity.Y = -3f;
                }
            }
            else
            {
                if (NPC.velocity.X < num2743)
                {
                    NPC.velocity.X = NPC.velocity.X + num2744;
                    if (NPC.velocity.X < 0f && num2743 > 0f)
                    {
                        NPC.velocity.X = NPC.velocity.X + num2744;
                    }
                }
                else if (NPC.velocity.X > num2743)
                {
                    NPC.velocity.X = NPC.velocity.X - num2744;
                    if (NPC.velocity.X > 0f && num2743 < 0f)
                    {
                        NPC.velocity.X = NPC.velocity.X - num2744;
                    }
                }
                if (NPC.velocity.Y < num2742)
                {
                    NPC.velocity.Y = NPC.velocity.Y + num2744;
                    if (NPC.velocity.Y < 0f && num2742 > 0f)
                    {
                        NPC.velocity.Y = NPC.velocity.Y + num2744;
                    }
                }
                else if (NPC.velocity.Y > num2742)
                {
                    NPC.velocity.Y = NPC.velocity.Y - num2744;
                    if (NPC.velocity.Y > 0f && num2742 < 0f)
                    {
                        NPC.velocity.Y = NPC.velocity.Y - num2744;
                    }
                }
                NPC.rotation = (float)Math.Atan2((double)num2742, (double)num2743);
            }
            if (NPC.type == 531)
            {
                NPC.rotation += 1.57079637f;
            }
            float num2733 = 0.5f;
            if (NPC.collideX)
            {
                NPC.netUpdate = true;
                NPC.velocity.X = NPC.oldVelocity.X * (0f - num2733);
                if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 2f)
                {
                    NPC.velocity.X = 2f;
                }
                if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -2f)
                {
                    NPC.velocity.X = -2f;
                }
            }
            if (NPC.collideY)
            {
                NPC.netUpdate = true;
                NPC.velocity.Y = NPC.oldVelocity.Y * (0f - num2733);
                if (NPC.velocity.Y > 0f && (double)NPC.velocity.Y < 1.5)
                {
                    NPC.velocity.Y = 2f;
                }
                if (NPC.velocity.Y < 0f && (double)NPC.velocity.Y > -1.5)
                {
                    NPC.velocity.Y = -2f;
                }
            }
            if (NPC.velocity.X > 0f && NPC.oldVelocity.X < 0f)
            {
                update();
            }
            if (NPC.velocity.X < 0f && NPC.oldVelocity.X > 0f)
            {
                update();
            }
            if (NPC.velocity.Y > 0f && NPC.oldVelocity.Y < 0f)
            {
                update();
            }
            if (NPC.velocity.Y < 0f && NPC.oldVelocity.Y > 0f)
            {
                update();
            }
            transform();
        }

        public void update()
        {
            if (!NPC.justHit)
            {
                NPC.netUpdate = true;
            }
            transform();
        }

        public void transform()
        {
            if (Main.netMode != 1)
            {
                int num2732 = (int)NPC.Center.X / 16;
                int num2731 = (int)NPC.Center.Y / 16;
                bool flag204 = false;
                int num3549;
                for (int num2730 = num2732 - 1; num2730 <= num2732 + 1; num2730 = num3549 + 1)
                {
                    for (int num2729 = num2731 - 1; num2729 <= num2731 + 1; num2729 = num3549 + 1)
                    {
                        if (Main.tile[num2730, num2729] == null)
                        {
                            return;
                        }
                        if (Main.tile[num2730, num2729].WallType > 0)
                        {
                            flag204 = true;
                        }
                        num3549 = num2729;
                    }
                    num3549 = num2730;
                }
                if (!flag204)
                {
                    NPC.Transform(NPCType<DuneCreeper>());
                }
            }
        }

        public override void OnKill()
        {
            Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ItemType<Items.Materials.Metals.DuneStone>(), Main.rand.Next(7, 14));
        }
    }
}
