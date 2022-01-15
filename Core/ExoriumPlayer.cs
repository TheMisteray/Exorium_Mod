using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Projectiles;
using ExoriumMod.Dusts;

namespace ExoriumMod.Core
{
    partial class ExoriumPlayer : ModPlayer
    {
        public bool morditeArmor;
        public bool rimestoneArmorHead;
        public bool frostStone;
        public bool wightQuiver;
        public bool blightSlime;
        public bool scrollCooldown;
        public bool shadowCloak;
        public bool deadCloak;
        public bool shield;
        public bool acidArrows;
        public bool ritualArrow;
        public bool reverseHandOut;

        public bool ZoneDeadlands;

        public int cloakHP = 40;

        public override void ResetEffects()
        {
            morditeArmor = false;
            rimestoneArmorHead = false;
            frostStone = false;
            wightQuiver = false;
            blightSlime = false;
            scrollCooldown = false;
            shadowCloak = false;
            shield = false;
            acidArrows = false;
            ritualArrow = false;
            reverseHandOut = false;
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            if (morditeArmor && Main.rand.Next(6) == 0)
            {
                int numberProjectiles = 7 + Main.rand.Next(2); // 7 to 8 shots
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(Main.rand.Next(-4, 4), Main.rand.Next(-4, 4)).RotatedByRandom(MathHelper.ToRadians(360)); // 360 degree spread.
                    // Stagger difference
                    float scale = 1f - (Main.rand.NextFloat() * .3f);
                    Projectile.NewProjectile(player.position.X, player.position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("MorditeSkull"), 50, 2, player.whoAmI);
                }
            }
            if (shadowCloak && !deadCloak)
                cloakHP -= damage;
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            if (morditeArmor && Main.rand.Next(6) == 1)
            {
                int numberProjectiles = 7 + Main.rand.Next(2); // 7 to 8 shots
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(Main.rand.Next(-4, 4), Main.rand.Next(-4, 4)).RotatedByRandom(MathHelper.ToRadians(360)); // 360 degree spread.
                    // Stagger difference
                    float scale = 1f - (Main.rand.NextFloat() * .3f);
                    Projectile.NewProjectile(player.position.X, player.position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("MorditeSkull"), 50, 2, player.whoAmI);
                }
            }
            if (shadowCloak && !deadCloak)
                cloakHP -= damage;
        }

        public override bool ConsumeAmmo(Item weapon, Item ammo)
        {
            if (rimestoneArmorHead && Main.rand.Next(6) == 0)
                return false;
            if (wightQuiver == true && Main.rand.Next(11) == 0)
                return false;
            return true;
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (item.melee == true && frostStone)
                target.AddBuff(BuffID.Frostburn, 120);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (proj.melee == true && frostStone)
                target.AddBuff(BuffID.Frostburn, 120);
            if (proj.type == ProjectileID.WoodenArrowFriendly && acidArrows)
                target.AddBuff(BuffType<Buffs.CausticAcid>(), 300);
        }

        public override void ModifyWeaponDamage(Item item, ref float add, ref float mult, ref float flat)
        {
            if (item.useAmmo == AmmoID.Arrow && wightQuiver)
                mult += 0.06f;
        }
    }
}

