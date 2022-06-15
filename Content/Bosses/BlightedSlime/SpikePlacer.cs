using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;

namespace ExoriumMod.Content.Bosses.BlightedSlime
{
    class SpikePlacer : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
        }

        public float spikeCounter
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public float placeTimer
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void AI()
        {
            placeTimer++;
            if (placeTimer%60 == 0)
            {
                Vector2 vector2 = new Vector2(Projectile.position.X + (float)(Projectile.width / 2), Projectile.position.Y + (float)Projectile.height);
                float num1 = Projectile.position.X + (float)Projectile.width * 0.5f - vector2.X;
                float num2 = Projectile.position.Y + Projectile.height * 0.5f - vector2.Y;
                float num3 = (float)Math.Sqrt((double)(num1 * num1 + num2 * num2));
                num3 = 5 / num3;
                num1 *= num3;
                num2 *= num3;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y - 150, num1, num2, ProjectileType<BlightedSpike>(), Projectile.damage, 1, Main.myPlayer, 0, 0);
                spikeCounter++;
            }
            if (spikeCounter >= 5)
            {
                Projectile.Kill();
            }
        }
    }
}
