using ExoriumMod.Core;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Weapons.Magic
{
    class Firebolt : ModItem
    {
        public override string Texture => AssetDirectory.MagicWeapon + Name;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.rare = 1;
            Item.mana = 7;
            Item.UseSound = SoundID.Item20;
            Item.noMelee = true;
            Item.useStyle = 5;
            Item.damage = 11;
            Item.useAnimation = 27;
            Item.useTime = 27;
            Item.width = 24;
            Item.height = 28;
            Item.shoot = ProjectileType<FireboltProj>();
            Item.scale = 0.9f;
            Item.shootSpeed = 9f;
            Item.knockBack = 5f;
            Item.DamageType = DamageClass.Magic;
            Item.value = 50000;
        }
    }

    class FireboltProj : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 2;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            int num2475 = 0;
            for (int num2378 = 0; num2378 < 5; num2378 = num2475 + 1)
            {
                float num2375 = Projectile.velocity.X / 3f * (float)num2378;
                float num2374 = Projectile.velocity.Y / 3f * (float)num2378;
                int num2373 = 4;
                Vector2 position41 = new Vector2(Projectile.position.X + (float)num2373, Projectile.position.Y + (float)num2373);
                int width29 = Projectile.width - num2373 * 2;
                int height29 = Projectile.height - num2373 * 2;
                int num2372 = Dust.NewDust(position41, width29, height29, 6, 0f, 0f, 100, default(Color), 1.2f);
                Main.dust[num2372].noGravity = true;
                Dust dust81 = Main.dust[num2372];
                dust81.velocity *= 0.1f;
                dust81 = Main.dust[num2372];
                dust81.velocity += Projectile.velocity * 0.1f;
                Dust expr_4827_cp_0 = Main.dust[num2372];
                expr_4827_cp_0.position.X = expr_4827_cp_0.position.X - num2375;
                Dust expr_4842_cp_0 = Main.dust[num2372];
                expr_4842_cp_0.position.Y = expr_4842_cp_0.position.Y - num2374;
                num2475 = num2378;
            }
            if (Main.rand.NextBool(5))
            {
                int num2377 = 4;
                Vector2 position42 = new Vector2(Projectile.position.X + (float)num2377, Projectile.position.Y + (float)num2377);
                int width30 = Projectile.width - num2377 * 2;
                int height30 = Projectile.height - num2377 * 2;
                int num2376 = Dust.NewDust(position42, width30, height30, 6 /*172*/, 0f, 0f, 100, default(Color), 0.6f);
                Dust dust81 = Main.dust[num2376];
                dust81.velocity *= 0.25f;
                dust81 = Main.dust[num2376];
                dust81.velocity += Projectile.velocity * 0.5f;
            }
            else
            {
                Projectile.rotation += 0.3f * (float)Projectile.direction;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                Projectile.ai[0] += 0.1f;
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(10) == 0)
                target.AddBuff(BuffID.OnFire, 300);
        }
    }
}
