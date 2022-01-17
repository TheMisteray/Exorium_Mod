using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Weapons.Ranger
{
    class BlightGun : ModItem
    {
        public override string Texture => AssetDirectory.RangerWeapon + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scourge");
            Tooltip.SetDefault("Right click to fire a wild shotgun blast");
        }

        public override void SetDefaults()
        {
            item.damage = 23;
            item.ranged = true;
            item.width = 40;
            item.height = 20;
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 6;
            item.value = Item.sellPrice(silver: 68); ;
            item.rare = 2;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = 10;
            item.shootSpeed = 14f;
            item.useAmmo = AmmoID.Bullet;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useAnimation = 84;
                item.useTime = 84;
                item.UseSound = SoundID.Item36;
            }
            else
            {
                item.useAnimation = 24;
                item.useTime = 24;
                item.UseSound = SoundID.Item11;
            }
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                damage /= 2;
                int numberProjectiles = 8 + Main.rand.Next(4); // 8 to 11 shots
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(30)); // 32 degree spread.
                    float scale = (1.5f - Main.rand.NextFloat() * 1.2f);
                    perturbedSpeed = perturbedSpeed * scale; 
                    Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
                }
                return false; // return false because we don't want tmodloader to shoot projectile
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Materials.Metals.BlightsteelBar>(), 12);
            recipe.AddIngredient(ItemType<Materials.TaintedGel>(), 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
