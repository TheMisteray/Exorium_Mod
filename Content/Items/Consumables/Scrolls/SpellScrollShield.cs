using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Consumables.Scrolls
{
    class SpellScrollShield : ModItem
    {
        public override string Texture => AssetDirectory.SpellScroll + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Casts Shield \n" +
                "Increases defense for a short time");
            DisplayName.SetDefault("Spell Scroll: Shield");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useAnimation = 20;
            Item.useTurn = true;
            Item.useTime = 20;
            Item.useStyle = 4;
            Item.UseSound = SoundID.Item4;
            Item.maxStack = 30;
            Item.mana = 20;
            Item.consumable = true;
            Item.rare = 1;
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
            player.AddBuff(BuffType<Buffs.Shield>(), 1600);
            player.AddBuff(BuffType<Buffs.ScrollCooldown>(), 5400);
        }

        //For some reason needed to have the item be consumed
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
    }
}
