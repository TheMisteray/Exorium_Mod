using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

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
            item.damage = 42;
            item.width = 50;
            item.height = 36;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.useAnimation = 15;
            item.useTime = 15;
            item.useTurn = true;
            item.UseSound = SoundID.Item1;
            item.noUseGraphic = true;
            item.maxStack = 1;
            item.consumable = true;
            item.rare = -1;
            item.value = 0;
            item.noMelee = true;
            item.shoot = 1;
        }

        public override void OnConsumeItem(Player player)
        {
            int d = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileID.Dynamite, 1000, 20, player.whoAmI);
            Main.projectile[d].hostile = true;
            Main.projectile[d].timeLeft = 2;
            Vector2 throwUp = new Vector2(0, -10);
            int bombs = Main.rand.Next(3, 6);
            for (int i = 0; i < bombs; i++)
            {
                Vector2 perturbedThrow = throwUp.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-30, 30)));
                Projectile.NewProjectile(player.Center, perturbedThrow, ProjectileID.MolotovCocktail, item.damage, item.knockBack, player.whoAmI);
            }
            base.OnConsumeItem(player);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.WoodBreastplate);
            recipe.AddIngredient(ItemID.MolotovCocktail, 3);
            recipe.AddIngredient(ItemID.RopeCoil);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return false;
        }
    }
}
