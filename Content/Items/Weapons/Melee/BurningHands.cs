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

    class BurningHand : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.height = 40;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.timeLeft = 25;
        }
        Dust dust81;
        int num2475;
        //ref float reference();
        public override void AI()
        {
            if (Main.rand.Next(3) == 0)
                FireDust();
            projectile.ai[0] += 1f; //Moved from FireDust to here to avoid extra calls since FireDust is called more than once per tick
            if (projectile.wet && !projectile.lavaWet) //water death
                projectile.active = false;
            if (projectile.ai[1] == 1)
            {
                projectile.alpha = 255;
                projectile.penetrate = -1;
                projectile.velocity = projectile.velocity * 0.95f;
                FireDust();
            }
            if (projectile.velocity != Vector2.Zero)
            {
                projectile.spriteDirection = projectile.direction;
                if (projectile.velocity.X >= 0)
                {
                    projectile.rotation = projectile.velocity.ToRotation();
                }
                else
                {
                    projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(180);
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.Kill();
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 420);
        }

        public override void Kill(int timeLeft)
        {
            if (projectile.ai[1] == 0)
            {
                projectile.velocity *= 0.4f;
                for (int i = 0; i < 25; i++)
                {
                    Vector2 randDir = projectile.velocity.RotatedByRandom(MathHelper.ToRadians(360));
                    FireDust();
                    //Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 6, randDir.X, randDir.Y, 0, default, Main.rand.NextFloat(3) + 1);
                }
                Main.PlaySound(SoundID.Item89, projectile.position);
            }
        }

        private void FireDust()
        {
            //ai for dust taken from flamethrower
            if (projectile.type == 188 && projectile.ai[0] < 8f)
            {
                projectile.ai[0] = 8f;
            }
            if (projectile.timeLeft > 60)
            {
                projectile.timeLeft = 60;
            }
            float num2127 = 1f;
            if (projectile.ai[0] == 8f)
            {
                num2127 = 0.25f;
            }
            else if (projectile.ai[0] == 9f)
            {
                num2127 = 0.5f;
            }
            else if (projectile.ai[0] == 10f)
            {
                num2127 = 0.75f;
            }
            int num2126 = 6;
            if (projectile.type == 101)
            {
                num2126 = 75;
            }
            if (num2126 == 6 || Main.rand.Next(2) == 0)
            {
                for (int num2125 = 0; num2125 < 1; num2125 = num2475 + 1)
                {
                    Vector2 position100 = new Vector2(projectile.position.X, projectile.position.Y);
                    int width87 = projectile.width;
                    int height87 = projectile.height;
                    int num2484 = num2126;
                    float speedX25 = projectile.velocity.X * 0.2f;
                    float speedY29 = projectile.velocity.Y * 0.2f;
                    Color newColor5 = default(Color);
                    int num2124 = Dust.NewDust(position100, width87, height87, num2484, speedX25, speedY29, 100, newColor5, 1f);
                    if (Main.rand.Next(3) != 0 || (num2126 == 75 && Main.rand.Next(3) == 0))
                    {
                        Main.dust[num2124].noGravity = true;
                        dust81 = Main.dust[num2124];
                        dust81.scale *= 3f;
                        Dust expr_DD41_cp_0 = Main.dust[num2124];
                        expr_DD41_cp_0.velocity.X = expr_DD41_cp_0.velocity.X * 2f;
                        Dust expr_DD61_cp_0 = Main.dust[num2124];
                        expr_DD61_cp_0.velocity.Y = expr_DD61_cp_0.velocity.Y * 2f;
                    }
                    if (projectile.type == 188)
                    {
                        dust81 = Main.dust[num2124];
                        dust81.scale *= 1.25f;
                    }
                    else
                    {
                        dust81 = Main.dust[num2124];
                        dust81.scale *= 1.5f;
                    }
                    Dust expr_DDC6_cp_0 = Main.dust[num2124];
                    expr_DDC6_cp_0.velocity.X = expr_DDC6_cp_0.velocity.X * 1.2f;
                    Dust expr_DDE6_cp_0 = Main.dust[num2124];
                    expr_DDE6_cp_0.velocity.Y = expr_DDE6_cp_0.velocity.Y * 1.2f;
                    dust81 = Main.dust[num2124];
                    dust81.scale *= num2127;
                    if (num2126 == 75)
                    {
                        dust81 = Main.dust[num2124];
                        dust81.velocity += projectile.velocity;
                        if (!Main.dust[num2124].noGravity)
                        {
                            dust81 = Main.dust[num2124];
                            dust81.velocity *= 0.5f;
                        }
                    }
                    num2475 = num2125;
                }
            }

        }
    }
}
