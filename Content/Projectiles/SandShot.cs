using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace ExoriumMod.Projectiles
{
    class SandShot : ModProjectile
    {
        int projectileBounce = 3;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bouncing Sand");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.SandBallGun);
            aiType = ProjectileID.SandBallGun;
            projectile.aiStyle = 1;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Player player = Main.player[projectile.owner];
            projectileBounce--;
            if (projectileBounce <= 0)
            {
                Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, 0, ProjectileID.SandBallFalling, 5, 5, player.whoAmI);
                projectile.Kill();
            }
            else
            {
                projectile.ai[0] += 0.1f;
                if (projectile.velocity.X != oldVelocity.X)
                {
                    projectile.velocity.X = -oldVelocity.X/1.5f;
                }
                if (projectile.velocity.Y != oldVelocity.Y)
                {
                    projectile.velocity.Y = -oldVelocity.Y/1.5f;
                }
                projectile.velocity *= 0.75f;
                Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, 0, ProjectileID.SandBallFalling, 5, 5, player.whoAmI);
                Main.PlaySound(SoundID.Item10, projectile.position);
            }
            return false;
        }
    }
}
