using ExoriumMod.Core;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria;

namespace ExoriumMod.Content.Items.Materials.Metals
{
    class DarksteelBar : ModItem
    {
        public override string Texture => AssetDirectory.Metal + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darksteel Alloy");
            Tooltip.SetDefault("Not the real stuff, but It'll do for now");
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.rare = 3;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 15000;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = TileType<Tiles.DarksteelBarTile>();
            Item.placeStyle = 0;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(2);
            recipe.AddIngredient(ItemType<BlightsteelBar>());
            recipe.AddIngredient(ItemID.HellstoneBar);
            recipe.AddIngredient(ItemID.CrimtaneBar);
            recipe.AddIngredient(ItemID.Bone, 4);
            recipe.AddTile(TileID.Hellforge);
            recipe.Register();

            Recipe recipeAlt = CreateRecipe(2);
            recipeAlt.AddIngredient(ItemType<BlightsteelBar>());
            recipeAlt.AddIngredient(ItemID.HellstoneBar);
            recipeAlt.AddIngredient(ItemID.DemoniteBar);
            recipeAlt.AddIngredient(ItemID.Bone, 4);
            recipeAlt.AddTile(TileID.Hellforge);
            recipeAlt.Register();
        }
    }
}
