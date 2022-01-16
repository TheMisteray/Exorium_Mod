using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Dusts
{
    class Shadow : ModDust
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = AssetDirectory.Dust + name;
            return base.Autoload(ref name, ref texture);
        }

        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 120;
            dust.noGravity = true;
            dust.noLight = true;
            dust.scale *= 2;
        }

        private float swap = 0;

        public override bool Update(Dust dust)
        {
            dust.rotation += 1.5f;
            int oldAlpha = dust.alpha;
            if (swap == 0)
            {
                dust.alpha = (int)(dust.alpha * 1.02);
                if (dust.alpha == oldAlpha)
                {
                    dust.alpha+=2;
                }
                if (dust.alpha >= 180)
                {
                    dust.alpha = 180;
                    swap = 1;
                }
            }
            if (swap == 1)
            {
                dust.alpha = (int)(dust.alpha * .98);
                if (dust.alpha == oldAlpha)
                {
                    dust.alpha-=2;
                }
                if (dust.alpha <= 60)
                {
                    dust.alpha = 60;
                    swap = 0;
                }
            }
            dust.scale *= .98f;
            if (dust.scale <= .1)
                dust.active = false;
            return true;
        }
    }
}
