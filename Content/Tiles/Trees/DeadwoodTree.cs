using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Dusts;

namespace ExoriumMod.Tiles
{
    class DeadwoodTree : ModTree
    {
        private Mod mod => ModLoader.GetMod("ExoriumMod");

        public override int GrowthFXGore()
        {
            return mod.GetGoreSlot("Gores/DeadwoodTreeFX");
        }

        public override int DropWood()
        {
            return ItemType<Items.Placeables.Deadwood>();
        }

        public override Texture2D GetTexture()
        {
            return mod.GetTexture("Tiles/DeadwoodTree");
        }

        public override Texture2D GetTopTextures(int i, int j, ref int frame, ref int frameWidth, ref int frameHeight, ref int xOffsetLeft, ref int yOffset)
        {
            return mod.GetTexture("Tiles/DeadwoodTree_Tops");
        }

        public override Texture2D GetBranchTextures(int i, int j, int trunkOffset, ref int frame)
        {
            return mod.GetTexture("Tiles/DeadwoodTree_Branches");
        }
    }
}
