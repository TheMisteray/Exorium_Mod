using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Biomes.Liquids
{
    class DeadlandsWater : ModWaterStyle
    {
        public override string Texture => AssetDirectory.Liquid + Name;

        public override int ChooseWaterfallStyle() => Find<ModWaterfallStyle>("ExoriumMod/DeadlandsWaterfall").Slot;

        public override int GetSplashDust() =>
            DustType<Dusts.DeadlandWaterDust>();

        public override int GetDropletGore() => 0;

        public override void LightColorMultiplier(ref float r, ref float g, ref float b)
        {
            r = 0.88f;
            g = 0.85f;
            b = 0.85f;
        }

        public override Color BiomeHairColor() => new Color(136, 136, 136);
    }

    public class DeadlandsWaterfall : ModWaterfallStyle
    {
        public override string Texture => AssetDirectory.Liquid + Name;
    }
}
