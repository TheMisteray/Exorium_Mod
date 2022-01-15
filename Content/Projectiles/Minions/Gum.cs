using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using ExoriumMod.Dusts;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Projectiles.Minions
{
    class Gum : ModProjectile
    {
        private int color;
        private const int MAX_TICKS = 25;
        private int ticks = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionShot[projectile.type] = true;
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 22;
            projectile.timeLeft = 300;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.alpha = 0;
            color = Main.rand.Next(6);
        }

        public override void AI()
        {
            // Loop frames
            int frameSpeed = 5;
            projectile.frameCounter++;
            if (projectile.frameCounter >= frameSpeed)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }

            ticks++;
            if (ticks >= MAX_TICKS)
            {
                const float velYmult = 0.5f;
                ticks = MAX_TICKS;
                projectile.velocity.Y += velYmult;
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //I don't think this is the best way to do this but I wanted to try drawcode
            Texture2D tex = GetTexture(Texture);
            Color[] colors = new Color[] {Color.Red, Color.Orange, Color.Yellow, Color.Blue, Color.Violet, Color.Pink};
            //spriteBatch.Draw(tex, projectile.position, tex.Frame(), colors[Main.rand.Next(colors.Length)], projectile.rotation, Vector2.Zero, projectile.scale, 0, 0);
            spriteBatch.Draw(tex, (projectile.position - Main.screenPosition) + new Vector2(0, Main.player[projectile.owner].gfxOffY), new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height), colors[color], projectile.rotation, Vector2.Zero, projectile.scale, 0, 0);
            base.PostDraw(spriteBatch, lightColor);
        }
    }
}
