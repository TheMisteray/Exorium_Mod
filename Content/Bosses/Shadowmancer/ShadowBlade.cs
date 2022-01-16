using ExoriumMod.Core;
using ExoriumMod.Content.Dusts;
using Terraria;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Bosses.Shadowmancer
{
    class ShadowBlade : ModProjectile
    {
        public override string Texture => AssetDirectory.Shadowmancer + Name;

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.penetrate = -1;
            projectile.timeLeft = 840;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = true;
        }

        public override void AI()
        {
            projectile.rotation += .2f;
            Vector2 delta = projectile.position - new Vector2(projectile.position.X + Main.rand.NextFloat(-1, 2), projectile.position.Y + Main.rand.NextFloat(-1, 2));
            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Shadow>(), delta.X, delta.Y);
            }
            if (Main.rand.NextBool(4))
            {
                int dust0 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Rainbow>(), delta.X, delta.Y);
                Main.dust[dust0].color = new Color(200, 0, 0);
            }
            if (projectile.timeLeft == 490)
            {
                for (int k = 0; k < 255; k++)
                {
                    if (Main.player[k].active)
                    {
                        Vector2 move = Main.player[k].Center - projectile.Center;
                        float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                        if (magnitude > 0)
                            move *= 5f / magnitude;
                        else
                            move = new Vector2(0f, 5f);
                        projectile.velocity = move;
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
                    Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Shadow>(), projectile.oldVelocity.X *1.5f, projectile.oldVelocity.Y *1.5f);
            }
        }

        public override bool ShouldUpdatePosition()
        {
            return (projectile.timeLeft < 490) || (projectile.timeLeft >= 720);
        }
    }
}
