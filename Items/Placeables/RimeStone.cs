using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ExoriumMod.Tiles;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Placeables
{
    class RimeStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.rare = 0;
            item.value = 100;
            item.maxStack = 999;
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 15;
            item.autoReuse = true;
            item.createTile = TileType<Tiles.RimeStone>();
        }
    }
}
