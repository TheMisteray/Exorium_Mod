using ExoriumMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Weapons.Ranger
{
    class AcidOrb : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A glass sphere filled with an acidic substance" +
                "\nCovers hit enemies with acid");
        }

        public override void SetDefaults()
        {
            item.damage = 20;
            item.useStyle = 5;
            item.useAnimation = 32;
            item.useTime = 32;
            item.shootSpeed = 13f;
            item.knockBack = 6.5f;
            item.width = 32;
            item.height = 32;
            item.scale = 1f;
            item.rare = 1;
            item.value = Item.sellPrice(silver: 4);
            item.consumable = true;
            item.maxStack = 999;
            item.ranged = true;
            item.noMelee = true; 
            item.noUseGraphic = true; 
            item.autoReuse = true; 
            item.UseSound = SoundID.Item1;
            item.shoot = ProjectileType<Projectiles.AcidOrb>();
        }
    }
}
