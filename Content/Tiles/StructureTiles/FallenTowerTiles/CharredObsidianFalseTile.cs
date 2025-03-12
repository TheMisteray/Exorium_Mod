using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace ExoriumMod.Content.Tiles.StructureTiles.FallenTowerTiles
{
    internal class CharredObsidianFalseTile : ModTile
    {
        public override string Texture => AssetDirectory.Tile + Name;

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMerge[TileID.Ash][this.Type] = true;
            Main.tileBlockLight[Type] = false;
            AddMapEntry(new Color(25, 6, 2)); //Match wall color
            DustType = DustID.Obsidian;
            HitSound = SoundID.Tink;
            MineResist = 5f;
            MinPick = 210;
            Main.dust[DustType].color = new Color(51, 12, 5);
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (NPC.AnyNPCs(NPCType<Bosses.CrimsonKnight.Caravene>()) || NPC.AnyNPCs(NPCType<Bosses.CrimsonKnight.ExoriumRed>()) || NPC.AnyNPCs(NPCType<Bosses.CrimsonKnight.CaraveneBattleIntermission>()) || NPC.AnyNPCs(NPCType<Bosses.CrimsonKnight.CaravenePhaseTransition>()))
                Main.tileSolid[Type] = true;
            else
                Main.tileSolid[Type] = false;
        }

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            if (NPC.AnyNPCs(NPCType<Bosses.CrimsonKnight.Caravene>()) || NPC.AnyNPCs(NPCType<Bosses.CrimsonKnight.ExoriumRed>()) || NPC.AnyNPCs(NPCType<Bosses.CrimsonKnight.CaraveneBattleIntermission>()) || NPC.AnyNPCs(NPCType<Bosses.CrimsonKnight.CaravenePhaseTransition>()))
            {
                int dust = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustID.Torch, 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = Main.rand.NextFloat() * 2 + 1.5f;
            }
            base.DrawEffects(i, j, spriteBatch, ref drawData);
        }
    }
}
