using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Buffs.Minions
{
    class SummonTagDebuffs : GlobalNPC
    {
        // This is required to store information on entities that isn't shared between them.
        public override bool InstancePerEntity => true;

        public bool markedBySparklingWhip;

        public bool markedByFlameTongue;

        public bool flameTongueBurn;

        public override void ResetEffects(NPC npc)
        {
            markedBySparklingWhip = false;
            markedByFlameTongue = false;
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            // Only player attacks should benefit from summon tag, hence the NPC and trap checks.
            if (!projectile.npcProj && !projectile.trap && (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type]))
            {
                if (markedBySparklingWhip)
                    damage += 8;
                if (markedByFlameTongue)
                {
                    damage += 7;
                    npc.AddBuff(BuffType<FlameTongueBurn>(), 90);
                }
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (markedByFlameTongue)
            {
                npc.defense -= 5;
            }
            if (flameTongueBurn)
            {
                npc.lifeRegen -= 40;
            }
        }
    }

    class SparklingWhipTag : ModBuff
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetStaticDefaults()
        {
            // This allows the debuff to be inflicted on NPCs that would otherwise be immune to all debuffs.
            // Other mods may check it for different purposes.
            BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<SummonTagDebuffs>().markedBySparklingWhip = true;
        }
    }

    class FlameTongueTag : ModBuff
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<SummonTagDebuffs>().markedByFlameTongue = true;
        }
    }

    class FlameTongueBurn : ModBuff
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<SummonTagDebuffs>().flameTongueBurn = true;
        }
    }
}
