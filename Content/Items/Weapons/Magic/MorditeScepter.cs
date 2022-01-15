using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace ExoriumMod.Items.Weapons.Magic
{
    class MorditeScepter : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;
            Tooltip.SetDefault("Shoot's bursts of homing skulls");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 40;
            item.rare = 3;
            item.value = Item.sellPrice(gold: 3, silver: 50);
            item.magic = true;
            item.mana = 25;
            item.useTime = 60;
            item.useAnimation = 60;
            item.useStyle = 5;
            item.noMelee = true;
            item.shootSpeed = 14f;
            item.autoReuse = true;
            item.damage = 38;
            item.shoot = ProjectileType<Projectiles.MorditeSkull>();
            item.UseSound = SoundID.Item20;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int numberProjectiles = 2 + Main.rand.Next(3); // 2 to 4 shots
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20)); //degree spread.
                // Stagger difference
                float scale = 1f - (Main.rand.NextFloat() * .3f);
                perturbedSpeed = perturbedSpeed * scale; 
                int projectile = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
                Main.projectile[projectile].magic = true;
            }
            return false; // return false because projectiles were already fired
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("MorditeBar"), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
