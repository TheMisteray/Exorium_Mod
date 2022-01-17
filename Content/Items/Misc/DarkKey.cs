using ExoriumMod.Core;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Items.Misc
{
    class DarkKey : ModItem
    {
        public override string Texture => AssetDirectory.Misc + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Unlocks dark chests");
        }

        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 20;
            item.maxStack = 99;
        }
    }
}
