using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Tiles
{
    public class DuneStone : ModTile
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = AssetDirectory.Tile + Name;
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileValue[Type] = 260; //above Silver
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("DuneStone");
            AddMapEntry(new Color(196, 188, 22), name);

            drop = ItemType<Items.Materials.Metals.DuneStone>();
            soundType = 21;
            soundStyle = 1;
            mineResist = 1f;
            minPick = 25;

            dustType = 1;
            Main.dust[dustType].color = new Color(196, 188, 22);
        }

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
        {
            if (Main.rand.NextBool(40))
            {
                int dust = Dust.NewDust(new Vector2(i * 16 + 4, j * 16 + 2), 4, 4, 32, 0f, 0f, 100, default(Color), 1f);
            }
        }
    }
}
