using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Weapons.Magic
{
    class Firebolt : ModItem
    {
        public override void SetDefaults()
        {
            item.autoReuse = true;
            item.rare = 1;
            item.mana = 7;
            item.UseSound = SoundID.Item20;
            item.noMelee = true;
            item.useStyle = 5;
            item.damage = 11;
            item.useAnimation = 27;
            item.useTime = 27;
            item.width = 24;
            item.height = 28;
            item.shoot = ProjectileType<Projectiles.FireboltProj>();
            item.scale = 0.9f;
            item.shootSpeed = 9f;
            item.knockBack = 5f;
            item.magic = true;
            item.value = 50000;
        }
    }
}
