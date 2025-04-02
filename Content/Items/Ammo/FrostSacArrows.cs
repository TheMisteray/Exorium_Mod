using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Ammo
{
    class FrostSacArrows : ModItem
    {
        public override string Texture => AssetDirectory.Ammo + "RimeArrow";

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.value = 2;
            Item.width = 12;
            Item.height = 12;
            Item.rare = 1;
            Item.maxStack = 999;
            Item.damage = 4;
            Item.consumable = true;
            Item.shoot = ProjectileType<FrostSacArrow>();
            Item.shootSpeed = 8;
            Item.ammo = AmmoID.Arrow;
            Item.DamageType = DamageClass.Ranged;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(120);
            recipe.AddIngredient(ItemType<Materials.Metals.RimestoneBar>());
            recipe.AddIngredient(ItemType<Weapons.Ranger.AcidOrb>(), 5);
            recipe.AddIngredient(ItemID.WoodenArrow, 120);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

    class FrostSacArrow : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + "RimeArrow";

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override void AI()
        {
            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 67, 0, 0);
            d.noGravity = true;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            if (Main.netMode != NetmodeID.MultiplayerClient)
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<FrostCloud>(), Projectile.damage/4, 0, Projectile.owner);
        }
    }

    class FrostCloud : ModProjectile
    {
        public override string Texture => AssetDirectory.Glow + Name;

        public override void SetDefaults()
        {
            Projectile.width = 72;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 280;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 20;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
        }

        float drawBalance = 0;
        bool goingUp = true;

        public override void AI()
        {
            if (Projectile.timeLeft > 120)
            {
                if (Projectile.alpha > 100)
                    Projectile.alpha -= 6;
                else
                    Projectile.alpha = 100;
            }
            else
            {
                if (Projectile.alpha < 255)
                    Projectile.alpha += 2;
                else
                    Projectile.timeLeft = 0;
            }

            if (goingUp)
            {
                drawBalance += 0.015f;
                if (drawBalance >= 1)
                {
                    drawBalance = 1;
                    goingUp = false;
                }
            }
            else
            {
                drawBalance -= 0.015f;
                if (drawBalance <= 0)
                {
                    drawBalance = 0;
                    goingUp = true;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(Texture).Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Color.Lerp(new Color(0, 0, 0, 0), new Color(173, 216, 230, 0), (float)(-1 * (Projectile.alpha - 255)) / 255f).MultiplyRGBA(lightColor) * drawBalance, Projectile.rotation, tex.Size() / 2, Projectile.scale, SpriteEffects.None);
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Color.Lerp(new Color(0, 0, 0, 0), new Color(173, 216, 230, 0), (float)(-1 * (Projectile.alpha - 255)) / 255f).MultiplyRGBA(lightColor) * (1 - drawBalance), Projectile.rotation, tex.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally);
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
    }
}
