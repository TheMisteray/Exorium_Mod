using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Dusts
{
    class DeadwoodTreeDust : ModDust
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = AssetDirectory.Dust + name;
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            Dust.CloneDust(7);
            base.SetDefaults();
        }
    }
}
