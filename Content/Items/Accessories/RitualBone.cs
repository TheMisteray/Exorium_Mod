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
            Item.value = 2;
            Item.width = 24;
            Item.height = 24;
            Item.rare = 2;
            Item.maxStack = 1;
            Item.accessory = true;
            Item.value = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ExoriumPlayer>().ritualArrow = true;
        }
    }
}
