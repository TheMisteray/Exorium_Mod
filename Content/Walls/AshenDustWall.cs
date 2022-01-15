using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Walls
{
    class AshenDustWall : ModWall
    {
        public override void SetDefaults()
        {
            AddMapEntry(new Color(110, 110, 110));
        }
    }
}
