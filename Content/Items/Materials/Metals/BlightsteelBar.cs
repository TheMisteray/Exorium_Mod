using ExoriumMod.Core;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria;

namespace ExoriumMod.Content.Items.Materials.Metals
{
    public class BlightsteelBar : ModItem
    {
        public override string Texture => AssetDirectory.Metal + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blightsteel Bar"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("It bites at your skin");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.value = 4200;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = 2;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = TileType<Tiles.BlightsteelBarTile>();
            Item.placeStyle = 0;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<BlightedOre>(), 4);
            recipe.AddTile(TileID.Furnaces);
            recipe.Register();
        }
    }

}
