using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Accessories
{
    class WightQuiver : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Reduced regeneration \n10% chance not to consume ammo\n6% increased arrow damage");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.value = 10000;
            item.accessory = true;
            item.rare = 2;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ExoriumPlayer>().wightQuiver = true;
            player.lifeRegen -= 1;
        }
    }
}
