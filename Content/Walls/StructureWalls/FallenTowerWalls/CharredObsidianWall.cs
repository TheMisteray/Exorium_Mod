using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Walls.StructureWalls.FallenTowerWalls
{
    class CharredObsidianWall : ModWall
    {
        public override string Texture => AssetDirectory.Wall + Name;

        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            ItemDrop = ItemType<Items.WallItems.StructureWallItems.FallenTowerWallItems.CharredObsidianWall>();
            AddMapEntry(new Color(25, 6, 2));
        }
    }
}
