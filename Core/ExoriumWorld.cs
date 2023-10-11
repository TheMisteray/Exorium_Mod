﻿using Microsoft.Xna.Framework;
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
        public static bool downedShadowmancer = false;
        public static bool downedBlightslime = false;

        public static int shadowAltarCoordsX = 0;
        public static int shadowAltarCoordsY = 0;

        public static Rectangle FallenTowerRect = new Rectangle();

        public override void SaveWorldData(TagCompound tag)/* Suggestion: Edit tag parameter rather than returning new TagCompound */
        {
            var downed = new List<string>();
            if (downedShadowmancer)
                downed.Add("shadowmancer");
            if (downedBlightslime)
                downed.Add("blightslime");

            tag["downed"] = downed;
            tag["shadowAltarCoordsX"] = shadowAltarCoordsX;
            tag["shadowAltarCoordsY"] = shadowAltarCoordsY;
            tag["FallenTowerPos"] = FallenTowerRect.TopLeft();
            tag["FallenTowerSize"] = FallenTowerRect.Size();
        }

        public override void LoadWorldData(TagCompound tag)
        {
            shadowAltarCoordsX = (int)tag.Get<int>("shadowAltarCoordsX");
            shadowAltarCoordsY = (int)tag.Get<int>("shadowAltarCoordsY");
            FallenTowerRect.X = (int)tag.Get<Vector2>("FallenTowerPos").X;
            FallenTowerRect.Y = (int)tag.Get<Vector2>("FallenTowerPos").Y;
            FallenTowerRect.Width = (int)tag.Get<Vector2>("FallenTowerSize").X;
            FallenTowerRect.Height = (int)tag.Get<Vector2>("FallenTowerSize").Y;

            var downed = tag.GetList<string>("downed");
            downedShadowmancer = downed.Contains("shadowmancer");
            downedBlightslime = downed.Contains("blightslime");
        }

        /* No older versions
        public override void LoadLegacy(BinaryReader reader)
        {
            int loadVersion = reader.ReadInt32();
            if (loadVersion == 0)
            {
                shadowAltarCoordsX = reader.ReadInt32();
                shadowAltarCoordsY = reader.ReadInt32();
                BitsByte flags = reader.ReadByte();
                downedShadowmancer = flags[0];
                downedBlightslime = flags[1];
            }
            else
            {
                Mod.Logger.Error("ExoriumMod: Unknown loadVersion:" + loadVersion);
            }
        }
        */

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(shadowAltarCoordsX);
            writer.Write(shadowAltarCoordsY);
            BitsByte bosses1 = new BitsByte(downedShadowmancer, downedBlightslime);
            writer.Write(bosses1);

            WriteRectangle(writer, FallenTowerRect);
        }

        public override void NetReceive(BinaryReader reader)
        {
            shadowAltarCoordsX = reader.ReadInt32();
            shadowAltarCoordsY = reader.ReadInt32();
            BitsByte bosses1 = reader.ReadByte();
            downedShadowmancer = bosses1[0];
            downedBlightslime = bosses1[1];

            ReadRectangle(reader);
        }

        private void WriteRectangle(BinaryWriter writer, Rectangle rect)
        {
            writer.Write(rect.X);
            writer.Write(rect.Y);
            writer.Write(rect.Width);
            writer.Write(rect.Height);
        }

        private Rectangle ReadRectangle(BinaryReader reader) => new Rectangle(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());

        public override void OnWorldLoad()
        {
            downedShadowmancer = false;
            downedBlightslime = false;
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (ShiniesIndex != -1)
            {
                tasks.Insert(ShiniesIndex + 1, new PassLegacy("Exorium Mod Ores", ExoriumOreGeneration));
            }

            int DirtRockWallRunnerIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Dirt Rock Wall Runner"));
            if (DirtRockWallRunnerIndex != -1)
            {
                tasks.Insert(DirtRockWallRunnerIndex + 1, new PassLegacy("Deadlands", delegate (GenerationProgress progress, GameConfiguration config)
                  {
                      progress.Message = "Generating Deadlands";
                      WorldGeneration.Biomes.ExoriumBiomes.DeadlandsGeneration();
                  }));
            }

            int FinalCleanupIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
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