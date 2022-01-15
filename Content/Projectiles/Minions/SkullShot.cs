using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Dusts;

namespace ExoriumMod.Projectiles.Minions
{
    class SkullShot : ModProjectile
    {
        public override string Texture => "ExoriumMod/Projectiles/DaggerCloud";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionShot[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.alpha = 255;
            projectile.timeLeft = 300;
            projectile.penetrate = 2;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
        }

        public override void AI()
        {
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<MorditeSpecks>(), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
        }
    }
}
