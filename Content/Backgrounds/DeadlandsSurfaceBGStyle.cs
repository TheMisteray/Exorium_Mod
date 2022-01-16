using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;

namespace ExoriumMod.Backgrounds
{
    public class DeadlandsSurfaceBGStyle : ModSurfaceBgStyle
    {
        public override bool ChooseBgStyle()
        {
            return !Main.gameMenu && Main.LocalPlayer.GetModPlayer<BiomeHandler>().ZoneDeadlands;
        }

        // Use this to keep far Backgrounds like the mountains.
        public override void ModifyFarFades(float[] fades, float transitionSpeed)
        {
            for (int i = 0; i < fades.Length; i++)
            {
                if (i == Slot)
                {
                    fades[i] += transitionSpeed;
                    if (fades[i] > 1f)
                    {
                        fades[i] = 1f;
                    }
                }
                else
                {
                    fades[i] -= transitionSpeed;
                    if (fades[i] < 0f)
                    {
                        fades[i] = 0f;
                    }
                }
            }
        }


        //TODO: Look into why this can't look into the asset folder, there should not have to be a backgrounds folder
        public override int ChooseFarTexture()
        {
            return mod.GetBackgroundSlot("Backgrounds/DeadlandsSurfaceFar");
        }

        public override int ChooseMiddleTexture()
        {
            return mod.GetBackgroundSlot("Backgrounds/DeadlandsSurfaceMiddle");
        }

        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            return mod.GetBackgroundSlot("Backgrounds/DeadlandsSurfaceClose");
        }
    }
}
