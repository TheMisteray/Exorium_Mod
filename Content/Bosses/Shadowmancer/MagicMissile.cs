using Microsoft.Xna.Framework;
using System;
using ExoriumMod.Dusts;
using ExoriumMod.Buffs;
using ExoriumMod.Projectiles.Bosses.AssierJassad;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.NPCs.Bosses.Shadowmancer
{
    class MagicMissile : ModNPC
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.MagicMissile;

        public override void SetDefaults()
        {
            npc.lifeMax = 5;
            npc.aiStyle = -1;
            npc.damage = 25;
            npc.defense = 0;
            npc.knockBackResist = 0f;
            npc.width = 20;
            npc.height = 20;
            npc.lavaImmune = true;
            npc.DeathSound = SoundID.Item9;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.alpha = 255;
        }

        public override bool PreAI()
        {
            npc.velocity.X = npc.ai[0];
            npc.velocity.Y = npc.ai[1];
            return true;
        }

        public override void AI()
        {
            if (Main.netMode != 1)
            {
                npc.TargetClosest(true);
                npc.netUpdate = true;
            }

            if (Main.player[npc.target].dead)
            {
                npc.TargetClosest(true);
                if (Main.player[npc.target].dead)
                    npc.timeLeft = 0;
            }
            Player player = Main.player[npc.target];
            Vector2 delta = player.Center - npc.Center;
            float magnitude = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
            if (magnitude > 0)
                delta *= 4f / magnitude;
            else
                delta = new Vector2(0f, 5f);
            npc.velocity = delta;
            Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustType<Shadow>());
            if (Main.rand.NextBool(5))
            {
                int dust0 = Dust.NewDust(npc.Center + npc.velocity, npc.width, npc.height, DustType<Rainbow>(), delta.X, delta.Y);
                Main.dust[dust0].color = new Color(200, 0, 0);
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            npc.life = 0;
        }
    }
}
