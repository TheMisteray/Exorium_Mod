using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Consumables.Scrolls
{
    class SpellScrollAcidArrow : ModItem
    {
        public override string Texture => AssetDirectory.SpellScroll + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Casts Acid Arrows \n" +
                "Your wooden arrows become tipped with acid for a short time");
            DisplayName.SetDefault("Spell Scroll: Acid Arrows");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useStyle = 4;
            Item.UseSound = SoundID.Item4;
            Item.maxStack = 30;
            Item.mana = 40;
            Item.consumable = true;
            Item.rare = 2;
            Item.value = Item.buyPrice(gold: 1);
            Item.shoot = 10;
            Item.noUseGraphic = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.HasBuff(BuffType<Buffs.ScrollCooldown>());
        }

        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(BuffType<Buffs.AcidArrows>(), 1800);
            player.AddBuff(BuffType<Buffs.ScrollCooldown>(), 7200);
        }

        public override bool CanShoot(Player player)
        {
            return false;
        }
    }
}
