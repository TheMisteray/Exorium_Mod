using ExoriumMod.Core;
using ExoriumMod.Helpers;
using Microsoft.Xna.Framework;
using System;
using ExoriumMod.Content.Dusts;
using ExoriumMod.Content.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Terraria.DataStructures;

namespace ExoriumMod.Content.Bosses.Shadowmancer
{
    [AutoloadBossHead]
    class AssierJassad : ModNPC
    {
        public override string Texture => AssetDirectory.Shadowmancer + Name;
        public override string BossHeadTexture => AssetDirectory.Shadowmancer + Name + "_Head_Boss";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Shadowmancer");
            Main.npcFrameCount[NPC.type] = 7;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;

            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 2600;
            NPC.damage = 29;
            NPC.defense = 11;
            NPC.knockBackResist = 0f;
            NPC.width = 42;
            NPC.height = 48;
            NPC.value = Item.buyPrice(0, 2, 50, 0);
            NPC.npcSlots = 15f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath52;
            NPC.timeLeft = NPC.activeTime * 30;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.Frostburn] = true;
            NPC.buffImmune[BuffType<ConsumingDark>()] = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            if (!Main.dedServ)
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Sounds/Music/BathrobeMan");
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.7 * balance);
            NPC.damage = (int)(NPC.damage * 0.8);
        }

        private float actionCool //Waiting after attack that ends animations and movement
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        private float actionCycle //Attack type
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }

        private float moveTime //Moves when >0
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }

        private float preFight //PreAI effects
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }

        private int phase = 1;
        private int pastAction1 = -1;
        private int pastAction2 = -1;
        private int attackLength = 0;
        private int attackProgress = 0;
        private bool showHP = true;
        private int target = 0;
        private int location = 0; //teleport location index
        private Vector2 moveTo = Vector2.Zero;

        #region Networking
        //TODO: Check if all of this is even necessary, it might work just fine without sending past actions etc.
        public override void SendExtraAI(System.IO.BinaryWriter writer)
        {
            writer.Write(phase);
            writer.Write(attackLength);
            writer.Write(attackProgress);
            writer.Write(location);
            writer.WriteVector2(moveTo);
        }

        public override void ReceiveExtraAI(System.IO.BinaryReader reader)
        {
            phase = reader.ReadInt32();
            attackLength = reader.ReadInt32();
            attackProgress = reader.ReadInt32();
            location = reader.ReadInt32();
            moveTo = reader.ReadVector2();
        }
        #endregion Networking

        public override bool PreAI()
        {
            if (preFight > 0)
            {
                Vector2 angle = new Vector2(0, 5).RotatedByRandom(360);
                Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustType<Shadow>(), angle.X, angle.Y, 0, default(Color), 4);
                if (Main.netMode == NetmodeID.Server)
                    NPC.netUpdate = true;
                preFight--;
                if (preFight == 0)
                {
                    DustHelper.DustRing(NPC.Center, DustType<Rainbow>(), 20, 0, .1f, 1, 0, 0, 0, Color.Red, true);
                }
                return false;
            }
            return true;
        }

        public override void AI()
        {
            int damage = NPC.damage / (Main.expertMode == true ? 4 : 2);
            NPC.aiAction = 0;

            Player player = Main.player[NPC.target];
            if (!player.active || player.dead && Main.netMode != NetmodeID.MultiplayerClient || (NPC.position - player.position).Length() > 3000)
            {
                NPC.TargetClosest(true);
                NPC.netUpdate = true;
                player = Main.player[NPC.target];
                if (!player.active || player.dead || (NPC.position - player.position).Length() > 3000)
                {
                    NPC.velocity = new Vector2(0f, 10f);
                    NPC.EncourageDespawn(10);
                    return;
                }
            }

            if (player.position.X + (float)(player.width / 2) > NPC.position.X + (float)(NPC.width / 2))
            {
                NPC.direction = -1;
            }
            else
            {
                NPC.direction = 1;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient && Main.expertMode && NPC.life <= NPC.lifeMax/2)
            {
                phase = 3;
                NPC.netUpdate = true;
            }
            else if (Main.netMode != NetmodeID.MultiplayerClient && NPC.life <= (NPC.lifeMax/3) * 2 || (Main.expertMode && NPC.life <= (NPC.lifeMax/4) * 3))
            {
                phase = 2;
                NPC.netUpdate = true;
            }
            
            if (moveTime > 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (target == 0)
                    {
                        moveTo = player.Center;
                        NPC.TargetClosest(false);
                        moveTo.Y -= Main.rand.NextFloat(0, 201) + 160;
                        moveTo.X += Main.rand.NextFloat(-655, 656);
                        target++;
                        NPC.netUpdate = true;
                    }
                }
                NPC.aiAction = 0;
                NPC.velocity = (moveTo - NPC.Center) / 90;
                NPC.velocity.Y *= 5.5f;
                moveTime--;
            }

            #region Action selection
            if ((Main.netMode != 1) && (actionCool <= 0f) && (moveTime <= 0f))
            {
                //Don't repeat past 2 actions
                while((actionCycle == pastAction1) || (actionCycle == pastAction2))
                {
                    actionCycle = (int)Main.rand.Next(0, (5 + (phase-1) * 2));
                    //Don't dash in final phase and make mirrors more common
                    if (phase == 3 && actionCycle == 2)
                        actionCycle = 5;
                }
                //Store past actions
                pastAction2 = pastAction1;
                pastAction1 = (int)actionCycle;
                switch (actionCycle)
                {
                    case 0: //Shadowbolt
                            moveTime = 120;
                            actionCool = 30;
                            attackLength = 90;
                        break;
                    case 1: //Adds
                            moveTime = 120;
                            actionCool = 60;
                            attackLength = 100;
                        break;
                    case 2: //Dash
                            actionCool = 20;
                            attackLength = 270;
                        break;
                    case 3: //waves
                            moveTime = 150;
                            actionCool = 30;
                            attackLength = 240;
                        break;
                    case 4: //Swipe
                            actionCool = 30;
                            attackLength = 180;
                        break;
                    case 5: //Mirror Image
                            if (Main.netMode != NetmodeID.MultiplayerClient) //choose a location index and sync
                            {
                                location = (int)Main.rand.Next((!(phase == 3) ? 2 : 3));
                                NPC.netUpdate = true;
                            }
                            moveTime = 90;
                            actionCool = 90;
                            attackLength = 120;
                        break;
                    case 6: //Blade
                            actionCool = 10;
                            attackLength = 340;
                        break;
                    case 7: //Magic Missiles
                            moveTime = 90;
                            actionCool = 10;
                            attackLength = 240;
                        break;
                    case 8: //Hex
                            moveTime = 30;
                            actionCool = 10;
                            attackLength = 180;
                        break;
                }
                attackProgress = 0;
                target = 0;
                NPC.velocity.X = 0;
                NPC.velocity.Y = 0;
                NPC.netUpdate = true;
            }
            else if ((actionCool <= 0f) && (moveTime <= 0f))
            {
                NPC.velocity.X = 0;
                NPC.velocity.Y = 0;
            }
            #endregion

            #region Attack
            if (actionCool > 0f && moveTime <= 0)
            {
                switch (actionCycle)
                {
                    case 0:
                        NPC.velocity = Vector2.Zero;
                        NPC.aiAction = 1;
                        if (attackProgress == 0)
                        {
                            Vector2 delta = player.Center - NPC.Center;
                            delta.X += NPC.direction * 25;
                            float magnitude = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
                            if (magnitude > 0)
                                delta *= 5f / magnitude;
                            else
                                delta = new Vector2(0f, 5f);
                            if (Main.netMode != 1)
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + NPC.direction * -25, NPC.Center.Y, delta.X, delta.Y, ProjectileType<ShadowBolt>(), damage, 2, Main.myPlayer);
                            if (phase >= 2 && Main.netMode != 1)
                            {
                                Vector2 perturbedSpeed = new Vector2(delta.X, delta.Y).RotatedBy(MathHelper.ToRadians(20));
                                Vector2 perturbedSpeed2 = new Vector2(delta.X, delta.Y).RotatedBy(MathHelper.ToRadians(-20));
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + NPC.direction * -25, NPC.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<ShadowBolt>(), damage, 2);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + NPC.direction * -25, NPC.Center.Y, perturbedSpeed2.X, perturbedSpeed2.Y, ProjectileType<ShadowBolt>(), damage, 2);
                            }
                        }
                        break;
                    case 1:
                        NPC.velocity = Vector2.Zero;
                        NPC.aiAction = 2;
                        if ((attackProgress == 0 || attackProgress == 30 || phase >=2 && attackProgress == 60 || (phase == 3 && attackProgress == 90)))
                        {
                            SoundEngine.PlaySound(SoundID.Item1, NPC.position);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + NPC.direction * -5, NPC.Center.Y, (attackProgress/2) * -NPC.direction, -8, ProjectileType<ShadowOrb>(), damage, 1, Main.myPlayer);
                        }
                        break;
                    case 2:
                        if (attackProgress < 180)
                        {
                            NPC.TargetClosest(false);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                moveTo = player.Center;
                                moveTo.X += 500 * NPC.direction;
                                NPC.netUpdate = true;
                            }
                            NPC.velocity = (moveTo - NPC.Center) / 60;
                        }
                        else if (attackProgress == 180)
                        {
                            NPC.velocity.Y = 0;
                            NPC.velocity.X = -10 * NPC.direction;
                        }
                        else if (attackProgress < 270)
                        {
                            Vector2 delta = NPC.Center - new Vector2(NPC.Center.X + Main.rand.NextFloat(5) * NPC.direction, NPC.Center.Y + Main.rand.NextFloat(-4, 5));
                            if (Main.rand.NextBool(1))
                                Dust.NewDust(NPC.Center + NPC.velocity, NPC.width, NPC.height, DustType<Shadow>(), delta.X, delta.Y);
                        }
                        break;
                    case 3:
                        NPC.velocity = Vector2.Zero;
                        NPC.aiAction = 1;
                        if (attackProgress == 0 || (attackProgress == 120 && phase >= 2) || (attackProgress == 240 && phase == 3))
                        {
                            if (Main.netMode != 1 && (attackProgress == 120 || attackProgress == 240))
                            {
                                Vector2 delta = player.Center - new Vector2(player.Center.X - 180, player.Center.Y);
                                float magnitude = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
                                if (magnitude > 0)
                                    delta *= 5f / magnitude;
                                else
                                    delta = new Vector2(0f, 5f);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X - 180, player.Center.Y, delta.X, delta.Y, ProjectileType<ShadowBolt>(), damage, 2, Main.myPlayer);
                                delta = player.Center - new Vector2(player.Center.X + 180, player.Center.Y);
                                magnitude = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
                                if (magnitude > 0)
                                    delta *= 5f / magnitude;
                                else
                                    delta = new Vector2(0f, 5f);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X + 180, player.Center.Y, delta.X, delta.Y, ProjectileType<ShadowBolt>(), damage, 2, Main.myPlayer);
                            }
                            if (Main.netMode != 1 && (attackProgress == 0 || attackProgress == 240))
                            {
                                Vector2 delta = player.Center - new Vector2(player.Center.X, player.Center.Y - 180);
                                float magnitude = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
                                if (magnitude > 0)
                                    delta *= 5f / magnitude;
                                else
                                    delta = new Vector2(0f, 5f);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X, player.Center.Y - 180, delta.X, delta.Y, ProjectileType<ShadowBolt>(), damage, 2, Main.myPlayer);
                                delta = player.Center - new Vector2(player.Center.X, player.Center.Y + 180);
                                magnitude = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
                                if (magnitude > 0)
                                    delta *= 5f / magnitude;
                                else
                                    delta = new Vector2(0f, 5f);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X, player.Center.Y + 180, delta.X, delta.Y, ProjectileType<ShadowBolt>(), damage, 2, Main.myPlayer);
                            }
                        }
                        break;
                    case 4:
                        if (attackProgress <= 120)
                        {
                            NPC.TargetClosest(false);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                moveTo = player.Center;
                                moveTo.X += 500 * NPC.direction;
                                NPC.netUpdate = true;
                            }
                            NPC.velocity = (moveTo - NPC.Center) / 60;
                        }
                        else if (attackProgress > 120 && attackProgress < 150)
                        {
                            NPC.velocity.X = 0;
                            NPC.velocity.Y = 0;
                            NPC.aiAction = 1;
                        }
                        if (attackProgress == 150)
                        {
                            if (Main.netMode != 1 && phase == 1)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X - 200 + (200 * i), player.Center.Y - 100, 0, 0, ProjectileType<ShadowFist>(), damage, 4, Main.myPlayer);
                                }
                            }
                            else if (Main.netMode != 1 && phase == 2)
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X - 300 + (150 * i), player.Center.Y - 100, 0, 0, ProjectileType<ShadowFist>(), damage, 4, Main.myPlayer);
                                }
                            }
                            else if (Main.netMode != 1)
                            {
                                for (int i = 0; i < 7; i++)
                                {
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X - 375 + (125 * i), player.Center.Y - 100, 0, 0, ProjectileType<ShadowFist>(), damage, 4, Main.myPlayer);
                                }
                            }
                        }
                        break;
                    case 5:
                        if (attackProgress == 120)
                        {
                            for (int i = 0; i <= (!(phase == 3) ? 2 : 4); i++)
                            {
                                int xmod = (i == 0) ? -160 : (i == 1) ? 0 : (i == 2) ? 160 : (i == 3) ? -300 : 300;
                                int ymod = (i == 0) ? -80 : (i == 1) ? -130 : (i == 2) ? -80 : (i == 3) ? -20 : -20;
                                if (!(location == i) && Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)player.Center.X + xmod, (int)player.Center.Y + ymod, NPCType<MirrorEntity>(), 0, 0, 0, 0, 0, NPC.target);
                                }
                                else if (location == i)
                                {
                                    NPC.position.X = player.Center.X + xmod - NPC.width / 2;
                                    NPC.position.Y = player.Center.Y + ymod - NPC.height;
                                }
                            }
                            NPC.velocity = Vector2.Zero;
                            NPC.alpha = 0;
                            showHP = false;
                        }
                        else if (attackProgress > 120 && attackProgress < 300)
                        {
                            if (Main.rand.NextBool(1))
                            {
                                Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustType<Shadow>(), NPC.velocity.X * 0.5f, NPC.velocity.Y * 0.5f);
                            }
                        }
                        else if (attackProgress < 120)
                            NPC.alpha += 3;
                        break;
                    case 6:
                        if (attackProgress < 60)
                        {
                            NPC.aiAction = 0;
                            NPC.TargetClosest(false);
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                moveTo = player.Center; 
                                moveTo.Y -= 160;
                                moveTo.X += 600 * NPC.direction;
                                NPC.netUpdate = true;
                            }
                            target++;
                            NPC.velocity = (moveTo - NPC.Center) / 60;
                        }
                        else
                        {
                            NPC.aiAction = 2;
                            NPC.velocity.Y = (float)Math.Sin((attackProgress)/60) * 1.5f;
                        }
                        if (attackProgress == 60)
                        {
                            NPC.velocity.X = 4 * -NPC.direction;
                        }
                        if (Main.netMode != 1 && (attackProgress % 60 == 0 && attackProgress > 60))
                        {
                            SoundEngine.PlaySound(SoundID.Item1, NPC.position);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y -20, 0, 5, ProjectileType<ShadowBlade>(), damage, 2, Main.myPlayer);
                        }
                        break;
                    case 7:
                        NPC.spriteDirection = -NPC.direction;
                        NPC.velocity = Vector2.Zero;
                        NPC.aiAction = 1;
                        if (Main.netMode != 1 && attackProgress == 0)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + NPC.direction * -50, NPC.Center.Y, 0, 0, ProjectileType<CollectiveDarkness>(), damage, 2, Main.myPlayer, 1, NPC.target);
                        }
                        break;
                    case 8:
                        NPC.velocity = Vector2.Zero;
                        NPC.aiAction = 1;
                        if (Main.netMode != 1 && (attackProgress % 60==0 && attackProgress != 0))
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X, player.Center.Y, 0, 0, ProjectileType<Hex>(), damage * 2, 2, Main.myPlayer);
                        }
                        break;
                }
                attackProgress++;
                if  (attackProgress > attackLength)
                {
                    NPC.aiAction = 0;
                    actionCool--;
                }
            }
            #endregion

            if (Main.netMode != NetmodeID.MultiplayerClient && actionCycle == 3 || actionCycle == 8 || !(actionCool > 0f && moveTime <= 0)) //Action 3 or 8 or not attacking
                NPC.spriteDirection = -NPC.direction;
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.aiAction == 0)
            {
                NPC.frame.Y = 0;
            }
            if (NPC.aiAction == 1)
            {
                if (NPC.frameCounter % 18 < 9)
                    NPC.frame.Y = 1 * frameHeight;
                else
                    NPC.frame.Y = 2 * frameHeight;
            }
            if (NPC.aiAction == 2)
            {
                if (NPC.frame.Y == 0)
                {
                    NPC.frame.Y = 3 * frameHeight;
                    NPC.frameCounter = 0;
                }
                else if (NPC.frameCounter >= 40)
                    NPC.frameCounter = 10;
                else if (NPC.frameCounter >= 30)
                    NPC.frame.Y = 6 * frameHeight;
                else if (NPC.frameCounter >= 20)
                    NPC.frame.Y = 5 * frameHeight;
                else if (NPC.frameCounter >= 10)
                    NPC.frame.Y = 4 * frameHeight;
            }
            NPC.frameCounter ++;
        }
        public override bool PreKill()
        {
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].active && (Main.npc[i].type == NPCType<ShadowAdd>() || Main.npc[i].type == NPCType<MirrorEntity>()))
                {
                    Main.npc[i].ai[2] = -1;
                }
            }

            //Update
            if (!Core.Systems.DownedBossSystem.downedShadowmancer)
            {
                Core.Systems.DownedBossSystem.downedShadowmancer = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                }
            }

            return true;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "The Shadowmancer";
            potionType = ItemID.LesserHealingPotion;
        }

        public override void OnKill()
        {
            //dust
            Vector2 dustSpeed = new Vector2(0, 7);
            for (int i = 0; i < 50; i++)
            {
                Vector2 perturbedDustSpeed = dustSpeed.RotatedBy(MathHelper.ToRadians(Main.rand.Next(0, 361)));
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<Shadow>(), perturbedDustSpeed.X * Main.rand.NextFloat(), perturbedDustSpeed.Y * Main.rand.NextFloat(), 0, default, Main.rand.NextFloat(3));
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ItemType<ShadowmancerBag>()));

            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

            notExpertRule.OnSuccess(ItemDropRule.Common(ItemType<Items.Weapons.Ranger.AcidOrb>(), 1, 31, 52));

            IItemDropRule scroll = notExpertRule.OnSuccess(ItemDropRule.NotScalingWithLuck(ItemType<Items.Consumables.Scrolls.ScrollOfMagicMissiles>(), 2, 1, 3));
            scroll.OnFailedRoll(ItemDropRule.NotScalingWithLuck(ItemType<Items.Consumables.Scrolls.SpellScrollShield>(), 1, 1, 3));

            IItemDropRule item1 = notExpertRule.OnSuccess(ItemDropRule.NotScalingWithLuck(ItemType<Items.Weapons.Magic.ShadowBolt>(), 3));
            IItemDropRule item2 = item1.OnFailedRoll(ItemDropRule.NotScalingWithLuck(ItemType<Items.Weapons.Melee.NineLivesStealer>(), 2));
            item2.OnFailedRoll(ItemDropRule.NotScalingWithLuck(ItemType<Items.Weapons.Summoner.ShadowOrb>(), 1, 12, 20));

            npcLoot.Add(notExpertRule);
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            showHP = true;
            base.ModifyIncomingHit(ref modifiers);
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return showHP;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("This one appears to have been experimenting with a strange ritual altar. To... Limited success. It is still a mystery what exactly they hoped to accomplish. Not that it matters now with them gone."),
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
            });
        }
    }
}
