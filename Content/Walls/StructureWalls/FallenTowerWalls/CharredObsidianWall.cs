using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Walls.StructureWalls.FallenTowerWalls
{
    class CharredObsidianWall : ModWall
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = AssetDirectory.Wall + name;
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            Main.wallHouse[Type] = false;
            drop = ItemType<Items.WallItems.StructureWallItems.FallenTowerWallItems.CharredObsidianWall>();
            AddMapEntry(new Color(51, 12, 5));
        }
    }
}
