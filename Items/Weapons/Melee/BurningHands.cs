using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ExoriumMod.Dusts;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Weapons.Melee
{
    class BurningHands : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Unleash fists of flame\n" +
                "Right click to shoot flames from your fingertips\n" +
                "(Right click uses Mana, and can't be used with mana sickness)");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.rare = 3;
            item.damage = 42;
            item.melee = true;
            item.mana = 7;
            item.noMelee = true;
            item.value = Item.sellPrice(silver: 60, copper: 15);
            item.useStyle = 1;
            item.useTime = 32;
            item.useAnimation = 32;
            item.knockBack = 7;
            item.autoReuse = true;
            item.UseSound = SoundID.Item109;
            item.shoot = ProjectileType<Projectiles.BurningHand>();
            item.shootSpeed = 29;
            item.useTurn = true;
            item.noUseGraphic = true;
        }

        int[] proj = new int[5];

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.mana = 30;
                item.useTime = 14;
                item.useAnimation = 14;
                item.UseSound = SoundID.Item34;
                if (player.HasBuff(BuffID.ManaSickness))
                    return false;
            }
            else
            {
                item.mana = 0;
                item.useTime = 28;
                item.useAnimation = 28;
                item.UseSound = SoundID.Item109;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                for (int i = 0; i<=2; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(-(Main.rand.NextFloat(10) + 16) + (Main.rand.NextFloat(10)+16)*i));
                    Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, (int)(damage/3.5f), knockBack, player.whoAmI, 0, 1);
                }
            }
            else
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15)); //degree spread.
                                                                                                                // Stagger difference
                float scale = 1f - (Main.rand.NextFloat() * .3f);
                perturbedSpeed = perturbedSpeed * scale;
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HellstoneBar, 22);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
