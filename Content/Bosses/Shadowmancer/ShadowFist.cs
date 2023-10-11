using ExoriumMod.Core;
using ExoriumMod.Content.Dusts;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;
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
            Projectile.width = 40;
            Projectile.height = 36;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 380;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
        }

        public override void AI()
        {
            Vector2 delta = Projectile.position - new Vector2(Projectile.position.X + Main.rand.NextFloat(-1, 2), Projectile.position.Y + Main.rand.NextFloat(-1, 2));
            if (Projectile.timeLeft >= 300)
            {
                if (Main.rand.NextBool(2))
                {
                     Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Shadow>(), delta.X, delta.Y);
                }
                if (Main.rand.NextBool(4))
                {
                    int dust0 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Rainbow>(), delta.X, delta.Y);
                    Main.dust[dust0].color = new Color(154, 0, 0);
                }
            }
            else
            {
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 13;
            }
            if (Main.rand.NextBool(5))
            {
                int dust0 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Rainbow>(), delta.X, delta.Y);
                Main.dust[dust0].color = new Color(200, 0, 0);
            }
            if (Projectile.timeLeft == 290)
            {
                Projectile.tileCollide = true;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.Kill();
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 8; i++)
            {
                 Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<Shadow>(), Projectile.oldVelocity.X * -1.5f, Projectile.oldVelocity.Y * -1.5f);
            }
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.Shadowmancer + Name + "_aGlow").Value;
            Vector2 drawCenter = Projectile.Center;
            drawCenter.Y += 2;
            drawCenter.X -= 2;
            Main.EntitySpriteDraw(tex, (drawCenter - Main.screenPosition), null, Color.White * .2f, 0, new Vector2(tex.Width / 2, tex.Height / 2), 1, SpriteEffects.None, 0);
        }
    }
}
