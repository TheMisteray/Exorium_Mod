using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExoriumMod.Items.Materials
{
    class WightBone : ModItem
    {
        public override void SetDefaults()
        {
            item.value = 2;
            item.width = 16;
            item.height = 14;
            item.rare = 0;
            item.maxStack = 999;
        }
    }
}
