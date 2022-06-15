using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Tools
{
    class BlightedHamaxe : ModItem
    {
        public override string Texture => AssetDirectory.Tool + Name;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frayed Hamaxe");
        }

        public override void SetDefaults()
        {
            Item.damage = 19;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 25;
            Item.useAnimation = 15;
            Item.axe = 20;
            Item.hammer = 55;
            Item.useStyle = 1;
            Item.knockBack = 6;
            Item.value = 2800;
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.scale = 1.3f;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(0, 11) == 1)
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustType<Dusts.BlightDust>(), 0f, 0f, 50, default(Color), 1);
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.BlightsteelBar>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
