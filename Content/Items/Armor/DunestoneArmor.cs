using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Armor
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
            Item.width = 24;
            Item.height = 20;
            Item.value = 2500;
            Item.rare = 1;
            Item.defense = 3;
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
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.DunestoneBar>(), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
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
            Item.width = 30;
            Item.height = 18;
            Item.value = 2200;
            Item.rare = 1;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Magic) += 0.04f;
            player.GetDamage(DamageClass.Summon) += 0.04f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.DunestoneBar>(), 25);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    class DunestoneGreaves : ModItem
    {
        public override string Texture => AssetDirectory.Armor + Name;

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = 2000;
            Item.rare = 1;
            Item.defense = 3;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.DunestoneBar>(), 20);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

        }
    }
}
