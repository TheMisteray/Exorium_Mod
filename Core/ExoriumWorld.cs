using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using static Terraria.ModLoader.ModContent;
using System;
using ExoriumMod.Content.Tiles;
using ExoriumMod.Content.Walls;

namespace ExoriumMod.Core
{
    public partial class ExoriumWorld : ModWorld
    {
        public static bool downedShadowmancer = false;
        public static bool downedBlightslime = false;

        public static int shadowAltarCoordsX;
        public static int shadowAltarCoordsY;

        public override TagCompound Save()
        {
            var downed = new List<string>();
            if (downedShadowmancer)
                downed.Add("shadowmancer");
            if (downedBlightslime)
                downed.Add("blightslime");

            return new TagCompound
            {
                ["downed"] = downed,
                ["shadowAltarCoordsX"] = shadowAltarCoordsX,
                ["shadowAltarCoordsY"] = shadowAltarCoordsY
            };
        }

        public override void Load(TagCompound tag)
        {
            shadowAltarCoordsX = (int)tag.Get<int>("shadowAltarCoordsX");
            shadowAltarCoordsY = (int)tag.Get<int>("shadowAltarCoordsY");

            var downed = tag.GetList<string>("downed");
            downedShadowmancer = downed.Contains("shadowmancer");
            downedBlightslime = downed.Contains("blightslime");
        }

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
                mod.Logger.Error("ExoriumMod: Unknown loadVersion:" + loadVersion);
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(shadowAltarCoordsX);
            writer.Write(shadowAltarCoordsY);
            BitsByte bosses1 = new BitsByte(downedShadowmancer, downedBlightslime);
            writer.Write(bosses1);
        }

        public override void NetReceive(BinaryReader reader)
        {
            shadowAltarCoordsX = reader.ReadInt32();
            shadowAltarCoordsY = reader.ReadInt32();
            BitsByte bosses1 = reader.ReadByte();
            downedShadowmancer = bosses1[0];
            downedBlightslime = bosses1[1];
        }

        public override void Initialize()
        {
            downedShadowmancer = false;
            downedBlightslime = false;
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWright)
        {
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (ShiniesIndex != -1)
            {
                tasks.Insert(ShiniesIndex + 1, new PassLegacy("Exorium Mod Ores", ExoriumOreGeneration));
            }

            int DirtRockWallRunnerIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Dirt Rock Wall Runner"));
            if (DirtRockWallRunnerIndex != -1)
            {
                tasks.Insert(DirtRockWallRunnerIndex + 1, new PassLegacy("Deadlands", delegate (GenerationProgress progress)
                  {
                      progress.Message = "Generating Deadlands";
                      WorldGeneration.Biomes.ExoriumBiomes.DeadlandsGeneration();
                  }));
            }
        }

        private void ExoriumOreGeneration(GenerationProgress progress)
        {
            progress.Message = "Generating Exorium Ores";
            //BlightSteel spawn
            for (int i = 0; i < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 12E-05); i++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY); //Spawn in cavern layer
                Tile tile = Framing.GetTileSafely(x, y);
                if (tile.active() && tile.type == TileID.Stone)
                {
                    WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(2, 3), WorldGen.genRand.Next(1, 3), TileType<BlightedOreTile>(), false, 0f, 0f, false, true);
                }
            }
            //RimeStone spawn
            for (int i = 0; i < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 5E-04); i++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)WorldGen.worldSurfaceLow, Main.maxTilesY);
                Tile tile = Framing.GetTileSafely(x, y);
                if (tile.active() && tile.type == TileID.IceBlock || tile.type == TileID.SnowBlock)
                {
                    WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(6, 7), WorldGen.genRand.Next(3, 5), TileType<RimeStoneTile>(), false, 0f, 0f, false, true);
                }
            }
            //DuneStone spawn
            for (int i = 0; i < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-04); i++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)WorldGen.worldSurfaceLow, Main.maxTilesY);
                Tile tile = Framing.GetTileSafely(x, y);
                if (tile.active() && (tile.type == TileID.Sandstone || tile.type == TileID.Sand || tile.type == TileID.HardenedSand))
                {
                    WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(6, 7), WorldGen.genRand.Next(3, 5), TileType<DuneStoneTile>(), false, 0f, 0f, false, true);
                }
            }
        }

        public override void PostWorldGen()
        {
            WorldGeneration.Structures.ExoriumStructures.ShadowHouse();
            WorldGeneration.Structures.ExoriumStructures.FillCrates();


        }
    }
}