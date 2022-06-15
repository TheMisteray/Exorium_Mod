using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Walls
{
    class DeadwoodWall : ModWall
    {
        public override string Texture => AssetDirectory.Wall + Name;

        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            ItemDrop = ItemType<Items.WallItems.DeadwoodWall>();
            AddMapEntry(new Color(140, 140, 140));
        }
    }
}
