using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;

namespace ExoriumMod.Projectiles
{
    class AcidOrb : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.friendly = true;
            projectile.magic = true;
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
                int dust = Dust.NewDust(projectile.position - projectile.velocity, projectile.width, projectile.height, 33, projectile.oldVelocity.X * 0.5f, projectile.oldVelocity.Y * 0.5f);
                Main.dust[dust].color = new Color(255, 110, 0);
            }
            Main.PlaySound(SoundID.Item27, projectile.position);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Buffs.CausticAcid>(), 900);
        }
    }
}
