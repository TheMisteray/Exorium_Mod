using ExoriumMod.Core;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria;

namespace ExoriumMod.Content.Items.WallItems
{
    class DeadwoodWall : ModItem
    {
        public override string Texture => AssetDirectory.WallItem + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Incredibly light");
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 7;
            Item.useTime = 4;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.createWall = WallType<Walls.DeadwoodWall>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<TileItems.Deadwood>());
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
