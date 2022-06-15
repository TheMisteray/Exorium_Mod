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
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = false;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 300;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.velocity.Y += .13f;
            Projectile.rotation += .2f;
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<BlightDust>(), 0, 0);
            }
        }
    }
}
