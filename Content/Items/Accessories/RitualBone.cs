using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Accessories
{
    class RitualBone : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("It seems that it is trying to lead you somewhere.");
        }

        public override void SetDefaults()
        {
            item.value = 2;
            item.width = 24;
            item.height = 24;
            item.rare = 2;
            item.maxStack = 1;
            item.accessory = true;
            item.value = 0;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ExoriumPlayer>().ritualArrow = true;
        }
    }
}
