using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ExoriumMod.Content.Dusts;

namespace ExoriumMod.Core
{
    partial class ExoriumGlobalNPC
    {
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (cDark)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, ModContent.DustType<Shadow>(), npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    if (Main.rand.NextBool(4))
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }
                drawColor = new Color(60, 60, 60);
            }
            if (cAcid)
            {
                if (Main.rand.Next(4) < 2)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, DustID.Water, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, Color.Orange);
                    Main.dust[dust].noGravity = false;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y += 0.5f;
                    Main.dust[dust].scale *= 0.5f;
                }
                drawColor = new Color(160, 200, 0);
            }
        }
    }
}
