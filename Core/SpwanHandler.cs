using System.Collections.Generic;
using ExoriumMod.Content.NPCs.Enemies;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Core
{
    public partial class ExoriumGlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (Main.LocalPlayer.GetModPlayer<BiomeHandler>().ZoneDeadlands)
            {
                pool.Clear();
                pool.Add(NPCType<WightArcher>(), .1f);
                pool.Add(NPCType<WightWarrior>(), .1f);
                pool.Add(NPCType<Poltergeist>(), .02f);
            }
        }
    }
}
