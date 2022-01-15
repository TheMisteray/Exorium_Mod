using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Weapons.Magic
{
    class Pestilence : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tome of Pestilence");
            Tooltip.SetDefault("Shoots a trail of blight specks");
        }

        public override void SetDefaults()
        {
            item.damage = 17;
            item.width = 15;
            item.height = 15;
            item.magic = true;
            item.mana = 6;
            item.useTime = 12;
            item.useAnimation = 24;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = Item.sellPrice(silver: 68); ;
            item.rare = 2;
            item.UseSound = SoundID.Item42;
            item.shoot = ProjectileType<Projectiles.BlightHail>();
            item.shootSpeed = 20;
            item.autoReuse = true;
            item.scale = 0.9f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("BlightsteelBar"), 12);
            recipe.AddIngredient(mod.GetItem("TaintedGel"), 6);
            recipe.AddTile(TileID.Bookcases);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
