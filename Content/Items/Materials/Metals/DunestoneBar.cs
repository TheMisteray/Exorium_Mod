using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Materials.Metals
{
    class DunestoneBar : ModItem
    {
        public override string Texture => AssetDirectory.Metal + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A hard stone with a sandy feel");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.rare = 0;
            Item.value = 400;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = TileType<Tiles.DunestoneBarTile>();
            Item.placeStyle = 0;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<DuneStone>(), 3);
            recipe.AddTile(TileID.Furnaces);
            recipe.Register();
        }
    }
}
