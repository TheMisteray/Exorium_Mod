using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Tools
{
    class DunestoneAxe : ModItem
    {
        public override string Texture => AssetDirectory.Tool + Name;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.axe = 11;
            Item.useStyle = 1;
            Item.knockBack = 4;
            Item.value = 1400;
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.scale = 1.3f;
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
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.DunestoneBar>(), 9);
            recipe.AddRecipeGroup("Wood", 3);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}

