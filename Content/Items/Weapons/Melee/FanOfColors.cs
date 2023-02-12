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

namespace ExoriumMod.Content.Items.Weapons.Melee
{
    class FanOfColors : ModItem
    {
        public override string Texture => AssetDirectory.MeleeWeapon + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fan of Colors");
            Tooltip.SetDefault("Throw a fan of colored knives");
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
                    Main.projectile[projectile].localAI[1] = i;
                    Main.projectile[projectile].localAI[0] = 5 * i;
                }
                else
                {
                    Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp((rotation/2), -rotation*1.5f, i / (numberProjectiles - 1))) * .2f;
                    int projectile = Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
                    Main.projectile[projectile].localAI[1] = i;
                    Main.projectile[projectile].localAI[0] = 5 * i;
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

    class ColorKnife : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 360;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Projectile.active = true;
            Projectile.alpha = 0;
            if (Projectile.timeLeft == 360)
                Projectile.netUpdate = true;
            if (Projectile.localAI[0] > 0)
            {
                Projectile.alpha = 255;
                Projectile.localAI[0]--;
                switch (Projectile.localAI[1])
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
            Projectile.frame = (int)Projectile.localAI[1];
        }

        public override bool ShouldUpdatePosition()
        {
            return Projectile.localAI[0] <= 0;
        }

        public override bool? CanDamage()/* Suggestion: Return null instead of false */
        {
            return Projectile.localAI[0] <= 0;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct ColorKnifeDrawer
    {
        private static VertexStrip _vertexStrip = new VertexStrip();

        public void Draw(Projectile proj)
        {
            Effect effect = Filters.Scene["KnifeTrail"].GetShader().Shader;
            Ref<Effect> effect2 = new Ref<Effect>(effect);

            MiscShaderData miscShaderData = new MiscShaderData(effect2, "ArmorBasic");
            miscShaderData.UseSaturation(-2.8f);
            miscShaderData.UseOpacity(2f);
            miscShaderData.Apply();
            _vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, StripColors, StripWidth, -Main.screenPosition + proj.Size / 2f);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
        }

        private Color StripColors(float progressOnStrip)
        {
            Color result = Color.Lerp(Color.White, Color.Violet, GetLerpValue(0f, 0.7f, progressOnStrip, clamped: true)) * (1f - GetLerpValue(0f, 0.98f, progressOnStrip));
            result.A /= 2;
            return result;
        }

        private float StripWidth(float progressOnStrip)
        {
            return MathHelper.Lerp(26f, 32f, GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true)) * GetLerpValue(0f, 0.07f, progressOnStrip, clamped: true);
        }

        public static float GetLerpValue(float from, float to, float t, bool clamped = false) //Taken from Lerp
        {
            if (clamped)
            {
                if (from < to)
                {
                    if (t < from)
                    {
                        return 0f;
                    }
                    if (t > to)
                    {
                        return 1f;
                    }
                }
                else
                {
                    if (t < to)
                    {
                        return 1f;
                    }
                    if (t > from)
                    {
                        return 0f;
                    }
                }
            }
            return (t - from) / (to - from);
        }
    }
}
