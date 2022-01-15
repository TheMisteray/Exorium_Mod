using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Projectiles;
using Microsoft.Xna.Framework;

namespace ExoriumMod.Items.Weapons.Melee
{
    class BlightedPike : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mildew");
            Tooltip.SetDefault("Launches specks of blight");
        }

        public override void SetDefaults()
        {
            item.damage = 22;
            item.melee = true;
            item.width = 80;
            item.height = 80;
            item.useTime = 36;
            item.shootSpeed = 3.7f;
            item.useAnimation = 20;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = Item.sellPrice(silver: 68);
            item.rare = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.shoot = ProjectileType<Projectiles.BlightedPikeProj>();
        }


        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[item.shoot] < 1;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX *3, speedY *3, mod.ProjectileType("BlightHail"), damage, knockBack, player.whoAmI); //fires additional projectile
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("BlightsteelBar"), 12);
            recipe.AddIngredient(mod.GetItem("TaintedGel"), 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
