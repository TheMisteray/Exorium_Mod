using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Content.Buffs;
using ExoriumMod.Core.Systems;

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
        public bool inflictInferno;
        public bool blightCore;

        //nearby mobs check
        public bool checkNearbyNPCs;
        public List<NPC> nearbyNPCs = new List<NPC>();

        //shadow cloak
        public int cloakHP = 40;
        public int cloakTimer = 0;

        public int ScreenMoveTime = 0;
        public Vector2 ScreenMoveTarget = new Vector2(0, 0);
        public Vector2 ScreenMovePan = new Vector2(0, 0);
        public bool ScreenMoveHold = false;
        private int ScreenMoveTimer = 0;

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
            inflictInferno = false;
            blightCore = false;

            checkNearbyNPCs = false;
        }

        public override void PreUpdate()
        {
            if ((!Main.hardMode && !DownedBossSystem.killedCrimsonKnight) && Player.getRect().Intersects(Systems.WorldDataSystem.FallenTowerRect)) //prevent messing up the charred tower before hardmode
            {
                Player.AddBuff(BuffID.NoBuilding, 2);
                Player.AddBuff(BuffType<NoGraves>(), 2);
            }
            base.PreUpdate();
        }

        public override void PostUpdateBuffs()
        {
            if (Player.HasBuff(BuffID.NoBuilding) && (!Main.hardMode && !DownedBossSystem.killedCrimsonKnight) && Player.getRect().Intersects(Systems.WorldDataSystem.FallenTowerRect))
            {
                Player.noBuilding = true;
            }
        }

        public override void PostUpdateEquips()
        {
            if (cloakTimer % 60 == 0 && shadowCloak)
            {
                cloakTimer++;
                cloakHP++;
            }
            else if (shadowCloak)
                cloakTimer++;
            else
                cloakTimer = 0;

            if (checkNearbyNPCs) //Loop for checking nearby npcs
            {
                nearbyNPCs.Clear(); //reset here instead of ResetEffects so the array can stay populated with last tick's data durring the equips check
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && (Math.Pow(Player.Center.X - npc.Center.X, 2) + Math.Pow(Player.Center.Y - npc.Center.Y, 2) < 600000)) //This is square distance so ~800
                        nearbyNPCs.Add(npc);
                }
            }

            if (blightCore)
            {
                if (Player.lifeRegen <= 0)
                {
                    Player.GetDamage(DamageClass.Generic) += -0.03f * Player.lifeRegen;
                }
                if (Player.lifeRegen > 0)
                {
                    Player.GetDamage(DamageClass.Generic) += -0.01f * Player.lifeRegen;
                }
            }

            base.PostUpdateEquips();
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (morditeArmor && Main.rand.NextBool(6))
            {
                int numberProjectiles = 7 + Main.rand.Next(2); // 7 to 8 shots
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(Main.rand.Next(-4, 4), Main.rand.Next(-4, 4)).RotatedByRandom(MathHelper.ToRadians(360)); // 360 degree spread.
                    // Stagger difference
                    float scale = 1f - (Main.rand.NextFloat() * .3f);
                    Projectile.NewProjectile(Player.GetSource_Misc("SetBonus_DarksteelArmor"), Player.position.X, Player.position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<Content.Projectiles.DarksteelSkull>(), 50, 2, Player.whoAmI);
                }
            }
            if (shadowCloak && !deadCloak)
                cloakHP -= hurtInfo.Damage;
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (morditeArmor && Main.rand.NextBool(6))
            {
                int numberProjectiles = 7 + Main.rand.Next(2); // 7 to 8 shots
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(Main.rand.Next(-4, 4), Main.rand.Next(-4, 4)).RotatedByRandom(MathHelper.ToRadians(360)); // 360 degree spread.
                    // Stagger difference
                    float scale = 1f - (Main.rand.NextFloat() * .3f);
                    Projectile.NewProjectile(Player.GetSource_Misc("SetBonus_DarksteelArmor"), Player.position.X, Player.position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<Content.Projectiles.DarksteelSkull>(), 50, 2, Player.whoAmI);
                }
            }
            if (shadowCloak && !deadCloak)
                cloakHP -= hurtInfo.Damage;
        }

        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            if (rimestoneArmorHead && Main.rand.NextBool(6))
                return false;
            if (wightQuiver == true && Main.rand.NextBool(11))
                return false;
            return true;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
        {
            if (item.DamageType == DamageClass.Melee && frostStone)
                target.AddBuff(BuffID.Frostburn, 120);
            if (inflictInferno)
            {
                target.AddBuff(BuffType<Content.Buffs.Inferno>(), 60);
                SoundEngine.PlaySound(SoundID.Item100, target.Center);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            if (proj.DamageType == DamageClass.Melee && frostStone)
                target.AddBuff(BuffID.Frostburn, 120);
            if (proj.type == ProjectileID.WoodenArrowFriendly && acidArrows)
                target.AddBuff(BuffType<Content.Buffs.CausticAcid>(), 300);
            if (inflictInferno)
            {
                target.AddBuff(BuffType<Content.Buffs.Inferno>(), 60);
                SoundEngine.PlaySound(SoundID.Item100, target.Center);
            }
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (item.useAmmo == AmmoID.Arrow && wightQuiver)
                Player.GetDamage(DamageClass.Ranged) += .06f;
        }

        public override void PostUpdate()
        {
            if (ScreenMoveTime > 0 && ScreenMoveTarget != Vector2.Zero)
            {
                //cutscene timers
                if (ScreenMoveTimer >= ScreenMoveTime)
                {
                    ScreenMoveTime = 0;
                    ScreenMoveTimer = 0;
                    ScreenMoveTarget = Vector2.Zero;
                    ScreenMovePan = Vector2.Zero;
                }

                if (ScreenMoveTimer < ScreenMoveTime - 30 || !ScreenMoveHold)
                    ScreenMoveTimer++;
            }
        }

        public override void ModifyScreenPosition()
        {
            if (ScreenMoveTime > 0 && ScreenMoveTarget != Vector2.Zero)
            {
                Vector2 off = (new Vector2(Main.screenWidth, Main.screenHeight) / -2) * 1 / ZoomHandler.ClampedExtraZoomTarget;

                if (ScreenMoveTimer <= 30) //go out
                    Main.screenPosition = Vector2.SmoothStep(Main.LocalPlayer.Center + off, ScreenMoveTarget + off, ScreenMoveTimer / 30f);
                else if (ScreenMoveTimer >= ScreenMoveTime - 30) //go in
                    Main.screenPosition = Vector2.SmoothStep((ScreenMovePan == Vector2.Zero ? ScreenMoveTarget : ScreenMovePan) + off, Main.LocalPlayer.Center + off, (ScreenMoveTimer - (ScreenMoveTime - 30)) / 30f);
                else
                {
                    if (ScreenMovePan == Vector2.Zero)
                        Main.screenPosition = ScreenMoveTarget + off; //stay on target

                    else if (ScreenMoveTimer <= ScreenMoveTime - 150)
                        Main.screenPosition = Vector2.Lerp(ScreenMoveTarget + off, ScreenMovePan + off, ScreenMoveTimer / (float)(ScreenMoveTime - 150));

                    else
                        Main.screenPosition = ScreenMovePan + off;
                }
            }
        }

        public override void OnEnterWorld()
        {
            ScreenMoveTime = 0;
            ScreenMoveTarget = Vector2.Zero;
            ScreenMovePan = Vector2.Zero;

            nearbyNPCs = new List<NPC>();
        }
    }
}

