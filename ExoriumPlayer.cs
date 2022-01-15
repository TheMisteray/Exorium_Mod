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

namespace ExoriumMod
{
    class ExoriumPlayer : ModPlayer
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

        public override void UpdateBiomes()
        {
            ZoneDeadlands = ExoriumWorld.deadlandTiles > 800;
        }

        public override bool CustomBiomesMatch(Player other)
        {
            ExoriumPlayer modOther = other.GetModPlayer<ExoriumPlayer>();
            return ZoneDeadlands == modOther.ZoneDeadlands;
        }

        public override void SendCustomBiomes(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = ZoneDeadlands;
            writer.Write(flags);
        }

        public override void ReceiveCustomBiomes(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            ZoneDeadlands = flags[0];
        }

        public override Texture2D GetMapBackgroundImage()
        {
            if (ZoneDeadlands)
            {
                return mod.GetTexture("DeadlandsBackground");
            }
            return null;
        }

        public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (shadowCloak && !deadCloak)
            {
                if (Main.rand.NextBool(4) && drawInfo.shadow == 0f)
                {
                    int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, DustType<Shadow>(), player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100, default(Color), 2f);
                    Main.dust[dust].noGravity = true;
                    Main.playerDrawDust.Add(dust);
                }
            }
            if (ritualArrow)
            {
                Texture2D tex = GetTexture("ExoriumMod/Projectiles/RitualArrow");
                float scale = 1;
                float disappearRange = 240;
                float shrinkRange = 600;

                //Rotation to point to Shadow Altar, super inelegant way to do this but I couldn't find another way so far. (I'd have to get data from ExoriumWorld to do so from what I can tell)
                Vector2 shadowAltar = new Vector2(ExoriumWorld.shadowAltarCoordsX, ExoriumWorld.shadowAltarCoordsY).ToWorldCoordinates();
                Vector2 toAltar = shadowAltar - player.Center;
                float rotation = toAltar.ToRotation() - MathHelper.ToRadians(45);
                if (toAltar.Length() > disappearRange)
                {
                    if (toAltar.Length() < shrinkRange) //Shrink when close
                        scale = (Math.Abs(toAltar.Length()) - disappearRange) / (shrinkRange - disappearRange);
                    Main.spriteBatch.Draw(tex, new Vector2(player.Center.X - Main.screenPosition.X, player.Center.Y - Main.screenPosition.Y), new Rectangle(0, 0, tex.Width, tex.Height), Color.White, rotation, Vector2.Zero, scale, 0, 0);
                }
                
            }
        }

        public static readonly PlayerLayer MiscEffectsBack = new PlayerLayer("ExoriumMod", "MiscEffectsBack", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo) 
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
        });

        public static readonly PlayerLayer MiscEffects = new PlayerLayer("ExoriumMod", "MiscEffects", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo) 
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("ExoriumMod");
            ExoriumPlayer modPlayer = drawPlayer.GetModPlayer<ExoriumPlayer>();

            if (modPlayer.shield)
            {
                Texture2D tex = GetTexture("ExoriumMod/Projectiles/Shield");

                DrawData data = new DrawData(tex, new Vector2(drawPlayer.Center.X - Main.screenPosition.X - tex.Width, drawPlayer.Center.Y - Main.screenPosition.Y - tex.Height), new Rectangle(0, 0, tex.Width, tex.Height), new Color(40, 140, 250), 0, Vector2.Zero, 2, 0, 0);
                Main.playerDrawData.Add(data);
            }
        });

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            MiscEffectsBack.visible = true;
            layers.Insert(0, MiscEffectsBack);
            MiscEffects.visible = true;
            layers.Add(MiscEffects);
        }
    }
}

