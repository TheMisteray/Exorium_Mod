using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent;

namespace ExoriumMod.Content.Projectiles
{
    class Fireball : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.BallofFire;

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BallofFire);
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Projectile.rotation -= .2f;
            Dust.NewDust(Projectile.Center, 0, 0, 6, 0f, 0f, 0, default(Color), 1f);
        }

        public override void Kill(int timeLeft)
        {
            int explosionArea = 320;
            Vector2 oldSize = Projectile.Size;
            // Resize the projectile hitbox to be bigger.
            Projectile.position = Projectile.Center;
            Projectile.Size += new Vector2(explosionArea);
            Projectile.Center = Projectile.position;

            Projectile.tileCollide = false;
            Projectile.velocity = Vector2.Zero;
            // Damage enemies inside the hitbox area
            Projectile.Damage();
            Projectile.Damage();
            Projectile.scale = 0.01f;

            //Resize the hitbox to its original size
            Projectile.position = Projectile.Center;
            Projectile.Size = new Vector2(10);
            Projectile.Center = Projectile.position;

            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            for (int i = 0; i < 200; i++)
            {
                Vector2 outward = new Vector2(0, Main.rand.NextFloat(20)).RotatedBy(Main.rand.NextFloat(MathHelper.Pi * 2));
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, 55, outward.X, outward.Y, 0, Color.Orange, 2f);
                dust.position += outward;
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 1200);
            hit.Crit = true;
        }
    }
}
