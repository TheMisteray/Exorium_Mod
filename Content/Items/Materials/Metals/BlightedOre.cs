using ExoriumMod.Core;
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
        }

        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.width = 16;
            item.height = 16;
            item.value = 1000;
            item.rare = 2;
            item.maxStack = 999;
            item.value = 200;
            item.consumable = true;
            item.createTile = TileType<Tiles.BlightedOreTile>();
        }
    }

}
