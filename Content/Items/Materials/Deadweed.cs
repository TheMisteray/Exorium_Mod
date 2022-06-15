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
            Item.value = 200;
            Item.width = 12;
            Item.height = 16;
            Item.rare = 1;
            Item.maxStack = 99;
        }
    }
}
