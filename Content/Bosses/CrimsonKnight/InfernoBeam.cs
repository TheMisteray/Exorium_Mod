using ExoriumMod.Core;
using ExoriumMod.Helpers;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using System;
using ExoriumMod.Primitives;
using System.IO;
using Terraria.Graphics.Shaders;
using ExoriumMod.Content.Bosses.GemsparklingHive;
using System.Linq;

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    internal class InfernoBeam : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + Name;

        private const float MAX_CHARGE = 50f;
        private const float LIFE_TIME = 480;
        private const float BEAM_LENGTH = 3200f;
        private const int SOUND_INTERVAL = 30;
        private float TURN_SPEED = 0.0015f;

        public bool BeBrighter => Projectile.ai[0] > 0f;

        public float LifeCounter
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public bool TurnLeft
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value ? 1f : 0f;
        }

        public float Charge
        {
            get => Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }

        public float AnchorX
        {
            get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }

        public float AnchorY
        {
            get => Projectile.localAI[1];
            set => Projectile.localAI[1] = value;
        }

        public bool IsAtMaxCharge => Charge == MAX_CHARGE;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CanDistortWater[Type] = false;
            ProjectileID.Sets.CanHitPastShimmer[Type] = true;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 6400;
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 300;

            Projectile.netImportant = true;
        }

        /*
        public override bool? CanDamage()
        {
            return Projectile.scale >= 5f;
        }*/

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (!IsAtMaxCharge || LifeCounter > LIFE_TIME - 30) return false;

            Vector2 unit = Projectile.velocity;
            unit.Normalize();
            float point = 0f;

            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
                Projectile.Center + unit * BEAM_LENGTH, 2, ref point);
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 300)
            {
                AnchorX = Projectile.position.X;
                AnchorY = Projectile.position.Y;
            }

            Projectile.position = new Vector2(AnchorX, AnchorY);
            Projectile.timeLeft = 2;

            //Turn after damage begins
            if (LifeCounter > 0)
            {
                Update();
                PlaySounds();
            }
            ChargeLaser();

            //After charging complete
            if (Charge < MAX_CHARGE) return;

            LifeCounter++;
            //CastLights();

            if (LifeCounter > LIFE_TIME)
                Projectile.Kill();
        }

        private void Update()
        {
            if (TURN_SPEED < .0003f)
                TURN_SPEED *= 1.005f;
            if (TurnLeft)
                Projectile.velocity = Projectile.velocity.RotatedBy(TURN_SPEED);
            else
                Projectile.velocity = Projectile.velocity.RotatedBy(-TURN_SPEED);
        }

        private void ChargeLaser()
        {
            if (Charge < MAX_CHARGE)
            {
                Charge++;
            }
        }

        private void PlaySounds()
        {
            if (Projectile.soundDelay <= 0)
            {
                Projectile.soundDelay = SOUND_INTERVAL;
                SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 unitVel = Projectile.velocity;
            unitVel.Normalize();
            if (IsAtMaxCharge)
                DrawHelper.DrawLaser(Request<Texture2D>(AssetDirectory.CrimsonKnight + "InfernoBeam").Value, Projectile.Center, unitVel, 10, new Rectangle(0, 0, 44, 44), new Rectangle(0, 48, 44, 60) ,new Rectangle(0, 112, 44, 44), -1.57f, 1f, BEAM_LENGTH, new Color(254, 121, 2) * ((60 - Math.Max(LifeCounter + 60 - LIFE_TIME, 0)) / 60), 0, BEAM_LENGTH);
            else
                DrawHelper.DrawLaser(Request<Texture2D>(AssetDirectory.CrimsonKnight + "InfernoBeamGuide").Value, Projectile.Center, unitVel, 10, new Rectangle(0, 0, 22, 22), new Rectangle(0, 24, 22, 30), new Rectangle(0, 56, 22, 22) , - 1.57f, 1f, BEAM_LENGTH, new Color(254, 121, 2) * ((60 - Math.Max(LifeCounter + 60 - LIFE_TIME, 0)) / 60), 0, BEAM_LENGTH);
            return false;
        }
    }

    internal class RotatingFireball : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + "CaraveneFireball";

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
        }

        private static float RadialSpeed = .012f;
        private static float Speed = 5;
        float radius = 0;
        float angle = 0;
        Vector2 origin;

        public bool Enrage
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value ? 1f : 0f;
        }

        public bool CounterClockwise
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value ? 1f : 0f;
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 1200)
            {
                origin = Projectile.Center;
                angle = Projectile.velocity.ToRotation();
                Projectile.velocity = Vector2.Zero;
            }
            else
            {
                radius += Speed;
                angle += RadialSpeed;
                Vector2 offset = Vector2.Zero;
                if (CounterClockwise)
                {
                    offset.X = radius * (float)Math.Cos(angle);
                    offset.Y = radius * (float)Math.Sin(angle);
                }
                else
                {
                    offset.X = radius * (float)Math.Sin(angle);
                    offset.Y = radius * (float)Math.Cos(angle);
                }
                Projectile.Center = origin + offset;
                Projectile.rotation += .2f;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, Enrage ? 600 : 300);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(Texture).Value;
            //Afterimages
            for (int k = Projectile.oldPos.Length - 1; k >= 0; k--)
            {
                Vector2 pos = Projectile.oldPos[k];

                Main.EntitySpriteDraw(tex, pos - Main.screenPosition + new Vector2(Projectile.width / 2, Projectile.height / 2), new Rectangle(0, 0, Projectile.width, Projectile.height), new Color(255, 255, 255, 255 / (k + 1)), Projectile.oldRot[k], new Vector2(tex.Width / 2, tex.Height / 2), Projectile.scale, SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, Projectile.width, Projectile.height), Color.White, Projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
