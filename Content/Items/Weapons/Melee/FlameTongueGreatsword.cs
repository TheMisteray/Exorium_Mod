using ExoriumMod.Core;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using ExoriumMod.Content.Projectiles;
using System.IO;

namespace ExoriumMod.Content.Items.Weapons.Melee
{
    internal class FlameTongueGreatsword : ModItem
    {
        public override string Texture => AssetDirectory.MeleeWeapon + Name;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 76;
            Item.useAnimation = 76;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 3;
            Item.value = Item.sellPrice(gold: 10, silver: 50);
            Item.rare = 5;
            Item.UseSound = SoundID.Item60;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<FlameToungeGreatswordBlade>();
            Item.shootSpeed = 10f;
            Item.noUseGraphic = true;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ProjectileType<FlameToungeGreatswordBlade>()] <= 0;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer, player.direction == 1 ? 3 : 1, player.direction == 1 ? -(float)(Math.PI * .35f) : (float)(Math.PI * .35f), 0);
            return false;
        }
    }
    
    internal class FlameToungeGreatswordBlade : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + "FlametongueSwing";

        private const float AimResponsiveness = 0.06f;
        private const float MinAngle = -(float)(Math.PI * .35f);
        private const float MaxAngle = (float)(Math.PI * .35f);
        private const float SwingDelay = 20;
        private const float SwingSpeed = (float)Math.PI / 20;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 532;
            Projectile.height = 90;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 216089;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.noEnchantmentVisuals = true;
        }

        public float state
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public float swordAngle
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public float timer
        {
            get => Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }

        public float swings
        {
            get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(swings);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            swings = reader.ReadInt32();
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.Center = player.Center;
            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true);

            //Visuals
            UpdatePlayerVisuals(player, rrp);

            if (player.dead)
                Projectile.Kill();

            // Client Side
            if (Main.myPlayer == Projectile.owner)
            {
                //Variables for dust
                Vector2 SwordTip = new Vector2(0, Projectile.width / 2);
                Vector2 SwingLength = SwordTip.RotatedBy(Projectile.rotation - MathHelper.PiOver2);
                Vector2 normal = Vector2.Normalize(Projectile.velocity);

                //Dynamic aim
                UpdateAim(rrp, player.HeldItem.shootSpeed);
                Projectile.netUpdate = true;
                switch (state)
                {
                    case 0: //Swing Up
                        if (timer == 0)
                        {
                            SoundEngine.PlaySound(SoundID.Item100, Projectile.Center);
                            timer++;
                        }

                        if (swordAngle < MaxAngle)
                            swordAngle += SwingSpeed * player.GetAttackSpeed(DamageClass.Melee);
                        else
                        {
                            state = 1;
                            timer = 0;
                            swings++;
                        }

                        if (swings >= 5)
                        {
                            SwordFan(rrp);
                            swings = 0;
                        }

                        //dust
                        for (int i = 0; i < 4; i++)
                        {
                            int dust2 = Dust.NewDust(Projectile.Center + (SwingLength * Main.rand.NextFloat()), 0, 0, DustID.SolarFlare, -normal.Y, normal.X, 1, default, Main.rand.NextFloat(1) + .8f);
                            Main.dust[dust2].noGravity = true;
                        }
                        break;
                    case 1: //hold
                        timer++;
                        if (timer > SwingDelay)
                        {
                            if (player.channel && !player.noItems && !player.CCed) //ends if not channeled, player is cursed, or player is stopped for other reasons
                            {
                                timer = 0;
                                state = 2;
                            }
                            else
                                Projectile.Kill();
                        }
                        break;
                     case 2: //Swing Down
                        if (timer == 0)
                        {
                            SoundEngine.PlaySound(SoundID.Item100, Projectile.Center);
                            timer++;
                        }

                        if (swordAngle > MinAngle)
                            swordAngle -= SwingSpeed * player.GetAttackSpeed(DamageClass.Melee);
                        else
                        {
                            state = 3;
                            timer = 0;
                            swings++;
                        }

                        if (swings >= 5)
                        {
                            SwordFan(rrp);
                            swings = 0;
                        }

                        //dust
                        for (int i = 0; i < 4; i++)
                        {
                            int dust2 = Dust.NewDust(Projectile.Center + (SwingLength * Main.rand.NextFloat()), 0, 0, DustID.SolarFlare, -normal.Y, normal.X, 1, default, Main.rand.NextFloat(1) + .8f);
                            Main.dust[dust2].noGravity = true;
                        }
                        break;
                    case 3: //hold
                        timer++;
                        if (timer > SwingDelay)
                        {
                            if (player.channel && !player.noItems && !player.CCed) //ends if not channeled, player is cursed, or player is stopped for other reasons
                            {
                                timer = 0;
                                state = 0;
                            }
                            else
                                Projectile.Kill();
                        }
                        break;
                }
            }
        }

        private void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
        {
            Projectile.Center = playerHandPos;
            Projectile.rotation = Projectile.velocity.ToRotation() + swordAngle;

            player.ChangeDir(Projectile.direction);

            player.heldProj = Projectile.whoAmI;
            player.itemTime = 15;
            player.itemAnimation = 15;

            //Hand direction
            player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
        }

        private void UpdateAim(Vector2 source, float speed)
        {
            Vector2 aim = Vector2.Normalize(Main.MouseWorld - source);
            if (aim.HasNaNs())
            {
                aim = -Vector2.UnitY;
            }

            // Slow turn to cursor
            //aim = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity.RotatedBy(-swordAngle)), aim, AimResponsiveness));
            aim = aim.RotatedBy(swordAngle); //add swing angle
            aim *= speed;

            if (aim != Projectile.velocity)
            {
                Projectile.netUpdate = true;
            }
            Projectile.velocity = aim;
        }

        public override Nullable<bool> CanDamage()/* tModPorter Suggestion: Return null instead of true *//* Suggestion: Return null instead of false */
        {
            return state == 0 || state == 2;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            //Right half
            hitbox.Y += Projectile.height - Projectile.width / 2;
            hitbox.Height = Projectile.width / 2;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 SwordTip = new Vector2(0, Projectile.width / 2);
            Vector2 hitSpot = SwordTip.RotatedBy(Projectile.rotation - MathHelper.PiOver2);
            float _ = float.NaN;

            //Check collision of line from sword center to sword end with target hitbox
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + hitSpot, (Projectile.height/2) * Projectile.scale, ref _);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public void SwordFan(Vector2 source)
        {
            Vector2 aim = Vector2.Normalize(Main.MouseWorld - source);

            for (float i = -2; i <= 2; i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), source, (aim * 20).RotatedBy((Math.PI / 18) * i), ProjectileType<FlameToungeGreatswordBeam>(), (int)(Projectile.damage * 1.5), Projectile.knockBack, Projectile.owner, 0, 60);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 300);
            base.OnHitNPC(target, hit, damageDone);
        }
    }
    
    internal class FlameToungeGreatswordBeam : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + "CaraveneBladeProj";
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 300;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.scale = .7f;
            Projectile.alpha = 255;
            Projectile.usesIDStaticNPCImmunity = true;
        }

        public bool FadeState
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value ? 1f : 0f;
        }

        public float fadeIn
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void AI()
        {

            if (Projectile.timeLeft == 300)
            {
                Projectile.netUpdate = true;
            }
            if (FadeState)
            {
                Projectile.timeLeft = 10;
                fadeIn += 2;
                Projectile.velocity *= .94f;

                if (fadeIn > 60)
                    Projectile.Kill();
            }
            else
            {
                if (fadeIn > 0)
                    fadeIn-=3;
                Vector2 normal = Vector2.Normalize(Projectile.velocity);
                int dust = Dust.NewDust(Projectile.Center + (normal * Main.rand.NextFloat(-Projectile.width / 2, Projectile.width / 2)) - new Vector2((Projectile.height/ 4) * Projectile.scale, (Projectile.height/ 4) * Projectile.scale), (int)(Projectile.height / 2 * Projectile.scale), (int)(Projectile.height / 2 * Projectile.scale), DustID.Torch, Projectile.velocity.X, Projectile.velocity.Y);
                Main.dust[dust].noGravity = true;
            }
            base.AI();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 300);
            FadeState = true;
            base.OnHitNPC(target, hit, damageDone);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (FadeState)
                return false;
            Vector2 SwordTip = new Vector2(0, Projectile.width / 2);
            SwordTip = SwordTip.RotatedBy(Projectile.velocity.ToRotation() - MathHelper.PiOver2);
            float _ = float.NaN;

            //Check collision of line from sword center to sword end with target hitbox
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - SwordTip, Projectile.Center + SwordTip, 42 * Projectile.scale, ref _);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.CrimsonKnight + "CaraveneBladeProj").Value;

            Main.EntitySpriteDraw(tex, (Projectile.Center - Main.screenPosition), null, new Color(254, 121, 2) * ((60 - fadeIn) / 60), Projectile.velocity.ToRotation() - MathHelper.PiOver2, new Vector2(tex.Width / 2, tex.Height / 2), Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
