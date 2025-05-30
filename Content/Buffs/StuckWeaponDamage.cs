﻿using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Buffs
{
    class ThrowingRapierDamage : ModBuff
    {
        public override string Texture => AssetDirectory.Invisible;

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<ExoriumGlobalNPC>().stuckByRapier = true;
		}
	}

    class RimeIcicleDamage : ModBuff
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<ExoriumGlobalNPC>().stuckByIcicle = true;
        }
    }
}
