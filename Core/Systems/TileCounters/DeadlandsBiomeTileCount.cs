using System;
using ExoriumMod.Content.Tiles;
using Terraria.ModLoader;

namespace ExoriumMod.Core.Systems.TileCounters
{
    public class DeadlandsBiomeTileCount : ModSystem
    {
        public int deadlandsBlockCount;

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            deadlandsBlockCount = tileCounts[ModContent.TileType<AshenDustTile>()];
        }
    }
}
