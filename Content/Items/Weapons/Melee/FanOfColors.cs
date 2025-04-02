using ExoriumMod.Core;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using System;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Luminance.Core.Graphics;

namespace ExoriumMod.Content.Items.Weapons.Melee
{
    class FanOfColors : ModItem
    {
        public override string Texture => AssetDirectory.MeleeWeapon + Name;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fan of Colors");
            // Tooltip.SetDefault("Throw a fan of colored knives");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 17;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 76;
            Item.useAnimation = 76;
            Item.useStyle = 1;
            Item.knockBack = 3;
            Item.value = Item.sellPrice(gold: 3, silver: 75);
            Item.rare = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<ColorKnife>();
            Item.shootSpeed = 48f;
            Item.useTurn = true;
            Item.noUseGraphic = true;
        }

        private bool mode = true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float numberProjectiles = 7;
            float rotation = MathHelper.ToRadians(15);
            position += Vector2.Normalize(velocity) * 45f;
            for (int i = 0; i<numberProjectiles; i++)
            {
                if (mode)
                {
                    Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-(rotation/2), 1.5f*rotation, i / (numberProjectiles - 1))) * .2f;
                    int projectile = Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
                    Main.projectile[projectile].ai[1] = i;
                    Main.projectile[projectile].ai[0] = 5 * i;
                }
                else
                {
                    Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp((rotation/2), -rotation*1.5f, i / (numberProjectiles - 1))) * .2f;
                    int projectile = Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
                    Main.projectile[projectile].ai[1] = i;
                    Main.projectile[projectile].ai[0] = 5 * i;
                }
            }
            mode = !mode;
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Amethyst);
            recipe.AddIngredient(ItemID.Topaz);
            recipe.AddIngredient(ItemID.Sapphire);
            recipe.AddIngredient(ItemID.Emerald);
            recipe.AddIngredient(ItemID.Ruby);
            recipe.AddIngredient(ItemID.Diamond);
            recipe.AddIngredient(ItemID.Amber);
            recipe.AddIngredient(ItemID.ThrowingKnife, 100);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

    class ColorKnife : ModProjectile, IPixelatedPrimitiveRenderer
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 7;
            ProjectileID.Sets.TrailCacheLength[Type] = 6;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 360;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Projectile.active = true;
            Projectile.alpha = 0;
            if (Projectile.timeLeft == 360)
                Projectile.netUpdate = true;
            if (Projectile.ai[0] > 0)
            {
                Projectile.alpha = 255;
                Projectile.ai[0]--;
                Projectile.position = Main.player[Projectile.owner].Center + Projectile.Size * -0.5f + Vector2.Normalize(Projectile.velocity) * 45f; //stick to owner
                switch (Projectile.ai[1])
                {
                    case 0:
                        Lighting.AddLight(Projectile.position, 255 * 0.002f, 0 * 0.002f, 0 * 0.002f);
                        break;
                    case 1:
                        Lighting.AddLight(Projectile.position, 255 * 0.002f, 110 * 0.002f, 0 * 0.002f);
                        break;
                    case 2:
                        Lighting.AddLight(Projectile.position, 255 * 0.002f, 247 * 0.002f, 0 * 0.002f);
                        break;
                    case 3:
                        Lighting.AddLight(Projectile.position, 0 * 0.002f, 255 * 0.002f, 0 * 0.002f);
                        break;
                    case 4:
                        Lighting.AddLight(Projectile.position, 0 * 0.002f, 255 * 0.002f, 204 * 0.002f);
                        break;
                    case 5:
                        Lighting.AddLight(Projectile.position, 35 * 0.002f, 0 * 0.002f, 255 * 0.002f);
                        break;
                    case 6:
                        Lighting.AddLight(Projectile.position, 149 * 0.002f, 0 * 0.002f, 255 * 0.002f);
                        break;
                }
            }
            else
            {
                Projectile.tileCollide = true;
            }
            Projectile.frame = (int)Projectile.ai[1];
        }

        public override bool ShouldUpdatePosition()
        {
            return Projectile.ai[0] <= 0;
        }

        public override Nullable<bool> CanDamage()/* tModPorter Suggestion: Return null instead of true *//* Suggestion: Return null instead of false */
        {
            return Projectile.ai[0] <= 0;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[0] <= 0)
                Main.EntitySpriteDraw(Request<Texture2D>(Texture).Value, Projectile.Center - Main.screenPosition, new Rectangle(0, (int)(Projectile.height * Projectile.ai[1]), Projectile.width, Projectile.height), Color.White, Projectile.rotation, new Vector2(Projectile.width, Projectile.height) / 2, 1, SpriteEffects.None);
            return false;
        }

        public void RenderPixelatedPrimitives(SpriteBatch spriteBatch)
        {
            if (Projectile.ai[0] > 0)
                return;

            Color trailColor = Color.White;
            switch (Projectile.ai[1])
            {
                case 0:
                    trailColor = new Color(255, 0, 0, 0);
                    break;
                case 1:
                    trailColor = new Color(255, 110, 0, 0);
                    break;
                case 2:
                    trailColor = new Color(255, 247, 0, 0);
                    break;
                case 3:
                    trailColor = new Color(0, 255, 0, 0);
                    break;
                case 4:
                    trailColor = new Color(0, 255, 204, 0);
                    break;
                case 5:
                    trailColor = new Color(35, 0, 255, 0);
                    break;
                case 6:
                    trailColor = new Color(149, 0, 255, 0);
                    break;
            }

            ManagedShader shader = ShaderManager.GetShader("ExoriumMod.BasicTailTrail");
            shader.TrySetParameter("trailColor", trailColor);

            Vector2 positionToCenter = Projectile.Size/ 2;
            PrimitiveRenderer.RenderTrail(Projectile.oldPos, new(_ => Projectile.width/4, _ => trailColor, _ => positionToCenter, true, true, shader), 4);
        }
    }
}
