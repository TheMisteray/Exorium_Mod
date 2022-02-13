using ExoriumMod.Core;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Materials.Metals
{
    class DuneStone : ModItem
    {
        public override string Texture => AssetDirectory.Metal + Name;

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 14;
            item.rare = 0;
            item.value = 100;
            item.maxStack = 999;
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 15;
            item.autoReuse = true;
            item.consumable = true;
            item.createTile = TileType<Tiles.DuneStoneTile>();
        }
    }
}
