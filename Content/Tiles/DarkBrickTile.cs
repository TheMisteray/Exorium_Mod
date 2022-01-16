using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Tiles
{
    class DarkBrickTile : ModTile
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = AssetDirectory.Tile + name;
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            drop = ItemType<Items.TileItems.DarkBrick>();
            AddMapEntry(new Color(100, 100, 100));
            dustType = 54;
            soundType = 21;
            soundStyle = 1;
            mineResist = 2f;
            minPick = 110;
            dustType = 1;
            Main.dust[dustType].color = new Color(60, 60, 60);
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}
