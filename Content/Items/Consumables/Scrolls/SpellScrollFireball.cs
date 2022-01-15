using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Buffs;

namespace ExoriumMod.Items.Weapons.Magic.Scrolls
{
    class SpellScrollFireball : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Casts Fireball");
            DisplayName.SetDefault("Spell Scroll: Fireball");
        }

        public override void SetDefaults()
        {
            item.damage = 120;
            item.useStyle = 4;
            item.useTime = 42;
            item.useAnimation = 42;
            item.knockBack = 2;
            item.rare = 3;
            item.value = Item.buyPrice(gold: 5);
            item.width = 32;
            item.height = 32;
            item.maxStack = 30;
            item.magic = true;
            item.mana = 50;
            item.noMelee = true;
            item.shootSpeed = 16f;
            item.autoReuse = false;
            item.shoot = ProjectileType<Projectiles.Fireball>();
            item.consumable = true;
            item.UseSound = SoundID.Item7;
            item.useTurn = true;
            item.noUseGraphic = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.HasBuff(BuffType<ScrollCooldown>());
        }

        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(BuffType<ScrollCooldown>(), 9000);
        }
    }
}
