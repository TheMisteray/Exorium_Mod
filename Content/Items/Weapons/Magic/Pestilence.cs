using ExoriumMod.Core;
using Terraria;
using Terraria.GameContent.Creative;
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
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 17;
            Item.width = 15;
            Item.height = 15;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 6;
            Item.useTime = 12;
            Item.useAnimation = 24;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = Item.sellPrice(silver: 68); ;
            Item.rare = 2;
            Item.UseSound = SoundID.Item42;
            Item.shoot = ProjectileType<BlightHail>();
            Item.shootSpeed = 20;
            Item.autoReuse = true;
            Item.scale = 0.9f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.BlightsteelBar>(), 12);
            recipe.AddIngredient(ItemType<Materials.TaintedGel>(), 6);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }
    }
    class BlightHail : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.alpha = 255;
            Projectile.timeLeft = 30;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            for (int i = 0; i < 4; i++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Dusts.BlightDust>(), Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f);
            }
            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Dusts.BlightDust>(), Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f);
            }
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Dusts.BlightDust>(), 0, 0);
            }
            if (Main.rand.NextBool(4))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Dusts.BlightDust>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
            if (Projectile.ai[0] == 2)
            {
                Projectile.timeLeft -= 2;
                Projectile.position = Projectile.Center;
                Projectile.scale *= 1.08f;
                Projectile.Center = Projectile.position;
            }
        }
    }
}
