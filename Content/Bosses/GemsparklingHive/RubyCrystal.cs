using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Content.Bosses.GemsparklingHive
{
    class RubyCrystal : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
        }

        public float fuse
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public float rotationSpeed
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void AI()
        {
            Projectile.rotation += rotationSpeed;
            if (rotationSpeed > 0)
                rotationSpeed -= .002f;
            Projectile.alpha = 225;
            Projectile.velocity *= .98f;
            if (Projectile.velocity.Length() <= .1f)
            {
                fuse++;
                if (fuse > 60)
                {
                    Projectile.Kill();
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 8; i++)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 v = new Vector2(0, 7);
                    Vector2 v2 = v.RotatedBy(MathHelper.PiOver4 * i + Projectile.rotation);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, v2, ProjectileType<GemDart>(), Projectile.damage, 1, Main.myPlayer, 0);
                }
            }
            base.Kill(timeLeft);
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.GemsparklingHive + Name).Value;

            Main.EntitySpriteDraw(tex, (Projectile.Center - Main.screenPosition), null, new Color(255, 0, 0, 0), Projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2), 1, SpriteEffects.None, 0);
            base.PostDraw(lightColor);
        }
    }
}
