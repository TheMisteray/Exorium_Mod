using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Walls
{
    class DarkBrickWall : ModWall
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = AssetDirectory.Wall + name;
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            Main.wallHouse[Type] = true;
            drop = ItemType < Items.WallItems.DarkBrickWall>();
            AddMapEntry(new Color(40, 40, 40));
        }
    }
}
