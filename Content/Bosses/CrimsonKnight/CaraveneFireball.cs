using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using ExoriumMod.Content.Dusts;
using Microsoft.Xna.Framework.Graphics;
using ExoriumMod.Core.Utilities;
using Terraria.Graphics.Shaders;

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    class CaraveneFireball : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + Name;

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

        public bool Enrage
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value ? 1f : 0f;
        }

        public override void AI()
        {
            Projectile.velocity.Y += .20f;
            Projectile.rotation += .2f;
            if (Main.rand.NextBool(6))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SolarFlare, Projectile.velocity.X * Main.rand.NextFloat(.25f), 0, 0, default, 1);
                Main.dust[dust].noGravity = true;
            }
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
