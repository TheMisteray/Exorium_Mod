using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using System;

namespace ExoriumMod.Content.Bosses.GemsparklingHive
{
    class TopazBeam : ModProjectile
    {
        class LustrousBeam : ModProjectile
        {
            public override string Texture => AssetDirectory.Invisible;

            public override void SetDefaults()
            {
                projectile.width = 12;
                projectile.height = 12;
                projectile.aiStyle = -1;
                projectile.friendly = false;
                projectile.hostile = true;
                projectile.extraUpdates = 50;
                projectile.timeLeft = 50;
                projectile.penetrate = 1;
            }

            public float mode
            {
                get => projectile.ai[0];
                set => projectile.ai[0] = value;
            }

            public override void AI()
            {
                projectile.alpha = 225;
                for (int i = 0; i < ((mode==1f)? 10 : 1); i++)
                {
                    int dust0 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Dusts.Rainbow>(), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 0, new Color(255, 247, 0));
                    Main.dust[dust0].position.X -= projectile.velocity.X / 10f * i;
                    Main.dust[dust0].position.Y -= projectile.velocity.Y / 10f * i;
                }
            }

            public override bool OnTileCollide(Vector2 oldVelocity)
            {
                if (projectile.velocity.X != oldVelocity.X)
                {
                    projectile.velocity.X = -oldVelocity.X;
                }
                if (projectile.velocity.Y != oldVelocity.Y)
                {
                    projectile.velocity.Y = -oldVelocity.Y;
                }
                return false;
            }

            public override bool CanHitPlayer(Player target)
            {
                return mode == 1;
            }
        }
    }
}
