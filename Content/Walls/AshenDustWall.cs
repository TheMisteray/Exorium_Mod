using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Walls
{
    class AshenDustWall : ModWall
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = AssetDirectory.Wall + Name;
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            AddMapEntry(new Color(110, 110, 110));
        }
    }
}
