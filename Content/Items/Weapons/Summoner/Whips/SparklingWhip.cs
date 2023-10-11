using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.GameContent;
using ExoriumMod.Core;
using ExoriumMod.Helpers;
using Terraria.Audio;
using ExoriumMod.Content.Buffs.Minions;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Weapons.Summoner.Whips
{
	public class SparklingWhip : ModItem
	{
		public override string Texture => AssetDirectory.SummonerWhip + Name;

        public override void SetStaticDefaults()
		{
			//CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			/* Tooltip.SetDefault("8 summon tag damage\n"
				+"Your summons will focus struck enemies\n"
				+"Incredibly heavy, may break"); */
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

		public override void SetDefaults()
		{
			// This method quickly sets the whip's properties.
			// Mouse over to see its parameters.
			Item.DefaultToWhip(ModContent.ProjectileType<SparklingWhipProjectile>(), 21, 3, 3, 60);

			Item.shootSpeed = 3;
			Item.rare = ItemRarityID.Green;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Amethyst)
				.AddIngredient(ItemID.Topaz)
				.AddIngredient(ItemID.Sapphire)
				.AddIngredient(ItemID.Emerald)
				.AddIngredient(ItemID.Ruby)
				.AddIngredient(ItemID.Amber)
				.AddIngredient(ItemID.Chain, 10)
				.AddIngredient(ItemID.PlatinumBar, 10)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.Amethyst)
				.AddIngredient(ItemID.Topaz)
				.AddIngredient(ItemID.Sapphire)
				.AddIngredient(ItemID.Emerald)
				.AddIngredient(ItemID.Ruby)
				.AddIngredient(ItemID.Amber)
				.AddIngredient(ItemID.Chain, 10)
				.AddIngredient(ItemID.GoldBar, 10)
				.AddTile(TileID.Anvils)
				.Register();
		}

		// Makes the whip receive melee prefixes
		public override bool MeleePrefix()
		{
			return true;
		}
	}

	public class SparklingWhipProjectile : ModProjectile
    {
		public override string Texture => AssetDirectory.WhipProjectile + Name;

        public override void SetStaticDefaults()
		{
			// This makes the projectile use whip collision detection and allows flasks to be applied to it.
			ProjectileID.Sets.IsAWhip[Type] = true;
			// DisplayName.SetDefault("SparklingWhip");
		}

		public override void SetDefaults()
		{
			// This method quickly sets the whip's properties.
			Projectile.DefaultToWhip();

			// use these to change from the vanilla defaults
			// Projectile.WhipSettings.Segments = 20;
			// Projectile.WhipSettings.RangeMultiplier = 1f;
		}

		private float Timer
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public bool BrokenTip
		{
			get => Projectile.ai[1] == 1f;
			set => Projectile.ai[1] = value ? 1f : 0f;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
			target.AddBuff(ModContent.BuffType<SparklingWhipTag>(), 300);

			switch (Main.rand.Next(6))
            {
				case 0:
					DustHelper.DustRing(target.Center, DustID.GemRuby, 2, 0, .3f, .5f, 0, 0, 0, Color.White, true);
					break;
				case 1:
					DustHelper.DustRing(target.Center, DustID.GemAmber, 2, 0, .3f, .5f, 0, 0, 0, Color.White, true);
					break;
				case 2:
					DustHelper.DustRing(target.Center, DustID.GemTopaz, 2, 0, .3f, .5f, 0, 0, 0, Color.White, true);
					break;
				case 3:
					DustHelper.DustRing(target.Center, DustID.GemEmerald, 2, 0, .3f, .5f, 0, 0, 0, Color.White, true);
					break;
				case 4:
					DustHelper.DustRing(target.Center, DustID.GemSapphire, 2, 0, .3f, .5f, 0, 0, 0, Color.White, true);
					break;
				default:
					DustHelper.DustRing(target.Center, DustID.GemAmethyst, 2, 0, .3f, .5f, 0, 0, 0, Color.White, true);
					break;
			}
		}

		// This method draws a line between all points of the whip, in case there's empty space between the sprites.
		private void DrawLine(List<Vector2> list)
		{
			Texture2D texture = TextureAssets.FishingLine.Value;
			Rectangle frame = texture.Frame();
			Vector2 origin = new Vector2(frame.Width / 2, 2);

			Vector2 pos = list[0];
			for (int i = 0; i < list.Count - 1; i++)
			{
				Vector2 element = list[i];
				Vector2 diff = list[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2;
				Color color = Lighting.GetColor(element.ToTileCoordinates(), Color.White);
				Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

				Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

				pos += diff;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			List<Vector2> list = new List<Vector2>();
			Projectile.FillWhipControlPoints(Projectile, list);

			DrawLine(list);

			//Vanilla draw removed
			//Main.DrawWhip_WhipBland(Projectile, list);

			SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			Main.instance.LoadProjectile(Type);
			Texture2D texture = TextureAssets.Projectile[Type].Value;

			Vector2 pos = list[0];

			for (int i = 0; i < list.Count - 1; i++)
			{
				Rectangle frame = new Rectangle(0, 0, 18, 24);
				Vector2 origin = new Vector2(9, 9);
				float scale = 1;

				// These statements determine what part of the spritesheet to draw for the current segment.
				// They can also be changed to suit your sprite.
				if (i == 0)
                {
					frame.Y = 0;
					frame.Height = 18;
				}
				if (i == list.Count - 2)
				{
					frame.Y = 132;
					frame.Height = 18;

					// For a more impactful look, this scales the tip of the whip up when fully extended, and down when curled up.
					Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
					float t = Timer / timeToFlyOut;
					scale = MathHelper.Lerp(0.5f, 1.5f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));

					if (Timer > (timeToFlyOut/3) * 2 && !BrokenTip)
                    {
						BrokenTip = true;
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), list[i], Projectile.velocity * 3, ModContent.ProjectileType<SparklingWhipHead>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
				}
				else if ((i - 1) % 6 == 0)
				{
					frame.Y = 24;
					frame.Height = 16;
				}
				else if ((i - 1) % 6 == 1)
				{
					frame.Y = 42;
					frame.Height = 16;
				}
				else if ((i - 1) % 6 == 2)
				{
					frame.Y = 60;
					frame.Height = 16;
				}
				else if ((i - 1) % 6 == 3)
				{
					frame.Y = 78;
					frame.Height = 16;
				}
				else if ((i - 1) % 6 == 4)
				{
					frame.Y = 96;
					frame.Height = 16;
				}
				else if ((i - 1) % 6 == 5)
				{
					frame.Y = 114;
					frame.Height = 16;
				}
				Vector2 element = list[i];
				Vector2 diff = list[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2; // This projectile's sprite faces down, so PiOver2 is used to correct rotation.
				Color color = Lighting.GetColor(element.ToTileCoordinates());

				if (!((i == list.Count - 2) && BrokenTip == true))//Not if the tip came off
					Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);

				pos += diff;
			}
			return false;
		}
	}

	public class SparklingWhipHead : ModProjectile
    {
		public override string Texture => AssetDirectory.WhipProjectile + Name;

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 16;
			Projectile.alpha = 0;
			Projectile.timeLeft = 300;
			Projectile.penetrate = 2;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
			Projectile.DamageType = DamageClass.Summon;
		}

		public override void AI()
		{
			Projectile.rotation += .25f * Projectile.direction;
			Projectile.velocity.Y += .3f;
			Projectile.velocity.X *= .988f;
		}

        public override void OnKill(int timeLeft)
        {
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            base.OnKill(timeLeft);
        }
    }
}
