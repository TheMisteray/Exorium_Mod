using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExoriumMod.Content.Bosses.GemsparklingHive
{
    class EmeraldSpike : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.alpha = 255;
            projectile.timeLeft = 600;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
        }

        private const int CHAIN_LENGTH = 60;

        private float drawAlpha = 0;

        bool projCreated = false;

        bool reCentered = false;

        public float chainPos
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public override void AI()
        {
            drawAlpha += MathHelper.PiOver2 / 20;
            if (Main.netMode != NetmodeID.MultiplayerClient && drawAlpha > MathHelper.PiOver4 && chainPos < CHAIN_LENGTH && !projCreated)
            {
                Projectile.NewProjectile(projectile.Center + projectile.velocity, projectile.velocity, ProjectileType<EmeraldSpike>(), projectile.damage, projectile.knockBack, Main.myPlayer, chainPos + 1);
                projCreated = true;
            }
            if (drawAlpha >= Math.PI)
                projectile.Kill();
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = GetTexture(AssetDirectory.GemsparklingHive + Name);

            Main.spriteBatch.Draw(tex, (projectile.Center - Main.screenPosition), null, new Color(0, (int)(255 * Math.Sin(drawAlpha)), 0, 0), projectile.velocity.ToRotation(), new Vector2(tex.Width / 2, tex.Height / 2), 1, SpriteEffects.None, 0f);
            base.PostDraw(spriteBatch, lightColor);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}
