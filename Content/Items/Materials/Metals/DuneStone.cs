using ExoriumMod.Core;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Materials.Metals
{
    class DuneStone : ModItem
    {
        public override string Texture => AssetDirectory.Metal + Name;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 14;
            Item.rare = 0;
            Item.value = 100;
            Item.maxStack = 999;
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = TileType<Tiles.DuneStoneTile>();
        }
    }
}
