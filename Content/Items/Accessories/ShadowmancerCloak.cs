using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Accessories
{
    class ShadowmancerCloak : ModItem
    {
        public override string Texture => AssetDirectory.Accessory + Name;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = 25000;
            Item.accessory = true;
            Item.rare = -12;
            Item.expert = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ExoriumPlayer>().shadowCloak = true;
            if (player.GetModPlayer<ExoriumPlayer>().cloakHP > 0 && !player.GetModPlayer<ExoriumPlayer>().deadCloak)
            {
                player.buffImmune[BuffID.OnFire] = true;
                player.buffImmune[BuffID.Frostburn] = true;
                player.statDefense += 6;
                player.lifeRegen += 1;
            }
            else if (player.GetModPlayer<ExoriumPlayer>().cloakHP < 0)
            {
                player.GetModPlayer<ExoriumPlayer>().deadCloak = true;
                player.GetModPlayer<ExoriumPlayer>().cloakHP = 0;
            }
            if (player.GetModPlayer<ExoriumPlayer>().cloakHP >= 40)
            {
                player.GetModPlayer<ExoriumPlayer>().deadCloak = false;
                player.GetModPlayer<ExoriumPlayer>().cloakHP = 40;
            }
        }
    }
}
