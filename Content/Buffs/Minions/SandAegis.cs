using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Buffs.Minions
{
    class SandAegis : ModBuff
    {
        public override string Texture => AssetDirectory.MinionBuff + Name;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sand Aegis");
            /* Description.SetDefault("The sand aegis will protect you \n" +
                "Sand aegis grant 1 defense each"); */
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ProjectileType<Projectiles.Minions.SandAegis>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
                player.statDefense += player.ownedProjectileCounts[ProjectileType<Projectiles.Minions.SandAegis>()];
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
