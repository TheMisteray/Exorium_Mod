using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using ExoriumMod.Content.Dusts;
using Microsoft.Xna.Framework.Graphics;
using ExoriumMod.Core.Utilities;
using Terraria.Graphics.Shaders;

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    class CaraveneFireball : ModProjectile, IDrawAdditive
    {
        public override string Texture => AssetDirectory.CrimsonKnight + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fireball");
        }

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

        public bool Enrage
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value ? 1f : 0f;
        }

        public override void AI()
        {
            Projectile.velocity.Y += .20f;
            Projectile.rotation += .2f;
            if (Main.rand.NextBool(3))
            {
                //Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<BlightDust>(), 0, 0);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            /*
            SpriteBatch spriteBatch = Main.spriteBatch;

            spriteBatch.End(); 
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            MiscShaderData shader = GameShaders.Misc["ExoriumMod:HeatDistortion"];

            shader.Shader.Parameters["scale"].SetValue(1);
            shader.Shader.Parameters["strength"].SetValue(.5f);
            shader.Shader.Parameters["heatDistort"].SetValue(ModContent.Request<Texture2D>(AssetDirectory.ShaderMap + "HeatDistortMap").Value);
            shader.Shader.CurrentTechnique.Passes[0].Apply();

            Texture2D area = ModContent.Request<Texture2D>(AssetDirectory.Effect + "Glows/BasicGlow").Value;

            spriteBatch.Draw(area, Projectile.Center, null, Color.White, 0, area.Size() / 2, Projectile.scale, SpriteEffects.None, 0);

            spriteBatch.End(); 
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            */

            return base.PreDraw(ref lightColor);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, Enrage ? 600 : 300);
        }

        public void AdditiveCall(SpriteBatch spriteBatch)
        {

        }
    }
}
