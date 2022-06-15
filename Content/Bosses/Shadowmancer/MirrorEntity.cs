using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using System;
using ExoriumMod.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Bestiary;

namespace ExoriumMod.Content.Bosses.Shadowmancer
{
    class MirrorEntity : ModNPC
    {
        public override string Texture => AssetDirectory.Shadowmancer + "AssierJassad";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadowmancer");
            Main.npcFrameCount[NPC.type] = 7;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 65;
            NPC.damage = 19;
            NPC.defense = 0;
            NPC.knockBackResist = 0f;
            NPC.width = 42;
            NPC.height = 48;
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath52;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.Frostburn] = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
        }

        private float wait = 90;

        public override bool PreAI()
        {
            if (NPC.ai[2] == -1 && Main.netMode != NetmodeID.MultiplayerClient) //Killed by collective Darkness
            {
                NPC.StrikeNPC(333, 0, 0, true);
                if (Main.netMode != NetmodeID.SinglePlayer)
                    NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, NPC.whoAmI, 333, 0, 0, 1);
                NPC.active = false;
                NPC.life = -1;
            }
            if (wait > 0)
            {
                if (Main.rand.NextBool(1))
                {
                    int offset = Main.rand.Next(-12, 13);
                    new Vector2(NPC.position.X + offset, NPC.position.Y + offset);
                    Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustType<Shadow>(), NPC.velocity.X * 0.5f, NPC.velocity.Y * 0.5f);
                }
                wait--;
                return false;
            }
            return true;
        }

        private float actionCool
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        private float actionCycle
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }

        private float moveTime //unaffected by anything
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }

        private float attackProgress
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.7 * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 0.8);
        }

        private int attackLength;

        private float target;

        private Vector2 moveTo;

        public override void SendExtraAI(System.IO.BinaryWriter writer)
        {
            writer.Write((double)moveTo.X);
            writer.Write((double)moveTo.Y);
        }

        public override void ReceiveExtraAI(System.IO.BinaryReader reader)
        {
            moveTo = new Vector2((float)reader.ReadDouble(), (float)reader.ReadDouble());
        }


        public override void AI()
        {
            int damage = NPC.damage / (Main.expertMode == true ? 4 : 2);
            NPC.aiAction = 0;

            if (Main.netMode != 1)
            {
                NPC.TargetClosest(true);
                NPC.netUpdate = true;
            }

            Player player = Main.player[NPC.target];
            if (!player.active || player.dead && Main.netMode != NetmodeID.MultiplayerClient || (NPC.position - player.position).Length() > 2000)
            {
                NPC.TargetClosest(true);
                NPC.netUpdate = true;
                player = Main.player[NPC.target];
                if (!player.active || player.dead || (NPC.position - player.position).Length() > 2000)
                {
                    NPC.velocity = new Vector2(0f, 10f);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
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

            if (Main.netMode != 1 && moveTime > 0)
            {
                if (target == 0)
                {
                    moveTo = player.Center;
                    NPC.aiAction = 0;
                    NPC.TargetClosest(false);
                    moveTo.Y -= Main.rand.NextFloat(0, 201) + 160;
                    moveTo.X += Main.rand.NextFloat(-655, 656);
                    target++;
                }
                NPC.velocity = (moveTo - NPC.Center) / 90;
                NPC.velocity.Y *= 5.5f;
                moveTime--;
            }

            if ((Main.netMode != 1) && (actionCool <= 0f) && (moveTime <= 0f))
            {
                actionCycle = (int)Main.rand.Next(0, 2);
                switch (actionCycle)
                {
                    case 0: //Shadowbolt
                        moveTime = 180;
                        actionCool = 120;
                        attackLength = 90; 
                        break;
                    case 1: //Dash
                        actionCool = 30;
                        attackLength = 420;
                        break;
                }
                attackProgress = 0;
                target = 0;
                NPC.velocity.X = 0;
                NPC.velocity.Y = 0;
            }

            if (Main.netMode != 1 && actionCool > 0f && moveTime <= 0)
            {
                switch (actionCycle)
                {
                    case 0:
                        NPC.velocity = Vector2.Zero;
                        NPC.aiAction = 1;
                        if (attackProgress == 0)
                        {
                            Vector2 delta = player.Center - NPC.Center;
                            float magnitude = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
                            if (magnitude > 0)
                            {
                                delta *= 5f / magnitude;
                            }
                            else
                            {
                                delta = new Vector2(0f, 5f);
                            }
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + NPC.direction * -25, NPC.Center.Y, delta.X, delta.Y, ProjectileType<ShadowBolt>(), damage, 2, Main.myPlayer);
                            NPC.netUpdate = true;
                        }
                        break;
                    case 1:
                        if (attackProgress < 300)
                        {
                            NPC.TargetClosest(false);
                            moveTo = player.Center;
                            moveTo.X += 500 * NPC.direction;
                            NPC.velocity = (moveTo - NPC.Center) / 60;
                        }
                        else if (attackProgress == 300)
                        {
                            NPC.velocity.Y = 0;
                            NPC.velocity.X = -10 * NPC.direction;
                        }
                        else if (attackProgress < 440)
                        {
                            Vector2 delta = NPC.Center - new Vector2(NPC.Center.X + Main.rand.NextFloat(5) * NPC.direction, NPC.Center.Y + Main.rand.NextFloat(-4, 5));
                            if (Main.rand.NextBool(1))
                                Dust.NewDust(NPC.Center + NPC.velocity, NPC.width, NPC.height, DustType<Shadow>(), delta.X, delta.Y);
                        }
                        break;
                }
                attackProgress++;
                if (attackProgress > attackLength)
                {
                    NPC.aiAction = 0;
                    actionCool--;
                }
            }
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

            if (!(actionCycle == 1 && attackProgress > 300))
                NPC.spriteDirection = -NPC.direction;
            NPC.frameCounter++;
        }

        public override void OnKill()
        {
            Vector2 dustSpeed = new Vector2(0, 5);
            for (int i = 0; i < 20; i++)
            {
                Vector2 perturbedDustSpeed = dustSpeed.RotatedBy(MathHelper.ToRadians(Main.rand.Next(0, 361)));
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<Shadow>(), perturbedDustSpeed.X * Main.rand.NextFloat(), perturbedDustSpeed.Y * Main.rand.NextFloat());
            }
            base.OnKill();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.Add(
            new FlavorTextBestiaryInfoElement("An illusory clone conjured by the \"Mirror Image\" spell. It is substantially weaker than its creator, and is likey meant as a distraction."));
        }
    }
}
