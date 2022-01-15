using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ExoriumMod.Dusts
{
    class Rainbow : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.velocity.Y *= 0f;
            dust.velocity.X *= 0f;
            dust.noGravity = true;
            dust.noLight = false;
            dust.scale *= 4;
        }

        public override bool MidUpdate(Dust dust)
        {
            dust.scale *= 0.82f;
            float strength = dust.scale * 1.4f;
            dust.rotation += dust.velocity.X / 14f;
            if (dust.scale < 0.5f)
            {
                dust.active = false;
            }
            if (strength > 1f)
            {
                strength = 1f;
            }
            Lighting.AddLight(dust.position, dust.color.R * strength * 0.002f, dust.color.G * strength * 0.002f, dust.color.B * strength * 0.002f);
            //Lighting.AddLight(dust.position, 0.5f * strength, 0.5f * strength, 0.5f * strength);
            return false;
        }
    }
}
