using System;
using System.Collections.Generic;
using ExoriumMod.Core;
using Terraria.ModLoader;

namespace ExoriumMod
{
    public partial class ExoriumMod : Mod
    {
        private void BossChecklistCC()
        {
            Mod bcl = ModLoader.GetMod("BossChecklist");
            if (bcl == null) return;

            List<int> ShadowmancerLootPool = new List<int>()
            {
                ModContent.ItemType<Content.Bosses.Shadowmancer.ShadowmancerBag>(),
                ModContent.ItemType<Content.Items.Accessories.ShadowmancerCloak>(),
                ModContent.ItemType<Content.Items.Weapons.Magic.ShadowBolt>(),
                ModContent.ItemType<Content.Items.Weapons.Melee.NineLivesStealer>(),
                ModContent.ItemType<Content.Items.Weapons.Ranger.AcidOrb>(),
                ModContent.ItemType<Content.Items.Weapons.Summoner.ShadowOrb>(),
                ModContent.ItemType<Content.Items.Consumables.Scrolls.SpellScrollShield>(),
                ModContent.ItemType<Content.Items.Consumables.Scrolls.ScrollOfMagicMissiles>(),
                28
            };
            List<int> ShadowmancerLootCollection = new List<int>();
            string ShadowmancerInfo = "Follow a [i:" + ModContent.ItemType<Content.Items.Accessories.RitualBone>() + "] and touch the Shadow Altar.";

            bcl.Call("AddBoss", 1.9, ModContent.NPCType<Content.Bosses.Shadowmancer.AssierJassad>(), this, "Shadowmancer", (Func<bool>)(() => ExoriumWorld.downedShadowmancer), null, ShadowmancerLootCollection, ShadowmancerLootPool, ShadowmancerInfo);

            List<int> BlightSlimeLootPool = new List<int>()
            {
                ModContent.ItemType<Content.Bosses.BlightedSlime.BlightedSlimeBag>(),
                ModContent.ItemType<Content.Items.Accessories.CoreOfBlight>(),
                ModContent.ItemType<Content.Items.Materials.TaintedGel>(),
                ModContent.ItemType<Content.Items.Materials.Metals.BlightedOre>(),
                28
            };
            List<int> BlightSlimeLootCollection = new List<int>();
            string BlightSlimeInfo = "Use a [i:" + ModContent.ItemType<Content.Bosses.BlightedSlime.TaintedSludge>() + "] in the Deadlands.";

            bcl.Call("AddBoss", 3.1, ModContent.NPCType<Content.Bosses.BlightedSlime.BlightedSlime>(), this, "Blighted Slime", (Func<bool>)(() => ExoriumWorld.downedBlightslime), null, BlightSlimeLootCollection, BlightSlimeLootPool, BlightSlimeInfo);
        }

        private void CensusCC()
        {
            Mod census = ModLoader.GetMod("Census");
            if (census != null)
            {
                census.Call("TownNPCCondition", ModContent.NPCType<Content.NPCs.Town.Lunatic>(), "Will show up when he feels like it. (After there are at least 3 other NPCs in your town).");
            }
        }

        private void FargoMutantCC()
        {
            Mod fargosMutant = ModLoader.GetMod("Fargowiltas");
            if (fargosMutant != null)
            {
                fargosMutant.Call("AddSummon", 3.1f, "ExoriumMod", "TaintedSludge", (Func<bool>)(() => ExoriumWorld.downedBlightslime), 125000);
            }
        }
    }
}
