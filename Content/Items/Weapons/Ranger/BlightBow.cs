using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Weapons.Ranger
{
    public class BlightBow : ModItem
    {
        public override string Texture => AssetDirectory.RangerWeapon + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blightsteel Bow");
        }

        public override void SetDefaults()
        {
            Item.damage = 22;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 22;
            Item.height = 40;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useAmmo = AmmoID.Arrow;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(silver: 36);
            Item.rare = 1;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = 10;
            Item.noMelee = true;
            Item.shootSpeed = 28;
            Item.useStyle = 5;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.BlightsteelBar>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
