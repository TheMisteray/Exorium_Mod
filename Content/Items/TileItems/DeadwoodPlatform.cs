using ExoriumMod.Core;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.TileItems
{
    class DeadwoodPlatform : ModItem
    {
        public override string Texture => AssetDirectory.TileItem + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Incredibly light");
        }

        public override void SetDefaults()
        {
            item.width = 8;
            item.height = 10;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 10;
            item.useTime = 5;
            item.useStyle = 1;
            item.consumable = true;
            item.createTile = TileType<Tiles.DeadwoodPlatformTile>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Deadwood>());
            recipe.SetResult(this, 2);
            recipe.AddRecipe();
        }
    }
}
