using ExoriumMod.Core;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Weapons.Ranger
{
    class AcidOrb : ModItem
    {
        public override string Texture => AssetDirectory.RangerWeapon + Name;

        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("A glass sphere filled with an acidic substance" +
                "\nCovers hit enemies with acid"); */
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.useStyle = 5;
            Item.useAnimation = 32;
            Item.useTime = 32;
            Item.shootSpeed = 13f;
            Item.knockBack = 0.5f;
            Item.width = 32;
            Item.height = 32;
            Item.scale = 1f;
            Item.rare = 1;
            Item.value = Item.sellPrice(silver: 4);
            Item.consumable = true;
            Item.maxStack = 999;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true; 
            Item.noUseGraphic = true; 
            Item.autoReuse = true; 
            Item.UseSound = SoundID.Item1;
            Item.shoot = ProjectileType<AcidOrbProj>();
        }
    }

    class AcidOrbProj : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + "AcidOrb";

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 600;
        }

        private const int MAX_TICKS = 25;

        public float ticks
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            ticks++;
            if (ticks >= MAX_TICKS)
            {
                const float velXmult = 0.98f;
                const float velYmult = 0.35f;
                ticks = MAX_TICKS;
                Projectile.velocity.X *= velXmult;
                Projectile.velocity.Y += velYmult;
            }
            Projectile.rotation =
                Projectile.velocity.ToRotation() +
                MathHelper.ToRadians(90f);

        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 12; k++)
            {
                int dust = Dust.NewDust(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, 33, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f, 0, new Color(255, 110, 0));
            }
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<Buffs.CausticAcid>(), 900);
        }
    }
}
