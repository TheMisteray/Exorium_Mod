using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ExoriumMod.Projectiles
{
    class RimeBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Bullet");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;               
            projectile.height = 8;              
            projectile.aiStyle = 1;            
            projectile.friendly = true;         
            projectile.ranged = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 2600;         
            projectile.alpha = 0;            
            projectile.light = 0.2f;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;       
            aiType = ProjectileID.Bullet;
            projectile.velocity.X *= 1.5f;
            projectile.velocity.Y *= 1.5f;
        }

        public override void AI()
        {
            if (Main.rand.Next(10) == 1)
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 67, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            damage -= 1;
            if (Main.rand.Next(0, 3) == 1)
            {
                target.AddBuff((BuffID.Frostburn), 200, false);
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Dig, projectile.position);
        }
    }
}
