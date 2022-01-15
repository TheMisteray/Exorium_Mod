using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Dusts;

namespace ExoriumMod.Projectiles
{
    class FireboltProj : ModProjectile
    {
        public override string Texture => "ExoriumMod/Projectiles/BlightShot";

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.alpha = 255;
            projectile.timeLeft = 600;
            projectile.penetrate = 2;
            projectile.magic = true;
        }

        public override void AI()
        {
            int num2475 = 0;
            for (int num2378 = 0; num2378 < 5; num2378 = num2475 + 1)
            {
                float num2375 = projectile.velocity.X / 3f * (float)num2378;
                float num2374 = projectile.velocity.Y / 3f * (float)num2378;
                int num2373 = 4;
                Vector2 position41 = new Vector2(projectile.position.X + (float)num2373, projectile.position.Y + (float)num2373);
                int width29 = projectile.width - num2373 * 2;
                int height29 = projectile.height - num2373 * 2;
                int num2372 = Dust.NewDust(position41, width29, height29, 6, 0f, 0f, 100, default(Color), 1.2f);
                Main.dust[num2372].noGravity = true;
                Dust dust81 = Main.dust[num2372];
                dust81.velocity *= 0.1f;
                dust81 = Main.dust[num2372];
                dust81.velocity += projectile.velocity * 0.1f;
                Dust expr_4827_cp_0 = Main.dust[num2372];
                expr_4827_cp_0.position.X = expr_4827_cp_0.position.X - num2375;
                Dust expr_4842_cp_0 = Main.dust[num2372];
                expr_4842_cp_0.position.Y = expr_4842_cp_0.position.Y - num2374;
                num2475 = num2378;
            }
            if (Main.rand.Next(5) == 0)
            {
                int num2377 = 4;
                Vector2 position42 = new Vector2(projectile.position.X + (float)num2377, projectile.position.Y + (float)num2377);
                int width30 = projectile.width - num2377 * 2;
                int height30 = projectile.height - num2377 * 2;
                int num2376 = Dust.NewDust(position42, width30, height30, 6 /*172*/, 0f, 0f, 100, default(Color), 0.6f);
                Dust dust81 = Main.dust[num2376];
                dust81.velocity *= 0.25f;
                dust81 = Main.dust[num2376];
                dust81.velocity += projectile.velocity * 0.5f;
            }
            else
            {
                projectile.rotation += 0.3f * (float)projectile.direction;
            }
        } 

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            projectile.penetrate--;
            if (projectile.penetrate <= 0)
            {
                projectile.Kill();
            }
            else
            {
                projectile.ai[0] += 0.1f;
                if (projectile.velocity.X != oldVelocity.X)
                {
                    projectile.velocity.X = -oldVelocity.X;
                }
                if (projectile.velocity.Y != oldVelocity.Y)
                {
                    projectile.velocity.Y = -oldVelocity.Y;
                }
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(10) == 0)
                target.AddBuff(BuffID.OnFire, 300);
        }
    }
}
