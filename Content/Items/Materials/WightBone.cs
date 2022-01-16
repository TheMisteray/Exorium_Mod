using ExoriumMod.Core;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Items.Materials
{
    class WightBone : ModItem
    {
        public override string Texture => AssetDirectory.Materials + Name;

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
