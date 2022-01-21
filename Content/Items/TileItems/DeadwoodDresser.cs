using ExoriumMod.Core;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Items.TileItems
{
    class DeadwoodDresser : ModItem
    {
        public override string Texture => AssetDirectory.TileItem + Name;

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 22;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 500;
			item.createTile = ModContent.TileType<Tiles.DeadwoodDresserTile>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Dresser);
			recipe.AddIngredient(ModContent.ItemType<Deadwood>(), 16);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
