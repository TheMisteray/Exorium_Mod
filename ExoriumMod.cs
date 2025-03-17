using Microsoft.Xna.Framework;
using ExoriumMod.Core.Systems;
using System;
using Terraria;
using Terraria.ModLoader;
using System.IO;
using Terraria.Graphics.Effects;
using ExoriumMod.Content.Skies;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Graphics;
using Terraria.GameContent.Shaders;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Reflection;
using System.Collections.Generic;
using ExoriumMod.Core;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Core.WorldGeneration.Structures;

namespace ExoriumMod
{
	public class ExoriumMod : Mod
	{
        internal static ExoriumMod instance;

        public ExoriumMod()
		{
            instance = this;
        }

        public override void Load()
        {
            instance = this;

            if (Main.netMode != NetmodeID.Server)
            {
                //Skies
                SkyManager.Instance["ExoriumMod:DeadlandsSky"] = new DeadlandsSky();
                Filters.Scene["ExoriumMod:DeadlandsSky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryLow);

                //Shader refs
                //Ref<Effect> HeatDistortEffectRef = new Ref<Effect>(Assets.Request<Effect>("Effects/HeatDistortion", AssetRequestMode.ImmediateLoad).Value);
                //Ref<Effect> screenRef = new Ref<Effect>(Assets.Request<Effect>("Effects/ShockwaveEffect", AssetRequestMode.ImmediateLoad).Value);
                //Ref<Effect> flamingSphereRef = new Ref<Effect>(Assets.Request<Effect>("Effects/Flaming
                //Ref<Effect> shadowmancerShadeRef = new Ref<Effect>(Assets.Request<Effect>("Effects/ShadowmancerShade", AssetRequestMode.ImmediateLoad).Value);
                //Ref<Effect> violetPortalRef = new Ref<Effect>(Assets.Request<Effect>("Effects/VioletPortal", AssetRequestMode.ImmediateLoad).Value);
                //Ref<Effect> fireAuraRef = new Ref<Effect>(Assets.Request<Effect>("Effects/FireAura", AssetRequestMode.ImmediateLoad).Value);
                //Ref<Effect> caraveneTitleRef = new Ref<Effect>(Assets.Request<Effect>("Effects/CaraveneTitle", AssetRequestMode.ImmediateLoad).Value);
                //Ref<Effect> infernalRiftRef = new Ref<Effect>(Assets.Request<Effect>("Effects/InfernalRift", AssetRequestMode.ImmediateLoad).Value);
                //Ref<Effect> laserEffectRef = new Ref<Effect>(Assets.Request<Effect>("Effects/Deathray", AssetRequestMode.ImmediateLoad).Value);
                Asset<Effect> HeatDistortEffectRef = this.Assets.Request<Effect>("Effects/HeatDistortion");
                Asset<Effect> screenRef = this.Assets.Request<Effect>("Effects/ShockwaveEffect");
                Asset<Effect> flamingSphereRef = this.Assets.Request<Effect>("Effects/FlamingSphere");
                Asset<Effect> shadowmancerShadeRef = this.Assets.Request<Effect>("Effects/ShadowmancerShade");
                Asset<Effect> violetPortalRef = this.Assets.Request<Effect>("Effects/VioletPortal");
                Asset<Effect> fireAuraRef = this.Assets.Request<Effect>("Effects/FireAura");
                Asset<Effect> caraveneTitleRef = this.Assets.Request<Effect>("Effects/CaraveneTitle");
                Asset<Effect> infernalRiftRef = this.Assets.Request<Effect>("Effects/InfernalRift");
                Asset<Effect> laserEffectRef = this.Assets.Request<Effect>("Effects/Deathray");




                //Screen shader loads
                Filters.Scene["ExoriumMod:HeatDistortion"] = new Filter(new ScreenShaderData(HeatDistortEffectRef, "Heat"), EffectPriority.High);
                Filters.Scene["ExoriumMod:HeatDistortion"].Load();

                Filters.Scene["ExoriumMod:CaraveneTitle"] = new Filter(new ScreenShaderData(caraveneTitleRef, "ScreenTextPass"), EffectPriority.VeryHigh);
                Filters.Scene["ExoriumMod:CaraveneTitle"].Load();

                Filters.Scene["ExoriumMod:InfernalRift"] = new Filter(new ScreenShaderData(infernalRiftRef, "Rift"), EffectPriority.VeryHigh);
                Filters.Scene["ExoriumMod:InfernalRift"].Load();

                Filters.Scene["ExoriumMod:Shockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave" /*Must be name of pass in shader*/), EffectPriority.VeryHigh);
                Filters.Scene["ExoriumMod:Shockwave"].Load();

                Filters.Scene["ExoriumMod:LaserEffect"] = new Filter(new ScreenShaderData(laserEffectRef, "TrailPass"), EffectPriority.Medium);
                Filters.Scene["ExoriumMod:LaserEffect"].Load();

                //Texture shaders
                Filters.Scene["ExoriumMod:FlamingSphere"] = new Filter(new ScreenShaderData(flamingSphereRef, "FlamingSpherePass"), EffectPriority.VeryHigh);
                Filters.Scene["ExoriumMod:FlamingSphere"].Load();

                Filters.Scene["ExoriumMod:ShadowmancerShade"] = new Filter(new ScreenShaderData(shadowmancerShadeRef, "ShadePass"), EffectPriority.VeryHigh);
                Filters.Scene["ExoriumMod:ShadowmancerShade"].Load();

                Filters.Scene["ExoriumMod:VioletPortal"] = new Filter(new ScreenShaderData(violetPortalRef, "PortalPass"), EffectPriority.Medium);
                Filters.Scene["ExoriumMod:VioletPortal"].Load();

                Filters.Scene["ExoriumMod:FireAura"] = new Filter(new ScreenShaderData(fireAuraRef, "FlamesPass"), EffectPriority.Low); //WHY DID I LEAVE THE PASS NAME P0 AHHHHHHHHH
                Filters.Scene["ExoriumMod:FireAura"].Load();

                GameShaders.Misc["FlamingSphere"] = new MiscShaderData(flamingSphereRef, "FlamingSpherePass");
                GameShaders.Misc["ExoriumMod:LaserEffect"] = new MiscShaderData(laserEffectRef, "TrailPass");
            }
        }

        public override void Unload()
        {
            instance = null;
        }

        public override void PostSetupContent()
        {
            BossChecklistCC();
            CensusCC();
            FargoMutantCC();
        }

        private void BossChecklistCC()
        {
            if (!ModLoader.TryGetMod("BossChecklist", out Mod bcl)) { return; }
            if (bcl.Version < new Version(1, 6)) { return; }

            bcl.Call(
                "LogBoss",
                this,
                nameof(Content.Bosses.Shadowmancer.AssierJassad),
                1.9f,
                (Func<bool>)(() => DownedBossSystem.downedShadowmancer),
                ModContent.NPCType<Content.Bosses.Shadowmancer.AssierJassad>(),
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = ModContent.ItemType<Content.Items.Accessories.RitualBone>(),
                });

            bcl.Call(
                "LogMiniBoss",
                this,
                nameof(Content.Bosses.GemsparklingHive.GemsparklingHive),
                2.01f,
                (() => DownedBossSystem.downedGemsparklingHive),
                NPCType<Content.Bosses.GemsparklingHive.GemsparklingHive>(),
                new Dictionary<string, object>()
                {

                });

            bcl.Call(
                "LogBoss",
                this, nameof(Content.Bosses.BlightedSlime.BlightedSlime),
                3.1f,
                (Func<bool>)(() => DownedBossSystem.downedBlightslime),
                ModContent.NPCType<Content.Bosses.BlightedSlime.BlightedSlime>(),
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = ModContent.ItemType<Content.Bosses.BlightedSlime.TaintedSludge>(),
                });

            bcl.Call(
                "LogBoss",
                this, nameof(Content.Bosses.CrimsonKnight.Caravene),
                6.99f,
                (() => DownedBossSystem.downedCrimsonKnight),
                ModContent.NPCType<Content.Bosses.CrimsonKnight.Caravene>(),
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = ModContent.ItemType<Content.Bosses.CrimsonKnight.TwistedCrown>(),
                    ["customPortrait"] = (SpriteBatch spriteBatch, Rectangle rect, Color color) => {
                        Texture2D texture = ModContent.Request<Texture2D>(AssetDirectory.BestiaryEnemyImage + "Caravene_Bestiary").Value;
                        Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 5 * 2));
                        spriteBatch.Draw(texture, centered, null, color, 0, Vector2.Zero, .8f, SpriteEffects.None, 0);
                    }
                });

        }

        private void CensusCC()
        {
            if (!ModLoader.TryGetMod("Census", out Mod census)) { return; }
            census.Call("TownNPCCondition", ModContent.NPCType<Content.NPCs.Town.Lunatic>(),
                ModContent.GetInstance<Content.NPCs.Town.Lunatic>().GetLocalization("Census.Spawncondition").WithFormatArgs());
        }

        private void FargoMutantCC()
        {
            if (!ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant)) { return; }
            fargosMutant.Call("AddSummon", 1.9f, ItemType<Content.Items.Accessories.RitualBone>(), (Func<bool>)(() => DownedBossSystem.downedShadowmancer), 10000);
            fargosMutant.Call("AddSummon", 3.1f, ItemType<Content.Bosses.BlightedSlime.TaintedSludge>(), (Func<bool>)(() => DownedBossSystem.downedBlightslime), 125000);
            fargosMutant.Call("AddSummon", 6.99f, ItemType<Content.Bosses.CrimsonKnight.TwistedCrown>(), (Func<bool>)(() => DownedBossSystem.downedCrimsonKnight), 170000);

            //fargosMutant.Call("AddIndestructibleRectangle", new Rectangle((WorldDataSystem.shadowAltarCoordsX - ExoriumStructures._shadowhouseWidth/2) * 16, (WorldDataSystem.shadowAltarCoordsY - ExoriumStructures._shadowhouseHeight/2) * 16, ExoriumStructures._shadowhouseWidth * 16, ExoriumStructures._shadowhouseHeight * 16));
            //fargosMutant.Call("AddIndestructibleRectangle", WorldDataSystem.FallenTowerRect);
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            ExoriumPacketType msgType = (ExoriumPacketType)reader.ReadByte();
            int npcType;
            int netID;
            switch (msgType)
            {
                case ExoriumPacketType.ShadowmancerSpawn:
                    npcType = reader.ReadInt32();
                    netID = reader.ReadInt32();
                    Content.Tiles.ShadowAltarTile.HandleNPC(npcType, netID, true, whoAmI);
                    break;
                case ExoriumPacketType.CaraveneBagDrop:
                    foreach(NPC npc in Main.npc)
                    {
                        if (npc.type == NPCType<Content.Bosses.CrimsonKnight.CaraveneBattleIntermission>())
                            npc.ai[3] = 1;
                    }
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var netMessage = instance.GetPacket();
                        netMessage.Write((byte)ExoriumPacketType.CaraveneBagDrop);
                        netMessage.Send();
                    }
                    break;
            }
        }
    }

    internal enum ExoriumPacketType : byte
    {
        ShadowmancerSpawn,
        CaraveneBagDrop
    }
}