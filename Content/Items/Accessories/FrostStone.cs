using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Accessories
{
    class FrostStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Your melee attacks inflict frostburn");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 28;
            item.value = 25000;
            item.accessory = true;
            item.rare = 3;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ExoriumPlayer>().frostStone = true;
        }
    }
}
