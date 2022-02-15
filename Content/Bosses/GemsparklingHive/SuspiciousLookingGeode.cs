using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Content.Bosses.GemsparklingHive
{
    class SuspiciousLookingGeode : ModNPC
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 150;
            npc.damage = 0;
            npc.defense = 7;
            npc.knockBackResist = 0;
            npc.width = 64;
            npc.height = 80;
            npc.HitSound = SoundID.NPCHit41;
            npc.DeathSound = SoundID.NPCHit41;
            npc.timeLeft = NPC.activeTime * 30;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }

        public override void NPCLoot()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
                NPC.NewNPC((int)npc.Center.X, (int)npc.Bottom.Y, NPCType<GemsparklingHive>());
            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GemsparklingHive/" + Name + "_gore1"), npc.scale);
            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GemsparklingHive/" + Name + "_gore2"), npc.scale);
            base.NPCLoot();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.ZoneRockLayerHeight)
            {
                return .005f;
            }
            return 0;
        }
    }
}
