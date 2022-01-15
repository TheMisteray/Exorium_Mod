using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Buffs
{
    class Shield : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Shield");
            Description.SetDefault("Increased Defense");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 5;
            player.GetModPlayer<ExoriumPlayer>().shield = true;
        }
    }
}
