using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Weapons.Ranger
{
    class BlightGun : ModItem
    {
        public override string Texture => AssetDirectory.RangerWeapon + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scourge");
            Tooltip.SetDefault("Right click to fire a wild shotgun blast");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 23;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 20;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.value = Item.sellPrice(silver: 68); ;
            Item.rare = 2;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = 10;
            Item.shootSpeed = 14f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useAnimation = 84;
                Item.useTime = 84;
                Item.UseSound = SoundID.Item36;
            }
            else
            {
                Item.useAnimation = 24;
                Item.useTime = 24;
                Item.UseSound = SoundID.Item11;
            }
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                damage /= 2;
                int numberProjectiles = 8 + Main.rand.Next(4); // 8 to 11 shots
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(30)); // 32 degree spread.
                    float scale = (1.5f - Main.rand.NextFloat() * 1.2f);
                    perturbedSpeed = perturbedSpeed * scale; 
                    Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
                }
                return false; // return false because we don't want tmodloader to shoot projectile
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.BlightsteelBar>(), 12);
            recipe.AddIngredient(ItemType<Materials.TaintedGel>(), 6);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
