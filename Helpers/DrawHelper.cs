using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.CodeAnalysis.Host.Mef;

namespace ExoriumMod.Helpers
{
    public static class DrawHelper
    {
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
		///
		public static void DrawLaser(Texture2D texture, Vector2 start, Vector2 unit, float step, Rectangle laserTail, Rectangle laserBody, Rectangle laserHead, float rotation = 0f, float scale = 1f, float maxDist = 2000f, Color color = default(Color), int transDist = 50, float distance = 2000f)
		{
			float r = unit.ToRotation() + rotation;

			// Draws the laser 'body'
			for (float i = transDist; i <= distance; i += step)
			{
				var origin = start + i * unit;
				Main.EntitySpriteDraw(texture, origin - Main.screenPosition,
					laserBody, i < transDist ? Color.Transparent : color, r,
					new Vector2(laserBody.Width/2, laserBody.Height/2), scale, 0, 0);
			}

			// Draws the laser 'tail'
			Main.EntitySpriteDraw(texture, start + unit * (transDist - step) - Main.screenPosition,
				laserTail, color, r, new Vector2(laserTail.Width/2, laserHead.Height/2), scale, 0, 0);

			// Draws the laser 'head'
			Main.EntitySpriteDraw(texture, start + (distance + step) * unit - Main.screenPosition,
				laserHead, color, r, new Vector2(laserHead.Width/2, laserHead.Height/2), scale, 0, 0);
		}
	}
}
