using ExoriumMod.Core;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Tiles.Trees
{
    class DeadwoodTree : ModTree
    {
        private Mod mod => ModLoader.GetMod("ExoriumMod");

        public override int GrowthFXGore()
        {
            return mod.GetGoreSlot(AssetDirectory.TreeGores + "DeadwoodTreeFX");
        }

        public override int DropWood()
        {
            return ItemType<Items.TileItems.Deadwood>();
        }

        public override Texture2D GetTexture()
        {
            return mod.GetTexture(AssetDirectory.Tree + "DeadwoodTree");
        }

        public override Texture2D GetTopTextures(int i, int j, ref int frame, ref int frameWidth, ref int frameHeight, ref int xOffsetLeft, ref int yOffset)
        {
            return mod.GetTexture(AssetDirectory.Tree + "DeadwoodTree_Tops");
        }

        public override Texture2D GetBranchTextures(int i, int j, int trunkOffset, ref int frame)
        {
            return mod.GetTexture(AssetDirectory.Tree + "DeadwoodTree_Branches");
        }
    }
}
