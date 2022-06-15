using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace ExoriumMod.Content.Tiles
{
    class DarkBrickTile : ModTile
    {
        public override string Texture => AssetDirectory.Tile + Name;

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            ItemDrop = ItemType<Items.TileItems.StructureTileItems.ShadowmancerTileItems.DarkBrick>();
            AddMapEntry(new Color(100, 100, 100));
            DustType = 54;
            HitSound = SoundID.Tink;
            MineResist = 2f;
            MinPick = 110;
            Main.dust[DustType].color = new Color(60, 60, 60);
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}
