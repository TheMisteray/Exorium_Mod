using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;
using ExoriumMod.Core.Systems.TileCounters;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Core;
using Mono.Cecil;

namespace ExoriumMod.Content.Biomes
{
	public class DeadlandBiome : ModBiome
	{
		// Select all the scenery
		public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("ExoriumMod/DeadlandsWater"); // Sets a water style for when inside this biome
		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("ExoriumMod/DeadlandsSurfaceBGStyle");
		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

		// Select Music
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Sounds/Music/TheGrime");

		//Biome Priority
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;

		//Sky
        public override void SpecialVisuals(Player player, bool isActive)
        {
			player.ManageSpecialBiomeVisuals("ExoriumMod:DeadlandsSky", isActive);
        }

        public override void OnLeave(Player player)
        {
			//Kill vfx when leave biome
			player.ManageSpecialBiomeVisuals("ExoriumMod:DeadlandsSky", false);
			base.OnLeave(player);
        }

		// Populate the Bestiary Filter
		public override string BestiaryIcon => AssetDirectory.BestiaryIcon + "BestiaryDeadlandsIcon";
		public override string BackgroundPath => AssetDirectory.BestiaryBackground + "BestiaryDeadlandsBackground";
		public override Color? BackgroundColor => new Color(.7f, .7f, .7f);
        public override string MapBackground => BackgroundPath;

        public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("The Deadlands");
		}

		// Calculate when the biome is active.
		public override bool IsBiomeActive(Player player)
		{
			bool b1 = ModContent.GetInstance<DeadlandsBiomeTileCount>().deadlandsBlockCount >= 300;

			bool b2 = /*player.ZoneSkyHeight || player.ZoneOverworldHeight;*/ true; //Not needed until an underground variant is added
			return b1 && b2;
		}
	}
}
