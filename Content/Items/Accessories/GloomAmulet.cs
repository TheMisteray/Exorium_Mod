using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

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
                "A staple spell focus for apprentice Shadowmancers and some more foolhardy Necromancers");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 64;
            item.accessory = true;
            item.value = 5000;
            item.rare = 1;
            item.defense = -2;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions++;
            player.minionDamage += 0.07f;
            player.meleeDamage -= .2f;
            player.rangedDamage -= .2f;
            player.magicDamage -= .2f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Weapons.Summoner.ShadowOrb>(), 7);
            recipe.AddIngredient(ItemType<Materials.Metals.DunestoneBar>(), 4);
            recipe.AddIngredient(ItemID.Ruby);
            recipe.AddTile(TileType<Tiles.ShadowAltar>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
