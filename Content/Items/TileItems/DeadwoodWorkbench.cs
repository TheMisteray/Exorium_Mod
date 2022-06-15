using ExoriumMod.Core;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria;

namespace ExoriumMod.Content.Items.TileItems
{
    class DeadwoodWorkbench : ModItem
    {
        public override string Texture => AssetDirectory.TileItem + Name;

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 14;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 10;
            Item.useTime = 5;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.value = 30;
            Item.createTile = TileType<Tiles.DeadwoodWorkbenchTile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Deadwood>(), 10);
            recipe.Register();
        }
    }
}
