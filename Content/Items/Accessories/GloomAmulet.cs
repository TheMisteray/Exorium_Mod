using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Accessories
{
    class GloomAmulet : ModItem
    {
        public override string Texture => AssetDirectory.Accessory + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("20% reduced magic, melee and ranged damage \n" +
                "-2 defense \n" +
                "7% increased minion damage \n" +
                "+1 max minions \n" +
                "A staple spell focus for apprentice Shadowmancers");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 64;
            Item.accessory = true;
            Item.value = 5000;
            Item.rare = 1;
            Item.defense = -2;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions++;
            player.GetDamage(DamageClass.Summon) *= 1.07f;
            player.GetDamage(DamageClass.Melee) *= .8f;
            player.GetDamage(DamageClass.Ranged) *= .8f;
            player.GetDamage(DamageClass.Magic) *= .8f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Weapons.Summoner.ShadowOrb>(), 7);
            recipe.AddIngredient(ItemType<Materials.Metals.DunestoneBar>(), 4);
            recipe.AddIngredient(ItemID.Ruby);
            recipe.AddTile(TileType<Tiles.ShadowAltarTile>());
            recipe.Register();
        }
    }
}
