using ExoriumMod.Content.Items.TileItems.StructureTileItems.FallenTowerTileItems;
using ExoriumMod.Core;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.WallItems.StructureWallItems.FallenTowerWallItems
{
    class CharredObsidianWall : ModItem
    {
        public override string Texture => AssetDirectory.WallItem + Name;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
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
            Item.createWall = WallType<Walls.StructureWalls.FallenTowerWalls.CharredObsidianWall>();
        }

        public override void AddRecipes()
        {
            Recipe recipe2 = CreateRecipe(4);
            recipe2.AddIngredient(ItemType<CharredObsidian>());
            recipe2.AddTile(TileID.WorkBenches);
            recipe2.Register();
        }
    }
}
