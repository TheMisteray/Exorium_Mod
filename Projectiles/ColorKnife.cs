using ExoriumMod.Dusts;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;

namespace ExoriumMod.Projectiles
{
    class ColorKnife : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 30;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.timeLeft = 360;
        }

        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
            projectile.active = true;
            projectile.alpha = 0;
            if (projectile.localAI[0] > 0)
            {
                projectile.alpha = 255;
                projectile.localAI[0]--;
                switch (projectile.localAI[1])
                {
                    case 0:
                        Lighting.AddLight(projectile.position, 255 * 0.002f, 0 * 0.002f, 0 * 0.002f);
                        break;
                    case 1:
                        Lighting.AddLight(projectile.position, 255 * 0.002f, 110 * 0.002f, 0 * 0.002f);
                        break;
                    case 2:
                        Lighting.AddLight(projectile.position, 255 * 0.002f, 247 * 0.002f, 0 * 0.002f);
                        break;
                    case 3:
                        Lighting.AddLight(projectile.position, 0 * 0.002f, 255 * 0.002f, 0 * 0.002f);
                        break;
                    case 4:
                        Lighting.AddLight(projectile.position, 0 * 0.002f, 255 * 0.002f, 204 * 0.002f);
                        break;
                    case 5:
                        Lighting.AddLight(projectile.position, 35 * 0.002f, 0 * 0.002f, 255 * 0.002f);
                        break;
                    case 6:
                        Lighting.AddLight(projectile.position, 149 * 0.002f, 0 * 0.002f, 255 * 0.002f);
                        break;
                }
            }
            projectile.frame = (int)projectile.localAI[1];
        }

        public override bool ShouldUpdatePosition()
        {
            return projectile.localAI[0] <= 0;
        }

        public override bool CanDamage()
        {
            return projectile.localAI[0] <= 0;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item27, projectile.position);
        }
    }
}
