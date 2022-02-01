using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
    }
}
