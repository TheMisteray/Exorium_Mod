using Microsoft.Xna.Framework;
using System;
using ExoriumMod.Dusts;
using ExoriumMod.Buffs;
using ExoriumMod.Projectiles.Bosses.AssierJassad;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.NPCs.Bosses.Shadowmancer
{
    class MirrorEntity : ModNPC
    {
        public override string Texture => "ExoriumMod/NPCs/Bosses/Shadowmancer/AssierJassad";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadowmancer");
            Main.npcFrameCount[npc.type] = 7;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 60;
            npc.damage = 18;
            npc.defense = 0;
            npc.knockBackResist = 0f;
            npc.width = 42;
            npc.height = 48;
            npc.lavaImmune = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath52;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Frostburn] = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
        }

        private float wait = 90;

        public override bool PreAI()
        {
            if (npc.ai[2] == -1) //Killed by collective Darkness
                npc.life = 0;
            if (wait > 0)
            {
                if (Main.rand.NextBool(1))
                {
                    int offset = Main.rand.Next(-12, 13);
                    new Vector2(npc.position.X + offset, npc.position.Y + offset);
                    Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustType<Shadow>(), npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
                }
                wait--;
                return false;
            }
            return true;
        }

        private float actionCool
        {
            get => npc.ai[0];
            set => npc.ai[0] = value;
        }

        private float actionCycle
        {
            get => npc.ai[1];
            set => npc.ai[1] = value;
        }

        private float moveTime //unaffected by anything
        {
            get => npc.ai[2];
            set => npc.ai[2] = value;
        }

        private float attackProgress
        {
            get => npc.ai[3];
            set => npc.ai[3] = value;
        }

        private int attackLength;

        private float target;

        private Vector2 moveTo;


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
                npc.velocity.X = 0;
                npc.velocity.Y = 0;
            }

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
                            float magnitude = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
                            if (magnitude > 0)
                            {
                                delta *= 5f / magnitude;
                            }
                            else
                            {
                                delta = new Vector2(0f, 5f);
                            }
                            Projectile.NewProjectile(npc.Center.X + npc.direction * -25, npc.Center.Y, delta.X, delta.Y, ProjectileType<ShadowBolt>(), damage, 2, Main.myPlayer);
                            npc.netUpdate = true;
                        }
                        break;
                    case 1:
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
                                Dust.NewDust(npc.Center + npc.velocity, npc.width, npc.height, DustType<Shadow>(), delta.X, delta.Y);
                        }
                        break;
                }
                attackProgress++;
                if (attackProgress > attackLength)
                {
                    npc.aiAction = 0;
                    actionCool--;
                }
            }
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

            if (!(actionCycle == 1 && attackProgress > 300))
                npc.spriteDirection = -npc.direction;
            npc.frameCounter++;
        }

        public override void NPCLoot()
        {
            Vector2 dustSpeed = new Vector2(0, 5);
            for (int i = 0; i < 20; i++)
            {
                Vector2 perturbedDustSpeed = dustSpeed.RotatedBy(MathHelper.ToRadians(Main.rand.Next(0, 361)));
                Dust.NewDust(npc.position, npc.width, npc.height, DustType<Shadow>(), perturbedDustSpeed.X * Main.rand.NextFloat(), perturbedDustSpeed.Y * Main.rand.NextFloat());
            }
            base.NPCLoot();
        }
    }
}
