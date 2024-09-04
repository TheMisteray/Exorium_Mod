using ExoriumMod.Content.Tiles;
using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Walls.StructureWalls.FallenTowerWalls
{
    class CharredObsidianWall_LavaDrain : ModWall
    {
        public override string Texture => AssetDirectory.Wall + "CharredObsidianWall";

        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            AddMapEntry(new Color(25, 6, 2));
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void RandomUpdate(int i, int j)
        {
            if (Main.tile[i, j].CheckingLiquid)
            {
                Main.tile[i, j].LiquidAmount = 0;
            }
        }
    }
}
