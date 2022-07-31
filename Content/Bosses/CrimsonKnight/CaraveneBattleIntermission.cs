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
            NPC.lifeMax = 99;
            NPC.damage = 29;
            NPC.defense = 11;
            NPC.knockBackResist = 0f;
            NPC.width = 140;
            NPC.height = 240;
            NPC.value = Item.buyPrice(0, 7, 7, 7);
            NPC.npcSlots = 30f;
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath52;
            NPC.timeLeft = NPC.activeTime * 30;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
        }

        public bool left
        {
            get => NPC.ai[0] == 1f;
            set => NPC.ai[0] = value ? 1f : 0f;
        }

        public override string GetChat()
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    return ".";
                case 1:
                    return ".";
                default:
                    return ".";
            }
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = true;
            }
        }

        public override bool CanGoToStatue(bool toKingStatue)
        {
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.CrimsonKnight + Name).Value;

            int ySourceHeight = (int)(NPC.frameCounter / 10) * 442;
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
    }
}
