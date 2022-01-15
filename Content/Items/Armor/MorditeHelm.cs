using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.GameInput;

namespace ExoriumMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class MorditeHelm : ModItem
    {
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
            return body.type == ItemType<MorditeBreastplate>() && legs.type == ItemType<MorditeLeggings>();
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
}