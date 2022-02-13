using ExoriumMod.Core;
using ExoriumMod.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Enums;

namespace ExoriumMod.Content.Items.Weapons.Ranger
{
    class IonRay : ModItem
    {
        public override string Texture => AssetDirectory.RangerWeapon + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Charges up small lasers");
        }

        public override void SetDefaults()
        {
            item.damage = 28;
            item.ranged = true;
            item.width = 46;
            item.height = 26;
            item.useTime = 60;
            item.useAnimation = 60;
            item.channel = true;
            item.knockBack = 2;
            item.value = Item.sellPrice(silver: 48);
            item.rare = 1;
            item.UseSound = SoundID.Item93;
            item.shoot = ProjectileType<IonBeam>();
            item.noMelee = true;
            item.shootSpeed = 28;
            item.useStyle = 5;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(25));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MeteoriteBar, 24);
			recipe.AddIngredient(ItemID.HellstoneBar, 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 4);
        }
    }

    class IonBeam : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + Name;

		private const float MAX_CHARGE = 50f;
		//The distance charge particle from the player center
		private const float MOVE_DISTANCE = 60f;

		private const float LIFE_TIME = 30;

		private const int NumSamplePoints = 3;

		private const float BeamTileCollisionWidth = 1f;

		private const float BeamLengthChangeFactor = 0.75f;

		public float Distance
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

		public float LifeCounter
        {
			get => projectile.ai[1];
			set => projectile.ai[1] = value;
        }

		// Are we at max charge? With c#6 you can simply use => which indicates this is a get only property
		public bool IsAtMaxCharge => Charge == MAX_CHARGE;

		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ranged = true;
			projectile.hide = true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (Charge == MAX_CHARGE)
				DrawHelper.DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center, projectile.velocity, 10, -1.57f, 1f, 1000f, Color.Lime, (int)MOVE_DISTANCE, Distance);
			else
            {
				Texture2D tex = GetTexture(AssetDirectory.Projectile + Name + "Guide");
				float ratio = ((MAX_CHARGE - Charge) / MAX_CHARGE);
				Vector2 offset = new Vector2(0, 30 * ratio);
				Vector2 offset2 = offset.RotatedBy(projectile.velocity.ToRotation());
				DrawHelper.DrawLaser(spriteBatch, tex, Main.player[projectile.owner].Center - offset2, projectile.velocity, 10, MathHelper.PiOver2, 1 - ratio, 1000f, Color.Lime, (int)MOVE_DISTANCE, Distance);
				DrawHelper.DrawLaser(spriteBatch, tex, Main.player[projectile.owner].Center + (offset2 * 2), projectile.velocity, 10, MathHelper.PiOver2, 1 - ratio, 1000f, Color.Lime, (int)MOVE_DISTANCE, Distance);
				DrawHelper.DrawLaser(spriteBatch, tex, Main.player[projectile.owner].Center - (offset2 * 2), projectile.velocity, 10, MathHelper.PiOver2, 1 - ratio, 1000f, Color.Lime, (int)MOVE_DISTANCE, Distance);
			}
			return false;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (!IsAtMaxCharge) return false;

			Player player = Main.player[projectile.owner];
			Vector2 unit = projectile.velocity;
			float point = 0f;

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), player.Center,
				player.Center + unit * Distance, 2, ref point);
		}

		// The AI of the projectile
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			projectile.position = player.Center + projectile.velocity * MOVE_DISTANCE;
			projectile.timeLeft = 2;

			//Keep player in use
			player.heldProj = projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;

			//Set Length
			float hitscanBeamLength = PerformBeamHitscan(Main.player[projectile.owner], Charge == MAX_CHARGE);
			Distance = MathHelper.Lerp(Distance, hitscanBeamLength, BeamLengthChangeFactor);

			//Player can't turn once damage begins
			if (LifeCounter == 0)
				UpdatePlayer(player);
			ChargeLaser(player);

			//After charging complete
			if (Charge < MAX_CHARGE) return;

			LifeCounter++;
			CastLights();

			if (LifeCounter > LIFE_TIME)
				projectile.Kill();
		}

		private float PerformBeamHitscan(Player playerOwn, bool fullCharge)
		{
			// By default, the hitscan interpolation starts at the projectile's center.
			// If the host Prism is fully charged, the interpolation starts at the Prism's center instead.
			Vector2 samplingPoint = playerOwn.Center;

			// Perform a laser scan to calculate the correct length of the beam.
			// Alternatively, if you want the beam to ignore tiles, just set it to be the max beam length with the following line.
			// return MaxBeamLength;
			float[] laserScanResults = new float[NumSamplePoints];
			Collision.LaserScan(samplingPoint, projectile.velocity, BeamTileCollisionWidth * projectile.scale, 1000, laserScanResults);
			float averageLengthSample = 0f;
			for (int i = 0; i < laserScanResults.Length; ++i)
			{
				averageLengthSample += laserScanResults[i];
			}
			averageLengthSample /= NumSamplePoints;

			return averageLengthSample;
		}

		private void ChargeLaser(Player player)
		{
			// Kill the projectile if the player stops channeling
			if (!player.channel)
			{
				projectile.Kill();
			}
			else
			{
				Vector2 offset = projectile.velocity;
				offset *= MOVE_DISTANCE - 20;
				if (Charge < MAX_CHARGE)
				{
					Charge++;
					if (Charge == MAX_CHARGE)
                    {
						Main.PlaySound(SoundID.Item75, player.position);
						DustHelper.DustRing(projectile.Center - offset/2, DustType<Dusts.Rainbow>(), 5, 0, .2f, 1, 0, 0, 0, Color.Lime, true);
					}
				}
			}
		}

		private void UpdatePlayer(Player player)
		{
			// Multiplayer support here, only run this code if the client running it is the owner of the projectile
			if (projectile.owner == Main.myPlayer)
			{
				Vector2 diff = Main.MouseWorld - player.Center;
				diff.Normalize();
				projectile.velocity = diff;
				projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
				projectile.netUpdate = true;
			}
			int dir = projectile.direction;
			player.ChangeDir(dir); // Set player direction to where we are shooting
			player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir); // Set the item rotation to where we are shooting
		}

		private void CastLights()
		{
			// Cast a light along the line of the laser
			DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
			Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * (Distance - MOVE_DISTANCE), 26, DelegateMethods.CastLight);
		}

		public override bool ShouldUpdatePosition() => false;

		/*
			* Update CutTiles so the laser will cut tiles (like grass)
			*/
		public override void CutTiles()
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Vector2 unit = projectile.velocity;
			Utils.PlotTileLine(projectile.Center, projectile.Center + unit * Distance, (projectile.width + 16) * projectile.scale, DelegateMethods.CutTiles);
		}
	}
}
