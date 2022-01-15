using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Items.Accessories;
using ExoriumMod.Items.Materials;
using System;
using Microsoft.Xna.Framework;

namespace ExoriumMod.NPCs.Enemies
{
    class Poltergeist : Hover
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Ghost];
            DisplayName.SetDefault("Specter");
        }

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.Ghost);
            animationType = NPCID.Ghost;
            npc.damage = 23;
            npc.defense = 3;
            npc.lifeMax = 66;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.value = 200f;
            npc.buffImmune[BuffID.Confused] = false;
        }

        public override void CustomBehavior(ref float ai)
        {
            Vector2 dist = Main.player[npc.target].position - npc.position;
            float magnitude = (float)Math.Sqrt(dist.X * dist.X + dist.Y * dist.Y);
            if (magnitude >= 270 && npc.alpha <= 255)
                npc.alpha += 5;
            else if (npc.alpha >= 40)
                npc.alpha -= 5; 
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            if (npc.alpha > 240)
                return false;
            return true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.player.GetModPlayer<ExoriumPlayer>().ZoneDeadlands ? .1f : 0f;
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            npc.alpha = 60;
        }
    }
}
