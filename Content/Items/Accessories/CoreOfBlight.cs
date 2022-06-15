using ExoriumMod.Core;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Items.Accessories
{
    class CoreOfBlight : ModItem
    {
        public override string Texture => AssetDirectory.Accessory + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Core of Blight");
            Tooltip.SetDefault("Reduced regeneration" +
                "\nIncreased damage when regeneration is negative" +
                "\nDecreased damage when regeneration is positive");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 8));
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 36;
            Item.accessory = true;
            Item.value = 20000;
            Item.rare = -12;
            Item.expert = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.lifeRegen -= 2;
            if (player.lifeRegen <=0)
            {
                player.GetDamage(DamageClass.Generic) += -0.03f * player.lifeRegen;
            }
            if (player.lifeRegen > 0)
            {
                player.GetDamage(DamageClass.Generic) += -0.01f * player.lifeRegen;
            }
        }
    }
}
