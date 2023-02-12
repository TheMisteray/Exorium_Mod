using ExoriumMod.Core;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Items.Misc
{
    class DarkKey : ModItem
    {
        public override string Texture => AssetDirectory.Misc + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Unlocks dark chests");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 20;
            Item.maxStack = 99;
        }
    }
}
