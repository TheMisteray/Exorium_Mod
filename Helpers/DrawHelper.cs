using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Helpers
{
    public static class DrawHelper
    {
		//TODO: rewrite this to work with other laser sizes
		/// <summary>
		/// Draws lasers
		/// </summary>
		/// <param name="spriteBatch">game spritebatch</param>
		/// <param name="texture">texture of laser</param>
		/// <param name="start">start pos of laser</param>
		/// <param name="unit">unitVector of laser direction</param>
		/// <param name="step">distance between each part of the laser</param>
		/// <param name="rotation">rotation added onto inital draw direction</param>
		/// <param name="scale"></param>
		/// <param name="maxDist">Max distance the laser can draw</param>
		/// <param name="color">Color override</param>
		/// <param name="transDist">distance from origin of start of laser</param>
		/// <param name="distance">Total length of laser</param>
		public static void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, float rotation = 0f, float scale = 1f, float maxDist = 2000f, Color color = default(Color), int transDist = 50, float distance = 2000f)
		{
			float r = unit.ToRotation() + rotation;

			// Draws the laser 'body'
			for (float i = transDist; i <= distance; i += step)
			{
				Color c = Color.White;
				var origin = start + i * unit;
				spriteBatch.Draw(texture, origin - Main.screenPosition,
					new Rectangle(0, 26, 28, 26), i < transDist ? Color.Transparent : c, r,
					new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
			}

			// Draws the laser 'tail'
			spriteBatch.Draw(texture, start + unit * (transDist - step) - Main.screenPosition,
				new Rectangle(0, 0, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);

			// Draws the laser 'head'
			spriteBatch.Draw(texture, start + (distance + step) * unit - Main.screenPosition,
				new Rectangle(0, 52, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
		}
	}
}
