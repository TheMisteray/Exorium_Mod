using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ExoriumMod.Content.Buffs
{
    class CausticAcid : ModBuff
    {
        public override string Texture => AssetDirectory.Buff + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Caustic Acid");
            Description.SetDefault("Your flesh boils and burns");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<ExoriumGlobalNPC>().cAcid = true;
        }
    }
}
