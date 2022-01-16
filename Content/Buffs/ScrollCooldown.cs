using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Buffs
{
    class ScrollCooldown : ModBuff
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = AssetDirectory.Buff + Name;
            return base.Autoload(ref name, ref texture);
        }

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
