using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Microsoft.Xna.Framework;

namespace ExoriumMod.Core
{
    public class BiomeHandler : ModPlayer
    {
        public bool ZoneDeadlands = false;

        public override void UpdateBiomes()
        {
            ZoneDeadlands = ExoriumWorld.deadlandTiles > 450;
        }

        public override bool CustomBiomesMatch(Player other)
        {
            BiomeHandler modOther = other.GetModPlayer<BiomeHandler>();
            return ZoneDeadlands == modOther.ZoneDeadlands;
        }

        public override void SendCustomBiomes(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = ZoneDeadlands;
            writer.Write(flags);
        }

        public override void ReceiveCustomBiomes(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            ZoneDeadlands = flags[0];
        }

        public override Texture2D GetMapBackgroundImage()
        {
            if (ZoneDeadlands)
            {
                return mod.GetTexture("Assets/Backgrounds/Map/DeadlandsMapBackground" /*AssetDirectory.MapBackground + "DeadlandsMapBackground"*/);
            }
            return null;
        }


        public override void UpdateBiomeVisuals()
        {
            player.ManageSpecialBiomeVisuals("ExoriumMod:DeadlandsSky", ZoneDeadlands);
            base.UpdateBiomeVisuals();
        }
    }

    public partial class ExoriumWorld : ModWorld
    {
        public static int deadlandTiles;

        public override void TileCountsAvailable(int[] tileCounts)
        {
            deadlandTiles = tileCounts[TileType<Content.Tiles.AshenDustTile>()];
        }

        public override void ResetNearbyTileEffects()
        {
            BiomeHandler modPlayer = Main.LocalPlayer.GetModPlayer<BiomeHandler>();
            deadlandTiles = 0;
        }
    }
}

namespace ExoriumMod
{
    public partial class ExoriumMod : Mod
    {
        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            if (Core.ExoriumWorld.deadlandTiles <= 250)
            {
                return;
            }

            float deadlandStrength = Core.ExoriumWorld.deadlandTiles / 1200f;
            deadlandStrength = Math.Min(deadlandStrength, 1f);

            int sunR = backgroundColor.R;
            int sunG = backgroundColor.G;
            int sunB = backgroundColor.B;
            sunB -= (int)(90f * deadlandStrength * (backgroundColor.R / 255f)); //backgroundColor.R On purpose to change how the lighting looks
            sunR -= (int)(180f * deadlandStrength * (backgroundColor.R / 255f));
            sunG -= (int)(90f * deadlandStrength * (backgroundColor.G / 255f));
            sunR = Utils.Clamp(sunR, 0, 255);
            sunG = Utils.Clamp(sunG, 0, 255);
            sunB = Utils.Clamp(sunB, 0, 255);
            backgroundColor.R = (byte)sunB;
            backgroundColor.G = (byte)sunB;
            backgroundColor.B = (byte)sunB;
        }
    }
}
