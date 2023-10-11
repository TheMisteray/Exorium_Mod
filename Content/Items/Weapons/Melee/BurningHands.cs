using ExoriumMod.Core;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Weapons.Melee
{
    class BurningHands : ModItem
    {
        public override string Texture => AssetDirectory.MeleeWeapon + Name;

        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Unleash flames from your fingertips\n" +
                "Uses Mana and can't be used with mana sickness\n" +
                "Right click to shoot a flaming fist\n" +
                "(Right click does not use mana)"); */
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.rare = 3;
            Item.damage = 42;
            Item.DamageType = DamageClass.Melee;
            Item.mana = 7;
            Item.noMelee = true;
            Item.value = Item.sellPrice(silver: 60, copper: 15);
            Item.useStyle = 1;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.knockBack = 7;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item109;
            Item.shoot = ProjectileType<BurningHand>();
            Item.shootSpeed = 29;
            Item.useTurn = true;
            Item.noUseGraphic = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (!(player.altFunctionUse == 2))
            {
                Item.mana = 30;
                Item.useTime = 14;
                Item.useAnimation = 14;
                Item.UseSound = SoundID.Item34;
                if (player.HasBuff(BuffID.ManaSickness))
                    return false;
            }
            else
            {
                Item.mana = 0;
                Item.useTime = 28;
                Item.useAnimation = 28;
                Item.UseSound = SoundID.Item109;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                for (int i = 0; i<=2; i++)
                {
                    Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(-(Main.rand.NextFloat(10) + 16) + (Main.rand.NextFloat(10)+16)*i));
                    Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, (int)(damage/4f), knockback, player.whoAmI, 0, 1);
                }
            }
            else
            {
                Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(15)); //degree spread.
                                                                                                                // Stagger difference
                float scale = 1f - (Main.rand.NextFloat() * .3f);
                perturbedSpeed = perturbedSpeed * scale;
                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HellstoneBar, 22);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

    class BurningHand : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 25;
            Projectile.penetrate = 3;
        }

        public override void AI()
        {
            if (Main.rand.Next(3) == 0)
                FireDust();
            Projectile.ai[0] += 1f; //Moved from FireDust to here to avoid extra calls since FireDust is called more than once per tick
            if (Projectile.wet && !Projectile.lavaWet) //water death
                Projectile.active = false;
            if (Projectile.ai[1] == 1)
            {
                Projectile.alpha = 255;
                Projectile.velocity = Projectile.velocity * 0.95f;
                FireDust();
            }
            else
                Projectile.penetrate = 1;
            if (Projectile.velocity != Vector2.Zero)
            {
                Projectile.spriteDirection = Projectile.direction;
                if (Projectile.velocity.X >= 0)
                {
                    Projectile.rotation = Projectile.velocity.ToRotation();
                }
                else
                {
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(180);
                }
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width /= 2;
            height /= 2;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 420);
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[1] == 0)
            {
                Projectile.velocity *= 0.4f;
                for (int i = 0; i < 25; i++)
                {
                    Vector2 randDir = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(360));
                    FireDust();
                    //Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 6, randDir.X, randDir.Y, 0, default, Main.rand.NextFloat(3) + 1);
                }
                SoundEngine.PlaySound(SoundID.Item89, Projectile.position);
            }
        }

        private void FireDust()
        {
            //These were moved in from outside
            Dust dust81;
            int num2475;

            //ai for dust taken from flamethrower
            if (Projectile.type == 188 && Projectile.ai[0] < 8f)
            {
                Projectile.ai[0] = 8f;
            }
            if (Projectile.timeLeft > 60)
            {
                Projectile.timeLeft = 60;
            }
            float num2127 = 1f;
            if (Projectile.ai[0] == 8f)
            {
                num2127 = 0.25f;
            }
            else if (Projectile.ai[0] == 9f)
            {
                num2127 = 0.5f;
            }
            else if (Projectile.ai[0] == 10f)
            {
                num2127 = 0.75f;
            }
            int num2126 = 6;
            if (Projectile.type == 101)
            {
                num2126 = 75;
            }
            if (num2126 == 6 || Main.rand.Next(2) == 0)
            {
                for (int num2125 = 0; num2125 < 1; num2125 = num2475 + 1)
                {
                    Vector2 position100 = new Vector2(Projectile.position.X, Projectile.position.Y);
                    int width87 = Projectile.width;
                    int height87 = Projectile.height;
                    int num2484 = num2126;
                    float speedX25 = Projectile.velocity.X * 0.2f;
                    float speedY29 = Projectile.velocity.Y * 0.2f;
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
                    if (Projectile.type == 188)
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
                        dust81.velocity += Projectile.velocity;
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
