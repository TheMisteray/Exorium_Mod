using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class BlightedHelm : ModItem
    {
        public override string Texture => AssetDirectory.Armor + Name;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 24;
            Item.value = 15000;
            Item.rare = 2;
            Item.defense = 7;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<BlightedChestplate>() && legs.type == ItemType<BlightedLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "3 defense"
                + "\n7% increased damage";
            player.GetDamage(DamageClass.Generic) += .07f;
            player.statDefense += 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed -= 0.02f;
            player.GetDamage(DamageClass.Generic) += .02f;
            player.lifeRegen -= 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.BlightsteelBar>(), 15);
            recipe.AddIngredient(ItemType<Materials.TaintedGel>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

    [AutoloadEquip(EquipType.Body)]
    class BlightedChestplate : ModItem
    {
        public override string Texture => AssetDirectory.Armor + Name;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.value = 9500;
            Item.rare = 2;
            Item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed -= 0.06f;
            player.GetDamage(DamageClass.Generic) += .03f;
            player.lifeRegen -= 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.BlightsteelBar>(), 25);
            recipe.AddIngredient(ItemType<Materials.TaintedGel>(), 20);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    class BlightedLeggings : ModItem
    {
        public override string Texture => AssetDirectory.Armor + Name;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = 7500;
            Item.rare = 2;
            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed -= 0.03f;
            player.GetDamage(DamageClass.Generic) += 0.02f;
            player.lifeRegen -= 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.BlightsteelBar>(), 20);
            recipe.AddIngredient(ItemType<Materials.TaintedGel>(), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
