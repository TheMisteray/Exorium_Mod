using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using System;

namespace ExoriumMod.Content.Items.Weapons.Melee
{
    public class MorditeBlade : ModItem
    {
        public override string Texture => AssetDirectory.MeleeWeapon + Name;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots a homing skull");
        }

        public override void SetDefaults()
        {
            item.damage = 38;
            item.melee = true;
            item.width = 48;
            item.height = 48;
            item.useTime = 32;
            item.useAnimation = 32;
            item.useStyle = 1;
            item.knockBack = 7;
            item.value = Item.sellPrice(gold: 3, silver: 50); ;
            item.rare = 3;
            item.UseSound = SoundID.Item8;
            item.autoReuse = true;
            item.scale = 1.3f;
            item.shoot = ProjectileType<MorditeSkullMelee>();
            item.shootSpeed = 12f;
            item.useTurn = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("MorditeBar"), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20)); //degree spread.
            // Stagger difference
            float scale = 1f - (Main.rand.NextFloat() * .3f);
            perturbedSpeed = perturbedSpeed * scale;
            int projectile = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            Main.projectile[projectile].magic = true;
            return false; // return false because projectiles were already fired
        }
    }

    class MorditeSkullMelee : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.CanDistortWater[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.width = 26;
            projectile.height = 26;
            projectile.melee = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 800;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            if (projectile.alpha != 0)
            {
                projectile.alpha -= 15;
            }
            if (projectile.localAI[0] == 0f)
            {
                AdjustMagnitude(ref projectile.velocity);
                projectile.localAI[0] = 1f;
            }
            Vector2 move = Vector2.Zero;
            float distance = 400f;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5)
                {
                    Vector2 newMove = Main.npc[k].Center - projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        distance = distanceTo;
                        target = true;
                    }
                }
            }
            if (target)
            {
                AdjustMagnitude(ref move);
                projectile.velocity = (10 * projectile.velocity + move) / 11f;
                AdjustMagnitude(ref projectile.velocity);
            }
            if (projectile.velocity != Vector2.Zero)
            {
                projectile.spriteDirection = projectile.direction;
                if (projectile.velocity.X >= 0)
                {
                    projectile.rotation = projectile.velocity.ToRotation();
                }
                else
                {
                    projectile.rotation = projectile.velocity.ToRotation() + MathHelper.Pi;
                }
            }
            if (Main.rand.NextBool(3))
            {
                int offset = Main.rand.Next(-4, 4);
                new Vector2(projectile.position.X + offset, projectile.position.Y + offset);
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Dusts.MorditeSpecks>(), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
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

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Dusts.MorditeSpecks>(), projectile.oldVelocity.X * 1.5f, projectile.oldVelocity.Y * 1.5f);
            }
            Main.PlaySound(SoundID.NPCDeath6);
        }
    }
}

