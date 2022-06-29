using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using ExoriumMod.Content.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    class FlamingSphere : ModProjectile 
    {
        public override string Texture => AssetDirectory.CrimsonKnight + Name;

        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 200;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.scale = .02f;
        }

        public float Target
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public bool Expanded
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value ? 1f : 0f;
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 3600)
            {
                if (Main.netMode != NetmodeID.Server && !Filters.Scene["ExoriumMod:HeatDistortion"].IsActive())
                {
                    Texture2D heatMap = Request<Texture2D>(AssetDirectory.ShaderMap + "HeatDistortMap").Value;

                    Filters.Scene.Activate("ExoriumMod:HeatDistortion", Projectile.Center).GetShader().UseColor(2, 1, .2f).UseTargetPosition(Projectile.Center).UseImage(heatMap, 1);
                }
            }

            if (Main.netMode != NetmodeID.Server && Filters.Scene["ExoriumMod:HeatDistortion"].IsActive())
            {
                Filters.Scene["ExoriumMod:HeatDistortion"].GetShader().UseTargetPosition(Projectile.Center).UseOpacity(100).UseProgress(Main.GameUpdateCount * 0.002f); //Make use game time for stoppin while paused
            }

            Player p = Main.player[(int)Target];
            if (true)
            {
                Vector2 trajectory = Main.player[(int)Target].Center - Projectile.Center;
                if (trajectory.Length() == 0)
                {
                    trajectory = new Vector2(0, 1);
                }
                trajectory.Normalize();
                trajectory *= 4;

                if (Projectile.scale >= .95f)
                    Projectile.velocity = trajectory;

                /*
                if ((p.Center - Projectile.Center).Length() < 160)
                {
                    Expanded = true;
                    Projectile.timeLeft = 150;
                }
                */
            }

            if (Projectile.scale < 1)
            {
                Projectile.scale += .02f;
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Expanded)
            {
                int explosionArea = 320;
                Vector2 oldSize = Projectile.Size;
                // Resize the projectile hitbox to be bigger.
                Projectile.position = Projectile.Center;
                Projectile.Size += new Vector2(explosionArea);
                Projectile.Center = Projectile.position;

                Projectile.tileCollide = false;
                Projectile.velocity = Vector2.Zero;
                // Damage enemies inside the hitbox area
                Projectile.Damage();
                Projectile.Damage();
                Projectile.scale = 0.01f;

                //Resize the hitbox to its original size
                Projectile.position = Projectile.Center;
                Projectile.Size = new Vector2(50);
                Projectile.Center = Projectile.position;

                SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

                //Draw explosion area
            }

            if (Main.netMode != NetmodeID.Server && Filters.Scene["ExoriumMod:HeatDistortion"].IsActive())
            {
                Filters.Scene["ExoriumMod:HeatDistortion"].Deactivate();
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 900);
        }

        public override bool CanHitPlayer(Player target)
        {
            return ((target.Center - Projectile.Center).Length() < (target.width/2) + (Projectile.width/2));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var fire = Filters.Scene["ExoriumMod:FlamingSphere"].GetShader().Shader;
            fire.Parameters["sampleTexture2"].SetValue(Request<Texture2D>(AssetDirectory.ShaderMap + "FlamingSphereMap").Value);
            fire.Parameters["sampleTexture2"].SetValue(Request<Texture2D>(AssetDirectory.ShaderMap + "FlamingSphere").Value);
            fire.Parameters["uTime"].SetValue(Main.GameUpdateCount * 0.01f);

            SpriteBatch spriteBatch = Main.spriteBatch;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.NonPremultiplied, default, default, default, fire, Main.GameViewMatrix.ZoomMatrix);

            spriteBatch.Draw(Request<Texture2D>(AssetDirectory.CrimsonKnight + Name).Value, Projectile.Center - Main.screenPosition, null, Color.White, 0, Projectile.Size / 2, Projectile.scale * 2, 0, 0);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, SamplerState.PointWrap, default, default, default, Main.GameViewMatrix.ZoomMatrix);

            return true;
        }
    }
}
