using ExoriumMod.Core;
using ExoriumMod.Content.Dusts;
using Terraria;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Content.Bosses.Shadowmancer
{
    class ShadowBlade : ModProjectile
    {
        public override string Texture => AssetDirectory.Shadowmancer + Name;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 840;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
        }

        public override void AI()
        {
            Projectile.rotation += .2f;
            Vector2 delta = Projectile.position - new Vector2(Projectile.position.X + Main.rand.NextFloat(-1, 2), Projectile.position.Y + Main.rand.NextFloat(-1, 2));
            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Shadow>(), delta.X, delta.Y);
            }
            if (Main.rand.NextBool(4))
            {
                int dust0 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Rainbow>(), delta.X, delta.Y);
                Main.dust[dust0].color = new Color(200, 0, 0);
            }
            if (Projectile.timeLeft == 490)
            {
                for (int k = 0; k < 255; k++)
                {
                    if (Main.player[k].active)
                    {
                        Vector2 move = Main.player[k].Center - Projectile.Center;
                        float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                        if (magnitude > 0)
                            move *= 5f / magnitude;
                        else
                            move = new Vector2(0f, 5f);
                        Projectile.velocity = move;
                        k = 225;
                    }
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i<8; i++)
            {
                if (Main.rand.NextBool(3))
                    Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Shadow>(), Projectile.oldVelocity.X *1.5f, Projectile.oldVelocity.Y *1.5f);
            }
        }

        public override bool ShouldUpdatePosition()
        {
            return (Projectile.timeLeft < 490) || (Projectile.timeLeft >= 720);
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.Shadowmancer + Name + "_aGlow").Value;
            Main.EntitySpriteDraw(tex, (Projectile.Center - Main.screenPosition), null, Color.White * .3f, Projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2), 1, SpriteEffects.None, 0);
        }
    }
}
