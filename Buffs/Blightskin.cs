using Terraria;
using Terraria.ModLoader;

namespace ExoriumMod.Buffs
{
    class Blightskin : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Blightskin");
            Description.SetDefault("Defense at the cost of regeneration");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            canBeCleared = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen -= 4;
            player.statDefense += 12;
        }
    }
}
