using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Tiles
{
    class ShadowAltarTile : ModTile
    {
        public int netID = 0;

        public override string HighlightTexture => AssetDirectory.Tile + Name + "_Highlight";

        public override string Texture => AssetDirectory.Tile + Name;

        public override void SetStaticDefaults()
        {
            TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2);
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileContainer[Type] = false;
            Main.tileLavaDeath[Type] = false;
            TileID.Sets.HasOutlines[Type] = true;
            TileObjectData.newTile.Origin = new Point16(1, 1);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16};
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.WaterDeath = false;
            TileObjectData.newTile.AnchorInvalidTiles = new[] { 127 };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
            TileObjectData.newTile.StyleHorizontal = true;
            LocalizedText name = CreateMapEntryName();
            TileObjectData.addTile(Type);
            // name.SetDefault("Shadow Altar");
            AddMapEntry(new Color(20, 20, 20), name);
            TileID.Sets.DisableSmartCursor[Type] = true;
            MinPick = 10000;
        }
        
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return false;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Chest.DestroyChest(i, j);
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            if (!NPC.AnyNPCs(NPCType<Bosses.Shadowmancer.AssierJassad>()))
            {
                HandleNPC(NPCType<Content.Bosses.Shadowmancer.AssierJassad>(), netID, false, Main.LocalPlayer.whoAmI);
                return true;
            }
            return false;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
            int left = i;
            int top = j;
            if (tile.TileFrameX % 36 != 0)
            {
                left--;
            }
            if (tile.TileFrameY != 0)
            {
                top--;
            }
            player.cursorItemIconID = -1;
            player.cursorItemIconText = Language.GetTextValue("Shadow Altar");
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
        }

        public override void MouseOverFar(int i, int j)
        {
            MouseOver(i, j);
            Player player = Main.LocalPlayer;
            if (player.cursorItemIconText == "")
            {
                player.cursorItemIconEnabled = false;
                player.cursorItemIconID = 0;
            }
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public static void HandleNPC(int type, int syncID = 0, bool forceHandle = false, int whoAmI = 0)
        {
            bool syncData = forceHandle || Main.netMode == 0;
            if (syncData)
            {
                SpawnNPC(type, forceHandle, syncID, whoAmI);
            }
            else
            {
                SyncNPC(type, syncID);
            }
        }

        private static void SyncNPC(int type, int syncID = 0)
        {
            var netMessage = ExoriumMod.instance.GetPacket();
            netMessage.Write((byte)ExoriumPacketType.ShadowmancerSpawn);
            netMessage.Write(type);
            netMessage.Write(syncID);
            netMessage.Send();
        }

        private static void SpawnNPC(int type, bool syncData = false, int syncID = 0, int whoAmI = 0)
        {
            Player player;
            if (!syncData)
            {
                player = Main.LocalPlayer;
            }
            else
            {
                player = Main.player[whoAmI];
            }
            int x = (int)player.Center.X;
            int y = (int)player.Bottom.Y - 200;
            int index = NPC.NewNPC(new EntitySource_SpawnNPC(), x, y, type, 0, 0, 0, 0, 180);
            if (syncID < 0)
            {
                //NPC refNPC = new NPC();
                //refNPC.netDefaults(syncID);
                Main.npc[index].SetDefaults(syncID);
            }
        }
    }
}
