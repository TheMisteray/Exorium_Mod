using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ID;
using System;

namespace ExoriumMod.Projectiles
{
    class DaggerCloudDagger : ModProjectile
    {
        public override string Texture => "ExoriumMod/Projectiles/DaggerCloud";

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.alpha = 20;
            projectile.timeLeft = 1600;
        }

        public override void AI()
        {
            if (projectile.velocity.X >= 0)
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(45);
            }
            else
            {
                projectile.rotation = projectile.velocity.ToRotation() - MathHelper.ToRadians(225);
            }
            projectile.spriteDirection = projectile.direction;
            if (Math.Sqrt(Math.Pow(projectile.localAI[0] - projectile.Center.X, 2) + Math.Pow(projectile.localAI[1] - projectile.Center.Y, 2)) > projectile.ai[0])
            {
                projectile.velocity.X /= 1.15f;
                projectile.velocity.Y /= 1.15f;
                projectile.alpha += 7;
                if (projectile.alpha >= 255)
                    projectile.Kill();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Dig, projectile.position);
            return true;
        }
    }
}
