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
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.alpha = 255;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }

        public float color
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.GemsparklingHive + Name).Value;

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
                    c = new Color(200, 200, 200, 0);
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

            Main.EntitySpriteDraw(tex, (Projectile.Center - Main.screenPosition), null, c, Projectile.velocity.ToRotation(), new Vector2(tex.Width/2, tex.Height/2), 1, SpriteEffects.None, 0);
            base.PostDraw(lightColor);
        }
    }
}
