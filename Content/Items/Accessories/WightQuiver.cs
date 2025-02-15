using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Accessories
{
    class WightQuiver : ModItem
    {
        public override string Texture => AssetDirectory.Accessory + Name;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = 10000;
            Item.accessory = true;
            Item.rare = 2;
            Item.lifeRegen = -1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ExoriumPlayer>().wightQuiver = true;
        }
    }
}
