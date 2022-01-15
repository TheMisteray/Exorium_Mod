using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Walls
{
    class DeadwoodWall : ModWall
    {
        public override void SetDefaults()
        {
            Main.wallHouse[Type] = true;
            drop = ItemType<Items.Placeables.DeadwoodWall>();
            AddMapEntry(new Color(140, 140, 140));
        }
    }
}
