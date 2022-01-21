using ExoriumMod.Core;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Items.TileItems
{
	public class DeadwoodBed : ModItem
	{
        public override string Texture => AssetDirectory.TileItem + Name;

        public override void SetDefaults()
		{
			item.width = 28;
			item.height = 20;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 2000;
			item.createTile = ModContent.TileType<Tiles.DeadwoodBedTile>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 5);
			recipe.AddIngredient(ModContent.ItemType<Deadwood>(), 15);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
