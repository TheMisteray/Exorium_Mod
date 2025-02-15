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
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;

namespace ExoriumMod.Content.Bosses.GemsparklingHive
{
    abstract class Gemsparkling : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 3;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0;
            NPC.lifeMax = 150;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath7;
            NPC.timeLeft = NPC.activeTime * 30;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.alpha = 155;
        }

        protected float attackTimer = 0;

        public float ticker
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        public float action
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        //5 - Lock on hive
        //0 - move
        //1 - moving attack
        //2 - stationary attack

        public bool retreat
        {
            get => NPC.ai[2] == 1f;
            set => NPC.ai[2] = value ? 1f : 0f;
        }

        public float hiveWhoAmI
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }

        public override void AI()
        {
            #region Targeting
            if (Main.netMode != 1)
            {
                NPC.TargetClosest(true);
                NPC.netUpdate = true;
            }

            Player player = Main.player[NPC.target];
            if (!player.active || player.dead && Main.netMode != NetmodeID.MultiplayerClient || (NPC.position - player.position).Length() > 3000)
            {
                NPC.TargetClosest(true);
                NPC.netUpdate = true;
                player = Main.player[NPC.target];
                if (!player.active || player.dead || (NPC.position - player.position).Length() > 3000)
                {
                    NPC.ai[1] = 5;
                }
            }

            //Despawn in hive despawned
            if (!Main.npc[(int)hiveWhoAmI].active || Main.npc[(int)hiveWhoAmI].type != NPCType<GemsparklingHive>())
            {
                NPC.velocity = new Vector2(0f, 10f);
                if (NPC.timeLeft > 10)
                {
                    NPC.timeLeft = 10;
                }
                return;
            }


            #endregion

            if (NPC.ai[1] == 0)
            {
                Move();
                ticker++;
            }
            else if (NPC.ai[1] == 1)
            {
                MovingAttack();
                Move();
            }
            else if (NPC.ai[1] == 2)
            {
                StatinaryAttack();
                NPC.velocity *= 0.96f;
            }
            else if (NPC.ai[1] == 5)
            {
                Hide();
            }

            if (ticker % 160 == 120)
                NPC.ai[1] = 1;
            if (ticker % 540 == 300)
                NPC.ai[1] = 2;
        }

        public override void FindFrame(int frameHeight)
        {
            int frameSpeed = 10;
            NPC.frameCounter++;
            if (NPC.frameCounter >= frameSpeed)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= frameHeight * 3)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public void Move()
        {
            Player player = Main.player[NPC.target];

            //Reset hide effects
            NPC.dontTakeDamage = false;
            NPC.alpha = 0;

            if (ticker % 30 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                float speed = 10f;
                float inertia = 20f;

                //Movement
                if (player.active)
                {
                    Vector2 direction = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
                    direction *= speed;
                    NPC.velocity = (NPC.velocity * (inertia - 1) + direction) / inertia;
                }
                else
                    NPC.velocity = new Vector2(0f, 10f);

                if (Vector2.Distance(NPC.Center, Main.npc[(int)hiveWhoAmI].Center) > 600)
                {
                    Vector2 direction = Main.npc[(int)hiveWhoAmI].Center - NPC.Center;
                    direction.Normalize();
                    NPC.velocity = direction * 2;
                }

                NPC.netUpdate = true;
            }
        }

        public void Hide()
        {
            //Make intangible
            if (NPC.alpha < 255)
                NPC.alpha += 5;
            NPC.dontTakeDamage = true;

            //Movement
            if (NPC.alpha >= 255)
            {
                Vector2 direction = Main.npc[(int)hiveWhoAmI].Center - NPC.Center;
                NPC.velocity = direction;
            }
            else
            {
                Vector2 direction = Main.npc[(int)hiveWhoAmI].Center - NPC.Center;
                direction.Normalize();
                direction *= 10;
                NPC.velocity = direction;
            }
        }

        public override void OnKill()
        {
            NPC hive = Main.npc[(int)hiveWhoAmI];
            if (hive.type == NPCType<GemsparklingHive>())
                hive.ai[3] = 1;
            base.OnKill();
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return NPC.ai[1] != 5;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.alpha < 200)
            {
                Vector2 drawCenter = NPC.Center;
                drawCenter.Y += 4;
                spriteBatch.Draw(Request<Texture2D>(AssetDirectory.GemsparklingHive + Name).Value, drawCenter - screenPos, new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height), Color.White, NPC.rotation, new Vector2(NPC.width, NPC.height) / 2, 1, SpriteEffects.None, 0);
            }
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }

        public virtual void StatinaryAttack() { }

        public virtual void MovingAttack() { }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            int associatedNPCType = NPCType<GemsparklingHive>();
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("Faint traces of magic suggest that transmutation magic may be at play here rather than natural phenomena. They only seem concerned with protecting their hive."),
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
            });
        }
    }

    internal class AmethystGemsparkling : Gemsparkling
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetDefaults()
        {
            NPC.damage = 12;
            NPC.defDamage = 3;
            NPC.width = 26;
            NPC.height = 30;
            base.SetDefaults();
        }

        public override void MovingAttack()
        {
            DustHelper.DustRing(NPC.Center, DustType<Rainbow>(), 4, 0, .2f, 1, 0, 0, 0, new Color(149, 0, 255), true);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 shoot = Main.player[NPC.target].Center - NPC.Center;
                shoot.Normalize();
                shoot *= 5;
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, shoot, ProjectileType<GemDart>(), NPC.damage / 3, 1, Main.myPlayer, 6);
            }
            NPC.ai[1] = 0;
            NPC.ai[0]++;
        }

        public override void StatinaryAttack()
        {
            attackTimer++;
            if (attackTimer % 10 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 shoot = new Vector2(0, 5);
                Vector2 offShoot = shoot.RotatedBy(MathHelper.ToRadians(45 * (attackTimer / 10)));
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, offShoot, ProjectileType<GemDart>(), NPC.damage / 3, 1, Main.myPlayer, 6);
            }
            else if (attackTimer % 10 == 0)
                DustHelper.DustRing(NPC.Center, DustType<Rainbow>(), 4, 0, .2f, 1, 0, 0, 0, new Color(149, 0, 255), true);
            if (attackTimer > 80)
            {
                NPC.ai[1] = 0;
                attackTimer = 0;
                NPC.ai[0]++;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy) //Chain still draws if player near spawn pos so this needs to cut it
                return base.PreDraw(spriteBatch, screenPos, drawColor);
            Vector2 hiveCenter = Main.npc[(int)hiveWhoAmI].Center;
            Vector2 center = NPC.Center;
            Vector2 distToHive = hiveCenter - center;
            float projRotation = distToHive.ToRotation() - 1.57f;
            float distance = distToHive.Length();
            while (distance > 30f && !float.IsNaN(distance))
            {
                distToHive.Normalize();                 //get unit vector
                distToHive *= 24f;                      //speed = 24
                center += distToHive;                   //update draw position
                distToHive = hiveCenter - center;    //update distance
                distance = distToHive.Length();
                Color col = new Color(149, 0, 255);

                //Draw chain
                Texture2D tex = Request<Texture2D>(AssetDirectory.GemsparklingHive + "GemChain").Value;
                spriteBatch.Draw(tex, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, tex.Width, tex.Height), col, projRotation,
                    new Vector2(tex.Width * 0.5f, tex.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public override void OnKill()
        {
            DustHelper.DustCircle(NPC.Center, DustID.GemAmethyst, 2, 50, 1, .5f, 0, 0, Color.White, true);
            base.OnKill();
        }
    }

    internal class TopazGemsparkling : Gemsparkling
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetDefaults()
        {
            NPC.damage = 10;
            NPC.defDamage = 9;
            NPC.width = 38;
            NPC.height = 16;
            base.SetDefaults();
        }

        Vector2 v;

        public override void MovingAttack()
        {
            attackTimer++;
            if (attackTimer % 20 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                DustHelper.DustRing(NPC.Center, DustType<Rainbow>(), 4, 0, .2f, 1, 0, 0, 0, new Color(255, 247, 0), true);
                Vector2 shoot = Main.player[NPC.target].Center - NPC.Center;
                shoot.Normalize();
                shoot *= 4;
                Vector2 offShoot = shoot.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-30, 30)));
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, offShoot, ProjectileType<GemDart>(), NPC.damage / 3, 1, Main.myPlayer, 2);
            }
            else if (attackTimer % 20 == 0)
                DustHelper.DustRing(NPC.Center, DustType<Rainbow>(), 4, 0, .2f, 1, 0, 0, 0, new Color(255, 247, 0), true);
            if (attackTimer > 60)
            {
                NPC.ai[1] = 0;
                attackTimer = 0;
                NPC.ai[0]++;
            }
        }

        public override void StatinaryAttack()
        {
            if (attackTimer == 0)
            {
                v = Main.player[NPC.target].Center - NPC.Center;
                v.Normalize();
                v *= 4;
            }
            attackTimer++;
            if (attackTimer % 2 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, v, ProjectileType<TopazBeam>(), NPC.damage / 3, 1, Main.myPlayer, 0);
            }
            if (attackTimer > 120)
            {
                DustHelper.DustRing(NPC.Center, DustType<Rainbow>(), 4, 0, .2f, 1, 0, 0, 0, new Color(255, 247, 0), true);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, v, ProjectileType<TopazBeam>(), NPC.damage / 3, 1, Main.myPlayer, 1);
                NPC.ai[1] = 0;
                attackTimer = 0;
                NPC.ai[0]++;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 hiveCenter = Main.npc[(int)hiveWhoAmI].Center;
            Vector2 center = NPC.Center;
            Vector2 distToHive = hiveCenter - center;
            float projRotation = distToHive.ToRotation() - 1.57f;
            float distance = distToHive.Length();
            while (distance > 30f && !float.IsNaN(distance))
            {
                distToHive.Normalize();                 //get unit vector
                distToHive *= 24f;                      //speed = 24
                center += distToHive;                   //update draw position
                distToHive = hiveCenter - center;    //update distance
                distance = distToHive.Length();
                Color col = new Color(255, 247, 0);

                //Draw chain
                Texture2D tex = Request<Texture2D>(AssetDirectory.GemsparklingHive + "GemChain").Value;
                spriteBatch.Draw(tex, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, tex.Width, tex.Height), col, projRotation,
                    new Vector2(tex.Width * 0.5f, tex.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public override void OnKill()
        {
            DustHelper.DustCircle(NPC.Center, DustID.GemTopaz, 2, 50, 1, .5f, 0, 0, Color.White, true);
            base.OnKill();
        }
    }

    internal class SapphireGemsparkling : Gemsparkling
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetDefaults()
        {
            NPC.damage = 16;
            NPC.defDamage = 7;
            NPC.width = 38;
            NPC.height = 32;
            base.SetDefaults();
        }

        Vector2 v;
        float drawAlpha;

        public override void AI()
        {
            if (NPC.ai[1] != 2 && drawAlpha > 0)
                drawAlpha -= MathHelper.PiOver2 / 20;
            base.AI();
        }

        public override void MovingAttack()
        {
            DustHelper.DustRing(NPC.Center, DustType<Rainbow>(), 4, 0, .2f, 1, 0, 0, 0, new Color(35, 0, 255), true);
            for (int i = 0; i < 4; i++)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 shoot = Main.player[NPC.target].Center - NPC.Center;
                    shoot.Normalize();
                    shoot *= 5;
                    Vector2 offShoot = shoot.RotatedBy(MathHelper.ToRadians(90 * i));
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, offShoot, ProjectileType<GemDart>(), NPC.damage / 3, 1, Main.myPlayer, 5);
                }
            }
            NPC.ai[1] = 0;
            NPC.ai[0]++;
        }

        public override void StatinaryAttack()
        {
            if (attackTimer == 0)
            {
                v = Main.player[NPC.target].Center - NPC.Center;
                v.Normalize();
                v *= 5;
            }
            attackTimer++;
            if (drawAlpha < MathHelper.PiOver2)
                drawAlpha += MathHelper.PiOver2 / 20;
            NPC.velocity = v;
            if (attackTimer > 180)
            {
                NPC.ai[1] = 0;
                attackTimer = 0;
                NPC.ai[0]++;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.ai[1] == 2)
            {
                Texture2D tex = Request<Texture2D>(AssetDirectory.GemsparklingHive + "SapphireRing").Value;

                float scalar = 2;
                Main.spriteBatch.Draw(tex, (NPC.Center - Main.screenPosition), null, new Color((int)(35 * Math.Sin(drawAlpha)), 0, (int)(255 * Math.Sin(drawAlpha)), 0), 0, new Vector2(tex.Width / 2, tex.Height / 2), scalar, SpriteEffects.None, 0f);
            }

            Vector2 hiveCenter = Main.npc[(int)hiveWhoAmI].Center;
            Vector2 center = NPC.Center;
            Vector2 distToHive = hiveCenter - center;
            float projRotation = distToHive.ToRotation() - 1.57f;
            float distance = distToHive.Length();
            while (distance > 30f && !float.IsNaN(distance))
            {
                distToHive.Normalize();                 //get unit vector
                distToHive *= 24f;                      //speed = 24
                center += distToHive;                   //update draw position
                distToHive = hiveCenter - center;    //update distance
                distance = distToHive.Length();
                Color col = new Color(35, 0, 255);

                //Draw chain
                Texture2D tex = Request<Texture2D>(AssetDirectory.GemsparklingHive + "GemChain").Value;
                spriteBatch.Draw(tex, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, tex.Width, tex.Height), col, projRotation,
                    new Vector2(tex.Width * 0.5f, tex.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.GemsparklingHive + "SapphireRing").Value;

            float dist = Vector2.Distance(target.Center, NPC.Center);
            if (dist < tex.Width + NPC.width)
                return true;
            return base.CanHitPlayer(target, ref cooldownSlot);
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            if (NPC.ai[1] == 2)
                modifiers.FinalDamage -= 10;
            base.ModifyIncomingHit(ref modifiers);
        }

        public override void OnKill()
        {
            DustHelper.DustCircle(NPC.Center, DustID.GemSapphire, 2, 50, 1, .5f, 0, 0, Color.White, true);
            base.OnKill();
        }
    }

    internal class EmeraldGemsparkling : Gemsparkling
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetDefaults()
        {
            NPC.damage = 17;
            NPC.defDamage = 2;
            NPC.width = 22;
            NPC.height = 40;
            base.SetDefaults();
        }

        public override void MovingAttack()
        {
            DustHelper.DustRing(NPC.Center, DustType<Rainbow>(), 4, 0, .2f, 1, 0, 0, 0, new Color(0, 255, 0), true);
            for (int i = 0; i < 3; i++)
            {
                Vector2 shoot = Main.player[NPC.target].Center - NPC.Center;
                shoot.Normalize();
                shoot *= 4;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, shoot.RotatedBy(MathHelper.ToRadians(-30 + 30 * i)), ProjectileType<GemDart>(), NPC.damage / 3, 1, Main.myPlayer, 3);
            }
            NPC.ai[1] = 0;
            NPC.ai[0]++;
        }

        public override void StatinaryAttack()
        {
            if (NPC.velocity.Length() < .1f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 shoot = Main.player[NPC.target].Center - NPC.Center;
                shoot.Normalize();
                shoot *= 20;
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + shoot, shoot, ProjectileType<EmeraldSpike>(), NPC.damage / 3, 1, Main.myPlayer, 0);
                NPC.ai[1] = 0;
                attackTimer = 0;
                NPC.ai[0]++;
            }
            else if (NPC.velocity.Length() < .1f)
                DustHelper.DustRing(NPC.Center, DustType<Rainbow>(), 4, 0, .2f, 1, 0, 0, 0, new Color(0, 255, 0), true);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 hiveCenter = Main.npc[(int)hiveWhoAmI].Center;
            Vector2 center = NPC.Center;
            Vector2 distToHive = hiveCenter - center;
            float projRotation = distToHive.ToRotation() - 1.57f;
            float distance = distToHive.Length();
            while (distance > 30f && !float.IsNaN(distance))
            {
                distToHive.Normalize();                 //get unit vector
                distToHive *= 24f;                      //speed = 24
                center += distToHive;                   //update draw position
                distToHive = hiveCenter - center;    //update distance
                distance = distToHive.Length();
                Color col = new Color(0, 255, 0);

                //Draw chain
                Texture2D tex = Request<Texture2D>(AssetDirectory.GemsparklingHive + "GemChain").Value;
                spriteBatch.Draw(tex, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, tex.Width, tex.Height), col, projRotation,
                    new Vector2(tex.Width * 0.5f, tex.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public override void OnKill()
        {
            DustHelper.DustCircle(NPC.Center, DustID.GemEmerald, 2, 50, 1, .5f, 0, 0, Color.White, true);
            base.OnKill();
        }
    }

    internal class RubyGemsparkling : Gemsparkling
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetDefaults()
        {
            NPC.damage = 20;
            NPC.defDamage = 0;
            NPC.width = 22;
            NPC.height = 38;
            base.SetDefaults();
        }

        public override void MovingAttack()
        {
            DustHelper.DustRing(NPC.Center, DustType<Rainbow>(), 4, 0, .2f, 1, 0, 0, 0, new Color(255, 0, 0), true);
            for (int i = 0; i < 3; i++)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 shoot = Main.player[NPC.target].Center - NPC.Center;
                    shoot.Normalize();
                    shoot *= 4 + i;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, shoot, ProjectileType<GemDart>(), NPC.damage / 3, 1, Main.myPlayer, 0);
                }
            }
            NPC.ai[1] = 0;
            NPC.ai[0]++;
        }

        public override void StatinaryAttack()
        {
            if (NPC.velocity.Length() < .1f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 shoot = Main.player[NPC.target].Center - NPC.Center;
                shoot.Normalize();
                shoot *= 6;
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, shoot, ProjectileType<RubyCrystal>(), NPC.damage / 3, 1, Main.myPlayer, 1, Main.rand.NextFloat(.1f, .3f));
                NPC.ai[1] = 0;
                attackTimer = 0;
                NPC.ai[0]++;
            }
            else if (NPC.velocity.Length() < .1f)
                DustHelper.DustRing(NPC.Center, DustType<Rainbow>(), 4, 0, .2f, 1, 0, 0, 0, new Color(255, 0, 0), true);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 hiveCenter = Main.npc[(int)hiveWhoAmI].Center;
            Vector2 center = NPC.Center;
            Vector2 distToHive = hiveCenter - center;
            float projRotation = distToHive.ToRotation() - 1.57f;
            float distance = distToHive.Length();
            while (distance > 30f && !float.IsNaN(distance))
            {
                distToHive.Normalize();                 //get unit vector
                distToHive *= 24f;                      //speed = 24
                center += distToHive;                   //update draw position
                distToHive = hiveCenter - center;    //update distance
                distance = distToHive.Length();
                Color col = new Color(255, 0, 0);

                //Draw chain
                Texture2D tex = Request<Texture2D>(AssetDirectory.GemsparklingHive + "GemChain").Value;
                spriteBatch.Draw(tex, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, tex.Width, tex.Height), col, projRotation,
                    new Vector2(tex.Width * 0.5f, tex.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public override void OnKill()
        {
            DustHelper.DustCircle(NPC.Center, DustID.GemRuby, 2, 50, 1, .5f, 0, 0, Color.White, true);
            base.OnKill();
        }
    }

    internal class DiamondGemsparkling : Gemsparkling
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetDefaults()
        {
            NPC.damage = 22;
            NPC.defDamage = 11;
            NPC.width = 38;
            NPC.height = 28;
            base.SetDefaults();
        }

        public override void MovingAttack()
        {
            DustHelper.DustRing(NPC.Center, DustType<Rainbow>(), 4, 0, .2f, 1, 0, 0, 0, Color.White, true);
            for (int i = 0; i < 5; i++)
            {
                Vector2 shoot = Main.player[NPC.target].Center - NPC.Center;
                shoot.Normalize();
                shoot *= 5;
                Vector2 lineOffset = shoot.RotatedBy(MathHelper.PiOver2) * 4;
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + (-2 + i) * lineOffset, shoot, ProjectileType<GemDart>(), NPC.damage / 3, 1, Main.myPlayer, 4);
            }
            NPC.ai[1] = 0;
            NPC.ai[0]++;
        }

        public override void StatinaryAttack()
        {
            if (attackTimer == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 shoot = Main.player[NPC.target].Center - NPC.Center;
                shoot.Normalize();

                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, shoot, ProjectileType<DiamondBeam>(), NPC.damage / 2, 1, Main.myPlayer, 0, NPC.whoAmI);
            }
            attackTimer++;
            if (attackTimer > 230)
            {
                NPC.ai[1] = 0;
                NPC.ai[0]++;
                attackTimer = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 hiveCenter = Main.npc[(int)hiveWhoAmI].Center;
            Vector2 center = NPC.Center;
            Vector2 distToHive = hiveCenter - center;
            float projRotation = distToHive.ToRotation() - 1.57f;
            float distance = distToHive.Length();
            while (distance > 30f && !float.IsNaN(distance))
            {
                distToHive.Normalize();                 //get unit vector
                distToHive *= 24f;                      //speed = 24
                center += distToHive;                   //update draw position
                distToHive = hiveCenter - center;    //update distance
                distance = distToHive.Length();
                Color col = Color.White;

                //Draw chain
                Texture2D tex = Request<Texture2D>(AssetDirectory.GemsparklingHive + "GemChain").Value;
                spriteBatch.Draw(tex, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, tex.Width, tex.Height), col, projRotation,
                    new Vector2(tex.Width * 0.5f, tex.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public override void OnKill()
        {
            DustHelper.DustCircle(NPC.Center, DustID.GemDiamond, 2, 50, 1, .5f, 0, 0, Color.White, true);
            base.OnKill();
        }
    }

    internal class AmberGemsparkling : Gemsparkling
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetDefaults()
        {
            NPC.damage = 12;
            NPC.defDamage = 3;
            NPC.width = 38;
            NPC.height = 24;
            base.SetDefaults();
        }

        public override void MovingAttack()
        {
            DustHelper.DustRing(NPC.Center, DustType<Rainbow>(), 4, 0, .2f, 1, 0, 0, 0, new Color(255, 110, 0), true);
            for (int i = 0; i < 5; i++)
            {
                Vector2 shoot = Main.player[NPC.target].Center - NPC.Center;
                shoot.Normalize();
                shoot *= 6 - Math.Abs(-2 + i);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, shoot.RotatedBy(MathHelper.ToRadians(-30 + 15 * i)), ProjectileType<GemDart>(), NPC.damage / 3, 1, Main.myPlayer, 1);
            }
            NPC.ai[1] = 0;
            NPC.ai[0]++;
        }

        public override void StatinaryAttack()
        {
            if (NPC.velocity.Length() < .1f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 shoot = Main.player[NPC.target].Center - NPC.Center;
                shoot.Normalize();
                shoot *= 2;
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, shoot, ProjectileType<AmberPulse>(), NPC.damage / 3, 1, Main.myPlayer);
                NPC.ai[1] = 0;
                attackTimer = 0;
                NPC.ai[0]++;
            }
            if (NPC.velocity.Length() < .1f)
                DustHelper.DustRing(NPC.Center, DustType<Rainbow>(), 4, 0, .2f, 1, 0, 0, 0, new Color(255, 110, 0), true);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 hiveCenter = Main.npc[(int)hiveWhoAmI].Center;
            Vector2 center = NPC.Center;
            Vector2 distToHive = hiveCenter - center;
            float projRotation = distToHive.ToRotation() - 1.57f;
            float distance = distToHive.Length();
            while (distance > 30f && !float.IsNaN(distance))
            {
                distToHive.Normalize();                 //get unit vector
                distToHive *= 24f;                      //speed = 24
                center += distToHive;                   //update draw position
                distToHive = hiveCenter - center;    //update distance
                distance = distToHive.Length();
                Color col = new Color(255, 110, 0);

                //Draw chain
                Texture2D tex = Request<Texture2D>(AssetDirectory.GemsparklingHive + "GemChain").Value;
                spriteBatch.Draw(tex, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, tex.Width, tex.Height), col, projRotation,
                    new Vector2(tex.Width * 0.5f, tex.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public override void OnKill()
        {
            DustHelper.DustCircle(NPC.Center, DustID.AmberBolt, 2, 50, 1, .5f, 0, 0, Color.White, true);
            base.OnKill();
        }
    }
}