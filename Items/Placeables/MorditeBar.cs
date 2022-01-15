using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Placeables
{
    class MorditeBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mordite Alloy");
            Tooltip.SetDefault("Not the real stuff... But It'll do");
        }
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 24;
            item.rare = 3;
            item.value = 15000;
            item.maxStack = 99;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.consumable = true;
            item.createTile = TileType<Tiles.MorditeBar>();
            item.placeStyle = 0;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("BlightsteelBar"));
            recipe.AddIngredient(ItemID.HellstoneBar);
            recipe.AddIngredient(ItemID.CrimtaneBar);
            recipe.AddIngredient(ItemID.Bone, 4);
            recipe.AddTile(TileID.Hellforge);
            recipe.SetResult(this, 2);
            recipe.AddRecipe();
            ModRecipe recipeAlt = new ModRecipe(mod);
            recipeAlt.AddIngredient(mod.GetItem("BlightsteelBar"));
            recipeAlt.AddIngredient(ItemID.HellstoneBar);
            recipeAlt.AddIngredient(ItemID.DemoniteBar);
            recipeAlt.AddIngredient(ItemID.Bone, 4);
            recipeAlt.AddTile(TileID.Hellforge);
            recipeAlt.SetResult(this, 2);
            recipeAlt.AddRecipe();
        }
    }
}
