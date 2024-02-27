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

namespace ExoriumMod.Core.Systems
{
    public class WorldDataSystem : ModSystem
    {
        public static int shadowAltarCoordsX = 0;
        public static int shadowAltarCoordsY = 0;

        public static Rectangle FallenTowerRect = new Rectangle();

        public override void SaveWorldData(TagCompound tag)
        {
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
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(shadowAltarCoordsX);
            writer.Write(shadowAltarCoordsY);

            WriteRectangle(writer, FallenTowerRect);
        }

        public override void NetReceive(BinaryReader reader)
        {
            shadowAltarCoordsX = reader.ReadInt32();
            shadowAltarCoordsY = reader.ReadInt32();

            ReadRectangle(reader);
        }

        //Not sure if this is necessary
        public override void ClearWorld()
        {
            shadowAltarCoordsX = 0;
            shadowAltarCoordsY = 0;

            FallenTowerRect = new Rectangle();
        }

        private void WriteRectangle(BinaryWriter writer, Rectangle rect)
        {
            writer.Write(rect.X);
            writer.Write(rect.Y);
            writer.Write(rect.Width);
            writer.Write(rect.Height);
        }

        private Rectangle ReadRectangle(BinaryReader reader) => new Rectangle(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
    }
}
