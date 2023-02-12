using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using ExoriumMod.Helpers;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Weapons.Melee
{
    class BlightedPike : ModItem
    {
        public override string Texture => AssetDirectory.MeleeWeapon + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mildew");
            Tooltip.SetDefault("Launches specks of blight");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 22;
            Item.DamageType = DamageClass.Melee;
            Item.width = 80;
            Item.height = 80;
            Item.useTime = 36;
            Item.shootSpeed = 3.7f;
            Item.useAnimation = 20;
            Item.useStyle = 5;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(silver: 68);
            Item.rare = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ProjectileType<BlightedPikeProj>();
        }


        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X * 3, velocity.Y * 3, Mod.Find<ModProjectile>("BlightHail").Type, damage, knockback, player.whoAmI); //fires additional projectile
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.BlightsteelBar>(), 12);
            recipe.AddIngredient(ItemType<Materials.TaintedGel>(), 6);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
    class BlightedPikeProj : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 19;
            Projectile.penetrate = -1;
            Projectile.scale = 1.3f;
            Projectile.alpha = 0;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }

        public float movementFactor //Speed of attack
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            Player spearUser = Main.player[Projectile.owner];
            Vector2 ownerMountedCenter = spearUser.RotatedRelativePoint(spearUser.MountedCenter, true);
            Projectile.direction = spearUser.direction;
            spearUser.heldProj = Projectile.whoAmI;
            spearUser.itemTime = spearUser.itemAnimation;
            Projectile.position.X = ownerMountedCenter.X - (float)(Projectile.width / 2);
            Projectile.position.Y = ownerMountedCenter.Y - (float)(Projectile.height / 2);
            if (!spearUser.frozen) //prevents spear from moving is owner is frozen
            {
                if (movementFactor == 0f)
                {
                    movementFactor = 3f; // Forward
                    Projectile.netUpdate = true;
                }
                if (spearUser.itemAnimation < spearUser.itemAnimationMax / 3) // Back
                {
                    movementFactor -= 2.4f;
                }
                else // Other
                {
                    movementFactor += 2.1f;
                }
            }

            Projectile.position += Projectile.velocity * movementFactor;

            if (spearUser.itemAnimation == 0)
            {
                Projectile.Kill();
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);

            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation -= MathHelper.ToRadians(90f);
            }

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, DustType<Dusts.BlightDust>(), Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 200, Scale: 1.1f);
                dust.velocity += Projectile.velocity * 0.2f;
                dust.velocity *= 0.2f;
            }
        }
    }

    class BlightHailMelee : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.alpha = 255;
            Projectile.timeLeft = 30;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 0.7f;
        }

        public override void AI()
        {
            //projectile.velocity.Y += projectile.ai[0];
            for (int i = 0; i < 4; i++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Dusts.BlightDust>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }
    }
}
