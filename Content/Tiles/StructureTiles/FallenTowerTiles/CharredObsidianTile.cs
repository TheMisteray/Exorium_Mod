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
        public override string Texture => AssetDirectory.Tile + Name;

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMerge[TileID.Ash][this.Type] = true;
            Main.tileBlockLight[Type] = true;
            ItemDrop = ItemType<Items.TileItems.StructureTileItems.FallenTowerTileItems.CharredObsidian>();
            AddMapEntry(new Color(51, 12, 5));
            DustType = DustID.Obsidian;
            HitSound = SoundID.Tink;
            MineResist = 5f;
            MinPick = 210;
            Main.dust[DustType].color = new Color(51, 12, 5);
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}
