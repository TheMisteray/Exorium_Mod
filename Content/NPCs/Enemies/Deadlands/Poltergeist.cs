﻿using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using ExoriumMod.Content.Biomes;

namespace ExoriumMod.Content.NPCs.Enemies.Deadlands
{
    class Poltergeist : Hover
    {
        public override string Texture => AssetDirectory.EnemyNPC + Name;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Ghost];
            // DisplayName.SetDefault("Specter");

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking -1 tiles in the x direction
                Direction = -1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Ghost);
            AnimationType = NPCID.Ghost;
            NPC.damage = 44;
            NPC.defense = 3;
            NPC.lifeMax = 99;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 200f;
            NPC.buffImmune[BuffID.Confused] = false;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<DeadlandBiome>().Type };
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.InModBiome<DeadlandBiome>() && spawnInfo.SpawnTileType == ModContent.TileType<Content.Tiles.AshenDustTile>()) return 0.06f;
            return 0;
        }

        public override void CustomBehavior(ref float ai)
        {
            Vector2 dist = Main.player[NPC.target].position - NPC.position;
            float magnitude = (float)Math.Sqrt(dist.X * dist.X + dist.Y * dist.Y);
            if (magnitude >= 400 && NPC.alpha <= 255)
                NPC.alpha += 5;
            else if (NPC.alpha >= 40)
                NPC.alpha -= 5; 
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            if (NPC.alpha > 240)
                return false;
            return true;
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            NPC.alpha = 60;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("The confused, invisible spirit of an individual with no sense of how they died."),
                new BestiaryPortraitBackgroundProviderPreferenceInfoElement(ModContent.GetInstance<DeadlandBiome>().ModBiomeBestiaryInfoElement),
            });
        }
    }
}
