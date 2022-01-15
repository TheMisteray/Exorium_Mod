using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Weapons.Summoner
{
    class ShadowOrb : ModItem
    {
        public override string Texture => "ExoriumMod/Projectiles/Bosses/AssierJassad/ShadowOrb";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Produces a shadow on impact that will do your bidding" +
                "\nShadows have a small chance to inflict consuming dark");
            ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 14;
            item.knockBack = 0f;
            item.width = 32;
            item.height = 32;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.value = Item.buyPrice(0, 0, 7, 0);
            item.rare = 1;
            item.UseSound = SoundID.Item1;
            item.maxStack = 999;
            item.noMelee = true;
            item.consumable = true;
            item.shootSpeed = 10f;
            item.summon = true;

            // These below are needed for a minion weapon
            item.noMelee = true;
            item.summon = true;
            // No buffTime because otherwise the item tooltip would say something like "1 minute duration"
            item.shoot = ProjectileType<Projectiles.Minions.ShadowOrbSummon>();
        }
    }
}
