using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Bosses.BlightedSlime
{
    class BlightedSpike : ModProjectile
    {
        public override string Texture => AssetDirectory.BlightedSlime + Name;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.width = 56;
            Projectile.height = 360;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 33000;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 99;
            Projectile.alpha = 255;
            Projectile.ai[0] = 0; //false
            Projectile.ai[1] = 0; //false
        }

        public bool hasTouchedGround
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value ? 1f : 0f;
        }

        public bool hasTouchedAir
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value ? 1f : 0f;
        }

        public override void AI()
        {
            if (!hasTouchedGround)
                Projectile.velocity.Y = 1;
            else if (Projectile.frame <= 1 && Main.rand.Next(200) == 0)
                Dust.NewDust(Projectile.BottomLeft, Projectile.width, 1, DustType<Dusts.BlightDust>(), 0, -10, 0, default, Main.rand.NextFloat(1, 2));

                if (Projectile.frame != 7)
                Projectile.hostile = false;
            else
                Projectile.hostile = true;

            if (Projectile.timeLeft >= 30000)
                Projectile.alpha = 255;
            else if (Projectile.timeLeft < 4000)
            {
                if (Projectile.timeLeft % 400 == 0)
                    --Projectile.frame;
                if (Projectile.frame == 0)
                    Projectile.Kill();
            }
            else if (Projectile.frame != 7)
            {
                if (++Projectile.frameCounter >= 400)
                {
                    Projectile.frameCounter = 0;
                    if (++Projectile.frame == 7)
                        SoundEngine.PlaySound(SoundID.Item89, Projectile.position);
                }
                Projectile.alpha = 0;
            }
        }

        //Checks only bottom tile for collision
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            Vector2 tileBottom = new Vector2(Projectile.position.X + Projectile.width/2, Projectile.position.Y + Projectile.height);
            if (!Main.tile[tileBottom.ToTileCoordinates().X, tileBottom.ToTileCoordinates().Y].IsActuated)
                return true;
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            hasTouchedGround = true;
            Projectile.velocity = Vector2.Zero;

            Projectile.position.Y += 1;
            return false;
        }
    }
}
