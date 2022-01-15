using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ExoriumMod.Dusts
{
    class DeadDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 1;
            dust.scale = 1.1f;
            dust.noGravity = true;
            dust.noLight = true;
        }

        public override bool Update(Dust dust)
        {
            dust.rotation += dust.velocity.X / 14f;
            dust.position.Y -= 1f;
            dust.alpha += 1;
            if (dust.alpha >= 255)
            {
                dust.alpha = 255;
                dust.active = false;
            }
            return false;
        }
    }
}
