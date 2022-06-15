using ExoriumMod.Core;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.TileItems.StructureTileItems.ShadowmancerTileItems
{
    class Crate : ModItem
    {
        public override string Texture => AssetDirectory.TileItem + Name;

        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 30;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.value = 500;
            Item.scale = .8f;
            Item.createTile = TileType<Tiles.CrateTile>();
        }
    }
}
