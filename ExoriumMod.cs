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
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.UI;
using Terraria.Localization;
using Terraria.ModLoader.Core;
using Terraria.Utilities;
using Terraria.UI.Chat;
using Terraria.ModLoader.IO;

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
                Ref<Effect> HeatDistortEffectRef = new Ref<Effect>(Assets.Request<Effect>("Effects/HeatDistortion", AssetRequestMode.ImmediateLoad).Value);
                Ref<Effect> screenRef = new Ref<Effect>(Assets.Request<Effect>("Effects/ShockwaveEffect", AssetRequestMode.ImmediateLoad).Value);
                Ref<Effect> flamingSphereRef = new Ref<Effect>(Assets.Request<Effect>("Effects/FlamingSphere", AssetRequestMode.ImmediateLoad).Value);
                Ref<Effect> shadowmancerShadeRef = new Ref<Effect>(Assets.Request<Effect>("Effects/ShadowmancerShade", AssetRequestMode.ImmediateLoad).Value);
                Ref<Effect> violetPortalRef = new Ref<Effect>(Assets.Request<Effect>("Effects/VioletPortal", AssetRequestMode.ImmediateLoad).Value);
                Ref<Effect> fireAuraRef = new Ref<Effect>(Assets.Request<Effect>("Effects/FireAura", AssetRequestMode.ImmediateLoad).Value);
                Ref<Effect> caraveneTitleRef = new Ref<Effect>(Assets.Request<Effect>("Effects/CaraveneTitle", AssetRequestMode.ImmediateLoad).Value);
                Ref<Effect> infernalRiftRef = new Ref<Effect>(Assets.Request<Effect>("Effects/InfernalRift", AssetRequestMode.ImmediateLoad).Value);
                Ref<Effect> laserEffectRef = new Ref<Effect>(Assets.Request<Effect>("Effects/Deathray", AssetRequestMode.ImmediateLoad).Value);



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
            //CensusCC();
            //FargoMutantCC();
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
                "LogBoss",
                this, nameof(Content.Bosses.BlightedSlime.BlightedSlime),
                3.1f,
                (Func<bool>)(() => DownedBossSystem.downedBlightslime),
                ModContent.NPCType<Content.Bosses.BlightedSlime.BlightedSlime>(),
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = ModContent.ItemType<Content.Bosses.BlightedSlime.TaintedSludge>(),
                });
        }

        private void CensusCC()
        {
            if (!ModLoader.TryGetMod("Census", out Mod census)) { return; }
            census.Call("TownNPCCondition", ModContent.NPCType<Content.NPCs.Town.Lunatic>(), "Will show up when he feels like it. (After there are at least 3 other NPCs in your town).");
        }

        private void FargoMutantCC()
        {
            if (!ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant)) { return; }
            fargosMutant.Call("AddSummon", 3.1f, "ExoriumMod", "TaintedSludge", (Func<bool>)(() => DownedBossSystem.downedBlightslime), 125000);
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            ExoriumPacketType msgType = (ExoriumPacketType)reader.ReadByte();
            switch (msgType)
            {
                case ExoriumPacketType.ShadowmancerSpawn:
                    int npcType = reader.ReadInt32();
                    int netID = reader.ReadInt32();
                    Content.Tiles.ShadowAltarTile.HandleNPC(npcType, netID, true, whoAmI);
                    break;
            }
        }
    }

    internal enum ExoriumPacketType : byte
    {
        ShadowmancerSpawn
    }
}