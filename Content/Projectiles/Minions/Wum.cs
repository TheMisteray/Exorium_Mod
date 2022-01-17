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
            DisplayName.SetDefault("Wum");
            // Sets the amount of frames this minion has on its spritesheet
            Main.projFrames[projectile.type] = 18;
            // this is necessary for right-click targeting
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;

            // These below are needed for a minion
            // Denotes that this projectile is a pet or minion
            Main.projPet[projectile.type] = true;
            // this is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            // Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public sealed override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 26;
            projectile.height = 44;
            // Makes the minion go through tiles freely
            projectile.tileCollide = true;

            // These below are needed for a minion weapon
            // Only controls if it deals damage to enemies on contact (more on that later)
            projectile.friendly = true;
            // Only determines the damage type
            projectile.minion = true;
            // Amount of slots projectile minion occupies from the total minion slots available to the player (more on that later)
            projectile.minionSlots = 1f;
            // Needed so the minion doesn't despawn on collision with enemies or tiles
            projectile.penetrate = -1;
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
			Player player = Main.player[projectile.owner];
			if (player.dead || !player.active)
			{
				player.ClearBuff(BuffType<Buffs.Minions.WumBuff>());
			}
			if (player.HasBuff(BuffType<Buffs.Minions.WumBuff>()))
			{
				projectile.timeLeft = 2;
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

			if (!Main.player[projectile.owner].active)
			{
				projectile.active = false;
			}
			else
			{
				flag = false;
				flag2 = false;
				flag6 = false;
				flag7 = false;
				int num113 = 85;
				flag9 = (true);
				if (projectile.type == 324)
				{
					num113 = 120;
				}
				if (projectile.type == 112)
				{
					num113 = 100;
				}
				if (projectile.type == 127)
				{
					num113 = 50;
				}
				if (flag9)
				{
					if (projectile.lavaWet)
					{
						projectile.ai[0] = 1f;
						projectile.ai[1] = 0f;
					}
					num113 = 60 + 30 * projectile.minionPos;
				}
				else if (projectile.type == 266)
				{
					num113 = 60 + 30 * projectile.minionPos;
				}
				if (projectile.type == 111)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].bunny = false;
					}
					if (Main.player[projectile.owner].bunny)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 112)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].penguin = false;
					}
					if (Main.player[projectile.owner].penguin)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 334)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].puppy = false;
					}
					if (Main.player[projectile.owner].puppy)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 353)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].grinch = false;
					}
					if (Main.player[projectile.owner].grinch)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 127)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].turtle = false;
					}
					if (Main.player[projectile.owner].turtle)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 175)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].eater = false;
					}
					if (Main.player[projectile.owner].eater)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 197)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].skeletron = false;
					}
					if (Main.player[projectile.owner].skeletron)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 198)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].hornet = false;
					}
					if (Main.player[projectile.owner].hornet)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 199)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].tiki = false;
					}
					if (Main.player[projectile.owner].tiki)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 200)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].lizard = false;
					}
					if (Main.player[projectile.owner].lizard)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 208)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].parrot = false;
					}
					if (Main.player[projectile.owner].parrot)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 209)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].truffle = false;
					}
					if (Main.player[projectile.owner].truffle)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 210)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].sapling = false;
					}
					if (Main.player[projectile.owner].sapling)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 324)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].cSapling = false;
					}
					if (Main.player[projectile.owner].cSapling)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 313)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].spider = false;
					}
					if (Main.player[projectile.owner].spider)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 314)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].squashling = false;
					}
					if (Main.player[projectile.owner].squashling)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 211)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].wisp = false;
					}
					if (Main.player[projectile.owner].wisp)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 236)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].dino = false;
					}
					if (Main.player[projectile.owner].dino)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 499)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].babyFaceMonster = false;
					}
					if (Main.player[projectile.owner].babyFaceMonster)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 266)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].slime = false;
					}
					if (Main.player[projectile.owner].slime)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 268)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].eyeSpring = false;
					}
					if (Main.player[projectile.owner].eyeSpring)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 269)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].snowman = false;
					}
					if (Main.player[projectile.owner].snowman)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 319)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].blackCat = false;
					}
					if (Main.player[projectile.owner].blackCat)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 380)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].zephyrfish = false;
					}
					if (Main.player[projectile.owner].zephyrfish)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type >= 390 && projectile.type <= 392)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].spiderMinion = false;
					}
					if (Main.player[projectile.owner].spiderMinion)
					{
						projectile.timeLeft = 2;
					}
				}
				if (projectile.type == 398)
				{
					if (Main.player[projectile.owner].dead)
					{
						Main.player[projectile.owner].miniMinotaur = false;
					}
					if (Main.player[projectile.owner].miniMinotaur)
					{
						projectile.timeLeft = 2;
					}
				}
				if (flag9 || projectile.type == 266 || (projectile.type >= 390 && projectile.type <= 392))
				{
					num113 = 10;
					int num170 = 40 * (projectile.minionPos + 1) * Main.player[projectile.owner].direction;
					if (Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) < projectile.position.X + (float)(projectile.width / 2) - (float)num113 + (float)num170)
					{
						flag = true;
					}
					else if (Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) > projectile.position.X + (float)(projectile.width / 2) + (float)num113 + (float)num170)
					{
						flag2 = true;
					}
				}
				else if (Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) < projectile.position.X + (float)(projectile.width / 2) - (float)num113)
				{
					flag = true;
				}
				else if (Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) > projectile.position.X + (float)(projectile.width / 2) + (float)num113)
				{
					flag2 = true;
				}
				if (projectile.type == 175)
				{
					float num169 = 0.1f;
					projectile.tileCollide = false;
					int num168 = 300;
					Vector2 vector15 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
					float num167 = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - vector15.X;
					float num166 = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - vector15.Y;
					if (projectile.type == 127)
					{
						num166 = Main.player[projectile.owner].position.Y - vector15.Y;
					}
					float num165 = (float)Math.Sqrt((double)(num167 * num167 + num166 * num166));
					float num164 = 7f;
					if (num165 < (float)num168 && Main.player[projectile.owner].velocity.Y == 0f && projectile.position.Y + (float)projectile.height <= Main.player[projectile.owner].position.Y + (float)Main.player[projectile.owner].height && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
					{
						projectile.ai[0] = 0f;
						if (projectile.velocity.Y < -6f)
						{
							projectile.velocity.Y = -6f;
						}
					}
					if (num165 < 150f)
					{
						if (Math.Abs(projectile.velocity.X) > 2f || Math.Abs(projectile.velocity.Y) > 2f)
						{
							projectile.velocity *= 0.99f;
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
						if (projectile.velocity.X < num167)
						{
							projectile.velocity.X = projectile.velocity.X + num169;
							if (num169 > 0.05f && projectile.velocity.X < 0f)
							{
								projectile.velocity.X = projectile.velocity.X + num169;
							}
						}
						if (projectile.velocity.X > num167)
						{
							projectile.velocity.X = projectile.velocity.X - num169;
							if (num169 > 0.05f && projectile.velocity.X > 0f)
							{
								projectile.velocity.X = projectile.velocity.X - num169;
							}
						}
					}
					if (Math.Abs(num167) <= Math.Abs(num166) || num169 == 0.05f)
					{
						if (projectile.velocity.Y < num166)
						{
							projectile.velocity.Y = projectile.velocity.Y + num169;
							if (num169 > 0.05f && projectile.velocity.Y < 0f)
							{
								projectile.velocity.Y = projectile.velocity.Y + num169;
							}
						}
						if (projectile.velocity.Y > num166)
						{
							projectile.velocity.Y = projectile.velocity.Y - num169;
							if (num169 > 0.05f && projectile.velocity.Y > 0f)
							{
								projectile.velocity.Y = projectile.velocity.Y - num169;
							}
						}
					}
					projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) - 1.57f;
					projectile.frameCounter++;
					if (projectile.frameCounter > 6)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame > 1)
					{
						projectile.frame = 0;
					}
				}
				else if (projectile.type == 197)
				{
					float num162 = 0.1f;
					projectile.tileCollide = false;
					int num161 = 300;
					Vector2 vector14 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
					float num160 = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - vector14.X;
					float num159 = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - vector14.Y;
					if (projectile.type == 127)
					{
						num159 = Main.player[projectile.owner].position.Y - vector14.Y;
					}
					float num158 = (float)Math.Sqrt((double)(num160 * num160 + num159 * num159));
					float num157 = 3f;
					if (num158 > 500f)
					{
						projectile.localAI[0] = 10000f;
					}
					if (projectile.localAI[0] >= 10000f)
					{
						num157 = 14f;
					}
					if (num158 < (float)num161 && Main.player[projectile.owner].velocity.Y == 0f && projectile.position.Y + (float)projectile.height <= Main.player[projectile.owner].position.Y + (float)Main.player[projectile.owner].height && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
					{
						projectile.ai[0] = 0f;
						if (projectile.velocity.Y < -6f)
						{
							projectile.velocity.Y = -6f;
						}
					}
					if (num158 < 150f)
					{
						if (Math.Abs(projectile.velocity.X) > 2f || Math.Abs(projectile.velocity.Y) > 2f)
						{
							projectile.velocity *= 0.99f;
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
					if (projectile.velocity.X < num160)
					{
						projectile.velocity.X = projectile.velocity.X + num162;
						if (num162 > 0.05f && projectile.velocity.X < 0f)
						{
							projectile.velocity.X = projectile.velocity.X + num162;
						}
					}
					if (projectile.velocity.X > num160)
					{
						projectile.velocity.X = projectile.velocity.X - num162;
						if (num162 > 0.05f && projectile.velocity.X > 0f)
						{
							projectile.velocity.X = projectile.velocity.X - num162;
						}
					}
					if (projectile.velocity.Y < num159)
					{
						projectile.velocity.Y = projectile.velocity.Y + num162;
						if (num162 > 0.05f && projectile.velocity.Y < 0f)
						{
							projectile.velocity.Y = projectile.velocity.Y + num162;
						}
					}
					if (projectile.velocity.Y > num159)
					{
						projectile.velocity.Y = projectile.velocity.Y - num162;
						if (num162 > 0.05f && projectile.velocity.Y > 0f)
						{
							projectile.velocity.Y = projectile.velocity.Y - num162;
						}
					}
					projectile.localAI[0] += (float)Main.rand.Next(10);
					if (projectile.localAI[0] > 10000f)
					{
						if (projectile.localAI[1] == 0f)
						{
							if (projectile.velocity.X < 0f)
							{
								projectile.localAI[1] = -1f;
							}
							else
							{
								projectile.localAI[1] = 1f;
							}
						}
						projectile.rotation += 0.25f * projectile.localAI[1];
						if (projectile.localAI[0] > 12000f)
						{
							projectile.localAI[0] = 0f;
						}
					}
					else
					{
						projectile.localAI[1] = 0f;
						float num155 = projectile.velocity.X * 0.1f;
						if (projectile.rotation > num155)
						{
							projectile.rotation -= (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.01f;
							if (projectile.rotation < num155)
							{
								projectile.rotation = num155;
							}
						}
						if (projectile.rotation < num155)
						{
							projectile.rotation += (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.01f;
							if (projectile.rotation > num155)
							{
								projectile.rotation = num155;
							}
						}
					}
					if ((double)projectile.rotation > 6.28)
					{
						projectile.rotation -= 6.28f;
					}
					if ((double)projectile.rotation < -6.28)
					{
						projectile.rotation += 6.28f;
					}
				}
				else
				{
					if (projectile.type != 198 && projectile.type != 380)
					{
						if (projectile.type == 211)
						{
							float num154 = 0.2f;
							float num153 = 5f;
							projectile.tileCollide = false;
							Vector2 vector13 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
							float num152 = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - vector13.X;
							float num151 = Main.player[projectile.owner].position.Y + Main.player[projectile.owner].gfxOffY + (float)(Main.player[projectile.owner].height / 2) - vector13.Y;
							if (Main.player[projectile.owner].controlLeft)
							{
								num152 -= 120f;
							}
							else if (Main.player[projectile.owner].controlRight)
							{
								num152 += 120f;
							}
							if (Main.player[projectile.owner].controlDown)
							{
								num151 += 120f;
							}
							else
							{
								if (Main.player[projectile.owner].controlUp)
								{
									num151 -= 120f;
								}
								num151 -= 60f;
							}
							float num149 = (float)Math.Sqrt((double)(num152 * num152 + num151 * num151));
							if (num149 > 1000f)
							{
								projectile.position.X = projectile.position.X + num152;
								projectile.position.Y = projectile.position.Y + num151;
							}
							if (projectile.localAI[0] == 1f)
							{
								if (num149 < 10f && Math.Abs(Main.player[projectile.owner].velocity.X) + Math.Abs(Main.player[projectile.owner].velocity.Y) < num153 && Main.player[projectile.owner].velocity.Y == 0f)
								{
									projectile.localAI[0] = 0f;
								}
								num153 = 12f;
								if (num149 < num153)
								{
									projectile.velocity.X = num152;
									projectile.velocity.Y = num151;
								}
								else
								{
									num149 = num153 / num149;
									projectile.velocity.X = num152 * num149;
									projectile.velocity.Y = num151 * num149;
								}
								if ((double)projectile.velocity.X > 0.5)
								{
									projectile.direction = -1;
								}
								else if ((double)projectile.velocity.X < -0.5)
								{
									projectile.direction = 1;
								}
								projectile.spriteDirection = projectile.direction;
								projectile.rotation -= (0.2f + Math.Abs(projectile.velocity.X) * 0.025f) * (float)projectile.direction;
								projectile.frameCounter++;
								if (projectile.frameCounter > 3)
								{
									projectile.frame++;
									projectile.frameCounter = 0;
								}
								if (projectile.frame < 5)
								{
									projectile.frame = 5;
								}
								if (projectile.frame > 9)
								{
									projectile.frame = 5;
								}
								for (int i2 = 0; i2 < 2; i2++)
								{
									int num146 = Dust.NewDust(new Vector2(projectile.position.X + 3f, projectile.position.Y + 4f), 14, 14, 156, 0f, 0f, 0, default(Color), 1f);
									Dust obj = Main.dust[num146];
									obj.velocity *= 0.2f;
									Main.dust[num146].noGravity = true;
									Main.dust[num146].scale = 1.25f;
									Main.dust[num146].shader = GameShaders.Armor.GetSecondaryShader(Main.player[projectile.owner].cLight, Main.player[projectile.owner]);
								}
							}
							else
							{
								if (num149 > 200f)
								{
									projectile.localAI[0] = 1f;
								}
								if ((double)projectile.velocity.X > 0.5)
								{
									projectile.direction = -1;
								}
								else if ((double)projectile.velocity.X < -0.5)
								{
									projectile.direction = 1;
								}
								projectile.spriteDirection = projectile.direction;
								if (num149 < 10f)
								{
									projectile.velocity.X = num152;
									projectile.velocity.Y = num151;
									projectile.rotation = projectile.velocity.X * 0.05f;
									if (num149 < num153)
									{
										projectile.position += projectile.velocity;
										projectile.velocity *= 0f;
										num154 = 0f;
									}
									projectile.direction = -Main.player[projectile.owner].direction;
								}
								num149 = num153 / num149;
								num152 *= num149;
								num151 *= num149;
								if (projectile.velocity.X < num152)
								{
									projectile.velocity.X = projectile.velocity.X + num154;
									if (projectile.velocity.X < 0f)
									{
										projectile.velocity.X = projectile.velocity.X * 0.99f;
									}
								}
								if (projectile.velocity.X > num152)
								{
									projectile.velocity.X = projectile.velocity.X - num154;
									if (projectile.velocity.X > 0f)
									{
										projectile.velocity.X = projectile.velocity.X * 0.99f;
									}
								}
								if (projectile.velocity.Y < num151)
								{
									projectile.velocity.Y = projectile.velocity.Y + num154;
									if (projectile.velocity.Y < 0f)
									{
										projectile.velocity.Y = projectile.velocity.Y * 0.99f;
									}
								}
								if (projectile.velocity.Y > num151)
								{
									projectile.velocity.Y = projectile.velocity.Y - num154;
									if (projectile.velocity.Y > 0f)
									{
										projectile.velocity.Y = projectile.velocity.Y * 0.99f;
									}
								}
								if (projectile.velocity.X != 0f || projectile.velocity.Y != 0f)
								{
									projectile.rotation = projectile.velocity.X * 0.05f;
								}
								projectile.frameCounter++;
								if (projectile.frameCounter > 3)
								{
									projectile.frame++;
									projectile.frameCounter = 0;
								}
								if (projectile.frame > 4)
								{
									projectile.frame = 0;
								}
							}
							return;
						}
						if (projectile.type == 199)
						{
							float num142 = 0.1f;
							projectile.tileCollide = false;
							int num141 = 200;
							Vector2 vector11 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
							float num140 = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - vector11.X;
							float num139 = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - vector11.Y;
							num139 -= 60f;
							num140 -= 2f;
							if (projectile.type == 127)
							{
								num139 = Main.player[projectile.owner].position.Y - vector11.Y;
							}
							float num136 = (float)Math.Sqrt((double)(num140 * num140 + num139 * num139));
							float num135 = 4f;
							float num172 = num136;
							if (num136 < (float)num141 && Main.player[projectile.owner].velocity.Y == 0f && projectile.position.Y + (float)projectile.height <= Main.player[projectile.owner].position.Y + (float)Main.player[projectile.owner].height && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
							{
								projectile.ai[0] = 0f;
								if (projectile.velocity.Y < -6f)
								{
									projectile.velocity.Y = -6f;
								}
							}
							if (num136 < 4f)
							{
								projectile.velocity.X = num140;
								projectile.velocity.Y = num139;
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
							if (projectile.velocity.X < num140)
							{
								projectile.velocity.X = projectile.velocity.X + num142;
								if (projectile.velocity.X < 0f)
								{
									projectile.velocity.X = projectile.velocity.X + num142;
								}
							}
							if (projectile.velocity.X > num140)
							{
								projectile.velocity.X = projectile.velocity.X - num142;
								if (projectile.velocity.X > 0f)
								{
									projectile.velocity.X = projectile.velocity.X - num142;
								}
							}
							if (projectile.velocity.Y < num139)
							{
								projectile.velocity.Y = projectile.velocity.Y + num142;
								if (projectile.velocity.Y < 0f)
								{
									projectile.velocity.Y = projectile.velocity.Y + num142;
								}
							}
							if (projectile.velocity.Y > num139)
							{
								projectile.velocity.Y = projectile.velocity.Y - num142;
								if (projectile.velocity.Y > 0f)
								{
									projectile.velocity.Y = projectile.velocity.Y - num142;
								}
							}
							projectile.direction = -Main.player[projectile.owner].direction;
							projectile.spriteDirection = 1;
							projectile.rotation = projectile.velocity.Y * 0.05f * (0f - (float)projectile.direction);
							if (num172 >= 50f)
							{
								projectile.frameCounter++;
								if (projectile.frameCounter > 6)
								{
									projectile.frameCounter = 0;
									if (projectile.velocity.X < 0f)
									{
										if (projectile.frame < 2)
										{
											projectile.frame++;
										}
										if (projectile.frame > 2)
										{
											projectile.frame--;
										}
									}
									else
									{
										if (projectile.frame < 6)
										{
											projectile.frame++;
										}
										if (projectile.frame > 6)
										{
											projectile.frame--;
										}
									}
								}
							}
							else
							{
								projectile.frameCounter++;
								if (projectile.frameCounter > 6)
								{
									projectile.frame += projectile.direction;
									projectile.frameCounter = 0;
								}
								if (projectile.frame > 7)
								{
									projectile.frame = 0;
								}
								if (projectile.frame < 0)
								{
									projectile.frame = 7;
								}
							}
							return;
						}
						if (projectile.ai[1] == 0f)
						{
							int num133 = 500;
							if (projectile.type == 127)
							{
								num133 = 200;
							}
							if (projectile.type == 208)
							{
								num133 = 300;
							}
							if (flag9 || projectile.type == 266 || (projectile.type >= 390 && projectile.type <= 392))
							{
								num133 += 40 * projectile.minionPos;
								if (projectile.localAI[0] > 0f)
								{
									num133 += 500;
								}
								if (projectile.type == 266 && projectile.localAI[0] > 0f)
								{
									num133 += 100;
								}
								if (projectile.type >= 390 && projectile.type <= 392 && projectile.localAI[0] > 0f)
								{
									num133 += 400;
								}
							}
							if (Main.player[projectile.owner].rocketDelay2 > 0)
							{
								projectile.ai[0] = 1f;
							}
							Vector2 vector10 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
							float num173 = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - vector10.X;
							num132 = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - vector10.Y;
							float num131 = (float)Math.Sqrt((double)(num173 * num173 + num132 * num132));
							if (num131 > 2000f)
							{
								projectile.position.X = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - (float)(projectile.width / 2);
								projectile.position.Y = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - (float)(projectile.height / 2);
								goto IL_289c;
							}
							if (!(num131 > (float)num133))
							{
								if (Math.Abs(num132) > 300f)
								{
									if (!flag9 && projectile.type != 266 && (projectile.type < 390 || projectile.type > 392))
									{
										goto IL_282c;
									}
									if (projectile.localAI[0] <= 0f)
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
					if (projectile.type == 380)
					{
						num33 = 0.3f;
					}
					projectile.tileCollide = false;
					int num32 = 100;
					Vector2 vector3 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
					float num31 = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - vector3.X;
					float num30 = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - vector3.Y;
					num30 += (float)Main.rand.Next(-10, 21);
					num31 += (float)Main.rand.Next(-10, 21);
					num31 += 60f * (0f - (float)Main.player[projectile.owner].direction);
					num30 -= 60f;
					if (projectile.type == 127)
					{
						num30 = Main.player[projectile.owner].position.Y - vector3.Y;
					}
					float num25 = (float)Math.Sqrt((double)(num31 * num31 + num30 * num30));
					float num24 = 14f;
					if (projectile.type == 380)
					{
						num24 = 6f;
					}
					if (num25 < (float)num32 && Main.player[projectile.owner].velocity.Y == 0f && projectile.position.Y + (float)projectile.height <= Main.player[projectile.owner].position.Y + (float)Main.player[projectile.owner].height && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
					{
						projectile.ai[0] = 0f;
						if (projectile.velocity.Y < -6f)
						{
							projectile.velocity.Y = -6f;
						}
					}
					if (num25 < 50f)
					{
						if (Math.Abs(projectile.velocity.X) > 2f || Math.Abs(projectile.velocity.Y) > 2f)
						{
							projectile.velocity *= 0.99f;
						}
						num33 = 0.01f;
					}
					else
					{
						if (projectile.type == 380)
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
						else if (projectile.type == 198)
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
					if (projectile.velocity.X < num31)
					{
						projectile.velocity.X = projectile.velocity.X + num33;
						if (num33 > 0.05f && projectile.velocity.X < 0f)
						{
							projectile.velocity.X = projectile.velocity.X + num33;
						}
					}
					if (projectile.velocity.X > num31)
					{
						projectile.velocity.X = projectile.velocity.X - num33;
						if (num33 > 0.05f && projectile.velocity.X > 0f)
						{
							projectile.velocity.X = projectile.velocity.X - num33;
						}
					}
					if (projectile.velocity.Y < num30)
					{
						projectile.velocity.Y = projectile.velocity.Y + num33;
						if (num33 > 0.05f && projectile.velocity.Y < 0f)
						{
							projectile.velocity.Y = projectile.velocity.Y + num33 * 2f;
						}
					}
					if (projectile.velocity.Y > num30)
					{
						projectile.velocity.Y = projectile.velocity.Y - num33;
						if (num33 > 0.05f && projectile.velocity.Y > 0f)
						{
							projectile.velocity.Y = projectile.velocity.Y - num33 * 2f;
						}
					}
					if ((double)projectile.velocity.X > 0.25)
					{
						projectile.direction = -1;
					}
					else if ((double)projectile.velocity.X < -0.25)
					{
						projectile.direction = 1;
					}
					projectile.spriteDirection = projectile.direction;
					projectile.rotation = projectile.velocity.X * 0.05f;
					projectile.frameCounter++;
					int num22 = 2;
					if (projectile.type == 380)
					{
						num22 = 5;
					}
					if (projectile.frameCounter > num22)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame > 3)
					{
						projectile.frame = 0;
					}
				}
			}
			return;
		IL_282c:
			if (projectile.type != 324)
			{
				if (num132 > 0f && projectile.velocity.Y < 0f)
				{
					projectile.velocity.Y = 0f;
				}
				if (num132 < 0f && projectile.velocity.Y > 0f)
				{
					projectile.velocity.Y = 0f;
				}
			}
			projectile.ai[0] = 1f;
			goto IL_289c;
		IL_289c:
			if (projectile.type == 209 && projectile.ai[0] != 0f)
			{
				if (Main.player[projectile.owner].velocity.Y == 0f && projectile.alpha >= 100)
				{
					projectile.position.X = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - (float)(projectile.width / 2);
					projectile.position.Y = Main.player[projectile.owner].position.Y + (float)Main.player[projectile.owner].height - (float)projectile.height;
					projectile.ai[0] = 0f;
				}
				else
				{
					projectile.velocity.X = 0f;
					projectile.velocity.Y = 0f;
					projectile.alpha += 5;
					if (projectile.alpha > 255)
					{
						projectile.alpha = 255;
					}
				}
			}
			else if (projectile.ai[0] != 0f)
			{
				float num130 = 0.2f;
				int num129 = 200;
				if (projectile.type == 127)
				{
					num129 = 100;
				}
				if (flag9)
				{
					num130 = 0.5f;
					num129 = 100;
				}
				projectile.tileCollide = false;
				Vector2 vector9 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num128 = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - vector9.X;
				if (flag9 || projectile.type == 266 || (projectile.type >= 390 && projectile.type <= 392))
				{
					num128 -= (float)(40 * Main.player[projectile.owner].direction);
					float num127 = 700f;
					if (flag9)
					{
						num127 += 100f;
					}
					bool flag8 = false;
					int num126 = -1;
					for (int n = 0; n < 200; n++)
					{
						if (Main.npc[n].CanBeChasedBy(projectile, false))
						{
							float num125 = Main.npc[n].position.X + (float)(Main.npc[n].width / 2);
							float num124 = Main.npc[n].position.Y + (float)(Main.npc[n].height / 2);
							if (Math.Abs(Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - num125) + Math.Abs(Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - num124) < num127)
							{
								if (Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[n].position, Main.npc[n].width, Main.npc[n].height))
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
						num128 -= (float)(40 * projectile.minionPos * Main.player[projectile.owner].direction);
					}
					if (flag8 && num126 >= 0)
					{
						projectile.ai[0] = 0f;
					}
				}
				float num123 = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - vector9.Y;
				if (projectile.type == 127)
				{
					num123 = Main.player[projectile.owner].position.Y - vector9.Y;
				}
				float num122 = (float)Math.Sqrt((double)(num128 * num128 + num123 * num123));
				float num121 = 10f;
				float num120 = num122;
				if (projectile.type == 111)
				{
					num121 = 11f;
				}
				if (projectile.type == 127)
				{
					num121 = 9f;
				}
				if (projectile.type == 324)
				{
					num121 = 20f;
				}
				if (flag9)
				{
					num130 = 0.4f;
					num121 = 12f;
					if (num121 < Math.Abs(Main.player[projectile.owner].velocity.X) + Math.Abs(Main.player[projectile.owner].velocity.Y))
					{
						num121 = Math.Abs(Main.player[projectile.owner].velocity.X) + Math.Abs(Main.player[projectile.owner].velocity.Y);
					}
				}
				if (projectile.type == 208 && Math.Abs(Main.player[projectile.owner].velocity.X) + Math.Abs(Main.player[projectile.owner].velocity.Y) > 4f)
				{
					num129 = -1;
				}
				if (num122 < (float)num129 && Main.player[projectile.owner].velocity.Y == 0f && projectile.position.Y + (float)projectile.height <= Main.player[projectile.owner].position.Y + (float)Main.player[projectile.owner].height && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
				{
					projectile.ai[0] = 0f;
					if (projectile.velocity.Y < -6f)
					{
						projectile.velocity.Y = -6f;
					}
				}
				if (num122 < 60f)
				{
					num128 = projectile.velocity.X;
					num123 = projectile.velocity.Y;
				}
				else
				{
					num122 = num121 / num122;
					num128 *= num122;
					num123 *= num122;
				}
				if (projectile.type == 324)
				{
					if (num120 > 1000f)
					{
						if ((double)(Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) < (double)num121 - 1.25)
						{
							projectile.velocity *= 1.025f;
						}
						if ((double)(Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) > (double)num121 + 1.25)
						{
							projectile.velocity *= 0.975f;
						}
					}
					else if (num120 > 600f)
					{
						if (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y) < num121 - 1f)
						{
							projectile.velocity *= 1.05f;
						}
						if (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y) > num121 + 1f)
						{
							projectile.velocity *= 0.95f;
						}
					}
					else if (num120 > 400f)
					{
						if ((double)(Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) < (double)num121 - 0.5)
						{
							projectile.velocity *= 1.075f;
						}
						if ((double)(Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) > (double)num121 + 0.5)
						{
							projectile.velocity *= 0.925f;
						}
					}
					else
					{
						if ((double)(Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) < (double)num121 - 0.25)
						{
							projectile.velocity *= 1.1f;
						}
						if ((double)(Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) > (double)num121 + 0.25)
						{
							projectile.velocity *= 0.9f;
						}
					}
					projectile.velocity.X = (projectile.velocity.X * 34f + num128) / 35f;
					projectile.velocity.Y = (projectile.velocity.Y * 34f + num123) / 35f;
				}
				else
				{
					if (projectile.velocity.X < num128)
					{
						projectile.velocity.X = projectile.velocity.X + num130;
						if (projectile.velocity.X < 0f)
						{
							projectile.velocity.X = projectile.velocity.X + num130 * 1.5f;
						}
					}
					if (projectile.velocity.X > num128)
					{
						projectile.velocity.X = projectile.velocity.X - num130;
						if (projectile.velocity.X > 0f)
						{
							projectile.velocity.X = projectile.velocity.X - num130 * 1.5f;
						}
					}
					if (projectile.velocity.Y < num123)
					{
						projectile.velocity.Y = projectile.velocity.Y + num130;
						if (projectile.velocity.Y < 0f)
						{
							projectile.velocity.Y = projectile.velocity.Y + num130 * 1.5f;
						}
					}
					if (projectile.velocity.Y > num123)
					{
						projectile.velocity.Y = projectile.velocity.Y - num130;
						if (projectile.velocity.Y > 0f)
						{
							projectile.velocity.Y = projectile.velocity.Y - num130 * 1.5f;
						}
					}
				}
				if (projectile.type == 111)
				{
					projectile.frame = 7;
				}
				if (projectile.type == 112)
				{
					projectile.frame = 2;
				}
				if (flag9 && projectile.frame < 12)
				{
					projectile.frame = Main.rand.Next(12, 18);
					projectile.frameCounter = 0;
				}
				if (projectile.type != 313)
				{
					if ((double)projectile.velocity.X > 0.5)
					{
						projectile.spriteDirection = -1;
					}
					else if ((double)projectile.velocity.X < -0.5)
					{
						projectile.spriteDirection = 1;
					}
				}
				if (projectile.type == 398)
				{
					if ((double)projectile.velocity.X > 0.5)
					{
						projectile.spriteDirection = 1;
					}
					else if ((double)projectile.velocity.X < -0.5)
					{
						projectile.spriteDirection = -1;
					}
				}
				if (projectile.type == 112)
				{
					if (projectile.spriteDirection == -1)
					{
						projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
					}
					else
					{
						projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
					}
				}
				else if (projectile.type >= 390 && projectile.type <= 392)
				{
					int num116 = (int)(projectile.Center.X / 16f);
					int num115 = (int)(projectile.Center.Y / 16f);
					if (Main.tile[num116, num115] != null && Main.tile[num116, num115].wall > 0)
					{
						projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;
						projectile.frameCounter += (int)(Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y));
						if (projectile.frameCounter > 5)
						{
							projectile.frame++;
							projectile.frameCounter = 0;
						}
						if (projectile.frame > 7)
						{
							projectile.frame = 4;
						}
						if (projectile.frame < 4)
						{
							projectile.frame = 7;
						}
					}
					else
					{
						projectile.frameCounter++;
						if (projectile.frameCounter > 2)
						{
							projectile.frame++;
							projectile.frameCounter = 0;
						}
						if (projectile.frame < 8 || projectile.frame > 10)
						{
							projectile.frame = 8;
						}
						projectile.rotation = projectile.velocity.X * 0.1f;
					}
				}
				else if (projectile.type == 334)
				{
					projectile.frameCounter++;
					if (projectile.frameCounter > 1)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame < 7 || projectile.frame > 10)
					{
						projectile.frame = 7;
					}
					projectile.rotation = projectile.velocity.X * 0.1f;
				}
				else if (projectile.type == 353)
				{
					projectile.frameCounter++;
					if (projectile.frameCounter > 6)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame < 10 || projectile.frame > 13)
					{
						projectile.frame = 10;
					}
					projectile.rotation = projectile.velocity.X * 0.05f;
				}
				else if (projectile.type == 127)
				{
					projectile.frameCounter += 3;
					if (projectile.frameCounter > 6)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame <= 5 || projectile.frame > 15)
					{
						projectile.frame = 6;
					}
					projectile.rotation = projectile.velocity.X * 0.1f;
				}
				else if (projectile.type == 269)
				{
					if (projectile.frame == 6)
					{
						projectile.frameCounter = 0;
					}
					else if (projectile.frame < 4 || projectile.frame > 6)
					{
						projectile.frameCounter = 0;
						projectile.frame = 4;
					}
					else
					{
						projectile.frameCounter++;
						if (projectile.frameCounter > 6)
						{
							projectile.frame++;
							projectile.frameCounter = 0;
						}
					}
					projectile.rotation = projectile.velocity.X * 0.05f;
				}
				else if (projectile.type == 266)
				{
					projectile.frameCounter++;
					if (projectile.frameCounter > 6)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame < 2 || projectile.frame > 5)
					{
						projectile.frame = 2;
					}
					projectile.rotation = projectile.velocity.X * 0.1f;
				}
				else if (projectile.type == 324)
				{
					projectile.frameCounter++;
					if (projectile.frameCounter > 1)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame < 6 || projectile.frame > 9)
					{
						projectile.frame = 6;
					}
					projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.58f;
					Lighting.AddLight((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16, 0.9f, 0.6f, 0.2f);
					for (int m = 0; m < 2; m++)
					{
						int num114 = 4;
						int num112 = Dust.NewDust(new Vector2(projectile.Center.X - (float)num114, projectile.Center.Y - (float)num114) - projectile.velocity * 0f, num114 * 2, num114 * 2, 6, 0f, 0f, 100, default(Color), 1f);
						Main.dust[num112].scale *= 1.8f + (float)Main.rand.Next(10) * 0.1f;
						Dust obj2 = Main.dust[num112];
						obj2.velocity *= 0.2f;
						if (m == 1)
						{
							Dust obj3 = Main.dust[num112];
							obj3.position -= projectile.velocity * 0.5f;
						}
						Main.dust[num112].noGravity = true;
						num112 = Dust.NewDust(new Vector2(projectile.Center.X - (float)num114, projectile.Center.Y - (float)num114) - projectile.velocity * 0f, num114 * 2, num114 * 2, 31, 0f, 0f, 100, default(Color), 0.5f);
						Main.dust[num112].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
						Dust obj4 = Main.dust[num112];
						obj4.velocity *= 0.05f;
						if (m == 1)
						{
							Dust obj5 = Main.dust[num112];
							obj5.position -= projectile.velocity * 0.5f;
						}
					}
				}
				else if (projectile.type == 268)
				{
					projectile.frameCounter++;
					if (projectile.frameCounter > 4)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame < 6 || projectile.frame > 7)
					{
						projectile.frame = 6;
					}
					projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.58f;
				}
				else if (projectile.type == 200)
				{
					projectile.frameCounter += 3;
					if (projectile.frameCounter > 6)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame <= 5 || projectile.frame > 9)
					{
						projectile.frame = 6;
					}
					projectile.rotation = projectile.velocity.X * 0.1f;
				}
				else if (projectile.type == 208)
				{
					projectile.rotation = projectile.velocity.X * 0.075f;
					projectile.frameCounter++;
					if (projectile.frameCounter > 6)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame > 4)
					{
						projectile.frame = 1;
					}
					if (projectile.frame < 1)
					{
						projectile.frame = 1;
					}
				}
				else if (projectile.type == 236)
				{
					projectile.rotation = projectile.velocity.Y * 0.05f * (float)projectile.direction;
					if (projectile.velocity.Y < 0f)
					{
						projectile.frameCounter += 2;
					}
					else
					{
						projectile.frameCounter++;
					}
					if (projectile.frameCounter >= 6)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame > 12)
					{
						projectile.frame = 9;
					}
					if (projectile.frame < 9)
					{
						projectile.frame = 9;
					}
				}
				else if (projectile.type == 499)
				{
					projectile.rotation = projectile.velocity.Y * 0.05f * (float)projectile.direction;
					if (projectile.velocity.Y < 0f)
					{
						projectile.frameCounter += 2;
					}
					else
					{
						projectile.frameCounter++;
					}
					if (projectile.frameCounter >= 6)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame >= 12)
					{
						projectile.frame = 8;
					}
					if (projectile.frame < 8)
					{
						projectile.frame = 8;
					}
				}
				else if (projectile.type == 314)
				{
					projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.58f;
					projectile.frameCounter++;
					if (projectile.frameCounter >= 3)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame > 12)
					{
						projectile.frame = 7;
					}
					if (projectile.frame < 7)
					{
						projectile.frame = 7;
					}
				}
				else if (projectile.type == 319)
				{
					projectile.rotation = projectile.velocity.X * 0.05f;
					projectile.frameCounter++;
					if (projectile.frameCounter >= 6)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame > 10)
					{
						projectile.frame = 6;
					}
					if (projectile.frame < 6)
					{
						projectile.frame = 6;
					}
				}
				else if (projectile.type == 210)
				{
					projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.58f;
					projectile.frameCounter += 3;
					if (projectile.frameCounter > 6)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame > 11)
					{
						projectile.frame = 7;
					}
					if (projectile.frame < 7)
					{
						projectile.frame = 7;
					}
				}
				else if (projectile.type == 313)
				{
					projectile.position.Y = projectile.position.Y + (float)projectile.height;
					projectile.height = 54;
					projectile.position.Y = projectile.position.Y - (float)projectile.height;
					projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
					projectile.width = 54;
					projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
					projectile.rotation += projectile.velocity.X * 0.01f;
					projectile.frameCounter = 0;
					projectile.frame = 11;
				}
				else if (projectile.type == 398)
				{
					projectile.frameCounter++;
					if (projectile.frameCounter > 1)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame < 6 || projectile.frame > 9)
					{
						projectile.frame = 6;
					}
					projectile.rotation = projectile.velocity.X * 0.1f;
				}
				else if (projectile.spriteDirection == -1)
				{
					projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
				}
				else
				{
					projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 3.14f;
				}
				if (!flag9 && projectile.type != 499 && projectile.type != 398 && projectile.type != 390 && projectile.type != 391 && projectile.type != 392 && projectile.type != 127 && projectile.type != 200 && projectile.type != 208 && projectile.type != 210 && projectile.type != 236 && projectile.type != 266 && projectile.type != 268 && projectile.type != 269 && projectile.type != 313 && projectile.type != 314 && projectile.type != 319 && projectile.type != 324 && projectile.type != 334 && projectile.type != 353)
				{
					//TODO: this might cause dust, look into it
					int num110 = Dust.NewDust(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 4f, projectile.position.Y + (float)(projectile.height / 2) - 4f) - projectile.velocity, 8, 8, 16, (0f - projectile.velocity.X) * 0.5f, projectile.velocity.Y * 0.5f, 50, default(Color), 1.7f);
					Main.dust[num110].velocity.X = Main.dust[num110].velocity.X * 0.2f;
					Main.dust[num110].velocity.Y = Main.dust[num110].velocity.Y * 0.2f;
					Main.dust[num110].noGravity = true;
				}
			}
			else
			{
				if (flag9)
				{
					float num109 = (float)(40 * projectile.minionPos);
					int num108 = 30;
					int num107 = 60;
					projectile.localAI[0] -= 1f;
					if (projectile.localAI[0] < 0f)
					{
						projectile.localAI[0] = 0f;
					}
					if (projectile.ai[1] > 0f)
					{
						projectile.ai[1] -= 1f;
					}
					else
					{
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
						if (num104 < num95 + num109 && num102 == -1)
						{
							float num94 = num106 - (projectile.position.X + (float)(projectile.width / 2));
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
							projectile.localAI[0] = (float)num107;
							float num93 = num106 - (projectile.position.X + (float)(projectile.width / 2));
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
							else if (projectile.owner == Main.myPlayer)
							{
								//Shoot code original location
								//shoot = 15;

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
								int num82 = Projectile.NewProjectile(vector7.X, vector7.Y, num92, num89, ProjectileType<Gum>(), projectile.damage, projectile.knockBack, Main.myPlayer, Main.rand.Next(6), 0f);
								if (num92 < 0f)
								{
									projectile.direction = -1;
								}
								if (num92 > 0f)
								{
									projectile.direction = 1;
								}
								projectile.netUpdate = true;
							}
						}
					}
				}
				bool flag5 = false;
				Vector2 vector6 = Vector2.Zero;
				bool flag4 = false;
				if (projectile.type == 266 || (projectile.type >= 390 && projectile.type <= 392))
				{
					float num81 = (float)(40 * projectile.minionPos);
					int num80 = 60;
					projectile.localAI[0] -= 1f;
					if (projectile.localAI[0] < 0f)
					{
						projectile.localAI[0] = 0f;
					}
					if (projectile.ai[1] > 0f)
					{
						projectile.ai[1] -= 1f;
					}
					else
					{
						float num79 = projectile.position.X;
						float num78 = projectile.position.Y;
						float num77 = 100000f;
						float num76 = num77;
						int num75 = -1;
						NPC ownerMinionAttackTargetNPC2 = projectile.OwnerMinionAttackTargetNPC;
						if (ownerMinionAttackTargetNPC2 != null && ownerMinionAttackTargetNPC2.CanBeChasedBy(projectile, false))
						{
							float x = ownerMinionAttackTargetNPC2.Center.X;
							float y = ownerMinionAttackTargetNPC2.Center.Y;
							float num74 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - x) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - y);
							if (num74 < num77)
							{
								if (num75 == -1 && num74 <= num76)
								{
									num76 = num74;
									num79 = x;
									num78 = y;
								}
								if (Collision.CanHit(projectile.position, projectile.width, projectile.height, ownerMinionAttackTargetNPC2.position, ownerMinionAttackTargetNPC2.width, ownerMinionAttackTargetNPC2.height))
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
								if (Main.npc[k].CanBeChasedBy(projectile, false))
								{
									float num73 = Main.npc[k].position.X + (float)(Main.npc[k].width / 2);
									float num72 = Main.npc[k].position.Y + (float)(Main.npc[k].height / 2);
									float num71 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num73) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num72);
									if (num71 < num77)
									{
										if (num75 == -1 && num71 <= num76)
										{
											num76 = num71;
											num79 = num73;
											num78 = num72;
										}
										if (Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[k].position, Main.npc[k].width, Main.npc[k].height))
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
						if (projectile.type >= 390 && projectile.type <= 392 && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
						{
							projectile.tileCollide = true;
						}
						if (num75 == -1 && num76 < num77)
						{
							num77 = num76;
						}
						else if (num75 >= 0)
						{
							flag5 = true;
							vector6 = new Vector2(num79, num78) - projectile.Center;
							if (projectile.type >= 390 && projectile.type <= 392)
							{
								if (Main.npc[num75].position.Y > projectile.position.Y + (float)projectile.height)
								{
									int num70 = (int)(projectile.Center.X / 16f);
									int num69 = (int)((projectile.position.Y + (float)projectile.height + 1f) / 16f);
									if (Main.tile[num70, num69] != null && Main.tile[num70, num69].active() && TileID.Sets.Platforms[Main.tile[num70, num69].type])
									{
										projectile.tileCollide = false;
									}
								}
								Rectangle rectangle = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
								Rectangle value = new Rectangle((int)Main.npc[num75].position.X, (int)Main.npc[num75].position.Y, Main.npc[num75].width, Main.npc[num75].height);
								int num68 = 10;
								value.X -= num68;
								value.Y -= num68;
								value.Width += num68 * 2;
								value.Height += num68 * 2;
								if (rectangle.Intersects(value))
								{
									flag4 = true;
									Vector2 vector8 = Main.npc[num75].Center - projectile.Center;
									if (projectile.velocity.Y > 0f && vector8.Y < 0f)
									{
										projectile.velocity.Y = projectile.velocity.Y * 0.5f;
									}
									if (projectile.velocity.Y < 0f && vector8.Y > 0f)
									{
										projectile.velocity.Y = projectile.velocity.Y * 0.5f;
									}
									if (projectile.velocity.X > 0f && vector8.X < 0f)
									{
										projectile.velocity.X = projectile.velocity.X * 0.5f;
									}
									if (projectile.velocity.X < 0f && vector8.X > 0f)
									{
										projectile.velocity.X = projectile.velocity.X * 0.5f;
									}
									if (vector8.Length() > 14f)
									{
										vector8.Normalize();
										vector8 *= 14f;
									}
									projectile.rotation = (projectile.rotation * 5f + vector8.ToRotation() + 1.57079637f) / 6f;
									projectile.velocity = (projectile.velocity * 9f + vector8) / 10f;
									for (int i = 0; i < 1000; i++)
									{
										if (projectile.whoAmI != i && projectile.owner == Main.projectile[i].owner && Main.projectile[i].type >= 390 && Main.projectile[i].type <= 392 && (Main.projectile[i].Center - projectile.Center).Length() < 15f)
										{
											float num67 = 0.5f;
											if (projectile.Center.Y > Main.projectile[i].Center.Y)
											{
												Projectile expr_4D74_cp_0 = Main.projectile[i];
												expr_4D74_cp_0.velocity.Y = expr_4D74_cp_0.velocity.Y - num67;
												projectile.velocity.Y = projectile.velocity.Y + num67;
											}
											else
											{
												Projectile expr_4DA5_cp_0 = Main.projectile[i];
												expr_4DA5_cp_0.velocity.Y = expr_4DA5_cp_0.velocity.Y + num67;
												projectile.velocity.Y = projectile.velocity.Y - num67;
											}
											if (projectile.Center.X > Main.projectile[i].Center.X)
											{
												projectile.velocity.X = projectile.velocity.X + num67;
												Projectile expr_4E07_cp_0 = Main.projectile[i];
												expr_4E07_cp_0.velocity.X = expr_4E07_cp_0.velocity.X - num67;
											}
											else
											{
												projectile.velocity.X = projectile.velocity.X - num67;
												Projectile expr_4E38_cp_0 = Main.projectile[i];
												expr_4E38_cp_0.velocity.Y = expr_4E38_cp_0.velocity.Y + num67;
											}
										}
									}
								}
							}
						}
						float num66 = 300f;
						if ((double)projectile.position.Y > Main.worldSurface * 16.0)
						{
							num66 = 150f;
						}
						if (projectile.type >= 390 && projectile.type <= 392)
						{
							num66 = 500f;
							if ((double)projectile.position.Y > Main.worldSurface * 16.0)
							{
								num66 = 250f;
							}
						}
						if (num77 < num66 + num81 && num75 == -1)
						{
							float num65 = num79 - (projectile.position.X + (float)(projectile.width / 2));
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
						if (projectile.type >= 390 && projectile.type <= 392 && projectile.localAI[1] > 0f)
						{
							flag3 = true;
							projectile.localAI[1] -= 1f;
						}
						if (num75 >= 0 && num77 < 800f + num81)
						{
							projectile.friendly = true;
							projectile.localAI[0] = (float)num80;
							float num64 = num79 - (projectile.position.X + (float)(projectile.width / 2));
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
							if (num78 < projectile.Center.Y - 100f && num64 > -50f && num64 < 50f && projectile.velocity.Y == 0f)
							{
								float num63 = Math.Abs(num78 - projectile.Center.Y);
								if (num63 < 120f)
								{
									projectile.velocity.Y = -10f;
								}
								else if (num63 < 210f)
								{
									projectile.velocity.Y = -13f;
								}
								else if (num63 < 270f)
								{
									projectile.velocity.Y = -15f;
								}
								else if (num63 < 310f)
								{
									projectile.velocity.Y = -17f;
								}
								else if (num63 < 380f)
								{
									projectile.velocity.Y = -18f;
								}
							}
							if (flag3)
							{
								projectile.friendly = false;
								if (projectile.velocity.X < 0f)
								{
									flag = true;
								}
								else if (projectile.velocity.X > 0f)
								{
									flag2 = true;
								}
							}
						}
						else
						{
							projectile.friendly = false;
						}
					}
				}
				if (projectile.ai[1] != 0f)
				{
					flag = false;
					flag2 = false;
				}
				else if (flag9 && projectile.localAI[0] == 0f)
				{
					projectile.direction = Main.player[projectile.owner].direction;
				}
				else if (projectile.type >= 390 && projectile.type <= 392)
				{
					int num62 = (int)(projectile.Center.X / 16f);
					int num61 = (int)(projectile.Center.Y / 16f);
					if (Main.tile[num62, num61] != null && Main.tile[num62, num61].wall > 0)
					{
						flag2 = (flag = false);
					}
				}
				if (projectile.type == 127)
				{
					if ((double)projectile.rotation > -0.1 && (double)projectile.rotation < 0.1)
					{
						projectile.rotation = 0f;
					}
					else if (projectile.rotation < 0f)
					{
						projectile.rotation += 0.1f;
					}
					else
					{
						projectile.rotation -= 0.1f;
					}
				}
				else if (projectile.type != 313 && !flag4)
				{
					projectile.rotation = 0f;
				}
				if (projectile.type < 390 || projectile.type > 392)
				{
					projectile.tileCollide = true;
				}
				float num60 = 0.08f;
				float num59 = 6.5f;
				if (projectile.type == 127)
				{
					num59 = 2f;
					num60 = 0.04f;
				}
				if (projectile.type == 112)
				{
					num59 = 6f;
					num60 = 0.06f;
				}
				if (projectile.type == 334)
				{
					num59 = 8f;
					num60 = 0.08f;
				}
				if (projectile.type == 268)
				{
					num59 = 8f;
					num60 = 0.4f;
				}
				if (projectile.type == 324)
				{
					num60 = 0.1f;
					num59 = 3f;
				}
				if (flag9 || projectile.type == 266 || (projectile.type >= 390 && projectile.type <= 392))
				{
					num59 = 6f;
					num60 = 0.2f;
					if (num59 < Math.Abs(Main.player[projectile.owner].velocity.X) + Math.Abs(Main.player[projectile.owner].velocity.Y))
					{
						num59 = Math.Abs(Main.player[projectile.owner].velocity.X) + Math.Abs(Main.player[projectile.owner].velocity.Y);
						num60 = 0.3f;
					}
				}
				if (projectile.type >= 390 && projectile.type <= 392)
				{
					num60 *= 2f;
				}
				if (flag)
				{
					if ((double)projectile.velocity.X > -3.5)
					{
						projectile.velocity.X = projectile.velocity.X - num60;
					}
					else
					{
						projectile.velocity.X = projectile.velocity.X - num60 * 0.25f;
					}
				}
				else if (flag2)
				{
					if ((double)projectile.velocity.X < 3.5)
					{
						projectile.velocity.X = projectile.velocity.X + num60;
					}
					else
					{
						projectile.velocity.X = projectile.velocity.X + num60 * 0.25f;
					}
				}
				else
				{
					projectile.velocity.X = projectile.velocity.X * 0.9f;
					if (projectile.velocity.X >= 0f - num60 && projectile.velocity.X <= num60)
					{
						projectile.velocity.X = 0f;
					}
				}
				if (projectile.type == 208)
				{
					projectile.velocity.X = projectile.velocity.X * 0.95f;
					if ((double)projectile.velocity.X > -0.1 && (double)projectile.velocity.X < 0.1)
					{
						projectile.velocity.X = 0f;
					}
					flag = false;
					flag2 = false;
				}
				if (flag | flag2)
				{
					int num58 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
					int j4 = (int)(projectile.position.Y + (float)(projectile.height / 2)) / 16;
					if (projectile.type == 236)
					{
						num58 += projectile.direction;
					}
					if (flag)
					{
						num58--;
					}
					if (flag2)
					{
						num58++;
					}
					num58 += (int)projectile.velocity.X;
					if (WorldGen.SolidTile(num58, j4))
					{
						flag7 = true;
					}
				}
				if (Main.player[projectile.owner].position.Y + (float)Main.player[projectile.owner].height - 8f > projectile.position.Y + (float)projectile.height)
				{
					flag6 = true;
				}
				if (projectile.type == 268 && projectile.frameCounter < 10)
				{
					flag7 = false;
				}
				Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref projectile.stepSpeed, ref projectile.gfxOffY, 1, false, 0);
				if (projectile.velocity.Y == 0f || projectile.type == 200)
				{
					if (!flag6 && (projectile.velocity.X < 0f || projectile.velocity.X > 0f))
					{
						int num56 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
						int j3 = (int)(projectile.position.Y + (float)(projectile.height / 2)) / 16 + 1;
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
						int num55 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
						int num54 = (int)(projectile.position.Y + (float)projectile.height) / 16 + 1;
						if (WorldGen.SolidTile(num55, num54) || Main.tile[num55, num54].halfBrick() || Main.tile[num55, num54].slope() > 0 || projectile.type == 200)
						{
							if (projectile.type == 200)
							{
								projectile.velocity.Y = -3.1f;
							}
							else
							{
								try
								{
									num55 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
									num54 = (int)(projectile.position.Y + (float)(projectile.height / 2)) / 16;
									if (flag)
									{
										num55--;
									}
									if (flag2)
									{
										num55++;
									}
									num55 += (int)projectile.velocity.X;
									if (!WorldGen.SolidTile(num55, num54 - 1) && !WorldGen.SolidTile(num55, num54 - 2))
									{
										projectile.velocity.Y = -5.1f;
									}
									else if (!WorldGen.SolidTile(num55, num54 - 2))
									{
										projectile.velocity.Y = -7.1f;
									}
									else if (WorldGen.SolidTile(num55, num54 - 5))
									{
										projectile.velocity.Y = -11.1f;
									}
									else if (WorldGen.SolidTile(num55, num54 - 4))
									{
										projectile.velocity.Y = -10.1f;
									}
									else
									{
										projectile.velocity.Y = -9.1f;
									}
								}
								catch
								{
									projectile.velocity.Y = -9.1f;
								}
							}
							if (projectile.type == 127)
							{
								projectile.ai[0] = 1f;
							}
						}
					}
					else if (projectile.type == 266 && (flag | flag2))
					{
						projectile.velocity.Y = projectile.velocity.Y - 6f;
					}
				}
				if (projectile.velocity.X > num59)
				{
					projectile.velocity.X = num59;
				}
				if (projectile.velocity.X < 0f - num59)
				{
					projectile.velocity.X = 0f - num59;
				}
				if (projectile.velocity.X < 0f)
				{
					projectile.direction = -1;
				}
				if (projectile.velocity.X > 0f)
				{
					projectile.direction = 1;
				}
				if (projectile.velocity.X > num60 & flag2)
				{
					projectile.direction = 1;
				}
				if (projectile.velocity.X < 0f - num60 & flag)
				{
					projectile.direction = -1;
				}
				if (projectile.type != 313)
				{
					if (projectile.direction == -1)
					{
						projectile.spriteDirection = 1;
					}
					if (projectile.direction == 1)
					{
						projectile.spriteDirection = -1;
					}
				}
				if (projectile.type == 398)
				{
					projectile.spriteDirection = projectile.direction;
				}
				if (flag9)
				{
					if (projectile.ai[1] > 0f)
					{
						if (projectile.localAI[1] == 0f)
						{
							projectile.localAI[1] = 1f;
							projectile.frame = 1;
						}
						if (projectile.frame != 0)
						{
							projectile.frameCounter++;
							if (projectile.frameCounter > 4)
							{
								projectile.frame++;
								projectile.frameCounter = 0;
							}
							if (projectile.frame == 4)
							{
								projectile.frame = 0;
							}
						}
					}
					else if (projectile.velocity.Y == 0f)
					{
						projectile.localAI[1] = 0f;
						if (projectile.velocity.X == 0f)
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
						else if ((double)projectile.velocity.X < -0.8 || (double)projectile.velocity.X > 0.8)
						{
							projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
							projectile.frameCounter++;
							if (projectile.frameCounter > 6)
							{
								projectile.frame++;
								projectile.frameCounter = 0;
							}
							if (projectile.frame < 5)
							{
								projectile.frame = 5;
							}
							if (projectile.frame >= 11)
							{
								projectile.frame = 5;
							}
						}
						else
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
					}
					else if (projectile.velocity.Y < 0f)
					{
						projectile.frameCounter = 0;
						projectile.frame = 4;
					}
					else if (projectile.velocity.Y > 0f)
					{
						projectile.frameCounter = 0;
						projectile.frame = 4;
					}
					projectile.velocity.Y = projectile.velocity.Y + 0.4f;
					if (projectile.velocity.Y > 10f)
					{
						projectile.velocity.Y = 10f;
					}
					Vector2 velocity = projectile.velocity;
				}
				else if (projectile.type == 268)
				{
					if (projectile.velocity.Y == 0f)
					{
						if (projectile.frame > 5)
						{
							projectile.frameCounter = 0;
						}
						if (projectile.velocity.X == 0f)
						{
							int num50 = 3;
							projectile.frameCounter++;
							if (projectile.frameCounter < num50)
							{
								projectile.frame = 0;
							}
							else if (projectile.frameCounter < num50 * 2)
							{
								projectile.frame = 1;
							}
							else if (projectile.frameCounter < num50 * 3)
							{
								projectile.frame = 2;
							}
							else if (projectile.frameCounter < num50 * 4)
							{
								projectile.frame = 3;
							}
							else
							{
								projectile.frameCounter = num50 * 4;
							}
						}
						else
						{
							projectile.velocity.X = projectile.velocity.X * 0.8f;
							projectile.frameCounter++;
							int num49 = 3;
							if (projectile.frameCounter < num49)
							{
								projectile.frame = 0;
							}
							else if (projectile.frameCounter < num49 * 2)
							{
								projectile.frame = 1;
							}
							else if (projectile.frameCounter < num49 * 3)
							{
								projectile.frame = 2;
							}
							else if (projectile.frameCounter < num49 * 4)
							{
								projectile.frame = 3;
							}
							else if (flag | flag2)
							{
								projectile.velocity.X = projectile.velocity.X * 2f;
								projectile.frame = 4;
								projectile.velocity.Y = -6.1f;
								projectile.frameCounter = 0;
								for (int num48 = 0; num48 < 4; num48++)
								{
									int num47 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + (float)projectile.height - 2f), projectile.width, 4, 5, 0f, 0f, 0, default(Color), 1f);
									Dust obj7 = Main.dust[num47];
									obj7.velocity += projectile.velocity;
									Dust obj8 = Main.dust[num47];
									obj8.velocity *= 0.4f;
								}
							}
							else
							{
								projectile.frameCounter = num49 * 4;
							}
						}
					}
					else if (projectile.velocity.Y < 0f)
					{
						projectile.frameCounter = 0;
						projectile.frame = 5;
					}
					else
					{
						projectile.frame = 4;
						projectile.frameCounter = 3;
					}
					projectile.velocity.Y = projectile.velocity.Y + 0.4f;
					if (projectile.velocity.Y > 10f)
					{
						projectile.velocity.Y = 10f;
					}
				}
				else if (projectile.type == 269)
				{
					if (projectile.velocity.Y >= 0f && (double)projectile.velocity.Y <= 0.8)
					{
						if (projectile.velocity.X == 0f)
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
						else if ((double)projectile.velocity.X < -0.8 || (double)projectile.velocity.X > 0.8)
						{
							int num46 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + (float)projectile.height - 2f), projectile.width, 6, 76, 0f, 0f, 0, default(Color), 1f);
							Main.dust[num46].noGravity = true;
							Dust obj9 = Main.dust[num46];
							obj9.velocity *= 0.3f;
							Main.dust[num46].noLight = true;
							projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
							projectile.frameCounter++;
							if (projectile.frameCounter > 6)
							{
								projectile.frame++;
								projectile.frameCounter = 0;
							}
							if (projectile.frame > 3)
							{
								projectile.frame = 0;
							}
						}
						else
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
					}
					else
					{
						projectile.frameCounter = 0;
						projectile.frame = 2;
					}
					projectile.velocity.Y = projectile.velocity.Y + 0.4f;
					if (projectile.velocity.Y > 10f)
					{
						projectile.velocity.Y = 10f;
					}
				}
				else if (projectile.type == 313)
				{
					int num45 = (int)(projectile.Center.X / 16f);
					int num44 = (int)(projectile.Center.Y / 16f);
					if (Main.tile[num45, num44] != null && Main.tile[num45, num44].wall > 0)
					{
						projectile.position.Y = projectile.position.Y + (float)projectile.height;
						projectile.height = 34;
						projectile.position.Y = projectile.position.Y - (float)projectile.height;
						projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
						projectile.width = 34;
						projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
						Vector2 vector5 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
						float num43 = Main.player[projectile.owner].Center.X - vector5.X;
						float num42 = Main.player[projectile.owner].Center.Y - vector5.Y;
						float num41 = (float)Math.Sqrt((double)(num43 * num43 + num42 * num42));
						float num40 = 4f / num41;
						num43 *= num40;
						num42 *= num40;
						if (num41 < 120f)
						{
							projectile.velocity.X = projectile.velocity.X * 0.9f;
							projectile.velocity.Y = projectile.velocity.Y * 0.9f;
							if ((double)(Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) < 0.1)
							{
								projectile.velocity *= 0f;
							}
						}
						else
						{
							projectile.velocity.X = (projectile.velocity.X * 9f + num43) / 10f;
							projectile.velocity.Y = (projectile.velocity.Y * 9f + num42) / 10f;
						}
						if (num41 >= 120f)
						{
							projectile.spriteDirection = projectile.direction;
							projectile.rotation = (float)Math.Atan2((double)(projectile.velocity.Y * (0f - (float)projectile.direction)), (double)(projectile.velocity.X * (0f - (float)projectile.direction)));
						}
						projectile.frameCounter += (int)(Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y));
						if (projectile.frameCounter > 6)
						{
							projectile.frame++;
							projectile.frameCounter = 0;
						}
						if (projectile.frame > 10)
						{
							projectile.frame = 5;
						}
						if (projectile.frame < 5)
						{
							projectile.frame = 10;
						}
					}
					else
					{
						projectile.rotation = 0f;
						if (projectile.direction == -1)
						{
							projectile.spriteDirection = 1;
						}
						if (projectile.direction == 1)
						{
							projectile.spriteDirection = -1;
						}
						projectile.position.Y = projectile.position.Y + (float)projectile.height;
						projectile.height = 30;
						projectile.position.Y = projectile.position.Y - (float)projectile.height;
						projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
						projectile.width = 30;
						projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
						if (projectile.velocity.Y >= 0f && (double)projectile.velocity.Y <= 0.8)
						{
							if (projectile.velocity.X == 0f)
							{
								projectile.frame = 0;
								projectile.frameCounter = 0;
							}
							else if ((double)projectile.velocity.X < -0.8 || (double)projectile.velocity.X > 0.8)
							{
								projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
								projectile.frameCounter++;
								if (projectile.frameCounter > 6)
								{
									projectile.frame++;
									projectile.frameCounter = 0;
								}
								if (projectile.frame > 3)
								{
									projectile.frame = 0;
								}
							}
							else
							{
								projectile.frame = 0;
								projectile.frameCounter = 0;
							}
						}
						else
						{
							projectile.frameCounter = 0;
							projectile.frame = 4;
						}
						projectile.velocity.Y = projectile.velocity.Y + 0.4f;
						if (projectile.velocity.Y > 10f)
						{
							projectile.velocity.Y = 10f;
						}
					}
				}
				else if (projectile.type >= 390 && projectile.type <= 392)
				{
					int num37 = (int)(projectile.Center.X / 16f);
					int num36 = (int)(projectile.Center.Y / 16f);
					if (Main.tile[num37, num36] != null && Main.tile[num37, num36].wall > 0)
					{
						projectile.position.Y = projectile.position.Y + (float)projectile.height;
						projectile.height = 34;
						projectile.position.Y = projectile.position.Y - (float)projectile.height;
						projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
						projectile.width = 34;
						projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
						float scaleFactor = 9f;
						float num35 = (float)(40 * (projectile.minionPos + 1));
						Vector2 vector12 = Main.player[projectile.owner].Center - projectile.Center;
						if (flag5)
						{
							vector12 = vector6;
							num35 = 10f;
						}
						else if (!Collision.CanHitLine(projectile.Center, 1, 1, Main.player[projectile.owner].Center, 1, 1))
						{
							projectile.ai[0] = 1f;
						}
						if (vector12.Length() < num35)
						{
							projectile.velocity *= 0.9f;
							if ((double)(Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) < 0.1)
							{
								projectile.velocity *= 0f;
							}
						}
						else if (vector12.Length() < 800f || !flag5)
						{
							projectile.velocity = (projectile.velocity * 9f + Vector2.Normalize(vector12) * scaleFactor) / 10f;
						}
						if (vector12.Length() >= num35)
						{
							projectile.spriteDirection = projectile.direction;
							projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;
						}
						else
						{
							projectile.rotation = vector12.ToRotation() + 1.57079637f;
						}
						projectile.frameCounter += (int)(Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y));
						if (projectile.frameCounter > 5)
						{
							projectile.frame++;
							projectile.frameCounter = 0;
						}
						if (projectile.frame > 7)
						{
							projectile.frame = 4;
						}
						if (projectile.frame < 4)
						{
							projectile.frame = 7;
						}
					}
					else
					{
						if (!flag4)
						{
							projectile.rotation = 0f;
						}
						if (projectile.direction == -1)
						{
							projectile.spriteDirection = 1;
						}
						if (projectile.direction == 1)
						{
							projectile.spriteDirection = -1;
						}
						projectile.position.Y = projectile.position.Y + (float)projectile.height;
						projectile.height = 30;
						projectile.position.Y = projectile.position.Y - (float)projectile.height;
						projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
						projectile.width = 30;
						projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
						if (!flag5 && !Collision.CanHitLine(projectile.Center, 1, 1, Main.player[projectile.owner].Center, 1, 1))
						{
							projectile.ai[0] = 1f;
						}
						if (!flag4 && projectile.frame >= 4 && projectile.frame <= 7)
						{
							Vector2 vector4 = Main.player[projectile.owner].Center - projectile.Center;
							if (flag5)
							{
								vector4 = vector6;
							}
							float num34 = 0f - vector4.Y;
							if (vector4.Y <= 0f)
							{
								if (num34 < 120f)
								{
									projectile.velocity.Y = -10f;
								}
								else if (num34 < 210f)
								{
									projectile.velocity.Y = -13f;
								}
								else if (num34 < 270f)
								{
									projectile.velocity.Y = -15f;
								}
								else if (num34 < 310f)
								{
									projectile.velocity.Y = -17f;
								}
								else if (num34 < 380f)
								{
									projectile.velocity.Y = -18f;
								}
							}
						}
						if (flag4)
						{
							projectile.frameCounter++;
							if (projectile.frameCounter > 3)
							{
								projectile.frame++;
								projectile.frameCounter = 0;
							}
							if (projectile.frame >= 8)
							{
								projectile.frame = 4;
							}
							if (projectile.frame <= 3)
							{
								projectile.frame = 7;
							}
						}
						else if (projectile.velocity.Y >= 0f && (double)projectile.velocity.Y <= 0.8)
						{
							if (projectile.velocity.X == 0f)
							{
								projectile.frame = 0;
								projectile.frameCounter = 0;
							}
							else if ((double)projectile.velocity.X < -0.8 || (double)projectile.velocity.X > 0.8)
							{
								projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
								projectile.frameCounter++;
								if (projectile.frameCounter > 5)
								{
									projectile.frame++;
									projectile.frameCounter = 0;
								}
								if (projectile.frame > 2)
								{
									projectile.frame = 0;
								}
							}
							else
							{
								projectile.frame = 0;
								projectile.frameCounter = 0;
							}
						}
						else
						{
							projectile.frameCounter = 0;
							projectile.frame = 3;
						}
						projectile.velocity.Y = projectile.velocity.Y + 0.4f;
						if (projectile.velocity.Y > 10f)
						{
							projectile.velocity.Y = 10f;
						}
					}
				}
				else if (projectile.type == 314)
				{
					if (projectile.velocity.Y >= 0f && (double)projectile.velocity.Y <= 0.8)
					{
						if (projectile.velocity.X == 0f)
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
						else if ((double)projectile.velocity.X < -0.8 || (double)projectile.velocity.X > 0.8)
						{
							projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
							projectile.frameCounter++;
							if (projectile.frameCounter > 6)
							{
								projectile.frame++;
								projectile.frameCounter = 0;
							}
							if (projectile.frame > 6)
							{
								projectile.frame = 1;
							}
						}
						else
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
					}
					else
					{
						projectile.frameCounter = 0;
						projectile.frame = 7;
					}
					projectile.velocity.Y = projectile.velocity.Y + 0.4f;
					if (projectile.velocity.Y > 10f)
					{
						projectile.velocity.Y = 10f;
					}
				}
				else if (projectile.type == 319)
				{
					if (projectile.velocity.Y >= 0f && (double)projectile.velocity.Y <= 0.8)
					{
						if (projectile.velocity.X == 0f)
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
						else if ((double)projectile.velocity.X < -0.8 || (double)projectile.velocity.X > 0.8)
						{
							projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
							projectile.frameCounter++;
							if (projectile.frameCounter > 8)
							{
								projectile.frame++;
								projectile.frameCounter = 0;
							}
							if (projectile.frame > 5)
							{
								projectile.frame = 2;
							}
						}
						else
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
					}
					else
					{
						projectile.frameCounter = 0;
						projectile.frame = 1;
					}
					projectile.velocity.Y = projectile.velocity.Y + 0.4f;
					if (projectile.velocity.Y > 10f)
					{
						projectile.velocity.Y = 10f;
					}
				}
				else if (projectile.type == 236)
				{
					if (projectile.velocity.Y >= 0f && (double)projectile.velocity.Y <= 0.8)
					{
						if (projectile.velocity.X == 0f)
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
						else if ((double)projectile.velocity.X < -0.8 || (double)projectile.velocity.X > 0.8)
						{
							if (projectile.frame < 2)
							{
								projectile.frame = 2;
							}
							projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
							projectile.frameCounter++;
							if (projectile.frameCounter > 6)
							{
								projectile.frame++;
								projectile.frameCounter = 0;
							}
							if (projectile.frame > 8)
							{
								projectile.frame = 2;
							}
						}
						else
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
					}
					else
					{
						projectile.frameCounter = 0;
						projectile.frame = 1;
					}
					projectile.velocity.Y = projectile.velocity.Y + 0.4f;
					if (projectile.velocity.Y > 10f)
					{
						projectile.velocity.Y = 10f;
					}
				}
				else if (projectile.type == 499)
				{
					if (projectile.velocity.Y >= 0f && (double)projectile.velocity.Y <= 0.8)
					{
						if (projectile.velocity.X == 0f)
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
						else if ((double)projectile.velocity.X < -0.8 || (double)projectile.velocity.X > 0.8)
						{
							if (projectile.frame < 2)
							{
								projectile.frame = 2;
							}
							projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
							projectile.frameCounter++;
							if (projectile.frameCounter > 6)
							{
								projectile.frame++;
								projectile.frameCounter = 0;
							}
							if (projectile.frame >= 8)
							{
								projectile.frame = 2;
							}
						}
						else
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
					}
					else
					{
						projectile.frameCounter = 0;
						projectile.frame = 1;
					}
					projectile.velocity.Y = projectile.velocity.Y + 0.4f;
					if (projectile.velocity.Y > 10f)
					{
						projectile.velocity.Y = 10f;
					}
				}
				else if (projectile.type == 266)
				{
					if (projectile.velocity.Y >= 0f && (double)projectile.velocity.Y <= 0.8)
					{
						if (projectile.velocity.X == 0f)
						{
							projectile.frameCounter++;
						}
						else
						{
							projectile.frameCounter += 3;
						}
					}
					else
					{
						projectile.frameCounter += 5;
					}
					if (projectile.frameCounter >= 20)
					{
						projectile.frameCounter -= 20;
						projectile.frame++;
					}
					if (projectile.frame > 1)
					{
						projectile.frame = 0;
					}
					if (projectile.wet && Main.player[projectile.owner].position.Y + (float)Main.player[projectile.owner].height < projectile.position.Y + (float)projectile.height && projectile.localAI[0] == 0f)
					{
						if (projectile.velocity.Y > -4f)
						{
							projectile.velocity.Y = projectile.velocity.Y - 0.2f;
						}
						if (projectile.velocity.Y > 0f)
						{
							projectile.velocity.Y = projectile.velocity.Y * 0.95f;
						}
					}
					else
					{
						projectile.velocity.Y = projectile.velocity.Y + 0.4f;
					}
					if (projectile.velocity.Y > 10f)
					{
						projectile.velocity.Y = 10f;
					}
				}
				else if (projectile.type == 334)
				{
					if (projectile.velocity.Y == 0f)
					{
						if (projectile.velocity.X == 0f)
						{
							if (projectile.frame > 0)
							{
								projectile.frameCounter += 2;
								if (projectile.frameCounter > 6)
								{
									projectile.frame++;
									projectile.frameCounter = 0;
								}
								if (projectile.frame >= 7)
								{
									projectile.frame = 0;
								}
							}
							else
							{
								projectile.frame = 0;
								projectile.frameCounter = 0;
							}
						}
						else if ((double)projectile.velocity.X < -0.8 || (double)projectile.velocity.X > 0.8)
						{
							projectile.frameCounter += (int)Math.Abs((double)projectile.velocity.X * 0.75);
							projectile.frameCounter++;
							if (projectile.frameCounter > 6)
							{
								projectile.frame++;
								projectile.frameCounter = 0;
							}
							if (projectile.frame >= 7 || projectile.frame < 1)
							{
								projectile.frame = 1;
							}
						}
						else if (projectile.frame > 0)
						{
							projectile.frameCounter += 2;
							if (projectile.frameCounter > 6)
							{
								projectile.frame++;
								projectile.frameCounter = 0;
							}
							if (projectile.frame >= 7)
							{
								projectile.frame = 0;
							}
						}
						else
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
					}
					else if (projectile.velocity.Y < 0f)
					{
						projectile.frameCounter = 0;
						projectile.frame = 2;
					}
					else if (projectile.velocity.Y > 0f)
					{
						projectile.frameCounter = 0;
						projectile.frame = 4;
					}
					projectile.velocity.Y = projectile.velocity.Y + 0.4f;
					if (projectile.velocity.Y > 10f)
					{
						projectile.velocity.Y = 10f;
					}
				}
				else if (projectile.type == 353)
				{
					if (projectile.velocity.Y == 0f)
					{
						if (projectile.velocity.X == 0f)
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
						else if ((double)projectile.velocity.X < -0.8 || (double)projectile.velocity.X > 0.8)
						{
							projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
							projectile.frameCounter++;
							if (projectile.frameCounter > 6)
							{
								projectile.frame++;
								projectile.frameCounter = 0;
							}
							if (projectile.frame > 9)
							{
								projectile.frame = 2;
							}
						}
						else
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
					}
					else if (projectile.velocity.Y < 0f)
					{
						projectile.frameCounter = 0;
						projectile.frame = 1;
					}
					else if (projectile.velocity.Y > 0f)
					{
						projectile.frameCounter = 0;
						projectile.frame = 1;
					}
					projectile.velocity.Y = projectile.velocity.Y + 0.4f;
					if (projectile.velocity.Y > 10f)
					{
						projectile.velocity.Y = 10f;
					}
				}
				else if (projectile.type == 111)
				{
					if (projectile.velocity.Y == 0f)
					{
						if (projectile.velocity.X == 0f)
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
						else if ((double)projectile.velocity.X < -0.8 || (double)projectile.velocity.X > 0.8)
						{
							projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
							projectile.frameCounter++;
							if (projectile.frameCounter > 6)
							{
								projectile.frame++;
								projectile.frameCounter = 0;
							}
							if (projectile.frame >= 7)
							{
								projectile.frame = 0;
							}
						}
						else
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
					}
					else if (projectile.velocity.Y < 0f)
					{
						projectile.frameCounter = 0;
						projectile.frame = 4;
					}
					else if (projectile.velocity.Y > 0f)
					{
						projectile.frameCounter = 0;
						projectile.frame = 6;
					}
					projectile.velocity.Y = projectile.velocity.Y + 0.4f;
					if (projectile.velocity.Y > 10f)
					{
						projectile.velocity.Y = 10f;
					}
				}
				else if (projectile.type == 112)
				{
					if (projectile.velocity.Y == 0f)
					{
						if (projectile.velocity.X == 0f)
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
						else if ((double)projectile.velocity.X < -0.8 || (double)projectile.velocity.X > 0.8)
						{
							projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
							projectile.frameCounter++;
							if (projectile.frameCounter > 6)
							{
								projectile.frame++;
								projectile.frameCounter = 0;
							}
							if (projectile.frame >= 3)
							{
								projectile.frame = 0;
							}
						}
						else
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
					}
					else if (projectile.velocity.Y < 0f)
					{
						projectile.frameCounter = 0;
						projectile.frame = 2;
					}
					else if (projectile.velocity.Y > 0f)
					{
						projectile.frameCounter = 0;
						projectile.frame = 2;
					}
					projectile.velocity.Y = projectile.velocity.Y + 0.4f;
					if (projectile.velocity.Y > 10f)
					{
						projectile.velocity.Y = 10f;
					}
				}
				else if (projectile.type == 127)
				{
					if (projectile.velocity.Y == 0f)
					{
						if (projectile.velocity.X == 0f)
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
						else if ((double)projectile.velocity.X < -0.1 || (double)projectile.velocity.X > 0.1)
						{
							projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
							projectile.frameCounter++;
							if (projectile.frameCounter > 6)
							{
								projectile.frame++;
								projectile.frameCounter = 0;
							}
							if (projectile.frame > 5)
							{
								projectile.frame = 0;
							}
						}
						else
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
					}
					else
					{
						projectile.frame = 0;
						projectile.frameCounter = 0;
					}
					projectile.velocity.Y = projectile.velocity.Y + 0.4f;
					if (projectile.velocity.Y > 10f)
					{
						projectile.velocity.Y = 10f;
					}
				}
				else if (projectile.type == 200)
				{
					if (projectile.velocity.Y == 0f)
					{
						if (projectile.velocity.X == 0f)
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
						else if ((double)projectile.velocity.X < -0.1 || (double)projectile.velocity.X > 0.1)
						{
							projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
							projectile.frameCounter++;
							if (projectile.frameCounter > 6)
							{
								projectile.frame++;
								projectile.frameCounter = 0;
							}
							if (projectile.frame > 5)
							{
								projectile.frame = 0;
							}
						}
						else
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
					}
					else
					{
						projectile.rotation = projectile.velocity.X * 0.1f;
						projectile.frameCounter++;
						if (projectile.velocity.Y < 0f)
						{
							projectile.frameCounter += 2;
						}
						if (projectile.frameCounter > 6)
						{
							projectile.frame++;
							projectile.frameCounter = 0;
						}
						if (projectile.frame > 9)
						{
							projectile.frame = 6;
						}
						if (projectile.frame < 6)
						{
							projectile.frame = 6;
						}
					}
					projectile.velocity.Y = projectile.velocity.Y + 0.1f;
					if (projectile.velocity.Y > 4f)
					{
						projectile.velocity.Y = 4f;
					}
				}
				else if (projectile.type == 208)
				{
					if (projectile.velocity.Y == 0f && projectile.velocity.X == 0f)
					{
						if (Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) < projectile.position.X + (float)(projectile.width / 2))
						{
							projectile.direction = -1;
						}
						else if (Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) > projectile.position.X + (float)(projectile.width / 2))
						{
							projectile.direction = 1;
						}
						projectile.rotation = 0f;
						projectile.frame = 0;
					}
					else
					{
						projectile.rotation = projectile.velocity.X * 0.075f;
						projectile.frameCounter++;
						if (projectile.frameCounter > 6)
						{
							projectile.frame++;
							projectile.frameCounter = 0;
						}
						if (projectile.frame > 4)
						{
							projectile.frame = 1;
						}
						if (projectile.frame < 1)
						{
							projectile.frame = 1;
						}
					}
					projectile.velocity.Y = projectile.velocity.Y + 0.1f;
					if (projectile.velocity.Y > 4f)
					{
						projectile.velocity.Y = 4f;
					}
				}
				else if (projectile.type == 209)
				{
					if (projectile.alpha > 0)
					{
						projectile.alpha -= 5;
						if (projectile.alpha < 0)
						{
							projectile.alpha = 0;
						}
					}
					if (projectile.velocity.Y == 0f)
					{
						if (projectile.velocity.X == 0f)
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
						else if ((double)projectile.velocity.X < -0.1 || (double)projectile.velocity.X > 0.1)
						{
							projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
							projectile.frameCounter++;
							if (projectile.frameCounter > 6)
							{
								projectile.frame++;
								projectile.frameCounter = 0;
							}
							if (projectile.frame > 11)
							{
								projectile.frame = 2;
							}
							if (projectile.frame < 2)
							{
								projectile.frame = 2;
							}
						}
						else
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
					}
					else
					{
						projectile.frame = 1;
						projectile.frameCounter = 0;
						projectile.rotation = 0f;
					}
					projectile.velocity.Y = projectile.velocity.Y + 0.4f;
					if (projectile.velocity.Y > 10f)
					{
						projectile.velocity.Y = 10f;
					}
				}
				else if (projectile.type == 324)
				{
					if (projectile.velocity.Y == 0f)
					{
						if ((double)projectile.velocity.X < -0.1 || (double)projectile.velocity.X > 0.1)
						{
							projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
							projectile.frameCounter++;
							if (projectile.frameCounter > 6)
							{
								projectile.frame++;
								projectile.frameCounter = 0;
							}
							if (projectile.frame > 5)
							{
								projectile.frame = 2;
							}
							if (projectile.frame < 2)
							{
								projectile.frame = 2;
							}
						}
						else
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
					}
					else
					{
						projectile.frameCounter = 0;
						projectile.frame = 1;
					}
					projectile.velocity.Y = projectile.velocity.Y + 0.4f;
					if (projectile.velocity.Y > 14f)
					{
						projectile.velocity.Y = 14f;
					}
				}
				else if (projectile.type == 210)
				{
					if (projectile.velocity.Y == 0f)
					{
						if ((double)projectile.velocity.X < -0.1 || (double)projectile.velocity.X > 0.1)
						{
							projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
							projectile.frameCounter++;
							if (projectile.frameCounter > 6)
							{
								projectile.frame++;
								projectile.frameCounter = 0;
							}
							if (projectile.frame > 6)
							{
								projectile.frame = 0;
							}
						}
						else
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
					}
					else
					{
						projectile.rotation = projectile.velocity.X * 0.05f;
						projectile.frameCounter++;
						if (projectile.frameCounter > 6)
						{
							projectile.frame++;
							projectile.frameCounter = 0;
						}
						if (projectile.frame > 11)
						{
							projectile.frame = 7;
						}
						if (projectile.frame < 7)
						{
							projectile.frame = 7;
						}
					}
					projectile.velocity.Y = projectile.velocity.Y + 0.4f;
					if (projectile.velocity.Y > 10f)
					{
						projectile.velocity.Y = 10f;
					}
				}
				else if (projectile.type == 398)
				{
					if (projectile.velocity.Y == 0f)
					{
						if (projectile.velocity.X == 0f)
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
						else if ((double)projectile.velocity.X < -0.8 || (double)projectile.velocity.X > 0.8)
						{
							projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
							projectile.frameCounter++;
							if (projectile.frameCounter > 6)
							{
								projectile.frame++;
								projectile.frameCounter = 0;
							}
							if (projectile.frame >= 5)
							{
								projectile.frame = 0;
							}
						}
						else
						{
							projectile.frame = 0;
							projectile.frameCounter = 0;
						}
					}
					else if (projectile.velocity.Y != 0f)
					{
						projectile.frameCounter = 0;
						projectile.frame = 5;
					}
					projectile.velocity.Y = projectile.velocity.Y + 0.4f;
					if (projectile.velocity.Y > 10f)
					{
						projectile.velocity.Y = 10f;
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
			ProjectileID.Sets.MinionShot[projectile.type] = true;
			Main.projFrames[projectile.type] = 3;
		}

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 22;
			projectile.timeLeft = 300;
			projectile.penetrate = 1;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ignoreWater = false;
			projectile.alpha = 0;
		}

		public float color
		{
			get => projectile.ai[0];
			set => projectile.ai[0] = value;
		}

		public float ticks
		{
			get => projectile.ai[1];
			set => projectile.ai[1] = value;
		}

		public override void AI()
		{
			// Loop frames
			int frameSpeed = 5;
			projectile.frameCounter++;
			if (projectile.frameCounter >= frameSpeed)
			{
				projectile.frameCounter = 0;
				projectile.frame++;
				if (projectile.frame >= Main.projFrames[projectile.type])
				{
					projectile.frame = 0;
				}
			}

			ticks++;
			if (ticks >= MAX_TICKS)
			{
				const float velYmult = 0.5f;
				ticks = MAX_TICKS;
				projectile.velocity.Y += velYmult;
			}
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			//I don't think this is the best way to do this but I wanted to try drawcode
			Texture2D tex = GetTexture(Texture);
			Color[] colors = new Color[] { Color.Red, Color.Orange, Color.Yellow, Color.Blue, Color.Violet, Color.Pink };
			//spriteBatch.Draw(tex, projectile.position, tex.Frame(), colors[Main.rand.Next(colors.Length)], projectile.rotation, Vector2.Zero, projectile.scale, 0, 0);
			spriteBatch.Draw(tex, (projectile.position - Main.screenPosition) + new Vector2(0, Main.player[projectile.owner].gfxOffY), new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height), colors[(int)color], projectile.rotation, Vector2.Zero, projectile.scale, 0, 0);
			base.PostDraw(spriteBatch, lightColor);
		}
	}
}