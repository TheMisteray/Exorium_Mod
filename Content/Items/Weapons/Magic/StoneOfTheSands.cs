using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Luminance.Common.Utilities;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace ExoriumMod.Content.Items.Weapons.Magic
{
    class StoneOfTheSands : ModItem
    {
        public override string Texture => AssetDirectory.MagicWeapon + Name;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Stone of the Sands");
            /* Tooltip.SetDefault("Creates a ring of sand balls that rotate aroung you \n" +
                "Right click to shoot them forward"); */
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.rare = 1;
            Item.damage = 16;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 7;
            Item.noMelee = true;
            Item.value = Item.sellPrice(silver: 14, copper: 35);
            Item.useStyle = 4;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.knockBack = 4;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item43;
            Item.shoot = ProjectileType<DuneBall>();
            Item.shootSpeed = 5;
        }

        int[] proj = new int[5];

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if (!ExistingCircling(player))
                    return false;
                for (int i = 0; i < 5; i++)
                {
                    Projectile project = Main.projectile[proj[i]];
                    if (project.type == ProjectileType<DuneBall>() && project.owner == player.whoAmI)
                    {
                        project.ai[0] = 1;
                        project.ai[2] = Main.rand.Next(20, 50);
                        project.damage = (int)(project.damage * 1.4f);
                        Vector2 away = project.Center - player.Center;
                        Vector2 back = project.Center - Main.MouseWorld;
                        away.Normalize();
                        back.Normalize();
                        Vector2 newVel = (away + back + back);
                        newVel.Normalize();
                        project.velocity = newVel * 10;
                    }
                }
                Item.mana = 0;
                Item.shoot = ProjectileID.None;
                Item.UseSound = SoundID.Item60;
            }
            else
            {
                if (RemainingCircling(player))
                    return false;
                Item.mana = 7;
                Item.shoot = ProjectileType<DuneBall>();
                Item.UseSound = SoundID.Item43;
            }
            return base.CanUseItem(player);
        }

        private static bool RemainingCircling(Player player)
        {
            foreach (Projectile p in Main.ActiveProjectiles)
            {
                if (p.type == ProjectileType<DuneBall>() && p.owner == player.whoAmI && p.ai[0] == 0)
                    return true;
            }
            return false;
        }

        private static bool ExistingCircling(Player player)
        {
            foreach (Projectile p in Main.ActiveProjectiles)
            {
                if (p.type == ProjectileType<DuneBall>() && p.owner == player.whoAmI && p.ai[0] == 0)
                    return true;
            }
            return false;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 5; i++)
            {
                proj[i] = Projectile.NewProjectile(source, position.X, position.Y, 0, 0, type, damage, knockback, player.whoAmI, 0, 60 * i);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.DunestoneBar>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
    class DuneBall : ModProjectile, IPixelatedPrimitiveRenderer
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 400;
            Projectile.penetrate = -1;
        }

        public float counter
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public float delayTimer
        {
            get => Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }

        private Vector2 cursorPos;
        private float speedDown = 1;

        public override void AI()
        {
            Vector2 dustPosition = Projectile.Center + new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));
            //Making player variable "player" set as the projectile's owner
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[0] == 0 || Projectile.ai[0] == 1)
            {
                Projectile.tileCollide = false;

                float focusX = player.Center.X;
                float focusY = player.Center.Y;
                double deg = (double)counter * 2.4; //Speed
                double rad = deg * (Math.PI / 180); //Convert degrees to radians
                double dist = 64; //Distance away from the player
                Projectile.position.X = focusX - (int)(Math.Cos(rad + 1.5) * dist) - Projectile.width / 2;
                Projectile.position.Y = focusY - (int)(Math.Sin(rad + 1.5) * dist) - Projectile.height / 2;
                //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
                Dust d = Dust.NewDustPerfect(dustPosition, 32, null, 100, default(Color), 0.8f);
                d.noGravity = true;

                if (Projectile.ai[0] == 1)
                {
                    delayTimer--;
                    speedDown -= .02f;
                    counter += 1f * speedDown;
                    if (delayTimer <= 0)
                    {
                        Projectile.ai[0]++;
                        SoundEngine.PlaySound(SoundID.Item43, Projectile.Center);
                    }
                }
                else
                {
                    counter += 1f;
                }
            }
            else
            {
                if (Projectile.ai[0] == 2)
                {
                    //Find cursor and shoot at
                    float maxDistance = 15f; // Speed
                    cursorPos = Main.MouseWorld;

                    Projectile.netUpdate = true;
                    Projectile.penetrate = 1;
                    Projectile.timeLeft = 400;
                    Projectile.ai[0]++;
                }
                else if (Projectile.ai[0] == 3)
                {
                    Vector2 toCursor = (cursorPos - Projectile.Center);
                    toCursor.Normalize();
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, toCursor * 20, .06f);
                    if ((Projectile.Center - cursorPos).LengthSquared() < 1200)
                        Projectile.ai[0]++;
                }
                else
                {
                    Projectile.tileCollide = true;
                }
            }
        }

        public override bool ShouldUpdatePosition()
        {
            return Projectile.ai[0] > 1;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < 8; i++)
            {
                Vector2 coneAngle = Projectile.velocity.RotatedByRandom(MathHelper.PiOver4 / 4);
                coneAngle /= 2;
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Sand, coneAngle.X, coneAngle.Y, 0, default, 1.2f);
            }
            base.OnKill(timeLeft);
        }

        public void RenderPixelatedPrimitives(SpriteBatch spriteBatch)
        {
            if (Projectile.ai[0] < 2)
                return;
            ManagedShader shader = ShaderManager.GetShader("ExoriumMod.BasicTailTrail");
            shader.TrySetParameter("trailColor", Color.SandyBrown);

            Vector2 positionToCenter = Projectile.Size / 2;
            PrimitiveRenderer.RenderTrail(Projectile.oldPos, new(_ => Projectile.width / 2, _ => Color.SandyBrown, _ => positionToCenter, true, true, shader), 4);
        }
    }
}
