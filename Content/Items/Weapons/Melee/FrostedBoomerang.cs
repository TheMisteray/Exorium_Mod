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
            item.CloneDefaults(ItemID.WoodenBoomerang);
            item.shoot = ProjectileType<RimeBoomerang>();
            item.damage = 15;
            item.useTime = 34;
            item.useAnimation = 34;
            item.autoReuse = true;
            item.rare = 1;
            item.value = Item.sellPrice(silver: 14);
            item.UseSound = SoundID.Item1;
            item.shootSpeed = 18;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("RimestoneBar"), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
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
            projectile.timeLeft = 30;
            projectile.height = 38;
            projectile.width = 38;
            projectile.friendly = true;
            projectile.hostile = false;
        }

        public override void AI()
        {
            projectile.rotation++;
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 67, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
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
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 67, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
        }
    }
}
