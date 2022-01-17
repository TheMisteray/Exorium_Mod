using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Core
{
    partial class ExoriumGlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if ((npc.type == NPCID.SnowFlinx || npc.type == NPCID.SpikedIceSlime) && Main.rand.Next(20) == 0)
                Item.NewItem(npc.getRect(), ItemType<Content.Items.Weapons.Summoner.IceCream>());
            if ((npc.type == NPCID.IceSlime || npc.type == NPCID.ZombieEskimo) && Main.rand.Next(100) == 0)
                Item.NewItem(npc.getRect(), ItemType<Content.Items.Weapons.Summoner.IceCream>());
        }
    }
}
