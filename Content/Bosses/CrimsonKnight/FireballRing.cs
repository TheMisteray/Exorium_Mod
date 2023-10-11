using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using ExoriumMod.Content.Dusts;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    class FireballRing : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + "CaraveneFireball";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fireball");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
        }

        private const float WIDTH = 1000;
        private const float HEIGHT = 60;
        Vector2 spawnAxis = Vector2.Zero;

        public float RotationOffset
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public bool Enrage
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value ? 1f : 0f;
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 1200)
                spawnAxis = Projectile.position;

            spawnAxis.Y += 5;
            Vector2 offsetAxel = new Vector2(WIDTH * (float)Math.Sin(RotationOffset), HEIGHT * (float)Math.Sin(RotationOffset - MathHelper.PiOver2));
            Projectile.position = spawnAxis + offsetAxel;

            Projectile.rotation += .2f;
            RotationOffset += .006f;
            if (Main.expertMode)
                RotationOffset += .003f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, Enrage ? 600 : 300);
        }
    }
}
