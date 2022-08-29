using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using ExoriumMod.Content.Dusts;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    internal class RiposteFireball : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + "CaraveneFireball";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fireball");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
        }

        private int counter;
        private Vector2 originalPos = Vector2.Zero;

        public float target
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public float num
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void AI()
        {
            if (Projectile.timeLeft > 120)
            {
                if (originalPos == Vector2.Zero)
                {
                    originalPos = Projectile.Center;
                }

                Vector2 orbitPoint = originalPos;
                Vector2 offset = new Vector2(50, 0);
                counter += 2;
                int orbitCount = 0;
                foreach (Projectile p in Main.projectile)
                {
                    if (p.type == this.Type)
                        orbitCount++;
                }
                float rotation = (360 / orbitCount) * num;
                offset = offset.RotatedBy(MathHelper.ToRadians(rotation + counter));

                Projectile.position = orbitPoint + offset;
            }
            else
            {
                Projectile.scale -= .01f;
                if (Projectile.scale <= 0)
                {
                    Projectile.Kill();
                    Vector2 velocityToPlayer = Main.player[(int)target].Center - Projectile.Center;
                    velocityToPlayer.Normalize();
                    velocityToPlayer *= 12;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocityToPlayer, ProjectileType<backupFireball>(), Projectile.damage, 1, Main.myPlayer, target);
                }
            }
        }
    }
}
