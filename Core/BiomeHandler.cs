using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Projectiles;
using ExoriumMod.Dusts;

namespace ExoriumMod.Core
{
    partial class ExoriumPlayer
    {
        public override void UpdateBiomes()
        {
            ZoneDeadlands = ExoriumWorld.deadlandTiles > 800;
        }

        public override bool CustomBiomesMatch(Player other)
        {
            ExoriumPlayer modOther = other.GetModPlayer<ExoriumPlayer>();
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
                return mod.GetTexture(AssetDirectory.MapBackground + "DeadlandsMapBackground");
            }
            return null;
        }
    }
}
