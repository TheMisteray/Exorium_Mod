﻿using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace ExoriumMod.Content.Items.Weapons.Magic
{
    class SettlingSands : ModItem
    {
        public override string Texture => AssetDirectory.MagicWeapon + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Launches bouncing sand balls \n" +
                "\"This is a horrible idea\"");
        }

        public override void SetDefaults()
        {
            item.damage = 18;
            item.width = 15;
            item.height = 15;
            item.magic = true;
            item.mana = 8;
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = Item.sellPrice(silver: 14);
            item.rare = 2;
            item.UseSound = SoundID.Item42;
            item.shoot = ProjectileType<SandShot>();
            item.shootSpeed = 7;
            item.autoReuse = true;
            item.scale = 0.9f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Materials.Metals.DunestoneBar>(), 8);
            recipe.AddTile(TileID.Bookcases);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
    class SandShot : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bouncing Sand");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.SandBallGun);
            aiType = ProjectileID.SandBallGun;
            projectile.aiStyle = 1;
            projectile.ai[1] = 0;
        }

        public float projectileBounce
        {
            get => projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Player player = Main.player[projectile.owner];
            projectileBounce++;
            if (projectileBounce >= 3)
            {
                Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, 0, ProjectileID.SandBallFalling, 5, 5, player.whoAmI);
                projectile.Kill();
            }
            else
            {
                projectile.ai[0] += 0.1f;
                if (projectile.velocity.X != oldVelocity.X)
                {
                    projectile.velocity.X = -oldVelocity.X / 1.5f;
                }
                if (projectile.velocity.Y != oldVelocity.Y)
                {
                    projectile.velocity.Y = -oldVelocity.Y / 1.5f;
                }
                projectile.velocity *= 0.75f;
                Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, 0, ProjectileID.SandBallFalling, 5, 5, player.whoAmI);
                Main.PlaySound(SoundID.Item10, projectile.position);
            }
            return false;
        }
    }
}