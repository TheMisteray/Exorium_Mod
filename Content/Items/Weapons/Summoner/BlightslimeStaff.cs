using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Weapons.Summoner
{
    class BlightslimeStaff : ModItem
    {
        public override string Texture => AssetDirectory.SummonerWeapon + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons a tiny blight slime to fight for you");
        }

        public override void SetDefaults()
        {
            Item.damage = 26;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 20;
            Item.width = 26;
            Item.height = 34;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(0, 0, 68, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ProjectileType<Projectiles.Minions.BlightSlime>();
            Item.shootSpeed = 10f;
            Item.buffType = BuffType<Buffs.Minions.BlightSlime>(); //The buff added to player after used the item
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
            recipe.AddIngredient(ItemType<Materials.Metals.BlightsteelBar>(), 12);
            recipe.AddIngredient(ItemType<Materials.TaintedGel>(), 6);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }
    }
}
