﻿using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Tiles
{
    public class BlightedOreTile : ModTile
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
            Main.tileValue[Type] = 420; //above Gold and Platinum values for detection
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            dustType = 1;
            Main.dust[dustType].color = new Color(54, 54, 42);

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Blightsteel Ore");
            AddMapEntry(new Color(75,75,0), name);

            drop = ItemType<Items.Materials.Metals.BlightedOre>();
            soundType = 21;
            soundStyle = 1;
            mineResist = 3f;
            minPick = 65;
        }
    }
}