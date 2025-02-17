using ExoriumMod.Core;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using ExoriumMod.Helpers;

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
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.rare = 3;
            Item.damage = 29;
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
            if (player.altFunctionUse == 2)
            {
                Item.useTime = 14;
                Item.useAnimation = 14;
                Item.UseSound = SoundID.Item34;
            }
            else
            {
                Item.mana = 0;
                Item.useTime = 32;
                Item.useAnimation = 32;
                Item.UseSound = SoundID.Item109;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                for (int i = 0; i < 5; i++)
                {
                    Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(-(Main.rand.NextFloat(5) + 8) * 2 + (Main.rand.NextFloat(5) + 8) * i));

                    Vector2 muzzleOffset = Vector2.Normalize(perturbedSpeed) * 75f; //Weapon looked weird when these spawned on player, so muzzle offset was added
                    if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                    {
                        position += muzzleOffset;
                    }

                    Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<FireCone>(), (int)(damage/4f), knockback, player.whoAmI, 0, 1);
                }
            }
            else
            {
                Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(15)); //degree spread. // Stagger difference
                float scale = 1f - (Main.rand.NextFloat() * .3f);
                perturbedSpeed = perturbedSpeed * scale;
                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HellstoneBar, 18);
            recipe.AddIngredient(ItemID.MeteoriteBar, 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

    class FireCone : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.timeLeft = 20;
            Projectile.penetrate = 3;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f; //Moved from FireDust to here to avoid extra calls since FireDust is called more than once per tick
            if (Projectile.wet && !Projectile.lavaWet) //water death
                Projectile.active = false;

            Projectile.alpha = 255;
            Projectile.velocity = Projectile.velocity * 0.92f;
            DustHelper.FireDust(Projectile);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width /= 2;
            height /= 2;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        //public override bool OnTileCollide(Vector2 oldVelocity)
        //{
        //    Projectile.Kill();
        //    return false;
        //}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 900);
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
            Projectile.timeLeft = 25;
            Projectile.penetrate = 1;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(3))
                DustHelper.FireDust(Projectile);
            Projectile.ai[0] += 1f; //Moved from FireDust to here to avoid extra calls since FireDust is called more than once per tick
            if (Projectile.wet && !Projectile.lavaWet) //water death
                Projectile.active = false;

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

        //public override bool OnTileCollide(Vector2 oldVelocity)
        //{
        //    Projectile.Kill();
        //    return false;
        //}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.HasBuff(BuffID.OnFire)) hit.Damage *= 5; //damage increase against burning targets
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.HasBuff(BuffID.OnFire)) modifiers.FinalDamage *= 1.5f;
               base.ModifyHitNPC(target, ref modifiers);
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.velocity *= 0.4f;
            for (int i = 0; i < 25; i++)
            {
                Vector2 randDir = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(360));
                DustHelper.FireDust(Projectile);
            }
            SoundEngine.PlaySound(SoundID.Item89, Projectile.position);
        }
    }
}
