using ExoriumMod.Core;
using ExoriumMod.Helpers;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Weapons.Melee
{
    public class BlightSword : ModItem
    {
        public override string Texture => AssetDirectory.MeleeWeapon + Name;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Blighted Greatsword");
            /* Tooltip.SetDefault("A massive two-handed weapon\n" +
                "Hold to charge a stronger swing"); */
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.DamageType = DamageClass.Melee;
            Item.width = 590;
            Item.height = 280;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.channel = true;
            Item.knockBack = 15;
            Item.value = Item.sellPrice(silver: 36); ;
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ProjectileType<SwungBlightedSword>();
            Item.shootSpeed = 10f;
            Item.noUseGraphic = true;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ProjectileType<SwungBlightedSword>()] <= 0;

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.BlightsteelBar>(), 16);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

    public class SwungBlightedSword : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + "BlightedSwordSwing";

        private const float AimResponsiveness = 0.06f;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Blighted Sword");
            Main.projFrames[Projectile.type] = 7;
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 500;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 216089;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.noEnchantmentVisuals = true;
        }

        public float state
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public float strength
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.Center = player.Center;
            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true);

            //Visuals
            UpdatePlayerVisuals(player, rrp);

            if (player.dead)
                Projectile.Kill();

            // Client Side
            if (Main.myPlayer == Projectile.owner && state == 0f)
            {
                if (player.channel && !player.noItems && !player.CCed) //ends if not channeled, player is cursed, or player is stopped for other reasons
                {
                    //Dynamic aim
                    UpdateAim(rrp, player.HeldItem.shootSpeed);

                    Projectile.netUpdate = true;
                    if (Projectile.timeLeft % 90 == 0 && strength < 5)
                    {
                        strength++;
                        Vector2 distance = new Vector2(0, 5);
                        DustHelper.DustRing(player.Center, DustType<Dusts.BlightDust>(), 5, 0, MathHelper.ToRadians(10 - (int)strength), .5f + .1f * strength, .5f + .1f * strength, 0, 0, default, true);
                        SoundEngine.PlaySound(SoundID.MaxMana, Projectile.Center);
                    }
                }

                // Move to swing
                else if (state == 0f)
                {
                    //Flip hand 180
                    player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();

                    SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                    state = 1f;
                    Projectile.timeLeft = 98;
                }

                for (int i = 0; i < strength; i++)
                {
                    //Create vector that is linearly dependant of the sword
                    Vector2 bladeLength = new Vector2(0, 1);
                    Vector2 blade = bladeLength.RotatedBy(Projectile.rotation + MathHelper.PiOver2);
                    blade *= Main.rand.NextFloat(Projectile.width / 2 - 30) + 30;
                    //dust along it
                    Dust.NewDust(player.Center + blade, 0, 0, DustType<Dusts.BlightDust>(), Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2), 0, default, Main.rand.NextFloat(.5f + (.3f * strength)));
                }
            }
            else if (state == 1f)
            {
                if (strength == 0f)
                    Projectile.Kill();
                if (Projectile.timeLeft % 3 == 0)
                {
                    Projectile.frame++;

                    //after frame is increased, so just as the final frame is reached
                    if (Projectile.frame == 6)
                    {
                        //Get vector to end of sword
                        Vector2 SwordTip = new Vector2(0, Projectile.width/2);
                        Vector2 hitSpot = SwordTip.RotatedBy(Projectile.rotation - MathHelper.PiOver2);
                        DustHelper.DustCircle(player.Center + hitSpot, DustType<Dusts.BlightDust>(), 8 + strength * 2, (strength + 1) * 20, .5f + .1f * strength, .5f + .1f * strength, 0, 0, default, true);

                        //ending
                        SoundEngine.PlaySound(SoundID.Item89, Projectile.Center);
                        state = 2f;
                    }
                }
            }
            else if (state == 2f)
            {
                state = 3f;
                Projectile.timeLeft = 30;
            }
        }

        private void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
        {
            Projectile.Center = playerHandPos;
            Projectile.rotation = Projectile.velocity.ToRotation();

            //Continuous holdout
            if (state == 0 || strength == 0)
                player.ChangeDir(-Projectile.direction);
            else //Flip player after swing
                player.ChangeDir(Projectile.direction);
            
            
            //player.ChangeDir(projectile.direction);

            player.heldProj = Projectile.whoAmI;
            player.itemTime = 15;
            player.itemAnimation = 15;

            //Hand direction
            player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
        }

        private void UpdateAim(Vector2 source, float speed)
        {
            Vector2 aim = Vector2.Normalize(Main.MouseWorld - source);
            if (aim.HasNaNs())
            {
                aim = -Vector2.UnitY;
            }

            // Slow turn to cursor
            aim = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), aim, AimResponsiveness));
            aim *= speed;

            if (aim != Projectile.velocity)
            {
                Projectile.netUpdate = true;
            }
            Projectile.velocity = aim;
        }

        public override Nullable<bool> CanDamage()/* tModPorter Suggestion: Return null instead of true *//* Suggestion: Return null instead of false */
        {
            return Projectile.frame == 6 && Projectile.timeLeft == 30;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            //Right half
            hitbox.Y += Projectile.height - Projectile.width/2;
            hitbox.Height = Projectile.width;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage *= (int)(strength + 2);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 SwordTip = new Vector2(0, Projectile.width / 2);
            Vector2 hitSpot = SwordTip.RotatedBy(Projectile.rotation - MathHelper.PiOver2);
            float _ = float.NaN;

            //Check collision of line from sword center to sword end with target hitbox
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + hitSpot, Projectile.height * Projectile.scale, ref _);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}

