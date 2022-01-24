using ExoriumMod.Core;
using ExoriumMod.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
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
            item.damage = 36;
            item.width = 32;
            item.height = 24;
            item.magic = true;
            item.mana = 10;
            item.useTime = 42;
            item.useAnimation = 42;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = Item.sellPrice(silver: 48);
            item.rare = 1;
            item.UseSound = SoundID.Item75;
            item.shoot = ProjectileType<EnergyRing>();
            item.shootSpeed = 2;
            item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MeteoriteBar, 24);
            recipe.AddIngredient(ItemID.HellstoneBar, 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
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
            projectile.width = 40;
            projectile.height = 40;
            projectile.alpha = 255;
            projectile.timeLeft = 1200;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.extraUpdates = 9;
        }

        public override void AI()
        {
            DustHelper.DustRing(projectile.Center, DustType<Dusts.Rainbow>(), projectile.width * projectile.scale / 2, 0, .14f, .16f, 0, 0, 0, Color.Lime, false);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            projectile.position = projectile.Center;
            projectile.scale -= .25f;
            projectile.Center = projectile.position;
            if (projectile.scale <= 0)
                projectile.Kill();
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.velocity.X = -oldVelocity.X;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.velocity.Y = -oldVelocity.Y;
            }
            projectile.position = projectile.Center;
            projectile.scale -= .25f;
            projectile.Center = projectile.position;
            if (projectile.scale <= 0)
                projectile.Kill();
            return false;
        }
    }
}
