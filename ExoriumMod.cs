using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod
{
	public class ExoriumMod : Mod
	{
		public ExoriumMod()
		{
		}
        public override void AddRecipeGroups()
        {
            RecipeGroup woodGroup = RecipeGroup.recipeGroups[RecipeGroup.recipeGroupIDs["Wood"]];
            woodGroup.ValidItems.Add(ModContent.ItemType<Items.Placeables.Deadwood>());
        }

        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            if (ExoriumWorld.deadlandTiles <= 250)
            {
                return;
            }

            float deadlandStrength = ExoriumWorld.deadlandTiles / 1200f;
            deadlandStrength = Math.Min(deadlandStrength, 1f);

            int sunR = backgroundColor.R;
            int sunG = backgroundColor.G;
            int sunB = backgroundColor.B;
            sunB -= (int)(90f * deadlandStrength * (backgroundColor.R / 255f)); //backgroundColor.R On purpose to change how the lighting looks
            sunR -= (int)(180f * deadlandStrength * (backgroundColor.R / 255f));
            sunG -= (int)(90f * deadlandStrength * (backgroundColor.G / 255f));
            sunR = Utils.Clamp(sunR, 10, 255);
            sunG = Utils.Clamp(sunG, 10, 255);
            sunB = Utils.Clamp(sunB, 10, 255);
            backgroundColor.R = (byte)sunB;
            backgroundColor.G = (byte)sunB;
            backgroundColor.B = (byte)sunB;
        }

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
            {
                return;
            }
            if (Main.LocalPlayer.GetModPlayer<ExoriumPlayer>().ZoneDeadlands)
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/TheGrime");
                priority = MusicPriority.BiomeLow;
            }
        }

        public override void PostSetupContent()
        {
            //Cross Content
            BossChecklistCC();
            CensusCC();
            FargoMutantCC();

            base.PostSetupContent();
        }

        private void BossChecklistCC()
        {
            Mod bcl = ModLoader.GetMod("BossChecklist");
            if (bcl == null) return;

            List<int> ShadowmancerLootPool = new List<int>()
            {
                ModContent.ItemType<Items.Consumables.Bosses.ShadowmancerBag>(),
                ModContent.ItemType<Items.Accessories.ShadowmancerCloak>(),
                ModContent.ItemType<Items.Weapons.Magic.ShadowBolt>(),
                ModContent.ItemType<Items.Weapons.Melee.NineLivesStealer>(),
                ModContent.ItemType<Items.Weapons.Ranger.AcidOrb>(),
                ModContent.ItemType<Items.Weapons.Summoner.ShadowOrb>(),
                ModContent.ItemType<Items.Weapons.Magic.Scrolls.SpellScrollShield>(),
                ModContent.ItemType<Items.Weapons.Magic.Scrolls.ScrollOfMagicMissiles>(),
                28
            };
            List<int> ShadowmancerLootCollection = new List<int>();
            string ShadowmancerInfo = "Follow a [i:" + ModContent.ItemType<Items.Accessories.RitualBone>() + "] and touch the Shadow Altar.";

            bcl.Call("AddBoss", 1.9, ModContent.NPCType<NPCs.Bosses.Shadowmancer.AssierJassad>(), this, "Shadowmancer", (Func<bool>)(() => ExoriumWorld.downedShadowmancer), null, ShadowmancerLootCollection, ShadowmancerLootPool, ShadowmancerInfo);

            List<int> BlightSlimeLootPool = new List<int>()
            {
                ModContent.ItemType<Items.Consumables.Bosses.BlightedSlimeBag>(),
                ModContent.ItemType<Items.Accessories.CoreOfBlight>(),
                ModContent.ItemType<Items.Materials.TaintedGel>(),
                ModContent.ItemType<Items.Placeables.BlightedOre>(),
                28
            };
            List<int> BlightSlimeLootCollection = new List<int>();
            string BlightSlimeInfo = "Use a [i:" + ModContent.ItemType<Items.Consumables.Bosses.TaintedSludge>() + "] in the Deadlands.";

            bcl.Call("AddBoss", 3.1, ModContent.NPCType<NPCs.Bosses.BlightedSlime.BlightedSlime>(), this, "Blighted Slime", (Func<bool>)(() => ExoriumWorld.downedBlightslime), null, BlightSlimeLootCollection, BlightSlimeLootPool, BlightSlimeInfo);
        }

        private void CensusCC()
        {
            Mod census = ModLoader.GetMod("Census");
            if (census != null)
            {
                census.Call("TownNPCCondition", ModContent.NPCType<NPCs.Town.Lunatic>(), "Will show up when he feels like it. (After there are at least 3 other NPCs in your town).");
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