using ExoriumMod.Core;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using System;
using ExoriumMod.Helpers;
using Microsoft.Win32;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Weapons.Melee
{
    class InfernalSledge : ModItem
    {
        public override string Texture => AssetDirectory.MeleeWeapon + Name;

        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("A hammer with an explosive impact\n" +
                "Hold longer for a stronger slam"); */
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.DamageType = DamageClass.Melee;
            Item.width = 64;
            Item.height = 64;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useStyle = 1;
            Item.knockBack = 6;
            Item.rare = 4;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.value = Item.sellPrice(gold: 2, silver: 20);
            Item.channel = true;
            Item.shoot = ProjectileType<HeldInfernalSledge>();
            Item.shootSpeed = 10f;
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ProjectileType<HeldInfernalSledge>()] <= 0;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, player.Center, (player.direction == 1) ? Vector2.UnitX.RotatedBy(MathHelper.PiOver4 * -3) : Vector2.UnitX.RotatedBy(MathHelper.PiOver4 * 3) * -1, ProjectileType<HeldInfernalSledge>(), damage, knockback, player.whoAmI, 0, 0);
            return false;
        }
    }

    class HeldInfernalSledge : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Infernal Sledge");
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 128;
            Projectile.height = 128;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 216089;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public float state
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public float strength
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
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
            if (Main.myPlayer == Projectile.owner && state == 0f)
            {
                if (player.channel && !player.noItems && !player.CCed) //ends if not channeled, player is cursed, or player is stopped for other reasons
                {
                    Projectile.netUpdate = true;
                    if (Projectile.timeLeft % 30 == 0 && strength < 5)
                    {
                        strength++;
                        if (strength == 5)
                        {
                            DustHelper.DustCircle(Projectile.Center + Projectile.velocity * 64, DustID.Torch, 3, 30, 1, .2f, 0, 0, Color.White, true);
                            SoundEngine.PlaySound(SoundID.MaxMana, Projectile.Center);
                        }
                        else
                        {
                            DustHelper.DustCircle(Projectile.Center + Projectile.velocity * 64, DustID.Torch, 1, 10, .7f, .1f, 0, 0, Color.White, true);
                        }
                    }
                    //This also affects the visual position of the hammer
                    Projectile.velocity = (player.direction == 1) ? Vector2.UnitX.RotatedBy(MathHelper.PiOver4 * -3) : Vector2.UnitX.RotatedBy(MathHelper.PiOver4 * 3) * -1;
                    //Lighting to scale with strength charge
                    Lighting.AddLight(Projectile.Center + Projectile.velocity, .15f * (strength + 1), .105f * (strength + 1), 0);
                }
                // Move to swing
                else if (state == 0f)
                {
                    SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                    state = 1f;
                    Projectile.timeLeft = 45;
                }
            }
            else if (state == 1f)
            {
                if (strength == 0f)
                    Projectile.Kill();
                Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(9f * Projectile.direction));
                //after frame is increased, so just as the final frame is reached
                if (Projectile.timeLeft == 30)
                {
                    //Projectiles
                    for (int i = 0; i < 3 + strength * 2; i++)
                    {
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity * 64, Projectile.velocity * 10, ProjectileType<InfernalTrails>(), Projectile.damage, 1, Projectile.owner, Projectile.velocity.X, Projectile.velocity.Y);
                        Main.projectile[proj].timeLeft = 20 + (int)strength * 20 + 1;
                    }
                    //Extra projectiles in random directins for effect
                    /* THIS CRASHES THE GAME FOR SOME REASON?!?!?!
                    for (float i = 0; i < MathHelper.TwoPi; i += ((MathHelper.TwoPi) / (3 + strength)))
                    {
                        Vector2 trajectory = Projectile.velocity * (3 + .5f * strength);
                        int projExtra = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity * 64, trajectory.RotatedBy(i), ProjectileType<InfernalTrails>(), Projectile.damage, 1, Projectile.owner, Projectile.velocity.X, Projectile.velocity.Y);
                        Main.projectile[projExtra].timeLeft = 20;
                        Main.projectile[projExtra].damage = 0;
                    }*/
                    //dust
                    DustHelper.DustCircle(Projectile.Center + Projectile.velocity * 64, DustID.Torch, 3, 20 + strength * 10, 1, .2f, 0, 0, Color.White, true);

                    //ending
                    SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
                    state = 2f;
                }
                Lighting.AddLight(Projectile.Center + Projectile.velocity, .2f * (strength + 2), .14f * (strength + 2), 0);
            }
            else if (state == 2f)
            {
                state = 3f;
                Projectile.timeLeft = 30;
            }
        }

        private void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
        {
            Projectile.Center = playerHandPos;
            Projectile.rotation = Projectile.velocity.RotatedBy(MathHelper.Pi/4).ToRotation();

            if (state == 0f) //once swing has begun stop player from changing directions
                player.ChangeDir((Main.MouseWorld.X > player.Center.X) ? 1 : -1);
            Projectile.direction = player.direction;

            player.heldProj = Projectile.whoAmI;
            player.itemTime = 15;
            player.itemAnimation = 15;

            //Hand direction
            player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
        }

        public override Nullable<bool> CanDamage()/* tModPorter Suggestion: Return null instead of true */
        {
            return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool PreDrawExtras()
        {
            //Fire Aura
            var fire = Filters.Scene["ExoriumMod:FireAura"].GetShader().Shader;
            fire.Parameters["noiseTexture"].SetValue(Request<Texture2D>(AssetDirectory.ShaderMap + "FlamingSphere").Value);
            fire.Parameters["gradientTexture"].SetValue(Request<Texture2D>(AssetDirectory.ShaderMap + "basicGradient").Value);
            fire.Parameters["uTime"].SetValue(Main.GameUpdateCount * 0.02f);
            Texture2D auraTex = Request<Texture2D>(AssetDirectory.Effect + "InfernalSledgeFire").Value;

            SpriteBatch spriteBatch = Main.spriteBatch;

            if (state != 2)
            {
                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.NonPremultiplied, default, default, default, fire, Main.GameViewMatrix.ZoomMatrix);

                Main.EntitySpriteDraw(auraTex, Projectile.Center + new Vector2(0, Projectile.width / 2).RotatedBy(Projectile.rotation - MathHelper.PiOver4 * 3) - Main.screenPosition + new Vector2(0, -auraTex.Height / 3), null, Color.White, 0, auraTex.Size() / 2, .8f + (.2f * strength), SpriteEffects.None, 0);

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
            }

            return base.PreDrawExtras();
        }
    }

    class InfernalTrails : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Infernal Sledge");
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }

        public float speedX
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public float speedY
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void AI()
        {
            if (Projectile.timeLeft % 20 == 0)
            {
                Vector2 alter;
                do
                {
                    alter = Projectile.velocity.RotatedByRandom(MathHelper.PiOver4);
                }
                while ((alter.ToRotation() - new Vector2(speedX, speedY).ToRotation()) > MathHelper.PiOver4); //New rotation cannot be too far outside cone around original velocity
                Projectile.velocity = alter;
            }

            Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, null, 0, default, 2);
            dust.noGravity = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 420);
        }
    }
}
