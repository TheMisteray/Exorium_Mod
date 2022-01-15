using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Weapons.Magic
{
    class GlitteringGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Or was it the glimmering gun...");
        }

        public override void SetDefaults()
        {
            item.damage = 12;
            item.magic = true;
            item.mana = 4;
            item.width = 40;
            item.height = 40;
            item.useTime = 9;
            item.useAnimation = 9;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 4;
            item.UseSound = SoundID.Item9;
            item.autoReuse = true;
            item.shoot = ProjectileType<Projectiles.SparkleShot>();
            item.shootSpeed = 16f;
            item.rare = 2;
            item.value = Item.sellPrice(gold: 3, silver: 50);
            item.scale = 1.2f;
        }

        float rainbow = 0;

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(3));
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 75f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            int projectile = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
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
            recipe.AddIngredient(ItemID.Minishark);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
    }
}
