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
            Projectile.width = 40;
            Projectile.height = 100;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
        }

        public float TargetPlayer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public float AIState
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
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
                    Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
                    if (Projectile.timeLeft < 540 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        AIState++;
                        Projectile.netUpdate = true;
                    }
                    break;
                case 1: // Fly above player
                    Vector2 highAbove = new Vector2(0, -1000);
                    Vector2 floatPos = highAbove.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-60, 60)));
                    Projectile.velocity = ((player.Center + floatPos) - Projectile.Center) / 30;
                    Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
                    if (Projectile.timeLeft < 450 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        AIState++;
                        Projectile.netUpdate = true;
                    }
                    break;
                case 2: // Point at player and send indicator
                    Projectile.velocity = Vector2.Zero;
                    Projectile.rotation = (player.Center - Projectile.Center).ToRotation() - MathHelper.PiOver2 + MathHelper.ToRadians(Main.rand.NextFloat(-30, 30));
                    //Add indicator
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        AIState++;
                        Projectile.netUpdate = true;
                    }
                    break;
                case 3: // Wait for a moment
                    if (Projectile.timeLeft < 330 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        AIState++;
                        Projectile.netUpdate = true;
                    }
                    break;
                case 4: // Shoot forward
                    Vector2 shootForward = new Vector2(0, 34);
                    Vector2 shoot = shootForward.RotatedBy(Projectile.rotation);
                    Projectile.velocity = shoot;
                    if (Projectile.Bottom.Y > player.Center.Y)
                        Projectile.tileCollide = true;
                    break;
                default: // Stay stuck in ground and don't deal damage again
                    Projectile.velocity = Vector2.Zero;
                    Projectile.hostile = false;
                    break;
            }
        }

        public override bool PreDrawExtras()
        {
            if (AIState == 3)
            {
                Texture2D tex = Request<Texture2D>(AssetDirectory.BlightedSlime + "BlightedThornIndicator").Value;
                DrawHelper.DrawLaser(tex, Projectile.Center, new Vector2(0, 1).RotatedBy(Projectile.rotation), 10, MathHelper.PiOver2, 1);
            }
            return true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            height = 2;
            fallThrough = false;
            for (int i = 0; i < 2; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, 1, DustType<Dusts.BlightDust>(), Projectile.velocity.X * -1, Projectile.velocity.Y * -1);
            }
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 25; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, 1, DustType<Dusts.BlightDust>(), Projectile.velocity.X * -1, Projectile.velocity.Y * -1);
            }
            Projectile.tileCollide = false;
            //No more AI
            AIState = 5;
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<Dusts.BlightDust>(), Projectile.velocity.X * -1, Projectile.velocity.Y * -1);
            }
        }
    }
}
