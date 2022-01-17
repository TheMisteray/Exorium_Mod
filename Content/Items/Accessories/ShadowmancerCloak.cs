using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ExoriumMod.Content.Items.Accessories
{
    class ShadowmancerCloak : ModItem
    {
        public override string Texture => AssetDirectory.Accessory + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Veils you in shadow" +
                "\nThe cloak has 40 health which can regenerate over time" +
                "\nIt grants 6 defense, increased regeneration, and immunity to fire and frostburn while it has health remaining" +
                "\nWhenever you take damage the cloak also takes that much damage" +
                "\nIf the cloak is destroyed it must regenerate fully before its effects return");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 28;
            item.value = 25000;
            item.accessory = true;
            item.rare = -12;
            item.expert = true;
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
