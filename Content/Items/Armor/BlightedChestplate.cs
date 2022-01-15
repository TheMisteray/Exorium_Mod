using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    class BlightedChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blightsteel Breastplate");
            Tooltip.SetDefault("\n6% decreased movement speed" +
                "\n3% increased damage"
                + "\nDecreased regeneration");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = 9500;
            item.rare = 2;
            item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed -= 0.06f;
            player.allDamage += 0.03f;
            player.lifeRegen -= 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("BlightsteelBar"), 25);
            recipe.AddIngredient(mod.GetItem("TaintedGel"), 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
