using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ExoriumMod.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Shield)]
    class BlightedShield : ModItem
    {
        public override string Texture => AssetDirectory.Accessory + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Reduced regeneration" +
                "\n25% decreased speed" +
                "\nAllows the player to dash into the enemy" +
                "\nDouble tap a direction");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 38;
            item.accessory = true;
            item.value = 4200;
            item.rare = 1;
            item.defense = 6;
            item.melee = true;
            item.damage = 42;
            item.knockBack = 7f;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.lifeRegen -= 1;
            player.moveSpeed -= 0.25f;
            player.dash = 2;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("BlightsteelBar"), 12);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
