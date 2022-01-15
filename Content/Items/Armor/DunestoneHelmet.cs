using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class DunestoneHelmet : ModItem
    {
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
            player.statManaMax += 40;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("DunestoneBar"), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
