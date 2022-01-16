using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Buffs.Minions
{
    class BlightSlime : ModBuff
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = AssetDirectory.MinionBuff + name;
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Mini Blighted Slime");
            Description.SetDefault("The mini blightslime will fight for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ProjectileType<Projectiles.Minions.BlightSlime>()] > 0)
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
