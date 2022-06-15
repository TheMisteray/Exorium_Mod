using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Dusts
{
    class BlightDust : ModDust
    {
        public override string Texture => AssetDirectory.Dust + Name;

        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = true;
            dust.scale *= 2;
        }

        public override bool Update(Dust dust)
        {
            dust.scale *= 0.98f;
            if(dust.scale < 0.5f)
            {
                dust.active = false;
            }
            return true;
        }
    }
}
