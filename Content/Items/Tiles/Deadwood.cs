using ExoriumMod.Core;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Tiles
{
    class Deadwood : ModItem
    {
        public override string Texture => AssetDirectory.TileItem + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Light and porous wood");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 22;
            item.rare = 0;
            item.maxStack = 999;
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 15;
            item.autoReuse = true;
            item.createTile = TileType<Tiles.Deadwood>();
        }
    }
}
