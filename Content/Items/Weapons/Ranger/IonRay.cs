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
using Terraria.Enums;
using Terraria.DataStructures;
using ReLogic.Content;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Weapons.Ranger
{
    class IonRay : ModItem
    {
        public override string Texture => AssetDirectory.RangerWeapon + Name;

        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Charges up small lasers");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 28;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 46;
            Item.height = 26;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.channel = true;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(silver: 48);
            Item.rare = 1;
            Item.UseSound = SoundID.Item93;
            Item.shoot = ProjectileType<IonBeam>();
            Item.noMelee = true;
            Item.shootSpeed = 28;
            Item.useStyle = 5;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(25));
			velocity = perturbedSpeed;
            return true;
        }

        public override void AddRecipes()
        {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MeteoriteBar, 24);
			recipe.AddIngredient(ItemID.HellstoneBar, 6);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
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
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		// The actual charge value is stored in the localAI0 field
		public float Charge
		{
			get => Projectile.localAI[0];
			set => Projectile.localAI[0] = value;
		}

		public float LifeCounter
        {
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
        }

		// Are we at max charge? With c#6 you can simply use => which indicates this is a get only property
		public bool IsAtMaxCharge => Charge == MAX_CHARGE;

		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.hide = true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteBatch spriteBatch = Main.spriteBatch;
			if (Charge == MAX_CHARGE)
				DrawHelper.DrawLaser(TextureAssets.Projectile[Projectile.type].Value, Main.player[Projectile.owner].Center, Projectile.velocity, 10, new Rectangle(0, 0, 28, 26), new Rectangle(0, 26, 28, 26), new Rectangle(0, 52, 28, 26) , - 1.57f, 1f, 1000f, Color.Lime, (int)MOVE_DISTANCE, Distance);
			else
            {
				Texture2D tex = Request<Texture2D>(AssetDirectory.Projectile + Name + "Guide").Value;
				float ratio = ((MAX_CHARGE - Charge) / MAX_CHARGE);
				Vector2 offset = new Vector2(0, 30 * ratio);
				Vector2 offset2 = offset.RotatedBy(Projectile.velocity.ToRotation());
				DrawHelper.DrawLaser(tex, Main.player[Projectile.owner].Center + offset2, Projectile.velocity, 10, new Rectangle(0, 0, 28, 26), new Rectangle(0, 26, 28, 26), new Rectangle(0, 52, 28, 26), MathHelper.PiOver2, 1 - ratio, 1000f, Color.Lime, (int)MOVE_DISTANCE, Distance);
				DrawHelper.DrawLaser(tex, Main.player[Projectile.owner].Center - offset2, Projectile.velocity, 10, new Rectangle(0, 0, 28, 26), new Rectangle(0, 26, 28, 26), new Rectangle(0, 52, 28, 26), MathHelper.PiOver2, 1 - ratio, 1000f, Color.Lime, (int)MOVE_DISTANCE, Distance);
				DrawHelper.DrawLaser(tex, Main.player[Projectile.owner].Center + (offset2 * 2), Projectile.velocity, 10, new Rectangle(0, 0, 28, 26), new Rectangle(0, 26, 28, 26), new Rectangle(0, 52, 28, 26), MathHelper.PiOver2, 1 - ratio, 1000f, Color.Lime, (int)MOVE_DISTANCE, Distance);
				DrawHelper.DrawLaser(tex, Main.player[Projectile.owner].Center - (offset2 * 2), Projectile.velocity, 10, new Rectangle(0, 0, 28, 26), new Rectangle(0, 26, 28, 26), new Rectangle(0, 52, 28, 26), MathHelper.PiOver2, 1 - ratio, 1000f, Color.Lime, (int)MOVE_DISTANCE, Distance);
			}
			return false;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (!IsAtMaxCharge) return false;

			Player player = Main.player[Projectile.owner];
			Vector2 unit = Projectile.velocity;
			float point = 0f;

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), player.Center,
				player.Center + unit * Distance, 2, ref point);
		}

		// The AI of the projectile
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Projectile.position = player.Center + Projectile.velocity * MOVE_DISTANCE;
			Projectile.timeLeft = 2;

			//Keep player in use
			player.heldProj = Projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;

			//Set Length
			float hitscanBeamLength = PerformBeamHitscan(Main.player[Projectile.owner], Charge == MAX_CHARGE);
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
				Projectile.Kill();
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
			Collision.LaserScan(samplingPoint, Projectile.velocity, BeamTileCollisionWidth * Projectile.scale, 1000, laserScanResults);
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
				Projectile.Kill();
			}
			else
			{
				Vector2 offset = Projectile.velocity;
				offset *= MOVE_DISTANCE - 20;
				if (Charge < MAX_CHARGE)
				{
					Charge++;
					if (Charge == MAX_CHARGE)
                    {
						SoundEngine.PlaySound(SoundID.Item75, player.position);
						DustHelper.DustRing(Projectile.Center - offset/2, DustType<Dusts.Rainbow>(), 5, 0, .2f, 1, 0, 0, 0, Color.Lime, true);
					}
				}
			}
		}

		private void UpdatePlayer(Player player)
		{
			// Multiplayer support here, only run this code if the client running it is the owner of the projectile
			if (Projectile.owner == Main.myPlayer)
			{
				Vector2 diff = Main.MouseWorld - player.Center;
				diff.Normalize();
				Projectile.velocity = diff;
				Projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
				Projectile.netUpdate = true;
			}
			int dir = Projectile.direction;
			player.ChangeDir(dir); // Set player direction to where we are shooting
			player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * dir, Projectile.velocity.X * dir); // Set the item rotation to where we are shooting
		}

		private void CastLights()
		{
			// Cast a light along the line of the laser
			DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
			Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * (Distance - MOVE_DISTANCE), 26, DelegateMethods.CastLight);
		}

		public override bool ShouldUpdatePosition() => false;

		/*
			* Update CutTiles so the laser will cut tiles (like grass)
			*/
		public override void CutTiles()
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Vector2 unit = Projectile.velocity;
			Utils.PlotTileLine(Projectile.Center, Projectile.Center + unit * Distance, (Projectile.width + 16) * Projectile.scale, DelegateMethods.CutTiles);
		}
	}
}
