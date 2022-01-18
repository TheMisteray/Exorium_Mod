using Terraria;
using Microsoft.Xna.Framework;

namespace ExoriumMod.Helpers
{
    public static class DustHelper
    {
        /// <summary>
        /// Creates dust in a circle at the given point
        /// </summary>
        public static void DustCircle(Vector2 position, int dustType, float radius, float dustCount, float dustSize, float sizeVariance, int alpha, int alphaVariance, Color color, bool burstOutward)
        {
            Vector2 rad = new Vector2(0, Main.rand.NextFloat(radius));

            if (burstOutward)
            {
                for (int i = 0; i < dustCount; i++)
                {
                    Vector2 shootPoint = rad.RotatedBy(MathHelper.TwoPi);
                    Dust.NewDustPerfect(position, dustType, shootPoint, alpha + Main.rand.Next(alphaVariance, alphaVariance + 1), color, dustSize + Main.rand.NextFloat(sizeVariance, sizeVariance));
                }
            }
            else
            {
                for (int i = 0; i < dustCount; i++)
                {
                    Vector2 placePoint = rad.RotatedBy(MathHelper.TwoPi);
                    Dust.NewDustPerfect(position + placePoint, dustType, null, alpha + Main.rand.Next(alphaVariance, alphaVariance + 1), color, dustSize + Main.rand.NextFloat(sizeVariance, sizeVariance));
                }
            }
        }

        /// <summary>
        /// Creates a ring of dust
        /// </summary>
        public static void DustRing(Vector2 center, int dustType, float radius, float randomness, float density, float dustCount, float dustSize, float sizeVariance, int alpha, int alphaVariance, Color color, bool burstOutward)
        {
            Vector2 rad = new Vector2(0, radius);

            if (burstOutward)
            {
                for (float i = 0; i < MathHelper.TwoPi; i += density)
                {
                    float rand = 0;
                    if (randomness > 0) rand += randomness * Main.rand.NextFloat(-.01f, .01f);

                    Vector2 shootPoint = rad.RotatedBy(i + rand);
                    Dust.NewDustPerfect(center, dustType, shootPoint, alpha + Main.rand.Next(alphaVariance, alphaVariance + 1), color, dustSize + Main.rand.NextFloat(sizeVariance, sizeVariance));
                }
            }
            else
            {
                for (float i = 0; i < MathHelper.TwoPi; i += density)
                {
                    float rand = 0;
                    if (randomness > 0) rand += randomness * Main.rand.NextFloat(-.01f, .01f);

                    Vector2 placePoint = rad.RotatedBy(i + rand);
                    Dust.NewDustPerfect(center + placePoint, dustType, null, alpha + Main.rand.Next(alphaVariance, alphaVariance + 1), color, dustSize + Main.rand.NextFloat(sizeVariance, sizeVariance));
                }
            }
        }
    }
}
