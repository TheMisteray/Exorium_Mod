using ExoriumMod.Core;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Bosses.Shadowmancer
{
    class ShadowmancerBag : ModItem
    {
        public override string Texture => AssetDirectory.Shadowmancer + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 30;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = -12;
            Item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            if (Main.rand.NextBool(1))
                player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemType<Items.Consumables.Scrolls.ScrollOfMagicMissiles>(), Main.rand.Next(1, 3));
            else
                player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemType<Items.Consumables.Scrolls.SpellScrollShield>(), Main.rand.Next(1, 3));
            player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemType<Items.Weapons.Ranger.AcidOrb>(), Main.rand.Next(21, 43));
            switch (Main.rand.Next(3))
            {
                case 0:
                    player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemType<Items.Weapons.Magic.ShadowBolt>());
                    break;
                case 1:
                    player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemType<Items.Weapons.Melee.NineLivesStealer>());
                    break;
                case 2:
                    player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemType<Items.Weapons.Summoner.ShadowOrb>(), Main.rand.Next(18, 24));
                    break;
            }
            player.QuickSpawnItem(player.GetSource_OpenItem(Type), ItemType<Items.Accessories.ShadowmancerCloak>());
        }

        public override int BossBagNPC => NPCType<AssierJassad>();
    }
}
