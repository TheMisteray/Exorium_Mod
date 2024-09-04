using ExoriumMod.Core;
using System.Collections.Generic;
using Terraria;
using System;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;

namespace ExoriumMod.Content.Items.Accessories
{
    internal class CrimsonCrest : ModItem
    {
        public override string Texture => AssetDirectory.Accessory + Name;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 36;
            Item.accessory = true;
            Item.value = 35000;
            Item.rare = -12;
            Item.expert = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ExoriumPlayer>().checkNearbyNPCs = true;

            float total = 0;
            float max = 0.15f;
            List<NPC> nearby = player.GetModPlayer<ExoriumPlayer>().nearbyNPCs;
            for (int i = 0; i < nearby.Count; i++) //Check nearby npc for burning
            {
                if (nearby[i].HasBuff(BuffID.OnFire) || nearby[i].HasBuff(BuffID.OnFire3) || nearby[i].HasBuff(BuffID.CursedInferno) || nearby[i].HasBuff(BuffID.ShadowFlame) || nearby[i].HasBuff(ModContent.BuffType<Buffs.Inferno>()))
                    total++;
            }

            if (player.HasBuff(BuffID.OnFire) || player.HasBuff(BuffID.OnFire3) || player.HasBuff(BuffID.CursedInferno) || player.HasBuff(BuffID.ShadowFlame) || player.HasBuff(ModContent.BuffType<Buffs.Inferno>()))
            {
                total += 5;
                max = 0.2f;
            }

            player.GetDamage(DamageClass.Generic) += Math.Min(0.015f * total, max);

            if (0.015f * total >= max)
                player.GetModPlayer<ExoriumPlayer>().inflictInferno = true;
        }
    }
}
