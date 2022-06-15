using ExoriumMod.Core;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace ExoriumMod.Content.Items.TileItems
{
    class DeadwoodDresser : ModItem
    {
        public override string Texture => AssetDirectory.TileItem + Name;

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 500;
			Item.createTile = ModContent.TileType<Tiles.DeadwoodDresserTile>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Dresser);
			recipe.AddIngredient(ModContent.ItemType<Deadwood>(), 16);
			recipe.Register();
		}
	}
}
