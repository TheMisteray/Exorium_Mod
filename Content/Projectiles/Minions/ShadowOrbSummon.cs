using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace ExoriumMod.Content.Projectiles.Minions
{
    class ShadowOrbSummon : ModProjectile
    {
        public override string Texture => AssetDirectory.Shadowmancer + "ShadowOrb";

        public override void SetStaticDefaults()
        {
            //Main.projPet[Projectile.type] = true;
            //ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 600;
            Projectile.damage = 14;
            Projectile.tileCollide = true;
        }

        private const int MAX_TICKS = 25;
        private int ticks = 0;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (player.dead || !player.active)
            {
                player.ClearBuff(BuffType<Buffs.Minions.ShadowSummon>());
            }
            if (player.HasBuff(BuffType<Buffs.Minions.ShadowSummon>()))
            {
                Projectile.timeLeft = 600;
            }

            ticks++;
            if (ticks >= MAX_TICKS)
            {
                const float velXmult = 0.98f;
                const float velYmult = 0.35f;
                ticks = MAX_TICKS;
                Projectile.velocity.X *= velXmult;
                Projectile.velocity.Y += velYmult;
            }
            Projectile.rotation =
                Projectile.velocity.ToRotation() +
                MathHelper.ToRadians(90f);

        }

        public override bool OnTileCollide(Vector2 oldVelocity) //die manually since this is technically a minion
        {
            Projectile.Kill();
            return base.OnTileCollide(oldVelocity);
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 12; k++)
            {
                int dust = Dust.NewDust(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustType<Dusts.Shadow>(), Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);

            Player player = Main.player[Projectile.owner];
            player.AddBuff(BuffType<Buffs.Minions.ShadowSummon>(), 2);

            player.SpawnMinionOnCursor(player.GetSource_FromThis(), player.whoAmI, ProjectileType<ShadowSummon>(), (int)Projectile.ai[0], Projectile.knockBack, Projectile.Center - (player.Center - (player.Center - Main.MouseWorld)));
        }
    }
}
