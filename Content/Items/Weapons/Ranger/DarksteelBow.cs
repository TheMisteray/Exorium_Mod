using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Weapons.Ranger
{
    public class DarksteelBow : ModItem
    {
        public override string Texture => AssetDirectory.RangerWeapon + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Has a chance to fire a Darksteel arrow, dealing double damage");
        }

        public override void SetDefaults()
        {
            item.damage = 24;
            item.ranged = true;
            item.width = 22;
            item.height = 40;
            item.useTime = 26;
            item.useAnimation = 26;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 2;
            item.value = Item.sellPrice(gold: 3, silver: 50); ;
            item.rare = 3;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = 10;
            item.noMelee = true;
            item.shootSpeed = 30;
            item.useStyle = 5;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int spShoot = Main.rand.Next(0,6);
            if (spShoot == 5)
            {
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ProjectileType<DarksteelArrow>(), damage, knockBack, player.whoAmI);
                return false;
            }
            else
            {
                return true;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Materials.Metals.DarksteelBar>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    class DarksteelArrow : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            aiType = ProjectileID.WoodenArrowFriendly;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage *= 2;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(3))
            {
                int offset = Main.rand.Next(-4, 4);
                new Vector2(projectile.position.X + offset, projectile.position.Y + offset);
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Dusts.DarksteelDust>(), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                int offset = Main.rand.Next(-4, 4);
                Dust.NewDust(new Vector2(projectile.position.X + offset, projectile.position.Y + offset), projectile.width, projectile.height, DustType<Dusts.DarksteelDust>(), projectile.oldVelocity.X * 1.5f, projectile.oldVelocity.Y * 1.5f);
            }
        }
    }
}
