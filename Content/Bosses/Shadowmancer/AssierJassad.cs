﻿using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using System;
using ExoriumMod.Content.Dusts;
using ExoriumMod.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Bosses.Shadowmancer
{
    [AutoloadBossHead]
    class AssierJassad : ModNPC
    {
        public override string Texture => AssetDirectory.Shadowmancer + Name;
        public override string BossHeadTexture => AssetDirectory.Shadowmancer + Name + "_Head_Boss";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadowmancer");
            Main.npcFrameCount[npc.type] = 7;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 3100;
            npc.damage = 29;
            npc.defense = 11;
            npc.knockBackResist = 0f;
            npc.width = 42;
            npc.height = 48;
            npc.value = Item.buyPrice(0, 2, 50, 0);
            npc.npcSlots = 15f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath52;
            npc.timeLeft = NPC.activeTime * 30;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Frostburn] = true;
            npc.buffImmune[BuffType<ConsumingDark>()] = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/BathrobeMan");
            bossBag = ItemType<ShadowmancerBag>();
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.7 * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.8);
        }

        private float actionCool //Waiting after attack that ends animations and movement
        {
            get => npc.ai[0];
            set => npc.ai[0] = value;
        }

        private float actionCycle //Attack type
        {
            get => npc.ai[1];
            set => npc.ai[1] = value;
        }

        private float moveTime //Moves when >0
        {
            get => npc.ai[2];
            set => npc.ai[2] = value;
        }

        private float preFight //PreAI effects
        {
            get => npc.ai[3];
            set => npc.ai[3] = value;
        }

        private int phase = 1;
        private int pastAction1 = -1;
        private int pastAction2 = -1;
        private int attackLength;
        private int attackProgress;
        private bool showHP = true;
        private int target = 0;
        private Vector2 moveTo = Vector2.Zero;

        #region Networking
        //TODO: Check if all of this is even necessary, it might work just fine without sending past actions etc.
        public override void SendExtraAI(System.IO.BinaryWriter writer)
        {
            writer.Write(phase);
            writer.Write(pastAction1);
            writer.Write(pastAction2);
            writer.Write(attackLength);
            writer.Write(attackProgress);
            writer.Write(showHP);
            writer.Write(target);
            writer.Write(moveTo.X);
            writer.Write(moveTo.Y);
        }

        public override void ReceiveExtraAI(System.IO.BinaryReader reader)
        {
            phase = reader.ReadInt32();
            pastAction1 = reader.ReadInt32();
            pastAction2 = reader.ReadInt32();
            attackLength = reader.ReadInt32();
            attackProgress = reader.ReadInt32();
            showHP = reader.ReadBoolean();
            target = reader.ReadInt32();
            moveTo = new Vector2(reader.ReadInt32(), reader.ReadInt32());
        }
        #endregion Networking

        public override bool PreAI()
        {
            if (preFight > 0)
            {
                Vector2 angle = new Vector2(0, 5).RotatedByRandom(360);
                Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustType<Shadow>(), angle.X, angle.Y, 0, default(Color), 4);
                preFight--;
                if (preFight == 0)
                {
                    for (int i = 0; i<36; i++)
                    {
                        Vector2 circle = new Vector2(0, 5).RotatedByRandom(10 * i);
                        Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustType<Rainbow>(), circle.X, circle.Y, 0, new Color(200, 0, 0));
                    }
                }
                return false;
            }
            return true;
        }

        public override void AI()
        {
            int damage = npc.damage / (Main.expertMode == true ? 4 : 2);
            npc.aiAction = 0;

            if (Main.netMode != 1)
            {
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

            Player player = Main.player[npc.target];
            if (!player.active || player.dead)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead)
                {
                    npc.velocity = new Vector2(0f, 10f);
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                    return;
                }
            }

            if (player.position.X + (float)(player.width / 2) > npc.position.X + (float)(npc.width / 2))
            {
                npc.direction = -1;
            }
            else
            {
                npc.direction = 1;
            }

            if (Main.expertMode && npc.life <= npc.lifeMax/2)
                phase = 3;
            else if (npc.life <= (npc.lifeMax/3) * 2 || (Main.expertMode && npc.life <= (npc.lifeMax/4) * 3))
                phase = 2;
            
            if (Main.netMode != 1 && moveTime > 0)
            {
                if (target == 0)
                {
                    moveTo = player.Center;
                    npc.aiAction = 0;
                    npc.TargetClosest(false);
                    moveTo.Y -= Main.rand.NextFloat(0, 201) + 160;
                    moveTo.X += Main.rand.NextFloat(-655, 656);
                    target++;
                }
                npc.velocity = (moveTo - npc.Center) / 90;
                npc.velocity.Y *= 5.5f;
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
                            actionCool = 90;
                            attackLength = 90;
                        Main.NewText("Shadowbolt", 100, 100, 100);
                        break;
                    case 1: //Adds
                            moveTime = 120;
                            actionCool = 120;
                            attackLength = 100;
                        Main.NewText("Adds", 100, 100, 100);
                        break;
                    case 2: //Dash
                            actionCool = 30;
                            attackLength = 420;
                        Main.NewText("Dash", 100, 100, 100);
                        break;
                    case 3: //waves
                            moveTime = 150;
                            actionCool = 90;
                            attackLength = 240;
                        Main.NewText("waves", 100, 100, 100);
                        break;
                    case 4: //Swipe
                            actionCool = 90;
                            attackLength = 360;
                        Main.NewText("Swipe", 100, 100, 100);
                        break;
                    case 5: //Mirror Image
                            moveTime = 90;
                            actionCool = 90;
                            attackLength = 120;
                        Main.NewText("Mirror Image", 100, 100, 100);
                        break;
                    case 6: //Blade
                            actionCool = 50;
                            attackLength = 400;
                        Main.NewText("Blade", 100, 100, 100);
                        break;
                    case 7: //Magic Missiles
                            moveTime = 90;
                            actionCool = 10;
                            attackLength = 240;
                        Main.NewText("Magic Missiles", 100, 100, 100);
                        break;
                    case 8: //Hex
                            moveTime = 90;
                            actionCool = 10;
                            attackLength = 360;
                        Main.NewText("Hex", 100, 100, 100);
                        break;
                }
                attackProgress = 0;
                target = 0;
                npc.velocity.X = 0;
                npc.velocity.Y = 0;
            }
            #endregion

            #region Attack
            if (Main.netMode != 1 && actionCool > 0f && moveTime <= 0)
            {
                switch (actionCycle)
                {
                    case 0:
                        npc.velocity = Vector2.Zero;
                        npc.aiAction = 1;
                        if (attackProgress == 0)
                        {
                            Vector2 delta = player.Center - npc.Center;
                            delta.X += npc.direction * 25;
                            float magnitude = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
                            if (magnitude > 0)
                                delta *= 5f / magnitude;
                            else
                                delta = new Vector2(0f, 5f);
                            Projectile.NewProjectile(npc.Center.X + npc.direction * -25, npc.Center.Y, delta.X, delta.Y, ProjectileType<ShadowBolt>(), damage, 2, Main.myPlayer);
                            if (phase >= 2)
                            {
                                Vector2 perturbedSpeed = new Vector2(delta.X, delta.Y).RotatedBy(MathHelper.ToRadians(20));
                                Vector2 perturbedSpeed2 = new Vector2(delta.X, delta.Y).RotatedBy(MathHelper.ToRadians(-20));
                                Projectile.NewProjectile(npc.Center.X + npc.direction * -25, npc.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<ShadowBolt>(), damage, 2);
                                Projectile.NewProjectile(npc.Center.X + npc.direction * -25, npc.Center.Y, perturbedSpeed2.X, perturbedSpeed2.Y, ProjectileType<ShadowBolt>(), damage, 2);
                            }
                            npc.netUpdate = true;
                        }
                        break;
                    case 1:
                        npc.velocity = Vector2.Zero;
                        npc.aiAction = 2;
                        if (attackProgress == 0 || attackProgress == 30 || phase >=2 && attackProgress == 60 || (phase == 3 && attackProgress == 90))
                        {
                            Main.PlaySound(SoundID.Item1, npc.position);
                            Projectile.NewProjectile(npc.Center.X + npc.direction * -5, npc.Center.Y, (attackProgress/2) * -npc.direction, -8, ProjectileType<ShadowOrb>(), damage, 1, Main.myPlayer);
                        }
                        break;
                    case 2:
                        if (attackProgress < 300)
                        {
                            npc.TargetClosest(false);
                            moveTo = player.Center;
                            moveTo.X += 500 * npc.direction;
                            npc.velocity = (moveTo - npc.Center) / 60;
                        }
                        else if (attackProgress == 300)
                        {
                            npc.velocity.Y = 0;
                            npc.velocity.X = -10 * npc.direction;
                        }
                        else if (attackProgress < 440)
                        {
                            Vector2 delta = npc.Center - new Vector2(npc.Center.X + Main.rand.NextFloat(5) * npc.direction, npc.Center.Y + Main.rand.NextFloat(-4, 5));
                            if (Main.rand.NextBool(1))
                                Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustType<Shadow>(), delta.X, delta.Y);
                        }
                        break;
                    case 3:
                        npc.velocity = Vector2.Zero;
                        npc.aiAction = 1;
                        if (attackProgress == 0 || (attackProgress == 120 && phase >= 2) || (attackProgress == 240 && phase == 3))
                        {
                            if (attackProgress == 120 || attackProgress == 240)
                            {
                                Vector2 delta = player.Center - new Vector2(player.Center.X - 180, player.Center.Y);
                                float magnitude = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
                                if (magnitude > 0)
                                    delta *= 5f / magnitude;
                                else
                                    delta = new Vector2(0f, 5f);
                                Projectile.NewProjectile(player.Center.X - 180, player.Center.Y, delta.X, delta.Y, ProjectileType<ShadowBolt>(), damage, 2, Main.myPlayer);
                                delta = player.Center - new Vector2(player.Center.X + 180, player.Center.Y);
                                magnitude = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
                                if (magnitude > 0)
                                    delta *= 5f / magnitude;
                                else
                                    delta = new Vector2(0f, 5f);
                                Projectile.NewProjectile(player.Center.X + 180, player.Center.Y, delta.X, delta.Y, ProjectileType<ShadowBolt>(), damage, 2, Main.myPlayer);
                            }
                            if (attackProgress == 0 || attackProgress == 240)
                            {
                                Vector2 delta = player.Center - new Vector2(player.Center.X, player.Center.Y - 180);
                                float magnitude = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
                                if (magnitude > 0)
                                    delta *= 5f / magnitude;
                                else
                                    delta = new Vector2(0f, 5f);
                                Projectile.NewProjectile(player.Center.X, player.Center.Y - 180, delta.X, delta.Y, ProjectileType<ShadowBolt>(), damage, 2, Main.myPlayer);
                                delta = player.Center - new Vector2(player.Center.X, player.Center.Y + 180);
                                magnitude = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
                                if (magnitude > 0)
                                    delta *= 5f / magnitude;
                                else
                                    delta = new Vector2(0f, 5f);
                                Projectile.NewProjectile(player.Center.X, player.Center.Y + 180, delta.X, delta.Y, ProjectileType<ShadowBolt>(), damage, 2, Main.myPlayer);
                            }
                            npc.netUpdate = true;
                        }
                        break;
                    case 4:
                        if (attackProgress <= 240)
                        {
                            npc.TargetClosest(false);
                            player = Main.player[npc.target];
                            Vector2 moveTo = player.Center;
                            moveTo.X += 500 * npc.direction;
                            npc.velocity = (moveTo - npc.Center) / 60;
                        }
                        else if (attackProgress > 240 && attackProgress < 360)
                        {
                            npc.velocity.X = 0;
                            npc.velocity.Y = 0;
                            npc.aiAction = 1;
                        }
                        if (attackProgress == 280)
                        {
                            if (phase == 1)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    Projectile.NewProjectile(player.Center.X - 200 + (200 * i), player.Center.Y - 100, 0, 0, ProjectileType<ShadowFist>(), damage, 4, Main.myPlayer);
                                }
                            }
                            else if (phase == 2)
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    Projectile.NewProjectile(player.Center.X - 300 + (150 * i), player.Center.Y - 100, 0, 0, ProjectileType<ShadowFist>(), damage, 4, Main.myPlayer);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < 7; i++)
                                {
                                    Projectile.NewProjectile(player.Center.X - 375 + (125 * i), player.Center.Y - 100, 0, 0, ProjectileType<ShadowFist>(), damage, 4, Main.myPlayer);
                                }
                            }
                        }
                        break;
                    case 5:
                        if (attackProgress == 120)
                        {
                            int location = (int)Main.rand.Next((!(phase == 3) ? 2 : 3));
                            for (int i = 0; i <= (!(phase == 3) ? 2 : 4); i++)
                            {
                                int xmod = (i == 0) ? -160 : (i == 1) ? 0 : (i == 2) ? 160 : (i == 3) ? -300 : 300;
                                int ymod = (i == 0) ? -80 : (i == 1) ? -130 : (i == 2) ? -80 : (i == 3) ? -20 : -20;
                                if (!(location == i)) NPC.NewNPC((int)player.Center.X + xmod, (int)player.Center.Y + ymod, NPCType<MirrorEntity>(), 0, 0, 0, 0, 0, npc.target);
                                else
                                {
                                    npc.position.X = player.Center.X + xmod - npc.width/2;
                                    npc.position.Y = player.Center.Y + ymod - npc.height;
                                    npc.netUpdate = true;
                                    showHP = false;
                                }
                            }
                            npc.velocity = Vector2.Zero;
                            npc.alpha = 0;
                        }
                        else if (attackProgress > 120 && attackProgress < 300)
                        {
                            if (Main.rand.NextBool(1))
                            {
                                int offset = Main.rand.Next(-12, 13);
                                new Vector2(npc.position.X + offset, npc.position.Y + offset);
                                Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustType<Shadow>(), npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
                            }
                        }
                        else if (attackProgress < 120)
                            npc.alpha += 3;
                        break;
                    case 6:
                        if (attackProgress < 120)
                        {
                            Vector2 moveTo = player.Center;
                            npc.aiAction = 0;
                            npc.TargetClosest(false);
                            player = Main.player[npc.target];
                            moveTo.Y -= 160;
                            moveTo.X += 600 * npc.direction;
                            target++;
                            npc.velocity = (moveTo - npc.Center) / 60;
                        }
                        else
                        {
                            npc.aiAction = 2;
                            npc.velocity.Y = (float)Math.Sin((attackProgress)/60) * 1.5f;
                        }
                        if (attackProgress == 120)
                        {
                            npc.velocity.X = 4 * -npc.direction;
                        }
                        if (attackProgress % 60 == 0 && attackProgress > 120)
                        {
                            Main.PlaySound(SoundID.Item1, npc.position);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y -20, 0, 5, ProjectileType<ShadowBlade>(), damage, 2, Main.myPlayer);
                        }
                        break;
                    case 7:
                        npc.spriteDirection = -npc.direction;
                        npc.velocity = Vector2.Zero;
                        npc.aiAction = 1;
                        if (attackProgress == 0)
                        {
                            Projectile.NewProjectile(npc.Center.X + npc.direction * -50, npc.Center.Y, 0, 0, ProjectileType<CollectiveDarkness>(), damage, 2, Main.myPlayer, 1, npc.target);
                            npc.netUpdate = true;
                        }
                        break;
                    case 8:
                        npc.velocity = Vector2.Zero;
                        npc.aiAction = 1;
                        if (attackProgress%120==0 && attackProgress != 0)
                        {
                            Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, ProjectileType<Hex>(), damage * 2, 2, Main.myPlayer);
                        }
                        break;
                }
                attackProgress++;
                if  (attackProgress > attackLength)
                {
                    npc.aiAction = 0;
                    actionCool--;
                }
            }
            #endregion

            if (actionCycle == 3 || actionCycle == 8 || !(actionCool > 0f && moveTime <= 0)) //Action 3 or 8 or not attacking
                npc.spriteDirection = -npc.direction;
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.aiAction == 0)
            {
                npc.frame.Y = 0;
            }
            if (npc.aiAction == 1)
            {
                if (npc.frameCounter % 18 < 9)
                    npc.frame.Y = 1 * frameHeight;
                else
                    npc.frame.Y = 2 * frameHeight;
            }
            if (npc.aiAction == 2)
            {
                if (npc.frame.Y == 0)
                {
                    npc.frame.Y = 3 * frameHeight;
                    npc.frameCounter = 0;
                }
                else if (npc.frameCounter >= 40)
                    npc.frameCounter = 10;
                else if (npc.frameCounter >= 30)
                    npc.frame.Y = 6 * frameHeight;
                else if (npc.frameCounter >= 20)
                    npc.frame.Y = 5 * frameHeight;
                else if (npc.frameCounter >= 10)
                    npc.frame.Y = 4 * frameHeight;
            }
            npc.frameCounter ++;
        }
        public override bool PreNPCLoot()
        {
            //Update
            if (!ExoriumWorld.downedShadowmancer)
            {
                ExoriumWorld.downedShadowmancer = true;
                Main.NewText("Kil", 100, 100, 100);
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                }
            }

            //dust
            Vector2 dustSpeed = new Vector2(0, 7);
            for (int i = 0; i < 50; i++)
            {
                Vector2 perturbedDustSpeed = dustSpeed.RotatedBy(MathHelper.ToRadians(Main.rand.Next(0, 361)));
                Dust.NewDust(npc.position, npc.width, npc.height, DustType<Shadow>(), perturbedDustSpeed.X * Main.rand.NextFloat(), perturbedDustSpeed.Y * Main.rand.NextFloat(), 0, default, Main.rand.NextFloat(3));
            }

            return true;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "The Shadowmancer";
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
                Item.NewItem(npc.getRect(), ItemType<Items.Weapons.Ranger.AcidOrb>(), Main.rand.Next(21, 43));
                if (Main.rand.NextBool(1))
                    Item.NewItem(npc.getRect(), ItemType<Items.Consumables.Scrolls.ScrollOfMagicMissiles>(), Main.rand.Next(1, 3));
                else
                    Item.NewItem(npc.getRect(), ItemType<Items.Consumables.Scrolls.SpellScrollShield>(), Main.rand.Next(1, 3));
                switch (Main.rand.Next(3))
                {
                    case 0:
                        Item.NewItem(npc.getRect(), ItemType<Items.Weapons.Magic.ShadowBolt>());
                        break;
                    case 1:
                        Item.NewItem(npc.getRect(), ItemType<Items.Weapons.Melee.NineLivesStealer>());
                        break;
                    case 2:
                        Item.NewItem(npc.getRect(), ItemType<Items.Weapons.Summoner.ShadowOrb>(), Main.rand.Next(12, 20));
                        break;
                }
            }
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            showHP = true;
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            showHP = true;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return showHP;
        }
    }
}