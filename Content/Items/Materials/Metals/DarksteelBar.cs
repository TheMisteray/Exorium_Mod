﻿using ExoriumMod.Core;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

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
            item.createTile = TileType<Tiles.DarksteelBarTile>();
            item.placeStyle = 0;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BlightsteelBar>());
            recipe.AddIngredient(ItemID.HellstoneBar);
            recipe.AddIngredient(ItemID.CrimtaneBar);
            recipe.AddIngredient(ItemID.Bone, 4);
            recipe.AddTile(TileID.Hellforge);
            recipe.SetResult(this, 2);
            recipe.AddRecipe();
            ModRecipe recipeAlt = new ModRecipe(mod);
            recipeAlt.AddIngredient(ItemType<BlightsteelBar>());
            recipeAlt.AddIngredient(ItemID.HellstoneBar);
            recipeAlt.AddIngredient(ItemID.DemoniteBar);
            recipeAlt.AddIngredient(ItemID.Bone, 4);
            recipeAlt.AddTile(TileID.Hellforge);
            recipeAlt.SetResult(this, 2);
            recipeAlt.AddRecipe();
        }
    }
}