using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;

namespace ExoriumMod.Content.Projectiles.Minions
{
    class MorditeSkullSummon : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + "MorditeSkull";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mordite Skull");
            // This is necessary for right-click targeting
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            // These below are needed for a minion
            // Denotes that this projectile is a pet or minion
            Main.projPet[projectile.type] = true;
            // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public sealed override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 28;
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
            return false;
        }

        private float shotTimer = 0;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            float numbskull = 0;
            #region Active check
            // This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
            if (player.dead || !player.active)
            {
                player.ClearBuff(BuffType<Buffs.Minions.MorditeSkull>());
            }
            if (player.HasBuff(BuffType<Buffs.Minions.MorditeSkull>()))
            {
                projectile.timeLeft = 2;
            }
            #endregion

            #region General behavior
            Vector2 idlePosition = player.Center;
            idlePosition.Y -= 64f;
            // If your minion doesn't aimlessly move around when it's idle, you need to "put" it into the line of other summoned minions
            // The index is projectile.minionPos
            for (int i = 0; i <= Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].type == ProjectileType<MorditeSkullSummon>() && Main.projectile[i].owner == projectile.owner)
                    numbskull++;
                if (i == projectile.whoAmI)
                    i = Main.maxProjectiles + 1;
            }
            idlePosition.X += (projectile.width*2) * ((numbskull-1) - (float)(player.ownedProjectileCounts[ProjectileType<MorditeSkullSummon>()]-1)/2);
            projectile.position.X = idlePosition.X - projectile.width/2;
            projectile.position.Y = idlePosition.Y - projectile.height * 1.5f;
            projectile.netUpdate = true;
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
                        bool closeThroughWall = between < 100f;
                        if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall))
                        {
                            targetCenter = npc.Center;
                            foundTarget = true;
                        }
                    }
                }
            }
            #endregion
            Vector2 delta = targetCenter - projectile.Center;
            #region Movement
            if (foundTarget)
            {
                shotTimer--;
                if (shotTimer <= 0)
                {
                    shotTimer = 90;
                    float magnitude = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
                    if (magnitude > 0)
                        delta *= 20f / magnitude;
                    else
                        delta = new Vector2(0f, 30f);
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, delta.X, delta.Y, ProjectileType<SkullShot>(), projectile.damage, projectile.knockBack, Main.myPlayer);
                    Main.PlaySound(SoundID.Item20, projectile.position);
                }
            }
            #endregion

            #region Animation and visuals
            if (foundTarget)
            {
                projectile.rotation = (float)Math.Atan2(delta.Y, delta.X);
                projectile.spriteDirection = (int)(delta.X / Math.Abs(delta.X));
                if (projectile.spriteDirection <= 0)
                    projectile.rotation = (float)(Math.Atan2(delta.Y, delta.X) + Math.PI);
            }
            else
            {
                projectile.spriteDirection = player.direction;
                projectile.rotation = 0;
            }
            #endregion
        }
    }
}
