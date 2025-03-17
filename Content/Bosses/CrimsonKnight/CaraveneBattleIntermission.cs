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
using Mono.Cecil;

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    internal class CaraveneBattleIntermission : ModNPC
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
            NPCID.Sets.ActsLikeTownNPC[Type] = true;
            NPCID.Sets.NoTownNPCHappiness[Type] = true;
            NPCID.Sets.HasNoPartyText[Type] = true;

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

        public float invulnerableTime
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }

        public bool interactedWith
        {
            get => NPC.ai[2] == 1f;
            set => NPC.ai[2] = value ? 1f : 0f;
        }

        public bool Despawn
        {
            get => NPC.ai[2] == 1f;
            set => NPC.ai[2] = value ? 1f : 0f;
        }

        private float counter = 0;
        private float despawnTime;
        private bool invisible = false;
        private float portalSize;
        private float bubbleAnimationTimer = 0;

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
                if (player.dead || (NPC.position - player.position).Length() > 8000)
                {
                    //TODO: this still will use the same exit if the player is far
                    NPC.EncourageDespawn(120);
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

            if (invulnerableTime > 0 || Despawn)
            {
                invulnerableTime--;
                NPC.dontTakeDamage = true;
            }
            else
                NPC.dontTakeDamage = false;

            counter++; //For animations
            if (counter >= 60)
                counter = 0;

            if (invulnerableTime > 5000)
                Despawn = true;

            if (Despawn)
            {
                despawnTime++;
                if (despawnTime == 10)
                    NPC.DropItemInstanced(NPC.position, new Vector2(NPC.width, NPC.height), ItemType<CrimsonKnightBag>(), 1);
                if (despawnTime == 30)
                {
                    //Say stuff
                    AdvancedPopupRequest popupRequest = new AdvancedPopupRequest();
                    popupRequest.Color = Color.Red;
                    if (Core.Systems.DownedBossSystem.killedCrimsonKnight)
                        popupRequest.Text = "...";
                    else if (Core.Systems.DownedBossSystem.dueledCrimsonKnight)
                        popupRequest.Text = "Good to see you've come to your senses";
                    else
                        popupRequest.Text = "It was a good fight!";
                    popupRequest.DurationInFrames = 90;
                    PopupText.NewText(popupRequest, NPC.Center + new Vector2(0, -NPC.width));
                }
                else if (despawnTime == 120)
                {
                    //Say stuff
                    if (Core.Systems.DownedBossSystem.killedCrimsonKnight)
                        despawnTime = 149;
                    else
                    {
                        AdvancedPopupRequest popupRequest = new AdvancedPopupRequest();
                        popupRequest.Color = Color.Red;
                        if (Core.Systems.DownedBossSystem.dueledCrimsonKnight)
                            popupRequest.Text = "I'm outta here";
                        else
                            popupRequest.Text = "Maybe we'll cross paths again sometime";
                        popupRequest.DurationInFrames = 90;
                        PopupText.NewText(popupRequest, NPC.Center + new Vector2(0, -NPC.width));
                    }
                }
                else if (despawnTime == 210)
                {
                    invisible = true;
                }
                else if (despawnTime == 270)
                {
                    NPC.active = false;
                }
            }
            else
            {
                bubbleAnimationTimer++;
            }
        }

        public override string GetChat()
        {
            if (!Despawn)
            {
                if (Core.Systems.DownedBossSystem.killedCrimsonKnight)
                    return "...";
                else if (Core.Systems.DownedBossSystem.dueledCrimsonKnight)
                    return "Against my better judgement, I'm giving you one more chance to call it a draw here.";
                else if (Core.Systems.DownedBossSystem.trucedCrimsonKnight)
                    return "Haha! Another great fight! Truce?";
                else
                    return "Okay! Okay!!! You can keep the crown... Say, you're pretty good. How about we call this a draw for now?";
            }
            return "";
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (!Despawn)
                button = "Accept Truce";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton && !Despawn)
            {
                Despawn = true;
                invulnerableTime = 9999;//Use this as alternative way of communicating despawn over net
                SoundEngine.PlaySound(SoundID.MenuClose, NPC.Center);
                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI);
                NPC.netUpdate = true;

                //Update
                if (!Core.Systems.DownedBossSystem.trucedCrimsonKnight)
                {
                    Core.Systems.DownedBossSystem.trucedCrimsonKnight = true;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                    }
                }
            }

            if (Main.netMode != NetmodeID.Server)
            {
                Main.npcChatCornerItem = 0;
                Main.editSign = false;
                Main.npcChatText = "";
                Main.LocalPlayer.releaseMount = false;
            }
        }

        public override bool CanGoToStatue(bool toKingStatue)
        {
            return false;
        }

        public override bool CanChat()
        {
            return !Despawn;
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.CrimsonKnight + "Caravene").Value;
            Texture2D speechBubble = Request<Texture2D>(AssetDirectory.CrimsonKnight + "SpeechBubble").Value;

            int ySourceHeight = (int)(counter / 10) * 442;
            int xSourceHeight = (int)(0 * 412);
            if (! invisible)
            {
                if (!interactedWith && !Despawn)
                {
                    Main.EntitySpriteDraw(speechBubble, NPC.Center + new Vector2(0, -150 + Math.Min(0, (float)Math.Sin(bubbleAnimationTimer/25) * 10)) - screenPos, null, Color.White, 0, new Vector2(speechBubble.Width / 2, speechBubble.Height / 2), 1, SpriteEffects.None);
                }

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

            //Portal
            if (Despawn && despawnTime >= 150)
            {
                var portal = Filters.Scene["ExoriumMod:VioletPortal"].GetShader().Shader;
                portal.Parameters["sampleTexture2"].SetValue(Request<Texture2D>(AssetDirectory.ShaderMap + "PortalMap").Value);
                portal.Parameters["uTime"].SetValue(Main.GameUpdateCount * 0.02f);
                portal.Parameters["uProgress"].SetValue(Main.GameUpdateCount * .003f);

                Texture2D texPortal = Request<Texture2D>(AssetDirectory.ShaderMap + "Portal").Value;
                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.NonPremultiplied, default, default, default, portal, Main.GameViewMatrix.ZoomMatrix);

                if (despawnTime < 210)
                    portalSize += .02f;
                else
                    portalSize -= .02f;

                if (portalSize > 0)
                    spriteBatch.Draw(texPortal, NPC.Center - screenPos, null, new Color(255, 255, 255, 0), Main.GameUpdateCount * .01f, texPortal.Size() / 2, 3f * portalSize, SpriteEffects.None, 0);

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.Additive, SamplerState.PointWrap, default, default, default, Main.GameViewMatrix.ZoomMatrix);
            }

            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }

        public override bool CanHitNPC(NPC target)/* tModPorter Suggestion: Return true instead of null */
        {
            return false;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }

        public override void OnKill()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
                NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Bottom.Y, NPCType<CaravenePhaseTransition>());
        }
    }
}
