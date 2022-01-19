using ExoriumMod.Core;
using ExoriumMod.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using System;

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
        }

        public override void SetDefaults()
        {
            item.damage = 11;
            item.melee = true;
            item.width = 34;
            item.height = 34;
            item.useTime = 26;
            item.useAnimation = 26;
            item.useStyle = 1;
            item.knockBack = 4;
            item.value = Item.sellPrice(silver: 14);
            item.rare = 1;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.scale = 1.5f;
            item.useTurn = true;
            item.shoot = ProjectileType<RimeBladeProj>();
            item.shootSpeed = 10;
        }

        private int frost = 0;

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
                item.noMelee = true;
            else
                item.noMelee = false;
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2 && frost/5 > 0)
            {
                int proj1 = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("RimeBladeProj"), damage *= (frost/5) * 3, knockBack, player.whoAmI, frost/5);
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
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Materials.Metals.RimestoneBar>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    class RimeBladeProj : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            projectile.timeLeft = 300;
            projectile.height = 16;
            projectile.width = 16;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.alpha = 255;
            projectile.melee = true;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            DustHelper.DustCircle(projectile.Center, DustType<Dusts.Rainbow>(), projectile.width / 2, (float)Math.Pow(projectile.ai[0], 2), 1, 0, 0, 0, Color.LightBlue, false);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 200 * (int)projectile.ai[0], true);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i <= projectile.ai[0] * 10; i++)
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 67, 0, 0);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            //Make hitbox smaller than dust
            //pythagorean
            float radius = (float)Math.Pow(width/2 * projectile.scale, 2);
            radius /= 2;
            radius = (float)Math.Sqrt(radius);
            width = (int)radius;
            height = (int)radius;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
    }
}
