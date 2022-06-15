using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Dusts
{
    class DarksteelDust : ModDust
    {
        public override string Texture => AssetDirectory.Dust + Name;

        public override void OnSpawn(Dust dust)
        {
            dust.velocity.Y *= 0.2f;
            dust.noGravity = false;
            dust.noLight = true;
            dust.scale *= 2;
        }

        public override bool Update(Dust dust)
        {
            dust.scale *= 0.95f;
            float light = 0.35f * dust.scale;
            if(dust.scale < 0.5f)
            {
                dust.active = false;
            }
            return false;
        }
    }
}
