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
            if (burstOutward)
            {
                for (int i = 0; i < dustCount; i++)
                {
                    Vector2 rad = new Vector2(0, Main.rand.NextFloat(radius));
                    Vector2 shootPoint = rad.RotatedBy(Main.rand.NextFloat(0,MathHelper.TwoPi));
                    Dust.NewDustPerfect(position, dustType, shootPoint, alpha + Main.rand.Next(-alphaVariance, alphaVariance + 1), color, dustSize + Main.rand.NextFloat(-sizeVariance, sizeVariance));
                }
            }
            else
            {
                for (int i = 0; i < dustCount; i++)
                {
                    Vector2 rad = new Vector2(0, Main.rand.NextFloat(radius));
                    Vector2 placePoint = rad.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi));
                    Dust.NewDustPerfect(position + placePoint, dustType, null, alpha + Main.rand.Next(-alphaVariance, alphaVariance + 1), color, dustSize + Main.rand.NextFloat(-sizeVariance, sizeVariance));
                }
            }
        }

        /// <summary>
        /// Creates a ring of dust
        /// </summary>
        public static void DustRing(Vector2 center, int dustType, float radius, float randomness, float density, float dustSize, float sizeVariance, int alpha, int alphaVariance, Color color, bool burstOutward)
        {
            Vector2 rad = new Vector2(0, radius);

            if (burstOutward)
            {
                for (float i = 0; i < MathHelper.TwoPi; i += density)
                {
                    float rand = 0;
                    if (randomness > 0) rand += randomness * Main.rand.NextFloat(-.01f, .01f);

                    Vector2 shootPoint = rad.RotatedBy(i + rand);
                    Dust.NewDustPerfect(center, dustType, shootPoint, alpha + Main.rand.Next(-alphaVariance, alphaVariance + 1), color, dustSize + Main.rand.NextFloat(-sizeVariance, sizeVariance));
                }
            }
            else
            {
                for (float i = 0; i < MathHelper.TwoPi; i += density)
                {
                    float rand = 0;
                    if (randomness > 0) rand += randomness * Main.rand.NextFloat(-.01f, .01f);

                    Vector2 placePoint = rad.RotatedBy(i + rand);
                    Dust.NewDustPerfect(center + placePoint, dustType, null, alpha + Main.rand.Next(-alphaVariance, alphaVariance + 1), color, dustSize + Main.rand.NextFloat(-sizeVariance, sizeVariance));
                }
            }
        }

        public static void DustLine(Vector2 start, Vector2 end, int dustType, float density = 20, float dustSize = 1, float sizeVariance = 0, int alpha = 0, int alphaVariance = 0, Color color = default, bool noGravity = false)
        {
            Vector2 diff = end - start;
            Vector2 segment = diff / density;
            for (int i = 0; i < density; i++)
            {
                Dust d = Dust.NewDustPerfect(start + segment * i, dustType, null, alpha + Main.rand.Next(-alphaVariance, alphaVariance + 1), color, dustSize + Main.rand.NextFloat(-sizeVariance, sizeVariance));
                if (noGravity)
                    d.noGravity = true;
            }
        }

        public static void FireDust(Projectile Projectile)
        {
            //These were moved in from outside
            Dust dust81;
            int num2475;

            //ai for dust taken from flamethrower
            if (Projectile.type == 188 && Projectile.ai[0] < 8f)
            {
                Projectile.ai[0] = 8f;
            }
            if (Projectile.timeLeft > 60)
            {
                Projectile.timeLeft = 60;
            }
            float num2127 = 1f;
            if (Projectile.ai[0] == 8f)
            {
                num2127 = 0.25f;
            }
            else if (Projectile.ai[0] == 9f)
            {
                num2127 = 0.5f;
            }
            else if (Projectile.ai[0] == 10f)
            {
                num2127 = 0.75f;
            }
            int num2126 = 6;
            if (Projectile.type == 101)
            {
                num2126 = 75;
            }
            if (num2126 == 6 || Main.rand.Next(2) == 0)
            {
                for (int num2125 = 0; num2125 < 1; num2125 = num2475 + 1)
                {
                    Vector2 position100 = new Vector2(Projectile.position.X, Projectile.position.Y);
                    int width87 = Projectile.width;
                    int height87 = Projectile.height;
                    int num2484 = num2126;
                    float speedX25 = Projectile.velocity.X * 0.2f;
                    float speedY29 = Projectile.velocity.Y * 0.2f;
                    Color newColor5 = default(Color);
                    int num2124 = Dust.NewDust(position100, width87, height87, num2484, speedX25, speedY29, 100, newColor5, 1f);
                    if (Main.rand.Next(3) != 0 || (num2126 == 75 && Main.rand.Next(3) == 0))
                    {
                        Main.dust[num2124].noGravity = true;
                        dust81 = Main.dust[num2124];
                        dust81.scale *= 3f;
                        Dust expr_DD41_cp_0 = Main.dust[num2124];
                        expr_DD41_cp_0.velocity.X = expr_DD41_cp_0.velocity.X * 2f;
                        Dust expr_DD61_cp_0 = Main.dust[num2124];
                        expr_DD61_cp_0.velocity.Y = expr_DD61_cp_0.velocity.Y * 2f;
                    }
                    if (Projectile.type == 188)
                    {
                        dust81 = Main.dust[num2124];
                        dust81.scale *= 1.25f;
                    }
                    else
                    {
                        dust81 = Main.dust[num2124];
                        dust81.scale *= 1.5f;
                    }
                    Dust expr_DDC6_cp_0 = Main.dust[num2124];
                    expr_DDC6_cp_0.velocity.X = expr_DDC6_cp_0.velocity.X * 1.2f;
                    Dust expr_DDE6_cp_0 = Main.dust[num2124];
                    expr_DDE6_cp_0.velocity.Y = expr_DDE6_cp_0.velocity.Y * 1.2f;
                    dust81 = Main.dust[num2124];
                    dust81.scale *= num2127;
                    if (num2126 == 75)
                    {
                        dust81 = Main.dust[num2124];
                        dust81.velocity += Projectile.velocity;
                        if (!Main.dust[num2124].noGravity)
                        {
                            dust81 = Main.dust[num2124];
                            dust81.velocity *= 0.5f;
                        }
                    }
                    num2475 = num2125;
                }
            }
        }
    }
}
