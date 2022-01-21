using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using System;

namespace ExoriumMod.Content.Items.Weapons.Melee
{
    class FanOfColors : ModItem
    {
        public override string Texture => AssetDirectory.MeleeWeapon + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fan of Colors");
            Tooltip.SetDefault("Throw a fan of colored knives");
        }

        public override void SetDefaults()
        {
            item.damage = 17;
            item.melee = true;
            item.noMelee = true;
            item.width = 32;
            item.height = 32;
            item.useTime = 76;
            item.useAnimation = 76;
            item.useStyle = 1;
            item.knockBack = 3;
            item.value = Item.sellPrice(gold: 3, silver: 75);
            item.rare = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = ProjectileType<ColorKnife>();
            item.shootSpeed = 48f;
            item.useTurn = true;
            item.noUseGraphic = true;
        }

        private bool mode = true;

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float numberProjectiles = 7;
            float rotation = MathHelper.ToRadians(15);
            position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
            for (int i = 0; i<numberProjectiles; i++)
            {
                if (mode)
                {
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-(rotation/2), 1.5f*rotation, i / (numberProjectiles - 1))) * .2f;
                    int projectile = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
                    Main.projectile[projectile].localAI[1] = i;
                    Main.projectile[projectile].localAI[0] = 5 * i;
                }
                else
                {
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp((rotation/2), -rotation*1.5f, i / (numberProjectiles - 1))) * .2f;
                    int projectile = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
                    Main.projectile[projectile].localAI[1] = i;
                    Main.projectile[projectile].localAI[0] = 5 * i;
                }
            }
            mode = !mode;
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Amethyst);
            recipe.AddIngredient(ItemID.Topaz);
            recipe.AddIngredient(ItemID.Sapphire);
            recipe.AddIngredient(ItemID.Emerald);
            recipe.AddIngredient(ItemID.Ruby);
            recipe.AddIngredient(ItemID.Diamond);
            recipe.AddIngredient(ItemID.Amber);
            recipe.AddIngredient(ItemID.ThrowingKnife, 100);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    class ColorKnife : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 30;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.timeLeft = 360;
        }

        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
            projectile.active = true;
            projectile.alpha = 0;
            if (projectile.timeLeft == 360)
                projectile.netUpdate = true;
            if (projectile.localAI[0] > 0)
            {
                projectile.alpha = 255;
                projectile.localAI[0]--;
                switch (projectile.localAI[1])
                {
                    case 0:
                        Lighting.AddLight(projectile.position, 255 * 0.002f, 0 * 0.002f, 0 * 0.002f);
                        break;
                    case 1:
                        Lighting.AddLight(projectile.position, 255 * 0.002f, 110 * 0.002f, 0 * 0.002f);
                        break;
                    case 2:
                        Lighting.AddLight(projectile.position, 255 * 0.002f, 247 * 0.002f, 0 * 0.002f);
                        break;
                    case 3:
                        Lighting.AddLight(projectile.position, 0 * 0.002f, 255 * 0.002f, 0 * 0.002f);
                        break;
                    case 4:
                        Lighting.AddLight(projectile.position, 0 * 0.002f, 255 * 0.002f, 204 * 0.002f);
                        break;
                    case 5:
                        Lighting.AddLight(projectile.position, 35 * 0.002f, 0 * 0.002f, 255 * 0.002f);
                        break;
                    case 6:
                        Lighting.AddLight(projectile.position, 149 * 0.002f, 0 * 0.002f, 255 * 0.002f);
                        break;
                }
            }
            projectile.frame = (int)projectile.localAI[1];
        }

        public override bool ShouldUpdatePosition()
        {
            return projectile.localAI[0] <= 0;
        }

        public override bool CanDamage()
        {
            return projectile.localAI[0] <= 0;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item27, projectile.position);
        }
    }
}
