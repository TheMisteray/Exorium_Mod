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
            projectile.width = 32;
            projectile.height = 32;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.timeLeft = 600;
            projectile.penetrate = 1;
            projectile.tileCollide = false;
        }

        public float fuse
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public float rotationSpeed
        {
            get => projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        public override void AI()
        {
            projectile.rotation += rotationSpeed;
            if (rotationSpeed > 0)
                rotationSpeed -= .002f;
            projectile.alpha = 225;
            projectile.velocity *= .98f;
            if (projectile.velocity.Length() <= .1f)
            {
                fuse++;
                if (fuse > 60)
                {
                    projectile.Kill();
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
                    Vector2 v2 = v.RotatedBy(MathHelper.PiOver4 * i + projectile.rotation);
                    Projectile.NewProjectile(projectile.Center, v2, ProjectileType<GemDart>(), projectile.damage, 1, Main.myPlayer, 0);
                }
            }
            base.Kill(timeLeft);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = GetTexture(AssetDirectory.GemsparklingHive + Name);

            Main.spriteBatch.Draw(tex, (projectile.Center - Main.screenPosition), null, new Color(255, 0, 0, 0), projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2), 1, SpriteEffects.None, 0f);
            base.PostDraw(spriteBatch, lightColor);
        }
    }
}
