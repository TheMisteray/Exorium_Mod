using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using ExoriumMod.Projectiles;

namespace ExoriumMod.Items.Weapons.Magic
{
    class ShadowBolt : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots shadowbolts that split apart on impact");
        }

        public override void SetDefaults()
        {
            item.damage = 10;
            item.width = 15;
            item.height = 15;
            item.magic = true;
            item.mana = 9;
            item.useTime = 41;
            item.useAnimation = 41;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = Item.sellPrice(silver: 20);
            item.rare = 1;
            item.UseSound = SoundID.Item20;
            item.shoot = ProjectileType<Projectiles.ShadowBoltSpell>();
            item.shootSpeed = 20;
            item.autoReuse = true;
            item.scale = 0.9f;
        }
    }
}
