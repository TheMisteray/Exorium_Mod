using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;

namespace ExoriumMod.Items.Consumables.Bosses
{
    class ShadowmancerBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            item.maxStack = 30;
            item.consumable = true;
            item.width = 24;
            item.height = 24;
            item.rare = -12;
            item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            player.TryGettingDevArmor();
            if (Main.rand.NextBool(1))
                player.QuickSpawnItem(ItemType<Weapons.Magic.Scrolls.ScrollOfMagicMissiles>(), Main.rand.Next(1, 3));
            else
                player.QuickSpawnItem(ItemType<Weapons.Magic.Scrolls.SpellScrollShield>(), Main.rand.Next(1, 3));
            player.QuickSpawnItem(ItemType<Weapons.Ranger.AcidOrb>(), Main.rand.Next(21, 43));
            switch (Main.rand.Next(3))
            {
                case 0:
                    player.QuickSpawnItem(ItemType<Weapons.Magic.ShadowBolt>());
                    break;
                case 1:
                    player.QuickSpawnItem(ItemType<Weapons.Melee.NineLivesStealer>());
                    break;
                case 2:
                    player.QuickSpawnItem(ItemType<Weapons.Summoner.ShadowOrb>(), Main.rand.Next(18, 24));
                    break;
            }
            player.QuickSpawnItem(ItemType<Accessories.ShadowmancerCloak>());
        }

        public override int BossBagNPC => NPCType<NPCs.Bosses.Shadowmancer.AssierJassad>();
    }
}
