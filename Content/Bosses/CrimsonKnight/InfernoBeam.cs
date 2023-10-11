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

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    internal class InfernoBeam : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + Name;

        private const float MAX_CHARGE = 50f;
        //The distance charge particle from the player center
        private const float MOVE_DISTANCE = 60f;

        private const float LIFE_TIME = 180;

        private const float TurnResponsiveness = 0.01f;

        private const float BeamLength = 1600f;

        private const int SoundInterval = 30;

        public bool BeBrighter => Projectile.ai[0] > 0f;

        public PrimDrawer LaserDrawer { get; private set; } = null;

        public float LifeCounter
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        // The actual charge value is stored in the localAI0 field
        public float Charge
        {
            get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }

        public float NPCWhoAmI
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
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

        // Are we at max charge? With c#6 you can simply use => which indicates this is a get only property
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

        public override bool? CanDamage()
        {
            return Projectile.scale >= 5f;
        }

        public float WidthFunction(float trailInterpolant)
        {
            // Grow rapidly from the start to full length. Any more than this notably distorts the texture.
            float baseWidth = Projectile.scale * Projectile.width;
            return baseWidth;
        }

        public Color ColorFunction(float trailInterpolant) => Color.Lerp(new(255, 51, 51, 100), new(255, 190, 61, 100), trailInterpolant);
        /*Commented out while fixing port
        public override bool PreDraw(ref Color lightColor)
        {
            // This should never happen, but just in case.
            if (Projectile.velocity == Vector2.Zero)
                return false;

            // If it isnt set, set the prim instance.
            LaserDrawer ??= new(WidthFunction, ColorFunction, GameShaders.Misc["FargowiltasSouls:MutantDeathray"]);

            // Get the laser end position.
            Vector2 laserEnd = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitY) * drawDistance;

            // Create 8 points that span across the draw distance from the projectile center.

            // This allows the drawing to be pushed back, which is needed due to the shader fading in at the start to avoid
            // sharp lines.
            Vector2 initialDrawPoint = Projectile.Center - Projectile.velocity * 400f;
            Vector2[] baseDrawPoints = new Vector2[8];
            for (int i = 0; i < baseDrawPoints.Length; i++)
                baseDrawPoints[i] = Vector2.Lerp(initialDrawPoint, laserEnd, i / (float)(baseDrawPoints.Length - 1f));

            // Set shader parameters. This one takes a fademap and a color.

            // The laser should fade to white in the middle.
            Color brightColor = new(194, 255, 242, 100);
            GameShaders.Misc["FargowiltasSouls:MutantDeathray"].UseColor(brightColor);
            // GameShaders.Misc["FargoswiltasSouls:MutantDeathray"].UseImage1(); cannot be used due to only accepting vanilla paths.
            GameShaders.Misc["FargowiltasSouls:MutantDeathray"].UseImage0(AssetDirectory.Invisible);//TODO: give this a texture
            // Draw a big glow above the start of the laser, to help mask the intial fade in due to the immense width.

            Texture2D glowTexture = ModContent.Request<Texture2D>("FargowiltasSouls/Projectiles/GlowRing").Value;

            Vector2 glowDrawPosition = Projectile.Center - Projectile.velocity * (BeBrighter ? 90f : 180f);

            Main.EntitySpriteDraw(glowTexture, glowDrawPosition - Main.screenPosition, null, brightColor, Projectile.rotation, glowTexture.Size() * 0.5f, Projectile.scale * 0.4f, SpriteEffects.None, 0);
            LaserDrawer.DrawPrims(baseDrawPoints, -Main.screenPosition, 60);
            return false;
        }*/

    }
}
