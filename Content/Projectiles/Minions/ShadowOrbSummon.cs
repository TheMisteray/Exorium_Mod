using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace ExoriumMod.Content.Projectiles.Minions
{
    class ShadowOrbSummon : ModProjectile
    {
        public override string Texture => AssetDirectory.Shadowmancer + "ShadowOrb";

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.penetrate = 3;
            projectile.timeLeft = 600;
        }

        private const int MAX_TICKS = 25;
        private int ticks = 0;

        public override void AI()
        {
            ticks++;
            if (ticks >= MAX_TICKS)
            {
                const float velXmult = 0.98f;
                const float velYmult = 0.35f;
                ticks = MAX_TICKS;
                projectile.velocity.X *= velXmult;
                projectile.velocity.Y += velYmult;
            }
            projectile.rotation =
                projectile.velocity.ToRotation() +
                MathHelper.ToRadians(90f);

        }

        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 12; k++)
            {
                int dust = Dust.NewDust(projectile.position - projectile.velocity, projectile.width, projectile.height, DustType<Dusts.Shadow>(), projectile.oldVelocity.X * 0.5f, projectile.oldVelocity.Y * 0.5f);
            }
            Main.PlaySound(SoundID.Item27, projectile.position);
            Main.player[projectile.owner].AddBuff(BuffType<Buffs.Minions.ShadowSummon>(), 1);
            Projectile.NewProjectile(projectile.position, Vector2.Zero, ProjectileType<ShadowSummon>(), projectile.damage, projectile.knockBack, projectile.owner);
        }
    }
}
