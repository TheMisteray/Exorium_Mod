using Terraria.ID;
using Terraria.ModLoader;

namespace ExoriumMod.Items.Weapons.Melee
{
	public class BruhBlade : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("BruhBlade"); //Bruh
			Tooltip.SetDefault("Bruh.");
		}

		public override void SetDefaults() 
		{
			item.damage = 1;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 400;
			item.useAnimation = 400;
			item.useStyle = 1;
			item.knockBack = 690000;
			item.value = 1;
			item.rare = -1;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

		public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DirtBlock, 999);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}