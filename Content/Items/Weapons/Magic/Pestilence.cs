using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Weapons.Magic
{
    class Pestilence : ModItem
    {
        public override string Texture => AssetDirectory.MagicWeapon + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tome of Pestilence");
            Tooltip.SetDefault("Shoots a trail of blight specks");
        }

        public override void SetDefaults()
        {
            item.damage = 17;
            item.width = 15;
            item.height = 15;
            item.magic = true;
            item.mana = 6;
            item.useTime = 12;
            item.useAnimation = 24;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = Item.sellPrice(silver: 68); ;
            item.rare = 2;
            item.UseSound = SoundID.Item42;
            item.shoot = ProjectileType<BlightHail>();
            item.shootSpeed = 20;
            item.autoReuse = true;
            item.scale = 0.9f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("BlightsteelBar"), 12);
            recipe.AddIngredient(mod.GetItem("TaintedGel"), 6);
            recipe.AddTile(TileID.Bookcases);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
    class BlightHail : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            projectile.width = 80;
            projectile.height = 80;
            projectile.alpha = 255;
            projectile.timeLeft = 30;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(1))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Dusts.BlightDust>(), projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f);
            }
            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Dusts.BlightDust>(), projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f);
            }
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Dusts.BlightDust>(), 0, 0);
            }
            if (Main.rand.NextBool(4))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Dusts.BlightDust>(), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            }
            if (projectile.ai[0] == 2)
            {
                projectile.timeLeft -= 2;
                projectile.position = projectile.Center;
                projectile.scale *= 1.08f;
                projectile.Center = projectile.position;
            }
        }
    }
}
