using ExoriumMod.Core;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace ExoriumMod.Content.Items.Weapons.Melee
{
	public class BruhBlade : ModItem
	{
		public override string Texture => AssetDirectory.MeleeWeapon + Name;

		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("BruhBlade"); //Bruh
			Tooltip.SetDefault("Bruh.");
		}

		public override void SetDefaults() 
		{
			Item.damage = 1;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 400;
			Item.useAnimation = 400;
			Item.useStyle = 1;
			Item.knockBack = 690000;
			Item.value = 1;
			Item.rare = -1;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void AddRecipes() 
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MudBlock, 999);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}