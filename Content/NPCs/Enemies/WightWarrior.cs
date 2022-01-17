using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.NPCs.Enemies
{
    class WightWarrior : ModNPC
    {
        public override string Texture => AssetDirectory.EnemyNPC + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wight Warrior");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ArmoredSkeleton];
        }

        public override void SetDefaults()
        {
            npc.width = 23;
            npc.height = 40;
            npc.damage = 32;
            npc.defense = 10;
            npc.lifeMax = 210;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.value = 160f;
            npc.knockBackResist = .2f;
            npc.aiStyle = 3;
            aiType = NPCID.ArmoredSkeleton;
            npc.buffImmune[BuffID.Confused] = false;
            animationType = NPCID.ArmoredSkeleton;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.player.GetModPlayer<BiomeHandler>().ZoneDeadlands ? .1f : 0f;
        }

        public override void NPCLoot()
        {
            if (Main.rand.NextBool(3))
                Item.NewItem(npc.getRect(), ItemType<Items.Materials.WightBone>());
            if (Main.rand.NextBool(40))
                Item.NewItem(npc.getRect(), ItemType<Items.Accessories.BlightedManacle>());
        }
    }
}
