using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using ExoriumMod.Content.Dusts;

namespace ExoriumMod.Content.Bosses.Shadowmancer
{
    class ShadowOrb : ModProjectile
    {
        public override string Texture => AssetDirectory.Shadowmancer + Name;

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.friendly = false;
            projectile.timeLeft = 600;
            projectile.hostile = true;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            projectile.velocity.Y += .18f;
            projectile.rotation += .2f;
            if (Math.Abs(projectile.velocity.X) >= 1)
            {
                projectile.velocity.X *= .96f;
            }
            if (projectile.timeLeft <= 500 )
                projectile.tileCollide = true;
            Vector2 delta = projectile.position - new Vector2(projectile.position.X + Main.rand.NextFloat(-1, 2), projectile.position.Y + Main.rand.NextFloat(-1, 2));
            if (Main.rand.NextBool(3))
            {
                int dust0 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Rainbow>(), delta.X, delta.Y);
                Main.dust[dust0].color = new Color(200, 0, 0);
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 5; k++)
            {
                int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Shadow>(), projectile.oldVelocity.X * 0.5f, projectile.oldVelocity.Y * 0.5f);
                Main.dust[dust].color = new Color(255, 110, 0);
            }
            Main.PlaySound(SoundID.Item27, projectile.position);
            NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y, NPCType<ShadowAdd>());
        }
    }
}
