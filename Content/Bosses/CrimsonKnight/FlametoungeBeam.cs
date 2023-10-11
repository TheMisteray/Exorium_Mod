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
    internal class FlametoungeBeam : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Flametounge Swordbeam");
        }

        public override void SetDefaults()
        {
            Projectile.width = 300;
            Projectile.height = 300;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.alpha = 255;
        }

        public float fadeIn
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            if (Projectile.velocity.Length() < 20)
                Projectile.velocity *= 1.04f;
            if (fadeIn > 0)
                fadeIn--;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (fadeIn == 0)
            {
                Vector2 SwordTip = new Vector2(0, Projectile.width / 2);
                SwordTip = SwordTip.RotatedBy(Projectile.velocity.ToRotation() - MathHelper.PiOver2);
                float _ = float.NaN;

                //Check collision of line from sword center to sword end with target hitbox
                return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - SwordTip, Projectile.Center + SwordTip, 42 * Projectile.scale, ref _);
            }
            else 
                return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.CrimsonKnight + "CaraveneBladeProj").Value;

            Main.spriteBatch.Draw(tex, (Projectile.Center - Main.screenPosition), null, new Color(254, 121, 2) * ((60 - fadeIn) / 60), Projectile.velocity.ToRotation() - MathHelper.PiOver2, new Vector2(tex.Width / 2, tex.Height / 2), 1, SpriteEffects.None, 0f);
            return false;
        }
    }
}
