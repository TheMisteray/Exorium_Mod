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
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.timeLeft = 1200;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.alpha = 255;
        }

        public float spikeCounter
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public float placeTimer
        {
            get => projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        public override void AI()
        {
            placeTimer++;
            if (placeTimer%60 == 0)
            {
                Vector2 vector2 = new Vector2(projectile.position.X + (float)(projectile.width / 2), projectile.position.Y + (float)projectile.height);
                float num1 = projectile.position.X + (float)projectile.width * 0.5f - vector2.X;
                float num2 = projectile.position.Y + projectile.height * 0.5f - vector2.Y;
                float num3 = (float)Math.Sqrt((double)(num1 * num1 + num2 * num2));
                num3 = 5 / num3;
                num1 *= num3;
                num2 *= num3;
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 150, num1, num2, ProjectileType<BlightedSpike>(), projectile.damage, 1, Main.myPlayer, 0, 0);
                spikeCounter++;
            }
            if (spikeCounter >= 5)
            {
                projectile.Kill();
            }
        }
    }
}
