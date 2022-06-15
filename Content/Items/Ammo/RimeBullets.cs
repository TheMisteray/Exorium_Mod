using ExoriumMod.Core;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Ammo
{
    class RimeBullets : ModItem
    {
        public override string Texture => AssetDirectory.Ammo + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Chance to inflict Frostburn");
            DisplayName.SetDefault("Rime Bullet");
        }

        public override void SetDefaults()
        {
            Item.value = 2;
            Item.width = 12;
            Item.height = 12;
            Item.rare = 1;
            Item.maxStack = 999;
            Item.damage = 5;
            Item.consumable = true;
            Item.shoot = ProjectileType<RimeBullet>();
            Item.shootSpeed = 14;
            Item.ammo = AmmoID.Bullet;
            Item.DamageType = DamageClass.Ranged;
            Item.crit = 4;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.RimestoneBar>());
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

    class RimeBullet : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rimestone Bullet");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 2600;
            Projectile.alpha = 0;
            Projectile.light = 0.2f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
            Projectile.velocity.X *= 1.5f;
            Projectile.velocity.Y *= 1.5f;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(10))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 67, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            damage -= 1;
            if (Main.rand.Next(0, 3) == 1)
            {
                target.AddBuff((BuffID.Frostburn), 200, false);
            }
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
        }
    }
}
