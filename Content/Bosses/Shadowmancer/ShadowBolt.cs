using ExoriumMod.Core;
using ExoriumMod.Content.Dusts;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Content.Bosses.Shadowmancer
{
    class ShadowBolt : ModProjectile
    {
        public override string Texture => AssetDirectory.Shadowmancer + Name;

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.penetrate = -1;
            projectile.timeLeft = 600;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = true;
        }

        public override void AI()
        {
            projectile.rotation += .1f;
            Vector2 delta = projectile.position - new Vector2(projectile.position.X + Main.rand.NextFloat(-1, 2), projectile.position.Y + Main.rand.NextFloat(-1, 2));
            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Shadow>(), delta.X, delta.Y);
            }
            if (Main.rand.NextBool(5))
            {
                int dust0 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Rainbow>(), delta.X, delta.Y);
                Main.dust[dust0].color = new Color(200, 0, 0);
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item20, projectile.position);
        }

        public override bool ShouldUpdatePosition()
        {
            return projectile.timeLeft <= 540;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = GetTexture(AssetDirectory.Shadowmancer + Name);
            Main.spriteBatch.Draw(tex, (projectile.Center - Main.screenPosition), null, Color.White * .2f, projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2), 1, SpriteEffects.None, 0f);
        }
    }
}
