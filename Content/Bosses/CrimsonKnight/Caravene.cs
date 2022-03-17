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
    }
}
