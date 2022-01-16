using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Weapons.Magic
{
    class ShadowBolt : ModItem
    {
        public override string Texture => AssetDirectory.MagicWeapon + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots shadowbolts that split apart on impact");
        }

        public override void SetDefaults()
        {
            item.damage = 10;
            item.width = 15;
            item.height = 15;
            item.magic = true;
            item.mana = 9;
            item.useTime = 41;
            item.useAnimation = 41;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = Item.sellPrice(silver: 20);
            item.rare = 1;
            item.UseSound = SoundID.Item20;
            item.shoot = ProjectileType<ShadowBoltSpell>();
            item.shootSpeed = 20;
            item.autoReuse = true;
            item.scale = 0.9f;
        }
    }

    class ShadowBoltSpell : ModProjectile
    {
        public override string Texture => "ExoriumMod/Projectiles/BlightHail";

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.timeLeft = 600;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
            projectile.alpha = 255;
            projectile.ai[0] = 0;
        }

        public override void AI()
        {
            if (projectile.ai[0] == 0)
            {
                for (int i = 0; i < 6; i++)
                    Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Dusts.Shadow>());
            }
            else
            {
                projectile.scale -= 0.03f;
                projectile.width = (int)(projectile.width * projectile.scale);
                projectile.height = (int)(projectile.height * projectile.scale);
                projectile.velocity.Y += .5f;
                for (int i = 0; i < 2; i++)
                    Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Dusts.Shadow>());
                if (projectile.scale <= .01)
                    projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            if (projectile.ai[0] == 0)
            {
                Main.PlaySound(SoundID.Item14, projectile.position);
                for (int i = 0; i <= Main.rand.Next(3, 5); i++)
                {
                    Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(360)) / 2;
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<ShadowBoltSpell>(), projectile.damage, projectile.knockBack, Main.myPlayer, 1);
                }
            }
        }
    }
}
