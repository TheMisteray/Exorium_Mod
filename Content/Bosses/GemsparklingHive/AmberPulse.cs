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
            projectile.width = 128;
            projectile.height = 128;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.alpha = 255;
            projectile.timeLeft = 600;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
        }

        public float scalar
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public override void AI()
        {
            scalar += MathHelper.PiOver4 / 30;
            projectile.rotation += .07f;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = GetTexture(AssetDirectory.GemsparklingHive + "SapphireRing");

            Main.spriteBatch.Draw(tex, (projectile.Center - Main.screenPosition), null, new Color(255, 110, 0, 0), projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2), 1 + (float)((Math.Sin(scalar) * 1.5) + 1), SpriteEffects.None, 0f) ;
            base.PostDraw(spriteBatch, lightColor);
        }

        public override bool CanHitPlayer(Player target)
        {
            Texture2D tex = GetTexture(AssetDirectory.GemsparklingHive + "SapphireRing");

            float dist = Vector2.Distance(target.Center, projectile.Center);
            if (dist < (tex.Width/2) * (1 + (float)((Math.Sin(scalar) * 2) + 1)) + target.width/2)
                return true;
            return false;
        }
    }
}
