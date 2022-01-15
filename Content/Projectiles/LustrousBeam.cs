using ExoriumMod.Dusts;
using Terraria;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Projectiles
{
    class LustrousBeam : ModProjectile
    {
        public override string Texture => "ExoriumMod/Projectiles/BlightShot";

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.extraUpdates = 50;
            projectile.timeLeft = 4500;
            projectile.penetrate = 1;
        }

        private bool retargeted = false;

        public override void AI()
        {
            Vector2 vectorToCursor = Main.MouseWorld - projectile.Center;
            Vector2 vectorToPlayer = (new Vector2(projectile.ai[0], projectile.ai[1])) - projectile.Center;
            float distanceToCursor = vectorToCursor.Length();
            float distanceToPlayer = vectorToPlayer.Length();
            if (distanceToPlayer > distanceToCursor && !retargeted)
            {
                retargeted = true;
                projectile.velocity = projectile.velocity.RotatedBy((float)(Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - (float)(Math.Atan2(vectorToCursor.X, vectorToCursor.Y))));
            }
            projectile.alpha = 225;
            for (int i = 0; i < 10; i++)
            {
                int dust0 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Rainbow>(), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 0, default(Color));
                Main.dust[dust0].position.X -= projectile.velocity.X / 10f * i;
                Main.dust[dust0].position.Y -= projectile.velocity.Y / 10f * i;
                switch (projectile.localAI[1])
                {
                    case 0:
                        Main.dust[dust0].color = new Color(255, 0, 0);
                        break;
                    case 1:
                        Main.dust[dust0].color = new Color(255, 110, 0);
                        break;
                    case 2:
                        Main.dust[dust0].color = new Color(255, 247, 0);
                        break;
                    case 3:
                        Main.dust[dust0].color = new Color(0, 255, 0);
                        break;
                    case 4:
                        Main.dust[dust0].color = new Color(0, 255, 204);
                        break;
                    case 5:
                        Main.dust[dust0].color = new Color(35, 0, 255);
                        break;
                    case 6:
                        Main.dust[dust0].color = new Color(149, 0, 255);
                        break;
                }
            }
        }
    }
}
