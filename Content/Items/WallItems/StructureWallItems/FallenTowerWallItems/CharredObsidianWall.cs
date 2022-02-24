using ExoriumMod.Core;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.WallItems.StructureWallItems.FallenTowerWallItems
{
    class CharredObsidianWall : ModItem
    {
        public override string Texture => AssetDirectory.WallItem + Name;

        public override void SetDefaults()
        {
            item.width = 12;
            item.height = 12;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 7;
            item.useStyle = 1;
            item.consumable = true;
            item.createWall = WallType<Walls.StructureWalls.FallenTowerWalls.CharredObsidianWall>();
        }
    }
}
