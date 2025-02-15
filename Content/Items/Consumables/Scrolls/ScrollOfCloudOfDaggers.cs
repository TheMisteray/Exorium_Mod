using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Consumables.Scrolls
{
    class ScrollOfCloudOfDaggers : ModItem
    {
        public override string Texture => AssetDirectory.SpellScroll + Name;

        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Casts Cloud of Daggers");
            // DisplayName.SetDefault("Spell Scroll: Cloud of Daggers");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
        }

        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.rare = 2;
            Item.useStyle = 4;
            Item.value = Item.buyPrice(gold: 3, silver: 50);
            Item.width = 32;
            Item.height = 32;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.maxStack = 30;
            Item.noMelee = true;
            Item.shootSpeed = 14f;
            Item.autoReuse = false;
            Item.shoot = ProjectileType<DaggerCloud>();
            Item.consumable = true;
            Item.UseSound = SoundID.Item7;
            Item.noUseGraphic = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.HasBuff(BuffType<Buffs.ScrollCooldown>());
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
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.alpha = 255;
            Projectile.timeLeft = 360;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }

        private int areaSize
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        private int variance
        {
            get => (int) Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

    public override void AI()
        {
            if (Projectile.timeLeft == 360)
            {
                areaSize = 180;
                variance = 110;
                Player player = Main.player[Projectile.owner];
                Projectile.position = player.Center - (player.Center - Main.MouseWorld);
                Projectile.netUpdate = true;
            }
            else
                Projectile.velocity = Vector2.Zero;
            if (Projectile.timeLeft % 4 == 0 && Projectile.timeLeft != 0)
            {
                int Xpos = Main.rand.Next(-areaSize, areaSize + 1);
                int Ypos = Main.rand.Next(-areaSize, areaSize + 1);
                while (Math.Sqrt(Math.Pow(Xpos, 2) + Math.Pow(Ypos, 2)) > areaSize && Math.Sqrt(Math.Pow(Xpos, 2) + Math.Pow(Ypos, 2)) < variance)
                {
                    Xpos = Main.rand.Next(-areaSize, areaSize + 1);
                    Ypos = Main.rand.Next(-areaSize, areaSize + 1);
                }
                Vector2 diff = (new Vector2(Projectile.Center.X + Xpos, Projectile.Center.Y + Ypos) - Projectile.Center);
                float distance = diff.Length();
                distance = -Main.rand.NextFloat(5, 10) / distance;
                diff *= distance;
                diff = new Vector2(diff.X, diff.Y).RotatedByRandom(MathHelper.ToRadians(45));
                int proj1 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X + Xpos, Projectile.Center.Y + Ypos, diff.X, diff.Y, ProjectileType<DaggerCloudDagger>(), Projectile.damage, 0, Main.myPlayer, areaSize + 35);
                Main.projectile[proj1].localAI[0] = Projectile.Center.X;
                Main.projectile[proj1].localAI[1] = Projectile.Center.Y;
            }
            for (int i = 0; i < 8; i++)
            {
                double rad = (Math.PI / 180) * Main.rand.NextFloat(361);
                int dust = Dust.NewDust(new Vector2(Projectile.Center.X + (float)(Math.Cos(rad + 1.5) * (areaSize + 90)), Projectile.Center.Y + (float)(Math.Sin(rad + 1.5) * (areaSize + 90))), 1, 1, 20, 0, 0, 0);
                //Main.dust[dust].scale *= 0.98f;
            }
        }
    }

    class DaggerCloudDagger : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + "DaggerCloud";

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.alpha = 20;
            Projectile.timeLeft = 1600;
        }

        public override void AI()
        {
            if (Projectile.velocity.X >= 0)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45);
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.ToRadians(225);
            }
            Projectile.spriteDirection = Projectile.direction;
            if (Math.Sqrt(Math.Pow(Projectile.localAI[0] - Projectile.Center.X, 2) + Math.Pow(Projectile.localAI[1] - Projectile.Center.Y, 2)) > Projectile.ai[0])
            {
                Projectile.velocity.X /= 1.15f;
                Projectile.velocity.Y /= 1.15f;
                Projectile.alpha += 7;
                if (Projectile.alpha >= 255)
                    Projectile.Kill();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            return true;
        }
    }
}
