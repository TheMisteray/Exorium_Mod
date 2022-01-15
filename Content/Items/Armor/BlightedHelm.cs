using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class BlightedHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blightsteel Crown");
            Tooltip.SetDefault("2% decreased movement speed"
                + "\n2% increased damage"
                + "\nDecreased regeneration");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.value = 15000;
            item.rare = 2;
            item.defense = 7;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<BlightedChestplate>() && legs.type == ItemType<BlightedLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "3 defense"
                + "\n7% increased damage";
            player.allDamage += 0.07f;
            player.statDefense += 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed -= 0.02f;
            player.allDamage += 0.02f;
            player.lifeRegen -= 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("BlightsteelBar"), 15);
            recipe.AddIngredient(mod.GetItem("TaintedGel"), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
