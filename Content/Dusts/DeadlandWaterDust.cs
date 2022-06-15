using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Dusts
{
    class DeadlandWaterDust : ModDust
    {
		public override string Texture => AssetDirectory.Dust + Name;

		public override void SetStaticDefaults()
		{
			UpdateType = 33;
		}

		public override void OnSpawn(Dust dust)
		{
			dust.alpha = 170;
			dust.velocity *= 0.5f;
			dust.velocity.Y += 1f;
		}
	}
}
