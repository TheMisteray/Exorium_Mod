using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Content.Projectiles.Minions
{
    class Wum : ModProjectile
    {
		public override string Texture => AssetDirectory.Minion + Name;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wum");
            // Sets the amount of frames this minion has on its spritesheet
            Main.projFrames[Projectile.type] = 18;
            // this is necessary for right-click targeting
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            // These below are needed for a minion
            // Denotes that this projectile is a pet or minion
            Main.projPet[Projectile.type] = true;
            // this is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            // Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public sealed override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 26;
            Projectile.height = 44;
            // Makes the minion go through tiles freely
            Projectile.tileCollide = true;

            // These below are needed for a minion weapon
            // Only controls if it deals damage to enemies on contact (more on that later)
            Projectile.friendly = true;
            // Only determines the damage type
            Projectile.minion = true;
            // Amount of slots projectile minion occupies from the total minion slots available to the player (more on that later)
            Projectile.minionSlots = 1f;
            // Needed so the minion doesn't despawn on collision with enemies or tiles
            Projectile.penetrate = -1;
        }

        // Here you can decide if your minion breaks things like grass or pots
        public override bool? CanCutTiles()
        {
            return false;
        }

        // this is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
        public override bool MinionContactDamage()
        {
            return false;
        }

        public override void AI()
        {
			bool flag;
			bool flag2;
			bool flag6;
			bool flag7;
			bool flag9;
			float num132;
			Player player = Main.player[Projectile.owner];
			if (player.dead || !player.active)
			{
				player.ClearBuff(BuffType<Buffs.Minions.WumBuff>());
			}
			if (player.HasBuff(BuffType<Buffs.Minions.WumBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			//Shoot if enabled
			/*
			if (shoot > 0 && tick == 0)
            {
				float num109 = (float)(40 * projectile.minionPos);
				int num108 = 30;
				int num107 = 60;
				projectile.localAI[0] -= 1f;
				float num106 = projectile.position.X;
				float num105 = projectile.position.Y;
				float num104 = 100000f;
				float num103 = num104;
				int num102 = -1;
				NPC ownerMinionAttackTargetNPC3 = projectile.OwnerMinionAttackTargetNPC;
				if (ownerMinionAttackTargetNPC3 != null && ownerMinionAttackTargetNPC3.CanBeChasedBy(projectile, false))
				{
					float num101 = ownerMinionAttackTargetNPC3.position.X + (float)(ownerMinionAttackTargetNPC3.width / 2);
					float num100 = ownerMinionAttackTargetNPC3.position.Y + (float)(ownerMinionAttackTargetNPC3.height / 2);
					float num99 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num101) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num100);
					if (num99 < num104)
					{
						if (num102 == -1 && num99 <= num103)
						{
							num103 = num99;
							num106 = num101;
							num105 = num100;
						}
						if (Collision.CanHit(projectile.position, projectile.width, projectile.height, ownerMinionAttackTargetNPC3.position, ownerMinionAttackTargetNPC3.width, ownerMinionAttackTargetNPC3.height))
						{
							num104 = num99;
							num106 = num101;
							num105 = num100;
							num102 = ownerMinionAttackTargetNPC3.whoAmI;
						}
					}
				}
				if (num102 == -1)
				{
					for (int l = 0; l < 200; l++)
					{
						if (Main.npc[l].CanBeChasedBy(projectile, false))
						{
							float num98 = Main.npc[l].position.X + (float)(Main.npc[l].width / 2);
							float num97 = Main.npc[l].position.Y + (float)(Main.npc[l].height / 2);
							float num96 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num98) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num97);
							if (num96 < num104)
							{
								if (num102 == -1 && num96 <= num103)
								{
									num103 = num96;
									num106 = num98;
									num105 = num97;
								}
								if (Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[l].position, Main.npc[l].width, Main.npc[l].height))
								{
									num104 = num96;
									num106 = num98;
									num105 = num97;
									num102 = l;
								}
							}
						}
					}
				}
				if (num102 == -1 && num103 < num104)
				{
					num104 = num103;
				}
				float num95 = 400f;
				if ((double)projectile.position.Y > Main.worldSurface * 16.0)
				{
					num95 = 200f;
				}
				//the projectile
				projectile.ai[1] = (float)num108;
				Vector2 vector7 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)(projectile.height / 2) - 8f);
				float num92 = num106 - vector7.X + (float)Main.rand.Next(-20, 21);
				float num91 = Math.Abs(num92) * 0.1f;
				num91 = num91 * (float)Main.rand.Next(0, 100) * 0.001f;
				float num89 = num105 - vector7.Y + (float)Main.rand.Next(-20, 21) - num91;
				float num88 = (float)Math.Sqrt((double)(num92 * num92 + num89 * num89));
				Vector2 velocity = new Vector2(num92, num89);
				//Spread to shots
				velocity.RotatedByRandom(MathHelper.ToRadians(10));
				num88 = 12f / num88;
				num92 *= num88;
				num89 *= num88;
				int num84 = projectile.damage;
				int num83 = 195;
				int num82 = Projectile.NewProjectile(vector7.X, vector7.Y, num92, num89, ProjectileType<Gum>(), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f);
				if (num92 < 0f)
				{
					projectile.direction = -1;
				}
				if (num92 > 0f)
				{
					projectile.direction = 1;
				}
				projectile.netUpdate = true;
				shoot--;
			}
			*/

			if (!Main.player[Projectile.owner].active)
			{
				Projectile.active = false;
			}
			else
			{
				flag = false;
				flag2 = false;
				flag6 = false;
				flag7 = false;
				int num113 = 85;
				flag9 = (true);
				if (Projectile.type == 324)
				{
					num113 = 120;
				}
				if (Projectile.type == 112)
				{
					num113 = 100;
				}
				if (Projectile.type == 127)
				{
					num113 = 50;
				}
				if (flag9)
				{
					if (Projectile.lavaWet)
					{
						Projectile.ai[0] = 1f;
						Projectile.ai[1] = 0f;
					}
					num113 = 60 + 30 * Projectile.minionPos;
				}
				else if (Projectile.type == 266)
				{
					num113 = 60 + 30 * Projectile.minionPos;
				}
				if (Projectile.type == 111)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].bunny = false;
					}
					if (Main.player[Projectile.owner].bunny)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 112)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].penguin = false;
					}
					if (Main.player[Projectile.owner].penguin)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 334)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].puppy = false;
					}
					if (Main.player[Projectile.owner].puppy)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 353)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].grinch = false;
					}
					if (Main.player[Projectile.owner].grinch)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 127)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].turtle = false;
					}
					if (Main.player[Projectile.owner].turtle)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 175)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].eater = false;
					}
					if (Main.player[Projectile.owner].eater)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 197)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].skeletron = false;
					}
					if (Main.player[Projectile.owner].skeletron)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 198)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].hornet = false;
					}
					if (Main.player[Projectile.owner].hornet)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 199)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].tiki = false;
					}
					if (Main.player[Projectile.owner].tiki)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 200)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].lizard = false;
					}
					if (Main.player[Projectile.owner].lizard)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 208)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].parrot = false;
					}
					if (Main.player[Projectile.owner].parrot)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 209)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].truffle = false;
					}
					if (Main.player[Projectile.owner].truffle)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 210)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].sapling = false;
					}
					if (Main.player[Projectile.owner].sapling)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 324)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].cSapling = false;
					}
					if (Main.player[Projectile.owner].cSapling)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 313)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].spider = false;
					}
					if (Main.player[Projectile.owner].spider)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 314)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].squashling = false;
					}
					if (Main.player[Projectile.owner].squashling)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 211)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].wisp = false;
					}
					if (Main.player[Projectile.owner].wisp)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 236)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].dino = false;
					}
					if (Main.player[Projectile.owner].dino)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 499)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].babyFaceMonster = false;
					}
					if (Main.player[Projectile.owner].babyFaceMonster)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 266)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].slime = false;
					}
					if (Main.player[Projectile.owner].slime)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 268)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].eyeSpring = false;
					}
					if (Main.player[Projectile.owner].eyeSpring)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 269)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].snowman = false;
					}
					if (Main.player[Projectile.owner].snowman)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 319)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].blackCat = false;
					}
					if (Main.player[Projectile.owner].blackCat)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 380)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].zephyrfish = false;
					}
					if (Main.player[Projectile.owner].zephyrfish)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type >= 390 && Projectile.type <= 392)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].spiderMinion = false;
					}
					if (Main.player[Projectile.owner].spiderMinion)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (Projectile.type == 398)
				{
					if (Main.player[Projectile.owner].dead)
					{
						Main.player[Projectile.owner].miniMinotaur = false;
					}
					if (Main.player[Projectile.owner].miniMinotaur)
					{
						Projectile.timeLeft = 2;
					}
				}
				if (flag9 || Projectile.type == 266 || (Projectile.type >= 390 && Projectile.type <= 392))
				{
					num113 = 10;
					int num170 = 40 * (Projectile.minionPos + 1) * Main.player[Projectile.owner].direction;
					if (Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) < Projectile.position.X + (float)(Projectile.width / 2) - (float)num113 + (float)num170)
					{
						flag = true;
					}
					else if (Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) > Projectile.position.X + (float)(Projectile.width / 2) + (float)num113 + (float)num170)
					{
						flag2 = true;
					}
				}
				else if (Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) < Projectile.position.X + (float)(Projectile.width / 2) - (float)num113)
				{
					flag = true;
				}
				else if (Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) > Projectile.position.X + (float)(Projectile.width / 2) + (float)num113)
				{
					flag2 = true;
				}
				if (Projectile.type == 175)
				{
					float num169 = 0.1f;
					Projectile.tileCollide = false;
					int num168 = 300;
					Vector2 vector15 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
					float num167 = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - vector15.X;
					float num166 = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - vector15.Y;
					if (Projectile.type == 127)
					{
						num166 = Main.player[Projectile.owner].position.Y - vector15.Y;
					}
					float num165 = (float)Math.Sqrt((double)(num167 * num167 + num166 * num166));
					float num164 = 7f;
					if (num165 < (float)num168 && Main.player[Projectile.owner].velocity.Y == 0f && Projectile.position.Y + (float)Projectile.height <= Main.player[Projectile.owner].position.Y + (float)Main.player[Projectile.owner].height && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
					{
						Projectile.ai[0] = 0f;
						if (Projectile.velocity.Y < -6f)
						{
							Projectile.velocity.Y = -6f;
						}
					}
					if (num165 < 150f)
					{
						if (Math.Abs(Projectile.velocity.X) > 2f || Math.Abs(Projectile.velocity.Y) > 2f)
						{
							Projectile.velocity *= 0.99f;
						}
						num169 = 0.01f;
						if (num167 < -2f)
						{
							num167 = -2f;
						}
						if (num167 > 2f)
						{
							num167 = 2f;
						}
						if (num166 < -2f)
						{
							num166 = -2f;
						}
						if (num166 > 2f)
						{
							num166 = 2f;
						}
					}
					else
					{
						if (num165 > 300f)
						{
							num169 = 0.2f;
						}
						num165 = num164 / num165;
						num167 *= num165;
						num166 *= num165;
					}
					if (Math.Abs(num167) > Math.Abs(num166) || num169 == 0.05f)
					{
						if (Projectile.velocity.X < num167)
						{
							Projectile.velocity.X = Projectile.velocity.X + num169;
							if (num169 > 0.05f && Projectile.velocity.X < 0f)
							{
								Projectile.velocity.X = Projectile.velocity.X + num169;
							}
						}
						if (Projectile.velocity.X > num167)
						{
							Projectile.velocity.X = Projectile.velocity.X - num169;
							if (num169 > 0.05f && Projectile.velocity.X > 0f)
							{
								Projectile.velocity.X = Projectile.velocity.X - num169;
							}
						}
					}
					if (Math.Abs(num167) <= Math.Abs(num166) || num169 == 0.05f)
					{
						if (Projectile.velocity.Y < num166)
						{
							Projectile.velocity.Y = Projectile.velocity.Y + num169;
							if (num169 > 0.05f && Projectile.velocity.Y < 0f)
							{
								Projectile.velocity.Y = Projectile.velocity.Y + num169;
							}
						}
						if (Projectile.velocity.Y > num166)
						{
							Projectile.velocity.Y = Projectile.velocity.Y - num169;
							if (num169 > 0.05f && Projectile.velocity.Y > 0f)
							{
								Projectile.velocity.Y = Projectile.velocity.Y - num169;
							}
						}
					}
					Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) - 1.57f;
					Projectile.frameCounter++;
					if (Projectile.frameCounter > 6)
					{
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					if (Projectile.frame > 1)
					{
						Projectile.frame = 0;
					}
				}
				else if (Projectile.type == 197)
				{
					float num162 = 0.1f;
					Projectile.tileCollide = false;
					int num161 = 300;
					Vector2 vector14 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
					float num160 = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - vector14.X;
					float num159 = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - vector14.Y;
					if (Projectile.type == 127)
					{
						num159 = Main.player[Projectile.owner].position.Y - vector14.Y;
					}
					float num158 = (float)Math.Sqrt((double)(num160 * num160 + num159 * num159));
					float num157 = 3f;
					if (num158 > 500f)
					{
						Projectile.localAI[0] = 10000f;
					}
					if (Projectile.localAI[0] >= 10000f)
					{
						num157 = 14f;
					}
					if (num158 < (float)num161 && Main.player[Projectile.owner].velocity.Y == 0f && Projectile.position.Y + (float)Projectile.height <= Main.player[Projectile.owner].position.Y + (float)Main.player[Projectile.owner].height && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
					{
						Projectile.ai[0] = 0f;
						if (Projectile.velocity.Y < -6f)
						{
							Projectile.velocity.Y = -6f;
						}
					}
					if (num158 < 150f)
					{
						if (Math.Abs(Projectile.velocity.X) > 2f || Math.Abs(Projectile.velocity.Y) > 2f)
						{
							Projectile.velocity *= 0.99f;
						}
						num162 = 0.01f;
						if (num160 < -2f)
						{
							num160 = -2f;
						}
						if (num160 > 2f)
						{
							num160 = 2f;
						}
						if (num159 < -2f)
						{
							num159 = -2f;
						}
						if (num159 > 2f)
						{
							num159 = 2f;
						}
					}
					else
					{
						if (num158 > 300f)
						{
							num162 = 0.2f;
						}
						num158 = num157 / num158;
						num160 *= num158;
						num159 *= num158;
					}
					if (Projectile.velocity.X < num160)
					{
						Projectile.velocity.X = Projectile.velocity.X + num162;
						if (num162 > 0.05f && Projectile.velocity.X < 0f)
						{
							Projectile.velocity.X = Projectile.velocity.X + num162;
						}
					}
					if (Projectile.velocity.X > num160)
					{
						Projectile.velocity.X = Projectile.velocity.X - num162;
						if (num162 > 0.05f && Projectile.velocity.X > 0f)
						{
							Projectile.velocity.X = Projectile.velocity.X - num162;
						}
					}
					if (Projectile.velocity.Y < num159)
					{
						Projectile.velocity.Y = Projectile.velocity.Y + num162;
						if (num162 > 0.05f && Projectile.velocity.Y < 0f)
						{
							Projectile.velocity.Y = Projectile.velocity.Y + num162;
						}
					}
					if (Projectile.velocity.Y > num159)
					{
						Projectile.velocity.Y = Projectile.velocity.Y - num162;
						if (num162 > 0.05f && Projectile.velocity.Y > 0f)
						{
							Projectile.velocity.Y = Projectile.velocity.Y - num162;
						}
					}
					Projectile.localAI[0] += (float)Main.rand.Next(10);
					if (Projectile.localAI[0] > 10000f)
					{
						if (Projectile.localAI[1] == 0f)
						{
							if (Projectile.velocity.X < 0f)
							{
								Projectile.localAI[1] = -1f;
							}
							else
							{
								Projectile.localAI[1] = 1f;
							}
						}
						Projectile.rotation += 0.25f * Projectile.localAI[1];
						if (Projectile.localAI[0] > 12000f)
						{
							Projectile.localAI[0] = 0f;
						}
					}
					else
					{
						Projectile.localAI[1] = 0f;
						float num155 = Projectile.velocity.X * 0.1f;
						if (Projectile.rotation > num155)
						{
							Projectile.rotation -= (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.01f;
							if (Projectile.rotation < num155)
							{
								Projectile.rotation = num155;
							}
						}
						if (Projectile.rotation < num155)
						{
							Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.01f;
							if (Projectile.rotation > num155)
							{
								Projectile.rotation = num155;
							}
						}
					}
					if ((double)Projectile.rotation > 6.28)
					{
						Projectile.rotation -= 6.28f;
					}
					if ((double)Projectile.rotation < -6.28)
					{
						Projectile.rotation += 6.28f;
					}
				}
				else
				{
					if (Projectile.type != 198 && Projectile.type != 380)
					{
						if (Projectile.type == 211)
						{
							float num154 = 0.2f;
							float num153 = 5f;
							Projectile.tileCollide = false;
							Vector2 vector13 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
							float num152 = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - vector13.X;
							float num151 = Main.player[Projectile.owner].position.Y + Main.player[Projectile.owner].gfxOffY + (float)(Main.player[Projectile.owner].height / 2) - vector13.Y;
							if (Main.player[Projectile.owner].controlLeft)
							{
								num152 -= 120f;
							}
							else if (Main.player[Projectile.owner].controlRight)
							{
								num152 += 120f;
							}
							if (Main.player[Projectile.owner].controlDown)
							{
								num151 += 120f;
							}
							else
							{
								if (Main.player[Projectile.owner].controlUp)
								{
									num151 -= 120f;
								}
								num151 -= 60f;
							}
							float num149 = (float)Math.Sqrt((double)(num152 * num152 + num151 * num151));
							if (num149 > 1000f)
							{
								Projectile.position.X = Projectile.position.X + num152;
								Projectile.position.Y = Projectile.position.Y + num151;
							}
							if (Projectile.localAI[0] == 1f)
							{
								if (num149 < 10f && Math.Abs(Main.player[Projectile.owner].velocity.X) + Math.Abs(Main.player[Projectile.owner].velocity.Y) < num153 && Main.player[Projectile.owner].velocity.Y == 0f)
								{
									Projectile.localAI[0] = 0f;
								}
								num153 = 12f;
								if (num149 < num153)
								{
									Projectile.velocity.X = num152;
									Projectile.velocity.Y = num151;
								}
								else
								{
									num149 = num153 / num149;
									Projectile.velocity.X = num152 * num149;
									Projectile.velocity.Y = num151 * num149;
								}
								if ((double)Projectile.velocity.X > 0.5)
								{
									Projectile.direction = -1;
								}
								else if ((double)Projectile.velocity.X < -0.5)
								{
									Projectile.direction = 1;
								}
								Projectile.spriteDirection = Projectile.direction;
								Projectile.rotation -= (0.2f + Math.Abs(Projectile.velocity.X) * 0.025f) * (float)Projectile.direction;
								Projectile.frameCounter++;
								if (Projectile.frameCounter > 3)
								{
									Projectile.frame++;
									Projectile.frameCounter = 0;
								}
								if (Projectile.frame < 5)
								{
									Projectile.frame = 5;
								}
								if (Projectile.frame > 9)
								{
									Projectile.frame = 5;
								}
								for (int i2 = 0; i2 < 2; i2++)
								{
									int num146 = Dust.NewDust(new Vector2(Projectile.position.X + 3f, Projectile.position.Y + 4f), 14, 14, 156, 0f, 0f, 0, default(Color), 1f);
									Dust obj = Main.dust[num146];
									obj.velocity *= 0.2f;
									Main.dust[num146].noGravity = true;
									Main.dust[num146].scale = 1.25f;
									Main.dust[num146].shader = GameShaders.Armor.GetSecondaryShader(Main.player[Projectile.owner].cLight, Main.player[Projectile.owner]);
								}
							}
							else
							{
								if (num149 > 200f)
								{
									Projectile.localAI[0] = 1f;
								}
								if ((double)Projectile.velocity.X > 0.5)
								{
									Projectile.direction = -1;
								}
								else if ((double)Projectile.velocity.X < -0.5)
								{
									Projectile.direction = 1;
								}
								Projectile.spriteDirection = Projectile.direction;
								if (num149 < 10f)
								{
									Projectile.velocity.X = num152;
									Projectile.velocity.Y = num151;
									Projectile.rotation = Projectile.velocity.X * 0.05f;
									if (num149 < num153)
									{
										Projectile.position += Projectile.velocity;
										Projectile.velocity *= 0f;
										num154 = 0f;
									}
									Projectile.direction = -Main.player[Projectile.owner].direction;
								}
								num149 = num153 / num149;
								num152 *= num149;
								num151 *= num149;
								if (Projectile.velocity.X < num152)
								{
									Projectile.velocity.X = Projectile.velocity.X + num154;
									if (Projectile.velocity.X < 0f)
									{
										Projectile.velocity.X = Projectile.velocity.X * 0.99f;
									}
								}
								if (Projectile.velocity.X > num152)
								{
									Projectile.velocity.X = Projectile.velocity.X - num154;
									if (Projectile.velocity.X > 0f)
									{
										Projectile.velocity.X = Projectile.velocity.X * 0.99f;
									}
								}
								if (Projectile.velocity.Y < num151)
								{
									Projectile.velocity.Y = Projectile.velocity.Y + num154;
									if (Projectile.velocity.Y < 0f)
									{
										Projectile.velocity.Y = Projectile.velocity.Y * 0.99f;
									}
								}
								if (Projectile.velocity.Y > num151)
								{
									Projectile.velocity.Y = Projectile.velocity.Y - num154;
									if (Projectile.velocity.Y > 0f)
									{
										Projectile.velocity.Y = Projectile.velocity.Y * 0.99f;
									}
								}
								if (Projectile.velocity.X != 0f || Projectile.velocity.Y != 0f)
								{
									Projectile.rotation = Projectile.velocity.X * 0.05f;
								}
								Projectile.frameCounter++;
								if (Projectile.frameCounter > 3)
								{
									Projectile.frame++;
									Projectile.frameCounter = 0;
								}
								if (Projectile.frame > 4)
								{
									Projectile.frame = 0;
								}
							}
							return;
						}
						if (Projectile.type == 199)
						{
							float num142 = 0.1f;
							Projectile.tileCollide = false;
							int num141 = 200;
							Vector2 vector11 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
							float num140 = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - vector11.X;
							float num139 = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - vector11.Y;
							num139 -= 60f;
							num140 -= 2f;
							if (Projectile.type == 127)
							{
								num139 = Main.player[Projectile.owner].position.Y - vector11.Y;
							}
							float num136 = (float)Math.Sqrt((double)(num140 * num140 + num139 * num139));
							float num135 = 4f;
							float num172 = num136;
							if (num136 < (float)num141 && Main.player[Projectile.owner].velocity.Y == 0f && Projectile.position.Y + (float)Projectile.height <= Main.player[Projectile.owner].position.Y + (float)Main.player[Projectile.owner].height && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
							{
								Projectile.ai[0] = 0f;
								if (Projectile.velocity.Y < -6f)
								{
									Projectile.velocity.Y = -6f;
								}
							}
							if (num136 < 4f)
							{
								Projectile.velocity.X = num140;
								Projectile.velocity.Y = num139;
								num142 = 0f;
							}
							else
							{
								if (num136 > 350f)
								{
									num142 = 0.2f;
									num135 = 10f;
								}
								num136 = num135 / num136;
								num140 *= num136;
								num139 *= num136;
							}
							if (Projectile.velocity.X < num140)
							{
								Projectile.velocity.X = Projectile.velocity.X + num142;
								if (Projectile.velocity.X < 0f)
								{
									Projectile.velocity.X = Projectile.velocity.X + num142;
								}
							}
							if (Projectile.velocity.X > num140)
							{
								Projectile.velocity.X = Projectile.velocity.X - num142;
								if (Projectile.velocity.X > 0f)
								{
									Projectile.velocity.X = Projectile.velocity.X - num142;
								}
							}
							if (Projectile.velocity.Y < num139)
							{
								Projectile.velocity.Y = Projectile.velocity.Y + num142;
								if (Projectile.velocity.Y < 0f)
								{
									Projectile.velocity.Y = Projectile.velocity.Y + num142;
								}
							}
							if (Projectile.velocity.Y > num139)
							{
								Projectile.velocity.Y = Projectile.velocity.Y - num142;
								if (Projectile.velocity.Y > 0f)
								{
									Projectile.velocity.Y = Projectile.velocity.Y - num142;
								}
							}
							Projectile.direction = -Main.player[Projectile.owner].direction;
							Projectile.spriteDirection = 1;
							Projectile.rotation = Projectile.velocity.Y * 0.05f * (0f - (float)Projectile.direction);
							if (num172 >= 50f)
							{
								Projectile.frameCounter++;
								if (Projectile.frameCounter > 6)
								{
									Projectile.frameCounter = 0;
									if (Projectile.velocity.X < 0f)
									{
										if (Projectile.frame < 2)
										{
											Projectile.frame++;
										}
										if (Projectile.frame > 2)
										{
											Projectile.frame--;
										}
									}
									else
									{
										if (Projectile.frame < 6)
										{
											Projectile.frame++;
										}
										if (Projectile.frame > 6)
										{
											Projectile.frame--;
										}
									}
								}
							}
							else
							{
								Projectile.frameCounter++;
								if (Projectile.frameCounter > 6)
								{
									Projectile.frame += Projectile.direction;
									Projectile.frameCounter = 0;
								}
								if (Projectile.frame > 7)
								{
									Projectile.frame = 0;
								}
								if (Projectile.frame < 0)
								{
									Projectile.frame = 7;
								}
							}
							return;
						}
						if (Projectile.ai[1] == 0f)
						{
							int num133 = 500;
							if (Projectile.type == 127)
							{
								num133 = 200;
							}
							if (Projectile.type == 208)
							{
								num133 = 300;
							}
							if (flag9 || Projectile.type == 266 || (Projectile.type >= 390 && Projectile.type <= 392))
							{
								num133 += 40 * Projectile.minionPos;
								if (Projectile.localAI[0] > 0f)
								{
									num133 += 500;
								}
								if (Projectile.type == 266 && Projectile.localAI[0] > 0f)
								{
									num133 += 100;
								}
								if (Projectile.type >= 390 && Projectile.type <= 392 && Projectile.localAI[0] > 0f)
								{
									num133 += 400;
								}
							}
							if (Main.player[Projectile.owner].rocketDelay2 > 0)
							{
								Projectile.ai[0] = 1f;
							}
							Vector2 vector10 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
							float num173 = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - vector10.X;
							num132 = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - vector10.Y;
							float num131 = (float)Math.Sqrt((double)(num173 * num173 + num132 * num132));
							if (num131 > 2000f)
							{
								Projectile.position.X = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - (float)(Projectile.width / 2);
								Projectile.position.Y = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - (float)(Projectile.height / 2);
								goto IL_289c;
							}
							if (!(num131 > (float)num133))
							{
								if (Math.Abs(num132) > 300f)
								{
									if (!flag9 && Projectile.type != 266 && (Projectile.type < 390 || Projectile.type > 392))
									{
										goto IL_282c;
									}
									if (Projectile.localAI[0] <= 0f)
									{
										goto IL_282c;
									}
								}
								goto IL_289c;
							}
							goto IL_282c;
						}
						goto IL_289c;
					}
					float num33 = 0.4f;
					if (Projectile.type == 380)
					{
						num33 = 0.3f;
					}
					Projectile.tileCollide = false;
					int num32 = 100;
					Vector2 vector3 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
					float num31 = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - vector3.X;
					float num30 = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - vector3.Y;
					num30 += (float)Main.rand.Next(-10, 21);
					num31 += (float)Main.rand.Next(-10, 21);
					num31 += 60f * (0f - (float)Main.player[Projectile.owner].direction);
					num30 -= 60f;
					if (Projectile.type == 127)
					{
						num30 = Main.player[Projectile.owner].position.Y - vector3.Y;
					}
					float num25 = (float)Math.Sqrt((double)(num31 * num31 + num30 * num30));
					float num24 = 14f;
					if (Projectile.type == 380)
					{
						num24 = 6f;
					}
					if (num25 < (float)num32 && Main.player[Projectile.owner].velocity.Y == 0f && Projectile.position.Y + (float)Projectile.height <= Main.player[Projectile.owner].position.Y + (float)Main.player[Projectile.owner].height && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
					{
						Projectile.ai[0] = 0f;
						if (Projectile.velocity.Y < -6f)
						{
							Projectile.velocity.Y = -6f;
						}
					}
					if (num25 < 50f)
					{
						if (Math.Abs(Projectile.velocity.X) > 2f || Math.Abs(Projectile.velocity.Y) > 2f)
						{
							Projectile.velocity *= 0.99f;
						}
						num33 = 0.01f;
					}
					else
					{
						if (Projectile.type == 380)
						{
							if (num25 < 100f)
							{
								num33 = 0.1f;
							}
							if (num25 > 300f)
							{
								num33 = 0.4f;
							}
						}
						else if (Projectile.type == 198)
						{
							if (num25 < 100f)
							{
								num33 = 0.1f;
							}
							if (num25 > 300f)
							{
								num33 = 0.6f;
							}
						}
						num25 = num24 / num25;
						num31 *= num25;
						num30 *= num25;
					}
					if (Projectile.velocity.X < num31)
					{
						Projectile.velocity.X = Projectile.velocity.X + num33;
						if (num33 > 0.05f && Projectile.velocity.X < 0f)
						{
							Projectile.velocity.X = Projectile.velocity.X + num33;
						}
					}
					if (Projectile.velocity.X > num31)
					{
						Projectile.velocity.X = Projectile.velocity.X - num33;
						if (num33 > 0.05f && Projectile.velocity.X > 0f)
						{
							Projectile.velocity.X = Projectile.velocity.X - num33;
						}
					}
					if (Projectile.velocity.Y < num30)
					{
						Projectile.velocity.Y = Projectile.velocity.Y + num33;
						if (num33 > 0.05f && Projectile.velocity.Y < 0f)
						{
							Projectile.velocity.Y = Projectile.velocity.Y + num33 * 2f;
						}
					}
					if (Projectile.velocity.Y > num30)
					{
						Projectile.velocity.Y = Projectile.velocity.Y - num33;
						if (num33 > 0.05f && Projectile.velocity.Y > 0f)
						{
							Projectile.velocity.Y = Projectile.velocity.Y - num33 * 2f;
						}
					}
					if ((double)Projectile.velocity.X > 0.25)
					{
						Projectile.direction = -1;
					}
					else if ((double)Projectile.velocity.X < -0.25)
					{
						Projectile.direction = 1;
					}
					Projectile.spriteDirection = Projectile.direction;
					Projectile.rotation = Projectile.velocity.X * 0.05f;
					Projectile.frameCounter++;
					int num22 = 2;
					if (Projectile.type == 380)
					{
						num22 = 5;
					}
					if (Projectile.frameCounter > num22)
					{
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					if (Projectile.frame > 3)
					{
						Projectile.frame = 0;
					}
				}
			}
			return;
		IL_282c:
			if (Projectile.type != 324)
			{
				if (num132 > 0f && Projectile.velocity.Y < 0f)
				{
					Projectile.velocity.Y = 0f;
				}
				if (num132 < 0f && Projectile.velocity.Y > 0f)
				{
					Projectile.velocity.Y = 0f;
				}
			}
			Projectile.ai[0] = 1f;
			goto IL_289c;
		IL_289c:
			if (Projectile.type == 209 && Projectile.ai[0] != 0f)
			{
				if (Main.player[Projectile.owner].velocity.Y == 0f && Projectile.alpha >= 100)
				{
					Projectile.position.X = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - (float)(Projectile.width / 2);
					Projectile.position.Y = Main.player[Projectile.owner].position.Y + (float)Main.player[Projectile.owner].height - (float)Projectile.height;
					Projectile.ai[0] = 0f;
				}
				else
				{
					Projectile.velocity.X = 0f;
					Projectile.velocity.Y = 0f;
					Projectile.alpha += 5;
					if (Projectile.alpha > 255)
					{
						Projectile.alpha = 255;
					}
				}
			}
			else if (Projectile.ai[0] != 0f)
			{
				float num130 = 0.2f;
				int num129 = 200;
				if (Projectile.type == 127)
				{
					num129 = 100;
				}
				if (flag9)
				{
					num130 = 0.5f;
					num129 = 100;
				}
				Projectile.tileCollide = false;
				Vector2 vector9 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
				float num128 = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - vector9.X;
				if (flag9 || Projectile.type == 266 || (Projectile.type >= 390 && Projectile.type <= 392))
				{
					num128 -= (float)(40 * Main.player[Projectile.owner].direction);
					float num127 = 700f;
					if (flag9)
					{
						num127 += 100f;
					}
					bool flag8 = false;
					int num126 = -1;
					for (int n = 0; n < 200; n++)
					{
						if (Main.npc[n].CanBeChasedBy(Projectile, false))
						{
							float num125 = Main.npc[n].position.X + (float)(Main.npc[n].width / 2);
							float num124 = Main.npc[n].position.Y + (float)(Main.npc[n].height / 2);
							if (Math.Abs(Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - num125) + Math.Abs(Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - num124) < num127)
							{
								if (Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[n].position, Main.npc[n].width, Main.npc[n].height))
								{
									num126 = n;
								}
								flag8 = true;
								break;
							}
						}
					}
					if (!flag8)
					{
						num128 -= (float)(40 * Projectile.minionPos * Main.player[Projectile.owner].direction);
					}
					if (flag8 && num126 >= 0)
					{
						Projectile.ai[0] = 0f;
					}
				}
				float num123 = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - vector9.Y;
				if (Projectile.type == 127)
				{
					num123 = Main.player[Projectile.owner].position.Y - vector9.Y;
				}
				float num122 = (float)Math.Sqrt((double)(num128 * num128 + num123 * num123));
				float num121 = 10f;
				float num120 = num122;
				if (Projectile.type == 111)
				{
					num121 = 11f;
				}
				if (Projectile.type == 127)
				{
					num121 = 9f;
				}
				if (Projectile.type == 324)
				{
					num121 = 20f;
				}
				if (flag9)
				{
					num130 = 0.4f;
					num121 = 12f;
					if (num121 < Math.Abs(Main.player[Projectile.owner].velocity.X) + Math.Abs(Main.player[Projectile.owner].velocity.Y))
					{
						num121 = Math.Abs(Main.player[Projectile.owner].velocity.X) + Math.Abs(Main.player[Projectile.owner].velocity.Y);
					}
				}
				if (Projectile.type == 208 && Math.Abs(Main.player[Projectile.owner].velocity.X) + Math.Abs(Main.player[Projectile.owner].velocity.Y) > 4f)
				{
					num129 = -1;
				}
				if (num122 < (float)num129 && Main.player[Projectile.owner].velocity.Y == 0f && Projectile.position.Y + (float)Projectile.height <= Main.player[Projectile.owner].position.Y + (float)Main.player[Projectile.owner].height && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				{
					Projectile.ai[0] = 0f;
					if (Projectile.velocity.Y < -6f)
					{
						Projectile.velocity.Y = -6f;
					}
				}
				if (num122 < 60f)
				{
					num128 = Projectile.velocity.X;
					num123 = Projectile.velocity.Y;
				}
				else
				{
					num122 = num121 / num122;
					num128 *= num122;
					num123 *= num122;
				}
				if (Projectile.type == 324)
				{
					if (num120 > 1000f)
					{
						if ((double)(Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) < (double)num121 - 1.25)
						{
							Projectile.velocity *= 1.025f;
						}
						if ((double)(Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) > (double)num121 + 1.25)
						{
							Projectile.velocity *= 0.975f;
						}
					}
					else if (num120 > 600f)
					{
						if (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) < num121 - 1f)
						{
							Projectile.velocity *= 1.05f;
						}
						if (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) > num121 + 1f)
						{
							Projectile.velocity *= 0.95f;
						}
					}
					else if (num120 > 400f)
					{
						if ((double)(Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) < (double)num121 - 0.5)
						{
							Projectile.velocity *= 1.075f;
						}
						if ((double)(Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) > (double)num121 + 0.5)
						{
							Projectile.velocity *= 0.925f;
						}
					}
					else
					{
						if ((double)(Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) < (double)num121 - 0.25)
						{
							Projectile.velocity *= 1.1f;
						}
						if ((double)(Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) > (double)num121 + 0.25)
						{
							Projectile.velocity *= 0.9f;
						}
					}
					Projectile.velocity.X = (Projectile.velocity.X * 34f + num128) / 35f;
					Projectile.velocity.Y = (Projectile.velocity.Y * 34f + num123) / 35f;
				}
				else
				{
					if (Projectile.velocity.X < num128)
					{
						Projectile.velocity.X = Projectile.velocity.X + num130;
						if (Projectile.velocity.X < 0f)
						{
							Projectile.velocity.X = Projectile.velocity.X + num130 * 1.5f;
						}
					}
					if (Projectile.velocity.X > num128)
					{
						Projectile.velocity.X = Projectile.velocity.X - num130;
						if (Projectile.velocity.X > 0f)
						{
							Projectile.velocity.X = Projectile.velocity.X - num130 * 1.5f;
						}
					}
					if (Projectile.velocity.Y < num123)
					{
						Projectile.velocity.Y = Projectile.velocity.Y + num130;
						if (Projectile.velocity.Y < 0f)
						{
							Projectile.velocity.Y = Projectile.velocity.Y + num130 * 1.5f;
						}
					}
					if (Projectile.velocity.Y > num123)
					{
						Projectile.velocity.Y = Projectile.velocity.Y - num130;
						if (Projectile.velocity.Y > 0f)
						{
							Projectile.velocity.Y = Projectile.velocity.Y - num130 * 1.5f;
						}
					}
				}
				if (Projectile.type == 111)
				{
					Projectile.frame = 7;
				}
				if (Projectile.type == 112)
				{
					Projectile.frame = 2;
				}
				if (flag9 && Projectile.frame < 12)
				{
					Projectile.frame = Main.rand.Next(12, 18);
					Projectile.frameCounter = 0;
				}
				if (Projectile.type != 313)
				{
					if ((double)Projectile.velocity.X > 0.5)
					{
						Projectile.spriteDirection = -1;
					}
					else if ((double)Projectile.velocity.X < -0.5)
					{
						Projectile.spriteDirection = 1;
					}
				}
				if (Projectile.type == 398)
				{
					if ((double)Projectile.velocity.X > 0.5)
					{
						Projectile.spriteDirection = 1;
					}
					else if ((double)Projectile.velocity.X < -0.5)
					{
						Projectile.spriteDirection = -1;
					}
				}
				if (Projectile.type == 112)
				{
					if (Projectile.spriteDirection == -1)
					{
						Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
					}
					else
					{
						Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
					}
				}
				else if (Projectile.type >= 390 && Projectile.type <= 392)
				{
					int num116 = (int)(Projectile.Center.X / 16f);
					int num115 = (int)(Projectile.Center.Y / 16f);
					if (Main.tile[num116, num115] != null && Main.tile[num116, num115].WallType > 0)
					{
						Projectile.rotation = Projectile.velocity.ToRotation() + 1.57079637f;
						Projectile.frameCounter += (int)(Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y));
						if (Projectile.frameCounter > 5)
						{
							Projectile.frame++;
							Projectile.frameCounter = 0;
						}
						if (Projectile.frame > 7)
						{
							Projectile.frame = 4;
						}
						if (Projectile.frame < 4)
						{
							Projectile.frame = 7;
						}
					}
					else
					{
						Projectile.frameCounter++;
						if (Projectile.frameCounter > 2)
						{
							Projectile.frame++;
							Projectile.frameCounter = 0;
						}
						if (Projectile.frame < 8 || Projectile.frame > 10)
						{
							Projectile.frame = 8;
						}
						Projectile.rotation = Projectile.velocity.X * 0.1f;
					}
				}
				else if (Projectile.type == 334)
				{
					Projectile.frameCounter++;
					if (Projectile.frameCounter > 1)
					{
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					if (Projectile.frame < 7 || Projectile.frame > 10)
					{
						Projectile.frame = 7;
					}
					Projectile.rotation = Projectile.velocity.X * 0.1f;
				}
				else if (Projectile.type == 353)
				{
					Projectile.frameCounter++;
					if (Projectile.frameCounter > 6)
					{
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					if (Projectile.frame < 10 || Projectile.frame > 13)
					{
						Projectile.frame = 10;
					}
					Projectile.rotation = Projectile.velocity.X * 0.05f;
				}
				else if (Projectile.type == 127)
				{
					Projectile.frameCounter += 3;
					if (Projectile.frameCounter > 6)
					{
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					if (Projectile.frame <= 5 || Projectile.frame > 15)
					{
						Projectile.frame = 6;
					}
					Projectile.rotation = Projectile.velocity.X * 0.1f;
				}
				else if (Projectile.type == 269)
				{
					if (Projectile.frame == 6)
					{
						Projectile.frameCounter = 0;
					}
					else if (Projectile.frame < 4 || Projectile.frame > 6)
					{
						Projectile.frameCounter = 0;
						Projectile.frame = 4;
					}
					else
					{
						Projectile.frameCounter++;
						if (Projectile.frameCounter > 6)
						{
							Projectile.frame++;
							Projectile.frameCounter = 0;
						}
					}
					Projectile.rotation = Projectile.velocity.X * 0.05f;
				}
				else if (Projectile.type == 266)
				{
					Projectile.frameCounter++;
					if (Projectile.frameCounter > 6)
					{
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					if (Projectile.frame < 2 || Projectile.frame > 5)
					{
						Projectile.frame = 2;
					}
					Projectile.rotation = Projectile.velocity.X * 0.1f;
				}
				else if (Projectile.type == 324)
				{
					Projectile.frameCounter++;
					if (Projectile.frameCounter > 1)
					{
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					if (Projectile.frame < 6 || Projectile.frame > 9)
					{
						Projectile.frame = 6;
					}
					Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.58f;
					Lighting.AddLight((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, 0.9f, 0.6f, 0.2f);
					for (int m = 0; m < 2; m++)
					{
						int num114 = 4;
						int num112 = Dust.NewDust(new Vector2(Projectile.Center.X - (float)num114, Projectile.Center.Y - (float)num114) - Projectile.velocity * 0f, num114 * 2, num114 * 2, 6, 0f, 0f, 100, default(Color), 1f);
						Main.dust[num112].scale *= 1.8f + (float)Main.rand.Next(10) * 0.1f;
						Dust obj2 = Main.dust[num112];
						obj2.velocity *= 0.2f;
						if (m == 1)
						{
							Dust obj3 = Main.dust[num112];
							obj3.position -= Projectile.velocity * 0.5f;
						}
						Main.dust[num112].noGravity = true;
						num112 = Dust.NewDust(new Vector2(Projectile.Center.X - (float)num114, Projectile.Center.Y - (float)num114) - Projectile.velocity * 0f, num114 * 2, num114 * 2, 31, 0f, 0f, 100, default(Color), 0.5f);
						Main.dust[num112].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
						Dust obj4 = Main.dust[num112];
						obj4.velocity *= 0.05f;
						if (m == 1)
						{
							Dust obj5 = Main.dust[num112];
							obj5.position -= Projectile.velocity * 0.5f;
						}
					}
				}
				else if (Projectile.type == 268)
				{
					Projectile.frameCounter++;
					if (Projectile.frameCounter > 4)
					{
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					if (Projectile.frame < 6 || Projectile.frame > 7)
					{
						Projectile.frame = 6;
					}
					Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.58f;
				}
				else if (Projectile.type == 200)
				{
					Projectile.frameCounter += 3;
					if (Projectile.frameCounter > 6)
					{
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					if (Projectile.frame <= 5 || Projectile.frame > 9)
					{
						Projectile.frame = 6;
					}
					Projectile.rotation = Projectile.velocity.X * 0.1f;
				}
				else if (Projectile.type == 208)
				{
					Projectile.rotation = Projectile.velocity.X * 0.075f;
					Projectile.frameCounter++;
					if (Projectile.frameCounter > 6)
					{
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					if (Projectile.frame > 4)
					{
						Projectile.frame = 1;
					}
					if (Projectile.frame < 1)
					{
						Projectile.frame = 1;
					}
				}
				else if (Projectile.type == 236)
				{
					Projectile.rotation = Projectile.velocity.Y * 0.05f * (float)Projectile.direction;
					if (Projectile.velocity.Y < 0f)
					{
						Projectile.frameCounter += 2;
					}
					else
					{
						Projectile.frameCounter++;
					}
					if (Projectile.frameCounter >= 6)
					{
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					if (Projectile.frame > 12)
					{
						Projectile.frame = 9;
					}
					if (Projectile.frame < 9)
					{
						Projectile.frame = 9;
					}
				}
				else if (Projectile.type == 499)
				{
					Projectile.rotation = Projectile.velocity.Y * 0.05f * (float)Projectile.direction;
					if (Projectile.velocity.Y < 0f)
					{
						Projectile.frameCounter += 2;
					}
					else
					{
						Projectile.frameCounter++;
					}
					if (Projectile.frameCounter >= 6)
					{
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					if (Projectile.frame >= 12)
					{
						Projectile.frame = 8;
					}
					if (Projectile.frame < 8)
					{
						Projectile.frame = 8;
					}
				}
				else if (Projectile.type == 314)
				{
					Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.58f;
					Projectile.frameCounter++;
					if (Projectile.frameCounter >= 3)
					{
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					if (Projectile.frame > 12)
					{
						Projectile.frame = 7;
					}
					if (Projectile.frame < 7)
					{
						Projectile.frame = 7;
					}
				}
				else if (Projectile.type == 319)
				{
					Projectile.rotation = Projectile.velocity.X * 0.05f;
					Projectile.frameCounter++;
					if (Projectile.frameCounter >= 6)
					{
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					if (Projectile.frame > 10)
					{
						Projectile.frame = 6;
					}
					if (Projectile.frame < 6)
					{
						Projectile.frame = 6;
					}
				}
				else if (Projectile.type == 210)
				{
					Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.58f;
					Projectile.frameCounter += 3;
					if (Projectile.frameCounter > 6)
					{
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					if (Projectile.frame > 11)
					{
						Projectile.frame = 7;
					}
					if (Projectile.frame < 7)
					{
						Projectile.frame = 7;
					}
				}
				else if (Projectile.type == 313)
				{
					Projectile.position.Y = Projectile.position.Y + (float)Projectile.height;
					Projectile.height = 54;
					Projectile.position.Y = Projectile.position.Y - (float)Projectile.height;
					Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
					Projectile.width = 54;
					Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
					Projectile.rotation += Projectile.velocity.X * 0.01f;
					Projectile.frameCounter = 0;
					Projectile.frame = 11;
				}
				else if (Projectile.type == 398)
				{
					Projectile.frameCounter++;
					if (Projectile.frameCounter > 1)
					{
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					if (Projectile.frame < 6 || Projectile.frame > 9)
					{
						Projectile.frame = 6;
					}
					Projectile.rotation = Projectile.velocity.X * 0.1f;
				}
				else if (Projectile.spriteDirection == -1)
				{
					Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
				}
				else
				{
					Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 3.14f;
				}
				if (!flag9 && Projectile.type != 499 && Projectile.type != 398 && Projectile.type != 390 && Projectile.type != 391 && Projectile.type != 392 && Projectile.type != 127 && Projectile.type != 200 && Projectile.type != 208 && Projectile.type != 210 && Projectile.type != 236 && Projectile.type != 266 && Projectile.type != 268 && Projectile.type != 269 && Projectile.type != 313 && Projectile.type != 314 && Projectile.type != 319 && Projectile.type != 324 && Projectile.type != 334 && Projectile.type != 353)
				{
					//TODO: this might cause dust, look into it
					int num110 = Dust.NewDust(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 4f, Projectile.position.Y + (float)(Projectile.height / 2) - 4f) - Projectile.velocity, 8, 8, 16, (0f - Projectile.velocity.X) * 0.5f, Projectile.velocity.Y * 0.5f, 50, default(Color), 1.7f);
					Main.dust[num110].velocity.X = Main.dust[num110].velocity.X * 0.2f;
					Main.dust[num110].velocity.Y = Main.dust[num110].velocity.Y * 0.2f;
					Main.dust[num110].noGravity = true;
				}
			}
			else
			{
				if (flag9)
				{
					float num109 = (float)(40 * Projectile.minionPos);
					int num108 = 30;
					int num107 = 60;
					Projectile.localAI[0] -= 1f;
					if (Projectile.localAI[0] < 0f)
					{
						Projectile.localAI[0] = 0f;
					}
					if (Projectile.ai[1] > 0f)
					{
						Projectile.ai[1] -= 1f;
					}
					else
					{
						float num106 = Projectile.position.X;
						float num105 = Projectile.position.Y;
						float num104 = 100000f;
						float num103 = num104;
						int num102 = -1;
						NPC ownerMinionAttackTargetNPC3 = Projectile.OwnerMinionAttackTargetNPC;
						if (ownerMinionAttackTargetNPC3 != null && ownerMinionAttackTargetNPC3.CanBeChasedBy(Projectile, false))
						{
							float num101 = ownerMinionAttackTargetNPC3.position.X + (float)(ownerMinionAttackTargetNPC3.width / 2);
							float num100 = ownerMinionAttackTargetNPC3.position.Y + (float)(ownerMinionAttackTargetNPC3.height / 2);
							float num99 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num101) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num100);
							if (num99 < num104)
							{
								if (num102 == -1 && num99 <= num103)
								{
									num103 = num99;
									num106 = num101;
									num105 = num100;
								}
								if (Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, ownerMinionAttackTargetNPC3.position, ownerMinionAttackTargetNPC3.width, ownerMinionAttackTargetNPC3.height))
								{
									num104 = num99;
									num106 = num101;
									num105 = num100;
									num102 = ownerMinionAttackTargetNPC3.whoAmI;
								}
							}
						}
						if (num102 == -1)
						{
							for (int l = 0; l < 200; l++)
							{
								if (Main.npc[l].CanBeChasedBy(Projectile, false))
								{
									float num98 = Main.npc[l].position.X + (float)(Main.npc[l].width / 2);
									float num97 = Main.npc[l].position.Y + (float)(Main.npc[l].height / 2);
									float num96 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num98) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num97);
									if (num96 < num104)
									{
										if (num102 == -1 && num96 <= num103)
										{
											num103 = num96;
											num106 = num98;
											num105 = num97;
										}
										if (Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[l].position, Main.npc[l].width, Main.npc[l].height))
										{
											num104 = num96;
											num106 = num98;
											num105 = num97;
											num102 = l;
										}
									}
								}
							}
						}
						if (num102 == -1 && num103 < num104)
						{
							num104 = num103;
						}
						float num95 = 400f;
						if ((double)Projectile.position.Y > Main.worldSurface * 16.0)
						{
							num95 = 200f;
						}
						if (num104 < num95 + num109 && num102 == -1)
						{
							float num94 = num106 - (Projectile.position.X + (float)(Projectile.width / 2));
							if (num94 < -5f)
							{
								flag = true;
								flag2 = false;
							}
							else if (num94 > 5f)
							{
								flag2 = true;
								flag = false;
							}
						}
						else if (num102 >= 0 && num104 < 900f /*sight range*/+ num109)
						{
							Projectile.localAI[0] = (float)num107;
							float num93 = num106 - (Projectile.position.X + (float)(Projectile.width / 2));
							if (num93 > 300f || num93 < -300f)
							{
								if (num93 < -50f)
								{
									flag = true;
									flag2 = false;
								}
								else if (num93 > 50f)
								{
									flag2 = true;
									flag = false;
								}
							}
							else if (Projectile.owner == Main.myPlayer)
							{
								//Shoot code original location
								//shoot = 15;

								Projectile.ai[1] = (float)num108;
								Vector2 vector7 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)(Projectile.height / 2) - 8f);
								float num92 = num106 - vector7.X + (float)Main.rand.Next(-20, 21);
								float num91 = Math.Abs(num92) * 0.1f;
								num91 = num91 * (float)Main.rand.Next(0, 100) * 0.001f;
								float num89 = num105 - vector7.Y + (float)Main.rand.Next(-20, 21) - num91;
								float num88 = (float)Math.Sqrt((double)(num92 * num92 + num89 * num89));
								Vector2 velocity = new Vector2(num92, num89);
								//Spread to shots
								velocity.RotatedByRandom(MathHelper.ToRadians(10));
								num88 = 12f / num88;
								num92 *= num88;
								num89 *= num88;
								int num84 = Projectile.damage;
								int num83 = 195;
								int num82 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), vector7.X, vector7.Y, num92, num89, ProjectileType<Gum>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, Main.rand.Next(6), 0f);
								if (num92 < 0f)
								{
									Projectile.direction = -1;
								}
								if (num92 > 0f)
								{
									Projectile.direction = 1;
								}
								Projectile.netUpdate = true;
							}
						}
					}
				}
				bool flag5 = false;
				Vector2 vector6 = Vector2.Zero;
				bool flag4 = false;
				if (Projectile.type == 266 || (Projectile.type >= 390 && Projectile.type <= 392))
				{
					float num81 = (float)(40 * Projectile.minionPos);
					int num80 = 60;
					Projectile.localAI[0] -= 1f;
					if (Projectile.localAI[0] < 0f)
					{
						Projectile.localAI[0] = 0f;
					}
					if (Projectile.ai[1] > 0f)
					{
						Projectile.ai[1] -= 1f;
					}
					else
					{
						float num79 = Projectile.position.X;
						float num78 = Projectile.position.Y;
						float num77 = 100000f;
						float num76 = num77;
						int num75 = -1;
						NPC ownerMinionAttackTargetNPC2 = Projectile.OwnerMinionAttackTargetNPC;
						if (ownerMinionAttackTargetNPC2 != null && ownerMinionAttackTargetNPC2.CanBeChasedBy(Projectile, false))
						{
							float x = ownerMinionAttackTargetNPC2.Center.X;
							float y = ownerMinionAttackTargetNPC2.Center.Y;
							float num74 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - x) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - y);
							if (num74 < num77)
							{
								if (num75 == -1 && num74 <= num76)
								{
									num76 = num74;
									num79 = x;
									num78 = y;
								}
								if (Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, ownerMinionAttackTargetNPC2.position, ownerMinionAttackTargetNPC2.width, ownerMinionAttackTargetNPC2.height))
								{
									num77 = num74;
									num79 = x;
									num78 = y;
									num75 = ownerMinionAttackTargetNPC2.whoAmI;
								}
							}
						}
						if (num75 == -1)
						{
							for (int k = 0; k < 200; k++)
							{
								if (Main.npc[k].CanBeChasedBy(Projectile, false))
								{
									float num73 = Main.npc[k].position.X + (float)(Main.npc[k].width / 2);
									float num72 = Main.npc[k].position.Y + (float)(Main.npc[k].height / 2);
									float num71 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num73) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num72);
									if (num71 < num77)
									{
										if (num75 == -1 && num71 <= num76)
										{
											num76 = num71;
											num79 = num73;
											num78 = num72;
										}
										if (Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[k].position, Main.npc[k].width, Main.npc[k].height))
										{
											num77 = num71;
											num79 = num73;
											num78 = num72;
											num75 = k;
										}
									}
								}
							}
						}
						if (Projectile.type >= 390 && Projectile.type <= 392 && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
						{
							Projectile.tileCollide = true;
						}
						if (num75 == -1 && num76 < num77)
						{
							num77 = num76;
						}
						else if (num75 >= 0)
						{
							flag5 = true;
							vector6 = new Vector2(num79, num78) - Projectile.Center;
							if (Projectile.type >= 390 && Projectile.type <= 392)
							{
								if (Main.npc[num75].position.Y > Projectile.position.Y + (float)Projectile.height)
								{
									int num70 = (int)(Projectile.Center.X / 16f);
									int num69 = (int)((Projectile.position.Y + (float)Projectile.height + 1f) / 16f);
									if (Main.tile[num70, num69] != null && Main.tile[num70, num69].HasTile && TileID.Sets.Platforms[Main.tile[num70, num69].TileType])
									{
										Projectile.tileCollide = false;
									}
								}
								Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
								Rectangle value = new Rectangle((int)Main.npc[num75].position.X, (int)Main.npc[num75].position.Y, Main.npc[num75].width, Main.npc[num75].height);
								int num68 = 10;
								value.X -= num68;
								value.Y -= num68;
								value.Width += num68 * 2;
								value.Height += num68 * 2;
								if (rectangle.Intersects(value))
								{
									flag4 = true;
									Vector2 vector8 = Main.npc[num75].Center - Projectile.Center;
									if (Projectile.velocity.Y > 0f && vector8.Y < 0f)
									{
										Projectile.velocity.Y = Projectile.velocity.Y * 0.5f;
									}
									if (Projectile.velocity.Y < 0f && vector8.Y > 0f)
									{
										Projectile.velocity.Y = Projectile.velocity.Y * 0.5f;
									}
									if (Projectile.velocity.X > 0f && vector8.X < 0f)
									{
										Projectile.velocity.X = Projectile.velocity.X * 0.5f;
									}
									if (Projectile.velocity.X < 0f && vector8.X > 0f)
									{
										Projectile.velocity.X = Projectile.velocity.X * 0.5f;
									}
									if (vector8.Length() > 14f)
									{
										vector8.Normalize();
										vector8 *= 14f;
									}
									Projectile.rotation = (Projectile.rotation * 5f + vector8.ToRotation() + 1.57079637f) / 6f;
									Projectile.velocity = (Projectile.velocity * 9f + vector8) / 10f;
									for (int i = 0; i < 1000; i++)
									{
										if (Projectile.whoAmI != i && Projectile.owner == Main.projectile[i].owner && Main.projectile[i].type >= 390 && Main.projectile[i].type <= 392 && (Main.projectile[i].Center - Projectile.Center).Length() < 15f)
										{
											float num67 = 0.5f;
											if (Projectile.Center.Y > Main.projectile[i].Center.Y)
											{
												Projectile expr_4D74_cp_0 = Main.projectile[i];
												expr_4D74_cp_0.velocity.Y = expr_4D74_cp_0.velocity.Y - num67;
												Projectile.velocity.Y = Projectile.velocity.Y + num67;
											}
											else
											{
												Projectile expr_4DA5_cp_0 = Main.projectile[i];
												expr_4DA5_cp_0.velocity.Y = expr_4DA5_cp_0.velocity.Y + num67;
												Projectile.velocity.Y = Projectile.velocity.Y - num67;
											}
											if (Projectile.Center.X > Main.projectile[i].Center.X)
											{
												Projectile.velocity.X = Projectile.velocity.X + num67;
												Projectile expr_4E07_cp_0 = Main.projectile[i];
												expr_4E07_cp_0.velocity.X = expr_4E07_cp_0.velocity.X - num67;
											}
											else
											{
												Projectile.velocity.X = Projectile.velocity.X - num67;
												Projectile expr_4E38_cp_0 = Main.projectile[i];
												expr_4E38_cp_0.velocity.Y = expr_4E38_cp_0.velocity.Y + num67;
											}
										}
									}
								}
							}
						}
						float num66 = 300f;
						if ((double)Projectile.position.Y > Main.worldSurface * 16.0)
						{
							num66 = 150f;
						}
						if (Projectile.type >= 390 && Projectile.type <= 392)
						{
							num66 = 500f;
							if ((double)Projectile.position.Y > Main.worldSurface * 16.0)
							{
								num66 = 250f;
							}
						}
						if (num77 < num66 + num81 && num75 == -1)
						{
							float num65 = num79 - (Projectile.position.X + (float)(Projectile.width / 2));
							if (num65 < -5f)
							{
								flag = true;
								flag2 = false;
							}
							else if (num65 > 5f)
							{
								flag2 = true;
								flag = false;
							}
						}
						bool flag3 = false;
						if (Projectile.type >= 390 && Projectile.type <= 392 && Projectile.localAI[1] > 0f)
						{
							flag3 = true;
							Projectile.localAI[1] -= 1f;
						}
						if (num75 >= 0 && num77 < 800f + num81)
						{
							Projectile.friendly = true;
							Projectile.localAI[0] = (float)num80;
							float num64 = num79 - (Projectile.position.X + (float)(Projectile.width / 2));
							if (num64 < -10f)
							{
								flag = true;
								flag2 = false;
							}
							else if (num64 > 10f)
							{
								flag2 = true;
								flag = false;
							}
							if (num78 < Projectile.Center.Y - 100f && num64 > -50f && num64 < 50f && Projectile.velocity.Y == 0f)
							{
								float num63 = Math.Abs(num78 - Projectile.Center.Y);
								if (num63 < 120f)
								{
									Projectile.velocity.Y = -10f;
								}
								else if (num63 < 210f)
								{
									Projectile.velocity.Y = -13f;
								}
								else if (num63 < 270f)
								{
									Projectile.velocity.Y = -15f;
								}
								else if (num63 < 310f)
								{
									Projectile.velocity.Y = -17f;
								}
								else if (num63 < 380f)
								{
									Projectile.velocity.Y = -18f;
								}
							}
							if (flag3)
							{
								Projectile.friendly = false;
								if (Projectile.velocity.X < 0f)
								{
									flag = true;
								}
								else if (Projectile.velocity.X > 0f)
								{
									flag2 = true;
								}
							}
						}
						else
						{
							Projectile.friendly = false;
						}
					}
				}
				if (Projectile.ai[1] != 0f)
				{
					flag = false;
					flag2 = false;
				}
				else if (flag9 && Projectile.localAI[0] == 0f)
				{
					Projectile.direction = Main.player[Projectile.owner].direction;
				}
				else if (Projectile.type >= 390 && Projectile.type <= 392)
				{
					int num62 = (int)(Projectile.Center.X / 16f);
					int num61 = (int)(Projectile.Center.Y / 16f);
					if (Main.tile[num62, num61] != null && Main.tile[num62, num61].WallType > 0)
					{
						flag2 = (flag = false);
					}
				}
				if (Projectile.type == 127)
				{
					if ((double)Projectile.rotation > -0.1 && (double)Projectile.rotation < 0.1)
					{
						Projectile.rotation = 0f;
					}
					else if (Projectile.rotation < 0f)
					{
						Projectile.rotation += 0.1f;
					}
					else
					{
						Projectile.rotation -= 0.1f;
					}
				}
				else if (Projectile.type != 313 && !flag4)
				{
					Projectile.rotation = 0f;
				}
				if (Projectile.type < 390 || Projectile.type > 392)
				{
					Projectile.tileCollide = true;
				}
				float num60 = 0.08f;
				float num59 = 6.5f;
				if (Projectile.type == 127)
				{
					num59 = 2f;
					num60 = 0.04f;
				}
				if (Projectile.type == 112)
				{
					num59 = 6f;
					num60 = 0.06f;
				}
				if (Projectile.type == 334)
				{
					num59 = 8f;
					num60 = 0.08f;
				}
				if (Projectile.type == 268)
				{
					num59 = 8f;
					num60 = 0.4f;
				}
				if (Projectile.type == 324)
				{
					num60 = 0.1f;
					num59 = 3f;
				}
				if (flag9 || Projectile.type == 266 || (Projectile.type >= 390 && Projectile.type <= 392))
				{
					num59 = 6f;
					num60 = 0.2f;
					if (num59 < Math.Abs(Main.player[Projectile.owner].velocity.X) + Math.Abs(Main.player[Projectile.owner].velocity.Y))
					{
						num59 = Math.Abs(Main.player[Projectile.owner].velocity.X) + Math.Abs(Main.player[Projectile.owner].velocity.Y);
						num60 = 0.3f;
					}
				}
				if (Projectile.type >= 390 && Projectile.type <= 392)
				{
					num60 *= 2f;
				}
				if (flag)
				{
					if ((double)Projectile.velocity.X > -3.5)
					{
						Projectile.velocity.X = Projectile.velocity.X - num60;
					}
					else
					{
						Projectile.velocity.X = Projectile.velocity.X - num60 * 0.25f;
					}
				}
				else if (flag2)
				{
					if ((double)Projectile.velocity.X < 3.5)
					{
						Projectile.velocity.X = Projectile.velocity.X + num60;
					}
					else
					{
						Projectile.velocity.X = Projectile.velocity.X + num60 * 0.25f;
					}
				}
				else
				{
					Projectile.velocity.X = Projectile.velocity.X * 0.9f;
					if (Projectile.velocity.X >= 0f - num60 && Projectile.velocity.X <= num60)
					{
						Projectile.velocity.X = 0f;
					}
				}
				if (Projectile.type == 208)
				{
					Projectile.velocity.X = Projectile.velocity.X * 0.95f;
					if ((double)Projectile.velocity.X > -0.1 && (double)Projectile.velocity.X < 0.1)
					{
						Projectile.velocity.X = 0f;
					}
					flag = false;
					flag2 = false;
				}
				if (flag | flag2)
				{
					int num58 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
					int j4 = (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16;
					if (Projectile.type == 236)
					{
						num58 += Projectile.direction;
					}
					if (flag)
					{
						num58--;
					}
					if (flag2)
					{
						num58++;
					}
					num58 += (int)Projectile.velocity.X;
					if (WorldGen.SolidTile(num58, j4))
					{
						flag7 = true;
					}
				}
				if (Main.player[Projectile.owner].position.Y + (float)Main.player[Projectile.owner].height - 8f > Projectile.position.Y + (float)Projectile.height)
				{
					flag6 = true;
				}
				if (Projectile.type == 268 && Projectile.frameCounter < 10)
				{
					flag7 = false;
				}
				Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY, 1, false, 0);
				if (Projectile.velocity.Y == 0f || Projectile.type == 200)
				{
					if (!flag6 && (Projectile.velocity.X < 0f || Projectile.velocity.X > 0f))
					{
						int num56 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
						int j3 = (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16 + 1;
						if (flag)
						{
							num56--;
						}
						if (flag2)
						{
							num56++;
						}
						WorldGen.SolidTile(num56, j3);
					}
					if (flag7)
					{
						int num55 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
						int num54 = (int)(Projectile.position.Y + (float)Projectile.height) / 16 + 1;
						if (WorldGen.SolidTile(num55, num54) || Main.tile[num55, num54].IsHalfBlock || Main.tile[num55, num54].Slope > 0 || Projectile.type == 200)
						{
							if (Projectile.type == 200)
							{
								Projectile.velocity.Y = -3.1f;
							}
							else
							{
								try
								{
									num55 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
									num54 = (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16;
									if (flag)
									{
										num55--;
									}
									if (flag2)
									{
										num55++;
									}
									num55 += (int)Projectile.velocity.X;
									if (!WorldGen.SolidTile(num55, num54 - 1) && !WorldGen.SolidTile(num55, num54 - 2))
									{
										Projectile.velocity.Y = -5.1f;
									}
									else if (!WorldGen.SolidTile(num55, num54 - 2))
									{
										Projectile.velocity.Y = -7.1f;
									}
									else if (WorldGen.SolidTile(num55, num54 - 5))
									{
										Projectile.velocity.Y = -11.1f;
									}
									else if (WorldGen.SolidTile(num55, num54 - 4))
									{
										Projectile.velocity.Y = -10.1f;
									}
									else
									{
										Projectile.velocity.Y = -9.1f;
									}
								}
								catch
								{
									Projectile.velocity.Y = -9.1f;
								}
							}
							if (Projectile.type == 127)
							{
								Projectile.ai[0] = 1f;
							}
						}
					}
					else if (Projectile.type == 266 && (flag | flag2))
					{
						Projectile.velocity.Y = Projectile.velocity.Y - 6f;
					}
				}
				if (Projectile.velocity.X > num59)
				{
					Projectile.velocity.X = num59;
				}
				if (Projectile.velocity.X < 0f - num59)
				{
					Projectile.velocity.X = 0f - num59;
				}
				if (Projectile.velocity.X < 0f)
				{
					Projectile.direction = -1;
				}
				if (Projectile.velocity.X > 0f)
				{
					Projectile.direction = 1;
				}
				if (Projectile.velocity.X > num60 & flag2)
				{
					Projectile.direction = 1;
				}
				if (Projectile.velocity.X < 0f - num60 & flag)
				{
					Projectile.direction = -1;
				}
				if (Projectile.type != 313)
				{
					if (Projectile.direction == -1)
					{
						Projectile.spriteDirection = 1;
					}
					if (Projectile.direction == 1)
					{
						Projectile.spriteDirection = -1;
					}
				}
				if (Projectile.type == 398)
				{
					Projectile.spriteDirection = Projectile.direction;
				}
				if (flag9)
				{
					if (Projectile.ai[1] > 0f)
					{
						if (Projectile.localAI[1] == 0f)
						{
							Projectile.localAI[1] = 1f;
							Projectile.frame = 1;
						}
						if (Projectile.frame != 0)
						{
							Projectile.frameCounter++;
							if (Projectile.frameCounter > 4)
							{
								Projectile.frame++;
								Projectile.frameCounter = 0;
							}
							if (Projectile.frame == 4)
							{
								Projectile.frame = 0;
							}
						}
					}
					else if (Projectile.velocity.Y == 0f)
					{
						Projectile.localAI[1] = 0f;
						if (Projectile.velocity.X == 0f)
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
						else if ((double)Projectile.velocity.X < -0.8 || (double)Projectile.velocity.X > 0.8)
						{
							Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
							Projectile.frameCounter++;
							if (Projectile.frameCounter > 6)
							{
								Projectile.frame++;
								Projectile.frameCounter = 0;
							}
							if (Projectile.frame < 5)
							{
								Projectile.frame = 5;
							}
							if (Projectile.frame >= 11)
							{
								Projectile.frame = 5;
							}
						}
						else
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
					}
					else if (Projectile.velocity.Y < 0f)
					{
						Projectile.frameCounter = 0;
						Projectile.frame = 4;
					}
					else if (Projectile.velocity.Y > 0f)
					{
						Projectile.frameCounter = 0;
						Projectile.frame = 4;
					}
					Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
					if (Projectile.velocity.Y > 10f)
					{
						Projectile.velocity.Y = 10f;
					}
					Vector2 velocity = Projectile.velocity;
				}
				else if (Projectile.type == 268)
				{
					if (Projectile.velocity.Y == 0f)
					{
						if (Projectile.frame > 5)
						{
							Projectile.frameCounter = 0;
						}
						if (Projectile.velocity.X == 0f)
						{
							int num50 = 3;
							Projectile.frameCounter++;
							if (Projectile.frameCounter < num50)
							{
								Projectile.frame = 0;
							}
							else if (Projectile.frameCounter < num50 * 2)
							{
								Projectile.frame = 1;
							}
							else if (Projectile.frameCounter < num50 * 3)
							{
								Projectile.frame = 2;
							}
							else if (Projectile.frameCounter < num50 * 4)
							{
								Projectile.frame = 3;
							}
							else
							{
								Projectile.frameCounter = num50 * 4;
							}
						}
						else
						{
							Projectile.velocity.X = Projectile.velocity.X * 0.8f;
							Projectile.frameCounter++;
							int num49 = 3;
							if (Projectile.frameCounter < num49)
							{
								Projectile.frame = 0;
							}
							else if (Projectile.frameCounter < num49 * 2)
							{
								Projectile.frame = 1;
							}
							else if (Projectile.frameCounter < num49 * 3)
							{
								Projectile.frame = 2;
							}
							else if (Projectile.frameCounter < num49 * 4)
							{
								Projectile.frame = 3;
							}
							else if (flag | flag2)
							{
								Projectile.velocity.X = Projectile.velocity.X * 2f;
								Projectile.frame = 4;
								Projectile.velocity.Y = -6.1f;
								Projectile.frameCounter = 0;
								for (int num48 = 0; num48 < 4; num48++)
								{
									int num47 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + (float)Projectile.height - 2f), Projectile.width, 4, 5, 0f, 0f, 0, default(Color), 1f);
									Dust obj7 = Main.dust[num47];
									obj7.velocity += Projectile.velocity;
									Dust obj8 = Main.dust[num47];
									obj8.velocity *= 0.4f;
								}
							}
							else
							{
								Projectile.frameCounter = num49 * 4;
							}
						}
					}
					else if (Projectile.velocity.Y < 0f)
					{
						Projectile.frameCounter = 0;
						Projectile.frame = 5;
					}
					else
					{
						Projectile.frame = 4;
						Projectile.frameCounter = 3;
					}
					Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
					if (Projectile.velocity.Y > 10f)
					{
						Projectile.velocity.Y = 10f;
					}
				}
				else if (Projectile.type == 269)
				{
					if (Projectile.velocity.Y >= 0f && (double)Projectile.velocity.Y <= 0.8)
					{
						if (Projectile.velocity.X == 0f)
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
						else if ((double)Projectile.velocity.X < -0.8 || (double)Projectile.velocity.X > 0.8)
						{
							int num46 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + (float)Projectile.height - 2f), Projectile.width, 6, 76, 0f, 0f, 0, default(Color), 1f);
							Main.dust[num46].noGravity = true;
							Dust obj9 = Main.dust[num46];
							obj9.velocity *= 0.3f;
							Main.dust[num46].noLight = true;
							Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
							Projectile.frameCounter++;
							if (Projectile.frameCounter > 6)
							{
								Projectile.frame++;
								Projectile.frameCounter = 0;
							}
							if (Projectile.frame > 3)
							{
								Projectile.frame = 0;
							}
						}
						else
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
					}
					else
					{
						Projectile.frameCounter = 0;
						Projectile.frame = 2;
					}
					Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
					if (Projectile.velocity.Y > 10f)
					{
						Projectile.velocity.Y = 10f;
					}
				}
				else if (Projectile.type == 313)
				{
					int num45 = (int)(Projectile.Center.X / 16f);
					int num44 = (int)(Projectile.Center.Y / 16f);
					if (Main.tile[num45, num44] != null && Main.tile[num45, num44].WallType > 0)
					{
						Projectile.position.Y = Projectile.position.Y + (float)Projectile.height;
						Projectile.height = 34;
						Projectile.position.Y = Projectile.position.Y - (float)Projectile.height;
						Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
						Projectile.width = 34;
						Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
						Vector2 vector5 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
						float num43 = Main.player[Projectile.owner].Center.X - vector5.X;
						float num42 = Main.player[Projectile.owner].Center.Y - vector5.Y;
						float num41 = (float)Math.Sqrt((double)(num43 * num43 + num42 * num42));
						float num40 = 4f / num41;
						num43 *= num40;
						num42 *= num40;
						if (num41 < 120f)
						{
							Projectile.velocity.X = Projectile.velocity.X * 0.9f;
							Projectile.velocity.Y = Projectile.velocity.Y * 0.9f;
							if ((double)(Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) < 0.1)
							{
								Projectile.velocity *= 0f;
							}
						}
						else
						{
							Projectile.velocity.X = (Projectile.velocity.X * 9f + num43) / 10f;
							Projectile.velocity.Y = (Projectile.velocity.Y * 9f + num42) / 10f;
						}
						if (num41 >= 120f)
						{
							Projectile.spriteDirection = Projectile.direction;
							Projectile.rotation = (float)Math.Atan2((double)(Projectile.velocity.Y * (0f - (float)Projectile.direction)), (double)(Projectile.velocity.X * (0f - (float)Projectile.direction)));
						}
						Projectile.frameCounter += (int)(Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y));
						if (Projectile.frameCounter > 6)
						{
							Projectile.frame++;
							Projectile.frameCounter = 0;
						}
						if (Projectile.frame > 10)
						{
							Projectile.frame = 5;
						}
						if (Projectile.frame < 5)
						{
							Projectile.frame = 10;
						}
					}
					else
					{
						Projectile.rotation = 0f;
						if (Projectile.direction == -1)
						{
							Projectile.spriteDirection = 1;
						}
						if (Projectile.direction == 1)
						{
							Projectile.spriteDirection = -1;
						}
						Projectile.position.Y = Projectile.position.Y + (float)Projectile.height;
						Projectile.height = 30;
						Projectile.position.Y = Projectile.position.Y - (float)Projectile.height;
						Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
						Projectile.width = 30;
						Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
						if (Projectile.velocity.Y >= 0f && (double)Projectile.velocity.Y <= 0.8)
						{
							if (Projectile.velocity.X == 0f)
							{
								Projectile.frame = 0;
								Projectile.frameCounter = 0;
							}
							else if ((double)Projectile.velocity.X < -0.8 || (double)Projectile.velocity.X > 0.8)
							{
								Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
								Projectile.frameCounter++;
								if (Projectile.frameCounter > 6)
								{
									Projectile.frame++;
									Projectile.frameCounter = 0;
								}
								if (Projectile.frame > 3)
								{
									Projectile.frame = 0;
								}
							}
							else
							{
								Projectile.frame = 0;
								Projectile.frameCounter = 0;
							}
						}
						else
						{
							Projectile.frameCounter = 0;
							Projectile.frame = 4;
						}
						Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
						if (Projectile.velocity.Y > 10f)
						{
							Projectile.velocity.Y = 10f;
						}
					}
				}
				else if (Projectile.type >= 390 && Projectile.type <= 392)
				{
					int num37 = (int)(Projectile.Center.X / 16f);
					int num36 = (int)(Projectile.Center.Y / 16f);
					if (Main.tile[num37, num36] != null && Main.tile[num37, num36].WallType > 0)
					{
						Projectile.position.Y = Projectile.position.Y + (float)Projectile.height;
						Projectile.height = 34;
						Projectile.position.Y = Projectile.position.Y - (float)Projectile.height;
						Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
						Projectile.width = 34;
						Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
						float scaleFactor = 9f;
						float num35 = (float)(40 * (Projectile.minionPos + 1));
						Vector2 vector12 = Main.player[Projectile.owner].Center - Projectile.Center;
						if (flag5)
						{
							vector12 = vector6;
							num35 = 10f;
						}
						else if (!Collision.CanHitLine(Projectile.Center, 1, 1, Main.player[Projectile.owner].Center, 1, 1))
						{
							Projectile.ai[0] = 1f;
						}
						if (vector12.Length() < num35)
						{
							Projectile.velocity *= 0.9f;
							if ((double)(Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) < 0.1)
							{
								Projectile.velocity *= 0f;
							}
						}
						else if (vector12.Length() < 800f || !flag5)
						{
							Projectile.velocity = (Projectile.velocity * 9f + Vector2.Normalize(vector12) * scaleFactor) / 10f;
						}
						if (vector12.Length() >= num35)
						{
							Projectile.spriteDirection = Projectile.direction;
							Projectile.rotation = Projectile.velocity.ToRotation() + 1.57079637f;
						}
						else
						{
							Projectile.rotation = vector12.ToRotation() + 1.57079637f;
						}
						Projectile.frameCounter += (int)(Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y));
						if (Projectile.frameCounter > 5)
						{
							Projectile.frame++;
							Projectile.frameCounter = 0;
						}
						if (Projectile.frame > 7)
						{
							Projectile.frame = 4;
						}
						if (Projectile.frame < 4)
						{
							Projectile.frame = 7;
						}
					}
					else
					{
						if (!flag4)
						{
							Projectile.rotation = 0f;
						}
						if (Projectile.direction == -1)
						{
							Projectile.spriteDirection = 1;
						}
						if (Projectile.direction == 1)
						{
							Projectile.spriteDirection = -1;
						}
						Projectile.position.Y = Projectile.position.Y + (float)Projectile.height;
						Projectile.height = 30;
						Projectile.position.Y = Projectile.position.Y - (float)Projectile.height;
						Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
						Projectile.width = 30;
						Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
						if (!flag5 && !Collision.CanHitLine(Projectile.Center, 1, 1, Main.player[Projectile.owner].Center, 1, 1))
						{
							Projectile.ai[0] = 1f;
						}
						if (!flag4 && Projectile.frame >= 4 && Projectile.frame <= 7)
						{
							Vector2 vector4 = Main.player[Projectile.owner].Center - Projectile.Center;
							if (flag5)
							{
								vector4 = vector6;
							}
							float num34 = 0f - vector4.Y;
							if (vector4.Y <= 0f)
							{
								if (num34 < 120f)
								{
									Projectile.velocity.Y = -10f;
								}
								else if (num34 < 210f)
								{
									Projectile.velocity.Y = -13f;
								}
								else if (num34 < 270f)
								{
									Projectile.velocity.Y = -15f;
								}
								else if (num34 < 310f)
								{
									Projectile.velocity.Y = -17f;
								}
								else if (num34 < 380f)
								{
									Projectile.velocity.Y = -18f;
								}
							}
						}
						if (flag4)
						{
							Projectile.frameCounter++;
							if (Projectile.frameCounter > 3)
							{
								Projectile.frame++;
								Projectile.frameCounter = 0;
							}
							if (Projectile.frame >= 8)
							{
								Projectile.frame = 4;
							}
							if (Projectile.frame <= 3)
							{
								Projectile.frame = 7;
							}
						}
						else if (Projectile.velocity.Y >= 0f && (double)Projectile.velocity.Y <= 0.8)
						{
							if (Projectile.velocity.X == 0f)
							{
								Projectile.frame = 0;
								Projectile.frameCounter = 0;
							}
							else if ((double)Projectile.velocity.X < -0.8 || (double)Projectile.velocity.X > 0.8)
							{
								Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
								Projectile.frameCounter++;
								if (Projectile.frameCounter > 5)
								{
									Projectile.frame++;
									Projectile.frameCounter = 0;
								}
								if (Projectile.frame > 2)
								{
									Projectile.frame = 0;
								}
							}
							else
							{
								Projectile.frame = 0;
								Projectile.frameCounter = 0;
							}
						}
						else
						{
							Projectile.frameCounter = 0;
							Projectile.frame = 3;
						}
						Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
						if (Projectile.velocity.Y > 10f)
						{
							Projectile.velocity.Y = 10f;
						}
					}
				}
				else if (Projectile.type == 314)
				{
					if (Projectile.velocity.Y >= 0f && (double)Projectile.velocity.Y <= 0.8)
					{
						if (Projectile.velocity.X == 0f)
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
						else if ((double)Projectile.velocity.X < -0.8 || (double)Projectile.velocity.X > 0.8)
						{
							Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
							Projectile.frameCounter++;
							if (Projectile.frameCounter > 6)
							{
								Projectile.frame++;
								Projectile.frameCounter = 0;
							}
							if (Projectile.frame > 6)
							{
								Projectile.frame = 1;
							}
						}
						else
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
					}
					else
					{
						Projectile.frameCounter = 0;
						Projectile.frame = 7;
					}
					Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
					if (Projectile.velocity.Y > 10f)
					{
						Projectile.velocity.Y = 10f;
					}
				}
				else if (Projectile.type == 319)
				{
					if (Projectile.velocity.Y >= 0f && (double)Projectile.velocity.Y <= 0.8)
					{
						if (Projectile.velocity.X == 0f)
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
						else if ((double)Projectile.velocity.X < -0.8 || (double)Projectile.velocity.X > 0.8)
						{
							Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
							Projectile.frameCounter++;
							if (Projectile.frameCounter > 8)
							{
								Projectile.frame++;
								Projectile.frameCounter = 0;
							}
							if (Projectile.frame > 5)
							{
								Projectile.frame = 2;
							}
						}
						else
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
					}
					else
					{
						Projectile.frameCounter = 0;
						Projectile.frame = 1;
					}
					Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
					if (Projectile.velocity.Y > 10f)
					{
						Projectile.velocity.Y = 10f;
					}
				}
				else if (Projectile.type == 236)
				{
					if (Projectile.velocity.Y >= 0f && (double)Projectile.velocity.Y <= 0.8)
					{
						if (Projectile.velocity.X == 0f)
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
						else if ((double)Projectile.velocity.X < -0.8 || (double)Projectile.velocity.X > 0.8)
						{
							if (Projectile.frame < 2)
							{
								Projectile.frame = 2;
							}
							Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
							Projectile.frameCounter++;
							if (Projectile.frameCounter > 6)
							{
								Projectile.frame++;
								Projectile.frameCounter = 0;
							}
							if (Projectile.frame > 8)
							{
								Projectile.frame = 2;
							}
						}
						else
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
					}
					else
					{
						Projectile.frameCounter = 0;
						Projectile.frame = 1;
					}
					Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
					if (Projectile.velocity.Y > 10f)
					{
						Projectile.velocity.Y = 10f;
					}
				}
				else if (Projectile.type == 499)
				{
					if (Projectile.velocity.Y >= 0f && (double)Projectile.velocity.Y <= 0.8)
					{
						if (Projectile.velocity.X == 0f)
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
						else if ((double)Projectile.velocity.X < -0.8 || (double)Projectile.velocity.X > 0.8)
						{
							if (Projectile.frame < 2)
							{
								Projectile.frame = 2;
							}
							Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
							Projectile.frameCounter++;
							if (Projectile.frameCounter > 6)
							{
								Projectile.frame++;
								Projectile.frameCounter = 0;
							}
							if (Projectile.frame >= 8)
							{
								Projectile.frame = 2;
							}
						}
						else
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
					}
					else
					{
						Projectile.frameCounter = 0;
						Projectile.frame = 1;
					}
					Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
					if (Projectile.velocity.Y > 10f)
					{
						Projectile.velocity.Y = 10f;
					}
				}
				else if (Projectile.type == 266)
				{
					if (Projectile.velocity.Y >= 0f && (double)Projectile.velocity.Y <= 0.8)
					{
						if (Projectile.velocity.X == 0f)
						{
							Projectile.frameCounter++;
						}
						else
						{
							Projectile.frameCounter += 3;
						}
					}
					else
					{
						Projectile.frameCounter += 5;
					}
					if (Projectile.frameCounter >= 20)
					{
						Projectile.frameCounter -= 20;
						Projectile.frame++;
					}
					if (Projectile.frame > 1)
					{
						Projectile.frame = 0;
					}
					if (Projectile.wet && Main.player[Projectile.owner].position.Y + (float)Main.player[Projectile.owner].height < Projectile.position.Y + (float)Projectile.height && Projectile.localAI[0] == 0f)
					{
						if (Projectile.velocity.Y > -4f)
						{
							Projectile.velocity.Y = Projectile.velocity.Y - 0.2f;
						}
						if (Projectile.velocity.Y > 0f)
						{
							Projectile.velocity.Y = Projectile.velocity.Y * 0.95f;
						}
					}
					else
					{
						Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
					}
					if (Projectile.velocity.Y > 10f)
					{
						Projectile.velocity.Y = 10f;
					}
				}
				else if (Projectile.type == 334)
				{
					if (Projectile.velocity.Y == 0f)
					{
						if (Projectile.velocity.X == 0f)
						{
							if (Projectile.frame > 0)
							{
								Projectile.frameCounter += 2;
								if (Projectile.frameCounter > 6)
								{
									Projectile.frame++;
									Projectile.frameCounter = 0;
								}
								if (Projectile.frame >= 7)
								{
									Projectile.frame = 0;
								}
							}
							else
							{
								Projectile.frame = 0;
								Projectile.frameCounter = 0;
							}
						}
						else if ((double)Projectile.velocity.X < -0.8 || (double)Projectile.velocity.X > 0.8)
						{
							Projectile.frameCounter += (int)Math.Abs((double)Projectile.velocity.X * 0.75);
							Projectile.frameCounter++;
							if (Projectile.frameCounter > 6)
							{
								Projectile.frame++;
								Projectile.frameCounter = 0;
							}
							if (Projectile.frame >= 7 || Projectile.frame < 1)
							{
								Projectile.frame = 1;
							}
						}
						else if (Projectile.frame > 0)
						{
							Projectile.frameCounter += 2;
							if (Projectile.frameCounter > 6)
							{
								Projectile.frame++;
								Projectile.frameCounter = 0;
							}
							if (Projectile.frame >= 7)
							{
								Projectile.frame = 0;
							}
						}
						else
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
					}
					else if (Projectile.velocity.Y < 0f)
					{
						Projectile.frameCounter = 0;
						Projectile.frame = 2;
					}
					else if (Projectile.velocity.Y > 0f)
					{
						Projectile.frameCounter = 0;
						Projectile.frame = 4;
					}
					Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
					if (Projectile.velocity.Y > 10f)
					{
						Projectile.velocity.Y = 10f;
					}
				}
				else if (Projectile.type == 353)
				{
					if (Projectile.velocity.Y == 0f)
					{
						if (Projectile.velocity.X == 0f)
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
						else if ((double)Projectile.velocity.X < -0.8 || (double)Projectile.velocity.X > 0.8)
						{
							Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
							Projectile.frameCounter++;
							if (Projectile.frameCounter > 6)
							{
								Projectile.frame++;
								Projectile.frameCounter = 0;
							}
							if (Projectile.frame > 9)
							{
								Projectile.frame = 2;
							}
						}
						else
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
					}
					else if (Projectile.velocity.Y < 0f)
					{
						Projectile.frameCounter = 0;
						Projectile.frame = 1;
					}
					else if (Projectile.velocity.Y > 0f)
					{
						Projectile.frameCounter = 0;
						Projectile.frame = 1;
					}
					Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
					if (Projectile.velocity.Y > 10f)
					{
						Projectile.velocity.Y = 10f;
					}
				}
				else if (Projectile.type == 111)
				{
					if (Projectile.velocity.Y == 0f)
					{
						if (Projectile.velocity.X == 0f)
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
						else if ((double)Projectile.velocity.X < -0.8 || (double)Projectile.velocity.X > 0.8)
						{
							Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
							Projectile.frameCounter++;
							if (Projectile.frameCounter > 6)
							{
								Projectile.frame++;
								Projectile.frameCounter = 0;
							}
							if (Projectile.frame >= 7)
							{
								Projectile.frame = 0;
							}
						}
						else
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
					}
					else if (Projectile.velocity.Y < 0f)
					{
						Projectile.frameCounter = 0;
						Projectile.frame = 4;
					}
					else if (Projectile.velocity.Y > 0f)
					{
						Projectile.frameCounter = 0;
						Projectile.frame = 6;
					}
					Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
					if (Projectile.velocity.Y > 10f)
					{
						Projectile.velocity.Y = 10f;
					}
				}
				else if (Projectile.type == 112)
				{
					if (Projectile.velocity.Y == 0f)
					{
						if (Projectile.velocity.X == 0f)
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
						else if ((double)Projectile.velocity.X < -0.8 || (double)Projectile.velocity.X > 0.8)
						{
							Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
							Projectile.frameCounter++;
							if (Projectile.frameCounter > 6)
							{
								Projectile.frame++;
								Projectile.frameCounter = 0;
							}
							if (Projectile.frame >= 3)
							{
								Projectile.frame = 0;
							}
						}
						else
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
					}
					else if (Projectile.velocity.Y < 0f)
					{
						Projectile.frameCounter = 0;
						Projectile.frame = 2;
					}
					else if (Projectile.velocity.Y > 0f)
					{
						Projectile.frameCounter = 0;
						Projectile.frame = 2;
					}
					Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
					if (Projectile.velocity.Y > 10f)
					{
						Projectile.velocity.Y = 10f;
					}
				}
				else if (Projectile.type == 127)
				{
					if (Projectile.velocity.Y == 0f)
					{
						if (Projectile.velocity.X == 0f)
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
						else if ((double)Projectile.velocity.X < -0.1 || (double)Projectile.velocity.X > 0.1)
						{
							Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
							Projectile.frameCounter++;
							if (Projectile.frameCounter > 6)
							{
								Projectile.frame++;
								Projectile.frameCounter = 0;
							}
							if (Projectile.frame > 5)
							{
								Projectile.frame = 0;
							}
						}
						else
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
					}
					else
					{
						Projectile.frame = 0;
						Projectile.frameCounter = 0;
					}
					Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
					if (Projectile.velocity.Y > 10f)
					{
						Projectile.velocity.Y = 10f;
					}
				}
				else if (Projectile.type == 200)
				{
					if (Projectile.velocity.Y == 0f)
					{
						if (Projectile.velocity.X == 0f)
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
						else if ((double)Projectile.velocity.X < -0.1 || (double)Projectile.velocity.X > 0.1)
						{
							Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
							Projectile.frameCounter++;
							if (Projectile.frameCounter > 6)
							{
								Projectile.frame++;
								Projectile.frameCounter = 0;
							}
							if (Projectile.frame > 5)
							{
								Projectile.frame = 0;
							}
						}
						else
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
					}
					else
					{
						Projectile.rotation = Projectile.velocity.X * 0.1f;
						Projectile.frameCounter++;
						if (Projectile.velocity.Y < 0f)
						{
							Projectile.frameCounter += 2;
						}
						if (Projectile.frameCounter > 6)
						{
							Projectile.frame++;
							Projectile.frameCounter = 0;
						}
						if (Projectile.frame > 9)
						{
							Projectile.frame = 6;
						}
						if (Projectile.frame < 6)
						{
							Projectile.frame = 6;
						}
					}
					Projectile.velocity.Y = Projectile.velocity.Y + 0.1f;
					if (Projectile.velocity.Y > 4f)
					{
						Projectile.velocity.Y = 4f;
					}
				}
				else if (Projectile.type == 208)
				{
					if (Projectile.velocity.Y == 0f && Projectile.velocity.X == 0f)
					{
						if (Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) < Projectile.position.X + (float)(Projectile.width / 2))
						{
							Projectile.direction = -1;
						}
						else if (Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) > Projectile.position.X + (float)(Projectile.width / 2))
						{
							Projectile.direction = 1;
						}
						Projectile.rotation = 0f;
						Projectile.frame = 0;
					}
					else
					{
						Projectile.rotation = Projectile.velocity.X * 0.075f;
						Projectile.frameCounter++;
						if (Projectile.frameCounter > 6)
						{
							Projectile.frame++;
							Projectile.frameCounter = 0;
						}
						if (Projectile.frame > 4)
						{
							Projectile.frame = 1;
						}
						if (Projectile.frame < 1)
						{
							Projectile.frame = 1;
						}
					}
					Projectile.velocity.Y = Projectile.velocity.Y + 0.1f;
					if (Projectile.velocity.Y > 4f)
					{
						Projectile.velocity.Y = 4f;
					}
				}
				else if (Projectile.type == 209)
				{
					if (Projectile.alpha > 0)
					{
						Projectile.alpha -= 5;
						if (Projectile.alpha < 0)
						{
							Projectile.alpha = 0;
						}
					}
					if (Projectile.velocity.Y == 0f)
					{
						if (Projectile.velocity.X == 0f)
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
						else if ((double)Projectile.velocity.X < -0.1 || (double)Projectile.velocity.X > 0.1)
						{
							Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
							Projectile.frameCounter++;
							if (Projectile.frameCounter > 6)
							{
								Projectile.frame++;
								Projectile.frameCounter = 0;
							}
							if (Projectile.frame > 11)
							{
								Projectile.frame = 2;
							}
							if (Projectile.frame < 2)
							{
								Projectile.frame = 2;
							}
						}
						else
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
					}
					else
					{
						Projectile.frame = 1;
						Projectile.frameCounter = 0;
						Projectile.rotation = 0f;
					}
					Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
					if (Projectile.velocity.Y > 10f)
					{
						Projectile.velocity.Y = 10f;
					}
				}
				else if (Projectile.type == 324)
				{
					if (Projectile.velocity.Y == 0f)
					{
						if ((double)Projectile.velocity.X < -0.1 || (double)Projectile.velocity.X > 0.1)
						{
							Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
							Projectile.frameCounter++;
							if (Projectile.frameCounter > 6)
							{
								Projectile.frame++;
								Projectile.frameCounter = 0;
							}
							if (Projectile.frame > 5)
							{
								Projectile.frame = 2;
							}
							if (Projectile.frame < 2)
							{
								Projectile.frame = 2;
							}
						}
						else
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
					}
					else
					{
						Projectile.frameCounter = 0;
						Projectile.frame = 1;
					}
					Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
					if (Projectile.velocity.Y > 14f)
					{
						Projectile.velocity.Y = 14f;
					}
				}
				else if (Projectile.type == 210)
				{
					if (Projectile.velocity.Y == 0f)
					{
						if ((double)Projectile.velocity.X < -0.1 || (double)Projectile.velocity.X > 0.1)
						{
							Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
							Projectile.frameCounter++;
							if (Projectile.frameCounter > 6)
							{
								Projectile.frame++;
								Projectile.frameCounter = 0;
							}
							if (Projectile.frame > 6)
							{
								Projectile.frame = 0;
							}
						}
						else
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
					}
					else
					{
						Projectile.rotation = Projectile.velocity.X * 0.05f;
						Projectile.frameCounter++;
						if (Projectile.frameCounter > 6)
						{
							Projectile.frame++;
							Projectile.frameCounter = 0;
						}
						if (Projectile.frame > 11)
						{
							Projectile.frame = 7;
						}
						if (Projectile.frame < 7)
						{
							Projectile.frame = 7;
						}
					}
					Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
					if (Projectile.velocity.Y > 10f)
					{
						Projectile.velocity.Y = 10f;
					}
				}
				else if (Projectile.type == 398)
				{
					if (Projectile.velocity.Y == 0f)
					{
						if (Projectile.velocity.X == 0f)
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
						else if ((double)Projectile.velocity.X < -0.8 || (double)Projectile.velocity.X > 0.8)
						{
							Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
							Projectile.frameCounter++;
							if (Projectile.frameCounter > 6)
							{
								Projectile.frame++;
								Projectile.frameCounter = 0;
							}
							if (Projectile.frame >= 5)
							{
								Projectile.frame = 0;
							}
						}
						else
						{
							Projectile.frame = 0;
							Projectile.frameCounter = 0;
						}
					}
					else if (Projectile.velocity.Y != 0f)
					{
						Projectile.frameCounter = 0;
						Projectile.frame = 5;
					}
					Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
					if (Projectile.velocity.Y > 10f)
					{
						Projectile.velocity.Y = 10f;
					}
				}
			}
		}
	}

	class Gum : ModProjectile
	{
		public override string Texture => AssetDirectory.Minion + Name;

		private const int MAX_TICKS = 25;

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
			Main.projFrames[Projectile.type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 22;
			Projectile.timeLeft = 300;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
			Projectile.alpha = 0;
		}

		public float color
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public float ticks
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		public override void AI()
		{
			// Loop frames
			int frameSpeed = 5;
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= frameSpeed)
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
				}
			}

			ticks++;
			if (ticks >= MAX_TICKS)
			{
				const float velYmult = 0.5f;
				ticks = MAX_TICKS;
				Projectile.velocity.Y += velYmult;
			}
		}

		public override void PostDraw(Color lightColor)
		{
			//I don't think this is the best way to do this but I wanted to try drawcode
			Texture2D tex = Request<Texture2D>(Texture).Value;
			Color[] colors = new Color[] { Color.Red, Color.Orange, Color.Yellow, Color.Blue, Color.Violet, Color.Pink };
			//spriteBatch.Draw(tex, projectile.position, tex.Frame(), colors[Main.rand.Next(colors.Length)], projectile.rotation, Vector2.Zero, projectile.scale, 0, 0);
			Main.EntitySpriteDraw(tex, (Projectile.position - Main.screenPosition) + new Vector2(0, Main.player[Projectile.owner].gfxOffY), new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), colors[(int)color], Projectile.rotation, Vector2.Zero, Projectile.scale, 0, 0);
			base.PostDraw(lightColor);
		}
	}
}