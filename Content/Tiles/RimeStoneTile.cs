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
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = AssetDirectory.Tile + name;
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
            Main.tileLighted[Type] = true;

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("RimeStone");
            AddMapEntry(new Color(194, 248, 255), name);

            drop = ItemType<Items.Materials.Metals.RimeStone>();
            soundType = 21;
            soundStyle = 1;
            mineResist = 1f;
            minPick = 25;

            dustType = 1;
            Main.dust[dustType].color = new Color(194, 248, 255);
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
