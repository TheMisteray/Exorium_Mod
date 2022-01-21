using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using System;

namespace ExoriumMod.Content.Items.Weapons.Ranger
{
    class LustrousBow : ModItem
    {
        public override string Texture => AssetDirectory.RangerWeapon + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Converts arrows into beams of colored light" +
                "\n Beams redirect towrds your cursor");
        }

        public override void SetDefaults()
        {
            item.damage = 18;
            item.ranged = true;
            item.width = 22;
            item.height = 40;
            item.useTime = 26;
            item.useAnimation = 26;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 2;
            item.value = Item.sellPrice(gold: 3, silver: 50); ;
            item.rare = 2;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = 10;
            item.noMelee = true;
            item.shootSpeed = 30;
            item.useStyle = 5;
        }

        private int rainbow;
        private bool side;

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            side = !side;
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(Utils.Clamp(Main.rand.NextFloat(40), 15, 25) * ((side) ? 1 : -1)));
            int projectile = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<LustrousBeam>(), damage, knockBack, player.whoAmI, player.position.X, player.position.Y);
            Main.projectile[projectile].localAI[1] = rainbow;
            rainbow++;
            if (rainbow == 7)
                rainbow = 0;
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Amethyst);
            recipe.AddIngredient(ItemID.Topaz);
            recipe.AddIngredient(ItemID.Sapphire);
            recipe.AddIngredient(ItemID.Emerald);
            recipe.AddIngredient(ItemID.Ruby);
            recipe.AddIngredient(ItemID.Diamond);
            recipe.AddIngredient(ItemID.Amber);
            recipe.AddIngredient(ItemID.PlatinumBar, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe altRecipe = new ModRecipe(mod);
            altRecipe.AddIngredient(ItemID.Amethyst);
            altRecipe.AddIngredient(ItemID.Topaz);
            altRecipe.AddIngredient(ItemID.Sapphire);
            altRecipe.AddIngredient(ItemID.Emerald);
            altRecipe.AddIngredient(ItemID.Ruby);
            altRecipe.AddIngredient(ItemID.Diamond);
            altRecipe.AddIngredient(ItemID.Amber);
            altRecipe.AddIngredient(ItemID.GoldBar, 20);
            altRecipe.AddTile(TileID.Anvils);
            altRecipe.SetResult(this);
            altRecipe.AddRecipe();
        }
    }

    class LustrousBeam : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.extraUpdates = 50;
            projectile.timeLeft = 4500;
            projectile.penetrate = 1;
        }

        private bool retargeted
        {
            get => projectile.ai[0] == 1f;
            set => projectile.ai[0] = value ? 1f : 0f;
        }


        public override void AI()
        {
            Vector2 vectorToCursor = Main.MouseWorld - projectile.Center;
            Vector2 vectorToPlayer = (new Vector2(projectile.ai[0], projectile.ai[1])) - projectile.Center;
            float distanceToCursor = vectorToCursor.Length();
            float distanceToPlayer = vectorToPlayer.Length();
            if (distanceToPlayer > distanceToCursor && !retargeted)
            {
                retargeted = true;
                projectile.velocity = projectile.velocity.RotatedBy((float)(Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - (float)(Math.Atan2(vectorToCursor.X, vectorToCursor.Y))));
                projectile.netUpdate = true;
            }
            projectile.alpha = 225;
            for (int i = 0; i < 10; i++)
            {
                int dust0 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Dusts.Rainbow>(), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 0, default(Color));
                Main.dust[dust0].position.X -= projectile.velocity.X / 10f * i;
                Main.dust[dust0].position.Y -= projectile.velocity.Y / 10f * i;
                switch (projectile.localAI[1])
                {
                    case 0:
                        Main.dust[dust0].color = new Color(255, 0, 0);
                        break;
                    case 1:
                        Main.dust[dust0].color = new Color(255, 110, 0);
                        break;
                    case 2:
                        Main.dust[dust0].color = new Color(255, 247, 0);
                        break;
                    case 3:
                        Main.dust[dust0].color = new Color(0, 255, 0);
                        break;
                    case 4:
                        Main.dust[dust0].color = new Color(0, 255, 204);
                        break;
                    case 5:
                        Main.dust[dust0].color = new Color(35, 0, 255);
                        break;
                    case 6:
                        Main.dust[dust0].color = new Color(149, 0, 255);
                        break;
                }
            }
        }
    }
}
