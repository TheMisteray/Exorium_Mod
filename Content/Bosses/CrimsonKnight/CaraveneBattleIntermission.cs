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

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    internal class CaraveneBattleIntermission : ModNPC
    {
        public override string Texture => AssetDirectory.CrimsonKnight + "Caravene" + "_Hitbox";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crimson Knight");
            Main.npcFrameCount[NPC.type] = 6;

            //Always draw so visuals don't fail while offscreen
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;

            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.OnFire,
                    BuffID.OnFire3,
                }
            };
            NPCID.Sets.DebuffImmunitySets[Type] = debuffData;

            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            NPCID.Sets.TeleportationImmune[Type] = true;
            NPCID.Sets.ActsLikeTownNPC[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 333;
            NPC.damage = 29;
            NPC.defense = 11;
            NPC.knockBackResist = 0f;
            NPC.width = 140;
            NPC.height = 240;
            NPC.value = 0;
            NPC.npcSlots = 30f;
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath52;
            NPC.timeLeft = NPC.activeTime * 30;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.75 * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 0.7);
        }

        public bool left
        {
            get => NPC.ai[0] == 1f;
            set => NPC.ai[0] = value ? 1f : 0f;
        }

        private float counter = 0;
        private float aliveCounter = 180;

        public override void AI()
        {
            #region Targeting  
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

            if (aliveCounter > 0)
                aliveCounter--;
            counter++;
            if (counter >= 60)
                counter = 0;
        }

        public override string GetChat()
        {
            return "I must say that was well fought. I'm willing to call a truce here to a warrior like you, what do you say?";
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Accept";
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                //TODO: add bag dropping, netsend truce data, and exit animation
            }
        }

        public override bool CanGoToStatue(bool toKingStatue)
        {
            return false;
        }

        public override bool CanChat()
        {
            return true;
        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.CrimsonKnight + "Caravene").Value;

            int ySourceHeight = (int)(counter / 10) * 442;
            int xSourceHeight = (int)(0 * 412);

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
                    new Rectangle((int)(NPC.TopLeft.X - 51) - (int)(screenPos.X), (int)(NPC.TopLeft.Y - 205) - (int)(screenPos.Y), 412, 442),
                    new Rectangle(xSourceHeight, ySourceHeight, 412, 442),
                    drawColor,
                    NPC.rotation,
                    Vector2.Zero,
                    SpriteEffects.FlipHorizontally,
                    0);
            }

            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (aliveCounter != 0)
                return false;
            return false;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            if (aliveCounter != 0)
                return false;
            return base.CanBeHitByItem(player, item);
        }
    }
}
