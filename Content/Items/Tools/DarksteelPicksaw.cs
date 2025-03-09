using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Tools
{
    class DarksteelPicksaw : ModItem
    {
        public override string Texture => AssetDirectory.Tool + Name;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 22;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.pick = 100;
            Item.axe = 22;
            Item.useStyle = 1;
            Item.knockBack = 4;
            Item.value = 6000;
            Item.rare = 3;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.scale = 1.4f;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(0, 11) == 1)
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustType<Dusts.DarksteelDust>(), 0f, 0f, 50, default(Color), 1);
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.DarksteelBar>(), 11);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
