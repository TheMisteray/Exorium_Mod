using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Dusts
{
    class DeadlandWaterDust : ModDust
    {
        public override bool Autoload(ref string name, ref string texture)
        {
			texture = AssetDirectory.Dust + Name;
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
		{
			updateType = 33;
		}

		public override void OnSpawn(Dust dust)
		{
			dust.alpha = 170;
			dust.velocity *= 0.5f;
			dust.velocity.Y += 1f;
		}
	}
}
