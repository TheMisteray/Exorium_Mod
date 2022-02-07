using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Content.Bosses.GemsparklingHive
{
    class GemDart : ModProjectile
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

        public float color
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = GetTexture(AssetDirectory.GemsparklingHive + Name);

            Color c;

            switch (color)
            {
                case 0:
                    c = new Color(255, 0, 0, 0);
                    break;
                case 1:
                    c = new Color(255, 110, 0, 0);
                    break;
                case 2:
                    c = new Color(255, 247, 0, 0);
                    break;
                case 3:
                    c = new Color(0, 255, 0, 0);
                    break;
                case 4:
                    c = new Color(100, 100, 100, 0);
                    break;
                case 5:
                    c = new Color(35, 0, 255, 0);
                    break;
                case 6:
                    c = new Color(149, 0, 255, 0);
                    break;
                default:
                    c = Color.White;
                    break;
            }

            Main.spriteBatch.Draw(tex, (projectile.Center - Main.screenPosition), null, c, projectile.velocity.ToRotation(), new Vector2(tex.Width/2, tex.Height/2), 1, SpriteEffects.None, 0f);
            base.PostDraw(spriteBatch, lightColor);
        }
    }
}
