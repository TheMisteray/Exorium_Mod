using ExoriumMod.Core;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.TileItems
{
	public class DeadwoodBookshelf : ModItem
	{
        public override string Texture => AssetDirectory.TileItem + Name;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 32;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 300;
			Item.createTile = ModContent.TileType<Tiles.DeadwoodBookshelfTile>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.WorkBench);
			recipe.AddIngredient(ModContent.ItemType<Deadwood>(), 20);
			recipe.AddIngredient(ItemID.Book, 5);
			recipe.Register();
		}
	}
}
