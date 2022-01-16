using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Buffs
{
    class Shield : ModBuff
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = AssetDirectory.Buff + name;
            return base.Autoload(ref name, ref texture);
        }

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
