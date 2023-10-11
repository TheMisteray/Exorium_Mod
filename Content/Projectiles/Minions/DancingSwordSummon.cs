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
            // DisplayName.SetDefault("DancingSword");
            // This is necessary for right-click targeting
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            // These below are needed for a minion
            // Denotes that this projectile is a pet or minion
            Main.projPet[Projectile.type] = true;
            // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            // Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public sealed override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.scale = 1.2f;
            // Makes the minion go through tiles freely
            Projectile.tileCollide = false;

            // These below are needed for a minion weapon
            // Only controls if it deals damage to enemies on contact (more on that later)
            Projectile.friendly = true;
            // Only determines the damage type
            Projectile.minion = true;
            // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
            Projectile.minionSlots = 1f;
            // Needed so the minion doesn't despawn on collision with enemies or tiles
            Projectile.penetrate = -1;
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
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public float Timer
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        Vector2 anchor = Vector2.Zero;

        public override void AI()
        {
            if (AIState == 0)
            {
                StartAI();
                return;
            }

            Player player = Main.player[Projectile.owner];

            #region Active check
            // This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
            if (player.dead || !player.active)
            {
                player.ClearBuff(BuffType<Buffs.Minions.DancingSwordSummonBuff>());
            }
            if (player.HasBuff(BuffType<Buffs.Minions.DancingSwordSummonBuff>()))
            {
                Projectile.timeLeft = 2;
            }
            #endregion

            #region General behavior
            Vector2 idlePosition = player.Center;

            // Teleport to player if distance is too big
            Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
            float distanceToIdlePosition = vectorToIdlePosition.Length();
            if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 2000f)
            {
                Projectile.position = idlePosition;
                Projectile.velocity *= 0.1f;
                Projectile.netUpdate = true;
            }

            // Fix overlap
            float overlapVelocity = 0.04f;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                // Fix overlap with other minions
                Projectile other = Main.projectile[i];
                if (i != Projectile.whoAmI && other.active && other.owner == Projectile.owner && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width)
                {
                    if (Projectile.position.X < other.position.X) Projectile.velocity.X -= overlapVelocity;
                    else Projectile.velocity.X += overlapVelocity;

                    if (Projectile.position.Y < other.position.Y) Projectile.velocity.Y -= overlapVelocity;
                    else Projectile.velocity.Y += overlapVelocity;
                }
            }
            #endregion

            #region Find target
            // Starting search distance
            float distanceFromTarget = 500f;
            Vector2 targetCenter = Projectile.position;
            bool foundTarget = false;

            // Right click targeting
            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, Projectile.Center);
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
                        float between = Vector2.Distance(npc.Center, Projectile.Center);
                        bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
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

            Projectile.friendly = foundTarget;
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
                    Projectile.velocity *= 3;
                    Projectile.netUpdate = true;
                }
                //Choose attack style
                switch(AIState)
                {
                    case 1: //Swift dashes at target
                        if (distanceFromTarget > 200f)
                        {
                            speed = 22;
                            Vector2 direction = targetCenter - Projectile.Center;
                            direction.Normalize();
                            direction *= speed;
                            Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
                            Projectile.rotation = Projectile.velocity.ToRotation();
                            Projectile.rotation += MathHelper.PiOver2;
                        }
                        break;
                    case 2: //slow spin at target
                        if (distanceFromTarget > 160f)
                        {
                            Vector2 direction2 = targetCenter - Projectile.Center;
                            direction2.Normalize();
                            direction2 *= speed;
                            Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction2) / inertia;
                        }
                        Projectile.rotation += .3f;
                        break;
                    case 3: //Swing in circles next to target
                        if (Timer == 0)
                        {
                            Vector2 diff = Projectile.Center - targetCenter;
                            diff.Normalize();
                            diff *= Projectile.width;
                            anchor = diff;
                        }
                        else if (((targetCenter + anchor) - Projectile.Center).Length() > Projectile.width * 2)
                        {
                            Projectile.velocity = ((targetCenter + anchor) - Projectile.Center) / 30;
                            Projectile.rotation = Projectile.velocity.ToRotation();
                            Projectile.rotation += MathHelper.PiOver2;
                        }
                        else
                        {
                            Vector2 off = new Vector2(0, Projectile.width);
                            Vector2 offRot = off.RotatedBy(MathHelper.ToRadians(Timer * 10));
                            Projectile.position = (targetCenter + anchor) + offRot;
                            Vector2 pointing = Projectile.Center - (targetCenter + anchor);
                            pointing.Normalize();
                            Projectile.rotation = pointing.ToRotation();
                            Projectile.rotation += MathHelper.PiOver2;
                        }
                        break;
                }
                Timer++;
            }
            else
            {
                Projectile.rotation += Projectile.velocity.X * 0.03f;
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
                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
                }
                else if (Projectile.velocity == Vector2.Zero)
                {
                    // If there is a case where it's not moving at all, give it a little "poke"
                    Projectile.velocity.X = -0.15f;
                    Projectile.velocity.Y = -0.05f;
                }
            }
            #endregion
            #region Animation and visuals

            #endregion
        }

        private void StartAI()
        {
            //Act like a thrown weapon at first
            Projectile.rotation += .1f;
            Timer++;
            if (Timer > 90 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                AIState = 1;
                Projectile.netUpdate = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (AIState == 2) //Slow spin
                hit.Damage /= 2;
        }
    }
}
