using ExoriumMod.Core;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria;

namespace ExoriumMod.Content.Items.TileItems
{
    class DeadwoodChest : ModItem
    {
        public override string Texture => AssetDirectory.TileItem + Name;

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 22;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.value = 500;
            Item.createTile = TileType<Tiles.DeadwoodChestTile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Deadwood>(), 8);
            recipe.AddRecipeGroup("IronBar", 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
