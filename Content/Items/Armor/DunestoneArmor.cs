using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Armor
{
    class DunestoneArmor
    {
        [AutoloadEquip(EquipType.Head)]
        class DunestoneHelmet : ModItem
        {
            public override string Texture => AssetDirectory.Armor + Name;

            public override void SetStaticDefaults()
            {
                Tooltip.SetDefault("+40 maximum mana \n+1 max minions");
            }

            public override void SetDefaults()
            {
                item.width = 24;
                item.height = 20;
                item.value = 2500;
                item.rare = 1;
                item.defense = 3;
            }

            public override bool IsArmorSet(Item head, Item body, Item legs)
            {
                return body.type == ItemType<DunestoneChainmail>() && legs.type == ItemType<DunestoneGreaves>();
            }

            public override void UpdateArmorSet(Player player)
            {
                player.setBonus = "Immunity to desert winds";
                player.buffImmune[BuffID.WindPushed] = true;
            }

            public override void UpdateEquip(Player player)
            {
                player.maxMinions += 1;
                player.statManaMax2 += 40;
            }

            public override void AddRecipes()
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemType<Materials.Metals.DunestoneBar>(), 15);
                recipe.AddTile(TileID.Anvils);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }

        [AutoloadEquip(EquipType.Body)]
        class DunestoneChainmail : ModItem
        {
            public override string Texture => AssetDirectory.Armor + Name;

            public override void SetStaticDefaults()
            {
                DisplayName.SetDefault("Dunestone Splint");
                Tooltip.SetDefault("4% increased magic and summon damage");
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
                player.magicDamage += 0.04f;
                player.minionDamage += 0.04f;
            }

            public override void AddRecipes()
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemType<Materials.Metals.DunestoneBar>(), 25);
                recipe.AddTile(TileID.Anvils);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }

        [AutoloadEquip(EquipType.Legs)]
        class DunestoneGreaves : ModItem
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
                recipe.AddIngredient(ItemType<Materials.Metals.DunestoneBar>(), 20);
                recipe.AddTile(TileID.Anvils);
                recipe.SetResult(this);
                recipe.AddRecipe();

            }
        }
    }
}
