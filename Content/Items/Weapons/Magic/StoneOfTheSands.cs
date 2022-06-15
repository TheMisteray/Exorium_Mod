using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Terraria.DataStructures;

namespace ExoriumMod.Content.Items.Weapons.Magic
{
    class StoneOfTheSands : ModItem
    {
        public override string Texture => AssetDirectory.MagicWeapon + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stone of the Sands");
            Tooltip.SetDefault("Creates a ring of sand balls that rotate aroung you \n" +
                "Right click to shoot them forward");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.rare = 1;
            Item.damage = 17;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 7;
            Item.noMelee = true;
            Item.value = Item.sellPrice(silver: 14, copper: 35);
            Item.useStyle = 4;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.knockBack = 4;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item43;
            Item.shoot = ProjectileType<DuneBall>();
            Item.shootSpeed = 5;
        }

        int[] proj = new int[5];

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                for (int i = 0; i < 5; i++)
                {
                    Projectile project = Main.projectile[proj[i]];
                    if (project.type == ProjectileType<DuneBall>() && project.owner == player.whoAmI)
                    {
                        Main.projectile[proj[i]].ai[0] = 1;
                        Main.projectile[proj[i]].damage = (Main.projectile[proj[i]].damage/3) * 2;
                    }
                }
                Item.mana = 0;
                Item.shoot = ProjectileID.None;
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Main.projectile[proj[i]].ai[0] == 0 && Main.projectile[proj[i]].type == ProjectileType<DuneBall>() && Main.projectile[proj[i]].owner == player.whoAmI) Main.projectile[proj[i]].Kill();
                }
                Item.mana = 7;
                Item.shoot = ProjectileType<DuneBall>();
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 5; i++)
            {
                proj[i] = Projectile.NewProjectile(source, position.X, position.Y, 0, 0, type, damage, knockback, player.whoAmI, 0, 60 * i);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.DunestoneBar>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
    class DuneBall : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 400;
            Projectile.penetrate = 3;

        }

        public override void AI()
        {
            Vector2 dustPosition = Projectile.Center + new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));
            //Making player variable "player" set as the projectile's owner
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[0] == 0)
            {
                Projectile.tileCollide = false;

                float focusX = player.Center.X;
                float focusY = player.Center.Y;
                double deg = (double)Projectile.ai[1] * 2.4; //Speed
                double rad = deg * (Math.PI / 180); //Convert degrees to radians
                double dist = 64; //Distance away from the player
                Projectile.position.X = focusX - (int)(Math.Cos(rad + 1.5) * dist) - Projectile.width / 2;
                Projectile.position.Y = focusY - (int)(Math.Sin(rad + 1.5) * dist) - Projectile.height / 2;
                //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
                Projectile.ai[1] += 1f;
            }
            else
            {
                if (Projectile.ai[0] == 1)
                {
                    //Find cursor and shoot at
                    float maxDistance = 15f; // Speed
                    Vector2 vectorToCursor = Main.MouseWorld - Projectile.Center;
                    float distanceToCursor = vectorToCursor.Length();
                    if (distanceToCursor > maxDistance)
                    {
                        distanceToCursor = maxDistance / distanceToCursor;
                        vectorToCursor *= distanceToCursor;
                    }
                    int velocityXBy1000 = (int)(vectorToCursor.X * 1000f);
                    int oldVelocityXBy1000 = (int)(Projectile.velocity.X * 1000f);
                    int velocityYBy1000 = (int)(vectorToCursor.Y * 1000f);
                    int oldVelocityYBy1000 = (int)(Projectile.velocity.Y * 1000f);
                    // Client Sync
                    if (velocityXBy1000 != oldVelocityXBy1000 || velocityYBy1000 != oldVelocityYBy1000)
                    {
                        Projectile.netUpdate = true;
                    }
                    Projectile.velocity = vectorToCursor;
                    Projectile.penetrate = 1;
                    Projectile.timeLeft = 400;
                    Projectile.ai[0]++;
                }
                if (Projectile.timeLeft == 390)
                    Projectile.tileCollide = true;
                Projectile.aiStyle = 0;
            }
            Dust.NewDustPerfect(dustPosition, 32, null, 100, default(Color), 0.8f);
        }
    }
}
