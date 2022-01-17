using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using System.IO;

namespace ExoriumMod.Content.Items.Weapons.Ranger
{
    class ThrowingRapier : ModItem
    {
        public override string Texture => AssetDirectory.RangerWeapon + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("\"Anything works if you roll a 20\"\n" +
                "If the tip of the rapier hits the enemy it will stick\n" +
                "Otherwise it will bounce");
        }

        public override void SetDefaults()
        {
            item.damage = 14;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useAnimation = 28;
            item.useTime = 28;
            item.shootSpeed = 17f;
            item.knockBack = 1f;
            item.width = 48;
            item.height = 48;
            item.scale = 1f;
            item.rare = 0;
            item.value = Item.sellPrice(silver: 4);
            item.consumable = true;
            item.maxStack = 9999;
            item.ranged = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
            item.shoot = ProjectileType<ThrowingRapierProj>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float throwSpeed = Main.rand.NextFloat(.8f, 1.2f);
            Projectile.NewProjectile(position.X, position.Y, speedX * throwSpeed, speedY * throwSpeed, ProjectileType<ThrowingRapierProj>(), damage, knockBack, player.whoAmI, Main.rand.NextFloat(.1f, .8f));
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 10);
            recipe.AddIngredient(ItemID.FallenStar);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 200);
            recipe.AddRecipe();
        }
    }

    class ThrowingRapierProj : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetDefaults()
        {
            projectile.width = 62;
            projectile.height = 62;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.penetrate = 3;
            projectile.timeLeft = 600;
        }

        private const int FALL_TIME = 580;
        private const int MAX_STUCK = 3;
        private readonly Point[] _sticking = new Point[MAX_STUCK];

        public float rotationSpeed
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public float stuckTargetWhoAmI
        {
            get => projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        public bool IsStickingToTarget = false;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(IsStickingToTarget);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            IsStickingToTarget = reader.ReadBoolean();
        }

        public override void AI()
        {
            if (IsStickingToTarget)
                StuckAI();
            else
                NormalAI();
        }

        public void StuckAI()
        {
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;

            projectile.localAI[0]++;

            bool hit = projectile.localAI[0] % 30f == 0f;

            const int aiFactor = 300;

            // Every 30 ticks, the javelin will perform a hit effect
            bool hitEffect = projectile.localAI[0] % 30f == 0f;
            int projTargetIndex = (int)stuckTargetWhoAmI;
            if (projectile.localAI[0] >= 60 * aiFactor || projTargetIndex < 0 || projTargetIndex >= 200)
            {
                projectile.Kill();
            }
            else if (Main.npc[projTargetIndex].active && !Main.npc[projTargetIndex].dontTakeDamage)
            { // If the target is active and can take damage
                projectile.Center = Main.npc[projTargetIndex].Center - projectile.velocity * 2f;
                projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;
                if (hitEffect)
                { //So it stays stuck
                    Main.npc[projTargetIndex].HitEffect(0, 1.0);
                }
            }
            else
            { // Otherwise, kill the projectile
                projectile.Kill();
            }
        }

        public void NormalAI()
        {
            if (projectile.timeLeft <= FALL_TIME)
            {
                const float velXmult = 0.98f;
                const float velYmult = 0.35f;
                projectile.velocity.X *= velXmult;
                projectile.velocity.Y += velYmult;
            }
            projectile.rotation += (float)rotationSpeed;
        }

        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 6; k++)
            {
                int dust = Dust.NewDust(projectile.position - projectile.velocity, projectile.width, projectile.height, DustID.Lead, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f);
            }
            Main.PlaySound(SoundID.Dig, projectile.position);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Main.PlaySound(SoundID.Dig, projectile.position);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            //line from proj center out along blade
            Vector2 line = new Vector2(0, projectile.width);
            Vector2 rotatedLine = line.RotatedBy(projectile.rotation - MathHelper.PiOver2);
            if (Collision.CheckAABBvLineCollision(target.position, new Vector2(target.width, target.height), projectile.Center, projectile.Center + rotatedLine))
            {
                IsStickingToTarget = true;
                stuckTargetWhoAmI = target.whoAmI; // Set the target whoAmI
                projectile.velocity =
                    (target.Center - projectile.Center) *
                    0.75f; // Change velocity based on delta center of targets (difference between entity centers)
                projectile.netUpdate = true; // netUpdate
                target.AddBuff(BuffType<Buffs.ThrowingRapierDamage>(), 900); // DOT

                projectile.damage = 0; // Doesn't deal damage anymore

                UpdateStuck(target);
            }
            else
            {
                projectile.velocity *= -1;
                Vector2 wildBounce = projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-20, 20)));
                projectile.velocity = wildBounce;
                rotationSpeed *= Main.rand.NextFloat(.7f, 1.3f);
            }
        }

        private void UpdateStuck(NPC target)
        {
            int currentIndex = 0;

            for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
            {
                Projectile currentProjectile = Main.projectile[i];
                if (i != projectile.whoAmI 
                    && currentProjectile.active 
                    && currentProjectile.owner == Main.myPlayer 
                    && currentProjectile.type == projectile.type 
                    && currentProjectile.modProjectile is ThrowingRapierProj proj 
                    && proj.IsStickingToTarget 
                    && proj.stuckTargetWhoAmI == target.whoAmI)
                {

                    _sticking[currentIndex++] = new Point(i, currentProjectile.timeLeft); // Add the current projectile's index and timeleft to the point array
                    if (currentIndex >= _sticking.Length)  // If the javelin's index is bigger than or equal to the point array's length, break
                        break;
                }
            }

            // Remove the oldest if over max
            if (currentIndex >= MAX_STUCK)
            {
                int oldJavelinIndex = 0;
                // Loop our point array
                for (int i = 1; i < MAX_STUCK; i++)
                {
                    // Remove the already existing javelin if it's timeLeft value (which is the Y value in our point array) is smaller than the new javelin's timeLeft
                    if (_sticking[i].Y < _sticking[oldJavelinIndex].Y)
                    {
                        oldJavelinIndex = i; // Remember the index of the removed javelin
                    }
                }
                // Remember that the X value in our point array was equal to the index of that javelin, so it's used here to kill it.
                Main.projectile[_sticking[oldJavelinIndex].X].Kill();
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = projectile.width / 2;
            height = projectile.height / 2;
            return true;
        }
    }
}
