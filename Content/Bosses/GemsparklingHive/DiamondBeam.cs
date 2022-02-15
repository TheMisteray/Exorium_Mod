using ExoriumMod.Core;
using ExoriumMod.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExoriumMod.Content.Bosses.GemsparklingHive
{
    class DiamondBeam : ModProjectile
    {
		public override string Texture => AssetDirectory.GemsparklingHive + Name;

		private const float MAX_CHARGE = 50f;
		//The distance charge particle from the player center
		private const float MOVE_DISTANCE = 60f;

		private const float LIFE_TIME = 180;

		private const float TurnResponsiveness = 0.01f;

		private const float BeamLength = 1600f;

		private const int SoundInterval = 30;

		public float LifeCounter
		{
			get => projectile.ai[0];
			set => projectile.ai[0] = value;
		}

		// The actual charge value is stored in the localAI0 field
		public float Charge
		{
			get => projectile.localAI[0];
			set => projectile.localAI[0] = value;
		}

		public float NPCWhoAmI
        {
			get => projectile.ai[1];
			set => projectile.ai[1] = value;
		}

		// Are we at max charge? With c#6 you can simply use => which indicates this is a get only property
		public bool IsAtMaxCharge => Charge == MAX_CHARGE;

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			NPC npc = Main.npc[(int)NPCWhoAmI];
			Vector2 unitVel = projectile.velocity;
			unitVel.Normalize();

			if (Charge == MAX_CHARGE)
				DrawHelper.DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], npc.Center, unitVel, 10, -1.57f, 1f, BeamLength, default, 30, BeamLength);
			else
			{
				Texture2D tex = GetTexture(AssetDirectory.GemsparklingHive + Name + "Guide");
				DrawHelper.DrawLaser(spriteBatch, tex, npc.Center, unitVel, 10, MathHelper.PiOver2, 1, BeamLength, default, 30, BeamLength);
			}
			return false;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (!IsAtMaxCharge) return false;

			NPC npc = Main.npc[(int)NPCWhoAmI];
			Vector2 unit = projectile.velocity;
			float point = 0f;

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), npc.Center,
				npc.Center + unit * 1000, 2, ref point);
		}

		// The AI of the projectile
		public override void AI()
		{
			NPC npc = Main.npc[(int)NPCWhoAmI];
			projectile.position = npc.Center + projectile.velocity * MOVE_DISTANCE;
			projectile.timeLeft = 2;
			if (npc.alpha > 10) //Ends laser when gemsparkling hides
				projectile.timeLeft = 0;

			//Turn after damage begins
			if (LifeCounter > 0)
            {
				Update(npc);
				PlaySounds();
            }
			ChargeLaser(npc);

			//After charging complete
			if (Charge < MAX_CHARGE) return;

			LifeCounter++;
			CastLights();

			if (LifeCounter > LIFE_TIME)
				projectile.Kill();
			if (!npc.active || npc.type != NPCType<DiamondGemsparkling>())
				projectile.Kill();
		}

		private void ChargeLaser(NPC npc)
		{
			if (Charge < MAX_CHARGE)
			{
				Charge++;
				if (Charge == MAX_CHARGE)
				{
					DustHelper.DustRing(projectile.Center, DustType<Dusts.Rainbow>(), 5, 0, .2f, 1, 0, 0, 0, Color.White, true);
				}
			}
		}

		private void Update(NPC npc)
		{
			Vector2 aim = Vector2.Normalize(Main.player[npc.target].Center - npc.Center);
			if (aim.HasNaNs())
			{
				aim = -Vector2.UnitY;
			}

			// Change a portion of the Prism's current velocity so that it points to the mouse. This gives smooth movement over time.
			aim = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(projectile.velocity), aim, TurnResponsiveness));

			if (aim != projectile.velocity)
			{
				projectile.netUpdate = true;
			}
			projectile.velocity = aim;

			//Push npc
			Vector2 push = projectile.velocity;
			push.Normalize();
			npc.position += push * -1;
		}

		private void CastLights()
		{
			// Cast a light along the line of the laser
			DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
			Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * (BeamLength - MOVE_DISTANCE), 26, DelegateMethods.CastLight);
		}

		private void PlaySounds()
		{
			// The Prism makes sound intermittently while in use, using the vanilla projectile variable soundDelay.
			if (projectile.soundDelay <= 0)
			{
				projectile.soundDelay = SoundInterval;
				Main.PlaySound(SoundID.NPCDeath7, projectile.position);
			}
		}

		public override bool ShouldUpdatePosition() => false;
	}
}
