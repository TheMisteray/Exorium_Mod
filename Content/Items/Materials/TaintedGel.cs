using ExoriumMod.Core;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Items.Materials
{
    class TaintedGel : ModItem
    {
        public override string Texture => AssetDirectory.Materials + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The ooze of deadlands");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.value = 1600;
            Item.width = 16;
            Item.height = 14;
            Item.rare = 3;
            Item.maxStack = 999;
        }
    }
}
