using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using ExoriumMod.Dusts;
using Terraria.ID;
using Terraria.ObjectData;

namespace ExoriumMod.Tiles
{
    class AshenDust : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            drop = ItemType<Items.Placeables.AshenDust>();
            AddMapEntry(new Color(90, 90, 90));
            SetModTree(new DeadwoodTree());
            dustType = DustType<DeadDust>();
        }

        public override int SaplingGrowthType(ref int style)
        {
            style = 0;
            return TileType<DeadwoodSapling>();
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
        {
            if (Main.rand.NextBool(700))
            {
                Dust.NewDust(new Vector2(i, j).ToWorldCoordinates(), 32, 48, DustType<DeadDust>(), 0f, 0f, 100, default(Color), 1f);
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
    }
}
