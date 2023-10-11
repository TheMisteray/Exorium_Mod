﻿using ExoriumMod.Core;
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

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 600;
            Projectile.damage = 14;
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
                Projectile.velocity.X *= velXmult;
                Projectile.velocity.Y += velYmult;
            }
            Projectile.rotation =
                Projectile.velocity.ToRotation() +
                MathHelper.ToRadians(90f);

        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 12; k++)
            {
                int dust = Dust.NewDust(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustType<Dusts.Shadow>(), Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);

            Player player = Main.player[Projectile.owner];
            player.AddBuff(BuffType<Buffs.Minions.ShadowSummon>(), 1);

            player.SpawnMinionOnCursor(player.GetSource_FromThis(), player.whoAmI, ProjectileType<ShadowSummon>(), (int)Projectile.ai[0], Projectile.knockBack, Projectile.Center - (player.Center - (player.Center - Main.MouseWorld)));
        }
    }
}
