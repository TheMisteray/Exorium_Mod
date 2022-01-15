using Terraria.ModLoader;

namespace ExoriumMod.Items.Consumables
{
    class DarkKey : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Unlocks dark chests");
        }

        public override void SetDefaults()
        {
            //item.CloneDefaults(ItemID.GoldenKey);
            item.width = 14;
            item.height = 20;
            item.maxStack = 99;
        }
    }
}
