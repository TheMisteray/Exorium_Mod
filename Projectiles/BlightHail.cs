using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Dusts;

namespace ExoriumMod.Projectiles
{
    class BlightHail : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 80;
            projectile.height = 80;
            projectile.alpha = 255;
            projectile.timeLeft = 30;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(1))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<BlightDust>(), projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f);
            }
            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<BlightDust>(), projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f);
            }
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<BlightDust>(), 0, 0);
            }
            if (Main.rand.NextBool(4))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<BlightDust>(), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            }
            if(projectile.ai[0] == 2)
            {
                projectile.timeLeft -= 2;
                projectile.position = projectile.Center;
                projectile.scale *= 1.08f;
                projectile.Center = projectile.position;
            }
        }
    }
}
