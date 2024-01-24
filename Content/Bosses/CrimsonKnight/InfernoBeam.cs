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
        private const float LIFE_TIME = 300;
        private const float BEAM_LENGTH = 1600f;
        private const int SOUND_INTERVAL = 30;
        private float TURN_SPEED = 0.001f;

        public bool BeBrighter => Projectile.ai[0] > 0f;

        public PrimDrawer LaserDrawer { get; private set; } = null;

        public float LifeCounter
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public float Charge
        {
            get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }

        public bool TurnLeft
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value ? 1f : 0f;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);

            writer.Write(Projectile.localAI[0]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);

            Projectile.localAI[0] = reader.ReadSingle();
        }

        public bool IsAtMaxCharge => Charge == MAX_CHARGE;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hostile = true;

            Projectile.netImportant = true;
        }

        /*
        public override bool? CanDamage()
        {
            return Projectile.scale >= 5f;
        }*/

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (!IsAtMaxCharge) return false;

            Vector2 unit = Projectile.velocity;
            unit.Normalize();
            float point = 0f;

            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center,
                Projectile.Center + unit * BEAM_LENGTH, 2, ref point);
        }

        public override void AI()
        {
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
            if (TURN_SPEED < .03f)
                TURN_SPEED *= 1.02f;
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
                //SoundEngine.PlaySound(SoundID.NPCDeath7, Projectile.position);
            }
        }

        public float WidthFunction(float trailInterpolant)
        {
            // Grow rapidly from the start to full length. Any more than this notably distorts the texture.
            float baseWidth = Projectile.scale * Projectile.width;
            return baseWidth;
        }

        public Color ColorFunction(float trailInterpolant) => Color.Lerp(new(255, 51, 51, 100), new(255, 190, 61, 100), trailInterpolant);
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 unitVel = Projectile.velocity;
            unitVel.Normalize();
            if (Charge == MAX_CHARGE)
                DrawHelper.DrawLaser(Request<Texture2D>(AssetDirectory.CrimsonKnight + "InfernoBeamGuide").Value, Projectile.Center, unitVel, 10, -1.57f, 1f, BEAM_LENGTH, default, 30, BEAM_LENGTH);
            else
            {
                // This should never happen, but just in case.
                if (Projectile.velocity == Vector2.Zero)
                    return false;

                // If it isnt set, set the prim instance.
                LaserDrawer ??= new(WidthFunction, ColorFunction, GameShaders.Misc["ExoriumMod:LaserEffect"]);

                // Get the laser end position.
                Vector2 laserEnd = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitY) * BEAM_LENGTH;

                // Create 8 points that span across the draw distance from the projectile center.

                // This allows the drawing to be pushed back, which is needed due to the shader fading in at the start to avoid
                // sharp lines.
                Vector2 initialDrawPoint = Projectile.Center - Projectile.velocity * 80f;
                Vector2[] baseDrawPoints = new Vector2[8];
                for (int i = 0; i < baseDrawPoints.Length; i++)
                    baseDrawPoints[i] = Vector2.Lerp(initialDrawPoint, laserEnd, i / (float)(baseDrawPoints.Length - 1f));

                // Set shader parameters. This one takes a fademap and a color.

                // The laser should fade to white in the middle.
                Color brightColor = new(194, 255, 242, 100);
                var shaderData = GameShaders.Misc["ExoriumMod:LaserEffect"];
                shaderData.UseColor(brightColor);
                shaderData.UseImage0(ModContent.Request<Texture2D>(AssetDirectory.Trail + "GenericLaser"));
                shaderData.Apply();

                /*
                Effect laser = GameShaders.Misc["ExoriumMod:LaserEffect"].Shader;
                laser.Parameters["uColor"].SetValue(brightColor);
                GameShaders.Misc["ExoriumMod:LaserEffect"].UseColor(brightColor);
                // GameShaders.Misc["FargoswiltasSouls:MutantDeathray"].UseImage1(); cannot be used due to only accepting vanilla paths.
                Texture2D laserTrail = ModContent.Request<Texture2D>(AssetDirectory.Trail + "GenericLaser").Value;
                GameShaders.Misc["ExoriumMod:LaserEffect"].UseImage0(AssetDirectory.Trail + "GenericLaser");
                // Draw a big glow above the start of the laser, to help mask the intial fade in due to the immense width.
                */

                Texture2D glowTexture = ModContent.Request<Texture2D>(AssetDirectory.Glow + "GlowRing").Value;

                Vector2 glowDrawPosition = Projectile.Center - Projectile.velocity * (BeBrighter ? 90f : 180f);

                Main.EntitySpriteDraw(glowTexture, glowDrawPosition - Main.screenPosition, null, brightColor, Projectile.rotation, glowTexture.Size() * 0.5f, Projectile.scale * 0.4f, SpriteEffects.None, 0);

                SpriteBatch spriteBatch = Main.spriteBatch;

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.NonPremultiplied, default, default, default, GameShaders.Misc["ExoriumMod:LaserEffect"].Shader, Main.GameViewMatrix.ZoomMatrix);

                DrawHelper.DrawLaser(ModContent.Request<Texture2D>(AssetDirectory.CrimsonKnight + Name).Value, Projectile.Center + unitVel, unitVel, 10, -1.57f, 1f, BEAM_LENGTH, default, 30, BEAM_LENGTH);
                LaserDrawer.DrawPrims(baseDrawPoints.ToList(), -Main.screenPosition, 60);

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
            }
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
        }
    }
}
