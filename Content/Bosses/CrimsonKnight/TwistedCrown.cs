using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using System;
using System.Drawing;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    internal class TwistedCrown : ModItem
    {
        public override string Texture => AssetDirectory.CrimsonKnight + Name;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 12;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.value = 0;
            Item.width = 16;
            Item.height = 14;
            Item.rare = 1;
            Item.maxStack = 99;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = 4;
            Item.UseSound = SoundID.Item44;
            Item.consumable = true;
        }
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossSpawners;
        }

        public override bool CanUseItem(Player player)
        {
            return Framing.GetTileSafely(player.Center.ToTileCoordinates()).WallType == WallType<Walls.StructureWalls.FallenTowerWalls.CharredObsidianWall>() && player.getRect().Intersects(Core.Systems.WorldDataSystem.FallenTowerRect) && !NPC.AnyNPCs(NPCType<Caravene>()) && !NPC.AnyNPCs(NPCType<CaraveneBattleIntermission>()) && !NPC.AnyNPCs(NPCType<ExoriumRed>());
        }

        public override Nullable<bool> UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.position);
                int type = NPCType<Caravene>();
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // If the player is not in multiplayer, spawn directly
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                }
                else
                {
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
                }
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Items.Materials.Metals.DarksteelBar>(), 2);
            recipe.AddIngredient(ItemID.Ruby);
            recipe.AddTile(TileType<Tiles.ShadowAltarTile>());
            recipe.Register();
        }
    }
}
