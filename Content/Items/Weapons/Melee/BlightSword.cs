using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Weapons.Melee
{
    public class BlightSword : ModItem
    {
        public override string Texture => AssetDirectory.MeleeWeapon + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blighted Greatsword");
            Tooltip.SetDefault("A massive two-handed weapon \n" +
                "Hold to charge a stronger swing");
        }

        public override void SetDefaults()
        {
            item.damage = 50;
            item.melee = true;
            item.width = 590;
            item.height = 280;
            item.useTime = 22;
            item.useAnimation = 22;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.channel = true;
            item.knockBack = 15;
            item.value = Item.sellPrice(silver: 36); ;
            item.rare = 1;
            item.UseSound = SoundID.Item1;
            item.shoot = ProjectileType<SwungBlightedSword>();
            item.shootSpeed = 10f;
            item.noUseGraphic = true;
            //item.useTurn = true;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(0, 6) == 1)
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustType<Dusts.BlightDust>(), 0f, 0f, 50, default(Color), 1);
            }
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ProjectileType<SwungBlightedSword>()] <= 0;

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("BlightsteelBar"), 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class SwungBlightedSword : ModProjectile
    {
        public override string Texture => "ExoriumMod/Projectiles/BlightedSwordSwing";

        private const float AimResponsiveness = 0.06f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blighted Sword");
            Main.projFrames[projectile.type] = 7;
            ProjectileID.Sets.NeedsUUID[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 500;
            projectile.height = 30;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 216089;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public float state
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public float strength
        {
            get => projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.Center = player.Center;
            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true);

            //Visuals
            UpdatePlayerVisuals(player, rrp);

            if (player.dead)
                projectile.Kill();

            // Client Side
            if (Main.myPlayer == projectile.owner && state == 0f)
            {
                if (player.channel && !player.noItems && !player.CCed) //ends if not channeled, player is cursed, or player is stopped for other reasons
                {
                    //Dynamic aim
                    UpdateAim(rrp, player.HeldItem.shootSpeed);

                    projectile.netUpdate = true;
                    if (projectile.timeLeft % 90 == 0 && strength < 5)
                    {
                        strength++;
                        Vector2 distance = new Vector2(0, 5);
                        for (int i = 0; i < 360; i += (10 - (int)strength))
                        {
                            Vector2 rotatedDist = distance.RotatedBy(MathHelper.ToRadians(i));
                            Dust.NewDust(player.Center, 0, 0, DustType<Dusts.BlightDust>(), rotatedDist.X, rotatedDist.Y, 0, default, Main.rand.NextFloat(1f + (.2f * strength)));
                        }
                        Main.PlaySound(SoundID.MaxMana, projectile.Center);
                    }
                }

                // Move to swing
                else if (state == 0f)
                {
                    //Flip hand 180
                    player.itemRotation = (projectile.velocity * projectile.direction).ToRotation();

                    Main.PlaySound(SoundID.Item1, projectile.Center);
                    state = 1f;
                    projectile.timeLeft = 98;
                }

                for (int i = 0; i < strength; i++)
                {
                    //Create vector that is linearly dependant of the sword
                    Vector2 bladeLength = new Vector2(0, 1);
                    Vector2 blade = bladeLength.RotatedBy(projectile.rotation + MathHelper.PiOver2);
                    blade *= Main.rand.NextFloat(projectile.width / 2 - 30) + 30;
                    //dust along it
                    Dust.NewDust(player.Center + blade, 0, 0, DustType<Dusts.BlightDust>(), Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2), 0, default, Main.rand.NextFloat(.5f + (.3f * strength)));
                }
            }
            else if (state == 1f)
            {
                if (strength == 0f)
                    projectile.Kill();
                if (projectile.timeLeft % 3 == 0)
                {
                    projectile.frame++;

                    //after frame is increased, so just as the final frame is reached
                    if (projectile.frame == 6)
                    {
                        //Get vector to end of sword
                        Vector2 SwordTip = new Vector2(0, projectile.width/2);
                        Vector2 hitSpot = SwordTip.RotatedBy(projectile.rotation - MathHelper.PiOver2);
                        for (int i = 0; i < (strength + 1) * 20; i++)
                        {
                            //Dust burst at hit location, more dust, larger dust, and greater spread at high strength
                            Vector2 dustSpeed = new Vector2(0, Main.rand.NextFloat(8 + strength * 2));
                            Vector2 perturbedDustSpeed = dustSpeed.RotatedBy(MathHelper.ToRadians(Main.rand.Next(0, 361)));
                            Dust.NewDust(player.Center + hitSpot, 0, 0, DustType<Dusts.BlightDust>(), perturbedDustSpeed.X, perturbedDustSpeed.Y, 0, default, Main.rand.NextFloat(1f + (.2f * strength)));
                        }
                        //ending
                        Main.PlaySound(SoundID.Item89, projectile.Center);
                        state = 2f;
                    }
                }
            }
            else if (state == 2f)
            {
                state = 3f;
                projectile.timeLeft = 30;
            }
        }

        private void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
        {
            projectile.Center = playerHandPos;
            projectile.rotation = projectile.velocity.ToRotation();

            /*
            //Continuous holdout
            if (state == 0)
                player.ChangeDir(-projectile.direction);
            else //Flip player after swing
                player.ChangeDir(projectile.direction);
            */
            player.ChangeDir(projectile.direction);

            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;

            //Hand direction
            player.itemRotation = (projectile.velocity * projectile.direction).ToRotation();
        }

        private void UpdateAim(Vector2 source, float speed)
        {
            Vector2 aim = Vector2.Normalize(Main.MouseWorld - source);
            if (aim.HasNaNs())
            {
                aim = -Vector2.UnitY;
            }

            // Slow turn to cursor
            aim = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(projectile.velocity), aim, AimResponsiveness));
            aim *= speed;

            if (aim != projectile.velocity)
            {
                projectile.netUpdate = true;
            }
            projectile.velocity = aim;
        }

        public override bool CanDamage()
        {
            return projectile.frame == 6 && projectile.timeLeft == 30;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            //Right half
            hitbox.Y += projectile.height - projectile.width/2;
            hitbox.Height = projectile.width;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage *= (int)(strength + 1);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 SwordTip = new Vector2(0, projectile.width / 2);
            Vector2 hitSpot = SwordTip.RotatedBy(projectile.rotation - MathHelper.PiOver2);
            float _ = float.NaN;

            //Check collision of line from sword center to sword end with target hitbox
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + hitSpot, projectile.height * projectile.scale, ref _);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}

