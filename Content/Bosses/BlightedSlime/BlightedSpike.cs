using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Bosses.BlightedSlime
{
    class BlightedSpike : ModProjectile
    {
        public override string Texture => AssetDirectory.BlightedSlime + Name;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            projectile.width = 56;
            projectile.height = 360;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.timeLeft = 33000;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.extraUpdates = 99;
            projectile.alpha = 255;
            projectile.ai[0] = 0; //false
            projectile.ai[1] = 0; //false
        }

        public bool hasTouchedGround
        {
            get => projectile.ai[0] == 1f;
            set => projectile.ai[0] = value ? 1f : 0f;
        }

        public bool hasTouchedAir
        {
            get => projectile.ai[1] == 1f;
            set => projectile.ai[1] = value ? 1f : 0f;
        }

        public override void AI()
        {
            /**Point pointBottom = projectile.Bottom.ToTileCoordinates();
            projectile.velocity.Y = 0;
            if (!(Main.tile[pointBottom.X, pointBottom.Y].nactive() && (Main.tile[pointBottom.X, pointBottom.Y].collisionType >= 2 || Main.tile[pointBottom.X, pointBottom.Y].collisionType == -1)))
            {
                if (hasTouchedGround)
                {
                    projectile.position.Y += 1;
                    hasTouchedAir = true;
                }
                else
                {
                    projectile.position.Y += 14;
                    hasTouchedAir = true;
                }
            }
            else
            {
                if (hasTouchedAir)
                {
                    projectile.position.Y -= 1;
                    hasTouchedGround = true;
                }
                else
                {
                    projectile.position.Y -= 14;
                    hasTouchedGround = true;
                }
            }
            if (hasTouchedGround)
            {
                projectile.position.Y += 1;
                hasTouchedAir = true;
            }
            else
            {
                projectile.position.Y += 7;
            }
            **/
            if (!hasTouchedGround)
                projectile.velocity.Y = 1;
            if (projectile.frame != 7)
                projectile.hostile = false;
            else
                projectile.hostile = true;

            if (projectile.timeLeft >= 30000)
                projectile.alpha = 255;
            else if (projectile.timeLeft < 4000)
            {
                if (projectile.timeLeft % 400 == 0)
                    --projectile.frame;
                if (projectile.frame == 0)
                    projectile.Kill();
            }
            else if (projectile.frame != 7)
            {
                if (++projectile.frameCounter >= 400)
                {
                    projectile.frameCounter = 0;
                    if (++projectile.frame == 7)
                        Main.PlaySound(SoundID.Item89, projectile.position);
                }
                projectile.alpha = 0;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = false;
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            /*if (!(hasTouchedAir && hasTouchedGround))
                projectile.position.Y -= 7;
            else
                projectile.position.Y -= 1;*/
            hasTouchedGround = true;
            projectile.velocity = Vector2.Zero;
            projectile.position.Y += 1;
            return false;
        }
    }
}
