using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ExoriumMod.Content.Buffs
{
    class ConsumingDark : ModBuff
    {
        public override string Texture => AssetDirectory.Buff + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Consuming Dark");
            Description.SetDefault("The darkness drains the life from you");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<ExoriumGlobalNPC>().cDark = true;
        }
    }
}
