using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Terraria.DataStructures;
using ExoriumMod.Content.Biomes;
using Terraria.WorldBuilding;

namespace ExoriumMod.Content.Bosses.GemsparklingHive
{
    class GemsparklingHive : ModNPC
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;

            new NPCDebuffImmunityData
            {
                ImmuneToAllBuffsThatAreNotWhips = true,
                ImmuneToWhips = true
            };
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 1000;
            NPC.damage = 16;
            NPC.defense = 7;
            NPC.knockBackResist = .3f;
            NPC.width = 64;
            NPC.height = 80;
            NPC.value = Item.buyPrice(0, 1, 0, 0);
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath43 ;
            NPC.timeLeft = NPC.activeTime * 30;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.boss = true;
        }

        int[] gemsparklings = new int[7];

        private static int HEALTH_UNTIL_BREAK = Main.expertMode ? 150 : 100;

        public float aiState
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }
        // 0 - closed
        // 1 - open
        // 2 - dash

        public float effectiveDamageTaken
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }

        public bool setGemsparklings
        {
            get => NPC.ai[2] == 1f;
            set => NPC.ai[2] = value ? 1f : 0f;
        }

        public bool checkSparklings
        {
            get => NPC.ai[3] == 1f;
            set => NPC.ai[3] = value ? 1f : 0f;
        }

        //lifeMax Really doesnt't matter for this boss
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.3 * balance);
            NPC.damage = (int)(NPC.damage * 0.6);
        }

        public float timer = 0;

        //Doesn't need netUpdate
        float rotatorSpeed = 0;

        public override void AI()
        {
            if (!setGemsparklings && Main.netMode != NetmodeID.MultiplayerClient)
            {
                gemsparklings[0] = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCType<AmethystGemsparkling>(), 0, 0, 5, 0, NPC.whoAmI, NPC.target);
                gemsparklings[1] = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCType<TopazGemsparkling>(), 0, 0, 5, 0, NPC.whoAmI, NPC.target);
                gemsparklings[2] = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCType<SapphireGemsparkling>(), 0, 0, 5, 0, NPC.whoAmI, NPC.target);
                gemsparklings[3] = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCType<EmeraldGemsparkling>(), 0, 0, 5, 0, NPC.whoAmI, NPC.target);
                gemsparklings[4] = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCType<RubyGemsparkling>(), 0, 0, 5, 0, NPC.whoAmI, NPC.target);
                gemsparklings[5] = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCType<DiamondGemsparkling>(), 0, 0, 5, 0, NPC.whoAmI, NPC.target);
                gemsparklings[6] = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCType<AmberGemsparkling>(), 0, 0, 5, 0, NPC.whoAmI, NPC.target);
                setGemsparklings = true;
            }

            #region Targeting
            if (Main.netMode != 1)
            {
                NPC.TargetClosest(true);
                NPC.netUpdate = true;
            }

            Player player = Main.player[NPC.target];
            if (!player.active || player.dead && Main.netMode != NetmodeID.MultiplayerClient || (NPC.position - player.position).Length() > 3000)
            {
                NPC.TargetClosest(true);
                NPC.netUpdate = true;
                player = Main.player[NPC.target];
                if (!player.active || player.dead || (NPC.position - player.position).Length() > 3000)
                {
                    NPC.velocity = new Vector2(0f, 10f);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
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

                int chosenSparklings = 0;
                int alreadyChosen = -1;
                int tries = 0;
                while (chosenSparklings < 2) //2 or all if less alive
                {
                    tries++;
                    int chosen = gemsparklings[Main.rand.Next(gemsparklings.Length)];
                    if (chosen == alreadyChosen)
                        continue;
                    if (CheckSparkling(Main.npc[chosen]))
                    {
                        chosenSparklings++;
                        alreadyChosen = chosen;
                        Main.npc[chosen].ai[1] = 0;
                        Main.npc[chosen].velocity = new Vector2(0, 4).RotatedByRandom(MathHelper.TwoPi);
                    }
                    if (chosenSparklings == 1 && sparkingsAlive == 1)
                        break;
                    if (sparkingsAlive == 0)
                        break;
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

            if (checkSparklings && Main.netMode != NetmodeID.MultiplayerClient)
                SparklingDied();
        }

        private void OpenAI(Player player)
        {
            NPC.aiAction = 1;
            NPC.velocity *= .9f;
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
                NPC.velocity = new Vector2(0, 10);
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
            NPC.aiAction = 0;
            if ((player.Center - NPC.Center).X >= 0 && rotatorSpeed < .06f)
            {
                rotatorSpeed += .005f;
            }
            else if ((player.Center - NPC.Center).X < 0 && rotatorSpeed > -.06f)
            {
                rotatorSpeed -= .005f;
            }
            else
                rotatorSpeed *= .96f;
            NPC.rotation += rotatorSpeed;

            float speed = 14f;
            float inertia = 100f;

            //Movement
            if (player.active)
            {
                float between = Vector2.Distance(player.Center, NPC.Center);
                if (between > 80f)
                {
                    Vector2 direction = player.Center - NPC.Center;
                    direction.Normalize();
                    direction *= speed;
                    NPC.velocity = (NPC.velocity * (inertia - 1) + direction) / inertia;
                }
            }
            else
                NPC.velocity *= 1.02f;

            if (timer >= 300)
            {
                aiState = 2;
                timer = 0;
            }
        }

        private void DashAI(Player player)
        {
            NPC.aiAction = 0;
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
                    Vector2 direction = player.Center - NPC.Center;
                    direction.Normalize();
                    direction *= speed;
                    NPC.velocity = (NPC.velocity * (inertia - 1) + direction) / inertia;
                }
                else
                    NPC.velocity *= 1.01f;
            }
            else
            {
                rotatorSpeed *= 1.01f;
                NPC.rotation += rotatorSpeed;
                NPC.velocity *= 0.92f;
            }
        }

        /// <summary>
        /// Checks if given npc is active and a gemsparkling
        /// </summary>
        /// <param name="sparkNpc">npcwhoami</param>
        /// <returns>if living gemsparking</returns>
        private bool CheckSparkling(NPC sparkNpc)
        {
            if (sparkNpc.active && sparkNpc.ModNPC is Gemsparkling)
                return true;
            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.aiAction == 1)
            {
                if (NPC.frame.Y == 0)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y = 1 * frameHeight;
                }
                if (NPC.frame.Y < 3 * frameHeight && NPC.frameCounter%10 == 0)
                {
                    NPC.frame.Y += frameHeight;
                }
            }
            else
            {
                if (NPC.frame.Y != 0 && NPC.frameCounter % 10 == 0)
                    NPC.frame.Y -= frameHeight;
            }
            NPC.frameCounter++;
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            if (aiState != 0)
                modifiers.Knockback.Flat = 0;
            NPC.life = NPC.lifeMax;
            base.ModifyIncomingHit(ref modifiers);
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            if (aiState != 1)
                effectiveDamageTaken += damageDone;
            base.OnHitByItem(player, item, hit, damageDone);
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (aiState != 1)
                effectiveDamageTaken += damageDone;
            base.OnHitByProjectile(projectile, hit, damageDone);
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (aiState == 1)
                return false;
            float dist = Vector2.Distance(target.Center, NPC.Center);
            if (dist < target.width + NPC.width)
                return true;
            return false;
        }

        public void SparklingDied()
        {
            checkSparklings = false;
            bool stillSpark = false;
            int numSparks = 0;
            foreach(int i in gemsparklings)
            {
                NPC npc = Main.npc[i];
                if (CheckSparkling(npc))
                {
                    numSparks++;
                    if (npc.ai[1] != 5)
                        stillSpark = true;
                }
            }
            if (!stillSpark)
            {
                aiState = 0;
                if (numSparks == 0)
                {
                    NPC.HitInfo hit = new NPC.HitInfo();
                    hit.Damage = 9999;
                    NPC.StrikeNPC(hit, true);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        NetMessage.SendStrikeNPC(NPC, hit);
                }
            }
        }

        public override void OnKill()
        {
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>(Name + "_gore1").Type, NPC.scale);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>(Name + "_gore2").Type, NPC.scale);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>(Name + "_gore3").Type, NPC.scale);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>(Name + "_gore4").Type, NPC.scale);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>(Name + "_gore5").Type, NPC.scale);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Amethyst, 1, 14, 20));
            npcLoot.Add(ItemDropRule.Common(ItemID.Topaz, 1, 14, 20));
            npcLoot.Add(ItemDropRule.Common(ItemID.Emerald, 1, 8, 15));
            npcLoot.Add(ItemDropRule.Common(ItemID.Sapphire, 1, 8, 15));
            npcLoot.Add(ItemDropRule.Common(ItemID.Ruby, 1, 5, 9));
            npcLoot.Add(ItemDropRule.Common(ItemID.Diamond, 1, 5, 9));
            npcLoot.Add(ItemDropRule.Common(ItemID.Amber, 1, 4, 7));
            npcLoot.Add(ItemDropRule.Common(ItemID.StoneBlock, 1, 15, 40));

            base.ModifyNPCLoot(npcLoot);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 drawCenter = NPC.Center;
            drawCenter.Y += 4;
            spriteBatch.Draw(Request<Texture2D>(AssetDirectory.GemsparklingHive + Name + "_Glow").Value, drawCenter - screenPos, new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height), Color.White, NPC.rotation, new Vector2(NPC.width, NPC.height) / 2f, 1f, SpriteEffects.None, 0);
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
            new FlavorTextBestiaryInfoElement("This hollowed out rock is home to a colony of living gemstones. To harvest the gemstones on its surface you will have to deal with its inhabitants."),
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
            });
        }
    }
}