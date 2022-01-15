using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Dusts;

namespace ExoriumMod.Projectiles
{
    class BlightHailMelee : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40;
            projectile.alpha = 255;
            projectile.timeLeft = 30;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 0.7f;
        }

        public override void AI()
        {
            projectile.velocity.Y += projectile.ai[0];
            if (Main.rand.NextBool(1))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<BlightDust>(), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            }
            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<BlightDust>(), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            }
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<BlightDust>(), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            }
            if (Main.rand.NextBool(4))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<BlightDust>(), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            }
        }
    }
}
