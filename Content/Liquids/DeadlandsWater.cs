using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Liquids
{
    class DeadlandsWater : ModWaterStyle
    {
        public override bool Autoload(ref string name, ref string texture, ref string blockTexture)
        {
            texture = "ExoriumMod/Liquids/DeadlandsWater";
            blockTexture = "ExoriumMod/Liquids/DeadlandsWaterBlock";
            return base.Autoload(ref name, ref texture, ref blockTexture);
        }

        public override bool ChooseWaterStyle()
        {
            ExoriumPlayer modPlayer = Main.LocalPlayer.GetModPlayer<ExoriumPlayer>();
            return modPlayer.ZoneDeadlands;
        }

        public override int ChooseWaterfallStyle() =>
            mod.GetWaterfallStyleSlot<DeadlandsWaterfall>();

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
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "ExoriumMod/Liquids/DeadlandsWaterfall";
            return base.Autoload(ref name, ref texture);
        }
    }
}
