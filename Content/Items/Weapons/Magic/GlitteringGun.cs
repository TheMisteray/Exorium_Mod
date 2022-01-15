using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Weapons.Magic
{
    class GlitteringGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Or was it the glimmering gun...");
        }

        public override void SetDefaults()
        {
            item.damage = 12;
            item.magic = true;
            item.mana = 4;
            item.width = 40;
            item.height = 40;
            item.useTime = 9;
            item.useAnimation = 9;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 4;
            item.UseSound = SoundID.Item9;
            item.autoReuse = true;
            item.shoot = ProjectileType<SparkleShot>();
            item.shootSpeed = 16f;
            item.rare = 2;
            item.value = Item.sellPrice(gold: 3, silver: 50);
            item.scale = 1.2f;
        }

        float rainbow = 0;

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(3));
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 75f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            int projectile = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            Main.projectile[projectile].localAI[1] = rainbow;
            rainbow++;
            if (rainbow == 7)
                rainbow = 0;
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
            recipe.AddIngredient(ItemID.Minishark);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
    }

    class SparkleShot : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.timeLeft = 600;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
        }

        public override void AI()
        {
            projectile.velocity.Y += projectile.ai[0];
            projectile.alpha = 225;
            projectile.extraUpdates = 2;
            for (int i = 0; i < 10; i++)
            {
                float newx = projectile.position.X - projectile.velocity.X / 10f * i;
                float newy = projectile.position.Y - projectile.velocity.Y / 10f * i;
                int dust0 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Rainbow>(), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 0, default(Color), 0.5f);
                Main.dust[dust0].position.X = newx;
                Main.dust[dust0].position.Y = newy;
                switch (projectile.localAI[1])
                {
                    case 0:
                        Main.dust[dust0].color = new Color(255, 0, 0);
                        break;
                    case 1:
                        Main.dust[dust0].color = new Color(255, 110, 0);
                        break;
                    case 2:
                        Main.dust[dust0].color = new Color(255, 247, 0);
                        break;
                    case 3:
                        Main.dust[dust0].color = new Color(0, 255, 0);
                        break;
                    case 4:
                        Main.dust[dust0].color = new Color(0, 255, 204);
                        break;
                    case 5:
                        Main.dust[dust0].color = new Color(35, 0, 255);
                        break;
                    case 6:
                        Main.dust[dust0].color = new Color(149, 0, 255);
                        break;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item27, projectile.position);
        }
    }
}
