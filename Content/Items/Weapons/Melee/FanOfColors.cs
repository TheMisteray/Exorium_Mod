using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Weapons.Melee
{
    class FanOfColors : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fan of Colors");
            Tooltip.SetDefault("Throw a fan of colored knives");
        }

        public override void SetDefaults()
        {
            item.damage = 17;
            item.melee = true;
            item.noMelee = true;
            item.width = 32;
            item.height = 32;
            item.useTime = 76;
            item.useAnimation = 76;
            item.useStyle = 1;
            item.knockBack = 3;
            item.value = Item.sellPrice(gold: 3, silver: 75);
            item.rare = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = ProjectileType<Projectiles.ColorKnife>();
            item.shootSpeed = 48f;
            item.useTurn = true;
            item.noUseGraphic = true;
        }

        private bool mode = true;

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float numberProjectiles = 7;
            float rotation = MathHelper.ToRadians(15);
            position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
            for (int i = 0; i<numberProjectiles; i++)
            {
                if (mode)
                {
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-(rotation/2), 1.5f*rotation, i / (numberProjectiles - 1))) * .2f;
                    int projectile = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
                    Main.projectile[projectile].localAI[1] = i;
                    Main.projectile[projectile].localAI[0] = 5 * i;
                }
                else
                {
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp((rotation/2), -rotation*1.5f, i / (numberProjectiles - 1))) * .2f;
                    int projectile = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
                    Main.projectile[projectile].localAI[1] = i;
                    Main.projectile[projectile].localAI[0] = 5 * i;
                }
            }
            mode = !mode;
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Amethyst);
            recipe.AddIngredient(ItemID.Topaz);
            recipe.AddIngredient(ItemID.Sapphire);
            recipe.AddIngredient(ItemID.Emerald);
            recipe.AddIngredient(ItemID.Ruby);
            recipe.AddIngredient(ItemID.Diamond);
            recipe.AddIngredient(ItemID.Amber);
            recipe.AddIngredient(ItemID.ThrowingKnife, 100);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
