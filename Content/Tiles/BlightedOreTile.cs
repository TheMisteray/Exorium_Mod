using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Tiles
{
    public class BlightedOreTile : ModTile
    {
        public override string Texture => AssetDirectory.Tile + Name;

        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileOreFinderPriority[Type] = 420; //above Gold and Platinum values for detection
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            DustType = 1;
            Main.dust[DustType].color = new Color(54, 54, 42);

            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Blightsteel Ore");
            AddMapEntry(new Color(75,75,0), name);

            HitSound = SoundID.Tink;
            MineResist = 3f;
            MinPick = 65;
        }
    }
}
