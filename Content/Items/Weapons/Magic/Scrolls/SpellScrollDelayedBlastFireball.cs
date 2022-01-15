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
    class SpellScrollDelayedBlastFireball : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Casts Delayed Blast Fireball");
            DisplayName.SetDefault("Spell Scroll: Delayed Blast Fireball");
        }

        public override void SetDefaults()
        {
            item.damage = 500;
            item.useStyle = 4;
            item.useTime = 42;
            item.useAnimation = 42;
            item.knockBack = 2;
            item.rare = 9;
            item.value = Item.buyPrice(gold: 18);
            item.width = 32;
            item.height = 32;
            item.maxStack = 30;
            item.magic = true;
            item.mana = 50;
            item.noMelee = true;
            item.shootSpeed = 16f;
            item.autoReuse = false;
            item.shoot = ProjectileType<Projectiles.DelayedBlastFireball>();
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
            player.AddBuff(BuffType<ScrollCooldown>(), 12600);
        }
    }
}
