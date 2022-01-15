using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Accessories
{
    class CoreOfBlight : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Core of Blight");
            Tooltip.SetDefault("Reduced regeneration" +
                "\nIncreased damage when regeneration is negative" +
                "\nDecreased damage when regeneration is positive");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 8));
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 36;
            item.accessory = true;
            item.value = 20000;
            item.rare = -12;
            item.expert = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.lifeRegen -= 2;
            if (player.lifeRegen <=0)
            {
                player.allDamage += -0.03f * player.lifeRegen;
            }
            if (player.lifeRegen > 0)
            {
                player.allDamage += -0.01f * player.lifeRegen;
            }
        }
    }
}
