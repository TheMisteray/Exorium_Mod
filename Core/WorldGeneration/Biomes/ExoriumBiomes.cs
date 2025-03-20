using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using System;
using ExoriumMod.Content.Tiles;
using ExoriumMod.Content.Walls;
using Microsoft.Xna.Framework;

namespace ExoriumMod.Core.WorldGeneration.Biomes
{
    public static partial class ExoriumBiomes
    {
        public static void DeadlandsGeneration()
        {
            bool success = false;
            int attempts = 0;
            while (!success)
            {
                attempts++;
                int width = 200;
                int i = WorldGen.genRand.Next((Main.maxTilesX / 8) + 10, Main.maxTilesX - (Main.maxTilesX / 8) - width - 10);
                int k = i + width;
                if ((i <= Main.maxTilesX / 2 - (Main.maxTilesX / 8) || i >= Main.maxTilesX / 2 + (Main.maxTilesX / 8)) && (k <= Main.maxTilesX / 2 - (Main.maxTilesX / 8) || k >= Main.maxTilesX / 2 + (Main.maxTilesX / 8))) //not within center 5th of world or outer 10ths(ocean)
                {
                    int j = 0;
                    int l = 0;
                    while (!Framing.GetTileSafely(i, j).HasTile && (double)j < Main.worldSurface)
                    {
                        j++;
                    }
                    while (!Framing.GetTileSafely(k, l).HasTile && (double)l < Main.worldSurface)
                    {
                        l++;
                    }
                    if (((Framing.GetTileSafely(k, l).TileType == TileID.Dirt) && (Framing.GetTileSafely(k, l).TileType == TileID.Dirt)) || attempts > 100000)//if too many attempts dirt is no longer required
                    {
                        j += 50;
                        l += 50;
                        bool placementOK = true;
                        for (int m = i - Main.maxTilesX / 60; m <= k + Main.maxTilesX / 60; m++)
                        {
                            for (int n = Math.Min(j, l) - Main.maxTilesX / 30; n <= Math.Max(j, l) + Main.maxTilesX / 30; n++)
                            {
                                if (!WorldGen.InWorld(m, n, 50))
                                {
                                    placementOK = false;
                                    break;
                                }
                                if (Framing.GetTileSafely(m, n).HasTile)
                                {
                                    int type = Framing.GetTileSafely(m, n).TileType;
                                    if (type == TileID.BlueDungeonBrick || type == TileID.GreenDungeonBrick || type == TileID.PinkDungeonBrick || type == TileID.Cloud || type == TileID.RainCloud || type == TileID.WoodBlock || type == TileID.LivingWood || type == TileID.LivingMahoganyLeaves || type == TileID.Ebonstone || type == TileID.Crimstone || type == TileID.SandstoneBrick)
                                    {
                                        placementOK = false;
                                        break;
                                    }
                                }
                            }
                        }
                        if (placementOK || attempts > 100000) //if far too many attempts, spawn anyway (this may disrupt other structures but hopefully won't happen too often)
                        {
                            success = SetDeadlands(i, j, k, l);
                        }
                    }
                }
            }
        }

        private static bool SetDeadlands(int i, int j, int k, int l)
        {
            float sizeScale = Main.maxTilesX / 14f;
            if (j < 150 || l < 150)
                return false;
            for (int m = i - Main.maxTilesX / 20; m <= k + Main.maxTilesX / 20; m++)
            {
                for (int n = Math.Min(j, l) - Main.maxTilesX / 20; n <= Math.Max(j, l) + Main.maxTilesX / 20; n++)
                {
                    double sumDist = (Math.Sqrt(Math.Pow(i - m, 2) + Math.Pow(j - n, 2))) + (Math.Sqrt(Math.Pow(k - m, 2) + Math.Pow(l - n, 2)));
                    int smoothSpread = Main.maxTilesX / 60;
                    if (sizeScale >= sumDist ||
                        (sizeScale + smoothSpread >= sumDist  //This and the lines that follow check out a little further for a more smooth transition
                        && smoothSpread - ((int)sumDist - (int)sizeScale) > 0 //Avoid divide by 0
                        && WorldGen.genRand.Next((smoothSpread + smoothSpread) / (smoothSpread - ((int)sumDist - (int)sizeScale))) == 1)) //Lower and lower chance to still place a tile up to smoothspread distance
                    {
                        if (WorldGen.InWorld(m, n, 30)) //Don't think this is actually necessary
                        {
                            switch (Framing.GetTileSafely(m, n).TileType)
                            {
                                case TileID.Copper:
                                case TileID.Tin:
                                case TileID.Lead:
                                case TileID.Iron:
                                case TileID.Silver:
                                case TileID.Tungsten:
                                case TileID.Gold:
                                case TileID.Platinum:
                                case TileID.Crimtane:
                                case TileID.Demonite:
                                    Framing.GetTileSafely(m, n).TileType = (ushort)TileType<BlightedOreTile>();
                                    break;
                                case TileID.BlueDungeonBrick:
                                case TileID.GreenDungeonBrick:
                                case TileID.PinkDungeonBrick:
                                case TileID.SandstoneBrick:
                                case TileID.LivingWood:
                                    break;
                                default:
                                    Framing.GetTileSafely(m, n).TileType = (ushort)TileType<AshenDustTile>();
                                    break;
                            }
                            if (Framing.GetTileSafely(m, n).WallType != 0 && Framing.GetTileSafely(m, n).WallType != 7 && Framing.GetTileSafely(m, n).WallType != 8 && Framing.GetTileSafely(m, n).WallType != 9 &&
                                Framing.GetTileSafely(m, n).WallType != 244 && Framing.GetTileSafely(m, n).WallType != 94 && Framing.GetTileSafely(m, n).WallType != 95 && Framing.GetTileSafely(m, n).WallType != 96 &&
                                Framing.GetTileSafely(m, n).WallType != 97 && Framing.GetTileSafely(m, n).WallType != 98 && Framing.GetTileSafely(m, n).WallType != 99)
                                Framing.GetTileSafely(m, n).WallType = (ushort)WallType<AshenDustWall>();
                        }
                    }
                }
            }
            Systems.WorldDataSystem.deadlandsNode1 = new Vector2(i, j);
            Systems.WorldDataSystem.deadlandsNode2 = new Vector2(k, l);
            return true;
        }

        public static void FinalizeDeadlands()
        {
            float sizeScale = Main.maxTilesX / 14f;
            int i = (int)Systems.WorldDataSystem.deadlandsNode1.X;
            int j = (int)Systems.WorldDataSystem.deadlandsNode1.Y;
            int k = (int)Systems.WorldDataSystem.deadlandsNode2.X;
            int l = (int)Systems.WorldDataSystem.deadlandsNode2.Y;

            //Repeat the normal generation, and remove grass/dirt from the main body. This is caused by a later world gen pass that does not allow generating before living trees, introduced in 1.4
            for (int m = i - Main.maxTilesX / 20; m <= k + Main.maxTilesX / 20; m++)
            {
                for (int n = Math.Min(j, l) - Main.maxTilesX / 20; n <= Math.Max(j, l) + Main.maxTilesX / 20; n++)
                {
                    double sumDist = (Math.Sqrt(Math.Pow(i - m, 2) + Math.Pow(j - n, 2))) + (Math.Sqrt(Math.Pow(k - m, 2) + Math.Pow(l - n, 2)));
                    int smoothSpread = Main.maxTilesX / 60;
                    if (sizeScale >= sumDist) //Lower and lower chance to still place a tile up to smoothspread distance
                    {
                        if (WorldGen.InWorld(m, n, 30)) //Don't think this is actually necessary
                        {
                            switch (Framing.GetTileSafely(m, n).TileType)
                            {
                                case TileID.Grass:
                                case TileID.Dirt:
                                    Framing.GetTileSafely(m, n).TileType = (ushort)TileType<AshenDustTile>();
                                    break;
                                case TileID.Copper:
                                case TileID.Tin:
                                case TileID.Lead:
                                case TileID.Iron:
                                case TileID.Silver:
                                case TileID.Tungsten:
                                case TileID.Gold:
                                case TileID.Platinum:
                                case TileID.Crimtane:
                                case TileID.Demonite:
                                    Framing.GetTileSafely(m, n).TileType = (ushort)TileType<BlightedOreTile>();
                                    break;
                                default:
                                    break;

                            }
                            if (Framing.GetTileSafely(m, n).WallType != 0 && Framing.GetTileSafely(m, n).WallType != 7 && Framing.GetTileSafely(m, n).WallType != 8 && Framing.GetTileSafely(m, n).WallType != 9 &&
                                Framing.GetTileSafely(m, n).WallType != 244 && Framing.GetTileSafely(m, n).WallType != 94 && Framing.GetTileSafely(m, n).WallType != 95 && Framing.GetTileSafely(m, n).WallType != 96 &&
                                Framing.GetTileSafely(m, n).WallType != 97 && Framing.GetTileSafely(m, n).WallType != 98 && Framing.GetTileSafely(m, n).WallType != 99)
                                Framing.GetTileSafely(m, n).WallType = (ushort)WallType<AshenDustWall>();
                        }
                    }
                }
            }
        }
    }
}
