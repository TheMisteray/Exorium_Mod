using ExoriumMod.Core;
using ExoriumMod.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using System;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using ExoriumMod.Content.Dusts;
using ReLogic.Content;

namespace ExoriumMod.Content.Items.Weapons.Melee
{
    class RimestoneBlade : ModItem
    {
        public override string Texture => AssetDirectory.MeleeWeapon + Name;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 11;
            Item.DamageType = DamageClass.Melee;
            Item.width = 34;
            Item.height = 34;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = 1;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(silver: 14);
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.scale = 1.5f;
            Item.useTurn = true;
            Item.shoot = ProjectileType<RimeBladeProj>();
            Item.shootSpeed = 10;
        }

        private int frost = 0;

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if (frost < 3)
                    return false;
                Item.noMelee = true;
                Item.useStyle = ItemUseStyleID.Shoot;
            }
            else
            {
                Item.noMelee = false;
                Item.useStyle = ItemUseStyleID.Swing;
            }
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2 && frost/3 > 0)
            {
                int proj1 = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, Mod.Find<ModProjectile>("RimeBladeProj").Type, (int)(damage * (frost/3) * 1.5), knockback, player.whoAmI, frost/3);
                Main.projectile[proj1].position = Main.projectile[proj1].Center;
                Main.projectile[proj1].width *= (frost / 3);
                Main.projectile[proj1].height *= (frost / 3);
                Main.projectile[proj1].Center = Main.projectile[proj1].position;
                Main.projectile[proj1].penetrate = frost / 3;
                frost = 0;
            }
            return false;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 200, true);

            if (frost <= 15 && target.type != NPCID.TargetDummy) //exclude target dummies
            {
                frost++;
            }
            if(frost % 3 == 0)
            {
                switch (frost / 3)
                {
                    case 1:
                        CombatText.NewText(player.Hitbox, Color.LightBlue, "1", true);
                        break;
                    case 2:
                        CombatText.NewText(player.Hitbox, Color.LightBlue, "2", true);
                        break;
                    case 3:
                        CombatText.NewText(player.Hitbox, Color.LightBlue, "3", true);
                        break;
                    case 4:
                        CombatText.NewText(player.Hitbox, Color.LightBlue, "4", true);
                        break;
                    case 5:
                        CombatText.NewText(player.Hitbox, Color.LightBlue, "5", true);
                        break;
                }

                DustHelper.DustLine(player.Center, target.Center, DustType<Rainbow>(), 20, 1, 0, 0, 0, Color.LightBlue);
            }
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(0, 11) == 1)
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 67, 0f, 0f, 50, default(Color), 1);
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.RimestoneBar>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

    class RimeBladeProj : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            Projectile.timeLeft = 300;
            Projectile.height = 16;
            Projectile.width = 16;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            DustHelper.DustCircle(Projectile.Center, DustType<Dusts.Rainbow>(), Projectile.width / 2, (float)Math.Pow(Projectile.ai[0], 2), 1, 0, 0, 0, Color.LightBlue, false);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 200 * (int)Projectile.ai[0], true);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i <= Projectile.ai[0] * 10; i++)
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 67, 0, 0);
        }
    }
}
