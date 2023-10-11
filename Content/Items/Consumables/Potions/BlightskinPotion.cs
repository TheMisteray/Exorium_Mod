using ExoriumMod.Core;
using Terraria;
using Terraria.GameContent.Creative;
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
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 12;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 30;
            Item.consumable = true;
            Item.rare = 3;
            Item.value = Item.buyPrice(gold: 1);
            Item.buffType = BuffType<Buffs.Blightskin>();
            Item.buffTime = 28800;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ItemType<Materials.Metals.BlightedOre>());
            recipe.AddIngredient(ItemType<Materials.Deadweed>());
            recipe.AddIngredient(ItemType<Materials.WightBone>());
            recipe.AddTile(TileID.Bottles);
            recipe.Register();
        }
    }
}
