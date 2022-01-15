using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Items.Weapons.Ranger
{
    public class MorditeBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Has a chance to fire a Mordite arrow, dealing double damage");
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
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("MorditeArrow"), damage, knockBack, player.whoAmI);
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
            recipe.AddIngredient(ItemType<Placeables.MorditeBar>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
