using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Weapons.Summoner
{
    class SandWispStaff : ModItem
    {
        public override string Texture => AssetDirectory.SummonerWeapon + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons a small sand spirit to fight for you");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.knockBack = 0f;
            Item.mana = 8;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(0, 0, 14, 0);
            Item.rare = 1;
            Item.UseSound = SoundID.Item44;

            // These below are needed for a minion weapon
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.buffType = ModContent.BuffType<Buffs.Minions.SandWisp>();
            // No buffTime because otherwise the item tooltip would say something like "1 minute duration"
            Item.shoot = ModContent.ProjectileType<Projectiles.Minions.SandWisp>();
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);

            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);

            return false;
        }

        public override Nullable<bool> UseItem(Player player)/* tModPorter Suggestion: Return null instead of false *//* Suggestion: Return null instead of false */
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim(true);
            }
            return base.UseItem(player);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.DunestoneBar>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
