using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ExoriumMod.Content.Buffs
{
    class Shield : ModBuff
    {
        public override string Texture => AssetDirectory.Buff + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shield");
            Description.SetDefault("Increased Defense");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 5;
            player.GetModPlayer<ExoriumPlayer>().shield = true;
        }
    }
}
