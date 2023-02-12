using ExoriumMod.Core;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Materials.Metals
{
    public class BlightedOre : ModItem
    {
        public override string Texture => AssetDirectory.Metal + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blightsteel Ore");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

        public override void SetDefaults()
        {
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.width = 16;
            Item.height = 16;
            Item.value = 1000;
            Item.rare = 2;
            Item.maxStack = 999;
            Item.value = 200;
            Item.consumable = true;
            Item.createTile = TileType<Tiles.BlightedOreTile>();
        }
    }

}
