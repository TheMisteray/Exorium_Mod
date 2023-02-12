using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Core
{
    partial class ExoriumGlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if ((npc.type == NPCID.SnowFlinx || npc.type == NPCID.SpikedIceSlime) && Main.rand.NextBool(200))
                Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ItemType<Content.Items.Weapons.Summoner.IceCream>());
            if ((npc.type == NPCID.IceSlime || npc.type == NPCID.ZombieEskimo) && Main.rand.NextBool(500))
                Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ItemType<Content.Items.Weapons.Summoner.IceCream>());
        }
    }
}
