using ExoriumMod.Content.Biomes;
using ExoriumMod.Core;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.NPCs.Enemies.Deadlands
{
    class WightWarrior : ModNPC
    {
        public override string Texture => AssetDirectory.EnemyNPC + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wight Warrior");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.ArmoredSkeleton];
        }

        public override void SetDefaults()
        {
            NPC.width = 23;
            NPC.height = 40;
            NPC.damage = 19;
            NPC.defense = 8;
            NPC.lifeMax = 160;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.value = 160f;
            NPC.knockBackResist = .2f;
            NPC.aiStyle = 3;
            AIType = NPCID.ArmoredSkeleton;
            NPC.buffImmune[BuffID.Confused] = false;
            AnimationType = NPCID.ArmoredSkeleton;
        }

        public override void OnKill()
        {
            if (Main.rand.NextBool(3))
                Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ItemType<Items.Materials.WightBone>());
            if (Main.rand.NextBool(40))
                Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ItemType<Items.Accessories.BlightedManacle>());
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<Items.Materials.WightBone>(), 3));
            npcLoot.Add(ItemDropRule.Common(ItemType<Items.Accessories.BlightedManacle>(), 40));

            base.ModifyNPCLoot(npcLoot);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("The word \"Wight\" meant \"Person\" in days long since past. The name now refers to evil undead who were once mortals, their passing likely connected to the fate of the land they reside in."),
                new BestiaryPortraitBackgroundProviderPreferenceInfoElement(ModContent.GetInstance<DeadlandBiome>().ModBiomeBestiaryInfoElement),
            });
        }
    }
}
