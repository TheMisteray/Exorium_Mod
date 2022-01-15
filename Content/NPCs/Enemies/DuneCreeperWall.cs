using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Items.Placeables;
using Microsoft.Xna.Framework;
using System;

namespace ExoriumMod.NPCs.Enemies
{
    class DuneCreeperWall : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dune Creeper");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.WallCreeperWall];
        }

        public override void SetDefaults()
        {
            npc.width = 60;
            npc.height = 120;
            npc.damage = 14;
            npc.defense = 12;
            npc.lifeMax = 120;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.value = 1600f;
            npc.knockBackResist = .4f;
            npc.aiStyle = -1;
            npc.buffImmune[BuffID.Confused] = false;
            npc.noGravity = true;
            animationType = NPCID.WallCreeperWall;
        }

        public override void AI()
        {
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead)
            {
                npc.TargetClosest(true);
            }
            float num2745 = 2f;
            float num2744 = 0.08f;
            Vector2 vector266 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
            float num2743 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2);
            float num2742 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2);
            num2743 = (float)((int)(num2743 / 8f) * 8);
            num2742 = (float)((int)(num2742 / 8f) * 8);
            vector266.X = (float)((int)(vector266.X / 8f) * 8);
            vector266.Y = (float)((int)(vector266.Y / 8f) * 8);
            num2743 -= vector266.X;
            num2742 -= vector266.Y;
            float num2737 = (float)Math.Sqrt((double)(num2743 * num2743 + num2742 * num2742));
            if (num2737 == 0f)
            {
                num2743 = npc.velocity.X;
                num2742 = npc.velocity.Y;
            }
            else
            {
                num2737 = num2745 / num2737;
                num2743 *= num2737;
                num2742 *= num2737;
            }
            if (Main.player[npc.target].dead)
            {
                num2743 = (float)npc.direction * num2745 / 2f;
                num2742 = (0f - num2745) / 2f;
            }
            npc.spriteDirection = -1;
            if (!Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
            {
                ref float reference = ref npc.ai[0];
                reference += 1f;
                if (npc.ai[0] > 0f)
                {
                    npc.velocity.Y = npc.velocity.Y + 0.023f;
                }
                else
                {
                    npc.velocity.Y = npc.velocity.Y - 0.023f;
                }
                if (npc.ai[0] < -100f || npc.ai[0] > 100f)
                {
                    npc.velocity.X = npc.velocity.X + 0.023f;
                }
                else
                {
                    npc.velocity.X = npc.velocity.X - 0.023f;
                }
                if (npc.ai[0] > 200f)
                {
                    npc.ai[0] = -200f;
                }
                npc.velocity.X = npc.velocity.X + num2743 * 0.007f;
                npc.velocity.Y = npc.velocity.Y + num2742 * 0.007f;
                npc.rotation = (float)Math.Atan2((double)npc.velocity.Y, (double)npc.velocity.X);
                if ((double)npc.velocity.X > 1.5)
                {
                    npc.velocity.X = npc.velocity.X * 0.9f;
                }
                if ((double)npc.velocity.X < -1.5)
                {
                    npc.velocity.X = npc.velocity.X * 0.9f;
                }
                if ((double)npc.velocity.Y > 1.5)
                {
                    npc.velocity.Y = npc.velocity.Y * 0.9f;
                }
                if ((double)npc.velocity.Y < -1.5)
                {
                    npc.velocity.Y = npc.velocity.Y * 0.9f;
                }
                if (npc.velocity.X > 3f)
                {
                    npc.velocity.X = 3f;
                }
                if (npc.velocity.X < -3f)
                {
                    npc.velocity.X = -3f;
                }
                if (npc.velocity.Y > 3f)
                {
                    npc.velocity.Y = 3f;
                }
                if (npc.velocity.Y < -3f)
                {
                    npc.velocity.Y = -3f;
                }
            }
            else
            {
                if (npc.velocity.X < num2743)
                {
                    npc.velocity.X = npc.velocity.X + num2744;
                    if (npc.velocity.X < 0f && num2743 > 0f)
                    {
                        npc.velocity.X = npc.velocity.X + num2744;
                    }
                }
                else if (npc.velocity.X > num2743)
                {
                    npc.velocity.X = npc.velocity.X - num2744;
                    if (npc.velocity.X > 0f && num2743 < 0f)
                    {
                        npc.velocity.X = npc.velocity.X - num2744;
                    }
                }
                if (npc.velocity.Y < num2742)
                {
                    npc.velocity.Y = npc.velocity.Y + num2744;
                    if (npc.velocity.Y < 0f && num2742 > 0f)
                    {
                        npc.velocity.Y = npc.velocity.Y + num2744;
                    }
                }
                else if (npc.velocity.Y > num2742)
                {
                    npc.velocity.Y = npc.velocity.Y - num2744;
                    if (npc.velocity.Y > 0f && num2742 < 0f)
                    {
                        npc.velocity.Y = npc.velocity.Y - num2744;
                    }
                }
                npc.rotation = (float)Math.Atan2((double)num2742, (double)num2743);
            }
            if (npc.type == 531)
            {
                npc.rotation += 1.57079637f;
            }
            float num2733 = 0.5f;
            if (npc.collideX)
            {
                npc.netUpdate = true;
                npc.velocity.X = npc.oldVelocity.X * (0f - num2733);
                if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < 2f)
                {
                    npc.velocity.X = 2f;
                }
                if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > -2f)
                {
                    npc.velocity.X = -2f;
                }
            }
            if (npc.collideY)
            {
                npc.netUpdate = true;
                npc.velocity.Y = npc.oldVelocity.Y * (0f - num2733);
                if (npc.velocity.Y > 0f && (double)npc.velocity.Y < 1.5)
                {
                    npc.velocity.Y = 2f;
                }
                if (npc.velocity.Y < 0f && (double)npc.velocity.Y > -1.5)
                {
                    npc.velocity.Y = -2f;
                }
            }
            if (npc.velocity.X > 0f && npc.oldVelocity.X < 0f)
            {
                update();
            }
            if (npc.velocity.X < 0f && npc.oldVelocity.X > 0f)
            {
                update();
            }
            if (npc.velocity.Y > 0f && npc.oldVelocity.Y < 0f)
            {
                update();
            }
            if (npc.velocity.Y < 0f && npc.oldVelocity.Y > 0f)
            {
                update();
            }
            transform();
        }

        public void update()
        {
            if (!npc.justHit)
            {
                npc.netUpdate = true;
            }
            transform();
        }

        public void transform()
        {
            if (Main.netMode != 1)
            {
                int num2732 = (int)npc.Center.X / 16;
                int num2731 = (int)npc.Center.Y / 16;
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
                        if (Main.tile[num2730, num2729].wall > 0)
                        {
                            flag204 = true;
                        }
                        num3549 = num2729;
                    }
                    num3549 = num2730;
                }
                if (!flag204)
                {
                    npc.Transform(NPCType<DuneCreeper>());
                }
            }
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemType<DuneStone>(), Main.rand.Next(7, 14));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss1)
            {
                return SpawnCondition.DesertCave.Chance * .1f;
            }
            else
            {
                return 0f;
            }
        }
    }
}
