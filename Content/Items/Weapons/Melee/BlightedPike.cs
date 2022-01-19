using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using ExoriumMod.Helpers;

namespace ExoriumMod.Content.Items.Weapons.Melee
{
    class BlightedPike : ModItem
    {
        public override string Texture => AssetDirectory.MeleeWeapon + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mildew");
            Tooltip.SetDefault("Launches specks of blight");
        }

        public override void SetDefaults()
        {
            item.damage = 22;
            item.melee = true;
            item.width = 80;
            item.height = 80;
            item.useTime = 36;
            item.shootSpeed = 3.7f;
            item.useAnimation = 20;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = Item.sellPrice(silver: 68);
            item.rare = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.shoot = ProjectileType<BlightedPikeProj>();
        }


        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[item.shoot] < 1;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX * 3, speedY * 3, mod.ProjectileType("BlightHail"), damage, knockBack, player.whoAmI); //fires additional projectile
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Materials.Metals.BlightsteelBar>(), 12);
            recipe.AddIngredient(ItemType<Materials.TaintedGel>(), 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
    class BlightedPikeProj : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.aiStyle = 19;
            projectile.penetrate = -1;
            projectile.scale = 1.3f;
            projectile.alpha = 0;
            projectile.hide = true;
            projectile.ownerHitCheck = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.friendly = true;
        }

        public float movementFactor //Speed of attack
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public override void AI()
        {
            Player spearUser = Main.player[projectile.owner];
            Vector2 ownerMountedCenter = spearUser.RotatedRelativePoint(spearUser.MountedCenter, true);
            projectile.direction = spearUser.direction;
            spearUser.heldProj = projectile.whoAmI;
            spearUser.itemTime = spearUser.itemAnimation;
            projectile.position.X = ownerMountedCenter.X - (float)(projectile.width / 2);
            projectile.position.Y = ownerMountedCenter.Y - (float)(projectile.height / 2);
            if (!spearUser.frozen) //prevents spear from moving is owner is frozen
            {
                if (movementFactor == 0f)
                {
                    movementFactor = 3f; // Forward
                    projectile.netUpdate = true;
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

            projectile.position += projectile.velocity * movementFactor;

            if (spearUser.itemAnimation == 0)
            {
                projectile.Kill();
            }

            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);

            if (projectile.spriteDirection == -1)
            {
                projectile.rotation -= MathHelper.ToRadians(90f);
            }

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.height, projectile.width, DustType<Dusts.BlightDust>(), projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 200, Scale: 1.1f);
                dust.velocity += projectile.velocity * 0.2f;
                dust.velocity *= 0.2f;
            }
        }
    }

    class BlightHailMelee : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40;
            projectile.alpha = 255;
            projectile.timeLeft = 30;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 0.7f;
        }

        public override void AI()
        {
            //projectile.velocity.Y += projectile.ai[0];
            for (int i = 0; i < 4; i++)
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Dusts.BlightDust>(), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            }
        }
    }
}
