using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace ExoriumMod.Content.Items.Weapons.Magic
{
    class BlightedStaff : ModItem
    {
        public override string Texture => AssetDirectory.MagicWeapon + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Withered Staff");
            Item.staff[item.type] = true;
            Tooltip.SetDefault("Bursts into blight specks on impact");
        }

        public override void SetDefaults()
        {
            item.damage = 16;
            item.magic = true;
            item.mana = 10;
            item.width = 40;
            item.height = 40;
            item.useTime = 21;
            item.useAnimation = 21;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 4;
            item.UseSound = SoundID.Item20;
            item.autoReuse = true;
            item.shoot = ProjectileType<BlightShot>();
            item.shootSpeed = 16f;
            item.rare = 1;
            item.value = Item.sellPrice(silver: 42);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Materials.Metals.BlightsteelBar>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    class BlightShot : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.timeLeft = 300;
        }

        public override void AI()
        {
            projectile.rotation += .5f;
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Dusts.BlightDust>(), 0, 0);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.Kill();
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Dusts.BlightDust>(), projectile.oldVelocity.X, projectile.oldVelocity.Y);
            }
            Projectile.NewProjectile(projectile.position.X, projectile.position.Y, projectile.oldVelocity.X, projectile.oldVelocity.Y, mod.ProjectileType("BlightHail"), projectile.damage, projectile.knockBack, projectile.owner, 2);
            Main.PlaySound(SoundID.Item10, projectile.position);
        }
    }
}
