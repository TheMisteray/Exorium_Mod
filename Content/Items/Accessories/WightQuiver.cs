using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Items.Accessories
{
    class WightQuiver : ModItem
    {
        public override string Texture => AssetDirectory.Accessory + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Reduced regeneration \n10% chance not to consume ammo\n6% increased arrow damage");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = 10000;
            Item.accessory = true;
            Item.rare = 2;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ExoriumPlayer>().wightQuiver = true;
            player.lifeRegen -= 1;
        }
    }
}
