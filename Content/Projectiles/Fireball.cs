using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ExoriumMod.Content.Projectiles
{
    class Fireball : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.BallofFire;

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BallofFire);
            projectile.aiStyle = -1;
        }

        public override void AI()
        {
            projectile.rotation -= .2f;
            Dust.NewDust(projectile.Center, 0, 0, 6, 0f, 0f, 0, default(Color), 1f);
        }

        public override void Kill(int timeLeft)
        {
            int explosionArea = 320;
            Vector2 oldSize = projectile.Size;
            // Resize the projectile hitbox to be bigger.
            projectile.position = projectile.Center;
            projectile.Size += new Vector2(explosionArea);
            projectile.Center = projectile.position;

            projectile.tileCollide = false;
            projectile.velocity = Vector2.Zero;
            // Damage enemies inside the hitbox area
            projectile.Damage();
            projectile.Damage();
            projectile.scale = 0.01f;

            //Resize the hitbox to its original size
            projectile.position = projectile.Center;
            projectile.Size = new Vector2(10);
            projectile.Center = projectile.position;

            Main.PlaySound(SoundID.Item14, projectile.position);
            for (int i = 0; i < 200; i++)
            {
                Vector2 outward = new Vector2(0, Main.rand.NextFloat(20)).RotatedBy(Main.rand.NextFloat(MathHelper.Pi * 2));
                Dust dust = Dust.NewDustDirect(projectile.position - projectile.velocity, projectile.width, projectile.height, 55, outward.X, outward.Y, 0, Color.Orange, 2f);
                dust.position += outward;
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 1200);
            crit = true;
        }
    }
}
