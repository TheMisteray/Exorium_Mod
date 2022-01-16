using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Tiles
{
    class ShadowAltarTile : ModTile
    {
        public override string HighlightTexture => AssetDirectory.Tile + Name + "_Highlight";

        public override bool Autoload(ref string name, ref string texture)
        {
            texture = AssetDirectory.Tile + name;
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
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
            TileObjectData.newTile.HookCheck = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.FindEmptyChest), -1, 0, true);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.AfterPlacement_Hook), -1, 0, false);
            TileObjectData.newTile.StyleHorizontal = true;
            ModTranslation name = CreateMapEntryName();
            TileObjectData.addTile(Type);
            name.SetDefault("Shadow Altar");
            AddMapEntry(new Color(20, 20, 20), name);
            disableSmartCursor = true;
            minPick = 10000;
        }
        
        public override bool HasSmartInteract()
        {
            return false;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 64, 32, chestDrop);
            Chest.DestroyChest(i, j);
        }

        public override bool NewRightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            if (!NPC.AnyNPCs(NPCType<Bosses.Shadowmancer.AssierJassad>()))
            {
                NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - 100, NPCType<Bosses.Shadowmancer.AssierJassad>(), 0, 0, 1, 100, 180);
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
            if (tile.frameX % 36 != 0)
            {
                left--;
            }
            if (tile.frameY != 0)
            {
                top--;
            }
            player.showItemIcon2 = -1;
            if (ExoriumWorld.downedShadowmancer)
            {
                player.showItemIconText = Language.GetTextValue("Shadow Altar");
            }
            else
                player.showItemIconText = Language.GetTextValue("???");
            player.noThrow = 2;
            player.showItemIcon = true;
        }

        public override void MouseOverFar(int i, int j)
        {
            MouseOver(i, j);
            Player player = Main.LocalPlayer;
            if (player.showItemIconText == "")
            {
                player.showItemIcon = false;
                player.showItemIcon2 = 0;
            }
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}
