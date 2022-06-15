using ExoriumMod.Core;
using ExoriumMod.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Weapons.Magic
{
    class PulseRinger : ModItem
    {
        public override string Texture => AssetDirectory.MagicWeapon + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots rings of energy");
        }

        public override void SetDefaults()
        {
            Item.damage = 36;
            Item.width = 32;
            Item.height = 24;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.useTime = 42;
            Item.useAnimation = 42;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = Item.sellPrice(silver: 48);
            Item.rare = 1;
            Item.UseSound = SoundID.Item75;
            Item.shoot = ProjectileType<EnergyRing>();
            Item.shootSpeed = 2;
            Item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MeteoriteBar, 24);
            recipe.AddIngredient(ItemID.HellstoneBar, 6);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
    }

    class EnergyRing : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.alpha = 255;
            Projectile.timeLeft = 1200;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 9;
        }

        public override void AI()
        {
            DustHelper.DustRing(Projectile.Center, DustType<Dusts.Rainbow>(), Projectile.width * Projectile.scale / 2, 0, .14f, .16f, 0, 0, 0, Color.Lime, false);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Projectile.position = Projectile.Center;
            Projectile.scale -= .25f;
            Projectile.Center = Projectile.position;
            if (Projectile.scale <= 0)
                Projectile.Kill();
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            Projectile.position = Projectile.Center;
            Projectile.scale -= .25f;
            Projectile.Center = Projectile.position;
            if (Projectile.scale <= 0)
                Projectile.Kill();
            return false;
        }
    }
}
