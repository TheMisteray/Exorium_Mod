using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
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
            item.damage = 500;
            item.useStyle = 4;
            item.useTime = 42;
            item.useAnimation = 42;
            item.knockBack = 2;
            item.rare = 9;
            item.value = Item.buyPrice(gold: 18);
            item.width = 32;
            item.height = 32;
            item.maxStack = 30;
            item.magic = true;
            item.mana = 50;
            item.noMelee = true;
            item.shootSpeed = 16f;
            item.autoReuse = false;
            item.shoot = ProjectileType<DelayedBlastFireball>();
            item.consumable = true;
            item.UseSound = SoundID.Item7;
            item.useTurn = true;
            item.noUseGraphic = true;
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
        public override string Texture => "ExoriumMod/Projectiles/BlightShot";

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.extraUpdates = 100;
            projectile.timeLeft = 12000;
            projectile.penetrate = -1;
        }

        private bool stop = false;

        public override void AI()
        {
            if (!stop)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 projectilePosition = projectile.position;
                    projectilePosition -= projectile.velocity * ((float)i * 0.25f);
                    projectile.alpha = 255;
                    int dust = Dust.NewDust(projectilePosition, 1, 1, 6, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].position = projectilePosition;
                    Main.dust[dust].scale = (float)Main.rand.Next(70, 110) * 0.013f;
                    Main.dust[dust].velocity *= 0.2f;
                }
            }
            else
                projectile.velocity = Vector2.Zero;
            if (projectile.timeLeft <= 11920)
            {
                stop = true;
            }
            if (projectile.timeLeft < 3000 && projectile.timeLeft % 1000 == 0)
            {
                int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, 612, projectile.damage, projectile.knockBack, Main.myPlayer, 1, 10);
                Main.PlaySound(SoundID.Item14, projectile.position);
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

        public override bool CanDamage()
        {
            return !stop;
        }
    }
}
