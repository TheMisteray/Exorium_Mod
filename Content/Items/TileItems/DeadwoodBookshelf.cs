using ExoriumMod.Core;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Items.TileItems
{
	public class DeadwoodBookshelf : ModItem
	{
        public override string Texture => AssetDirectory.TileItem + Name;

        public override void SetDefaults()
		{
			item.width = 24;
			item.height = 32;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 300;
			item.createTile = ModContent.TileType<Tiles.DeadwoodBookshelfTile>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WorkBench);
			recipe.AddIngredient(ModContent.ItemType<Deadwood>(), 20);
			recipe.AddIngredient(ItemID.Book, 5);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
