using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Dusts;

namespace ExoriumMod.Projectiles
{
    class MorditeArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            aiType = ProjectileID.WoodenArrowFriendly;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage *= 2;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(3))
            {
                int offset = Main.rand.Next(-4, 4);
                new Vector2(projectile.position.X + offset, projectile.position.Y + offset);
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<MorditeSpecks>(), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                int offset = Main.rand.Next(-4, 4);
                Dust.NewDust(new Vector2(projectile.position.X + offset, projectile.position.Y + offset), projectile.width, projectile.height, DustType<MorditeSpecks>(), projectile.oldVelocity.X * 1.5f, projectile.oldVelocity.Y * 1.5f);
            }
        }
    }
}
