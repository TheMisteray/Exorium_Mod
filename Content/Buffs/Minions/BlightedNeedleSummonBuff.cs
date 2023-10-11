﻿using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Buffs.Minions
{
    class BlightedNeedleSummonBuff : ModBuff
    {
        public override string Texture => AssetDirectory.MinionBuff + Name;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Blightsteel Needle");
            // Description.SetDefault("The Blightsteel Needle will fight for you \n");
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
        public override string Texture => AssetDirectory.MinionBuff + "BlightedNeedleSummonBuff";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Stuck");
            // Description.SetDefault("Ouch");
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<ExoriumGlobalNPC>().stuckByNeedles = true;
        }
    }
}
