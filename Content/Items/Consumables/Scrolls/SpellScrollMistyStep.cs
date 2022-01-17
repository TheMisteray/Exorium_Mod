using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

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
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useTurn = true;
            item.useStyle = 4;
            item.UseSound = SoundID.Item4;
            item.maxStack = 30;
            item.consumable = true;
            item.rare = 2;
            item.mana = 50;
            item.maxStack = 30;
            item.value = Item.buyPrice(gold: 5);
            item.noUseGraphic = true;
            item.shoot = 10;
            item.scale = .5f;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.HasBuff(BuffType<Buffs.ScrollCooldown>());
        }

        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(BuffType<Buffs.ScrollCooldown>(), 7200);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
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
            while ((Main.tile[(int)telePos.X, (int)telePos.Y].active() && Main.tileSolid[Main.tile[(int)telePos.X, (int)telePos.Y].type]) ||
                (Main.tile[(int)telePos.X + 1, (int)telePos.Y].active() && Main.tileSolid[Main.tile[(int)telePos.X + 1, (int)telePos.Y].type]) ||
                (Main.tile[(int)telePos.X, (int)telePos.Y + 1].active() && Main.tileSolid[Main.tile[(int)telePos.X, (int)telePos.Y + 1].type]) ||
                (Main.tile[(int)telePos.X + 1, (int)telePos.Y + 1].active() && Main.tileSolid[Main.tile[(int)telePos.X + 1, (int)telePos.Y + 1].type]) ||
                (Main.tile[(int)telePos.X, (int)telePos.Y + 2].active() && Main.tileSolid[Main.tile[(int)telePos.X, (int)telePos.Y + 2].type]) ||
                (Main.tile[(int)telePos.X + 1, (int)telePos.Y + 2].active() && Main.tileSolid[Main.tile[(int)telePos.X + 1, (int)telePos.Y + 2].type]) ||
                (Main.tile[(int)telePos.X + 2, (int)telePos.Y].active() && Main.tileSolid[Main.tile[(int)telePos.X + 2, (int)telePos.Y].type]) ||
                (Main.tile[(int)telePos.X + 2, (int)telePos.Y + 1].active() && Main.tileSolid[Main.tile[(int)telePos.X + 2, (int)telePos.Y + 1].type]) ||
                (Main.tile[(int)telePos.X + 2, (int)telePos.Y + 2].active() && Main.tileSolid[Main.tile[(int)telePos.X + 2, (int)telePos.Y + 2].type]) ||
                (Main.tile[(int)telePos.X, (int)telePos.Y + 3].active() && Main.tileSolid[Main.tile[(int)telePos.X, (int)telePos.Y].type + 3]) ||
                (Main.tile[(int)telePos.X + 1, (int)telePos.Y + 3].active() && Main.tileSolid[Main.tile[(int)telePos.X + 1, (int)telePos.Y + 3].type]) ||
                (Main.tile[(int)telePos.X + 2, (int)telePos.Y + 3].active() && Main.tileSolid[Main.tile[(int)telePos.X + 2, (int)telePos.Y + 3].type]))
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
