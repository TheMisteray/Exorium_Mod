using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace ExoriumMod.Content.Buffs
{
    internal class Inferno : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_51";

        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<ExoriumGlobalNPC>().infernoFire = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen -= 90;
            if (Main.rand.Next(4) < 2)
            {
                Vector2 rad = new Vector2(0, Main.rand.NextFloat(player.width / 20));
                Vector2 shootPoint = rad.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi));
                Dust dust = Dust.NewDustPerfect(player.Center, DustID.SolarFlare, shootPoint, 1, default, .4f + Main.rand.NextFloat(-.3f, .3f));
                dust.noGravity = true;
                dust.color = new Color(184, 58, 24);
            }
        }
    }
}
