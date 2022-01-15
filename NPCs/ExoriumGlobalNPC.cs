using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.NPCs
{

    class ExoriumGlobalNPC : GlobalNPC
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

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (cDark)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, ModContent.DustType<Dusts.Shadow>(), npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    if (Main.rand.NextBool(4))
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }
                drawColor = new Color(60, 60, 60);
            }
            if (cAcid)
            {
                if (Main.rand.Next(4) < 2)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, DustID.Water, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, Color.Orange);
                    Main.dust[dust].noGravity = false;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y += 0.5f;
                    Main.dust[dust].scale *= 0.5f;
                }
                drawColor = new Color(160, 200, 0);
            }
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {            
            if (Main.LocalPlayer.GetModPlayer<ExoriumPlayer>().ZoneDeadlands)
            {
                pool.Clear();
                pool.Add(NPCType<NPCs.Enemies.WightArcher>(), .1f);
                pool.Add(NPCType<NPCs.Enemies.WightWarrior>(), .1f);
                pool.Add(NPCType<NPCs.Enemies.Poltergeist>(), .02f);
            }
        }
    }
}
