using ExoriumMod.Core;
using Terraria;
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
            item.value = 2;
            item.width = 12;
            item.height = 12;
            item.rare = 1;
            item.maxStack = 999;
            item.damage = 5;
            item.consumable = true;
            item.shoot = ProjectileType<RimeBullet>();
            item.shootSpeed = 14;
            item.ammo = AmmoID.Bullet;
            item.ranged = true;
            item.crit = 4;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Materials.Metals.RimestoneBar>());
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 70);
            recipe.AddRecipe();
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
            projectile.width = 8;
            projectile.height = 8;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 2600;
            projectile.alpha = 0;
            projectile.light = 0.2f;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
            aiType = ProjectileID.Bullet;
            projectile.velocity.X *= 1.5f;
            projectile.velocity.Y *= 1.5f;
        }

        public override void AI()
        {
            if (Main.rand.Next(10) == 1)
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 67, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
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
            Main.PlaySound(SoundID.Dig, projectile.position);
        }
    }
}
