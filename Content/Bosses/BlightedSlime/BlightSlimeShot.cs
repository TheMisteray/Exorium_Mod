using ExoriumMod.Core;
using ExoriumMod.Content.Dusts;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace ExoriumMod.Content.Bosses.BlightedSlime
{
    class BlightSlimeShot : ModProjectile
    {
        public override string Texture => AssetDirectory.BlightedSlime + Name;

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = false;
            projectile.penetrate = 2;
            projectile.timeLeft = 300;
            projectile.hostile = true;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            projectile.velocity.Y += .13f;
            projectile.rotation += .2f;
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<BlightDust>(), 0, 0);
            }
        }
    }
}
