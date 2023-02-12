using ExoriumMod.Core;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.WallItems.StructureWallItems.ShadowmancerWalls
{
    class DarkBrickWall : ModItem
    {
        public override string Texture => AssetDirectory.WallItem + Name;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 400;
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 7;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.createWall = WallType<Walls.StructureWalls.ShadowmancerWalls.DarkBrickWall>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(4);
            recipe.AddIngredient(ItemType<Items.TileItems.StructureTileItems.ShadowmancerTileItems.DarkBrick>());
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
