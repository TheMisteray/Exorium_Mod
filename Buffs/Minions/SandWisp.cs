using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
namespace ExoriumMod.Buffs.Minions
{
    class SandWisp : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Sand Wisp");
            Description.SetDefault("The sand wisp will fight for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ProjectileType<Projectiles.Minions.SandWisp>()] > 0)
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
