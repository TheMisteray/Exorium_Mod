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

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    class Caravene : ModNPC
    {
        public override string Texture => AssetDirectory.CrimsonKnight + Name;
        public override string BossHeadTexture => AssetDirectory.CrimsonKnight + Name + "_Head_Boss";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crimson Knight");
            Main.npcFrameCount[npc.type] = 7;

            //Always draw so visuals don't fail while offscreen
            NPCID.Sets.MustAlwaysDraw[npc.type] = true;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 6666;
            npc.damage = 29;
            npc.defense = 11;
            npc.knockBackResist = 0f;
            npc.width = 42;
            npc.height = 48;
            npc.value = Item.buyPrice(0, 7, 7, 7);
            npc.npcSlots = 30f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath52;
            npc.timeLeft = NPC.activeTime * 30;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.noGravity = false;
            npc.noTileCollide = false;
            //music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/BathrobeMan");
            //bossBag = ItemType<ShadowmancerBag>();
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.8 * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.8);
        }

        private bool introAnimation = true;
        private int introTicker = 180;

        private bool exitAnimation = false;

        private bool leftTele = false;

        //Actions
        //0 - jump
        //1 - dash
        //2 - Teleport next to player
        //3 - Send down flame ring
        //4 - parry
        //5 - swing up with fireballs
        //6 - swords come down
        //7 - toss fireball arc
        //8 - sweep up
        //9 - flame breath
        //10 - portal dash
        //11 - Enrage
        public float Action
        {
            get => npc.ai[0];
            set => npc.ai[0] = value;
        }

        private float wait = 0;

        private float actionTimer;

        public override void AI()
        {
            int damage = npc.damage / (Main.expertMode == true ? 4 : 2);

            if (Main.netMode != 1)
            {
                npc.TargetClosest(true);
            }

            Player player = Main.player[npc.target];
            if (!player.active || player.dead && Main.netMode != NetmodeID.MultiplayerClient || (npc.position - player.position).Length() > 3000)
            {
                npc.TargetClosest(true);
                npc.netUpdate = true;
                player = Main.player[npc.target];
                if (!player.active || player.dead || (npc.position - player.position).Length() > 3000)
                {
                    //TODO: this still will use the same exit if the player is far
                    exitAnimation = true;
                    return;
                }
            }




            if (introAnimation)
            {
                IntroAI();
                return;
            }
            else if (wait > 0) //What to do while waiting
            {
                npc.velocity = Vector2.Zero;
                npc.noGravity = false;

                switch (Action)
                {

                }

                return;
            }

            //Action to make
            switch (Action)
            {
                case 0:
                    npc.velocity = new Vector2(0, -12);
                    npc.noGravity = false;
                    if (actionTimer > 60)
                    {
                        //TODO: add move selection and horizontal movement
                    }
                    break;
                case 1:
                    npc.velocity = new Vector2(7, 0) * ((player.Center.X > npc.Center.X) ? 1 : -1);
                    break;
                case 2:
                    if (actionTimer > 0)
                    {
                        if ((npc.Center - player.Center).Length() > 0)
                            leftTele = false;
                        else
                            leftTele = true;
                    }

                    if (actionTimer == 90)
                    {
                        npc.velocity = (player.Center - npc.Center) / 4;
                        npc.noGravity = true;
                    }
                    if (actionTimer == 94)
                    {
                        npc.velocity = Vector2.Zero;
                    }
                    //TODO swing and then gain gravity

                    break;
                case 3:
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            Projectile.NewProjectile(player.Center + new Vector2(0, -1000), Vector2.Zero, ProjectileType<FireballRing>(), damage, 1, Main.myPlayer, (MathHelper.Pi / 6) * i, (npc.life < (npc.lifeMax / 2)) ? 1 : 0);
                        }
                    }
                    break;
                case 4:

                    break;
                case 5:
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            Projectile.NewProjectile(npc.Bottom, new Vector2(-20 + (10 * i), 12), ProjectileType<CaraveneFireball>(), damage, 2, Main.myPlayer, 0, (npc.life < (npc.lifeMax / 2)) ? 1 : 0);
                        }
                    }
                    break;
                case 6:
                    if (actionTimer % 10 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(player.Center + new Vector2(Main.rand.NextFloat(-600, 600), -400), Vector2.Zero, ProjectileType<CaraveneBladeProj>(), (int)(damage * 1.5f), 1, Main.myPlayer, 0, (npc.life < (npc.lifeMax / 2)) ? 1 : 0);
                    }
                    break;
                case 7:
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            Projectile.NewProjectile(npc.Bottom, new Vector2(0 + (15 * i), 16 - i), ProjectileType<CaraveneFireball>(), damage, 2, Main.myPlayer, 0, (npc.life < (npc.lifeMax / 2)) ? 1 : 0);
                        }
                    }
                    break;
                case 8:
                    npc.velocity = new Vector2(0, -12);
                    npc.noGravity = false;
                    if (actionTimer > 60)
                    {
                        //TODO: add move selection
                    }
                    break;
                case 9:
                    break;
                case 10:
                    break;
                case 11:
                    npc.velocity = Vector2.Zero;
                    npc.noGravity = false;
                    if (actionTimer > 120)
                    {

                    }
                    break;
            }

            actionTimer++;
        }

        //Play intro animation
        public void IntroAI()
        {
            introTicker--;
            if (introTicker <= 0)
                introAnimation = false;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (introAnimation)
                return false;
            return base.CanHitPlayer(target, ref cooldownSlot);
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (introAnimation)
                return false;
            return base.CanBeHitByProjectile(projectile);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }
    }
}
