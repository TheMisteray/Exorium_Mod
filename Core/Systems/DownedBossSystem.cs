using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ExoriumMod.Core.Systems
{
    public class DownedBossSystem : ModSystem
    {
        public static bool downedShadowmancer = false;
        public static bool downedBlightslime = false;
        public static bool downedCrimsonKnight = false;

        public static bool downedGemsparklingHive = false;

        public static bool foughtCrimsonKnight = false;
        public static bool trucedCrimsonKnight = false;
        public static bool dueledCrimsonKnight = false;
        public static bool killedCrimsonKnight = false;

        public override void ClearWorld()
        {
            downedShadowmancer = false;
            downedBlightslime = false;
            downedCrimsonKnight = false;

            downedGemsparklingHive = false;

            foughtCrimsonKnight = false;
            trucedCrimsonKnight = false;
            dueledCrimsonKnight = false;
            killedCrimsonKnight = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            var downed = new List<string>();
            if (downedShadowmancer)
                downed.Add("shadowmancer");
            if (downedBlightslime)
                downed.Add("blightslime");
            if (downedGemsparklingHive)
                downed.Add("gemsparkling");
            if (downedCrimsonKnight)
                downed.Add("crimsonknight");

            var crimsonKnightData = new List<string>();
            if (foughtCrimsonKnight)
                crimsonKnightData.Add("fought");
            if (dueledCrimsonKnight)
                crimsonKnightData.Add("dueled");
            if (killedCrimsonKnight)
                crimsonKnightData.Add("killed");

            tag["downed"] = downed;
            tag["crimsonknightdata"] = crimsonKnightData;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            var downed = tag.GetList<string>("downed");
            downedShadowmancer = downed.Contains("shadowmancer");
            downedBlightslime = downed.Contains("blightslime");
            downedGemsparklingHive = downed.Contains("gemsparkling");
            downedCrimsonKnight = downed.Contains("crimsonknight");

            var crimsonKnightData = tag.GetList<string>("crimsonknightdata");
            foughtCrimsonKnight = crimsonKnightData.Contains("fought");
            dueledCrimsonKnight = crimsonKnightData.Contains("dueled");
            killedCrimsonKnight = crimsonKnightData.Contains("killed");
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte bosses1 = new BitsByte(downedShadowmancer, downedBlightslime, downedGemsparklingHive, downedCrimsonKnight);
            writer.Write(bosses1);
            BitsByte crimsonKnight = new BitsByte(foughtCrimsonKnight, dueledCrimsonKnight, killedCrimsonKnight);
            writer.Write(crimsonKnight);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte bosses1 = reader.ReadByte();
            downedShadowmancer = bosses1[0];
            downedBlightslime = bosses1[1];
            downedGemsparklingHive = bosses1[2];
            downedCrimsonKnight = bosses1[3];

            BitsByte crimsonKnight = reader.ReadByte();
            foughtCrimsonKnight = crimsonKnight[0];
            dueledCrimsonKnight = crimsonKnight[1];
            killedCrimsonKnight = crimsonKnight[2];
        }
    }
}            

             
             
             
             