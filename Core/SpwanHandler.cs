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
            if (spawnInfo.Player.InModBiome(GetInstance<Content.Biomes.DeadlandBiome>()))
            {
                if (pool.ContainsKey(0))
                {
                    pool[0] = 0f;
                }
            }
        }
    }
}
