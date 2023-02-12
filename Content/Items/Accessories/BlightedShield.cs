using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Shield)]
    class BlightedShield : ModItem
    {
        public override string Texture => AssetDirectory.Accessory + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Reduced regeneration" +
                "\n25% decreased speed" +
                "\nAllows the player to dash into the enemy" +
                "\nDouble tap a direction");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 38;
            Item.accessory = true;
            Item.value = 4200;
            Item.rare = 1;
            Item.defense = 6;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 42;
            Item.knockBack = 7f;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.lifeRegen -= 1;
            player.moveSpeed -= 0.25f;
            player.dash = 2;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.BlightsteelBar>(), 12);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
