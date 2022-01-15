using ExoriumMod.Tiles;
using ExoriumMod.Walls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using static Terraria.ModLoader.ModContent;
using System;

namespace ExoriumMod.Core
{
    public partial class ExoriumGlobalNPC
    {
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
