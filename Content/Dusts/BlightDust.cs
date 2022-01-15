using Terraria;
using Terraria.ModLoader;

namespace ExoriumMod.Dusts
{
    class BlightDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            //dust.velocity.Y *= 0.2f;
            dust.noGravity = true;
            dust.noLight = true;
            dust.scale *= 2;
        }

        public override bool Update(Dust dust)
        {
            dust.scale *= 0.98f;
            float light = 0.35f * dust.scale;
            if(dust.scale < 0.5f)
            {
                dust.active = false;
            }
            return true;
        }
    }
}
