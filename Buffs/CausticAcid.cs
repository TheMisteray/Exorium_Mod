using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Buffs
{
    class CausticAcid : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Caustic Acid");
            Description.SetDefault("Your flesh boils and burns");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCs.ExoriumGlobalNPC>().cAcid = true;
        }
    }
}
