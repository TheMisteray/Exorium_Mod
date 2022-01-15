using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Weapons.Melee
{
    public class MorditeBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots a homing skull");
        }

        public override void SetDefaults()
        {
            item.damage = 38;
            item.melee = true;
            item.width = 48;
            item.height = 48;
            item.useTime = 32;
            item.useAnimation = 32;
            item.useStyle = 1;
            item.knockBack = 7;
            item.value = Item.sellPrice(gold: 3, silver: 50); ;
            item.rare = 3;
            item.UseSound = SoundID.Item8;
            item.autoReuse = true;
            item.scale = 1.3f;
            item.shoot = ProjectileType<Projectiles.MorditeSkull>();
            item.shootSpeed = 12f;
            item.useTurn = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("MorditeBar"), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20)); //degree spread.
            // Stagger difference
            float scale = 1f - (Main.rand.NextFloat() * .3f);
            perturbedSpeed = perturbedSpeed * scale;
            int projectile = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            Main.projectile[projectile].magic = true;
            return false; // return false because projectiles were already fired
        }
    }
}

