using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Ammo
{
    class RimeBullets : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Chance to inflict Frostburn");
            DisplayName.SetDefault("Rime Bullet");
        }

        public override void SetDefaults()
        {
            item.value = 2;
            item.width = 12;
            item.height = 12;
            item.rare = 1;
            item.maxStack = 999;
            item.damage = 5;
            item.consumable = true;
            item.shoot = mod.ProjectileType("RimeBullet");
            item.shootSpeed = 14;
            item.ammo = AmmoID.Bullet;
            item.ranged = true;
            item.crit = 4;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("RimestoneBar"));
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 70);
            recipe.AddRecipe();
        }
    }
}
