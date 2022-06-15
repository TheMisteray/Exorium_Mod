using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Dusts
{
    class DeadwoodTreeDust : ModDust
    {
        public override string Texture => AssetDirectory.Dust + Name;

        public override void SetStaticDefaults()
        {
            Dust.CloneDust(7);
            base.SetStaticDefaults();
        }
    }
}
