using ExoriumMod.Dusts;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace ExoriumMod.Projectiles
{
    class BlightShot : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.timeLeft = 300;
        }

        public override void AI()
        {
            projectile.rotation += .5f;
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<BlightDust>(), 0, 0);
            }
        } 

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.Kill();
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<BlightDust>(), projectile.oldVelocity.X, projectile.oldVelocity.Y);
            }
            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, projectile.oldVelocity.X, projectile.oldVelocity.Y, mod.ProjectileType("BlightHail"), projectile.damage, projectile.knockBack, projectile.owner, 2);
            Main.PlaySound(SoundID.Item10, projectile.position);
        }
    }
}
