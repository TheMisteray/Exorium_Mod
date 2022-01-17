using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Armor
{
    class BlightedArmor
    {
        [AutoloadEquip(EquipType.Head)]
        class BlightedHelm : ModItem
        {
            public override string Texture => AssetDirectory.Armor + Name;
            public override void SetStaticDefaults()
            {
                DisplayName.SetDefault("Blightsteel Crown");
                Tooltip.SetDefault("2% decreased movement speed"
                    + "\n2% increased damage"
                    + "\nDecreased regeneration");
            }

            public override void SetDefaults()
            {
                item.width = 28;
                item.height = 24;
                item.value = 15000;
                item.rare = 2;
                item.defense = 7;
            }

            public override bool IsArmorSet(Item head, Item body, Item legs)
            {
                return body.type == ItemType<BlightedChestplate>() && legs.type == ItemType<BlightedLeggings>();
            }

            public override void UpdateArmorSet(Player player)
            {
                player.setBonus = "3 defense"
                    + "\n7% increased damage";
                player.allDamage += 0.07f;
                player.statDefense += 3;
            }

            public override void UpdateEquip(Player player)
            {
                player.moveSpeed -= 0.02f;
                player.allDamage += 0.02f;
                player.lifeRegen -= 1;
            }

            public override void AddRecipes()
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemType<Materials.Metals.BlightsteelBar>(), 15);
                recipe.AddIngredient(ItemType<Materials.TaintedGel>(), 10);
                recipe.AddTile(TileID.Anvils);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }

        [AutoloadEquip(EquipType.Body)]
        class BlightedChestplate : ModItem
        {
            public override string Texture => AssetDirectory.Armor + Name;

            public override void SetStaticDefaults()
            {
                DisplayName.SetDefault("Blightsteel Breastplate");
                Tooltip.SetDefault("\n6% decreased movement speed" +
                    "\n3% increased damage"
                    + "\nDecreased regeneration");
            }

            public override void SetDefaults()
            {
                item.width = 30;
                item.height = 20;
                item.value = 9500;
                item.rare = 2;
                item.defense = 8;
            }

            public override void UpdateEquip(Player player)
            {
                player.moveSpeed -= 0.06f;
                player.allDamage += 0.03f;
                player.lifeRegen -= 1;
            }

            public override void AddRecipes()
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemType<Materials.Metals.BlightsteelBar>(), 25);
                recipe.AddIngredient(ItemType<Materials.TaintedGel>(), 20);
                recipe.AddTile(TileID.Anvils);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }

        [AutoloadEquip(EquipType.Legs)]
        class BlightedLeggings : ModItem
        {
            public override string Texture => AssetDirectory.Armor + Name;
            public override void SetStaticDefaults()
            {
                DisplayName.SetDefault("Blightsteel Leggings");
                Tooltip.SetDefault("3% decreased movement speed" +
                    "\n2% increased damage"
                    + "\nDecreased regeneration");
            }

            public override void SetDefaults()
            {
                item.width = 22;
                item.height = 18;
                item.value = 7500;
                item.rare = 2;
                item.defense = 7;
            }

            public override void UpdateEquip(Player player)
            {
                player.moveSpeed -= 0.03f;
                player.allDamage += 0.02f;
                player.lifeRegen -= 1;
            }

            public override void AddRecipes()
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemType<Materials.Metals.BlightsteelBar>(), 20);
                recipe.AddIngredient(ItemType<Materials.TaintedGel>(), 15);
                recipe.AddTile(TileID.Anvils);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }
}
