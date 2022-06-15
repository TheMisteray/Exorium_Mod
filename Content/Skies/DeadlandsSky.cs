using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Skies
{
    public class DeadlandsSky : CustomSky
    {
		private bool skyActive;

		private float intensity;

		public override void OnLoad()
		{
		}

		public override void Update(GameTime gameTime)
		{
			if (skyActive && intensity < 1f)
			{
				intensity += 0.01f;
			}
			else if (!skyActive && intensity > 0f)
			{
				intensity -= 0.005f;
			}
		}

		private float GetIntensity()
		{
			return 1f - Utils.SmoothStep(1000f, 6000f, 200f);
		}

		public override Color OnTileColor(Color inColor)
		{
			float amt = intensity * .3f;
			return inColor.MultiplyRGB(new Color(1f - amt, 1f - amt, 1f - amt));
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f)
			{
				//Light multiplier. Convert to system where dark at night and smooth transition over day.
				double timeMult;
				double trueTime = 0;
				if (Main.dayTime)
					trueTime = Main.time;
				if (!Main.dayTime) //dark at night
					trueTime = 0;
				timeMult = Math.Sin(MathHelper.PiOver2 * (1 - Math.Abs((trueTime - 27000) / 27000)));
				spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color((int)(120 * timeMult), (int)(120 * timeMult), (int)(120 * timeMult), 220) * intensity);
			}
		}

		public override float GetCloudAlpha() => 1 - intensity;

		public override void Activate(Vector2 position, params object[] args)
		{
			intensity = 0.002f;
			skyActive = true;
		}

		public override void Deactivate(params object[] args) => skyActive = false;

		public override void Reset() => skyActive = false;

		public override bool IsActive() => skyActive || intensity > 0.001f;
	}
}
