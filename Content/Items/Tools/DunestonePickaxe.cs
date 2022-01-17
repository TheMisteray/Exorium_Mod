using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Tools
{
    class DunestonePickaxe : ModItem
    {
        public override string Texture => AssetDirectory.Tool + Name;

        public override void SetDefaults()
        {
            item.damage = 6;
            item.melee = true;
            item.width = 40;
            item.height = 40;
            item.useTime = 21;
            item.useAnimation = 21;
            item.pick = 50;
            item.useStyle = 1;
            item.knockBack = 4;
            item.value = 1400;
            item.rare = 1;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.scale = 1.3f;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(0, 11) == 1)
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 32, 0f, 0f, 50, default(Color), 1f);
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Materials.Metals.DunestoneBar>(), 12);
            recipe.AddRecipeGroup("Wood", 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
