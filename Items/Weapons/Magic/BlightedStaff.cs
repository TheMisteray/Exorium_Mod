using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Weapons.Magic
{
    class BlightedStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Withered Staff");
            Item.staff[item.type] = true;
            Tooltip.SetDefault("Bursts into blight specks on impact");
        }

        public override void SetDefaults()
        {
            item.damage = 16;
            item.magic = true;
            item.mana = 10;
            item.width = 40;
            item.height = 40;
            item.useTime = 21;
            item.useAnimation = 21;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 4;
            item.UseSound = SoundID.Item20;
            item.autoReuse = true;
            item.shoot = ProjectileType<Projectiles.BlightShot>();
            item.shootSpeed = 16f;
            item.rare = 1;
            item.value = Item.sellPrice(silver: 42);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("BlightsteelBar"), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
