using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Terraria.Audio;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Content.Projectiles.Minions
{
    class SandWisp : ModProjectile
    {
        public override string Texture => AssetDirectory.Minion + Name;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sand Wisp");
            // Sets the amount of frames this minion has on its spritesheet
            Main.projFrames[Projectile.type] = 4;
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
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.scale = .6f;
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
            return action == 0;
        }

        public float attackTimer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public float action
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public float waitTimer
        {
            get => Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            #region Active check
            // This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
            if (player.dead || !player.active)
            {
                player.ClearBuff(BuffType<Buffs.Minions.SandWisp>());
            }
            if (player.HasBuff(BuffType<Buffs.Minions.SandWisp>()))
            {
                Projectile.timeLeft = 2;
            }
            #endregion

            #region General behavior
            Vector2 idlePosition = player.Center;
            idlePosition.Y -= 48f; // Go up 48 coordinates (three tiles from the center of the player)

            // If your minion doesn't aimlessly move around when it's idle, you need to "put" it into the line of other summoned minions
            // The index is projectile.minionPos
            float minionPositionOffsetX = (10 + Projectile.minionPos * 40) * -player.direction;
            idlePosition.X += minionPositionOffsetX; // Go behind the player

            // All of this code below this line is adapted from Spazmamini code (ID 388, aiStyle 66)

            // Teleport to player if distance is too big
            Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
            float distanceToIdlePosition = vectorToIdlePosition.Length();
            if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 2000f)
            {
                // Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
                // and then set netUpdate to true
                Projectile.position = idlePosition;
                Projectile.velocity *= 0.1f;
                Projectile.netUpdate = true;
            }

            // If your minion is flying, you want to do this independently of any conditions
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
            float distanceFromTarget = 700f;
            Vector2 targetCenter = Projectile.position;
            bool foundTarget = false;

            // This code is required if your minion weapon has the targeting feature
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
                        bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
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
            Projectile.friendly = foundTarget;
            #endregion

            #region Movement & Attacks

            // Default movement parameters (here for attacking)
            float speed = 12f;
            float inertia = 20f;

            if (foundTarget)
            {
                //Tick timer
                attackTimer--;

                //action == 2 ethis logic should happen reguardless of target, so it is elsewhere
                if (action == 1) //Stop and shoot sand
                {
                    Projectile.velocity *= 0.85f;
                    waitTimer++;
                    if (waitTimer > 45 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        //ATTACK
                        Vector2 toTarget = targetCenter - Projectile.Center;
                        toTarget.Normalize();
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, toTarget * 12f, ProjectileType<SandSpray>(), Projectile.damage * 3, 5, Projectile.owner);
                        action = 2;
                        attackTimer = Main.rand.Next(360, 600);
                        waitTimer = 0;
                        Projectile.scale = 0.15f;
                        Projectile.velocity = toTarget * -16f;
                        Projectile.netUpdate = true;
                    }
                    SoundEngine.PlaySound(SoundID.DoubleJump, Projectile.Center);

                    Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Sand, 0, 0);
                    d.noGravity = true;
                }
                else if (attackTimer <= 0) //Standard bash attacking
                {
                    if (Projectile.velocity.LengthSquared() < 400)
                        Projectile.velocity *= 1.04f;

                    float adjustedDistance = (targetCenter - Projectile.Center).Length();
                    if (adjustedDistance > 120)
                        action = 1;

                    if (Main.rand.NextBool(3))
                    {
                        Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Sand, 0, 0);
                        d.noGravity = true;
                    }
                } 
                else
                {
                    // Minion has a target: attack (here, fly towards the enemy)
                    if (distanceFromTarget > 80f)
                    {
                        // The immediate range around the target (so it doesn't latch onto it when close)
                        Vector2 direction = targetCenter - Projectile.Center;
                        direction.Normalize();
                        direction *= speed;
                        Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
                    }
                }
            }
            else
            {
                // Minion doesn't have a target: return to player and idle
                if (distanceToIdlePosition > 600f)
                {
                    // Speed up the minion if it's away from the player
                    speed = 16f;
                    inertia = 30f;
                }
                else
                {
                    // Slow down the minion if closer to the player
                    speed = 10f;
                    inertia = 60f;
                }
                if (distanceToIdlePosition > 20f)
                {
                    // The immediate range around the player (when it passively floats about)

                    // This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
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

            if (action == 2)
            {
                Projectile.velocity *= 0.99f;
                if (Projectile.velocity.Length() < 2f)
                {
                    Projectile.velocity *= 0;
                    if (Main.rand.NextBool(3))
                    {
                        Vector2 offset = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * 30;
                        Dust d = Dust.NewDustPerfect(Projectile.Center + offset, DustID.Sand, -offset / 10);
                        d.noGravity = true;
                    }
                }
                Projectile.scale += 0.003f;
                if (Projectile.scale >= .6f) //.4f is base scale for this
                {
                    Projectile.scale = .6f;
                    action = 0;
                }
            }
            #endregion

            #region Animation and visuals
            // So it will lean slightly towards the direction it's moving
            Projectile.rotation = Projectile.velocity.X * 0.05f;

            // This is a simple "loop through all frames from top to bottom" animation
            int frameSpeed = 5;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= frameSpeed)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

            // Some visuals here
            //Lighting.AddLight(projectile.Center, Color.White.ToVector3() * 0.78f);
            #endregion
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(Texture).Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, tex.Height/4 * Projectile.frame, tex.Width, tex.Height / 4), lightColor, Projectile.rotation, Projectile.Size/2, Projectile.scale, SpriteEffects.None);
            return false;
        }
    }

    class SandSpray : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.penetrate = 3;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
        }

        public override void AI()
        {
            Projectile.position = Projectile.Center;
            Projectile.scale += 0.15f;
            Projectile.Center = Projectile.position;
            for (int i = 0; i < 5; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, (int)(Projectile.width * Projectile.scale), (int)(Projectile.height * Projectile.scale), DustID.Sand, 0, 0, 0, default, Main.rand.NextFloat(0.8f, 1.8f));
                d.noGravity = true;
            }
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox.Width = (int)(hitbox.Width * Projectile.scale);
            hitbox.Height = (int)(hitbox.Height * Projectile.scale);
            base.ModifyDamageHitbox(ref hitbox);
        }
    }
}
