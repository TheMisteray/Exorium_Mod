using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Buffs.Minions
{
    class DancingSwordSummonBuff : ModBuff
    {
        public override string Texture => AssetDirectory.MinionBuff + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("A Dancing Sword");
            Description.SetDefault("The Dancing Sword has a mind of its own");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ProjectileType<Projectiles.Minions.DancingSwordSummon>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
