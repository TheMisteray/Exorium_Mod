﻿using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Bosses.BlightedSlime
{
    class BlightedSlimeBag : ModItem
    {
        public override string Texture => Core.AssetDirectory.BlightedSlime + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            item.maxStack = 30;
            item.consumable = true;
            item.width = 24;
            item.height = 24;
            item.rare = -12;
            item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            player.TryGettingDevArmor();
            player.QuickSpawnItem(ItemType<Items.Materials.TaintedGel>(), Main.rand.Next(50, 66));
            player.QuickSpawnItem(ItemType<Items.Materials.Metals.BlightedOre>(), Main.rand.Next(80, 121));
        }

        public override int BossBagNPC => NPCType<BlightedSlime>();
    }
}