using ExoriumMod.Core;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Items.Weapons.Melee
{
    internal class InfernalSledge : ModItem
    {
        public override string Texture => AssetDirectory.MeleeWeapon + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Instantly kills non-boss creatures with 50 hp or less (100 in expert mode) \n" +
                "Heals the player whenever something is killed this way");
        }

        public override void SetDefaults()
        {
            Item.damage = 19;
            Item.DamageType = DamageClass.Melee;
            Item.width = 64;
            Item.height = 64;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useStyle = 1;
            Item.knockBack = 6;
            Item.rare = 3;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.value = Item.sellPrice(gold: 2, silver: 20);
            Item.channel = true;
        }
    }
}
