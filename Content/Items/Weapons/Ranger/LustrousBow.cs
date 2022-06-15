using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using System;
using Terraria.DataStructures;

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
            Item.damage = 18;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 22;
            Item.height = 40;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useAmmo = AmmoID.Arrow;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(gold: 3, silver: 50); ;
            Item.rare = 2;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = 10;
            Item.noMelee = true;
            Item.shootSpeed = 30;
            Item.useStyle = 5;
        }

        private int rainbow;
        private bool side;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            side = !side;
            Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(Utils.Clamp(Main.rand.NextFloat(40), 15, 25) * ((side) ? 1 : -1)));
            int projectile = Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<LustrousBeam>(), damage, knockback, player.whoAmI, player.position.X, player.position.Y);
            Main.projectile[projectile].localAI[1] = rainbow;
            rainbow++;
            if (rainbow == 7)
                rainbow = 0;
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
            recipe.AddIngredient(ItemID.PlatinumBar, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

            Recipe altRecipe = CreateRecipe();
            altRecipe.AddIngredient(ItemID.Amethyst);
            altRecipe.AddIngredient(ItemID.Topaz);
            altRecipe.AddIngredient(ItemID.Sapphire);
            altRecipe.AddIngredient(ItemID.Emerald);
            altRecipe.AddIngredient(ItemID.Ruby);
            altRecipe.AddIngredient(ItemID.Diamond);
            altRecipe.AddIngredient(ItemID.Amber);
            altRecipe.AddIngredient(ItemID.GoldBar, 20);
            altRecipe.AddTile(TileID.Anvils);
            altRecipe.Register();
        }
    }

    class LustrousBeam : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 50;
            Projectile.timeLeft = 4500;
            Projectile.penetrate = 1;
        }

        private bool retargeted
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value ? 1f : 0f;
        }


        public override void AI()
        {
            Vector2 vectorToCursor = Main.MouseWorld - Projectile.Center;
            Vector2 vectorToPlayer = (new Vector2(Projectile.ai[0], Projectile.ai[1])) - Projectile.Center;
            float distanceToCursor = vectorToCursor.Length();
            float distanceToPlayer = vectorToPlayer.Length();
            if (distanceToPlayer > distanceToCursor && !retargeted)
            {
                retargeted = true;
                Projectile.velocity = Projectile.velocity.RotatedBy((float)(Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - (float)(Math.Atan2(vectorToCursor.X, vectorToCursor.Y))));
                Projectile.netUpdate = true;
            }
            Projectile.alpha = 225;
            for (int i = 0; i < 10; i++)
            {
                int dust0 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Dusts.Rainbow>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 0, default(Color));
                Main.dust[dust0].position.X -= Projectile.velocity.X / 10f * i;
                Main.dust[dust0].position.Y -= Projectile.velocity.Y / 10f * i;
                switch (Projectile.localAI[1])
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
