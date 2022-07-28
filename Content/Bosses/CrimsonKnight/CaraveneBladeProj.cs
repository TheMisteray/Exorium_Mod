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
        public override string Texture => AssetDirectory.CrimsonKnight + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flametounge SwordBeam");
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

            Main.spriteBatch.Draw(tex, (Projectile.Center - Main.screenPosition), null, new Color(254, 121, 2) * ((600 - spawnActionTimer) / 600), Projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2), 1, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, Enrage ? 600 : 300);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (spawnActionTimer > 0)
                return false;
            return base.Colliding(projHitbox, targetHitbox);
        }
    }

    class CaraveneBladeProjHorizontal : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + "CaraveneBladeProj";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flametounge SwordBeam");
        }

        public override void SetDefaults()
        {
            Projectile.width = 300;
            Projectile.height = 42;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.alpha = 255;
        }

        public bool left
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value ? 1f : 0f;
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
                if (left)
                {
                    spawnActionTimer -= 10;
                    Projectile.alpha -= 25;
                    Projectile.position.X++;
                }
                else
                {
                    spawnActionTimer -= 10;
                    Projectile.alpha -= 25;
                    Projectile.position.X--;
                }
            }
            else
            {
                if (left)
                    Projectile.velocity.X = -14;
                else
                    Projectile.velocity.X = 14;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.CrimsonKnight + "CaraveneBladeProj", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            Main.spriteBatch.Draw(tex, (Projectile.Center - Main.screenPosition), null, new Color(254, 121, 2) * ((600 - spawnActionTimer) / 600), left? MathHelper.PiOver2 : -MathHelper.PiOver2, new Vector2(tex.Width / 2, tex.Height / 2), 1, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, Enrage ? 600 : 300);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (spawnActionTimer > 0)
                return false;
            return base.Colliding(projHitbox, targetHitbox);
        }
    }
}
