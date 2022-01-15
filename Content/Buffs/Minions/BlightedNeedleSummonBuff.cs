using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Buffs.Minions
{
    class BlightedNeedleSummonBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Blightsteel Needle");
            Description.SetDefault("The Blightsteel Needle will fight for you \n");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ProjectileType<Projectiles.Minions.BlightedNeedleSummon>()] > 0)
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

    class StuckBlightedNeedleDebuff : ModBuff
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            // NPC only buff so we'll just assign it a useless buff icon.
            texture = "ExoriumMod/Buffs/Minions/BlightedNeedleSummonBuff";
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Stuck");
            Description.SetDefault("Ouch");
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCs.ExoriumGlobalNPC>().stuckByNeedles = true;
        }
    }
}
