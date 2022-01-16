using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Bosses.BlightedSlime
{
    class TaintedSludge : ModItem
    {
        public override string Texture => AssetDirectory.BlightedSlime + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons the Blighted Slime");
            ItemID.Sets.SortingPriorityBossSpawns[item.type] = 13;
        }

        public override void SetDefaults()
        {
            item.value = 0;
            item.width = 16;
            item.height = 14;
            item.rare = 1;
            item.maxStack = 99;
            item.useAnimation = 45;
            item.useTime = 45;
            item.useStyle = 4;
            item.UseSound = SoundID.Item44;
            item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return Main.LocalPlayer.GetModPlayer<BiomeHandler>().ZoneDeadlands && !NPC.AnyNPCs(NPCType<BlightedSlime>());
        }

        public override bool UseItem(Player player)
        {
            Main.PlaySound(SoundID.Roar, player.position, 0);
            NPC.SpawnOnPlayer(player.whoAmI, NPCType<BlightedSlime>());
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Gel, 30);
            recipe.AddIngredient(ItemType<Items.Materials.WightBone>(), 15);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
