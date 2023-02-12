using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Weapons.Summoner
{
    class ShadowOrb : ModItem
    {
        public override string Texture => AssetDirectory.Shadowmancer +  "ShadowOrb";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Produces a shadow on impact that will do your bidding" +
                "\nShadows have a small chance to inflict consuming dark");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 50;
        }

        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.knockBack = 0f;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(0, 0, 7, 0);
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;
            Item.maxStack = 999;
            Item.noMelee = true;
            Item.consumable = true;
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;

            // No buffTime because otherwise the item tooltip would say something like "1 minute duration"
            Item.shoot = ProjectileType<Projectiles.Minions.ShadowOrbSummon>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, Item.damage);
            return false;
        }
    }
}
