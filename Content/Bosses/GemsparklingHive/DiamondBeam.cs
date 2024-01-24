using ExoriumMod.Core;
using ExoriumMod.Helpers;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
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
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		// The actual charge value is stored in the localAI0 field
		public float Charge
		{
			get => Projectile.localAI[0];
			set => Projectile.localAI[0] = value;
		}

		public float NPCWhoAmI
        {
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		// Are we at max charge? With c#6 you can simply use => which indicates this is a get only property
		public bool IsAtMaxCharge => Charge == MAX_CHARGE;

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			NPC npc = Main.npc[(int)NPCWhoAmI];
			Vector2 unitVel = Projectile.velocity;
			unitVel.Normalize();

			if (Charge == MAX_CHARGE)
				DrawHelper.DrawLaser(TextureAssets.Projectile[Projectile.type].Value, npc.Center, unitVel, 10, -1.57f, 1f, BeamLength, default, 30, BeamLength);
			else
			{
				Texture2D tex = Request<Texture2D>(AssetDirectory.GemsparklingHive + Name + "Guide").Value;
				DrawHelper.DrawLaser(tex, npc.Center, unitVel, 10, MathHelper.PiOver2, 1, BeamLength, default, 30, BeamLength);
			}
			return false;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (!IsAtMaxCharge) return false;

			NPC npc = Main.npc[(int)NPCWhoAmI];
			Vector2 unit = Projectile.velocity;
			float point = 0f;

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), npc.Center,
				npc.Center + unit * 1000, 2, ref point);
		}

		// The AI of the projectile
		public override void AI()
		{
			NPC npc = Main.npc[(int)NPCWhoAmI];
			Projectile.position = npc.Center + Projectile.velocity * MOVE_DISTANCE;
			Projectile.timeLeft = 2;
			if (npc.alpha > 10) //Ends laser when gemsparkling hides
				Projectile.timeLeft = 0;

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
				Projectile.Kill();
			if (!npc.active || npc.type != NPCType<DiamondGemsparkling>())
				Projectile.Kill();
		}

		private void ChargeLaser(NPC npc)
		{
			if (Charge < MAX_CHARGE)
			{
				Charge++;
				if (Charge == MAX_CHARGE)
				{
					DustHelper.DustRing(Projectile.Center, DustType<Dusts.Rainbow>(), 5, 0, .2f, 1, 0, 0, 0, Color.White, true);
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

			aim = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), aim, TurnResponsiveness));

			if (aim != Projectile.velocity)
			{
				Projectile.netUpdate = true;
			}
			Projectile.velocity = aim;

			//Push npc
			Vector2 push = Projectile.velocity;
			push.Normalize();
			npc.position += push * -1;
		}

		private void CastLights()
		{
			// Cast a light along the line of the laser
			DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
			Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * (BeamLength - MOVE_DISTANCE), 26, DelegateMethods.CastLight);
		}

		private void PlaySounds()
		{
			// The Prism makes sound intermittently while in use, using the vanilla projectile variable soundDelay.
			if (Projectile.soundDelay <= 0)
			{
				Projectile.soundDelay = SoundInterval;
				SoundEngine.PlaySound(SoundID.NPCDeath7, Projectile.position);
			}
		}

		public override bool ShouldUpdatePosition() => false;
	}
}
