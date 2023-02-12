using ExoriumMod.Core;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.TileItems
{
    class DeadwoodPlatform : ModItem
    {
        public override string Texture => AssetDirectory.TileItem + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Incredibly light");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 200;
        }

        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 10;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 10;
            Item.useTime = 5;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.createTile = TileType<Tiles.DeadwoodPlatformTile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(2);
            recipe.AddIngredient(ItemType<Deadwood>());
            recipe.Register();
        }
    }
}
