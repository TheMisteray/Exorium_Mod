using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;

namespace ExoriumMod.Content.Items.Consumables.Scrolls
{
    class ScrollOfCloudOfDaggers : ModItem
    {
        public override string Texture => AssetDirectory.SpellScroll + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Casts Cloud of Daggers");
            DisplayName.SetDefault("Spell Scroll: Cloud of Daggers");
        }

        public override void SetDefaults()
        {
            item.damage = 15;
            item.useTime = 40;
            item.useAnimation = 40;
            item.rare = 2;
            item.useStyle = 4;
            item.value = Item.buyPrice(gold: 3, silver: 50);
            item.width = 32;
            item.height = 32;
            item.magic = true;
            item.mana = 20;
            item.maxStack = 30;
            item.noMelee = true;
            item.shootSpeed = 14f;
            item.autoReuse = false;
            item.shoot = ProjectileType<DaggerCloud>();
            item.consumable = true;
            item.UseSound = SoundID.Item7;
            item.noUseGraphic = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.HasBuff(BuffType<Buffs.ScrollCooldown>());
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return true;
        }

        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(BuffType<Buffs.ScrollCooldown>(), 3600);
        }
    }

    class DaggerCloud : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = false;
            projectile.magic = true;
            projectile.alpha = 255;
            projectile.timeLeft = 360;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
        }

        private int areaSize = 180;
        private int variance = 110;

        public override void AI()
        {
            if (projectile.timeLeft == 360)
            {
                Player player = Main.player[projectile.owner];
                projectile.position = player.Center - (player.Center - Main.MouseWorld);
                projectile.netUpdate = true;
            }
            else
                projectile.velocity = Vector2.Zero;
            if (projectile.timeLeft % 4 == 0 && projectile.timeLeft != 0)
            {
                int Xpos = Main.rand.Next(-areaSize, areaSize + 1);
                int Ypos = Main.rand.Next(-areaSize, areaSize + 1);
                while (Math.Sqrt(Math.Pow(Xpos, 2) + Math.Pow(Ypos, 2)) > areaSize && Math.Sqrt(Math.Pow(Xpos, 2) + Math.Pow(Ypos, 2)) < variance)
                {
                    Xpos = Main.rand.Next(-areaSize, areaSize + 1);
                    Ypos = Main.rand.Next(-areaSize, areaSize + 1);
                }
                Vector2 diff = (new Vector2(projectile.Center.X + Xpos, projectile.Center.Y + Ypos) - projectile.Center);
                float distance = diff.Length();
                distance = -Main.rand.NextFloat(5, 10) / distance;
                diff *= distance;
                diff = new Vector2(diff.X, diff.Y).RotatedByRandom(MathHelper.ToRadians(45));
                int proj1 = Projectile.NewProjectile(projectile.Center.X + Xpos, projectile.Center.Y + Ypos, diff.X, diff.Y, ProjectileType<DaggerCloudDagger>(), projectile.damage, 0, Main.myPlayer, areaSize + 35);
                Main.projectile[proj1].localAI[0] = projectile.Center.X;
                Main.projectile[proj1].localAI[1] = projectile.Center.Y;
            }
            for (int i = 0; i < 8; i++)
            {
                double rad = (Math.PI / 180) * Main.rand.NextFloat(361);
                int dust = Dust.NewDust(new Vector2(projectile.Center.X + (float)(Math.Cos(rad + 1.5) * (areaSize + 45)), projectile.Center.Y + (float)(Math.Sin(rad + 1.5) * (areaSize + 45))), 1, 1, 20, 0, 0, 0);
                //Main.dust[dust].scale *= 0.98f;
            }
        }
    }

    class DaggerCloudDagger : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + "DaggerCloud";

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.alpha = 20;
            projectile.timeLeft = 1600;
        }

        public override void AI()
        {
            if (projectile.velocity.X >= 0)
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(45);
            }
            else
            {
                projectile.rotation = projectile.velocity.ToRotation() - MathHelper.ToRadians(225);
            }
            projectile.spriteDirection = projectile.direction;
            if (Math.Sqrt(Math.Pow(projectile.localAI[0] - projectile.Center.X, 2) + Math.Pow(projectile.localAI[1] - projectile.Center.Y, 2)) > projectile.ai[0])
            {
                projectile.velocity.X /= 1.15f;
                projectile.velocity.Y /= 1.15f;
                projectile.alpha += 7;
                if (projectile.alpha >= 255)
                    projectile.Kill();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Dig, projectile.position);
            return true;
        }
    }
}
