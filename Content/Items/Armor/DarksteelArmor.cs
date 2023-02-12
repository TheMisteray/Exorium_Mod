using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class DarksteelHelm : ModItem
    {
        public override string Texture => AssetDirectory.Armor + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("4% increased damage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 20;
            Item.value = 21000;
            Item.rare = 3;
            Item.defense = 6;
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
                + "\nChance to fire Darksteel Skulls upon getting hit";
            player.lifeRegen += 1;
            player.moveSpeed += 0.1f;
            player.GetCritChance(DamageClass.Generic) += 5;
            player.GetModPlayer<ExoriumPlayer>().morditeArmor = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.04f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.DarksteelBar>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

    [AutoloadEquip(EquipType.Body)]
    class DarksteelBreastplate : ModItem
    {
        public override string Texture => AssetDirectory.Armor + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("5% increased movement speed");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 22;
            Item.value = 20000;
            Item.rare = 3;
            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.05f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.DarksteelBar>(), 20);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    class DarksteelLeggings : ModItem
    {
        public override string Texture => AssetDirectory.Armor + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("15% increased movement speed");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = 18000;
            Item.rare = 3;
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.15f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.DarksteelBar>(), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}