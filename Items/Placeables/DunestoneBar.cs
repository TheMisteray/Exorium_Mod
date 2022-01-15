using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Placeables
{
    class DunestoneBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A hard stone with a sandy feel");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 24;
            item.rare = 0;
            item.value = 400;
            item.maxStack = 99;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.consumable = true;
            item.createTile = TileType<Tiles.DunestoneBar>();
            item.placeStyle = 0;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("DuneStone"), 3);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
