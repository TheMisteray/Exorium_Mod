using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Tiles;

namespace ExoriumMod.Items.Placeables
{
    class DuneStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            
        }

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
            item.createTile = TileType<Tiles.DuneStone>();
        }
    }
}
