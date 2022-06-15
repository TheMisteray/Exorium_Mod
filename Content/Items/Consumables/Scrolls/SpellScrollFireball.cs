using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Items.Consumables.Scrolls
{
    class SpellScrollFireball : ModItem
    {
        public override string Texture => AssetDirectory.SpellScroll + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Casts Fireball");
            DisplayName.SetDefault("Spell Scroll: Fireball");
        }

        public override void SetDefaults()
        {
            Item.damage = 125;
            Item.useStyle = 4;
            Item.useTime = 42;
            Item.useAnimation = 42;
            Item.knockBack = 2;
            Item.rare = 3;
            Item.value = Item.buyPrice(gold: 5);
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 30;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 50;
            Item.noMelee = true;
            Item.shootSpeed = 16f;
            Item.autoReuse = false;
            Item.shoot = ProjectileType<Projectiles.Fireball>();
            Item.consumable = true;
            Item.UseSound = SoundID.Item7;
            Item.useTurn = true;
            Item.noUseGraphic = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.HasBuff(BuffType<Buffs.ScrollCooldown>());
        }

        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(BuffType<Buffs.ScrollCooldown>(), 7200);
        }
    }
}
