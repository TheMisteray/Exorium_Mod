using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Dusts
{
    class CustomFlame : ModDust
    {
        private int type; //Dusts are assigned a type on spawn that determines their behavior

        public override bool Autoload(ref string name, ref string texture)
        {
            texture = AssetDirectory.Dust + name;
            return base.Autoload(ref name, ref texture);
        }

        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = false;
            dust.noLight = false;
            if (Main.rand.Next(5) >= 1)
            {
                type = 0;
                dust.scale = 5;
            }
            else if (Main.rand.Next(2) == 0)
            {
                type = 1;
                dust.scale = 3;
            }
            else
            {
                type = 2;
                dust.scale = 1;
            }
            dust.alpha = 160;
        }

        public override bool Update(Dust dust)
        {
            dust.scale *= (0.98f);

            if (dust.scale < 0.5f)
                dust.active = false;
            dust.rotation += dust.velocity.X / 14f;

            if (type == 1)  //Some dust fall
            {
                dust.velocity.Y = (float)Main.rand.Next(-10, 6) * 0.1f;
                Dust expr_43F_cp_0 = dust;
                expr_43F_cp_0.velocity.X = expr_43F_cp_0.velocity.X * 0.3f;
                dust.velocity.Y += 0.5f;
            }

            float lightIntensity = 0.6f /*0.35f * dust.scale*/;
            Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), lightIntensity, lightIntensity * 0.65f, lightIntensity * 0.4f);
            return false;
        }
    }
}
