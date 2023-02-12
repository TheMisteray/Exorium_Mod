using ExoriumMod.Core;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Weapons.Magic
{
    class SettlingSands : ModItem
    {
        public override string Texture => AssetDirectory.MagicWeapon + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Launches bouncing sand balls \n" +
                "\"This is a horrible idea\"");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.width = 15;
            Item.height = 15;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 8;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = Item.sellPrice(silver: 14);
            Item.rare = 1;
            Item.UseSound = SoundID.Item42;
            Item.shoot = ProjectileType<SandShot>();
            Item.shootSpeed = 7;
            Item.autoReuse = true;
            Item.scale = 0.9f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.DunestoneBar>(), 8);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }
    }
    class SandShot : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bouncing Sand");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.SandBallGun);
            AIType = ProjectileID.SandBallGun;
            Projectile.aiStyle = 1;
            Projectile.ai[1] = 0;
        }

        public float projectileBounce
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Player player = Main.player[Projectile.owner];
            projectileBounce++;
            if (projectileBounce >= 3)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, 0, 0, ProjectileID.SandBallFalling, 5, 5, player.whoAmI);
                Projectile.Kill();
            }
            else
            {
                Projectile.ai[0] += 0.1f;
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X / 1.5f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y / 1.5f;
                }
                Projectile.velocity *= 0.75f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, 0, 0, ProjectileID.SandBallFalling, 5, 5, player.whoAmI);
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            }
            return false;
        }
    }
}
