using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

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
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            if (proj.DamageType == DamageClass.Melee && frostStone)
                target.AddBuff(BuffID.Frostburn, 120);
            if (proj.type == ProjectileID.WoodenArrowFriendly && acidArrows)
                target.AddBuff(BuffType<Content.Buffs.CausticAcid>(), 300);
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
        }
    }
}

