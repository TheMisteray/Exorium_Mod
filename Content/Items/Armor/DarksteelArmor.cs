using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Armor
{
    class DarksteelArmor
    {
        [AutoloadEquip(EquipType.Head)]
        class DarksteelHelm : ModItem
        {
            public override string Texture => AssetDirectory.Armor + Name;

            public override void SetStaticDefaults()
            {
                Tooltip.SetDefault("4% increased damage");
            }

            public override void SetDefaults()
            {
                item.width = 26;
                item.height = 20;
                item.value = 21000;
                item.rare = 3;
                item.defense = 6;
            }

            public override bool IsArmorSet(Item head, Item body, Item legs)
            {
                return body.type == ItemType<DarksteelBreastplate>() && legs.type == ItemType<DarksteelLeggings>();
            }

            public override void UpdateArmorSet(Player player)
            {
                player.setBonus = "10% increased movement speed"
                    + "\n5% increased critcal strike chance"
                    + "\nIncreased regeneration"
                    + "\nChance to fire Mordite Skulls upon getting hit";
                player.lifeRegen += 1;
                player.moveSpeed += 0.1f;
                player.meleeCrit += 5;
                player.rangedCrit += 5;
                player.magicCrit += 5;
                player.thrownCrit += 5;
                player.GetModPlayer<ExoriumPlayer>().morditeArmor = true;
            }

            public override void UpdateEquip(Player player)
            {
                player.allDamage += 0.04f;
            }

            public override void AddRecipes()
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(mod.GetItem("MorditeBar"), 10);
                recipe.AddTile(TileID.Anvils);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }

        [AutoloadEquip(EquipType.Body)]
        class DarksteelBreastplate : ModItem
        {
            public override string Texture => AssetDirectory.Armor + Name;

            public override void SetStaticDefaults()
            {
                Tooltip.SetDefault("5% increased movement speed");
            }

            public override void SetDefaults()
            {
                item.width = 26;
                item.height = 22;
                item.value = 20000;
                item.rare = 3;
                item.defense = 7;
            }

            public override void UpdateEquip(Player player)
            {
                player.moveSpeed += 0.05f;
            }

            public override void AddRecipes()
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(mod.GetItem("MorditeBar"), 20);
                recipe.AddTile(TileID.Anvils);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }

        [AutoloadEquip(EquipType.Legs)]
        class DarksteelLeggings : ModItem
        {
            public override string Texture => AssetDirectory.Armor + Name;

            public override void SetStaticDefaults()
            {
                Tooltip.SetDefault("15% increased movement speed");
            }

            public override void SetDefaults()
            {
                item.width = 22;
                item.height = 18;
                item.value = 18000;
                item.rare = 3;
                item.defense = 6;
            }

            public override void UpdateEquip(Player player)
            {
                player.moveSpeed += 0.15f;
            }

            public override void AddRecipes()
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(mod.GetItem("MorditeBar"), 15);
                recipe.AddTile(TileID.Anvils);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }
}