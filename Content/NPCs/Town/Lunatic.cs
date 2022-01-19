using ExoriumMod.Core;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.NPCs.Town
{
    [AutoloadHead]
    class Lunatic : ModNPC
    {
        public override string Texture => AssetDirectory.TownNPC + Name;
        public override string HeadTexture => Texture + "_Head";

        public override bool Autoload(ref string name)
        {
            name = "Lunatic";
            return mod.Properties.Autoload;
        }

        public override string TownNPCName()
        {
            return "Fravrick";
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lunatic");
            Main.npcFrameCount[npc.type] = 23;
            NPCID.Sets.ExtraFramesCount[npc.type] = NPCID.Sets.ExtraFramesCount[NPCID.Wizard];
            NPCID.Sets.AttackFrameCount[npc.type] = NPCID.Sets.AttackFrameCount[NPCID.Wizard];
            NPCID.Sets.DangerDetectRange[npc.type] = 700;
            NPCID.Sets.AttackType[npc.type] = NPCID.Sets.AttackType[NPCID.Wizard];
            NPCID.Sets.AttackTime[npc.type] = 90;
            NPCID.Sets.AttackAverageChance[npc.type] = 30;
            NPCID.Sets.HatOffsetY[npc.type] = 8;
        }

        public override void SetDefaults()
        {
            npc.townNPC = true;
            npc.friendly = true;
            npc.width = 18;
            npc.height = 40;
            npc.aiStyle = 7;
            npc.damage = 10;
            npc.defense = 15;
            npc.lifeMax = 250;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.5f;
            animationType = NPCID.Wizard;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            if (numTownNPCs > 2)
            {
                return true;
            }
            return false;
        }

        public override string GetChat()
        {
            int Wizard = NPC.FindFirstNPC(NPCID.Wizard);
            int Clothier = NPC.FindFirstNPC(NPCID.Clothier);
            int Guide = NPC.FindFirstNPC(NPCID.Guide);
            if (Main.rand.Next(50) == 0)
            {
                switch (Main.rand.Next(3))
                {
                    case 0:
                        return "Someone once told me that I was their father, but they didn't look like me at all.";
                    case 1:
                        return "I hate bright colors.";
                    default:
                        return "Do you smell that?";
                }
            }
            if (ExoriumWorld.deadlandTiles >= 50 && Main.rand.NextBool(4))
            {
                return "This place make me feel... Uncomfortable";
            }
            if (Wizard >= 0 && Main.rand.NextBool(7))
            {
                return "Tell " + Main.npc[Wizard].GivenName + " that his crystal ball isn't worth as much as he thinks it is.";
            }
            if (Clothier >=0 && Main.rand.NextBool(7))
            {
                return "Do you know who taught " + Main.npc[Clothier].GivenName + " those tricks? I'd like to meet them.";
            }
            if (Guide >= 0 && Main.rand.NextBool(7))
            {
                return Main.npc[Guide].GivenName + " always refuses to talk to me. Look, it's not my fault I keep forgetting all the recipies.";
            }
            if (Main.rand.Next(3) >= 1)
            {
                switch (Main.rand.Next(5))
                {
                    case 0:
                        return "I like dragons... Do you like dragons?";
                    case 1:
                        return "Why do I wear a mask you ask? Well... I don't know.";
                    case 2:
                        return "I feel like I'm forgetting something.";
                    case 3:
                        return "I'm not crazy, who told you that?";
                    default:
                        return "How did I get here?";
                }
            }
            else
            {
                switch (Main.rand.Next(4))
                {
                    case 0:
                        return "I could sell you more powerful scrolls, but that would be no fun.";
                    case 1:
                        return "No, you cannot search my pockets.";
                    case 2:
                        return "You look like you could use a magical scroll of dubious safety and amateur craftsmanship. Which is a shame because I only sell high quality ones.";
                    default:
                        return "Don't ask where I get all this stuff.";
                }
            }
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = true;
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            shop.item[nextSlot].SetDefaults(ItemID.Amethyst);
            Item.buyPrice(0, 0, 50, 0);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemID.Topaz);
            Item.buyPrice(0, 0, 50, 0);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemType<Items.Weapons.Magic.Firebolt>());
            Item.buyPrice(0, 3, 5, 0);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemType<Items.Accessories.RitualBone>());
            Item.buyPrice(0, 0, 50, 0);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemType<Items.Consumables.Scrolls.ScrollOfMagicMissiles>());
            Item.buyPrice(0, 0, 70, 0);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemType<Items.Consumables.Scrolls.SpellScrollShield>());
            Item.buyPrice(0, 1, 50, 0);
            nextSlot++;
            if (NPC.downedBoss1)
            {
                shop.item[nextSlot].SetDefaults(ItemType<Items.Consumables.Scrolls.ScrollOfCloudOfDaggers>());
                Item.buyPrice(0, 1, 0, 0);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemType<Items.Consumables.Scrolls.SpellScrollAcidArrow>());
                Item.buyPrice(0, 1, 7, 0);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemType<Items.Consumables.Scrolls.SpellScrollMistyStep>());
                Item.buyPrice(0, 3, 0, 0);
                nextSlot++;
            }
            if (NPC.downedBoss3)
            {
                shop.item[nextSlot].SetDefaults(ItemType<Items.Consumables.Scrolls.SpellScrollFireball>());
                Item.buyPrice(0, 3, 0, 0);
                nextSlot++;
            }
            if (NPC.downedAncientCultist)
            {
                shop.item[nextSlot].SetDefaults(ItemType<Items.Consumables.Scrolls.SpellScrollDelayedBlastFireball>());
                Item.buyPrice(0, 18, 0, 0);
                nextSlot++;
            }
        }

        public override bool CanGoToStatue(bool toKingStatue)
        {
            return true;
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 12;
            knockback = 3f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 90;
            randExtraCooldown = 60;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileType<Projectiles.Fireball>();
            attackDelay = 60;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
        }
    }
}
