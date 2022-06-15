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
            Projectile.width = 128;
            Projectile.height = 128;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.alpha = 255;
            Projectile.timeLeft = 1200;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }

        public float scalar
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            scalar += MathHelper.PiOver4 / 30;
            Projectile.rotation += .07f;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.GemsparklingHive + "SapphireRing").Value;

            Main.EntitySpriteDraw(tex, (Projectile.Center - Main.screenPosition), null, new Color(255, 110, 0, 0), Projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2), 1 + (float)((Math.Sin(scalar) * 1.5) + 1), SpriteEffects.None, 0) ;
            base.PostDraw(lightColor);
        }

        public override bool CanHitPlayer(Player target)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.GemsparklingHive + "SapphireRing").Value;

            float dist = Vector2.Distance(target.Center, Projectile.Center);
            if (dist < (tex.Width/2) * (1 + (float)((Math.Sin(scalar) * 2) + 1)) + target.width/2)
                return true;
            return false;
        }
    }
}
