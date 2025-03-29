using ExoriumMod.Core;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Content.Items.Weapons.Magic
{
    class SettlingSands : ModItem
    {
        public override string Texture => AssetDirectory.MagicWeapon + Name;

        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Launches bouncing sand balls \n" +
                "\"This is a horrible idea\""); */
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 17;
            Item.width = 15;
            Item.height = 15;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 8;
            Item.useTime = 29;
            Item.useAnimation = 29;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = Item.sellPrice(silver: 14);
            Item.rare = 1;
            Item.UseSound = SoundID.Item42;
            Item.shoot = ProjectileType<SandShot>();
            Item.shootSpeed = 16;
            Item.autoReuse = true;
            Item.scale = 0.9f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.DunestoneBar>(), 8);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }
    }
    class SandShot : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.penetrate = 3;
            Projectile.friendly = true;
        }

        public override void AI()
        {
            if (Projectile.velocity.X > 0)
                Projectile.velocity.X -= 0.06f;
            else
                Projectile.velocity.X += 0.06f;

            Projectile.velocity.Y += 0.3f;
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Sand);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            Projectile.velocity.Y *= 0.75f;
            Projectile.velocity.X *= 0.75f;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, 0, 0, ProjectileType<SandRing>(), Projectile.damage/2, 0, Projectile.owner, 0.8f);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.velocity.X = -Projectile.velocity.X;
            Projectile.velocity.Y = -Projectile.velocity.Y;
            Projectile.velocity.Y *= 0.75f;
            Projectile.velocity.X *= 0.75f;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, 0, 0, ProjectileType<SandRing>(), Projectile.damage / 2, 0, Projectile.owner, 0.8f);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            base.OnHitNPC(target, hit, damageDone);
        }
    }

    class SandRing : ModProjectile
    {
        public override string Texture => AssetDirectory.Glow + Name;

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 400;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 20;
        }

        private float counter = 0;

        public override void AI()
        {
            Projectile.rotation += 0.02f;
            Projectile.ai[0] += 0.003f;
            Projectile.scale = Projectile.ai[0];
            counter++;
            if (counter > 10)
            {
                Projectile.alpha += 4;
                if (Projectile.alpha >= 250)
                    Projectile.timeLeft = 0;
            }
            base.AI();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(Texture).Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Color.Lerp(new Color(0, 0, 0, 0), new Color(244, 164, 96, 0), (float)(-1 * (Projectile.alpha - 255)) / 255f).MultiplyRGBA(lightColor), Projectile.rotation, tex.Size() / 2, Projectile.scale, SpriteEffects.None);
            return false;
        }
    }
}
