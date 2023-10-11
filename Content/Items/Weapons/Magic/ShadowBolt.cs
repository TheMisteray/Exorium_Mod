using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
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
            // Tooltip.SetDefault("Shoots shadowbolts that split apart on impact");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.width = 15;
            Item.height = 15;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 9;
            Item.useTime = 41;
            Item.useAnimation = 41;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = Item.sellPrice(silver: 20);
            Item.rare = 1;
            Item.UseSound = SoundID.Item20;
            Item.shoot = ProjectileType<ShadowBoltSpell>();
            Item.shootSpeed = 20;
            Item.autoReuse = true;
            Item.scale = 0.9f;
        }
    }

    class ShadowBoltSpell : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            for (int i = 0; i < 6; i++)
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Dusts.Shadow>());
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = 0; i <= Main.rand.Next(3, 5); i++)
                {
                    Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(360)) / 2;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<ShadowBoltSpellShard>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 1);
                }
            }
        }
    }

    class ShadowBoltSpellShard : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.alpha = 255;
            Projectile.ai[0] = 0;
        }

        public override void AI()
        {
            Projectile.scale -= 0.03f;
            Projectile.width = (int)(Projectile.width * Projectile.scale);
            Projectile.height = (int)(Projectile.height * Projectile.scale);
            Projectile.velocity.Y += .5f;
            for (int i = 0; i < 2; i++)
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Dusts.Shadow>());
            if (Projectile.scale <= .01)
                Projectile.Kill();
        }
    }
}
