using ExoriumMod.Core;
using ExoriumMod.Helpers;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Content.Bosses.BlightedSlime
{
    class BlightedThorn : ModProjectile
    {
        public override string Texture => AssetDirectory.BlightedSlime + Name;

        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 100;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 600;
            projectile.hostile = true;
            projectile.tileCollide = false;
        }

        public float TargetPlayer
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public float AIState
        {
            get => projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        public float TargetPlayerX
        {
            get => Main.player[(int)TargetPlayer].Center.X;
        }

        public float TargetPlayerY
        {
            get => Main.player[(int)TargetPlayer].Center.Y;
        }

        public override void AI()
        {
            Player player = Main.player[(int)TargetPlayer];
            switch (AIState)
            {
                case 0: //Keep given velocity
                    projectile.rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2;
                    if (projectile.timeLeft < 540 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        AIState++;
                        projectile.netUpdate = true;
                    }
                    break;
                case 1: // Fly above player
                    Vector2 highAbove = new Vector2(0, -1000);
                    Vector2 floatPos = highAbove.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-60, 60)));
                    projectile.velocity = ((player.Center + floatPos) - projectile.Center) / 30;
                    projectile.rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2;
                    if (projectile.timeLeft < 450 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        AIState++;
                        projectile.netUpdate = true;
                    }
                    break;
                case 2: // Point at player and send indicator
                    projectile.velocity = Vector2.Zero;
                    projectile.rotation = (player.Center - projectile.Center).ToRotation() - MathHelper.PiOver2 + MathHelper.ToRadians(Main.rand.NextFloat(-30, 30));
                    //Add indicator
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        AIState++;
                        projectile.netUpdate = true;
                    }
                    break;
                case 3: // Wait for a moment
                    if (projectile.timeLeft < 330 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        AIState++;
                        projectile.netUpdate = true;
                    }
                    break;
                case 4: // Shoot forward
                    Vector2 shootForward = new Vector2(0, 34);
                    Vector2 shoot = shootForward.RotatedBy(projectile.rotation);
                    projectile.velocity = shoot;
                    if (projectile.Bottom.Y > player.Center.Y)
                        projectile.tileCollide = true;
                    break;
                default: // Stay stuck in ground and don't deal damage again
                    projectile.velocity = Vector2.Zero;
                    projectile.hostile = false;
                    break;
            }
        }

        public override bool PreDrawExtras(SpriteBatch spriteBatch)
        {
            if (AIState == 3)
            {
                Texture2D tex = GetTexture(AssetDirectory.BlightedSlime + "BlightedThornIndicator");
                DrawHelper.DrawLaser(spriteBatch, tex, projectile.Center, new Vector2(0, 1).RotatedBy(projectile.rotation), 10, MathHelper.PiOver2, 1);
            }
            return true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            height = 2;
            fallThrough = false;
            for (int i = 0; i < 2; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, 1, DustType<Dusts.BlightDust>(), projectile.velocity.X * -1, projectile.velocity.Y * -1);
            }
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 25; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, 1, DustType<Dusts.BlightDust>(), projectile.velocity.X * -1, projectile.velocity.Y * -1);
            }
            projectile.tileCollide = false;
            //No more AI
            AIState = 5;
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustType<Dusts.BlightDust>(), projectile.velocity.X * -1, projectile.velocity.Y * -1);
            }
        }
    }
}
