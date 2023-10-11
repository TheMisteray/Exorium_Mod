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
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Weapons.Magic
{
    internal class BurningSphere : ModItem
    {
        public override string Texture => AssetDirectory.MagicWeapon + Name;

        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Conjure a slowly moving sphere of flames");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.width = 32;
            Item.height = 42;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 12;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(silver: 77); ;
            Item.rare = 3;
            Item.UseSound = SoundID.Item45;
            Item.shoot = ProjectileType<BurningSphereProjectile>();
            Item.shootSpeed = 20;
            Item.autoReuse = false;
            Item.scale = 0.9f;
            Item.channel = true;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ProjectileType<BurningSphereProjectile>()] <= 0;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, ProjectileType<BurningSphereProjectile>(), damage, knockback, player.whoAmI, 0, 0);
            return false;
        }
    }

    internal class BurningSphereProjectile : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + "FlamingSphere";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Burning Sphere");
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 200;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
        }

        public float scalar
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public bool EndChannel
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value ? 1f : 0f;
        }
        private Vector2 targetPoint;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true);
            Vector2 trajectory = Main.MouseWorld - Projectile.Center;

            //Visuals
            if (!EndChannel)
                UpdatePlayerVisuals(player, rrp);

            // Client Side
            if (Main.myPlayer == Projectile.owner)
            {
                if (player.channel && !player.noItems && !player.CCed && !player.dead) //ends if not channeled, player is cursed, or player is stopped for other reasons
                {
                    float inertia = 10f;
                    Projectile.timeLeft = 300;
                    Projectile.netUpdate = true;
                    Projectile.velocity = trajectory / 12;

                    if (scalar < 1)
                    {
                        scalar += .02f;
                    }

                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + (trajectory/12)) / inertia;
                    if (Projectile.velocity.Length() > 5)
                    {
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= 4;
                    }
                }
                else
                {
                    EndChannel = true;
                    Projectile.netUpdate = true;
                    scalar -= .02f;
                    if (scalar < 0)
                    {
                        Projectile.timeLeft = 0;
                    }
                }
            }
        }

        private void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
        {
            player.ChangeDir((Math.Abs((Projectile.Center - player.Center).ToRotation()) > MathHelper.PiOver2)? 0: 1);

            player.itemTime = 15;

            //Hand direction
            player.itemRotation = (Projectile.Center - player.Center).ToRotation();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            
            var fire = Filters.Scene["ExoriumMod:FlamingSphere"].GetShader().Shader;
            fire.Parameters["sampleTexture2"].SetValue(Request<Texture2D>(AssetDirectory.ShaderMap + "FlamingSphere").Value);
            fire.Parameters["sampleTexture3"].SetValue(Request<Texture2D>(AssetDirectory.ShaderMap + "FlamingSphere").Value);
            fire.Parameters["uTime"].SetValue(Main.GameUpdateCount * 0.01f);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.NonPremultiplied, default, default, default, fire, Main.GameViewMatrix.ZoomMatrix);

            spriteBatch.Draw(Request<Texture2D>(AssetDirectory.CrimsonKnight + "FlamingSphere").Value, Projectile.Center - Main.screenPosition, null, Color.White, 0, Projectile.Size / 2, scalar * 1.2f, 0, 0);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (((target.Center - Projectile.Center).Length() < (target.width / 2) + (Projectile.width / 3) * scalar))
                return base.CanHitNPC(target);
            else
                return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 600);
        }
    }
}
