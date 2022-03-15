using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
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
            Main.projFrames[projectile.type] = 9;
        }

        public override void SetDefaults()
        {
            projectile.width = 90;
            projectile.height = 90;
            projectile.penetrate = -1;
            projectile.timeLeft = 260;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.alpha = 30;
        }

        public override void AI()
        {
            if (projectile.timeLeft == 260)
            {
                Main.PlaySound(SoundID.Item121, projectile.position);
                projectile.hostile = false;
            }
            else if (projectile.timeLeft > 180)
            {
                if (projectile.timeLeft % 5 == 0 && projectile.frame != 7)
                    projectile.frame ++;
            }
            else if (projectile.timeLeft == 180)
            {
                projectile.frame++;
                projectile.hostile = true;
                Main.PlaySound(SoundID.Item124, projectile.position);
            }
            else if (projectile.timeLeft < 180)
            {
                projectile.position = projectile.Center;
                projectile.scale += 0.02f;
                projectile.Center = projectile.position;
                projectile.alpha += 5;
                projectile.hostile = false;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff((BuffID.Confused), 240, false);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = GetTexture(AssetDirectory.Shadowmancer + Name + "_aGlow");
            int frameHeight = tex.Height / Main.projFrames[projectile.type];
            int startY = frameHeight * projectile.frame;
            Main.spriteBatch.Draw(tex, (projectile.Center - Main.screenPosition), new Rectangle(0, startY, projectile.width, projectile.height), Color.Lerp(new Color(0, 0, 0, 0), new Color(50, 50, 50, 50), (float)(-1 * (projectile.alpha - 255)) / 255f), 0, new Vector2(tex.Width / 2, frameHeight / 2), projectile.scale, SpriteEffects.None, 0f); ;
        }
    }
}
