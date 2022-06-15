using ExoriumMod.Core;
using Terraria;
using Terraria.Audio;
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
            Item.staff[Item.type] = true;
            Tooltip.SetDefault("Bursts into blight specks on impact");
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 21;
            Item.useAnimation = 21;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<BlightShot>();
            Item.shootSpeed = 16f;
            Item.rare = 1;
            Item.value = Item.sellPrice(silver: 42);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.BlightsteelBar>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

    class BlightShot : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            Projectile.rotation += .5f;
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Dusts.BlightDust>(), 0, 0);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Dusts.BlightDust>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y);
            }
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, ProjectileType<BlightHail>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 2);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
    }
}
