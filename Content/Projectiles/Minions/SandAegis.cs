using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExoriumMod.Content.Projectiles.Minions
{
    class SandAegis : ModProjectile, IPixelatedPrimitiveRenderer
    {
        public override string Texture => AssetDirectory.Minion + Name;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sand Aegis");
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
            Projectile.width = 36;
            Projectile.height = 36;
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

        float tickerSync
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        float rotationSpeed
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        bool IsMaster
        {
            get => Projectile.ai[2] == 1;
            set => Projectile.ai[2] = value ? 1 : 0;
        }

        private static float RotationRadius = 80f;
        private bool setSelf = false;
        Vector2 anchorPoint;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            IsMaster = false;

            if (!setSelf)
            {
                anchorPoint = player.Center;
                setSelf = true;
            }

            #region Active check
                // This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
            if (player.dead || !player.active)
            {
                player.ClearBuff(BuffType<Buffs.Minions.SandAegis>());
            }
            if (player.HasBuff(BuffType<Buffs.Minions.SandAegis>()))
            {
                Projectile.timeLeft = 2;
            }
            #endregion

            #region Find target
            // Starting search distance
            float distanceFromTarget = 10f;
            Vector2 targetCenter = player.position;
            bool foundTarget = false;

            // This code is required if your minion weapon has the targeting feature
            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, player.Center);
                // Reasonable distance away so it doesn't target across multiple screens
                if (between < 300f)
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
                        float between = Vector2.Distance(npc.Center, player.Center);
                        bool closest = Vector2.Distance(player.Center, targetCenter) > between;
                        bool inRange = between < distanceFromTarget;
                        if (between < 450f && ((closest && inRange) || !foundTarget))
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

            #region General behavior
            Vector2 anchorPointTarget = player.Center;
            float numShield = 0;

            //Find and index self based on other existing summons
            for (int i = 0; i <= Main.maxProjectiles; i++)
            {
                if (i == Projectile.whoAmI) //Exit once self is found
                {
                    if (numShield == 0 && Main.netMode != NetmodeID.MultiplayerClient) //use this one as master if it is first and set all other's ticker to it
                    {
                        foreach (Projectile p in Main.ActiveProjectiles)
                        {
                            if (p.type == ProjectileType<SandAegis>() && p.whoAmI != Projectile.whoAmI && p.ai[0] != tickerSync)
                            {
                                p.ai[0] = tickerSync;
                                p.ai[1] = rotationSpeed;
                                p.netUpdate = true;
                            }
                        }

                        IsMaster = true;
                    }

                    i = Main.maxProjectiles + 1;
                }
                else if (Main.projectile[i].type == ProjectileType<SandAegis>() && Main.projectile[i].owner == Projectile.owner)
                    numShield++;
            }

            tickerSync += rotationSpeed;
            Vector2 towardsTarget = (targetCenter - player.Center);
            towardsTarget.Normalize();
            anchorPointTarget += towardsTarget * Math.Max(distanceFromTarget - RotationRadius, 0);

            if (foundTarget)
                anchorPoint = Vector2.Lerp(anchorPoint, anchorPointTarget, 0.06f * (350 / distanceFromTarget));
            else
                anchorPoint = Vector2.Lerp(anchorPoint, anchorPointTarget, 0.1f);

            Vector2 rotationPoint = anchorPoint + (Vector2.UnitX * RotationRadius).RotatedBy(MathHelper.TwoPi / player.ownedProjectileCounts[ProjectileType<SandAegis>()] * numShield + MathHelper.ToRadians(tickerSync * 3));
            Projectile.Center = rotationPoint;

            //Vector2 idlePosition = player.Center;

            //// Teleport to player if distance is too big
            //Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
            //float distanceToIdlePosition = vectorToIdlePosition.Length();
            //if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 2000f)
            //{
            //    // Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
            //    // and then set netUpdate to true
            //    Projectile.position = idlePosition;
            //    Projectile.velocity *= 0.1f;
            //    Projectile.netUpdate = true;
            //}
            #endregion

            #region Movement & Actions
            if (foundTarget)
            {
                if (rotationSpeed < 1.5f)
                    rotationSpeed += 0.01f;
                else
                    rotationSpeed = 1.5f;
            }
            else
            {
                if (rotationSpeed > 0.5f)
                    rotationSpeed -= 0.01f;
                else
                    rotationSpeed = 0.5f;
            }
            #endregion

            #region Animation and visuals
            // So it will lean slightly towards the direction it's moving
            Projectile.rotation = Projectile.velocity.X * 0.05f;
            #endregion
        }

        public void RenderPixelatedPrimitives(SpriteBatch spriteBatch)
        {
            if (IsMaster)
                PrimitiveRenderer.RenderCircleEdge(anchorPoint, new(_ => 2, _ => Color.SandyBrown, _ => RotationRadius, true));
        }
    }
}
