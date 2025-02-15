using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ExoriumMod.Content.Tiles
{
	public class DeadwoodBedTile : ModTile
	{
        public override string HighlightTexture => AssetDirectory.Tile + Name + "_Highlight";

		public override string Texture => AssetDirectory.Tile + Name;

		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.HasOutlines[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2); //this style already takes care of direction for us
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Deadwood Bed");
			AddMapEntry(new Color(40, 40, 40), name);
			DustType = ModContent.DustType<Dusts.DeadwoodTreeDust>();
			TileID.Sets.DisableSmartCursor[Type] = true;
			AdjTiles = new int[] { TileID.Beds };
			TileID.Sets.CanBeSleptIn[Type] = true;
		}

		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
		{
			return true;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 1;
		}

		//public override void KillMultiTile(int i, int j, int frameX, int frameY)
		//{
		//	Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 64, 32, ModContent.ItemType<Items.TileItems.DeadwoodBed>());
		//}

		public override bool RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			int spawnX = i - tile.TileFrameX / 18;
			int spawnY = j + 2;
			spawnX += tile.TileFrameX >= 72 ? 5 : 2;
			if (tile.TileFrameY % 38 != 0)
			{
				spawnY--;
			}
			player.FindSpawn();
			if (player.SpawnX == spawnX && player.SpawnY == spawnY)
			{
				player.RemoveSpawn();
				if (Main.netMode != NetmodeID.Server)
					Main.NewText("Spawn point removed!", new Color(255, 240, 20));
			}
			else if (Player.CheckSpawn(spawnX, spawnY))
			{
				player.ChangeSpawn(spawnX, spawnY);
				if (Main.netMode != NetmodeID.Server)
					Main.NewText("Spawn point set!", new Color(255, 240, 20));
			}
			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<Items.TileItems.DeadwoodBed>();
		}
	}
}
