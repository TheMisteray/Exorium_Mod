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
using Terraria.Graphics.Shaders;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Localization;
using Terraria.Audio;
using Steamworks;

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    internal class CaravenePhaseTransition : ModNPC
    {
        public override string Texture => AssetDirectory.CrimsonKnight + "Caravene" + "_Hitbox";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crimson Knight");
            Main.npcFrameCount[NPC.type] = 6;

            //Always draw so visuals don't fail while offscreen
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true;

            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            NPCID.Sets.TeleportationImmune[Type] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 200;
            NPC.damage = 29;
            NPC.defense = 11;
            NPC.knockBackResist = 0f;
            NPC.width = 140;
            NPC.height = 240;
            NPC.value = 0;
            NPC.npcSlots = 30f;
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.timeLeft = NPC.activeTime * 30;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.dontTakeDamage = true;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.75 * balance);
            NPC.damage = (int)(NPC.damage * 0.7);
        }

        public bool left
        {
            get => NPC.ai[0] == 1f;
            set => NPC.ai[0] = value ? 1f : 0f;
        }

        public float time
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }

        private float counter = 0;
        private bool invisible = false;
        private float frameX = 0;

        public override void AI()
        {
            #region Targeting  
            if (Main.netMode != NetmodeID.MultiplayerClient)
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
                    return;
                }
            }
            #endregion

            if (Math.Abs(NPC.Center.X - player.Center.X) > 100)
            {
                if ((NPC.Center - player.Center).X > 0)
                    left = false;
                else
                    left = true;
            }

            counter++; //For animations
            if (counter >= 60)
            {
                counter = 0;
                if (frameX == 5) //Jank way to keep animations smooth
                    frameX = 4;
            }

            time++;
            if (time == 1)
            {
                AdvancedPopupRequest popupRequest = new AdvancedPopupRequest();
                popupRequest.Color = Color.Red;
                if (Core.Systems.DownedBossSystem.killedCrimsonKnight)
                    popupRequest.Text = "...";
                else if (Core.Systems.DownedBossSystem.dueledCrimsonKnight)
                    popupRequest.Text = "Ugh";
                else
                    popupRequest.Text = "Seriously?";
                popupRequest.DurationInFrames = 90;
                PopupText.NewText(popupRequest, NPC.Center + new Vector2(0, -NPC.width));
            }
            else if (time == 90)
            {
                if (Core.Systems.DownedBossSystem.killedCrimsonKnight)
                    time = 269;
                else
                {
                    AdvancedPopupRequest popupRequest = new AdvancedPopupRequest();
                    popupRequest.Color = Color.Red;
                    if (Core.Systems.DownedBossSystem.dueledCrimsonKnight)
                        popupRequest.Text = "I shouldn't have expected anything...";
                    else
                        popupRequest.Text = "So that's how it is then";
                    popupRequest.DurationInFrames = 90;
                    PopupText.NewText(popupRequest, NPC.Center + new Vector2(0, -NPC.width));
                }
            }
            else if (time == 180)
            {
                if (Core.Systems.DownedBossSystem.dueledCrimsonKnight)
                    time = 269;
                else
                {
                    AdvancedPopupRequest popupRequest = new AdvancedPopupRequest();
                    popupRequest.Color = Color.Red;
                    popupRequest.Text = "Well... You asked for it";
                    popupRequest.DurationInFrames = 90;
                    PopupText.NewText(popupRequest, NPC.Center + new Vector2(0, -NPC.width));
                }
            }
            else if (time == 270)
            {
                frameX = 5;
                counter = 0;
            }
            else if (time > 270 && time < 330)
            {
                counter++;
            }
            else if (time == 330)
            {
                SoundEngine.PlaySound(SoundID.Roar, NPC.position);
            }
            else if (time > 330 && time < 510)
            {
                counter++;
                for (int i = 0; i < 20; i++)
                {
                    Vector2 rad = new Vector2(0, Main.rand.NextFloat(30));
                    Vector2 shootPoint = rad.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi));
                    Dust dust = Dust.NewDustPerfect(NPC.Center, DustID.SolarFlare, shootPoint, 1, default, 1 + Main.rand.NextFloat(-.5f, .5f));
                    dust.noGravity = true;
                    dust.color = new Color(184, 58, 24);
                }
            }
            else if (time == 510)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Bottom.Y, NPCType<ExoriumRed>(), 0, 6);
                if (!Core.Systems.DownedBossSystem.dueledCrimsonKnight)
                {
                    Core.Systems.DownedBossSystem.dueledCrimsonKnight = true;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                    }
                }
                NPC.active = false;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.CrimsonKnight + "Caravene").Value;

            int ySourceHeight = (int)(counter / 10) * 442;
            int xSourceHeight = (int)(frameX * 412);
            if (! invisible)
            {
                if (!left)
                {
                    spriteBatch.Draw(tex,
                        new Rectangle((int)(NPC.TopLeft.X - 221) - (int)(screenPos.X), (int)(NPC.TopLeft.Y - 200) - (int)(screenPos.Y), 412, 442),
                        new Rectangle(xSourceHeight, ySourceHeight, 412, 442),
                        drawColor,
                        NPC.rotation,
                        Vector2.Zero,
                        SpriteEffects.None,
                        0);
                }
                else
                {
                    spriteBatch.Draw(tex,
                        new Rectangle((int)(NPC.TopLeft.X - 51) - (int)(screenPos.X), (int)(NPC.TopLeft.Y - 200) - (int)(screenPos.Y), 412, 442),
                        new Rectangle(xSourceHeight, ySourceHeight, 412, 442),
                        drawColor,
                        NPC.rotation,
                        Vector2.Zero,
                        SpriteEffects.FlipHorizontally,
                        0);
                }
            }
            return false;
        }

        public override bool CanHitNPC(NPC target)/* tModPorter Suggestion: Return true instead of null */
        {
            return false;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }
    }
}
