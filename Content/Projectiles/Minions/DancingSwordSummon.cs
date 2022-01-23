using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;

namespace ExoriumMod.Content.Projectiles.Minions
{
    class DancingSwordSummon : ModProjectile
    {
        public override string Texture => AssetDirectory.Minion + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("DancingSword");
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
            projectile.width = 48;
            projectile.height = 48;
            projectile.scale = 1.2f;
            // Makes the minion go through tiles freely
            projectile.tileCollide = false;

            // These below are needed for a minion weapon
            // Only controls if it deals damage to enemies on contact (more on that later)
            projectile.friendly = true;
            // Only determines the damage type
            projectile.minion = true;
            // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
            projectile.minionSlots = 1f;
            // Needed so the minion doesn't despawn on collision with enemies or tiles
            projectile.penetrate = -1;
        }

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

        public float AIState
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public float Timer
        {
            get => projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        Vector2 anchor = Vector2.Zero;

        public override void AI()
        {
            if (AIState == 0)
            {
                StartAI();
                return;
            }

            Player player = Main.player[projectile.owner];

            #region Active check
            // This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
            if (player.dead || !player.active)
            {
                player.ClearBuff(BuffType<Buffs.Minions.DancingSwordSummonBuff>());
            }
            if (player.HasBuff(BuffType<Buffs.Minions.DancingSwordSummonBuff>()))
            {
                projectile.timeLeft = 2;
            }
            #endregion

            #region General behavior
            Vector2 idlePosition = player.Center;

            // Teleport to player if distance is too big
            Vector2 vectorToIdlePosition = idlePosition - projectile.Center;
            float distanceToIdlePosition = vectorToIdlePosition.Length();
            if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 2000f)
            {
                projectile.position = idlePosition;
                projectile.velocity *= 0.1f;
                projectile.netUpdate = true;
            }

            // Fix overlap
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
            float distanceFromTarget = 500f;
            Vector2 targetCenter = projectile.position;
            bool foundTarget = false;

            // Right click targeting
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

                        if (((closest && inRange) || !foundTarget))
                        {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                        }
                    }
                }
            }

            projectile.friendly = foundTarget;
            #endregion

            #region Movement

            float speed = 12f;
            float inertia = 6f;

            if (foundTarget)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient && Timer > 180)
                {
                    AIState = Main.rand.Next(3) + 1;
                    Timer = 0;
                    projectile.velocity *= 3;
                    projectile.netUpdate = true;
                }
                //Choose attack style
                switch(AIState)
                {
                    case 1: //Swift dashes at target
                        if (distanceFromTarget > 160f)
                        {
                            speed = 22;
                            Vector2 direction = targetCenter - projectile.Center;
                            direction.Normalize();
                            direction *= speed;
                            projectile.velocity = (projectile.velocity * (inertia - 1) + direction) / inertia;
                            projectile.rotation = projectile.velocity.ToRotation();
                            projectile.rotation += MathHelper.PiOver2;
                        }
                        break;
                    case 2: //slow spin at target
                        if (distanceFromTarget > 100f)
                        {
                            Vector2 direction2 = targetCenter - projectile.Center;
                            direction2.Normalize();
                            direction2 *= speed;
                            projectile.velocity = (projectile.velocity * (inertia - 1) + direction2) / inertia;
                        }
                        projectile.rotation += .3f;
                        break;
                    case 3: //Swing in circles next to target
                        if (Timer == 0)
                        {
                            Vector2 diff = projectile.Center - targetCenter;
                            diff.Normalize();
                            diff *= projectile.width;
                            anchor = diff;
                        }
                        else if (((targetCenter + anchor) - projectile.Center).Length() > projectile.width * 2)
                        {
                            projectile.velocity = ((targetCenter + anchor) - projectile.Center) / 30;
                            projectile.rotation = projectile.velocity.ToRotation();
                            projectile.rotation += MathHelper.PiOver2;
                        }
                        else
                        {
                            Vector2 off = new Vector2(0, projectile.width);
                            Vector2 offRot = off.RotatedBy(MathHelper.ToRadians(Timer * 10));
                            projectile.position = (targetCenter + anchor) + offRot;
                            Vector2 pointing = projectile.Center - (targetCenter + anchor);
                            pointing.Normalize();
                            projectile.rotation = pointing.ToRotation();
                            projectile.rotation += MathHelper.PiOver2;
                        }
                        break;
                }
                Timer++;
            }
            else
            {
                projectile.rotation += projectile.velocity.X * 0.03f;
                // Minion doesn't have a target: return to player and idle
                if (distanceToIdlePosition > 600f)
                {
                    // Speed up the minion if it's away from the player
                    speed = 20f;
                    inertia = 60f;
                }
                else
                {
                    // Slow down the minion if closer to the player
                    speed = 12f;
                    inertia = 80f;
                }
                if (distanceToIdlePosition > 200f)
                {
                    // Swords kind float about nearby
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

            #endregion
        }

        private void StartAI()
        {
            //Act like a thrown weapon at first
            projectile.rotation += .1f;
            Timer++;
            if (Timer > 90 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                AIState = 1;
                projectile.netUpdate = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (AIState == 2) //Slow spin
                damage /= 2;
        }
    }
}
