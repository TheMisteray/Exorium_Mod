using ExoriumMod.Core;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Items.Accessories
{
    class FrostStone : ModItem
    {
        public override string Texture => AssetDirectory.Accessory + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Your melee attacks inflict frostburn");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = 25000;
            Item.accessory = true;
            Item.rare = 3;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ExoriumPlayer>().frostStone = true;
        }
    }
}
