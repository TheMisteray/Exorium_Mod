using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ExoriumMod.Content.Buffs
{
    class ScrollCooldown : ModBuff
    {
        public override string Texture => AssetDirectory.Buff + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scroll Cooldown");
            Description.SetDefault("You cannot use Spell Scrolls");
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<ExoriumPlayer>().scrollCooldown = true;
        }
    }
}
