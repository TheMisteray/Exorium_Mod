using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Core
{
    partial class ExoriumGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool cDark;
        public bool cAcid;
        public bool stuckByNeedles;

        public override void ResetEffects(NPC npc)
        {
            cDark = false;
            cAcid = false;
            stuckByNeedles = false;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (cDark)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 154;
                if (damage < 7)
                {
                    damage = 7;
                }
            }
            if (cAcid)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 20;
                if (damage < 1)
                {
                    damage = 3;
                }
            }
            if (stuckByNeedles)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 30;
                if (damage < 2)
                {
                    damage = 2;
                }
            }
        }
    }
}
