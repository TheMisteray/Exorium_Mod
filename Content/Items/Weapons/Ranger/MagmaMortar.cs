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

namespace ExoriumMod.Content.Items.Weapons.Ranger
{
    class MagmaMortar : ModItem
    {
        public override string Texture => AssetDirectory.RangerWeapon + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Launchs Explosive Blobs of Lava\n" +
                "Uses 5 gel per use");
        }

        public override void SetDefaults()
        {
            Item.damage = 21;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 46;
            Item.height = 36;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 7;
            Item.value = Item.sellPrice(gold: 2, silver: 20);
            Item.rare = 3;
            Item.UseSound = SoundID.Item61;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<MagmaBlob>();
            Item.shootSpeed = 10f;
            Item.useAmmo = AmmoID.Gel;
        }

        public override bool CanUseItem(Player player)
        {
            int index = player.FindItem(ItemID.Gel);
            if (index != -1 && player.inventory[index].stack >= 5)
                return true;
            return false;
        }

        public override void OnConsumeAmmo(Item ammo, Player player)
        {
            int index = player.FindItem(ItemID.Gel);
            if (index != -1) //Should never be the case, but just so there are no problems
                player.inventory[index].stack -= 4;
            base.OnConsumeAmmo(ammo, player);
        }
    }

    class MagmaBlob : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + "FlamingSphere";

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            Projectile.velocity.Y -= .02f;
            Projectile.velocity.X *= .998f;
        }

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = -112; i <= 112; i+=16)
                {
                    //Checks for tiles
                    Vector2 positionUp = Projectile.Center + new Vector2(i, 0);
                    Vector2 positionDown = Projectile.Center + new Vector2(i, 0);
                    Vector2 truePosition = Projectile.Center + new Vector2(i, 0);

                    bool startInTile = Main.tile[truePosition.ToTileCoordinates().X, truePosition.ToTileCoordinates().Y].HasTile;
                    for (int j = 0; j < 10; j++)
                    {
                        //If leaves or enters solid
                        if ((!startInTile && Main.tile[positionUp.ToTileCoordinates().X, positionUp.ToTileCoordinates().Y].HasTile) ||
                            startInTile && !Main.tile[positionUp.ToTileCoordinates().X, positionUp.ToTileCoordinates().Y].HasTile)
                        {
                            if (startInTile)
                                Projectile.rotation = MathHelper.Pi;
                            truePosition = positionUp;
                            return;
                        }
                        else
                            positionUp += new Vector2(0, -16);

                        if (!startInTile && Main.tile[positionDown.ToTileCoordinates().X, positionDown.ToTileCoordinates().Y].HasTile)
                        {
                            truePosition = positionDown;
                            return;
                        }
                        else
                            positionDown += new Vector2(0, 16);
                    }

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), truePosition, Vector2.Zero, ProjectileType<LingeringFlame>(), Projectile.damage, 0, Projectile.owner);
                }
            }

            //Dust
            for (int i = 0; i < 20; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.SolarFlare, -i * .5f, 0f, 0, default, 1 + Main.rand.NextFloat(-.5f, .5f));
                Dust d1 = Main.dust[dust];
                d1.noGravity = true;
                d1.color = new Color(184, 58, 24);
                dust = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.SolarFlare, i * .5f, 0f, 0, default, 1 + Main.rand.NextFloat(-.5f, .5f));
                d1 = Main.dust[dust];
                d1.noGravity = true;
                d1.color = new Color(184, 58, 24);
            }

            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            base.Kill(timeLeft);
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

            spriteBatch.Draw(Request<Texture2D>(AssetDirectory.CrimsonKnight + "FlamingSphere").Value, Projectile.Center - Main.screenPosition, null, Color.White, 0, Projectile.Size / 2, .4f, 0, 0);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }
    }

    class LingeringFlame : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + "FlameTrail";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Projectile.timeLeft < 60)
            {
                Projectile.alpha -= 3;
            }
        }
    }
}
