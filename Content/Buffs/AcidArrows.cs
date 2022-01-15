using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Buffs
{
    class AcidArrows : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Acid Arrows");
            Description.SetDefault("Your wooden arrows inflict acid");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<ExoriumPlayer>().acidArrows = true;
        }
    }
}
