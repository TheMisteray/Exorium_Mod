using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Dusts;

namespace ExoriumMod.Projectiles
{
    class BlightedPikeProj : ModProjectile
    {

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.aiStyle = 19;
            projectile.penetrate = -1;
            projectile.scale = 1.3f;
            projectile.alpha = 0;
            projectile.hide = true;
            projectile.ownerHitCheck = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.friendly = true;
        }

        public float movementFactor //Speed of attack
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public override void AI()
        {
            Player spearUser = Main.player[projectile.owner];
            Vector2 ownerMountedCenter = spearUser.RotatedRelativePoint(spearUser.MountedCenter, true);
            projectile.direction = spearUser.direction;
            spearUser.heldProj = projectile.whoAmI;
            spearUser.itemTime = spearUser.itemAnimation;
            projectile.position.X = ownerMountedCenter.X - (float)(projectile.width / 2);
            projectile.position.Y = ownerMountedCenter.Y - (float)(projectile.height / 2);
            if (!spearUser.frozen) //prevents spear from moving is owner is frozen
            {
                if (movementFactor == 0f)
                {
                    movementFactor = 3f; // Forward
                    projectile.netUpdate = true;
                }
                if (spearUser.itemAnimation < spearUser.itemAnimationMax / 3) // Back
                {
                    movementFactor -= 2.4f;
                }
                else // Other
                {
                    movementFactor += 2.1f;
                }
            }

            projectile.position += projectile.velocity * movementFactor;

            if (spearUser.itemAnimation == 0)
            {
                projectile.Kill();
            }

            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);

            if (projectile.spriteDirection == -1)
            {
                projectile.rotation -= MathHelper.ToRadians(90f);
            }

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.height, projectile.width, DustType<BlightDust>(), projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 200, Scale: 1.1f);
                dust.velocity += projectile.velocity * 0.2f;
                dust.velocity *= 0.2f;
            }
        }
    }
}
