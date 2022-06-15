using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class DeadwoodHelmet : ModItem
    {
        public override string Texture => AssetDirectory.Armor + Name;

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 24;
            Item.rare = 0;
            Item.defense = 1;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<DeadwoodBreastplate>() && legs.type == ItemType<DeadwoodGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "10% increased speed";
            player.moveSpeed += 0.1f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<TileItems.Deadwood>(), 20);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }

    [AutoloadEquip(EquipType.Body)]
    class DeadwoodBreastplate : ModItem
    {
        public override string Texture => AssetDirectory.Armor + Name;

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 18;
            Item.rare = 0;
            Item.defense = 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<TileItems.Deadwood>(), 20);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    class DeadwoodGreaves : ModItem
    {
        public override string Texture => AssetDirectory.Armor + Name;

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.rare = 0;
            Item.defense = 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<TileItems.Deadwood>(), 25);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
