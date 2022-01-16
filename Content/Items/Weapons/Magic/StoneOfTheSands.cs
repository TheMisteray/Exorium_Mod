using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;

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
            item.width = 30;
            item.height = 30;
            item.rare = 1;
            item.damage = 17;
            item.magic = true;
            item.mana = 7;
            item.noMelee = true;
            item.value = Item.sellPrice(silver: 14, copper: 35);
            item.useStyle = 4;
            item.useTime = 30;
            item.useAnimation = 30;
            item.knockBack = 4;
            item.autoReuse = false;
            item.UseSound = SoundID.Item43;
            item.shoot = ProjectileType<DuneBall>();
            item.shootSpeed = 5;
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
                        Main.projectile[proj[i]].damage /= 2;
                    }
                }
                item.mana = 0;
                item.shoot = ProjectileID.None;
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Main.projectile[proj[i]].ai[0] == 0 && Main.projectile[proj[i]].type == ProjectileType<DuneBall>() && Main.projectile[proj[i]].owner == player.whoAmI) Main.projectile[proj[i]].Kill();
                }
                item.mana = 7;
                item.shoot = ProjectileType<DuneBall>();
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < 5; i++)
            {
                proj[i] = Projectile.NewProjectile(position.X, position.Y, 0, 0, type, damage, knockBack, player.whoAmI, 0, 60 * i);
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("DunestoneBar"), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
    class DuneBall : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.timeLeft = 400;
            projectile.penetrate = 3;

        }

        public override void AI()
        {
            Vector2 dustPosition = projectile.Center + new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));
            //Making player variable "player" set as the projectile's owner
            Player player = Main.player[projectile.owner];
            if (projectile.ai[0] == 0)
            {
                projectile.tileCollide = false;

                float focusX = player.Center.X;
                float focusY = player.Center.Y;
                double deg = (double)projectile.ai[1] * 2.4; //Speed
                double rad = deg * (Math.PI / 180); //Convert degrees to radians
                double dist = 64; //Distance away from the player
                projectile.position.X = focusX - (int)(Math.Cos(rad + 1.5) * dist) - projectile.width / 2;
                projectile.position.Y = focusY - (int)(Math.Sin(rad + 1.5) * dist) - projectile.height / 2;
                //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
                projectile.ai[1] += 1f;
            }
            else
            {
                if (projectile.ai[0] == 1)
                {
                    //Find cursor and shoot at
                    float maxDistance = 15f; // Speed
                    Vector2 vectorToCursor = Main.MouseWorld - projectile.Center;
                    float distanceToCursor = vectorToCursor.Length();
                    if (distanceToCursor > maxDistance)
                    {
                        distanceToCursor = maxDistance / distanceToCursor;
                        vectorToCursor *= distanceToCursor;
                    }
                    int velocityXBy1000 = (int)(vectorToCursor.X * 1000f);
                    int oldVelocityXBy1000 = (int)(projectile.velocity.X * 1000f);
                    int velocityYBy1000 = (int)(vectorToCursor.Y * 1000f);
                    int oldVelocityYBy1000 = (int)(projectile.velocity.Y * 1000f);
                    // Client Sync
                    if (velocityXBy1000 != oldVelocityXBy1000 || velocityYBy1000 != oldVelocityYBy1000)
                    {
                        projectile.netUpdate = true;
                    }
                    projectile.velocity = vectorToCursor;
                    projectile.penetrate = 1;
                    projectile.timeLeft = 400;
                    projectile.ai[0]++;
                }
                if (projectile.timeLeft == 390)
                    projectile.tileCollide = true;
                projectile.aiStyle = 0;
            }
            Dust.NewDustPerfect(dustPosition, 32, null, 100, default(Color), 0.8f);
        }
    }
}
