using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Tiles.StructureTiles.FallenTowerTiles
{
    class CharredObsidianTile : ModTile
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = AssetDirectory.Tile + name;
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMerge[TileID.Ash][this.Type] = true;
            Main.tileBlockLight[Type] = true;
            drop = ItemType<Items.TileItems.StructureTileItems.FallenTowerTileItems.CharredObsidian>();
            AddMapEntry(new Color(51, 12, 5));
            dustType = DustID.Obsidian;
            soundType = 21;
            soundStyle = 1;
            mineResist = 2f;
            minPick = 210;
            Main.dust[dustType].color = new Color(51, 12, 5);
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}
