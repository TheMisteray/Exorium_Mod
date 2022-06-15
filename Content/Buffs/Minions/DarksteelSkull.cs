using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Buffs.Minions
{
    class DarksteelSkull : ModBuff
    {
        public override string Texture => AssetDirectory.MinionBuff + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mordite Skull");
            Description.SetDefault("The Darksteel skull will fight for you \n" +
                "Each skull gives +3% summon damage");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ProjectileType<Projectiles.Minions.DarksteelSkullSummon>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
                player.GetDamage(DamageClass.Summon) += player.ownedProjectileCounts[ProjectileType<Projectiles.Minions.DarksteelSkullSummon>()] * .03f;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
