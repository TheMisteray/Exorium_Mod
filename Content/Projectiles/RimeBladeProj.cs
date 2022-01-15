using ExoriumMod.Dusts;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;

namespace ExoriumMod.Projectiles
{
    class RimeBladeProj : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.timeLeft = 300;
            projectile.height = 16;
            projectile.width = 16;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.alpha = 255;
            projectile.melee = true;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            projectile.rotation += .04f;
            for (int i = 0; i <= Math.Pow(projectile.ai[0], 2); i++)
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Rainbow>(), projectile.oldVelocity.X, projectile.oldVelocity.Y, 0, Color.LightBlue);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 200 * (int)projectile.ai[0], true);
        }

        public override void Kill(int timeLeft)
        {
            for(int i=0; i<= projectile.ai[0] * 10; i++)
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width * (int)projectile.ai[0], projectile.height * (int)projectile.ai[0], 67, 0, 0);
        }
    }
}
