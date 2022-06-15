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
    class CaraveneBladeProj : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flametounge Greatsword");
        }

        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 300;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.alpha = 255;
        }

        public float SpawnBehavior
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public bool Enrage
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value ? 1f : 0f;
        }

        public float spawnActionTimer = 600;

        public override void AI()
        {
            if (spawnActionTimer > 0)
            {
                spawnActionTimer -= 10;
                Projectile.alpha -= 25;
                Projectile.position.Y--;
            }
            else
            {
                Projectile.velocity.Y = 14;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.CrimsonKnight + Name).Value;

            Main.spriteBatch.Draw(tex, (Projectile.Center - Main.screenPosition), null, new Color(254, 121, 2) * ((600 - spawnActionTimer)/600), Projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2), 1, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, Enrage ? 600 : 300);
        }
    }
}
