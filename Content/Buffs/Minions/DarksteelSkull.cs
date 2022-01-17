using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Buffs.Minions
{
    class DarksteelSkull : ModBuff
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = AssetDirectory.MinionBuff + name;
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
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
                player.minionDamage += player.ownedProjectileCounts[ProjectileType<Projectiles.Minions.DarksteelSkullSummon>()] * .03f;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
