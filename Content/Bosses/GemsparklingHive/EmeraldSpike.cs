using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExoriumMod.Content.Bosses.GemsparklingHive
{
    class EmeraldSpike : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.alpha = 255;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }

        private const int CHAIN_LENGTH = 60;

        private float drawAlpha = 0;

        bool projCreated = false;

        public float chainPos
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            drawAlpha += MathHelper.PiOver2 / 10;
            if (Main.netMode != NetmodeID.MultiplayerClient && drawAlpha > MathHelper.PiOver4 && chainPos < CHAIN_LENGTH && !projCreated)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity, Projectile.velocity, ProjectileType<EmeraldSpike>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, chainPos + 1);
                projCreated = true;
            }
            if (drawAlpha >= Math.PI)
                Projectile.Kill();
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.GemsparklingHive + Name).Value;

            Main.EntitySpriteDraw(tex, (Projectile.Center - Main.screenPosition), null, new Color(0, (int)(255 * Math.Sin(drawAlpha)), 0, 0), Projectile.velocity.ToRotation(), new Vector2(tex.Width / 2, tex.Height / 2), 1, SpriteEffects.None, 0);
            base.PostDraw(lightColor);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}
