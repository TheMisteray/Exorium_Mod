using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Placeables
{
    class DeadwoodWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Incredibly light");
        }

        public override void SetDefaults()
        {
            item.width = 12;
            item.height = 12;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 7;
            item.useTime = 4;
            item.useStyle = 1;
            item.consumable = true;
            item.createWall = WallType<Walls.DeadwoodWall>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Deadwood>());
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 4);
            recipe.AddRecipe();
        }
    }
}
