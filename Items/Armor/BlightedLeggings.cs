using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    class BlightedLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blightsteel Leggings");
            Tooltip.SetDefault("3% decreased movement speed" +
                "\n2% increased damage"
                + "\nDecreased regeneration");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = 7500;
            item.rare = 2;
            item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed -= 0.03f;
            player.allDamage += 0.02f;
            player.lifeRegen -= 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("BlightsteelBar"), 20);
            recipe.AddIngredient(mod.GetItem("TaintedGel"), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
