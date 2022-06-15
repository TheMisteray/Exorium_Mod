using Microsoft.Xna.Framework;
using ExoriumMod.Core;
using System;
using Terraria;
using Terraria.ModLoader;
using System.IO;
using Terraria.Graphics.Effects;
using ExoriumMod.Content.Skies;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Graphics;

namespace ExoriumMod
{
	public partial class ExoriumMod : Mod
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
                SkyManager.Instance["ExoriumMod:DeadlandsSky"] = new DeadlandsSky();
                Filters.Scene["ExoriumMod:DeadlandsSky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryLow);
            }
        }

        public override void Unload()
        {
            instance = null;
        }

        public override void AddRecipeGroups()
        {
            RecipeGroup woodGroup = RecipeGroup.recipeGroups[RecipeGroup.recipeGroupIDs["Wood"]];
            woodGroup.ValidItems.Add(ModContent.ItemType<Content.Items.TileItems.Deadwood>());
        }

        public override void PostSetupContent()
        {
            //Cross Content
            //BossChecklistCC();
            //CensusCC();
            //FargoMutantCC();

            base.PostSetupContent();
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