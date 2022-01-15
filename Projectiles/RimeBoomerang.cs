using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ExoriumMod.Projectiles
{
    class RimeBoomerang : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frosted Boomerang");
        }

        public override void SetDefaults()
        {
            projectile.timeLeft = 30;
            projectile.height = 38;
            projectile.width = 38;
            projectile.friendly = true;
            projectile.hostile = false;
        }

        public override void AI()
        {
            projectile.rotation++;
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 67, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 300, true);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 300, true);
        }

        public override void Kill(int timeLeft)
        {
            for(int i=0; i<=9; i++)
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 67, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
        }
    }
}
