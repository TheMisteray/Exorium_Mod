using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader.Utilities;

namespace ExoriumMod.Content.Bosses.GemsparklingHive
{
    class SuspiciousLookingGeode : ModNPC
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 150;
            NPC.damage = 0;
            NPC.defense = 7;
            NPC.knockBackResist = 0;
            NPC.width = 64;
            NPC.height = 80;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCHit41;
            NPC.timeLeft = NPC.activeTime * 30;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
        }

        public override void OnKill()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
                NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Bottom.Y, NPCType<GemsparklingHive>());
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>(Name + "_gore1").Type, NPC.scale);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>(Name + "_gore2").Type, NPC.scale);
            base.OnKill();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Cavern.Chance * .008f;
        }
    }
}