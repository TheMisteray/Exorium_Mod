using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Armor
{
    class DeadwoodArmor
    {

        [AutoloadEquip(EquipType.Head)]
        class DeadwoodHelmet : ModItem
        {
            public override string Texture => AssetDirectory.Armor + Name;

            public override void SetDefaults()
            {
                item.width = 28;
                item.height = 24;
                item.rare = 0;
                item.defense = 1;
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
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemType<TileItems.Deadwood>(), 20);
                recipe.AddTile(TileID.WorkBenches);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }

        [AutoloadEquip(EquipType.Body)]
        class DeadwoodBreastplate : ModItem
        {
            public override string Texture => AssetDirectory.Armor + Name;

            public override void SetDefaults()
            {
                item.width = 30;
                item.height = 18;
                item.rare = 0;
                item.defense = 1;
            }

            public override void AddRecipes()
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemType<TileItems.Deadwood>(), 20);
                recipe.AddTile(TileID.WorkBenches);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }

        [AutoloadEquip(EquipType.Legs)]
        class DeadwoodGreaves : ModItem
        {
            public override string Texture => AssetDirectory.Armor + Name;

            public override void SetDefaults()
            {
                item.width = 22;
                item.height = 18;
                item.rare = 0;
                item.defense = 1;
            }

            public override void AddRecipes()
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemType<TileItems.Deadwood>(), 25);
                recipe.AddTile(TileID.WorkBenches);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }
}
