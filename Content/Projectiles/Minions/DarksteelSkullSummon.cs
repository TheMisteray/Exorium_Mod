using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;

namespace ExoriumMod.Content.Projectiles.Minions
{
    class DarksteelSkullSummon : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + "DarksteelSkull";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Darksteel Skull");
            // This is necessary for right-click targeting
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            // These below are needed for a minion
            // Denotes that this projectile is a pet or minion
            Main.projPet[Projectile.type] = true;
            // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public sealed override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 28;
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
            return false;
        }

        private float shotTimer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            float numbskull = 0;
            #region Active check
            // This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
            if (player.dead || !player.active)
            {
                player.ClearBuff(BuffType<Buffs.Minions.DarksteelSkull>());
            }
            if (player.HasBuff(BuffType<Buffs.Minions.DarksteelSkull>()))
            {
                Projectile.timeLeft = 2;
            }
            #endregion

            #region General behavior
            Vector2 idlePosition = player.Center;
            idlePosition.Y -= 64f;
            // If your minion doesn't aimlessly move around when it's idle, you need to "put" it into the line of other summoned minions
            // The index is projectile.minionPos
            for (int i = 0; i <= Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].type == ProjectileType<DarksteelSkullSummon>() && Main.projectile[i].owner == Projectile.owner)
                    numbskull++;
                if (i == Projectile.whoAmI)
                    i = Main.maxProjectiles + 1;
            }
            idlePosition.X += (Projectile.width*2) * ((numbskull-1) - (float)(player.ownedProjectileCounts[ProjectileType<DarksteelSkullSummon>()]-1)/2);
            Projectile.position.X = idlePosition.X - Projectile.width/2;
            Projectile.position.Y = idlePosition.Y - Projectile.height * 1.5f;
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

            #region Movement
            Vector2 delta = targetCenter - Projectile.Center;
            if (foundTarget)
            {
                shotTimer--;
                if (shotTimer <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    shotTimer = Main.rand.Next(70, 111);
                    Projectile.netUpdate = true;
                    float magnitude = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
                    if (magnitude > 0)
                        delta *= 20f / magnitude;
                    else
                        delta = new Vector2(0f, 30f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, delta.X, delta.Y, ProjectileType<SkullShot>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
                }
                else if (shotTimer <= 0)
                    SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
            }
            #endregion

            #region Animation and visuals
            if (foundTarget)
            {
                Projectile.rotation = (float)Math.Atan2(delta.Y, delta.X);
                Projectile.spriteDirection = (int)(delta.X / Math.Abs(delta.X));
                if (Projectile.spriteDirection <= 0)
                    Projectile.rotation = (float)(Math.Atan2(delta.Y, delta.X) + Math.PI);
            }
            else
            {
                Projectile.spriteDirection = player.direction;
                Projectile.rotation = 0;
            }
            #endregion
        }
    }

    class SkullShot : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.alpha = 255;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 2;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
        }

        public override void AI()
        {
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Dusts.DarksteelDust>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
        }
    }
}
