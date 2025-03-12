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
    class FireballRing : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + "CaraveneFireball";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
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

        private const float HEIGHT = 60;
        Vector2 spawnAxis = Vector2.Zero;

        public float RotationOffset
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public bool Enrage
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value ? 1f : 0f;
        }

        public float WIDTH
        {
            get => Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 1200)
                spawnAxis = Projectile.position;

            spawnAxis.Y += 5;
            Vector2 offsetAxel = new Vector2(WIDTH * (float)Math.Sin(RotationOffset), HEIGHT * (float)Math.Sin(RotationOffset - MathHelper.PiOver2));
            Projectile.position = spawnAxis + offsetAxel;

            Projectile.rotation += .2f;
            RotationOffset += .006f;
            if (Main.expertMode)
                RotationOffset += .003f;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, Enrage ? 600 : 300);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(Texture).Value;
            //Afterimages
            for (int k = Projectile.oldPos.Length - 1; k >= 0; k--)
            {
                Vector2 pos = Projectile.oldPos[k];

                Main.EntitySpriteDraw(tex, pos - Main.screenPosition + new Vector2(Projectile.width / 2, Projectile.height / 2), new Rectangle(0, 0, Projectile.width, Projectile.height), new Color(255 / (k + 1), 255 / (k + 1), 255 / (k + 1), 255 / (k + 1)), Projectile.oldRot[k], new Vector2(tex.Width / 2, tex.Height / 2), Projectile.scale, SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, Projectile.width, Projectile.height), Color.White, Projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
