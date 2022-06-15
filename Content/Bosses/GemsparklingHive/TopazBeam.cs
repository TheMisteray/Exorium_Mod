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
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.extraUpdates = 50;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = false;
        }

        public float mode
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            Projectile.alpha = 225;
            if (mode == 0)
            {
                if (Main.rand.NextBool(100))
                {
                    int dust0 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Dusts.Rainbow>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 0, new Color(255, 247, 0));
                    Main.dust[dust0].position.X -= Projectile.velocity.X / 10f * Main.rand.Next(10);
                    Main.dust[dust0].position.Y -= Projectile.velocity.Y / 10f * Main.rand.Next(10);
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    int dust0 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Dusts.Rainbow>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 0, new Color(255, 247, 0));
                    Main.dust[dust0].position.X -= Projectile.velocity.X / 10f * i;
                    Main.dust[dust0].position.Y -= Projectile.velocity.Y / 10f * i;
                }
            }
        }

        public override bool CanHitPlayer(Player target)
        {
            return mode == 1;
        }
    }
}
