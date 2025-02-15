using ReLogic.Content;
using ExoriumMod.Core;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent;
using Terraria;
using Microsoft.Xna.Framework;

namespace ExoriumMod.Content.Tiles.Trees
{
    class DeadwoodTree : ModTree
    {
        private Mod mod => ModLoader.GetMod("ExoriumMod");

        private Asset<Texture2D> texture;
        private Asset<Texture2D> branchesTexture;
        private Asset<Texture2D> topsTexture;

        public override void SetStaticDefaults()
        {
            GrowsOnTileId = new int[1] { TileType<AshenDustTile>() };
            texture = ModContent.Request<Texture2D>(AssetDirectory.Tree + "DeadwoodTree");
            branchesTexture = ModContent.Request<Texture2D>(AssetDirectory.Tree + "DeadwoodTree_Branches");
            topsTexture = ModContent.Request<Texture2D>(AssetDirectory.Tree + "DeadwoodTree_Tops");
        }

        public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings
        {
            UseSpecialGroups = true,
            SpecialGroupMinimalHueValue = 11f / 72f,
            SpecialGroupMaximumHueValue = 0.25f,
            SpecialGroupMinimumSaturationValue = 0.88f,
            SpecialGroupMaximumSaturationValue = 1f
        };

        // This is the primary texture for the trunk. Branches and foliage use different settings.
        public override Asset<Texture2D> GetTexture() => texture;

        // Branch Textures
        public override Asset<Texture2D> GetBranchTextures() => branchesTexture;

        // Top Textures
        public override Asset<Texture2D> GetTopTextures() => topsTexture;

        public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
        {
            //Needed for the abstract
        }

        public override bool Shake(int x, int y, ref bool createLeaves)
        {
            Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), new Vector2(x, y) * 16, ItemType<Items.TileItems.Deadwood>());
            return false;
        }

        public override int DropWood()
        {
            return ItemType<Items.TileItems.Deadwood>();
        }

        public override int CreateDust() => DustType<Dusts.DeadwoodTreeDust>();
    }
}
