using ExoriumMod.Dusts;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Projectiles
{
    class SparkleShot : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.timeLeft = 600;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
        }

        public override void AI()
        {
            projectile.velocity.Y += projectile.ai[0];
            projectile.alpha = 225;
            projectile.extraUpdates = 2;
            for (int i = 0; i < 10; i++)
            {
                float newx = projectile.position.X - projectile.velocity.X / 10f * i;
                float newy = projectile.position.Y - projectile.velocity.Y / 10f * i;
                int dust0 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Rainbow>(), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 0, default(Color), 0.5f);
                Main.dust[dust0].position.X = newx;
                Main.dust[dust0].position.Y = newy;
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

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item27, projectile.position);
        }
    }
}
