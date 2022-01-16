using Microsoft.Xna.Framework;
using ExoriumMod.Core;
using System;
using Terraria;
using Terraria.ModLoader;

namespace ExoriumMod
{
	public partial class ExoriumMod : Mod
	{
		public ExoriumMod()
		{
		}

        public override void AddRecipeGroups()
        {
            RecipeGroup woodGroup = RecipeGroup.recipeGroups[RecipeGroup.recipeGroupIDs["Wood"]];
            woodGroup.ValidItems.Add(ModContent.ItemType<Content.Items.TileItems.Deadwood>());
        }

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
            {
                return;
            }
            if (Main.LocalPlayer.GetModPlayer<BiomeHandler>().ZoneDeadlands)
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
    }
}