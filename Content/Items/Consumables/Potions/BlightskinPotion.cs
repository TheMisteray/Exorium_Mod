﻿using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Consumables.Potions
{
    class BlightskinPotion : ModItem
    {
        public override string Texture => AssetDirectory.Potion + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("12 defense \nGreatly reduced life regeneration");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 26;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.useAnimation = 15;
            item.useTime = 15;
            item.useTurn = true;
            item.UseSound = SoundID.Item3;
            item.maxStack = 30;
            item.consumable = true;
            item.rare = 3;
            item.value = Item.buyPrice(gold: 1);
            item.buffType = BuffType<Buffs.Blightskin>();
            item.buffTime = 28800;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ItemType<Materials.Metals.BlightedOre>());
            recipe.AddIngredient(ItemType<Materials.Deadweed>());
            recipe.AddIngredient(ItemType<Materials.WightBone>());
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}