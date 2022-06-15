using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Content.Tiles
{
    class AshenDustTile : ModTile
    {
        public override string Texture => AssetDirectory.Tile + Name;

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            ItemDrop = ItemType<Items.TileItems.AshenDust>();
            AddMapEntry(new Color(90, 90, 90));
            DustType = DustType<Dusts.DeadDust>();
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            if (Main.rand.NextBool(700))
            {
                Dust.NewDust(new Vector2(i, j).ToWorldCoordinates(), 32, 48, DustType<Dusts.DeadDust>(), 0f, 0f, 100, default(Color), 1f);
            }
        }

        public override bool HasWalkDust()
        {
            return true;
        }

        public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color)
        {
            Main.dust[dustType].alpha = 220;
        }

        public override void RandomUpdate(int i, int j)
        {
            if (!Main.tile[i, j - 1].HasTile && Main.rand.NextBool(1400))
                WorldGen.PlaceTile(i, j - 1, TileType<DeadweedTile>(), true, false);
        }
    }
}
