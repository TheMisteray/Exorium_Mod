using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Tiles
{
    class DeadwoodTile : ModTile
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
            drop = ItemType<Items.TileItems.Deadwood>();
            AddMapEntry(new Color(40, 40, 40));
            dustType = ModContent.DustType<Dusts.DeadwoodTreeDust>();
            Main.dust[dustType].color = new Color(40, 40, 40);
        }
    }
}
