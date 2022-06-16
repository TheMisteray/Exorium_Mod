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
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    class Caravene : ModNPC
    {
        public override string Texture => AssetDirectory.CrimsonKnight + Name;
        public override string BossHeadTexture => AssetDirectory.CrimsonKnight + Name + "_Head_Boss";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crimson Knight");
            Main.npcFrameCount[NPC.type] = 6;

            //Always draw so visuals don't fail while offscreen
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 6666;
            NPC.damage = 29;
            NPC.defense = 11;
            NPC.knockBackResist = 0f;
            NPC.width = 140;
            NPC.height = 240;
            NPC.value = Item.buyPrice(0, 7, 7, 7);
            NPC.npcSlots = 30f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath52;
            NPC.timeLeft = NPC.activeTime * 30;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            //music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/BathrobeMan");
            //bossBag = ItemType<ShadowmancerBag>();
        }

        //May want to make teleport next to player not damage when teleporting

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.75 * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 0.7);
        }

        private bool left = false;

        private int frameX = 0;

        private int loopCounter = 0;

        //Action trackers
        private bool teleIndicator = false;
        private bool parry = false;
        private bool dashIndicator = false;

        //Phase trackers
        private bool introAnimation = true;
        private int introTicker = 180;

        private bool exitAnimation = false;

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
        //11 - Burning Sphere
        //12 - enrage
        public float Action
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        private float wait = 60;

        private float actionTimer;

        public override void AI()
        {
            int damage = NPC.damage / (Main.expertMode == true ? 4 : 2);

            if (Main.netMode != 1)
            {
                NPC.TargetClosest(true);
            }

            Player player = Main.player[NPC.target];
            if (!player.active || player.dead && Main.netMode != NetmodeID.MultiplayerClient || (NPC.position - player.position).Length() > 3000)
            {
                NPC.TargetClosest(true);
                NPC.netUpdate = true;
                player = Main.player[NPC.target];
                if (!player.active || player.dead || (NPC.position - player.position).Length() > 3000)
                {
                    //TODO: this still will use the same exit if the player is far
                    exitAnimation = true;
                    return;
                }
            }

            //Loop counter
            loopCounter++;
            if (loopCounter >= 20)
                loopCounter = 0;

            //Override normal action
            if (introAnimation)
            {
                IntroAI();
                return;
            }
            else if (wait > 0) //What to do while waiting
            {
                NPC.velocity = Vector2.Zero;
                NPC.noGravity = false;

                wait--;

                //Change animation when attack starts
                if (wait == 0)
                {
                    switch (Action)
                    {
                        case 0:
                            frameX = 9;
                            break;
                        case 1:
                            frameX = 10;
                            break;
                        case 2:
                            frameX = 3;
                            break;
                        case 3:
                            frameX = 1;
                            break;
                        case 4:
                            frameX = 7;
                            break;
                        case 5:
                            frameX = 3;
                            break;
                        case 6:
                            frameX = 5;
                            break;
                        case 7:
                            frameX = 3;
                            break;
                        case 8:
                            frameX = 9;
                            break;
                        case 9:
                            frameX = 0;
                            break;
                        case 10:
                            frameX = 0;
                            break;
                        case 11:
                            frameX = 5;
                            break;
                        case 12:
                            frameX = 5;
                            break;
                    }

                    NPC.frameCounter = 0;
                }
                return;
            }

            //Choose facing direction
            if (actionTimer == 0)
            {
                if ((NPC.Center - player.Center).X > 0)
                    left = false;
                else
                    left = true;
            }

            //Action to make
            switch (Action)
            {
                case 0:
                    NPC.noGravity = false;
                    if (actionTimer == 0)
                    {
                        float xDiff = player.Center.X - NPC.Center.X;
                        NPC.velocity = new Vector2(xDiff / 150, -16);
                    }
                    else
                    {
                        if (NPC.velocity.Y == 0f)
                        {
                            NPC.velocity.X *= 0.8f;
                            frameX = 0;
                        }

                        if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
                        {
                            NPC.velocity.X = 0f;
                        }
                    }

                    if (NPC.velocity == Vector2.Zero)
                    {
                        if (NPC.life < NPC.lifeMax / 2 && Main.rand.Next(4) == 0)
                        {
                            Action = 10;
                            wait = 60;
                        }
                        else if (Main.rand.Next(2) == 0)
                        {
                            Action = 1;
                            wait = 30;
                        }
                        else if (Main.rand.Next(1) == 0)
                        {
                            Action = 6;
                            wait = 20;
                        }
                        else
                        {
                            Action = 7;
                            wait = 20;
                        }

                        actionTimer = -1;
                    }
                    break;
                case 1:
                    if (actionTimer <= 60)
                        dashIndicator = true;
                    else if (actionTimer <= 150)
                    {
                        dashIndicator = false;
                        NPC.velocity = new Vector2(20, 0) * (left ? 1 : -1);

                        //Push out of wall
                        Vector2 sideCheck = left ? NPC.Left : NPC.Right;
                        if (Main.tile[sideCheck.ToTileCoordinates().X, sideCheck.ToTileCoordinates().Y].HasTile)
                        {
                            int counter = 0; //Limit loops to 200 just in case
                            while (Main.tile[sideCheck.ToTileCoordinates().X, sideCheck.ToTileCoordinates().Y].HasTile && counter < 200)
                            {
                                NPC.position.X += (left? 2: -2);
                                sideCheck = left ? NPC.Left : NPC.Right;
                                counter++;
                            }
                        }
                    }
                    if (actionTimer >= 150)
                    {
                        if (Main.rand.Next(3) == 0)
                        {
                            Action = 3;
                            wait = 20;
                        }
                        else if (Main.rand.Next(2) == 0)
                        {
                            Action = 5;
                            wait = 5;
                        }
                        else if (Main.rand.Next(1) == 0)
                        {
                            Action = 6;
                            wait = 20;
                        }
                        else
                        {
                            Action = 7;
                            wait = 20;
                        }

                        actionTimer = -1;
                    }
                    break;
                case 2:
                    if (actionTimer == 0)
                        teleIndicator = true;
                    else if (actionTimer == 90)
                    {
                        Vector2 offset = new Vector2(!left ? NPC.width : -NPC.width, -NPC.height/2.45f);
                        NPC.velocity = ((player.Center + offset) - NPC.Center) / 4;
                        NPC.noGravity = true;
                        NPC.noTileCollide = true;
                        teleIndicator = false;
                    }
                    else if (actionTimer == 94)
                    {
                        NPC.velocity = Vector2.Zero;
                    }
                    else if (actionTimer == 140)
                    {
                        NPC.noGravity = false;
                        NPC.noTileCollide = false;
                        frameX = 1;
                        NPC.frameCounter = 0;
                    }
                    else if (actionTimer >= 160)
                    {
                        //0 1 4 7
                        if (Main.rand.Next(4) == 0)
                        {
                            Action = 0;
                            wait = 20;
                        }
                        else if (Main.rand.Next(3) == 0)
                        {
                            Action = 1;
                            wait = 20;
                        }
                        else if (Main.rand.Next(2) == 0)
                        {
                            Action = 4;
                            wait = 5;
                        }
                        else
                        {
                            Action = 7;
                            wait = 20;
                        }
                        actionTimer = -1;
                    }
                    break;
                case 3:
                    if (actionTimer == 5)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (int i = 0; i < 12; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center + new Vector2(0, -900), Vector2.Zero, ProjectileType<FireballRing>(), damage, 1, Main.myPlayer, (MathHelper.Pi / 6) * i, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                            }
                        }
                    }
                    else if (actionTimer == 60)
                    {
                        if (Main.rand.NextBool(4))
                        {
                            Action = 0;
                            wait = 20;
                        }
                        else if (Main.rand.NextBool(3))
                        {
                            Action = 1;
                            wait = 20;
                        }
                        else if (Main.rand.NextBool(2))
                        {
                            Action = 2;
                            wait = 5;
                        }
                        else
                        {
                            Action = 7;
                            wait = 20;
                        }
                        actionTimer = -1;
                    }
                    break;
                case 4:
                    if (actionTimer > 60)
                        parry = true;
                    if (actionTimer > 180)
                    {
                        parry = false;
                        //TODO chose new action
                        if (Main.rand.Next(3) == 0)
                        {
                            Action = 1;
                            wait = 5;
                        }
                        else if (Main.rand.Next(2) == 0)
                        {
                            Action = 3;
                            wait = 5;
                        }
                        else
                        {
                            Action = 6;
                            wait = 5;
                        }
                        actionTimer = -1;
                    }
                    break;
                case 5:
                    if (Main.netMode != NetmodeID.MultiplayerClient && actionTimer == 0)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            Vector2 vel = new Vector2(0, -14);
                            vel = vel.RotatedBy(MathHelper.ToRadians(-30 + (15 * i)));
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Bottom + new Vector2(left? NPC.width: -NPC.width, 0), vel, ProjectileType<CaraveneFireball>(), damage, 2, Main.myPlayer, 0, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                        }
                    }
                    if (actionTimer == 30)
                    {
                        //TODO: add move selection
                        // 0 2 4 6
                        if (Main.rand.Next(4) == 0)
                        {
                            Action = 0;
                            wait = 20;
                        }
                        else if (Main.rand.Next(3) == 0)
                        {
                            Action = 2;
                            wait = 5;
                        }
                        else if (Main.rand.Next(2) == 0)
                        {
                            Action = 4;
                            wait = 5;
                        }
                        else
                        {
                            Action = 6;
                            wait = 20;
                        }
                        actionTimer = -1;
                    }
                    break;
                case 6:
                    if (actionTimer % 10 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center + new Vector2(Main.rand.NextFloat(-800, 800), -400), Vector2.Zero, ProjectileType<CaraveneBladeProj>(), (int)(damage * 1.5f), 1, Main.myPlayer, 0, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                    }
                    if (actionTimer >= 150)
                    {
                        //0 1 2 3
                        if (Main.rand.Next(4) == 0)
                        {
                            Action = 0;
                            wait = 20;
                        }
                        else if (Main.rand.Next(3) == 0)
                        {
                            Action = 1;
                            wait = 30;
                        }
                        else if (Main.rand.Next(2) == 0)
                        {
                            Action = 2;
                            wait = 30;
                        }
                        else
                        {
                            Action = 3;
                            wait = 20;
                        }
                        actionTimer = -1;
                    }
                    break;
                case 7:
                    if (Main.netMode != NetmodeID.MultiplayerClient && actionTimer == 0)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            Vector2 vel = new Vector2(0, -12);
                            vel = vel.RotatedBy(MathHelper.ToRadians((-10 + (20 * i))) * (left? 1: -1));
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vel, ProjectileType<CaraveneFireball>(), damage, 2, Main.myPlayer, 0, (NPC.life < (NPC.lifeMax / 2)) ? 1 : 0);
                        }
                    }
                    if (actionTimer >= 20)
                    {
                        //1 2 4 6
                        if (Main.rand.Next(4) == 0)
                        {
                            Action = 1;
                            wait = 20;
                        }
                        else if (Main.rand.Next(3) == 0)
                        {
                            Action = 2;
                            wait = 30;
                        }
                        else if (Main.rand.Next(2) == 0)
                        {
                            Action = 4;
                            wait = 30;
                        }
                        else
                        {
                            Action = 6;
                            wait = 20;
                        }

                        actionTimer = -1;
                    }
                    break;
                case 8:
                    NPC.velocity = new Vector2(0, -12);
                    NPC.noGravity = false;
                    if (actionTimer > 60)
                    {
                        //TODO: add move selection
                    }
                    break;
                case 9:
                    NPC.velocity = Vector2.Zero;
                    if (actionTimer < 90)
                    {
                        //Fire
                    }
                    else
                    {
                        //Edn attack
                    }
                    break;
                case 10:
                    break;
                case 11:
                    NPC.velocity = Vector2.Zero;
                    NPC.noGravity = false;
                    if (actionTimer > 120)
                    {

                    }
                    break;
            }

            actionTimer++;
        }

        public override void FindFrame(int frameHeight)
        {
            //Increment speed changed by column
            switch (frameX)
            {
                case 0:
                case 2:
                case 4:
                case 6:
                case 8:
                case 10: 
                    NPC.frameCounter += 2;
                    break;

                case 1:
                case 3:
                case 7:
                    NPC.frameCounter += 5;
                    break;

                case 5:
                case 9:
                    NPC.frameCounter += 3;
                    break;
            }

            if (NPC.frameCounter >= 60)
            {
                NPC.frameCounter = 0;

                switch (frameX)
                {
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 9:
                        frameX--;
                        break;
                }
            }
        }

        //Play intro animation
        public void IntroAI()
        {
            introTicker--;
            if (introTicker <= 0)
                introAnimation = false;
            
            foreach (Player player in Main.player)
            {
                //Set each player's screen target if not set
                if ((player.Center - NPC.Center).Length() < 3000 && player.GetModPlayer<ExoriumPlayer>().ScreenMoveTarget == Vector2.Zero)
                {
                    player.GetModPlayer<ExoriumPlayer>().ScreenMoveTarget = NPC.Center;
                    player.GetModPlayer<ExoriumPlayer>().ScreenMoveTime = introTicker;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.CrimsonKnight + Name).Value;

            if (!left)
            {
                spriteBatch.Draw(tex,
                    new Rectangle((int)(NPC.TopLeft.X - 221) - (int)(screenPos.X), (int)(NPC.TopLeft.Y - 200) - (int)(screenPos.Y), 412, 442),
                    new Rectangle((int)(frameX * 412), (int)(NPC.frameCounter / 10) * 442, 412, 442),
                    drawColor,
                    NPC.rotation,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0);
            }
            else
            {
                spriteBatch.Draw(tex,
                    new Rectangle((int)(NPC.TopLeft.X - 51) - (int)(screenPos.X), (int)(NPC.TopLeft.Y - 205) - (int)(screenPos.Y), 412, 442),
                    new Rectangle((int)(frameX * 412), (int)(NPC.frameCounter / 10) * 442, 412, 442),
                    drawColor,
                    NPC.rotation,
                    Vector2.Zero,
                    SpriteEffects.FlipHorizontally,
                    0);
            }

            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texTele = Request<Texture2D>(AssetDirectory.CrimsonKnight + "TeleportIndicator").Value;

            if (teleIndicator)
                spriteBatch.Draw(texTele, (Main.player[NPC.target].Bottom + new Vector2(!left ? texTele.Width : -texTele.Width, -texTele.Height / 2)) - screenPos, null, Color.Lerp(new Color(0, 0, 0, 0), new Color(255, 255, 255, 255), (float)(-1 * (loopCounter - 30)) / 30f), 0, new Vector2(texTele.Width, texTele.Height) / 2, 1 + (0.02f * loopCounter), !left ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            Texture2D texDash = Request<Texture2D>(AssetDirectory.CrimsonKnight + "DashIndicator").Value;

            if (dashIndicator)
                spriteBatch.Draw(texDash, NPC.Center + (new Vector2(left ? 30 : -30, 0) * actionTimer) - screenPos, null, new Color(255, 255, 255, 0), 0, new Vector2(texDash.Width, texDash.Height) / 2, 1, left ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            base.PostDraw(spriteBatch, screenPos, drawColor);
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

        public override bool? CanFallThroughPlatforms()
        {
            if (Main.tile[NPC.Bottom.ToTileCoordinates().X, NPC.Bottom.ToTileCoordinates().Y].HasTile ||
                Main.tile[NPC.Bottom.ToTileCoordinates().X, NPC.Bottom.ToTileCoordinates().Y - 1].HasTile ||
                Main.tile[NPC.Bottom.ToTileCoordinates().X, NPC.Bottom.ToTileCoordinates().Y - 2].HasTile ||
                Main.tile[NPC.Bottom.ToTileCoordinates().X, NPC.Bottom.ToTileCoordinates().Y + 1].HasTile)
                return false;
            return true;
        }
    }
}
