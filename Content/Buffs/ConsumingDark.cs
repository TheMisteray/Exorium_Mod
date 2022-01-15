using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Buffs
{
    class ConsumingDark : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Consuming Dark");
            Description.SetDefault("The darkness drains the life from you");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCs.ExoriumGlobalNPC>().cDark = true;
        }
    }
}
