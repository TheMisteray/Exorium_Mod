using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace ExoriumMod.Content.NPCs.Enemies.Deadlands
{
    class Poltergeist : Hover
    {
        public override string Texture => AssetDirectory.EnemyNPC + Name;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Ghost];
            DisplayName.SetDefault("Specter");

            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.OnFire,
                    BuffID.Frostburn,
                }
            };
            NPCID.Sets.DebuffImmunitySets[Type] = debuffData;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Ghost);
            AnimationType = NPCID.Ghost;
            NPC.damage = 44;
            NPC.defense = 3;
            NPC.lifeMax = 99;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 200f;
            NPC.buffImmune[BuffID.Confused] = false;
        }

        public override void CustomBehavior(ref float ai)
        {
            Vector2 dist = Main.player[NPC.target].position - NPC.position;
            float magnitude = (float)Math.Sqrt(dist.X * dist.X + dist.Y * dist.Y);
            if (magnitude >= 270 && NPC.alpha <= 255)
                NPC.alpha += 5;
            else if (NPC.alpha >= 40)
                NPC.alpha -= 5; 
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            if (NPC.alpha > 240)
                return false;
            return true;
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            NPC.alpha = 60;
        }
    }
}
