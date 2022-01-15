using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ExoriumMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    class MorditeBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("5% increased movement speed");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 22;
            item.value = 20000;
            item.rare = 3;
            item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.05f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("MorditeBar"), 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
