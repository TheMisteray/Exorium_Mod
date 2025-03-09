using ExoriumMod.Core;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.GameContent.Creative;
using ExoriumMod.Core.Utilities;

namespace ExoriumMod.Content.Items.Weapons.Ranger
{
    class MagmaMortar : ModItem
    {
        public override string Texture => AssetDirectory.RangerWeapon + Name;

        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Launches explosive blobs of lava\n" +
                "Sets fires nearby\n" +
                "Consumes 5 gel per use"); */
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 27;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 46;
            Item.height = 36;
            Item.useTime = 90;
            Item.useAnimation = 90;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 7;
            Item.value = Item.sellPrice(gold: 2, silver: 20);
            Item.rare = 4;
            Item.UseSound = SoundID.Item61;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<MagmaBlob>();
            Item.shootSpeed = 14f;
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
            {
                if (player.inventory[index].stack >= 5)
                    player.inventory[index].stack -= 4;
                else
                    player.inventory[index].stack = 1;
            }
            base.OnConsumeAmmo(ammo, player);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
    }

    class MagmaBlob : ModProjectile
    {
        //Commented out stuff is from scrapped version of weapon 
        public override string Texture => AssetDirectory.CrimsonKnight + "FlamingSphere";

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            Projectile.velocity.Y += .16f;
            Projectile.velocity.X *= .99f;
            Lighting.AddLight(Projectile.Center, 1, .7f, 0);
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<Bosses.CrimsonKnight.gridCollision>(), Projectile.damage/2, 0, Projectile.owner);
                Main.projectile[proj].hostile = false;
                Main.projectile[proj].friendly = true;
                Main.projectile[proj].timeLeft = 120;

                /*for (int i = -112; i <= 112; i+=32)
                {
                    //Checks for tiles
                    Vector2 positionUp = Projectile.Center + new Vector2(i, 0);
                    Vector2 positionDown = Projectile.Center + new Vector2(i, 0);
                    Vector2 truePosition = Projectile.Center + new Vector2(i, 0);
                    Vector2 origin = truePosition;

                    bool found = false;
                    bool flip = false;
                    Tile start = Main.tile[truePosition.ToTileCoordinates().X, truePosition.ToTileCoordinates().Y];
                    bool startInTile = start.HasUnactuatedTile && Main.tileSolid[(int)start.BlockType];
                    for (int j = 0; j < 10; j++)
                    {
                        if (found)
                            continue;
                        //If leaves or enters solid
                        Tile tile = Main.tile[positionUp.ToTileCoordinates().X, positionUp.ToTileCoordinates().Y];
                        Tile tileDown = Main.tile[positionDown.ToTileCoordinates().X, positionDown.ToTileCoordinates().Y];
                        if ((!startInTile && tile.HasUnactuatedTile) && Main.tileSolid[((int)tile.BlockType)] ||
                            startInTile && !tile.HasUnactuatedTile)
                        {
                            if (!startInTile)
                            {
                                positionUp += new Vector2(0, 16);
                                flip = true;
                            }
                            truePosition = positionUp;
                            found = true;
                        }
                        else
                            positionUp += new Vector2(0, -16);

                        if (!startInTile && tileDown.HasUnactuatedTile && Main.tileSolid[((int)tileDown.BlockType)] ||
                            startInTile && !tileDown.HasUnactuatedTile)
                        {
                            if (!startInTile)
                                positionDown += new Vector2(0, -16);
                            else
                                flip = true;
                            truePosition = positionDown;
                            found = true;
                        }
                        else
                            positionDown += new Vector2(0, 16);
                    }

                    if (found)
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), truePosition, Vector2.Zero, ProjectileType<LingeringFlame>(), Projectile.damage, 0, Projectile.owner, flip? 1:0);
                }*/
            }

            //Dust
            /*for (int i = 0; i < 40; i++)
            {
                
                int dust = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.SolarFlare, -i * .3f, 0f, 0, default, 1);
                Dust d1 = Main.dust[dust];
                d1.noGravity = true;
                d1.color = new Color(184, 58, 24);
                dust = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.SolarFlare, i * .3f, 0f, 0, default, 1);
                d1 = Main.dust[dust];
                d1.noGravity = true;
                d1.color = new Color(184, 58, 24);
            }*/

            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            base.OnKill(timeLeft);
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

            Texture2D tex = Request<Texture2D>(AssetDirectory.CrimsonKnight + "FlamingSphere").Value;
            spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, 0, tex.Size() / 2, .3f, 0, 0);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 420);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
    }

/*    class LingeringFlame : ModProjectile
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
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            if (Projectile.timeLeft < 60)
            {
                Projectile.alpha += 4;
            }

            Projectile.frameCounter++;

            //Frame loop
            if (Projectile.frameCounter >= 8)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 3;
            }

            if (Projectile.ai[0] == 1)
                Projectile.rotation = MathHelper.Pi;

            Lighting.AddLight(Projectile.Center, .75f * (float)((255 - Projectile.alpha) / 255), .45f * (float)((255 - Projectile.alpha) / 255), 0);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 420);
        }
    }*/
}
