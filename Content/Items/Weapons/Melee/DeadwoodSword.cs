using ExoriumMod.Core;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria;

namespace ExoriumMod.Content.Items.Weapons.Melee
{
    class DeadwoodSword : ModItem
    {
        public override string Texture => AssetDirectory.MeleeWeapon + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Incredibly light");
        }

        public override void SetDefaults()
        {
            Item.damage = 6;
            Item.DamageType = DamageClass.Melee;
            Item.width = 10;
            Item.height = 10;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = 1;
            Item.knockBack = 2;
            Item.value = 20;
            Item.rare = 0;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<TileItems.Deadwood>(), 7);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
