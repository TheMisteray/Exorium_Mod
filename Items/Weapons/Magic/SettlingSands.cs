using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Weapons.Magic
{
    class SettlingSands : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Launches bouncing sand balls");
        }

        public override void SetDefaults()
        {
            item.damage = 18;
            item.width = 15;
            item.height = 15;
            item.magic = true;
            item.mana = 8;
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = Item.sellPrice(silver: 14);
            item.rare = 2;
            item.UseSound = SoundID.Item42;
            item.shoot = ProjectileType<Projectiles.SandShot>();
            item.shootSpeed = 7;
            item.autoReuse = true;
            item.scale = 0.9f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("DunestoneBar"), 8);
            recipe.AddTile(TileID.Bookcases);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
