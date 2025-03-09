using ExoriumMod.Core;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Weapons.Magic
{
    class DarksteelScepter : ModItem
    {
        public override string Texture => AssetDirectory.MagicWeapon + Name;

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            // Tooltip.SetDefault("Shoots bursts of homing skulls");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 40;
            Item.rare = 3;
            Item.value = Item.sellPrice(gold: 3, silver: 50);
            Item.DamageType = DamageClass.Magic;
            Item.mana = 25;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.shootSpeed = 14f;
            Item.autoReuse = true;
            Item.damage = 38;
            Item.shoot = ProjectileType<DarksteelSkullMagic>();
            Item.UseSound = SoundID.Item20;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numberProjectiles = 2 + Main.rand.Next(3); // 2 to 4 shots
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(20)); //degree spread.
                // Stagger difference
                float scale = 1f - (Main.rand.NextFloat() * .3f);
                perturbedSpeed = perturbedSpeed * scale; 
                int projectile = Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
            }
            return false; // return false because projectiles were already fired
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.DarksteelBar>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
    class DarksteelSkullMagic : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + "DarksteelSkull";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.CanDistortWater[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 800;
            Projectile.alpha = 255;
        }

        bool TargetReached
        {
            get => Projectile.ai[0] == 1;
            set => Projectile.ai[0] = value ? 1 : 0;
        }

        public override void AI()
        {
            if (Projectile.alpha != 0)
            {
                Projectile.alpha -= 15;
            }
            if (Projectile.localAI[0] == 0f)
            {
                AdjustMagnitude(ref Projectile.velocity);
                Projectile.localAI[0] = 1f;
            }
            Vector2 move = Vector2.Zero;
            float distance = 400f;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5)
                {
                    Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        distance = distanceTo;
                        target = true;
                        if (distanceTo < 20)
                            TargetReached = true;
                    }
                }
            }
            if (target && !TargetReached)
            {
                AdjustMagnitude(ref move);
                Projectile.velocity = (10 * Projectile.velocity + move) / 11f;
                AdjustMagnitude(ref Projectile.velocity);
            }
            if (Projectile.velocity != Vector2.Zero)
            {
                Projectile.spriteDirection = Projectile.direction;
                if (Projectile.velocity.X >= 0)
                {
                    Projectile.rotation = Projectile.velocity.ToRotation();
                }
                else
                {
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
                }
            }
            if (Main.rand.NextBool(3))
            {
                int offset = Main.rand.Next(-4, 4);
                new Vector2(Projectile.position.X + offset, Projectile.position.Y + offset);
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Dusts.DarksteelDust>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 12f)
            {
                vector *= 12f / magnitude;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Dusts.DarksteelDust>(), Projectile.oldVelocity.X * 1.5f, Projectile.oldVelocity.Y * 1.5f);
            }
            SoundEngine.PlaySound(SoundID.NPCDeath6);
        }
    }
}
