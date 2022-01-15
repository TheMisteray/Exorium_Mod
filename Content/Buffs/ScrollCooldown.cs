using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Buffs
{
    class ScrollCooldown : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Scroll Cooldown");
            Description.SetDefault("You cannot use Spell Scrolls");
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            canBeCleared = false;
            //Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<ExoriumPlayer>().scrollCooldown = true;
        }
    }
}
