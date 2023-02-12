using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Consumables.Scrolls
{
    class SpellScrollMistyStep : ModItem
    {
        public override string Texture => AssetDirectory.SpellScroll + Name;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Casts Misty Step \n" +
                "Teleports you to anywhere your character can see, but not through blocks");
            DisplayName.SetDefault("Spell Scroll: Misty Step");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useTurn = true;
            Item.useStyle = 4;
            Item.UseSound = SoundID.Item4;
            Item.maxStack = 30;
            Item.consumable = true;
            Item.rare = 2;
            Item.mana = 50;
            Item.maxStack = 30;
            Item.value = Item.buyPrice(gold: 5);
            Item.noUseGraphic = true;
            Item.shoot = 10;
            Item.scale = .5f;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.HasBuff(BuffType<Buffs.ScrollCooldown>());
        }

        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(BuffType<Buffs.ScrollCooldown>(), 7200);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //Dust at player
            Vector2 dustSpeed = new Vector2(0, 3);
            for (int i = 0; i < 20; i++)
            {
                Vector2 perturbedDustSpeed = dustSpeed.RotatedBy(MathHelper.ToRadians(Main.rand.Next(0, 361)));
                Dust.NewDust(player.position, player.width, player.height, 20, perturbedDustSpeed.X * Main.rand.NextFloat(), perturbedDustSpeed.Y * Main.rand.NextFloat(), 100, default, Main.rand.NextFloat(1.5f));
            }

            Vector2 mouse = Main.MouseWorld;
            Vector2 offset = Vector2.Zero;

            Vector2 unit = player.Center - mouse;
            unit.Normalize();

            //If tile in the way shorten teleport
            while (!Collision.CanHitLine(player.position, 0, 0, mouse, 0, 0))
            {
                offset += unit;
                mouse += unit;
            }

            //Make teleport spot player's feet
            mouse.X -= player.width / 2;
            mouse.Y -= player.height;

            Vector2 telePos = new Vector2(mouse.ToTileCoordinates().X, mouse.ToTileCoordinates().Y);

            //If teleport spot is bad shorten teleport, (3x4 spot)
            while ((Main.tile[(int)telePos.X, (int)telePos.Y].HasTile && Main.tileSolid[Main.tile[(int)telePos.X, (int)telePos.Y].TileType]) ||
                (Main.tile[(int)telePos.X + 1, (int)telePos.Y].HasTile && Main.tileSolid[Main.tile[(int)telePos.X + 1, (int)telePos.Y].TileType]) ||
                (Main.tile[(int)telePos.X, (int)telePos.Y + 1].HasTile && Main.tileSolid[Main.tile[(int)telePos.X, (int)telePos.Y + 1].TileType]) ||
                (Main.tile[(int)telePos.X + 1, (int)telePos.Y + 1].HasTile && Main.tileSolid[Main.tile[(int)telePos.X + 1, (int)telePos.Y + 1].TileType]) ||
                (Main.tile[(int)telePos.X, (int)telePos.Y + 2].HasTile && Main.tileSolid[Main.tile[(int)telePos.X, (int)telePos.Y + 2].TileType]) ||
                (Main.tile[(int)telePos.X + 1, (int)telePos.Y + 2].HasTile && Main.tileSolid[Main.tile[(int)telePos.X + 1, (int)telePos.Y + 2].TileType]) ||
                (Main.tile[(int)telePos.X + 2, (int)telePos.Y].HasTile && Main.tileSolid[Main.tile[(int)telePos.X + 2, (int)telePos.Y].TileType]) ||
                (Main.tile[(int)telePos.X + 2, (int)telePos.Y + 1].HasTile && Main.tileSolid[Main.tile[(int)telePos.X + 2, (int)telePos.Y + 1].TileType]) ||
                (Main.tile[(int)telePos.X + 2, (int)telePos.Y + 2].HasTile && Main.tileSolid[Main.tile[(int)telePos.X + 2, (int)telePos.Y + 2].TileType]) ||
                (Main.tile[(int)telePos.X, (int)telePos.Y + 3].HasTile && Main.tileSolid[Main.tile[(int)telePos.X, (int)telePos.Y].TileType + 3]) ||
                (Main.tile[(int)telePos.X + 1, (int)telePos.Y + 3].HasTile && Main.tileSolid[Main.tile[(int)telePos.X + 1, (int)telePos.Y + 3].TileType]) ||
                (Main.tile[(int)telePos.X + 2, (int)telePos.Y + 3].HasTile && Main.tileSolid[Main.tile[(int)telePos.X + 2, (int)telePos.Y + 3].TileType]))
            {
                mouse += unit;
                offset += unit;

                //if the player would teleport backwards
                if ((Main.MouseWorld - player.Center).Length() <= offset.Length())
                {
                    player.Teleport(player.position, 5);
                    return false;
                }
                telePos = new Vector2(mouse.ToTileCoordinates().X, mouse.ToTileCoordinates().Y);
            }

            player.Teleport(mouse, 5);

            //Dust at teleport spot
            for (int i = 0; i < 20; i++)
            {
                Vector2 perturbedDustSpeed = dustSpeed.RotatedBy(MathHelper.ToRadians(Main.rand.Next(0, 361)));
                Dust.NewDust(player.position, player.width, player.height, 20, perturbedDustSpeed.X * Main.rand.NextFloat(), perturbedDustSpeed.Y * Main.rand.NextFloat(), 100, default, Main.rand.NextFloat(1.5f));
            }

            return false;
        }
    }
}
