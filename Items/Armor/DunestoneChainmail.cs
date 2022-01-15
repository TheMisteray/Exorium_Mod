using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    class DunestoneChainmail : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("4% increased magic and summon damage");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 18;
            item.value = 2200;
            item.rare = 1;
            item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.magicDamage += 0.04f;
            player.minionDamage += 0.04f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("DunestoneBar"), 25);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
