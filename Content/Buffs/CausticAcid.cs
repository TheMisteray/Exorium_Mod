using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Buffs
{
    class CausticAcid : ModBuff
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = AssetDirectory.Buff + name;
            return base.Autoload(ref name, ref texture);
        }

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
            npc.GetGlobalNPC<ExoriumGlobalNPC>().cAcid = true;
        }
    }
}
