using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Weapons.Ranger
{
    public class DarksteelBow : ModItem
    {
        public override string Texture => AssetDirectory.RangerWeapon + Name;

        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Has a chance to fire a Darksteel arrow, dealing double damage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 38;
            Item.height = 64;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useAmmo = AmmoID.Arrow;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(gold: 3, silver: 50); ;
            Item.rare = 3;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = 10;
            Item.noMelee = true;
            Item.shootSpeed = 30;
            Item.useStyle = 5;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int spShoot = Main.rand.Next(0,6);
            if (spShoot == 5)
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileType<DarksteelArrow>(), damage, knockback, player.whoAmI);
                return false;
            }
            else
            {
                return true;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.DarksteelBar>(), 14);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
    }

    class DarksteelArrow : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage *= 2;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(3))
            {
                int offset = Main.rand.Next(-4, 4);
                new Vector2(Projectile.position.X + offset, Projectile.position.Y + offset);
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustType<Dusts.DarksteelDust>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                int offset = Main.rand.Next(-4, 4);
                Dust.NewDust(new Vector2(Projectile.position.X + offset, Projectile.position.Y + offset), Projectile.width, Projectile.height, DustType<Dusts.DarksteelDust>(), Projectile.oldVelocity.X * 1.5f, Projectile.oldVelocity.Y * 1.5f);
            }
        }
    }
}
