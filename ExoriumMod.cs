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
            woodGroup.ValidItems.Add(ModContent.ItemType<Content.Items.Tiles.Deadwood>());
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
    }
}