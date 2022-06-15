using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Weapons.Summoner
{
    class BlightedPinStaff : ModItem
    {
        public override string Texture => AssetDirectory.SummonerWeapon + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons a Blightsteel Needle to fight for you \n" +
                "Needles stick for 15 seconds after striking an enemy \n" +
                "If 4 or more needles are stuck to the same enemy, that enemy takes additional damage over time\n" +
                "Two needles occupy one minion slot");
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.knockBack = 0f;
            Item.mana = 11;
            Item.width = 38;
            Item.height = 38;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(0, 0, 36, 0);
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;

            // These below are needed for a minion weapon
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.buffType = ModContent.BuffType<Buffs.Minions.BlightedNeedleSummonBuff>();
            // No buffTime because otherwise the item tooltip would say something like "1 minute duration"
            Item.shoot = ModContent.ProjectileType<Projectiles.Minions.BlightedNeedleSummon>();
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
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);

            return false;
        }

        public override bool? UseItem(Player player)/* Suggestion: Return null instead of false */
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
            recipe.AddIngredient(ItemType<Materials.Metals.BlightsteelBar>(), 9);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
