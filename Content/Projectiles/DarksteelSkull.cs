using ExoriumMod.Core;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using System;

namespace ExoriumMod.Content.Projectiles
{
    class DarksteelSkull : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.CanDistortWater[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 800;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            if (Projectile.alpha != 0)
            {
                Projectile.alpha -= 15;
            }
            if (Projectile.localAI[0] == 0f)
            {
                AdjustMagnitude(ref Projectile.velocity);
                Projectile.localAI[0] = 1f;
            }
            Vector2 move = Vector2.Zero;
            float distance = 400f;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5)
                {
                    Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        distance = distanceTo;
                        target = true;
                    }
                }
            }
            if (target)
            {
                AdjustMagnitude(ref move);
                Projectile.velocity = (10 * Projectile.velocity + move) / 11f;
                AdjustMagnitude(ref Projectile.velocity);
            }
            if (Projectile.velocity != Vector2.Zero)
            {
                Projectile.spriteDirection = Projectile.direction;
                if (Projectile.velocity.X >=0)
                {
                    Projectile.rotation = Projectile.velocity.ToRotation();
                }
                else
                {
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
                }
            }
            if (Main.rand.NextBool(3))
            {
                int offset = Main.rand.Next(-4, 4);
                new Vector2(Projectile.position.X + offset, Projectile.position.Y + offset);
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Dusts.DarksteelDust>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 12f)
            {
                vector *= 12f / magnitude;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Dusts.DarksteelDust>(), Projectile.oldVelocity.X * 1.5f, Projectile.oldVelocity.Y * 1.5f);
            }
            SoundEngine.PlaySound(SoundID.NPCDeath6);
        }
    }
}
