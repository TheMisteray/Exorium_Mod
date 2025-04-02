using ExoriumMod.Core;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Luminance.Common.Utilities;
using Terraria.DataStructures;
using ReLogic.Content;
using Terraria.Audio;
using ExoriumMod.Content.Dusts;

namespace ExoriumMod.Content.Items.Weapons.Melee
{
    class FrostedBoomerang : ModItem
    {
        public override string Texture => AssetDirectory.MeleeWeapon + Name;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Frosted Chackram");
            // Tooltip.SetDefault("Inflicts Frostburn");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WoodenBoomerang);
            Item.shoot = ProjectileType<RimeBoomerang>();
            Item.damage = 21;
            Item.useTime = 34;
            Item.useAnimation = 34;
            Item.autoReuse = true;
            Item.rare = 1;
            Item.value = Item.sellPrice(silver: 14);
            Item.UseSound = SoundID.Item1;
            Item.shootSpeed = 15;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.RimestoneBar>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

    }

    class RimeBoomerang : ModProjectile
    {
        public override string Texture => AssetDirectory.MeleeWeapon + "FrostedBoomerang";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Frosted Chackram");
        }

        public override void SetDefaults()
        {
            Projectile.timeLeft = 22;
            Projectile.height = 38;
            Projectile.width = 38;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }

        public override void AI()
        {
            Projectile.rotation+= Projectile.direction;
            if (Main.rand.NextBool(10))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 67, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
                d.noGravity = true;
            }

            if (Projectile.timeLeft == 1)
            {
                SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    float num = Main.rand.Next(2, 4);
                    for (int i = 0; i < num; i++)
                    {
                        Vector2 bonus = Projectile.velocity;
                        bonus.Normalize();
                        bonus *= Main.rand.NextFloat(5);
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, (Projectile.velocity + bonus).RotatedByRandom(MathHelper.ToRadians(18)), ProjectileType<RimeBoomerangShrapnel>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 300, true);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
                target.AddBuff(BuffID.Frostburn, 300, true);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 67, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 0, default, .5f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            return base.OnTileCollide(oldVelocity);
        }
    }

    class RimeBoomerangShrapnel : ModProjectile, IPixelatedPrimitiveRenderer
    {
        public override string Texture => AssetDirectory.MeleeWeapon + "FrostedBoomerang";
        Texture2D texToUse;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.timeLeft = 300;
            Projectile.height = 18;
            Projectile.width = 18;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }

        public override void AI()
        {
            Projectile.rotation += Projectile.direction;
            Projectile.velocity.X *= 0.98f;
            Projectile.velocity.Y += 0.6f;
            if (Main.rand.NextBool(30))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 67, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    texToUse = Request<Texture2D>(AssetDirectory.Projectile + "FrostedBoomerangShrapnel1").Value;
                    break;
                case 1:
                    texToUse = Request<Texture2D>(AssetDirectory.Projectile + "FrostedBoomerangShrapnel2").Value;
                    break;
                case 2:
                default:
                    texToUse = Request<Texture2D>(AssetDirectory.Projectile + "FrostedBoomerangShrapnel3").Value;
                    break;
            }
            base.OnSpawn(source);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.EntitySpriteDraw(texToUse, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texToUse.Size() / 2, 1, SpriteEffects.None);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 300, true);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
                target.AddBuff(BuffID.Frostburn, 300, true);
        }

        public override void OnKill(int timeLeft)
        {
            Helpers.DustHelper.DustRing(Projectile.Center, DustType<Rainbow>(), Projectile.width / 5, 0, .2f, 1, 0, 0, 0, Color.LightBlue, true);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            return base.OnTileCollide(oldVelocity);
        }

        public void RenderPixelatedPrimitives(SpriteBatch spriteBatch)
        {
            ManagedShader shader = ShaderManager.GetShader("ExoriumMod.BasicTailTrail");
            shader.TrySetParameter("trailColor", Color.LightBlue);

            Vector2 positionToCenter = Projectile.Size / 2;
            PrimitiveRenderer.RenderTrail(Projectile.oldPos, new(_ => Projectile.width / 3, _ => Color.LightBlue, _ => positionToCenter, true, true, shader), 4);
        }
    }
}
