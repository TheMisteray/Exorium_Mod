using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class DeadwoodHelmet : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.rare = 0;
            item.defense = 1;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<DeadwoodBreastplate>() && legs.type == ItemType<DeadwoodGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "10% increased speed";
            player.moveSpeed += 0.1f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("Deadwood"), 20);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
