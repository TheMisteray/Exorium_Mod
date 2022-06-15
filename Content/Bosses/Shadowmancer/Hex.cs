using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Bosses.Shadowmancer
{
    class Hex : ModProjectile
    {
        public override string Texture => AssetDirectory.Shadowmancer + Name;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 9;
        }

        public override void SetDefaults()
        {
            Projectile.width = 90;
            Projectile.height = 90;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 260;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.alpha = 30;
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 260)
            {
                SoundEngine.PlaySound(SoundID.Item121, Projectile.position);
                Projectile.hostile = false;
            }
            else if (Projectile.timeLeft > 180)
            {
                if (Projectile.timeLeft % 5 == 0 && Projectile.frame != 7)
                    Projectile.frame ++;
            }
            else if (Projectile.timeLeft == 180)
            {
                Projectile.frame++;
                Projectile.hostile = true;
                SoundEngine.PlaySound(SoundID.Item124, Projectile.position);
            }
            else if (Projectile.timeLeft < 180)
            {
                Projectile.position = Projectile.Center;
                Projectile.scale += 0.02f;
                Projectile.Center = Projectile.position;
                Projectile.alpha += 5;
                Projectile.hostile = false;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff((BuffID.Confused), 240, false);
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.Shadowmancer + Name + "_aGlow").Value;
            int frameHeight = tex.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;
            Main.EntitySpriteDraw(tex, (Projectile.Center - Main.screenPosition), new Rectangle(0, startY, Projectile.width, Projectile.height), Color.Lerp(new Color(0, 0, 0, 0), new Color(50, 50, 50, 50), (float)(-1 * (Projectile.alpha - 255)) / 255f), 0, new Vector2(tex.Width / 2, frameHeight / 2), Projectile.scale, SpriteEffects.None, 0); ;
        }
    }
}
