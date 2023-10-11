using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using ExoriumMod.Content.Dusts;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Content.Bosses.Shadowmancer
{
    class ShadowOrb : ModProjectile
    {
        public override string Texture => AssetDirectory.Shadowmancer + Name;

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = false;
            Projectile.timeLeft = 600;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.velocity.Y += .18f;
            Projectile.rotation += .2f;
            if (Math.Abs(Projectile.velocity.X) >= 1)
            {
                Projectile.velocity.X *= .96f;
            }
            if (Projectile.timeLeft <= 500 )
                Projectile.tileCollide = true;
            Vector2 delta = Projectile.position - new Vector2(Projectile.position.X + Main.rand.NextFloat(-1, 2), Projectile.position.Y + Main.rand.NextFloat(-1, 2));
            if (Main.rand.NextBool(6))
            {
                int dust0 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Rainbow>(), delta.X, delta.Y);
                Main.dust[dust0].color = new Color(200, 0, 0);
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 5; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Shadow>(), Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
                Main.dust[dust].color = new Color(255, 110, 0);
            }
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            NPC.NewNPC(Projectile.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y, NPCType<ShadowAdd>());
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.Shadowmancer + Name + "_aGlow").Value;
            Main.EntitySpriteDraw(tex, (Projectile.Center - Main.screenPosition), null, Color.White * .4f, Projectile.velocity.ToRotation(), new Vector2(tex.Width / 2, tex.Height / 2), 1, SpriteEffects.None, 0);
        }
    }
}
