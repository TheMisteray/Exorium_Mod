using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Core.Utilities;
using ExoriumMod.Content.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.ModLoader.UI.ModBrowser;

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    internal class CaraveneClone : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + "Caravene";

        public override void SetDefaults()
        {
            Projectile.width = 140;
            Projectile.height = 236;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 290;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
        }

        public bool left
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value ? 1f : 0f;
        }

        public float startingCycle
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        private int frameX = 2;

        public override void AI()
        {
            if (Projectile.timeLeft == 260)
            {
                frameX = 1;
                Projectile.frameCounter = 0;
            }
            else if (Projectile.timeLeft < 180 && Projectile.alpha <= 255)
            {
                Projectile.alpha+=5;

                if (Projectile.alpha >= 0)
                    Projectile.hostile = false;
            }

            //Sprite frameX data stolen from boss
            switch (frameX)
            {
                case 0:
                case 2:
                case 4:
                case 6:
                case 8:
                case 10:
                    Projectile.frameCounter += 2;
                    break;

                case 1:
                case 3:
                    if (Projectile.frameCounter == 0)
                    {
                        SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                    }
                    else if (Projectile.frameCounter == 20)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2((!left ? Projectile.width : -Projectile.width), 0), Vector2.Zero, ProjectileType<SwordHitbox>(), Projectile.damage, 7, Main.myPlayer);
                    }
                    Projectile.frameCounter += 5;
                    break;
                case 7:
                    Projectile.frameCounter += 5;
                    break;

                case 5:
                case 9:
                    Projectile.frameCounter += 3;
                    break;
            }
            //Change column of animation used after a loop
            if (Projectile.frameCounter >= 60)
            {
                Projectile.frameCounter = 0;

                switch (frameX)
                {
                    case 1:
                    case 3:
                    case 5:
                    case 9:
                        frameX--;
                        break;/*
                    case 6:
                        if (shieldDown)
                            frameX++;
                        break;
                    case 7:
                        if (shieldDown)
                        {
                            frameX = 0;
                            shieldDown = false;
                        }
                        else
                            frameX--;
                        break;*/
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.CrimsonKnight + "Caravene").Value;

            //Fire Aura
            var fire = Filters.Scene["ExoriumMod:FireAura"].GetShader().Shader;
            fire.Parameters["noiseTexture"].SetValue(Request<Texture2D>(AssetDirectory.ShaderMap + "FlamingSphere").Value);
            fire.Parameters["gradientTexture"].SetValue(Request<Texture2D>(AssetDirectory.ShaderMap + "basicGradient").Value);
            fire.Parameters["uTime"].SetValue(Main.GameUpdateCount * 0.02f);
            fire.Parameters["uOpacity"].SetValue(Projectile.alpha);

            int ySourceHeight = (int)(Projectile.frameCounter / 10) * 442;
            int xSourceHeight = (int)(frameX * 412);
            Vector2 screenPos = Main.screenPosition;
            SpriteBatch spritebatch = Main.spriteBatch;

            if (true)
            {
                Texture2D auraTex = Request<Texture2D>(AssetDirectory.CrimsonKnight + "Aura").Value;
                Color alpha = new Color(255, 255, 255, Projectile.alpha);

                spritebatch.End();
                spritebatch.Begin(default, BlendState.NonPremultiplied, default, default, default, fire, Main.GameViewMatrix.ZoomMatrix);

                spritebatch.Draw(auraTex, Projectile.Center - screenPos + new Vector2(0, -150), null, alpha, 0, auraTex.Size() / 2, 1.8f, 0, 0);

                spritebatch.End();
                spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
            }

            Color alphaColor = Color.Lerp(new Color(0, 0, 0, 0), lightColor, (float)(-1 * (Projectile.alpha - 255)) / 255f);

            if (left)
            {
                spritebatch.Draw(tex,
                    new Rectangle((int)(Projectile.position.X - 221) - (int)(screenPos.X), (int)(Projectile.position.Y - 200) - (int)(screenPos.Y), 412, 442),
                    new Rectangle(xSourceHeight, ySourceHeight, 412, 442),
                    alphaColor,
                    Projectile.rotation,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0);

            }
            else
            {
                spritebatch.Draw(tex,
                    new Rectangle((int)(Projectile.position.X - 51) - (int)(screenPos.X), (int)(Projectile.position.Y - 200) - (int)(screenPos.Y), 412, 442),
                    new Rectangle(xSourceHeight, ySourceHeight, 412, 442),
                    alphaColor,
                    Projectile.rotation,
                    Vector2.Zero,
                    SpriteEffects.FlipHorizontally,
                    0);
            }
            return false;
        }
    }
}
