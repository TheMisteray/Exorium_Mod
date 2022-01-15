using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class RimestoneHelmet : ModItem
    {
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
            recipe.AddIngredient(mod.GetItem("RimestoneBar"), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
