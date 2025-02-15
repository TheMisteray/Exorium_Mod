using ExoriumMod.Core;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;

namespace ExoriumMod.Content.Items.Accessories
{
    class CoreOfBlight : ModItem
    {
        public override string Texture => AssetDirectory.Accessory + Name;

        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 8));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 36;
            Item.accessory = true;
            Item.value = 20000;
            Item.rare = -12;
            Item.expert = true;
            Item.lifeRegen = -2;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //player.lifeRegen -= 2;
            player.GetModPlayer<ExoriumPlayer>().blightCore = true;
        }
    }
}
