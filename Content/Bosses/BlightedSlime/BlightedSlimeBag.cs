using Terraria;
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
            Item.maxStack = 30;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = -12;
            Item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemType<Items.Materials.TaintedGel>(), Main.rand.Next(50, 66));
            player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemType<Items.Materials.Metals.BlightedOre>(), Main.rand.Next(80, 121));
            player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemType<Items.Accessories.CoreOfBlight>());
        }

        public override int BossBagNPC => NPCType<BlightedSlime>();
    }
}
