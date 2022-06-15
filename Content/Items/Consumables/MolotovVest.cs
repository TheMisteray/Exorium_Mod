using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;

namespace ExoriumMod.Content.Items.Consumables
{
    class MolotovVest : ModItem
    {
        public override string Texture => AssetDirectory.Consumable + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("\"A masterful invention\"\n" +
                "Don't use near any structures you care about");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.damage = 42;
            Item.width = 50;
            Item.height = 36;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.maxStack = 1;
            Item.consumable = true;
            Item.rare = -1;
            Item.value = 0;
            Item.noMelee = true;
            Item.shoot = 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int d = Projectile.NewProjectile(source, player.Center, Vector2.Zero, ProjectileID.Dynamite, 1000, 20, player.whoAmI);
            Main.projectile[d].hostile = true;
            Main.projectile[d].timeLeft = 2;
            Vector2 throwUp = new Vector2(0, -10);
            int bombs = Main.rand.Next(5, 8);
            for (int i = 0; i < bombs; i++)
            {
                Vector2 perturbedThrow = throwUp.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-30, 30)));
                Projectile.NewProjectile(source, player.Center, perturbedThrow, ProjectileID.MolotovCocktail, Item.damage, Item.knockBack, player.whoAmI);
            }
            base.OnConsumeItem(player);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.WoodBreastplate);
            recipe.AddIngredient(ItemID.MolotovCocktail, 3);
            recipe.AddIngredient(ItemID.RopeCoil);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
