using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using System;
using ExoriumMod.Content.Tiles;
using ExoriumMod.Content.Walls;
using Terraria.WorldBuilding;
using Terraria.IO;

namespace ExoriumMod.Core
{
    public partial class ExoriumWorld : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (ShiniesIndex != -1)
            {
                tasks.Insert(ShiniesIndex + 1, new PassLegacy("Exorium Mod Ores", ExoriumOreGeneration));
            }

            int DirtRockWallRunnerIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Dirt Rock Wall Runner"));
            if (DirtRockWallRunnerIndex != -1) //Right before living trees
            {
                tasks.Insert(DirtRockWallRunnerIndex, new PassLegacy("Deadlands", delegate (GenerationProgress progress, GameConfiguration config)
                  {
                      progress.Message = "Generating Deadlands";
                      WorldGeneration.Biomes.ExoriumBiomes.DeadlandsGeneration();
                  }));
            }

            int PlaceFallenLogIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Place Fallen Log"));
            if (PlaceFallenLogIndex != -1) //Right before fallen logs
            {
                tasks.Insert(PlaceFallenLogIndex + 1, new PassLegacy("Smoothing Deadlands", delegate (GenerationProgress progress, GameConfiguration config)
                {
                    progress.Message = "Smoothing Deadlands";
                    WorldGeneration.Biomes.ExoriumBiomes.FinalizeDeadlands();
                }));
            }

            int FinalCleanupIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Tile Cleanup"));
            if (FinalCleanupIndex != -1)
            {
                tasks.Insert(FinalCleanupIndex + 1, new PassLegacy("Exorium Structures", delegate (GenerationProgress progress, GameConfiguration config)
               {
                   progress.Message = "Generating Exorium Structures";
                   WorldGeneration.Structures.ExoriumStructures.ShadowHouse();
                   WorldGeneration.Structures.ExoriumStructures.FillCrates();
                   WorldGeneration.Structures.ExoriumStructures.FallenTower();
               }));
            }
        }

        private void ExoriumOreGeneration(GenerationProgress progress, GameConfiguration config)
        {
            progress.Message = "Generating Exorium Ores";
            //BlightSteel spawn
            for (int i = 0; i < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 12E-05); i++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)GenVars.rockLayer, Main.maxTilesY); //Spawn in cavern layer
                Tile tile = Framing.GetTileSafely(x, y);
                if (tile.HasTile && tile.TileType == TileID.Stone)
                {
                    WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(2, 3), WorldGen.genRand.Next(1, 3), TileType<BlightedOreTile>(), false, 0f, 0f, false, true);
                }
            }
            //RimeStone spawn
            for (int i = 0; i < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 5E-04); i++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, Main.maxTilesY);
                Tile tile = Framing.GetTileSafely(x, y);
                if (tile.HasTile && tile.TileType == TileID.IceBlock || tile.TileType == TileID.SnowBlock)
                {
                    WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(6, 7), WorldGen.genRand.Next(3, 5), TileType<RimeStoneTile>(), false, 0f, 0f, false, true);
                }
            }
            //DuneStone spawn
            for (int i = 0; i < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-04); i++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, Main.maxTilesY);
                Tile tile = Framing.GetTileSafely(x, y);
                if (tile.HasTile && (tile.TileType == TileID.Sandstone || tile.TileType == TileID.Sand || tile.TileType == TileID.HardenedSand))
                {
                    WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(6, 7), WorldGen.genRand.Next(3, 5), TileType<DuneStoneTile>(), false, 0f, 0f, false, true);
                }
            }
        }

        public override void PostWorldGen()
        {
            //WorldGeneration.Structures.ExoriumStructures.ShadowHouse();
            //WorldGeneration.Structures.ExoriumStructures.FillCrates();

            //WorldGeneration.Structures.ExoriumStructures.FallenTower();
        }
    }
}