using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ExoriumMod.Content.Buffs.Minions
{
    internal class SummonTagDebuffs : GlobalNPC
    {
        // This is required to store information on entities that isn't shared between them.
        public override bool InstancePerEntity => true;

        public bool markedBySparklingWhip;

        public override void ResetEffects(NPC npc)
        {
            markedBySparklingWhip = false;
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            // Only player attacks should benefit from this buff, hence the NPC and trap checks.
            if (markedBySparklingWhip && !projectile.npcProj && !projectile.trap && (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type]))
            {
                damage += 8;
            }
        }
    }

    internal class SparklingWhipTag : ModBuff
    {
        public override string Texture => AssetDirectory.Buff + Name;

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
}
