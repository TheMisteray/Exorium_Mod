using ExoriumMod.Core;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Items.TileItems
{
	public class DeadwoodTable : ModItem
	{
        public override string Texture => AssetDirectory.TileItem + Name;

        public override void SetDefaults()
		{
			item.width = 30;
			item.height = 22;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 200;
			item.createTile = ModContent.TileType<Tiles.DeadwoodTableTile>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WorkBench);
			recipe.AddIngredient(ModContent.ItemType<Deadwood>(), 8);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
