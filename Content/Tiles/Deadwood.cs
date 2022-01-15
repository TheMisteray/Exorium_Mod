using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Tiles
{
    class Deadwood : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            drop = ItemType<Items.Placeables.Deadwood>();
            AddMapEntry(new Color(40, 40, 40));
            dustType = 1;
            Main.dust[dustType].color = new Color(40, 40, 40);
        }
    }
}
