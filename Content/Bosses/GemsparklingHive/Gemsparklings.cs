using ExoriumMod.Core;
using ExoriumMod.Helpers;
using Microsoft.Xna.Framework;
using System;
using ExoriumMod.Content.Dusts;
using ExoriumMod.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Bosses.GemsparklingHive
{
    abstract class Gemsparkling : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.knockBackResist = 0;
            npc.lifeMax = 150;
            npc.HitSound = SoundID.NPCHit5;
            npc.DeathSound = SoundID.NPCDeath7;
            npc.timeLeft = NPC.activeTime * 30;
            npc.noGravity = true;
            npc.noTileCollide = true;
        }

        protected float attackTimer = 0;

        public float ticker
        {
            get => npc.ai[0];
            set => npc.ai[0] = value;
        }

        public float action
        {
            get => npc.ai[1];
            set => npc.ai[1] = value;
        }
        //5 - Lock on hive
        //0 - move
        //1 - moving attack
        //2 - stationary attack

        public bool retreat
        {
            get => npc.ai[2] == 1f;
            set => npc.ai[2] = value ? 1f : 0f;
        }

        public float hiveWhoAmI
        {
            get => npc.ai[3];
            set => npc.ai[3] = value;
        }

        public override void AI()
        {
            #region Targeting
            if (Main.netMode != 1)
            {
                npc.TargetClosest(true);
                npc.netUpdate = true;
            }

            Player player = Main.player[npc.target];
            if (!player.active || player.dead && Main.netMode != NetmodeID.MultiplayerClient || (npc.position - player.position).Length() > 3000)
            {
                npc.TargetClosest(true);
                npc.netUpdate = true;
                player = Main.player[npc.target];
                if (!player.active || player.dead || (npc.position - player.position).Length() > 3000)
                {
                    npc.ai[1] = 5;
                }
            }

            //Despawn in hive despawned
            if (!Main.npc[(int)hiveWhoAmI].active || Main.npc[(int)hiveWhoAmI].type != NPCType<GemsparklingHive>())
            {
                npc.velocity = new Vector2(0f, 10f);
                if (npc.timeLeft > 10)
                {
                    npc.timeLeft = 10;
                }
                return;
            }


            #endregion

            if (npc.ai[1] == 0)
            {
                Move();
                ticker++;
            }
            else if (npc.ai[1] == 1)
            {
                MovingAttack();
                Move();
            }
            else if (npc.ai[1] == 2)
            {
                StatinaryAttack();
                npc.velocity *= 0.96f;
            }
            else if (npc.ai[1] == 5)
            {
                Hide();
            }


            if (ticker % 160 == 120)
                npc.ai[1] = 1;
            if (ticker % 540 == 300)
                npc.ai[1] = 2;
        }

        public override void FindFrame(int frameHeight)
        {
            int frameSpeed = 10;
            npc.frameCounter++;
            if (npc.frameCounter >= frameSpeed)
            {
                npc.frameCounter = 0;
                npc.frame.Y += frameHeight;
                if (npc.frame.Y >= frameHeight * 3)
                {
                    npc.frame.Y = 0;
                }
            }
        }

        public void Move()
        {
            Player player = Main.player[npc.target];

            //Reset hide effects
            npc.dontTakeDamage = false;
            npc.alpha = 0;

            if (ticker % 60 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                float speed = 10f;
                float inertia = 20f;

                //Movement
                if (player.active)
                {
                    Vector2 direction = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
                    direction *= speed;
                    npc.velocity = (npc.velocity * (inertia - 1) + direction) / inertia;
                }
                else
                    npc.velocity = new Vector2(0f, 10f);

                if (Vector2.Distance(npc.Center, Main.npc[(int)hiveWhoAmI].Center) > 600)
                {
                    Vector2 direction = Main.npc[(int)hiveWhoAmI].Center - npc.Center;
                    direction.Normalize();
                    npc.velocity = direction * 2;
                }

                npc.netUpdate = true;
            }
        }

        public void Hide()
        {
            //Make intangible
            if (npc.alpha < 255)
                npc.alpha += 5;
            npc.dontTakeDamage = true;

            //Movement
            if (npc.alpha >= 255)
            {
                float between = Vector2.Distance(Main.npc[(int)hiveWhoAmI].Center, npc.Center);
                Vector2 direction = Main.npc[(int)hiveWhoAmI].Center - npc.Center;
                npc.velocity = direction;
            }
            else
            {
                float between = Vector2.Distance(Main.npc[(int)hiveWhoAmI].Center, npc.Center);
                Vector2 direction = Main.npc[(int)hiveWhoAmI].Center - npc.Center;
                direction.Normalize();
                direction *= 10;
                npc.velocity = direction;
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return npc.ai[1] != 5;
        }

        public virtual void StatinaryAttack() { }

        public virtual void MovingAttack() { }
    }

    internal class AmethystGemsparkling : Gemsparkling
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetDefaults()
        {
            npc.damage = 12;
            npc.defDamage = 3;
            npc.width = 23;
            npc.height = 30;
            base.SetDefaults();
        }

        public override void MovingAttack()
        {
            DustHelper.DustRing(npc.Center, DustType<Rainbow>(), 4, 0, .2f, 1, 0, 0, 0, new Color(149, 0, 255), true);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 shoot = Main.player[npc.target].Center - npc.Center;
                shoot.Normalize();
                shoot *= 5;
                Projectile.NewProjectile(npc.Center, shoot, ProjectileType<GemDart>(), npc.damage / 2, 1, Main.myPlayer, 6);
            }
            npc.ai[1] = 0;
            npc.ai[0] ++;
        }

        public override void StatinaryAttack()
        {
            attackTimer++;
            if (attackTimer % 10 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 shoot = new Vector2(0, 5);
                Vector2 offShoot = shoot.RotatedBy(MathHelper.ToRadians(45 * (attackTimer / 10)));
                Projectile.NewProjectile(npc.Center, offShoot, ProjectileType<GemDart>(), npc.damage / 2, 1, Main.myPlayer, 6);
            }
            if (attackTimer > 80)
            {
                npc.ai[1] = 0;
                attackTimer = 0;
                npc.ai[0]++;
            }
        }
    }

    internal class TopazGemsparkling : Gemsparkling
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetDefaults()
        {
            npc.damage = 10;
            npc.defDamage = 9;
            npc.width = 38;
            npc.height = 48;
            base.SetDefaults();
        }

        public override void MovingAttack()
        {
            attackTimer++;
            if (attackTimer % 20 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                DustHelper.DustRing(npc.Center, DustType<Rainbow>(), 4, 0, .2f, 1, 0, 0, 0, new Color(255, 247, 0), true);
                Vector2 shoot = Main.player[npc.target].Center - npc.Center;
                shoot.Normalize();
                shoot *= 4;
                Vector2 offShoot = shoot.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-30, 30)));
                Projectile.NewProjectile(npc.Center, offShoot, ProjectileType<GemDart>(), npc.damage / 2, 1, Main.myPlayer, 2);
            }
            if (attackTimer > 60)
            {
                npc.ai[1] = 0;
                attackTimer = 0;
                npc.ai[0]++;
            }
        }
    }

    internal class SapphireGemsparkling : Gemsparkling
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetDefaults()
        {
            npc.damage = 16;
            npc.defDamage = 7;
            npc.width = 38;
            npc.height = 32;
            base.SetDefaults();
        }

        public override void MovingAttack()
        {
            DustHelper.DustRing(npc.Center, DustType<Rainbow>(), 4, 0, .2f, 1, 0, 0, 0, new Color(35, 0, 255), true);
            for (int i = 0; i < 4; i++)
            {
                Vector2 shoot = Main.player[npc.target].Center - npc.Center;
                shoot.Normalize();
                shoot *= 5;
                Vector2 offShoot = shoot.RotatedBy(MathHelper.ToRadians(90 * i));
                Projectile.NewProjectile(npc.Center, offShoot, ProjectileType<GemDart>(), npc.damage / 2, 1, Main.myPlayer, 5);
            }
            npc.ai[1] = 0;
            npc.ai[0]++;
        }
    }

    internal class EmeraldGemsparkling : Gemsparkling
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetDefaults()
        {
            npc.damage = 17;
            npc.defDamage = 2;
            npc.width = 22;
            npc.height = 40;
            base.SetDefaults();
        }

        public override void MovingAttack()
        {
            DustHelper.DustRing(npc.Center, DustType<Rainbow>(), 4, 0, .2f, 1, 0, 0, 0, new Color(0, 255, 0), true);
            for (int i = 0; i < 3; i++)
            {
                Vector2 shoot = Main.player[npc.target].Center - npc.Center;
                shoot.Normalize();
                shoot *= 4;
                Projectile.NewProjectile(npc.Center, shoot.RotatedBy(MathHelper.ToRadians(-30 + 30 * i)), ProjectileType<GemDart>(), npc.damage / 2, 1, Main.myPlayer, 3);
            }
            npc.ai[1] = 0;
            npc.ai[0]++;
        }
    }

    internal class RubyGemsparkling : Gemsparkling
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetDefaults()
        {
            npc.damage = 20;
            npc.defDamage = 0;
            npc.width = 22;
            npc.height = 38;
            base.SetDefaults();
        }

        public override void MovingAttack()
        {
            DustHelper.DustRing(npc.Center, DustType<Rainbow>(), 4, 0, .2f, 1, 0, 0, 0, new Color(255, 0, 0), true);
            for (int i = 0; i < 3; i++)
            {
                Vector2 shoot = Main.player[npc.target].Center - npc.Center;
                shoot.Normalize();
                shoot *= 4 + i;
                Projectile.NewProjectile(npc.Center, shoot, ProjectileType<GemDart>(), npc.damage / 2, 1, Main.myPlayer, 0);
            }
            npc.ai[1] = 0;
            npc.ai[0]++;
        }
    }

    internal class DiamondGemsparkling : Gemsparkling
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetDefaults()
        {
            npc.damage = 22;
            npc.defDamage = 11;
            npc.width = 23;
            npc.height = 30;
            base.SetDefaults();
        }

        public override void MovingAttack()
        {
            DustHelper.DustRing(npc.Center, DustType<Rainbow>(), 4, 0, .2f, 1, 0, 0, 0, new Color(100, 100, 100), true);
            for (int i = 0; i < 5; i++)
            {
                Vector2 shoot = Main.player[npc.target].Center - npc.Center;
                shoot.Normalize();
                shoot *= 5;
                Vector2 lineOffset = shoot.RotatedBy(MathHelper.PiOver2) * 4;
                Projectile.NewProjectile(npc.Center + (-2 + i) * lineOffset, shoot, ProjectileType<GemDart>(), npc.damage / 2, 1, Main.myPlayer, 4);
            }
            npc.ai[1] = 0;
            npc.ai[0]++;
        }
    }

    internal class AmberGemsparkling : Gemsparkling
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetDefaults()
        {
            npc.damage = 12;
            npc.defDamage = 3;
            npc.width = 23;
            npc.height = 30;
            base.SetDefaults();
        }

        public override void MovingAttack()
        {
            DustHelper.DustRing(npc.Center, DustType<Rainbow>(), 4, 0, .2f, 1, 0, 0, 0, new Color(255, 110, 0), true);
            for (int i = 0; i < 5; i++)
            {
                Vector2 shoot = Main.player[npc.target].Center - npc.Center;
                shoot.Normalize();
                shoot *= 6 - Math.Abs(-2 + i);
                Projectile.NewProjectile(npc.Center, shoot.RotatedBy(MathHelper.ToRadians(-30 + 15 * i)), ProjectileType<GemDart>(), npc.damage / 2, 1, Main.myPlayer, 1);
            }
            npc.ai[1] = 0;
            npc.ai[0]++;
        }
    }
}