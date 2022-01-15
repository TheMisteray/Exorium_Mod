using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;

namespace ExoriumMod.Projectiles
{
    class DuneBall : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.timeLeft = 400;
            projectile.penetrate = 3;
        }

        private float stopper = 0;

        public override void AI()
        {
            Vector2 dustPosition = projectile.Center + new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));
            //Making player variable "player" set as the projectile's owner
            Player player = Main.player[projectile.owner];
            if (projectile.ai[0] == 0)
            {
                projectile.tileCollide = false;

                float focusX = player.Center.X;
                float focusY = player.Center.Y;
                double deg = (double)projectile.ai[1] * 2.4; //Speed
                double rad = deg * (Math.PI / 180); //Convert degrees to radians
                double dist = 64; //Distance away from the player
                projectile.position.X = focusX - (int)(Math.Cos(rad + 1.5) * dist) - projectile.width / 2;
                projectile.position.Y = focusY - (int)(Math.Sin(rad + 1.5) * dist) - projectile.height / 2;
                //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
                projectile.ai[1] += 1f;
            }
            else
            {
                if (projectile.ai[0] == 1 && stopper == 0)
                {
                    //Find cursor and shoot at
                    float maxDistance = 15f; // Speed
                    Vector2 vectorToCursor = Main.MouseWorld - projectile.Center;
                    float distanceToCursor = vectorToCursor.Length();
                    if (distanceToCursor > maxDistance)
                    {
                        distanceToCursor = maxDistance / distanceToCursor;
                        vectorToCursor *= distanceToCursor;
                    }
                    int velocityXBy1000 = (int)(vectorToCursor.X * 1000f);
                    int oldVelocityXBy1000 = (int)(projectile.velocity.X * 1000f);
                    int velocityYBy1000 = (int)(vectorToCursor.Y * 1000f);
                    int oldVelocityYBy1000 = (int)(projectile.velocity.Y * 1000f);
                    // Client Sync
                    if (velocityXBy1000 != oldVelocityXBy1000 || velocityYBy1000 != oldVelocityYBy1000)
                    {
                        projectile.netUpdate = true;
                    }
                    projectile.velocity = vectorToCursor;
                    projectile.penetrate = 1;
                    projectile.timeLeft = 400;
                    stopper++;
                }
                if (projectile.timeLeft == 390)
                    projectile.tileCollide = true;
                projectile.aiStyle = 0;
            }
            Dust.NewDustPerfect(dustPosition, 32, null, 100, default(Color), 0.8f);
        }
    }
}
