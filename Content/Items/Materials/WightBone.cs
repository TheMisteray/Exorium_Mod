using ExoriumMod.Core;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Items.Materials
{
    class WightBone : ModItem
    {
        public override string Texture => AssetDirectory.Materials + Name;

        public override void SetDefaults()
        {
            Item.value = 2;
            Item.width = 16;
            Item.height = 14;
            Item.rare = 0;
            Item.maxStack = 999;
        }
    }
}
