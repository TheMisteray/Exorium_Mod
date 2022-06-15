using ReLogic.Content;
using ExoriumMod.Core;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent;
using Terraria;

namespace ExoriumMod.Content.Tiles.Trees
{
    /*
    class DeadwoodTree : ModTree
    {
        private Mod mod => ModLoader.GetMod("ExoriumMod");

        public override void SetStaticDefaults()
        {
            GrowsOnTileId = new int[1] { TileType<AshenDustTile>() };
        }

        public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings
        {
            UseSpecialGroups = true,
            SpecialGroupMinimalHueValue = 11f / 72f,
            SpecialGroupMaximumHueValue = 0.25f,
            SpecialGroupMinimumSaturationValue = 0.88f,
            SpecialGroupMaximumSaturationValue = 1f
        };

        public override void SetTreeFoliageSettings(Tile tile, int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
        {
            //TODO
        }

        public override int GrowthFXGore()
        {
            return mod.Find<ModGore>(AssetDirectory.TreeGores + "DeadwoodTreeFX").Type;
        }

        public override int DropWood()
        {
            return ItemType<Items.TileItems.Deadwood>();
        }

        public override Asset<Texture2D> GetTexture()
        {
            return Request<Texture2D>("Assets/Tiles/Trees/DeadwoodTree" /*AssetDirectory.Tree + "DeadwoodTree"*//*;
        }

        public override Asset<Texture2D> GetTopTextures()
        {
            return Request<Texture2D>("Assets/Tiles/Trees/DeadwoodTree" + "_Tops");
        }

        public override Asset<Texture2D> GetBranchTextures()
        {
            return Request<Texture2D>("Assets/Tiles/Trees/DeadwoodTree" + "_Branches");
        }

        public override int CreateDust() => DustType<Dusts.DeadwoodTreeDust>();
    }
*/
}
