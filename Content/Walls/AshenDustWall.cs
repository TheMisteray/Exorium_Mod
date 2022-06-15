using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Walls
{
    class AshenDustWall : ModWall
    {
        public override string Texture => AssetDirectory.Wall + Name;

        public override void SetStaticDefaults()
        {
            AddMapEntry(new Color(110, 110, 110));
        }
    }
}
