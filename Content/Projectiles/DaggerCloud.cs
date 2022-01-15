using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Projectiles
{
    class DaggerCloud : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = false;
            projectile.magic = true;
            projectile.alpha = 255;
            projectile.timeLeft = 360;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
        }

        private int areaSize = 180;
        private int variance = 110;

        public override void AI()
        {
            if (projectile.timeLeft == 360)
            {
                Player player = Main.player[projectile.owner]; 
                projectile.position = player.Center - (player.Center - Main.MouseWorld);
                projectile.netUpdate = true;
            }
            else
                projectile.velocity = Vector2.Zero;
            if (projectile.timeLeft % 4 == 0 && projectile.timeLeft != 0)
            {
                int Xpos = Main.rand.Next(-areaSize, areaSize + 1);
                int Ypos = Main.rand.Next(-areaSize, areaSize + 1);
                while (Math.Sqrt(Math.Pow(Xpos, 2) + Math.Pow(Ypos, 2)) > areaSize && Math.Sqrt(Math.Pow(Xpos, 2) + Math.Pow(Ypos, 2)) < variance)
                {
                    Xpos = Main.rand.Next(-areaSize, areaSize + 1);
                    Ypos = Main.rand.Next(-areaSize, areaSize + 1);
                }
                Vector2 diff = (new Vector2(projectile.Center.X + Xpos, projectile.Center.Y + Ypos) - projectile.Center);
                float distance = diff.Length();
                distance = -Main.rand.NextFloat(5,10) / distance;
                diff *= distance;
                diff = new Vector2(diff.X, diff.Y).RotatedByRandom(MathHelper.ToRadians(45));
                int proj1 = Projectile.NewProjectile(projectile.Center.X + Xpos, projectile.Center.Y + Ypos, diff.X, diff.Y, ProjectileType<DaggerCloudDagger>(), projectile.damage, 0, Main.myPlayer, areaSize + 35);
                Main.projectile[proj1].localAI[0] = projectile.Center.X;
                Main.projectile[proj1].localAI[1] = projectile.Center.Y;
            }
            for (int i = 0; i < 8; i++)
            {
                double rad = (Math.PI / 180) * Main.rand.NextFloat(361);
                int dust = Dust.NewDust(new Vector2 (projectile.Center.X + (float)(Math.Cos(rad + 1.5) * (areaSize + 45)), projectile.Center.Y + (float)(Math.Sin(rad + 1.5) * (areaSize + 45))), 1, 1, 20, 0, 0, 0);
                //Main.dust[dust].scale *= 0.98f;
            }
        }
    }
}
