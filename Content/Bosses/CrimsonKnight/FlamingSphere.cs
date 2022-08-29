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

        private float scalar;
        private bool lightener = false;
        private float killTimer = 0;
        private bool exploded = false;

        public override void AI()
        {
            /*
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
                Filters.Scene["ExoriumMod:HeatDistortion"].GetShader().UseTargetPosition(Projectile.Center).UseOpacity(100).UseProgress(Main.GameUpdateCount * 0.0015f); //Make use game time for stoppin while paused
            }
            */

            Player p = Main.player[(int)Target];
            Vector2 trajectory = Main.player[(int)Target].Center - Projectile.Center;
            if (trajectory.Length() == 0)
            {
                trajectory = new Vector2(0, 1);
            }
            trajectory.Normalize();
            trajectory *= 4;

            if (scalar >= .95f && !Expanded)
                Projectile.velocity = trajectory;
            else if (Expanded)
            {
                Projectile.velocity *= 0.95f;
                Helpers.DustHelper.DustRing(Projectile.Center, DustType<Rainbow>(), 360, 6, .04f, .5f, 0, 0, 0, Color.OrangeRed, false);
            }

                
            if ((p.Center - Projectile.Center).Length() < 300 && !Expanded && Projectile.timeLeft <= 3540)
            {
                Expanded = true;
                Projectile.timeLeft = 240;
                lightener = true;
            }
            if (Projectile.timeLeft <= 240)
            {
                if (scalar < 1.4f)
                    scalar += .01f;
            }

            if (scalar < 1)
            {
                scalar += .02f;
            }
        }

        public override bool PreKill(int timeLeft)
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
                scalar = 0.01f;

                //Resize the hitbox to its original size
                Projectile.position = Projectile.Center;
                Projectile.Size = new Vector2(50);
                Projectile.Center = Projectile.position;

                SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

                exploded = true;
                killTimer = 30;
            }

            if (exploded)
            {
                killTimer--;
            }

            /*
            if (Main.netMode != NetmodeID.Server && Filters.Scene["ExoriumMod:HeatDistortion"].IsActive())
            {
                Filters.Scene["ExoriumMod:HeatDistortion"].Deactivate();
            }
            */

            if (killTimer > 0)
                return false;
            else
                return false;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 900);
        }

        public override bool CanHitPlayer(Player target)
        {
            return ((target.Center - Projectile.Center).Length() < (target.width/2) + (Projectile.width/2) * scalar);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var fire = Filters.Scene["ExoriumMod:FlamingSphere"].GetShader().Shader;
            fire.Parameters["sampleTexture2"].SetValue(Request<Texture2D>(AssetDirectory.ShaderMap + "FlamingSphere").Value);
            fire.Parameters["sampleTexture3"].SetValue(Request<Texture2D>(AssetDirectory.ShaderMap + "FlamingSphere").Value);
            fire.Parameters["uTime"].SetValue(Main.GameUpdateCount * 0.01f);

            SpriteBatch spriteBatch = Main.spriteBatch;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.NonPremultiplied, default, default, default, fire, Main.GameViewMatrix.ZoomMatrix);

            spriteBatch.Draw(Request<Texture2D>(AssetDirectory.CrimsonKnight + Name).Value, Projectile.Center - Main.screenPosition, null, lightener ? Color.Red : Color.White, 0, Projectile.Size / 2, scalar * 1.8f, 0, 0);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, SamplerState.PointWrap, default, default, default, Main.GameViewMatrix.ZoomMatrix);

            if (killTimer > 0)
            {
                Texture2D blastArea = Request<Texture2D>(AssetDirectory.CrimsonKnight + Name).Value;
                Main.EntitySpriteDraw(blastArea, Projectile.Center, null, Color.OrangeRed, 0, blastArea.Size() / 2, 320 / (blastArea.Width / 2), SpriteEffects.None, 0);
            }

            return false;
        }
    }

    class backupFireball : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + "FlameBreath";

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
        }

        public float playerTarget
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        //draw stuff
        Vector2 oldPos1 = Vector2.Zero;
        Vector2 oldPos2 = Vector2.Zero;
        Vector2 oldPos3 = Vector2.Zero;
        Vector2 oldPos4 = Vector2.Zero;
        Vector2 oldPos5 = Vector2.Zero;
        Vector2 oldPos6 = Vector2.Zero;
        Vector2 oldPos7 = Vector2.Zero;

        public override void AI()
        {
            //Minor homing in expert mode
            if (Main.expertMode)
            {
                Player player = Main.player[(int)playerTarget];
                Vector2 toPlayer = player.Center - Projectile.Center;
                Vector2 aim = Vector2.Zero;

                if (player.active && toPlayer.Length() < 500)
                {
                    aim = Vector2.Lerp(Vector2.Normalize(Projectile.velocity), Vector2.Normalize(toPlayer), Main.masterMode ? .06f : .03f); //Home harder in expert

                    if (aim.ToRotation() != Projectile.velocity.ToRotation())
                    {
                        Projectile.netUpdate = true;
                    }
                    Projectile.velocity = aim * Projectile.velocity.Length();
                }
            }

            //Set draw positions
            if (Projectile.timeLeft % 5 == 0)
            {
                oldPos7 = oldPos6;
                oldPos6 = oldPos5;
                oldPos5 = oldPos4;
                oldPos4 = oldPos3;
                oldPos3 = oldPos2;
                oldPos2 = oldPos1;
                oldPos1 = Projectile.Center;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.CrimsonKnight + "FlameBreath", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Main.EntitySpriteDraw(tex, oldPos1 - Main.screenPosition, new Rectangle(0, 0, tex.Width, tex.Height / 7), new Color(255, 255, 255, 0), 0, new Vector2(tex.Width / 2, tex.Width / 2), .33f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(tex, oldPos2 - Main.screenPosition, new Rectangle(0, tex.Height / 7, tex.Width, tex.Height / 7), new Color(255, 255, 255, 0), 0, new Vector2(tex.Width / 2, tex.Width / 2), .33f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(tex, oldPos3 - Main.screenPosition, new Rectangle(0, (tex.Height / 7) * 2, tex.Width, tex.Height / 7), new Color(255, 255, 255, 0), 0, new Vector2(tex.Width / 2, tex.Width / 2), .33f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(tex, oldPos4 - Main.screenPosition, new Rectangle(0, (tex.Height / 7) * 3, tex.Width, tex.Height / 7), new Color(255, 255, 255, 0), 0, new Vector2(tex.Width / 2, tex.Width / 2), .33f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(tex, oldPos5 - Main.screenPosition, new Rectangle(0, (tex.Height / 7) * 4, tex.Width, tex.Height / 7), new Color(255, 255, 255, 0), 0, new Vector2(tex.Width / 2, tex.Width / 2), .33f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(tex, oldPos6 - Main.screenPosition, new Rectangle(0, (tex.Height / 7) * 5, tex.Width, tex.Height / 7), new Color(255, 255, 255, 0), 0, new Vector2(tex.Width / 2, tex.Width / 2), .33f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(tex, oldPos7 - Main.screenPosition, new Rectangle(0, (tex.Height / 7) * 6, tex.Width, tex.Height / 7), new Color(255, 255, 255, 0), 0, new Vector2(tex.Width / 2, tex.Width / 2), .33f, SpriteEffects.None, 0);
            return false;
        }
    }
}
