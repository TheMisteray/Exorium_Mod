using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;
using ExoriumMod.Core.Systems.TileCounters;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Biomes
{
	public class DeadlandBiome : ModBiome
	{
		// Select all the scenery
		public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("ExoriumMod/DeadlandsWater"); // Sets a water style for when inside this biome

		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("ExoriumMod/DeadlandsSurfaceBGStyle");
		//public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Crimson;

		// Select Music
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Sounds/Music/TheGrime");

		//Biome Priority
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;

		//Sky
        public override void SpecialVisuals(Player player)
        {
			player.ManageSpecialBiomeVisuals("ExoriumMod:DeadlandsSky", player.InModBiome(GetInstance<DeadlandBiome>()));
            base.SpecialVisuals(player);
        }

        public override void OnLeave(Player player)
        {
			//Kill vfx when leave biome
			player.ManageSpecialBiomeVisuals("ExoriumMod:DeadlandsSky", false);
			base.OnLeave(player);
        }

        // Populate the Bestiary Filter
        public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => base.BackgroundPath;
		public override Color? BackgroundColor => base.BackgroundColor;

		// Use SetStaticDefaults to assign the display name
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Deadlands");
		}

		// Calculate when the biome is active.
		public override bool IsBiomeActive(Player player)
		{
			bool b1 = ModContent.GetInstance<DeadlandsBiomeTileCount>().deadlandsBlockCount >= 250;

			bool b2 = player.ZoneSkyHeight || player.ZoneOverworldHeight;
			return b1 && b2;
		}
	}
}
