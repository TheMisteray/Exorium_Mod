using System.Collections.Generic;
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
                pool.Add(NPCType<Content.NPCs.Enemies.Deadlands.WightArcher>(), .07f);
                pool.Add(NPCType<Content.NPCs.Enemies.Deadlands.WightWarrior>(), .1f);
                pool.Add(NPCType<Content.NPCs.Enemies.Deadlands.Poltergeist>(), .03f);
            }
        }
    }
}
