using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Walls
{
    class DarkBrickWall : ModWall
    {
        public override void SetDefaults()
        {
            Main.wallHouse[Type] = true;
            drop = ItemType<Items.Placeables.DarkBrickWall>();
            AddMapEntry(new Color(40, 40, 40));
        }
    }
}
