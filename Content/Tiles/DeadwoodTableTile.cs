using ExoriumMod.Core;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ExoriumMod.Content.Tiles
{
	public class DeadwoodTableTile : ModTile
	{
		public override string Texture => AssetDirectory.Tile + Name;

		public override void SetStaticDefaults()
		{
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.CoordinateHeights = new[] { 18, 18};
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Deadwood Table");
			AddMapEntry(new Color(90, 90, 90), name);
			DustType = ModContent.DustType<Dusts.DeadwoodTreeDust>();
			TileID.Sets.DisableSmartCursor[Type] = true;
			AdjTiles = new int[] { TileID.Tables };
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		//public override void KillMultiTile(int i, int j, int frameX, int frameY)
		//{
		//	Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<Items.TileItems.DeadwoodTable>());
		//}
	}
}
