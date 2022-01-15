using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Weapons.Ranger
{
    class LustrousBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Converts arrows into beams of colored light" +
                "\n Beams redirect towrds your cursor");
        }

        public override void SetDefaults()
        {
            item.damage = 18;
            item.ranged = true;
            item.width = 22;
            item.height = 40;
            item.useTime = 26;
            item.useAnimation = 26;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 2;
            item.value = Item.sellPrice(gold: 3, silver: 50); ;
            item.rare = 2;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = 10;
            item.noMelee = true;
            item.shootSpeed = 30;
            item.useStyle = 5;
        }

        private int rainbow;
        private bool side;

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            side = !side;
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(Utils.Clamp(Main.rand.NextFloat(40), 15, 25) * ((side) ? 1 : -1)));
            int projectile = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<Projectiles.LustrousBeam>(), damage, knockBack, player.whoAmI, player.position.X, player.position.Y);
            Main.projectile[projectile].localAI[1] = rainbow;
            rainbow++;
            if (rainbow == 7)
                rainbow = 0;
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
            recipe.AddIngredient(ItemID.PlatinumBar, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe altRecipe = new ModRecipe(mod);
            altRecipe.AddIngredient(ItemID.Amethyst);
            altRecipe.AddIngredient(ItemID.Topaz);
            altRecipe.AddIngredient(ItemID.Sapphire);
            altRecipe.AddIngredient(ItemID.Emerald);
            altRecipe.AddIngredient(ItemID.Ruby);
            altRecipe.AddIngredient(ItemID.Diamond);
            altRecipe.AddIngredient(ItemID.Amber);
            altRecipe.AddIngredient(ItemID.GoldBar, 20);
            altRecipe.AddTile(TileID.Anvils);
            altRecipe.SetResult(this);
            altRecipe.AddRecipe();
        }
    }
}
