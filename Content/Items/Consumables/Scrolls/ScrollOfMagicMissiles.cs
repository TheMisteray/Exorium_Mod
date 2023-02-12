using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Consumables.Scrolls
{
    class ScrollOfMagicMissiles : ModItem
    {
        public override string Texture => AssetDirectory.SpellScroll + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Casts Magic Missiles");
            DisplayName.SetDefault("Spell Scroll: Magic Missiles");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
        }

        public override void SetDefaults()
        {
            Item.damage = 44;
            Item.useStyle = 4;
            Item.useTime = 14;
            Item.useAnimation = 42;
            Item.knockBack = 2;
            Item.rare = 1;
            Item.value = Item.buyPrice(silver: 50);
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 30;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            //item.channel = true;
            Item.noMelee = true;
            Item.shootSpeed = 16f;
            Item.autoReuse = false;
            Item.shoot = ProjectileID.MagicMissile;
            Item.consumable = true;
            Item.UseSound = SoundID.Item7;
            Item.useTurn = true;
            Item.noUseGraphic = true;
        }

        private int firstShot = 0;

        public override bool CanUseItem(Player player)
        {
            firstShot = 0;
            return !player.HasBuff(BuffType<Buffs.ScrollCooldown>());
        }

        public override bool ConsumeItem(Player player)
        {            
            return firstShot == 3;
        }

        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(BuffType<Buffs.ScrollCooldown>(), 2700);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(20));
            perturbedSpeed.RotatedBy(MathHelper.ToRadians(180));
            Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
            firstShot++;
            return false;
        }
    }
}
