using ExoriumMod.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using ExoriumMod.Content.Items.Ammo;
using Terraria.Audio;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Content.Items.Weapons.Ranger
{
    class RimestoneBow : ModItem
    {
        public override string Texture => AssetDirectory.RangerWeapon + Name;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 22;
            Item.height = 40;
            Item.useTime = 33;
            Item.useAnimation = 33;
            Item.useAmmo = AmmoID.Arrow;
            Item.knockBack = 0;
            Item.value = Item.sellPrice(silver: 14); ;
            Item.rare = 1;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = 10;
            Item.noMelee = true;
            Item.shootSpeed = 7;
            Item.useStyle = 5;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(2));
            velocity = perturbedSpeed;
            if (Main.rand.NextBool(5))
            {
                velocity *= 1.3f;
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileType<RimeIcicle>(), damage, knockback, player.whoAmI, 0, 0, 0);
                velocity = perturbedSpeed;
                return false;
            }
            else
            {
                return true;
            }
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(2));
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemType<Materials.Metals.RimestoneBar>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

    class RimeIcicle : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 900;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
        }

        private const int MAX_STUCK = 5;
        private readonly Point[] _sticking = new Point[MAX_STUCK];
        private bool tilehit = false;

        public bool isStickingToTarget = false;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(isStickingToTarget);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            isStickingToTarget = reader.ReadBoolean();
        }

        public float stuckTargetWhoAmI
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void AI()
        {
            if (tilehit)
            {
                return;
            }
            else if (isStickingToTarget)
            {
                StuckAI();
                return;
            }
            else
            {
                NormalAI();
            }
            return;
        }

        public void StuckAI()
        {
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;

            Projectile.localAI[0]++;

            bool hit = Projectile.localAI[0] % 30f == 0f;

            const int aiFactor = 300;

            bool hitEffect = Projectile.localAI[0] % 30f == 0f;
            int projTargetIndex = (int)stuckTargetWhoAmI;
            if (Projectile.localAI[0] >= 60 * aiFactor || projTargetIndex < 0 || projTargetIndex >= 200)
            {
                Projectile.Kill();
            }
            else if (Main.npc[projTargetIndex].active && !Main.npc[projTargetIndex].dontTakeDamage)
            { // If the target is active and can take damage
                Projectile.Center = Main.npc[projTargetIndex].Center - Projectile.velocity * 2f;
                Projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;
                if (hitEffect)
                { //So it stays stuck
                    Main.npc[projTargetIndex].HitEffect(0, 1.0);
                }
            }
            else
            { // Otherwise, kill the projectile
                Projectile.Kill();
            }
        }

        public void NormalAI()
        {
            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 67, 0, 0);
            d.noGravity = true;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.velocity.Y += 0.09f;
            base.AI();
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 20; k++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceRod, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f);
                d.noGravity = true;
            }

            if (!isStickingToTarget)
                SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            //line from proj center out along blade
            isStickingToTarget = true;
            stuckTargetWhoAmI = target.whoAmI; // Set the target whoAmI
            Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;
            Projectile.netUpdate = true; // netUpdate
            target.AddBuff(BuffType<Buffs.RimeIcicleDamage>(), 900); // DOT

            Projectile.damage = 0; // Doesn't deal damage anymore

            UpdateStuck(target);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.tileCollide = false;
            Projectile.position += Projectile.velocity;
            Projectile.velocity = Vector2.Zero;
            Projectile.friendly = false;
            tilehit = true;
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            return false;
        }

        private void UpdateStuck(NPC target)
        {
            int currentIndex = 0;

            for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
            {
                Projectile currentProjectile = Main.projectile[i];
                if (i != Projectile.whoAmI
                    && currentProjectile.active
                    && currentProjectile.owner == Main.myPlayer
                    && currentProjectile.type == Projectile.type
                    && currentProjectile.ModProjectile is RimeIcicle proj
                    && proj.isStickingToTarget
                    && proj.stuckTargetWhoAmI == target.whoAmI)
                {

                    _sticking[currentIndex++] = new Point(i, currentProjectile.timeLeft); // Add the current projectile's index and timeleft to the point array
                    if (currentIndex >= _sticking.Length)  // If the javelin's index is bigger than or equal to the point array's length, break
                        break;
                }
            }

            // Remove the oldest if over max
            if (currentIndex >= MAX_STUCK)
            {
                int oldJavelinIndex = 0;
                // Loop our point array
                for (int i = 1; i < MAX_STUCK; i++)
                {
                    // Remove the already existing javelin if it's timeLeft value (which is the Y value in our point array) is smaller than the new javelin's timeLeft
                    if (_sticking[i].Y < _sticking[oldJavelinIndex].Y)
                    {
                        oldJavelinIndex = i; // Remember the index of the removed javelin
                    }
                }
                // Remember that the X value in our point array was equal to the index of that javelin, so it's used here to kill it.
                Main.projectile[_sticking[oldJavelinIndex].X].Kill();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.Projectile + Name).Value;

            Main.EntitySpriteDraw(tex, (Projectile.Center - Main.screenPosition), null, lightColor, Projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2), 1, SpriteEffects.None, 0);
            return false;
        }
    }
}
