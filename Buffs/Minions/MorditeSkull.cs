using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Buffs.Minions
{
    class MorditeSkull : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Mordite Skull");
            Description.SetDefault("The mordite skull will fight for you \n" +
                "Each skull gives +3% summon damage");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ProjectileType<Projectiles.Minions.MorditeSkullSummon>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
                player.minionDamage += player.ownedProjectileCounts[ProjectileType<Projectiles.Minions.MorditeSkullSummon>()] * .03f;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
