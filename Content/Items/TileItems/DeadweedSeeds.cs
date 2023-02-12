using ExoriumMod.Core;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.TileItems
{
    class DeadweedSeeds : ModItem
    {
        public override string Texture => AssetDirectory.Plant + Name;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.useStyle = 1;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.maxStack = 99;
            Item.consumable = true;
            Item.placeStyle = 0;
            Item.width = 12;
            Item.height = 14;
            Item.value = 80;
            Item.createTile = TileType<Tiles.DeadweedTile>();
        }
    }
}
