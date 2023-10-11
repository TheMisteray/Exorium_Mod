using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ExoriumMod.Content.Tiles
{
    //An enum on the 3 stages of herb growth.
    public enum PlantStage : byte
    {
        Planted,
        Growing,
        Grown
    }

    class DeadweedTile : ModTile
    {
        public override string Texture => AssetDirectory.Tile + Name;

        private const int FrameWidth = 18; //a field for readibilty and to kick out those magic numbers

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileCut[Type] = true;
            Main.tileNoFail[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.StyleAlch);
            TileObjectData.newTile.AnchorValidTiles = new int[]
            {
				ModContent.TileType<AshenDustTile>()
            };
            TileObjectData.newTile.AnchorAlternateTiles = new int[]
            {
                TileID.ClayPot,
                TileID.PlanterBox
            };
            TileObjectData.addTile(Type);
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;
        }

        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            PlantStage stage = GetStage(i, j);

            Vector2 worldPosition = new Vector2(i, j).ToWorldCoordinates();
            Player nearestPlayer = Main.player[Player.FindClosest(worldPosition, 16, 16)];

            int herbItemType = ModContent.ItemType<Items.Materials.Deadweed>();
            int herbItemStack = 1;

            int seedItemType = ModContent.ItemType<Items.TileItems.DeadweedSeeds>();
            int seedItemStack = 1;

            if (stage == PlantStage.Growing)
            {
                // Default yields, only when growing
                seedItemStack = 1;

                if (nearestPlayer.active && nearestPlayer.HeldItem.type == ItemID.StaffofRegrowth)
                {
                    // Increased yields with Staff of Regrowth, even when not fully grown
                    herbItemStack = 1;
                    seedItemStack = Main.rand.Next(1, 2);
                }
            }
            else if (stage == PlantStage.Grown)
            {
                // Default yields, only when fully grown
                herbItemStack = 1;
                seedItemStack = 2;

                if (nearestPlayer.active && nearestPlayer.HeldItem.type == ItemID.StaffofRegrowth)
                {
                    // Increased yields with Staff of Regrowth, even when not fully grown
                    herbItemStack = Main.rand.Next(1, 2);
                    seedItemStack = Main.rand.Next(1, 4);
                }
            }

            if (herbItemType > 0 && herbItemStack > 0)
            {
                yield return new Item(herbItemType, herbItemStack);
            }

            if (seedItemType > 0 && seedItemStack > 0)
            {
                yield return new Item(seedItemType, seedItemStack);
            }
        }

        public override void RandomUpdate(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j); //Safe way of getting a tile instance
            PlantStage stage = GetStage(i, j); //The current stage of the herb

            if (stage != PlantStage.Grown)
            {
                //Increase the x frame to change the stage
                tile.TileFrameX += FrameWidth;

                //If in multiplayer, sync the frame change
                if (Main.netMode != NetmodeID.SinglePlayer)
                    NetMessage.SendTileSquare(-1, i, j, 1);
            }
        }

        //A method to quickly get the current stage of the herb
        private PlantStage GetStage(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j); //Always use Framing.GetTileSafely instead of Main.tile as it prevents any errors caused from other mods
            return (PlantStage)(tile.TileFrameX / FrameWidth);
        }
    }
}
