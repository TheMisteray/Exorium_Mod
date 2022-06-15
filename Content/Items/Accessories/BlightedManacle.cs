using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Accessories
{
    class BlightedManacle : ModItem
    {
        public override string Texture => AssetDirectory.Accessory + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Reduced regeneration");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.value = 1000;
            Item.rare = 1;
            Item.defense = 4;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.lifeRegen -= 1;          
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Shackle);
            recipe.AddIngredient(ItemType<Materials.Metals.BlightsteelBar>(), 2);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
