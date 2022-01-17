using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Tools
{
    class RimestoneAxe : ModItem
    {
        public override string Texture => AssetDirectory.Tool + Name;

        public override void SetDefaults()
        {
            item.damage = 7;
            item.melee = true;
            item.width = 40;
            item.height = 40;
            item.useTime = 28;
            item.useAnimation = 28;
            item.axe = 11;
            item.useStyle = 1;
            item.knockBack = 4;
            item.value = 1400;
            item.rare = 1;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.scale = 1.3f;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if(Main.rand.Next(5) == 1)
            {
                target.AddBuff(BuffID.Frostburn, 200, true);
            }
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(0,11) == 1)
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 67, 0f, 0f, 50, default(Color), 1);
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Materials.Metals.RimestoneBar>(), 9);
            recipe.AddRecipeGroup("Wood", 3);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
