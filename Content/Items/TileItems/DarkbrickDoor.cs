﻿using ExoriumMod.Core;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.TileItems
{
    class DarkbrickDoor : ModItem
    {
        public override string Texture => AssetDirectory.TileItem + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Becomes impassable in the presence of Shadowmancers");
        }

        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 46;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.value = 150;
            item.createTile = TileType<Tiles.DarkbrickDoorClosed>();
        }
    }
}