using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Content.Bosses.GemsparklingHive
{
    class AmberPulse : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.alpha = 255;
            projectile.timeLeft = 600;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
        }

        public bool hasSpawned
        {
            get => projectile.ai[0] == 1f;
            set => projectile.ai[0] = value ? 1f : 0f;
        }

        public override void AI()
        {
            if (!hasSpawned)
            {
                for (int i = 0; i < 6; i++)
                {
                    Projectile.NewProjectile(projectile.position, Vector2.Zero, ProjectileType<AmberRotator>(), projectile.damage, projectile.knockBack, Main.myPlayer, projectile.whoAmI, i);
                }
            }
        }

        public override bool CanHitPlayer(Player target)
        {
            return false;
        }
    }

    class AmberRotator : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gem Dart");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.alpha = 255;
            projectile.timeLeft = 600;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
        }

        private int ticker;

        public float pulseWhoAmI
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public float number
        {
            get => projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        public override void AI()
        {
            Projectile origin = Main.projectile[(int)pulseWhoAmI];
            ticker++;

            if (!origin.active || origin.type != ProjectileType<AmberPulse>())
            {
                projectile.Kill();
            }

            Vector2 offset = Vector2.UnitX;
            offset *= (int)((1 + Math.Sin(ticker/60) * 30));
            offset += Vector2.UnitX;
            Vector2 placePos = (origin.Center + offset).RotatedBy(MathHelper.ToRadians(ticker * 2 + (number * 60)));
            projectile.position = origin.Center + placePos;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = GetTexture(AssetDirectory.GemsparklingHive + "GemDart");

            Main.spriteBatch.Draw(tex, (projectile.Center - Main.screenPosition), null, new Color(255, 110, 0, 0), (projectile.Center - Main.projectile[(int)pulseWhoAmI].Center).ToRotation(), new Vector2(tex.Width / 2, tex.Height / 2), 1, SpriteEffects.None, 0f);
            base.PostDraw(spriteBatch, lightColor);
        }
    }
}
