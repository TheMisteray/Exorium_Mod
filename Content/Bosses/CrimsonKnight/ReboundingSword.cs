using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    internal class ReboundingSword : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Flametounge Swordbeam");
        }

        public override void SetDefaults()
        {
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1800;
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

        public float bottom
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        bool rebounded = false;

        public override void AI()
        {
            if (fadeIn > 0)
                fadeIn--;

            if (!rebounded)
            {
                if (bottom == 1 && Projectile.Center.Y >= Core.Systems.WorldDataSystem.FallenTowerRect.Bottom - 80)
                {
                    Projectile.velocity.Y *= -1;
                    rebounded = true;
                    SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
                }
                else if (bottom == 0 && Projectile.Center.Y <= Core.Systems.WorldDataSystem.FallenTowerRect.Top + 160)
                {
                    Projectile.velocity.Y *= -1;
                    rebounded = true;
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (fadeIn == 0)
            {
                Vector2 SwordTip = new Vector2(0, Projectile.width / 2);
                SwordTip = SwordTip.RotatedBy(Projectile.velocity.ToRotation() - MathHelper.PiOver2);
                float _ = float.NaN;

                //Check collision of line from sword center to sword end with target hitbox
                return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - SwordTip, Projectile.Center + SwordTip, 21 * Projectile.scale, ref _);
            }
            else
                return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.CrimsonKnight + "CaraveneBladeProj").Value;

            Main.spriteBatch.Draw(tex, (Projectile.Center - Main.screenPosition), null, new Color(254, 121, 2) * ((60 - fadeIn) / 60), Projectile.velocity.ToRotation() - MathHelper.PiOver2, new Vector2(tex.Width / 2, tex.Height / 2), .5f, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }
    }

    internal class indicatorRainSword : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Flametounge Swordbeam");
        }

        public override void SetDefaults()
        {
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
        }

        public float fadeIn
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            if (fadeIn > 0)
                fadeIn-=5;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.CrimsonKnight + "CaraveneBladeProj").Value;

            Main.spriteBatch.Draw(tex, (Projectile.Center - Main.screenPosition), null, new Color(254, 121, 2) * ((60 - fadeIn) / 60), Projectile.velocity.ToRotation() - MathHelper.PiOver2, new Vector2(tex.Width / 2, tex.Height / 2), .5f, SpriteEffects.None, 0f);
            return false;
        }
    }
}
