using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Placeables
{
    class Crate : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 48;
            item.height = 30;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.value = 500;
            item.scale = .8f;
            item.createTile = TileType<Tiles.Crate>();
        }
    }
}
