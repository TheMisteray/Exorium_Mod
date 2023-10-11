﻿using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ExoriumMod.Content.Tiles
{
    class CrateTile : ModTile
    {
        public override string HighlightTexture => AssetDirectory.Tile + Name + "_Highlight";

        public override string Texture => AssetDirectory.Tile + Name;

        public override void SetStaticDefaults()
        {
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = false;
            Main.tileContainer[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.Origin = new Point16(1, 1);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
            TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
            TileObjectData.newTile.AnchorInvalidTiles = new[] { 127 };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Crate");
            AddMapEntry(new Color(200, 200, 200), name);
            TileID.Sets.DisableSmartCursor[Type] = true;

            //Keeping one instance of this just in case
            //ContainerName/* tModPorter Note: Removed. Override DefaultContainerName instead */.SetDefault("Crate");

            TileID.Sets.BasicDresser[Type] = true;
        }

        public override LocalizedText DefaultContainerName(int frameX, int frameY)
        {
            return CreateMapEntryName();
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Main.CancelClothesWindow(true);
            Main.mouseRightRelease = false;
            int left = (int)(Main.tile[Player.tileTargetX, Player.tileTargetY].TileFrameX / 18);
            left %= 3;
            left = Player.tileTargetX - left;
            int top = Player.tileTargetY - (int)(Main.tile[Player.tileTargetX, Player.tileTargetY].TileFrameY / 18);
            if (player.sign > -1)
            {
                SoundEngine.PlaySound(SoundID.MenuClose);
                player.sign = -1;
                Main.editSign = false;
                Main.npcChatText = string.Empty;
            }
            if (Main.editChest)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                Main.editChest = false;
                Main.npcChatText = string.Empty;
            }
            if (player.editedChestName)
            {
                NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f, 0f, 0f, 0, 0, 0);
                player.editedChestName = false;
            }
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                if (left == player.chestX && top == player.chestY && player.chest != -1)
                {
                    player.chest = -1;
                    Recipe.FindRecipes();
                    SoundEngine.PlaySound(SoundID.MenuClose);
                }
                else
                {
                    NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, left, (float)top, 0f, 0f, 0, 0, 0);
                    Main.stackSplit = 600;
                }
            }
            else
            {
                player.piggyBankProjTracker.Clear(); 
                player.voidLensChest.Clear();
                int num213 = Chest.FindChest(left, top);
                if (num213 != -1)
                {
                    Main.stackSplit = 600;
                    if (num213 == player.chest)
                    {
                        player.chest = -1;
                        Recipe.FindRecipes();
                        SoundEngine.PlaySound(SoundID.MenuClose);
                    }
                    else if (num213 != player.chest && player.chest == -1)
                    {
                        player.chest = num213;
                        Main.playerInventory = true;
                        Main.recBigList = false;
                        SoundEngine.PlaySound(SoundID.MenuOpen);
                        player.chestX = left;
                        player.chestY = top;
                    }
                    else
                    {
                        player.chest = num213;
                        Main.playerInventory = true;
                        Main.recBigList = false;
                        SoundEngine.PlaySound(SoundID.MenuTick);
                        player.chestX = left;
                        player.chestY = top;
                    }
                    Recipe.FindRecipes();
                }
            }
            return true;
        }

        public override void MouseOverFar(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
            int left = Player.tileTargetX;
            int top = Player.tileTargetY;
            left -= (int)(tile.TileFrameX % 54 / 18);
            if (tile.TileFrameY % 36 != 0)
            {
                top--;
            }
            int chestIndex = Chest.FindChest(left, top);
            player.cursorItemIconID = -1;
            if (Main.chest[chestIndex].name != "")
            {
                player.cursorItemIconText = Main.chest[chestIndex].name;
            }
            else
            {
                player.cursorItemIconText = Name; //Unsure about this
            }
            if (player.cursorItemIconText == Name)
            {
                player.cursorItemIconID = ModContent.ItemType<Items.TileItems.StructureTileItems.ShadowmancerTileItems.Crate>();
                player.cursorItemIconText = "";
            }
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            if (player.cursorItemIconText == "")
            {
                player.cursorItemIconEnabled = false;
                player.cursorItemIconID = 0;
            }
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
            int left = Player.tileTargetX;
            int top = Player.tileTargetY;
            left -= (int)(tile.TileFrameX % 54 / 18);
            if (tile.TileFrameY % 36 != 0)
            {
                top--;
            }
            int num138 = Chest.FindChest(left, top);
            player.cursorItemIconID = -1;
            if (Main.chest[num138].name != "")
            {
                player.cursorItemIconText = Main.chest[num138].name;
            }
            else
            {
                player.cursorItemIconText = Name;
            }
            if (player.cursorItemIconText == Name)
            {
                player.cursorItemIconID = ModContent.ItemType<Items.TileItems.StructureTileItems.ShadowmancerTileItems.Crate>();
                player.cursorItemIconText = "";
            }
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            //Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 32, ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */);
            Chest.DestroyChest(i, j);
        }
    }
}
