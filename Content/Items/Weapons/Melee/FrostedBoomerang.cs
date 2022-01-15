using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Weapons.Melee
{
    class FrostedBoomerang : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frosted Chackram");
            Tooltip.SetDefault("Inflicts Frostburn");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.WoodenBoomerang);
            item.shoot = ProjectileType<Projectiles.RimeBoomerang>();
            item.damage = 15;
            item.useTime = 34;
            item.useAnimation = 34;
            item.autoReuse = true;
            item.rare = 1;
            item.value = Item.sellPrice(silver: 14);
            item.UseSound = SoundID.Item1;
            item.shootSpeed = 18;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("RimestoneBar"), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
}
