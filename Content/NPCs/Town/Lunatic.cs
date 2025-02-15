using ExoriumMod.Core;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using ExoriumMod.Content.Biomes;

namespace ExoriumMod.Content.NPCs.Town
{
    [AutoloadHead]
    class Lunatic : ModNPC
    {
        public override string Texture => AssetDirectory.TownNPC + Name;
        public override string HeadTexture => Texture + "_Head";

        public override string Name => base.Name;
        public const string ShopName = "Shop";
        private static Profiles.StackedNPCProfile NPCProfile;

        public override List<string> SetNPCNameList()/* tModPorter Suggestion: Return a list of names */
        {
            return new List<string>
            {
                "Fravrick"
            };
        }

        public override ITownNPCProfile TownNPCProfile()
        {
            return NPCProfile;
        }

        public override void SetStaticDefaults() //TODO:A lot to make this work like a 1.4 npc
        {
            // DisplayName.SetDefault("Lunatic");
            Main.npcFrameCount[NPC.type] = 23;
            NPCID.Sets.ExtraFramesCount[NPC.type] = NPCID.Sets.ExtraFramesCount[NPCID.Wizard];
            NPCID.Sets.AttackFrameCount[NPC.type] = NPCID.Sets.AttackFrameCount[NPCID.Wizard];
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = NPCID.Sets.AttackType[NPCID.Wizard];
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
            NPCID.Sets.HatOffsetY[NPC.type] = 8;

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f,
                Direction = -1
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            NPCID.Sets.TownNPCBestiaryPriority.Add(Type);

            // Set Example Person's biome and neighbor preferences with the NPCHappiness hook. You can add happiness text and remarks with localization (See an example in ExampleMod/Localization/en-US.lang).
            // NOTE: The following code uses chaining - a style that works due to the fact that the SetXAffection methods return the same NPCHappiness instance they're called on.
            NPC.Happiness
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
                .SetBiomeAffection<SnowBiome>(AffectionLevel.Like)
                .SetBiomeAffection<DeadlandBiome>(AffectionLevel.Hate)
                .SetNPCAffection(NPCID.Wizard, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Clothier, AffectionLevel.Love)
                .SetNPCAffection(NPCID.Guide, AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.Dryad, AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Princess, AffectionLevel.Love)
            ;
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            AnimationType = NPCID.Wizard;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)/* tModPorter Suggestion: Copy the implementation of NPC.SpawnAllowed_Merchant in vanilla if you to count money, and be sure to set a flag when unlocked, so you don't count every tick. */
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
            if (Main.rand.NextBool(500))
            {
                switch (Main.rand.Next(3))
                {
                    case 0:
                        return "Someone once told me that I was their father, but they didn't look like me at all.";
                    case 1:
                        return "I hate bright colors.";
                    default:
                        return "Do you... smell that?";
                }
            }
            if (ModContent.GetInstance<Core.Systems.TileCounters.DeadlandsBiomeTileCount>().deadlandsBlockCount >= 250 && Main.rand.NextBool(4))
            {
                return "What happened here?";
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
                return Main.npc[Guide].GivenName + " always refuses to talk to me. Look, I just keep forgetting how to make a bucket.";
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
                        return "I'm not crazy. Who told you that?";
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
                        return "You look like you could use a magical scroll of dubious safety and amateur craftsmanship... Which is a shame because I only sell high quality ones.";
                    default:
                        return "Don't ask where I get this stuff.";
                }
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("A strange man that approached you one night looking for lodging, supposedly lost. It's not clear what his deal is, or if he knows either. Fravrick seems to be suffering from some form of memory loss, and becuase of this doesn't remember where the magical knick-knacks he sells came from."),
            });
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                shopName = ShopName;
            }
        }

        public override void AddShops()
        {
            var npcShop = new NPCShop(Type, ShopName)
                .Add(ItemID.Amethyst)
                .Add(ItemID.Topaz)
                .Add<Items.Weapons.Magic.Firebolt>()
                .Add<Items.Accessories.RitualBone>()
                .Add<Items.Consumables.Scrolls.ScrollOfMagicMissiles>()
                .Add<Items.Consumables.Scrolls.SpellScrollShield>()
                .Add<Items.Consumables.Scrolls.ScrollOfCloudOfDaggers>(Condition.DownedEarlygameBoss)
                .Add<Items.Consumables.Scrolls.SpellScrollAcidArrow>()
                .Add<Items.Consumables.Scrolls.SpellScrollMistyStep>()
                .Add<Items.Consumables.Scrolls.SpellScrollFireball>(Condition.DownedSkeletron)
                .Add<Items.Consumables.Scrolls.SpellScrollDelayedBlastFireball>(Condition.DownedCultist);

            npcShop.Register();
        }

        public override void ModifyActiveShop(string shopName, Item[] items)
        {
            if (shopName == ShopName)
            {
                items[0].shopCustomPrice = Item.buyPrice(0, 0, 50, 0);
                items[1].shopCustomPrice = Item.buyPrice(0, 0, 50, 0);
                items[2].shopCustomPrice = Item.buyPrice(0, 1, 5, 0);
                items[3].shopCustomPrice = Item.buyPrice(0, 0, 50, 0);
                items[4].shopCustomPrice = Item.buyPrice(0, 0, 30, 0);
                items[5].shopCustomPrice = Item.buyPrice(0, 1, 50, 0);
                items[6].shopCustomPrice = Item.buyPrice(0, 1, 0, 0);
                items[7].shopCustomPrice = Item.buyPrice(0, 1, 7, 0);
                items[8].shopCustomPrice = Item.buyPrice(0, 3, 0, 0);
                items[9].shopCustomPrice = Item.buyPrice(0, 3, 0, 0);
                items[10].shopCustomPrice = Item.buyPrice(0, 18, 0, 0);
            }
            /*
            shop.item[nextSlot].SetDefaults(ItemID.Amethyst);
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 50, 0);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemID.Topaz);
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 50, 0);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemType<Items.Weapons.Magic.Firebolt>());
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 1, 5, 0);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemType<Items.Accessories.RitualBone>());
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 50, 0);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemType<Items.Consumables.Scrolls.ScrollOfMagicMissiles>());
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 30, 0);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemType<Items.Consumables.Scrolls.SpellScrollShield>());
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 1, 50, 0);
            nextSlot++;
            if (NPC.downedBoss1)
            {
                shop.item[nextSlot].SetDefaults(ItemType<Items.Consumables.Scrolls.ScrollOfCloudOfDaggers>());
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 1, 0, 0);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemType<Items.Consumables.Scrolls.SpellScrollAcidArrow>());
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 1, 7, 0);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemType<Items.Consumables.Scrolls.SpellScrollMistyStep>());
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 3, 0, 0);
                nextSlot++;
            }
            if (NPC.downedBoss3)
            {
                shop.item[nextSlot].SetDefaults(ItemType<Items.Consumables.Scrolls.SpellScrollFireball>());
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 3, 0, 0);
                nextSlot++;
            }
            if (NPC.downedAncientCultist)
            {
                shop.item[nextSlot].SetDefaults(ItemType<Items.Consumables.Scrolls.SpellScrollDelayedBlastFireball>());
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 18, 0, 0);
                nextSlot++;
            }*/
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
