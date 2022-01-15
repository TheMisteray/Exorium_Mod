using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Dusts;
using System;

namespace ExoriumMod.Projectiles
{
    class BurningHand : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.height = 40;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.timeLeft = 25;
        }
		Dust dust81;
		int num2475;
		//ref float reference();
		public override void AI()
        {
			if (Main.rand.Next(3) == 0)
				FireDust();
			projectile.ai[0] += 1f; //Moved from FireDust to here to avoid extra calls since FireDust is called more than once per tick
			if (projectile.wet && !projectile.lavaWet) //water death
                projectile.active = false;
            if (projectile.ai[1] == 1)
            {
                projectile.alpha = 255;
                projectile.penetrate = -1;
                projectile.velocity = projectile.velocity * 0.95f;
				FireDust();
			}
            if (projectile.velocity != Vector2.Zero)
            {
                projectile.spriteDirection = projectile.direction;
                if (projectile.velocity.X >= 0)
                {
                    projectile.rotation = projectile.velocity.ToRotation();
                }
                else
                {
                    projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(180);
                }
            }
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.Kill();
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 420);
        }

        public override void Kill(int timeLeft)
        {
            if (projectile.ai[1] == 0)
            {
                projectile.velocity *= 0.4f;
                for (int i = 0; i < 25; i++)
                {
                    Vector2 randDir = projectile.velocity.RotatedByRandom(MathHelper.ToRadians(360));
					FireDust();
                    //Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 6, randDir.X, randDir.Y, 0, default, Main.rand.NextFloat(3) + 1);
                }
                Main.PlaySound(SoundID.Item89, projectile.position);
            }
        }

		private void FireDust()
        {
			//ai for dust taken from flamethrower
			if (projectile.type == 188 && projectile.ai[0] < 8f)
			{
				projectile.ai[0] = 8f;
			}
			if (projectile.timeLeft > 60)
			{
				projectile.timeLeft = 60;
			}
			float num2127 = 1f;
			if (projectile.ai[0] == 8f)
			{
				num2127 = 0.25f;
			}
			else if (projectile.ai[0] == 9f)
			{
				num2127 = 0.5f;
			}
			else if (projectile.ai[0] == 10f)
			{
				num2127 = 0.75f;
			}
			int num2126 = 6;
			if (projectile.type == 101)
			{
				num2126 = 75;
			}
			if (num2126 == 6 || Main.rand.Next(2) == 0)
			{
				for (int num2125 = 0; num2125 < 1; num2125 = num2475 + 1)
				{
					Vector2 position100 = new Vector2(projectile.position.X, projectile.position.Y);
					int width87 = projectile.width;
					int height87 = projectile.height;
					int num2484 = num2126;
					float speedX25 = projectile.velocity.X * 0.2f;
					float speedY29 = projectile.velocity.Y * 0.2f;
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
					if (projectile.type == 188)
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
						dust81.velocity += projectile.velocity;
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
