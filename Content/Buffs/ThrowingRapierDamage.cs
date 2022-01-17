using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Buffs
{
    class ThrowingRapierDamage : ModBuff
    {
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = AssetDirectory.Invisible;
			return base.Autoload(ref name, ref texture);
		}

		public override void SetDefaults()
		{
			DisplayName.SetDefault("20!!!");
			Description.SetDefault("");
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<ExoriumGlobalNPC>().stuckByRapier = true;
		}
	}
}
