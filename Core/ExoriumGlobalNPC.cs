﻿using System.Collections.Generic;
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
        public bool stuckByRapier;

        public override void ResetEffects(NPC npc)
        {
            cDark = false;
            cAcid = false;
            stuckByNeedles = false;
            stuckByRapier = false;
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
            if (stuckByRapier)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                int stuckCount = 0;
                for (int i = 0; i < 1000; i++)
                {
                    Projectile p = Main.projectile[i];
                    if (p.active && p.type == ProjectileType<Content.Items.Weapons.Ranger.ThrowingRapierProj>() && p.ai[1] == npc.whoAmI)
                    {
                        if (p.ModProjectile is Content.Items.Weapons.Ranger.ThrowingRapierProj proj && proj.IsStickingToTarget)
                            stuckCount++;
                    }
                }
                npc.lifeRegen -= stuckCount * 2 * 5;
                if (damage < stuckCount * 5)
                {
                    damage = stuckCount * 5;
                }
            }
        }
    }
}
