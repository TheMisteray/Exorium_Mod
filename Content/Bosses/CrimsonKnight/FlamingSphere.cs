using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using ExoriumMod.Content.Dusts;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    class FlamingSphere : ModProjectile 
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
        }

        public float Target
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public bool Expanded
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value ? 1f : 0f;
        }

        public override void AI()
        {
            Player p = Main.player[(int)Target];
            if (p.active)
            {
                Vector2 trajectory = Main.player[(int)Target].Center - Projectile.Center;
                trajectory.Normalize();
                trajectory *= 4;
                Projectile.velocity = trajectory;

                if ((p.Center - Projectile.Center).Length() < 160)
                {
                    Expanded = true;
                    Projectile.timeLeft = 150;
                }
            }

            if (Projectile.scale < 1)
                Projectile.scale += .02f;
        }

        public override void Kill(int timeLeft)
        {
            if (Expanded)
            {
                int explosionArea = 320;
                Vector2 oldSize = Projectile.Size;
                // Resize the projectile hitbox to be bigger.
                Projectile.position = Projectile.Center;
                Projectile.Size += new Vector2(explosionArea);
                Projectile.Center = Projectile.position;

                Projectile.tileCollide = false;
                Projectile.velocity = Vector2.Zero;
                // Damage enemies inside the hitbox area
                Projectile.Damage();
                Projectile.Damage();
                Projectile.scale = 0.01f;

                //Resize the hitbox to its original size
                Projectile.position = Projectile.Center;
                Projectile.Size = new Vector2(50);
                Projectile.Center = Projectile.position;

                SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

                //Draw explosion area
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 900);
        }

        public override bool CanHitPlayer(Player target)
        {
            return ((target.Center - Projectile.Center).Length() < (target.width/2) + (Projectile.width/2));
        }
    }
}
