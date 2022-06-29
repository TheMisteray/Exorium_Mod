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
    class FlameTrail : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + Name;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 28;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.ai[0] = 0; //false
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
            else if (Projectile.frame <= 1 && Main.rand.NextBool(60))
                Dust.NewDust(Projectile.BottomLeft, Projectile.width, 1, DustID.SolarFlare, 0, -4, 0, default, Main.rand.NextFloat(.25f, 1));

            Projectile.frameCounter++;

            //Frame loop
            if (Projectile.frameCounter >= 8)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 3;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            Vector2 tileBottom = new Vector2(Projectile.position.X + Projectile.width / 2, Projectile.position.Y + Projectile.height);
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

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }
    }
}
