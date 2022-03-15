using ExoriumMod.Core;
using ExoriumMod.Content.Dusts;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Content.Bosses.Shadowmancer
{
    class ShadowFist : ModProjectile
    {
        public override string Texture => AssetDirectory.Shadowmancer + Name;

        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 36;
            projectile.penetrate = -1;
            projectile.timeLeft = 380;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = true;
        }

        public override void AI()
        {
            Vector2 delta = projectile.position - new Vector2(projectile.position.X + Main.rand.NextFloat(-1, 2), projectile.position.Y + Main.rand.NextFloat(-1, 2));
            if (projectile.timeLeft >= 300)
            {
                if (Main.rand.NextBool(2))
                {
                     Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Shadow>(), delta.X, delta.Y);
                }
                if (Main.rand.NextBool(4))
                {
                    int dust0 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Rainbow>(), delta.X, delta.Y);
                    Main.dust[dust0].color = new Color(154, 0, 0);
                }
            }
            else
            {
                projectile.velocity.X = 0;
                projectile.velocity.Y = 13;
            }
            if (Main.rand.NextBool(5))
            {
                int dust0 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Rainbow>(), delta.X, delta.Y);
                Main.dust[dust0].color = new Color(200, 0, 0);
            }
            if (projectile.timeLeft == 290)
            {
                projectile.tileCollide = true;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            projectile.Kill();
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 8; i++)
            {
                 Dust.NewDust(projectile.position, projectile.width, projectile.height, DustType<Shadow>(), projectile.oldVelocity.X * -1.5f, projectile.oldVelocity.Y * -1.5f);
            }
            Main.PlaySound(SoundID.Item14, projectile.position);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = GetTexture(AssetDirectory.Shadowmancer + Name + "_aGlow");
            Vector2 drawCenter = projectile.Center;
            drawCenter.Y += 2;
            drawCenter.X -= 2;
            Main.spriteBatch.Draw(tex, (drawCenter - Main.screenPosition), null, Color.White * .2f, 0, new Vector2(tex.Width / 2, tex.Height / 2), 1, SpriteEffects.None, 0f);
        }
    }
}
