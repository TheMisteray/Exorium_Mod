using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Armor
{
    class RimestoneArmor
    {
        [AutoloadEquip(EquipType.Head)]
        class RimestoneHelmet : ModItem
        {
            public override string Texture => AssetDirectory.Armor + Name;

            public override void SetStaticDefaults()
            {
                Tooltip.SetDefault("20% chance not to consume ammo \n5% Increased melee speed");
            }

            public override void SetDefaults()
            {
                item.width = 24;
                item.height = 22;
                item.value = 2500;
                item.rare = 1;
                item.defense = 4;
            }

            public override bool IsArmorSet(Item head, Item body, Item legs)
            {
                return body.type == ItemType<RimestoneChainmail>() && legs.type == ItemType<RimestoneGreaves>();
            }

            public override void UpdateArmorSet(Player player)
            {
                player.setBonus = "Immunity to chilled";
                player.buffImmune[BuffID.Chilled] = true;
            }

            public override void UpdateEquip(Player player)
            {
                player.meleeSpeed += 0.05f;
                player.GetModPlayer<ExoriumPlayer>().rimestoneArmorHead = true;
            }

            public override void AddRecipes()
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemType<Materials.Metals.RimestoneBar>(), 15);
                recipe.AddTile(TileID.Anvils);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }

        [AutoloadEquip(EquipType.Body)]
        class RimestoneChainmail : ModItem
        {
            public override string Texture => AssetDirectory.Armor + Name;

            public override void SetStaticDefaults()
            {
                Tooltip.SetDefault("4% Increased melee and ranged damage");
            }

            public override void SetDefaults()
            {
                item.width = 30;
                item.height = 18;
                item.value = 2200;
                item.rare = 1;
                item.defense = 4;
            }

            public override void UpdateEquip(Player player)
            {
                player.meleeDamage += 0.04f;
                player.rangedDamage += 0.04f;
            }

            public override void AddRecipes()
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemType<Materials.Metals.RimestoneBar>(), 25);
                recipe.AddTile(TileID.Anvils);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }

        [AutoloadEquip(EquipType.Legs)]
        class RimestoneGreaves : ModItem
        {
            public override string Texture => AssetDirectory.Armor + Name;

            public override void SetDefaults()
            {
                item.width = 22;
                item.height = 18;
                item.value = 2000;
                item.rare = 1;
                item.defense = 3;
            }

            public override void AddRecipes()
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemType<Materials.Metals.RimestoneBar>(), 20);
                recipe.AddTile(TileID.Anvils);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }
}
