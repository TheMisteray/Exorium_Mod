using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Tiles
{
    public class RimeStoneTile : ModTile
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
            Main.tileLighted[Type] = true;

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("RimeStone");
            AddMapEntry(new Color(194, 248, 255), name);

            ItemDrop = ItemType<Items.Materials.Metals.RimeStone>();

            HitSound = SoundID.Tink;

            MineResist = 1f;
            MinPick = 25;

            DustType = 1;
            Main.dust[DustType].color = new Color(194, 248, 255);
        }

        

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Main.tile[i, j];
            r = 0.04f;
            g = 0.04f;
            b = 0.04f;
        }
    }
}
