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

namespace ExoriumMod.Content.Bosses.GemsparklingHive
{
    class GemsparklingHive : ModNPC
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 1000;
            npc.damage = 16;
            npc.defense = 9999;
            npc.knockBackResist = .3f;
            npc.width = 64;
            npc.height = 80;
            npc.value = Item.buyPrice(0, 1, 0, 0);
            npc.HitSound = SoundID.NPCHit41;
            npc.DeathSound = SoundID.NPCDeath43 ;
            npc.timeLeft = NPC.activeTime * 30;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
            npc.noGravity = true;
            npc.noTileCollide = true;
        }

        int[] gemsparklings = new int[7];

        private static int HEALTH_UNTIL_BREAK = Main.expertMode ? 150 : 100;

        float effectiveDamageTaken = 0;

        float aiState = 0;
        // 0 - closed
        // 1 - open
        // 2 - dash

        float timer = 0;

        //Doesn't need netUpdate
        float rotatorSpeed = 0;

        public bool setGemsparklings
        {
            get => npc.ai[2] == 1f;
            set => npc.ai[2] = value ? 1f : 0f;
        }

        public override void AI()
        {
            if (!setGemsparklings && Main.netMode != NetmodeID.MultiplayerClient)
            {
                gemsparklings[0] = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCType<AmethystGemsparkling>(), 0, 0, 5, 0, npc.whoAmI, npc.target);
                gemsparklings[1] = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCType<TopazGemsparkling>(), 0, 0, 5, 0, npc.whoAmI, npc.target);
                gemsparklings[2] = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCType<SapphireGemsparkling>(), 0, 0, 5, 0, npc.whoAmI, npc.target);
                gemsparklings[3] = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCType<EmeraldGemsparkling>(), 0, 0, 5, 0, npc.whoAmI, npc.target);
                gemsparklings[4] = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCType<RubyGemsparkling>(), 0, 0, 5, 0, npc.whoAmI, npc.target);
                gemsparklings[5] = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCType<DiamondGemsparkling>(), 0, 0, 5, 0, npc.whoAmI, npc.target);
                gemsparklings[6] = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCType<AmberGemsparkling>(), 0, 0, 5, 0, npc.whoAmI, npc.target);
                setGemsparklings = true;
            }

            #region Targeting
            if (Main.netMode != 1)
            {
                npc.TargetClosest(true);
                npc.netUpdate = true;
            }

            Player player = Main.player[npc.target];
            if (!player.active || player.dead && Main.netMode != NetmodeID.MultiplayerClient || (npc.position - player.position).Length() > 3000)
            {
                npc.TargetClosest(true);
                npc.netUpdate = true;
                player = Main.player[npc.target];
                if (!player.active || player.dead || (npc.position - player.position).Length() > 3000)
                {
                    npc.velocity = new Vector2(0f, 10f);
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                    return;
                }
            }
            #endregion

            if (Main.netMode != NetmodeID.MultiplayerClient && effectiveDamageTaken >= HEALTH_UNTIL_BREAK)
            {
                effectiveDamageTaken = 0;
                aiState = 1;
                timer = 0;

                int sparkingsAlive = 0;
                foreach(int i in gemsparklings)
                {
                    NPC sparkNpc = Main.npc[i];
                    if (CheckSparkling(sparkNpc))
                        sparkingsAlive++;
                }
                if (sparkingsAlive == 0)
                {
                    npc.NPCLoot();
                    npc.timeLeft = 0;
                    npc.netUpdate = true;
                }

                int chosenSparklings = 0;
                while (chosenSparklings < Math.Min(sparkingsAlive, Main.expertMode? 3 : 2)) //2 in normal 3 in expert or all if less alive
                {
                    int chosen = gemsparklings[Main.rand.Next(gemsparklings.Length)];
                    if (CheckSparkling(Main.npc[chosen]))
                    {
                        chosenSparklings++;
                        Main.npc[chosen].ai[1] = 0;
                        Main.npc[chosen].velocity = new Vector2(0, 5).RotatedByRandom(MathHelper.TwoPi);
                    }
                }
            }

            timer++;
            switch (aiState)
            {
                case 0:
                    ClosedAI(player);
                    break;
                case 1:
                    OpenAI(player);
                    break;
                case 2:
                    DashAI(player);
                    break;
            }
        }

        private void OpenAI(Player player)
        {
            npc.aiAction = 1;
            npc.velocity *= .9f;
            if (timer > 900)
            {
                foreach (int i in gemsparklings)
                {
                    NPC sparkNpc = Main.npc[i];
                    if (CheckSparkling(sparkNpc))
                        sparkNpc.ai[1] = 5;
                }
                timer = 0;
                aiState = 0;
                timer = 0;
            }
            if (player.active == false)
            {
                foreach (int i in gemsparklings)
                {
                    NPC sparkNpc = Main.npc[i];
                    if (CheckSparkling(sparkNpc))
                        sparkNpc.ai[1] = 5;
                }
                aiState = 0;
                npc.velocity = new Vector2(0, 10);
                timer = 0;
            }
        }

        private void ClosedAI(Player player)
        {
            foreach (int i in gemsparklings)
            {
                NPC sparkNpc = Main.npc[i];
                if (CheckSparkling(sparkNpc))
                    sparkNpc.ai[1] = 5;
            }
            npc.aiAction = 0;
            if ((player.Center - npc.Center).X >= 0 && rotatorSpeed < .06f)
            {
                rotatorSpeed += .005f;
            }
            else if ((player.Center - npc.Center).X < 0 && rotatorSpeed > -.06f)
            {
                rotatorSpeed -= .005f;
            }
            else
                rotatorSpeed *= .96f;
            npc.rotation += rotatorSpeed;

            float speed = 14f;
            float inertia = 100f;

            //Movement
            if (player.active)
            {
                float between = Vector2.Distance(player.Center, npc.Center);
                if (between > 80f)
                {
                    Vector2 direction = player.Center - npc.Center;
                    direction.Normalize();
                    direction *= speed;
                    npc.velocity = (npc.velocity * (inertia - 1) + direction) / inertia;
                }
            }
            else
                npc.velocity *= 1.02f;

            if (timer >= 300)
            {
                aiState = 2;
                timer = 0;
            }
        }

        private void DashAI(Player player)
        {
            npc.aiAction = 0;
            if (timer >= 150)
            {
                aiState = 0;
                timer = 0;
                rotatorSpeed = 0;
            }
            if (timer >= 120)
            {
                float speed = 28f;
                float inertia = 50f;

                //Movement
                if (player.active)
                {
                    Vector2 direction = player.Center - npc.Center;
                    direction.Normalize();
                    direction *= speed;
                    npc.velocity = (npc.velocity * (inertia - 1) + direction) / inertia;
                }
                else
                    npc.velocity *= 1.01f;
            }
            else
            {
                rotatorSpeed *= 1.01f;
                npc.rotation += rotatorSpeed;
                npc.velocity *= 0.92f;
            }
        }

        /// <summary>
        /// Checks if given npc is active and a gemsparkling
        /// </summary>
        /// <param name="sparkNpc">npcwhoami</param>
        /// <returns>if living gemsparking</returns>
        private bool CheckSparkling(NPC sparkNpc)
        {
            if (sparkNpc.active &&
                        (sparkNpc.type == NPCType<AmethystGemsparkling>() ||
                        sparkNpc.type == NPCType<TopazGemsparkling>() ||
                        sparkNpc.type == NPCType<RubyGemsparkling>() ||
                        sparkNpc.type == NPCType<EmeraldGemsparkling>() ||
                        sparkNpc.type == NPCType<SapphireGemsparkling>() ||
                        sparkNpc.type == NPCType<DiamondGemsparkling>() ||
                        sparkNpc.type == NPCType<AmberGemsparkling>()))
                return true;
            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.aiAction == 1)
            {
                if (npc.frame.Y == 0)
                {
                    npc.frameCounter = 0;
                    npc.frame.Y = 1 * frameHeight;
                }
                if (npc.frame.Y < 3 * frameHeight && npc.frameCounter%20 == 0)
                {
                    npc.frame.Y += frameHeight;
                }
            }
            else
            {
                if (npc.frame.Y != 0 && npc.frameCounter % 20 == 0)
                    npc.frame.Y -= frameHeight;
            }
            npc.frameCounter++;
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if (aiState != 1)
                effectiveDamageTaken += (float)damage;
            if (aiState != 0)
                knockback = 0;
            damage = 0;
            return base.StrikeNPC(ref damage, defense, ref knockback, hitDirection, ref crit);
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (aiState == 1)
                return false;
            float dist = Vector2.Distance(target.Center, npc.Center);
            if (dist < target.width + npc.width)
                return true;
            return false;
        }
    }
}