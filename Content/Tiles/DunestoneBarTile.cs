using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.DataStructures;

namespace ExoriumMod.Content.Tiles
{
    class DunestoneBarTile : ModTile
    {
        public override string Texture => AssetDirectory.Tile + Name;

        public override void SetStaticDefaults()
        {
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileSolid[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            HitSound = SoundID.Tink;

            AddMapEntry(new Color(196, 188, 22), Language.GetText("MapObject.MetalBar")); // localized text for "Metal Bar"
        }

        public override bool Drop(int i, int j)
        {
            Tile t = Main.tile[i, j];
            int style = t.TileFrameX / 18;
            if (style == 0) // It can be useful to share a single tile with multiple styles. This code will let you drop the appropriate bar if you had multiple.
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ItemType<Items.Materials.Metals.DunestoneBar>());
            }
            return base.Drop(i, j);
        }
    }
}
