using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace ExoriumMod.Content.Items.Weapons.Ranger
{
    class AcidOrb : ModItem
    {
        public override string Texture => AssetDirectory.RangerWeapon + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A glass sphere filled with an acidic substance" +
                "\nCovers hit enemies with acid");
        }

        public override void SetDefaults()
        {
            item.damage = 20;
            item.useStyle = 5;
            item.useAnimation = 32;
            item.useTime = 32;
            item.shootSpeed = 13f;
            item.knockBack = 6.5f;
            item.width = 32;
            item.height = 32;
            item.scale = 1f;
            item.rare = 1;
            item.value = Item.sellPrice(silver: 4);
            item.consumable = true;
            item.maxStack = 999;
            item.ranged = true;
            item.noMelee = true; 
            item.noUseGraphic = true; 
            item.autoReuse = true; 
            item.UseSound = SoundID.Item1;
            item.shoot = ProjectileType<AcidOrbProj>();
        }
    }

    class AcidOrbProj : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + "AcidOrb";

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.penetrate = 3;
            projectile.timeLeft = 600;
        }

        private const int MAX_TICKS = 25;

        public float ticks
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public override void AI()
        {
            ticks++;
            if (ticks >= MAX_TICKS)
            {
                const float velXmult = 0.98f;
                const float velYmult = 0.35f;
                ticks = MAX_TICKS;
                projectile.velocity.X *= velXmult;
                projectile.velocity.Y += velYmult;
            }
            projectile.rotation =
                projectile.velocity.ToRotation() +
                MathHelper.ToRadians(90f);

        }

        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 12; k++)
            {
                int dust = Dust.NewDust(projectile.position - projectile.velocity, projectile.width, projectile.height, 33, projectile.oldVelocity.X * 0.5f, projectile.oldVelocity.Y * 0.5f, 0, new Color(255, 110, 0));
            }
            Main.PlaySound(SoundID.Item27, projectile.position);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<Buffs.CausticAcid>(), 900);
        }
    }
}
