using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Items.Accessories
{
    class RitualBone : ModItem
    {
        public override string Texture => AssetDirectory.Accessory + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("It seems that it is trying to lead you somewhere.");
        }

        public override void SetDefaults()
        {
            item.value = 2;
            item.width = 24;
            item.height = 24;
            item.rare = 2;
            item.maxStack = 1;
            item.accessory = true;
            item.value = 0;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ExoriumPlayer>().ritualArrow = true;
        }
    }
}
