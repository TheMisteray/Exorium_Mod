using ExoriumMod.Core;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.TileItems.StructureTileItems.ShadowmancerTileItems
{
    class DarkChest : ModItem
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
            Item.createTile = TileType<Tiles.DarkChestTile>();
        }
    }
}
