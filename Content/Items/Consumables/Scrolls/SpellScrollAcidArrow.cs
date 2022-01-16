using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Weapons.Magic.Scrolls
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
            item.width = 32;
            item.height = 32;
            item.useAnimation = 20;
            item.useTime = 20;
            item.useStyle = 4;
            item.UseSound = SoundID.Item4;
            item.maxStack = 30;
            item.mana = 40;
            item.consumable = true;
            item.rare = 2;
            item.value = Item.buyPrice(gold: 1);
            item.shoot = 10;
            item.noUseGraphic = true;
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

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return false;
        }
    }
}
