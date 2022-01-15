using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Placeables
{
    class DeadwoodWorkbench : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 14;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 10;
            item.useTime = 5;
            item.useStyle = 1;
            item.consumable = true;
            item.value = 30;
            item.createTile = TileType<Tiles.DeadwoodWorkbench>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Deadwood>(), 10);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
