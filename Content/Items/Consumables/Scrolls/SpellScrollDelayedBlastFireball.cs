using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Consumables.Scrolls
{
    class SpellScrollDelayedBlastFireball : ModItem
    {
        public override string Texture => AssetDirectory.SpellScroll + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Casts Delayed Blast Fireball");
            DisplayName.SetDefault("Spell Scroll: Delayed Blast Fireball");
        }

        public override void SetDefaults()
        {
            Item.damage = 500;
            Item.useStyle = 4;
            Item.useTime = 42;
            Item.useAnimation = 42;
            Item.knockBack = 2;
            Item.rare = 9;
            Item.value = Item.buyPrice(gold: 18);
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 30;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 50;
            Item.noMelee = true;
            Item.shootSpeed = 16f;
            Item.autoReuse = false;
            Item.shoot = ProjectileType<DelayedBlastFireball>();
            Item.consumable = true;
            Item.UseSound = SoundID.Item7;
            Item.useTurn = true;
            Item.noUseGraphic = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.HasBuff(BuffType<Buffs.ScrollCooldown>());
        }

        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(BuffType<Buffs.ScrollCooldown>(), 12600);
        }
    }

    class DelayedBlastFireball : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 12000;
            Projectile.penetrate = -1;
        }

        private bool stop = false;

        public override void AI()
        {
            if (!stop)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 projectilePosition = Projectile.position;
                    projectilePosition -= Projectile.velocity * ((float)i * 0.25f);
                    Projectile.alpha = 255;
                    int dust = Dust.NewDust(projectilePosition, 1, 1, 6, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].position = projectilePosition;
                    Main.dust[dust].scale = (float)Main.rand.Next(70, 110) * 0.013f;
                    Main.dust[dust].velocity *= 0.2f;
                }
            }
            else
                Projectile.velocity = Vector2.Zero;
            if (Projectile.timeLeft <= 11920)
            {
                stop = true;
            }
            if (Projectile.timeLeft < 3000 && Projectile.timeLeft % 1000 == 0)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, 612, Projectile.damage, Projectile.knockBack, Main.myPlayer, 1, 10);
                SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
                Main.projectile[proj].position = Main.projectile[proj].Center;
                Main.projectile[proj].width *= 60;
                Main.projectile[proj].height *= 60;
                Main.projectile[proj].Center = Main.projectile[proj].position;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            stop = true;
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            stop = true;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = 0;
            knockback = 0;
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            stop = true;
        }

        public override bool? CanDamage()/* Suggestion: Return null instead of false */
        {
            return !stop;
        }
    }
}
