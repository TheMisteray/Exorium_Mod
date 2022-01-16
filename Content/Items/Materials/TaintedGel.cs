using ExoriumMod.Core;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Items.Materials
{
    class TaintedGel : ModItem
    {
        public override string Texture => AssetDirectory.Materials + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The ooze of deadlands");
        }

        public override void SetDefaults()
        {
            item.value = 1600;
            item.width = 16;
            item.height = 14;
            item.rare = 3;
            item.maxStack = 999;
        }
    }
}
