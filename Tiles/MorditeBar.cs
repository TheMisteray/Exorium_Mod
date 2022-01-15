using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace ExoriumMod.Tiles
{
    class MorditeBar : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(0, 0, 0), Language.GetText("MapObject.MetalBar")); // localized text for "Metal Bar"
        }

        public override bool Drop(int i, int j)
        {
            Tile t = Main.tile[i, j];
            Item.NewItem(i * 16, j * 16, 16, 16, ItemType<Items.Placeables.MorditeBar>());
            return base.Drop(i, j);
        }
    }
}
