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
    class BlightsteelBarTile : ModTile
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

            AddMapEntry(new Color(75, 75, 0), Language.GetText("MapObject.MetalBar")); // localized text for "Metal Bar"
        }
        
        //Holding on to this instance of drop just in case
        //public override bool Drop(int i, int j)/* tModPorter Note: Removed. Use CanDrop to decide if an item should drop. Use GetItemDrops to decide which item drops. Item drops based on placeStyle are handled automatically now, so this method might be able to be removed altogether. */
        //{
        //    Tile t = Main.tile[i, j];
        //    int style = t.TileFrameX / 18;
        //    if (style == 0) // It can be useful to share a single tile with multiple styles. This code will let you drop the appropriate bar if you had multiple.
        //    {
        //        Item.NewItem(new EntitySource_TileBreak(i, j), new Rectangle(i * 16, j * 16, 16, 16), ItemType<Items.Materials.Metals.BlightsteelBar>());
        //    }
        //    return base.Drop(i, j);
        //}
    }
}
