using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;

namespace ExoriumMod.Content.Projectiles.Minions
{
    class BlightedNeedleSummon : ModProjectile
    {
        public override string Texture => AssetDirectory.Minion + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blighsteel Needle");
            // This is necessary for right-click targeting
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;

            // These below are needed for a minion
            // Denotes that this projectile is a pet or minion
            Main.projPet[projectile.type] = true;
            // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            // Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public sealed override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 6;
            // Makes the minion go through tiles freely
            projectile.tileCollide = false;

            // These below are needed for a minion weapon
            // Only controls if it deals damage to enemies on contact (more on that later)
            projectile.friendly = true;
            // Only determines the damage type
            projectile.minion = true;
            // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
            projectile.minionSlots = .5f;
            // Needed so the minion doesn't despawn on collision with enemies or tiles
            projectile.penetrate = -1;

            //So they actually hit
            projectile.usesLocalNPCImmunity = true;
        }

        public bool IsStickingToTarget
        {
            get => projectile.ai[0] == 1f;
            set => projectile.ai[0] = value ? 1f : 0f;
        }

        // Index of the current target
        public int TargetWhoAmI
        {
            get => (int)projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        public int HitTicker
        {
            get => (int)projectile.localAI[0];
            set => projectile.localAI[0] = value;
        }

        public int bufferTime
        {
            get => (int)projectile.localAI[1];
            set => projectile.localAI[1] = value;
        }

        private const int TICKS_PER_HIT = 60;

        // Here you can decide if your minion breaks things like grass or pots
        public override bool? CanCutTiles()
        {
            return false;
        }

        // This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
        public override bool MinionContactDamage()
        {
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            #region Active check
            // This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
            if (player.dead || !player.active)
            {
                player.ClearBuff(BuffType<Buffs.Minions.BlightedNeedleSummonBuff>());
            }
            if (player.HasBuff(BuffType<Buffs.Minions.BlightedNeedleSummonBuff>()))
            {
                projectile.timeLeft = 2;
            }
            #endregion
            if (bufferTime > 0)
            {
                bufferTime--;
                IsStickingToTarget = false;
                NormalAI(player);
            }
            if (IsStickingToTarget)
                StuckAI(player);
            else
                NormalAI(player);
        }

        private void StuckAI(Player player)
        {
            NPC target = Main.npc[TargetWhoAmI]; // target
            projectile.netUpdate = true;

            const int aiFactor = 15; // Time Left stuck in seconds
            //Tick for damage
            HitTicker++;

            //Remove or not
            int projTargetIndex = (int)TargetWhoAmI;
            if (HitTicker >= 60 * aiFactor + 1|| projTargetIndex < 0 || projTargetIndex >= 200)
            { // Get unstuck
                IsStickingToTarget = false;
                HitTicker = 60;
                bufferTime = 60;
            }
            else if (Main.npc[projTargetIndex].active && !Main.npc[projTargetIndex].dontTakeDamage)
            { // If the target is active and can take damage
              // Set the projectile's position relative to the target's center
                projectile.Center = Main.npc[projTargetIndex].Center - projectile.velocity * 2f;
                projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;
            }
            else
            { // Get Unstuck
                IsStickingToTarget = false;
                HitTicker = 60;
                bufferTime = 30;
            }

            Vector2 idlePosition = player.Center;
            idlePosition.Y -= 48f; // Go up 48 coordinates (three tiles from the center of the player)
            // Teleport to player if distance is too big
            Vector2 vectorToIdlePosition = idlePosition - projectile.Center;
            float distanceToIdlePosition = vectorToIdlePosition.Length();
            if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 2000f)
            {
                // Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
                // and then set netUpdate to true
                projectile.position = idlePosition;
                projectile.velocity *= 0.1f;
                projectile.netUpdate = true;
                projectile.localAI[0] = 1f;
                bufferTime = 60;
                IsStickingToTarget = false;
            }
        }

        private void NormalAI(Player player)
        {
            #region General behavior
            Vector2 idlePosition = player.Center;
            idlePosition.Y -= 48f; // Go up 48 coordinates (three tiles from the center of the player)

            //Clump up behind the player
            float minionPositionOffsetX = 50 * -player.direction;
            idlePosition.X += minionPositionOffsetX; // Go behind the player

            // All of this code below this line is adapted from Spazmamini code (ID 388, aiStyle 66)

            // Teleport to player if distance is too big
            Vector2 vectorToIdlePosition = idlePosition - projectile.Center;
            float distanceToIdlePosition = vectorToIdlePosition.Length();
            if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 2000f)
            {
                // Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
                // and then set netUpdate to true
                projectile.position = idlePosition;
                projectile.velocity *= 0.1f;
                projectile.netUpdate = true;
            }

            // If your minion is flying, you want to do this independently of any conditions
            float overlapVelocity = 0.04f;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                // Fix overlap with other minions
                Projectile other = Main.projectile[i];
                if (i != projectile.whoAmI && other.active && other.owner == projectile.owner && Math.Abs(projectile.position.X - other.position.X) + Math.Abs(projectile.position.Y - other.position.Y) < projectile.width)
                {
                    if (projectile.position.X < other.position.X) projectile.velocity.X -= overlapVelocity;
                    else projectile.velocity.X += overlapVelocity;

                    if (projectile.position.Y < other.position.Y) projectile.velocity.Y -= overlapVelocity;
                    else projectile.velocity.Y += overlapVelocity;
                }
            }
            #endregion

            #region Find target
            // Starting search distance
            float distanceFromTarget = 700f;
            Vector2 targetCenter = projectile.position;
            bool foundTarget = false;

            // This code is required if your minion weapon has the targeting feature
            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, projectile.Center);
                // Reasonable distance away so it doesn't target across multiple screens
                if (between < 2000f)
                {
                    distanceFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                }
            }
            if (!foundTarget)
            {
                // This code is required either way, used for finding a target
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy())
                    {
                        float between = Vector2.Distance(npc.Center, projectile.Center);
                        bool closest = Vector2.Distance(projectile.Center, targetCenter) > between;
                        bool inRange = between < distanceFromTarget;
                        bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
                        // Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
                        // The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
                        bool closeThroughWall = between < 100f;
                        if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall))
                        {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                        }
                    }
                }
            }

            // friendly needs to be set to true so the minion can deal contact damage
            // friendly needs to be set to false so it doesn't damage things like target dummies while idling
            // Both things depend on if it has a target or not, so it's just one assignment here
            // You don't need this assignment if your minion is shooting things instead of dealing contact damage
            projectile.friendly = foundTarget;
            #endregion

            #region Movement

            // Default movement parameters (here for attacking)
            float speed = 20f;
            float inertia = 10f;

            if (foundTarget)
            {
                // Minion has a target: attack (here, fly towards the enemy)
                if (distanceFromTarget > 80f)
                {
                    // The immediate range around the target (so it doesn't latch onto it when close)
                    Vector2 direction = targetCenter - projectile.Center;
                    direction.Normalize();
                    direction *= speed;
                    projectile.velocity = (projectile.velocity * (inertia - 1) + direction) / inertia;
                }
            }
            else
            {
                // Minion doesn't have a target: return to player and idle
                if (distanceToIdlePosition > 600f)
                {
                    // Speed up the minion if it's away from the player
                    speed = 12f;
                    inertia = 60f;
                }
                else
                {
                    // Slow down the minion if closer to the player
                    speed = 4f;
                    inertia = 80f;
                }
                if (distanceToIdlePosition > 20f)
                {
                    // The immediate range around the player (when it passively floats about)

                    // This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
                    vectorToIdlePosition.Normalize();
                    vectorToIdlePosition *= speed;
                    projectile.velocity = (projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
                }
                else if (projectile.velocity == Vector2.Zero)
                {
                    // If there is a case where it's not moving at all, give it a little "poke"
                    projectile.velocity.X = -0.15f;
                    projectile.velocity.Y = -0.05f;
                }
            }
            #endregion

            #region Animation and visuals
            // So it will lean slightly towards the direction it's moving
            projectile.rotation = projectile.velocity.ToRotation();

            // Some visuals here
            //Lighting.AddLight(projectile.Center, Color.White.ToVector3() * 0.78f);
            #endregion
        }

        private void UpdateStuckNeedles(NPC target)
        {
            int counter = 0;
            //find number of stuck needles
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile currentProjectile = Main.projectile[i];
                if (currentProjectile.type == projectile.type
                    && currentProjectile.active 
                    && currentProjectile.owner == Main.myPlayer
                    && currentProjectile.modProjectile is BlightedNeedleSummon blightedNeedle
                    && blightedNeedle.IsStickingToTarget
                    && blightedNeedle.TargetWhoAmI == target.whoAmI)
                {
                    counter++;
                }
            }
            if (target.active && counter >= 4)
                target.AddBuff(BuffType<Buffs.Minions.StuckBlightedNeedleDebuff>(), 90);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!(bufferTime > 0))
            {
                TargetWhoAmI = target.whoAmI; // Set the target whoAmI
                projectile.velocity = (target.Center - projectile.Center); // Change velocity based on delta center of targets (difference between entity centers)
                IsStickingToTarget = true; // we are sticking to a target
                Main.npc[TargetWhoAmI].HitEffect(0, damage);
                damage = 0;
                UpdateStuckNeedles(target);
            }
        }

        public override bool CanDamage()
        {
            return (IsStickingToTarget && HitTicker % TICKS_PER_HIT == 0) || (!IsStickingToTarget && bufferTime <= 0);
        }
    }
}
