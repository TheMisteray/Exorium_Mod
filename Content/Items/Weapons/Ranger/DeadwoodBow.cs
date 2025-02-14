using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Weapons.Ranger
{
    class DeadwoodBow : ModItem
    {
        public override string Texture => AssetDirectory.RangerWeapon + Name;

        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Incredibly light");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 2;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 22;
            Item.height = 40;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useAmmo = AmmoID.Arrow;
            Item.knockBack = 0;
            Item.value = 20;
            Item.rare = 0;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = 10;
            Item.noMelee = true;
            Item.shootSpeed = 28;
            Item.useStyle = 5;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(25));
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<TileItems.Deadwood>(), 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
