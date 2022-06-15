using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Tiles
{
    public class DuneStoneTile : ModTile
    {
        public override string Texture => AssetDirectory.Tile + Name;

        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileOreFinderPriority[Type] = 260; //above Silver
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("DuneStone");
            AddMapEntry(new Color(196, 188, 22), name);

            ItemDrop = ItemType<Items.Materials.Metals.DuneStone>();

            HitSound = SoundID.Tink;

            MineResist = 1f;
            MinPick = 25;

            DustType = 1;
            Main.dust[DustType].color = new Color(196, 188, 22);
        }

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            if (Main.rand.NextBool(40))
            {
                int dust = Dust.NewDust(new Vector2(i * 16 + 4, j * 16 + 2), 4, 4, 32, 0f, 0f, 100, default(Color), 1f);
            }
        }
    }
}
