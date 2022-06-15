using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Weapons.Melee
{
    class FrostedBoomerang : ModItem
    {
        public override string Texture => AssetDirectory.MeleeWeapon + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frosted Chackram");
            Tooltip.SetDefault("Inflicts Frostburn");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WoodenBoomerang);
            Item.shoot = ProjectileType<RimeBoomerang>();
            Item.damage = 15;
            Item.useTime = 34;
            Item.useAnimation = 34;
            Item.autoReuse = true;
            Item.rare = 1;
            Item.value = Item.sellPrice(silver: 14);
            Item.UseSound = SoundID.Item1;
            Item.shootSpeed = 18;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.RimestoneBar>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

    }

    class RimeBoomerang : ModProjectile
    {
        public override string Texture => AssetDirectory.MeleeWeapon + "FrostedBoomerang";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frosted Chackram");
        }

        public override void SetDefaults()
        {
            Projectile.timeLeft = 30;
            Projectile.height = 38;
            Projectile.width = 38;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }

        public override void AI()
        {
            Projectile.rotation++;
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 67, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 300, true);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 300, true);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i <= 9; i++)
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 67, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
        }
    }
}
