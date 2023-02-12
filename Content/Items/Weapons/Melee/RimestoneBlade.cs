using ExoriumMod.Core;
using ExoriumMod.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using System;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Weapons.Melee
{
    class RimestoneBlade : ModItem
    {
        public override string Texture => AssetDirectory.MeleeWeapon + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Inflicts Frostburn \n" +
                "Striking targets has a chance to build up energy up to 5 times \n" +
                "Right click to release the built up energy");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 11;
            Item.DamageType = DamageClass.Melee;
            Item.width = 34;
            Item.height = 34;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = 1;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(silver: 14);
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.scale = 2f;
            Item.useTurn = true;
            Item.shoot = ProjectileType<RimeBladeProj>();
            Item.shootSpeed = 10;
        }

        private int frost = 0;

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
                Item.noMelee = true;
            else
                Item.noMelee = false;
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2 && frost/5 > 0)
            {
                int proj1 = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, Mod.Find<ModProjectile>("RimeBladeProj").Type, damage *= (frost/5) * 3, knockback, player.whoAmI, frost/5);
                Main.projectile[proj1].position = Main.projectile[proj1].Center;
                Main.projectile[proj1].width *= (frost / 5);
                Main.projectile[proj1].height *= (frost / 5);
                Main.projectile[proj1].Center = Main.projectile[proj1].position;
                if ((frost/5) > 2)
                    Main.projectile[proj1].penetrate = 2;
                else if ((frost/5) == 5)
                    Main.projectile[proj1].penetrate = 3;
                frost = 0;
            }
            return false;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 200, true);
            if (frost <= 26)
            {
                frost++;
            }
            if(frost % 5 == 0)
            {
                switch (frost / 5)
                {
                    case 1:
                        CombatText.NewText(player.Hitbox, Color.LightBlue, "1", true);
                        break;
                    case 2:
                        CombatText.NewText(player.Hitbox, Color.LightBlue, "2", true);
                        break;
                    case 3:
                        CombatText.NewText(player.Hitbox, Color.LightBlue, "3", true);
                        break;
                    case 4:
                        CombatText.NewText(player.Hitbox, Color.LightBlue, "4", true);
                        break;
                    case 5:
                        CombatText.NewText(player.Hitbox, Color.LightBlue, "5", true);
                        break;
                }
            }
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(0, 11) == 1)
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 67, 0f, 0f, 50, default(Color), 1);
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.RimestoneBar>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

    class RimeBladeProj : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            Projectile.timeLeft = 300;
            Projectile.height = 16;
            Projectile.width = 16;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            DustHelper.DustCircle(Projectile.Center, DustType<Dusts.Rainbow>(), Projectile.width / 2, (float)Math.Pow(Projectile.ai[0], 2), 1, 0, 0, 0, Color.LightBlue, false);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 200 * (int)Projectile.ai[0], true);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i <= Projectile.ai[0] * 10; i++)
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 67, 0, 0);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            //Make hitbox smaller than dust
            //pythagorean
            float radius = (float)Math.Pow(width/2 * Projectile.scale, 2);
            radius /= 2;
            radius = (float)Math.Sqrt(radius);
            width = (int)radius;
            height = (int)radius;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
    }
}
