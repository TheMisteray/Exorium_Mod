using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Buffs;
using ExoriumMod.Projectiles;

namespace ExoriumMod.Items.Weapons.Magic.Scrolls
{
    class ScrollOfCloudOfDaggers : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Casts Cloud of Daggers");
            DisplayName.SetDefault("Spell Scroll: Cloud of Daggers");
        }

        public override void SetDefaults()
        {
            item.damage = 15;
            item.useTime = 40;
            item.useAnimation = 40;
            item.rare = 2;
            item.useStyle = 4;
            item.value = Item.buyPrice(gold: 3, silver: 50);
            item.width = 32;
            item.height = 32;
            item.magic = true;
            item.mana = 20;
            item.maxStack = 30;
            item.noMelee = true;
            item.shootSpeed = 14f;
            item.autoReuse = false;
            item.shoot = ProjectileType<DaggerCloud>();
            item.consumable = true;
            item.UseSound = SoundID.Item7;
            item.noUseGraphic = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.HasBuff(BuffType<ScrollCooldown>());
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return true;
        }

        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(BuffType<ScrollCooldown>(), 5400);
        }
    }
}
