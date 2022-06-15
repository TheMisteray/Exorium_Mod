using ExoriumMod.Core;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace ExoriumMod.Content.Items.TileItems
{
	public class DeadwoodBed : ModItem
	{
        public override string Texture => AssetDirectory.TileItem + Name;

        public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 20;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 2000;
			Item.createTile = ModContent.TileType<Tiles.DeadwoodBedTile>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Silk, 5);
			recipe.AddIngredient(ModContent.ItemType<Deadwood>(), 15);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
