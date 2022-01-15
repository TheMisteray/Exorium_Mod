using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using ExoriumMod.Projectiles;

namespace ExoriumMod.Items.Weapons.Magic
{
    class StoneOfTheSands : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stone of the Sands");
            Tooltip.SetDefault("Creates a ring of sand balls that rotate aroung you \n" +
                "Right click to shoot them forward");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.rare = 1;
            item.damage = 17;
            item.magic = true;
            item.mana = 7;
            item.noMelee = true;
            item.value = Item.sellPrice(silver: 14, copper: 35);
            item.useStyle = 4;
            item.useTime = 30;
            item.useAnimation = 30;
            item.knockBack = 4;
            item.autoReuse = false;
            item.UseSound = SoundID.Item43;
            item.shoot = ProjectileType<Projectiles.DuneBall>();
            item.shootSpeed = 5;
        }

        int[] proj = new int[5];

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                for (int i = 0; i < 5; i++)
                {
                    Main.projectile[proj[i]].ai[0] = 1;
                    Main.projectile[proj[i]].damage/=2;
                }
                item.mana = 0;
                item.shoot = ProjectileID.None;
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Main.projectile[proj[i]].ai[0] == 0 && Main.projectile[proj[i]].type == mod.ProjectileType("DuneBall")) Main.projectile[proj[i]].Kill();
                }
                item.mana = 7;
                item.shoot = mod.ProjectileType("DuneBall");
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < 5; i++)
            {
                proj[i] = Projectile.NewProjectile(position.X, position.Y, 0, 0, type, damage, knockBack, player.whoAmI, 0, 60 * i);
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("DunestoneBar"), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
