using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Weapons.Magic.Scrolls
{
    class ScrollOfMagicMissiles : ModItem
    {
        public override string Texture => AssetDirectory.SpellScroll + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Casts Magic Missiles");
            DisplayName.SetDefault("Spell Scroll: Magic Missiles");
        }

        public override void SetDefaults()
        {
            item.damage = 44;
            item.useStyle = 4;
            item.useTime = 14;
            item.useAnimation = 42;
            item.knockBack = 2;
            item.rare = 1;
            item.value = Item.buyPrice(silver: 50);
            item.width = 32;
            item.height = 32;
            item.maxStack = 30;
            item.magic = true;
            item.mana = 20;
            //item.channel = true;
            item.noMelee = true;
            item.shootSpeed = 16f;
            item.autoReuse = false;
            item.shoot = ProjectileID.MagicMissile;
            item.consumable = true;
            item.UseSound = SoundID.Item7;
            item.useTurn = true;
            item.noUseGraphic = true;
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
            player.AddBuff(BuffType<Buffs.ScrollCooldown>(), 3600);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20));
            perturbedSpeed.RotatedBy(MathHelper.ToRadians(180));
            Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            firstShot++;
            return false;
        }
    }
}
