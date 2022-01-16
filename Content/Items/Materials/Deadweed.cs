using ExoriumMod.Core;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Items.Materials
{
    class Deadweed : ModItem
    {
        public override string Texture => AssetDirectory.Plant + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A withered plant");
        }

        public override void SetDefaults()
        {
            item.value = 200;
            item.width = 12;
            item.height = 16;
            item.rare = 1;
            item.maxStack = 99;
        }
    }
}
