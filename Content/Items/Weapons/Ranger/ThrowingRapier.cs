using ExoriumMod.Core;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;

namespace ExoriumMod.Content.Items.Weapons.Ranger
{
    class ThrowingRapier : ModItem
    {
        public override string Texture => AssetDirectory.RangerWeapon + Name;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("\"Anything works if you roll a 20\"\n" +
                "If the tip of the rapier hits the enemy it will stick\n" +
                "Otherwise it will bounce");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 250;
        }

        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 28;
            Item.useTime = 28;
            Item.shootSpeed = 17f;
            Item.knockBack = 1f;
            Item.width = 48;
            Item.height = 48;
            Item.scale = 1f;
            Item.rare = 0;
            Item.value = Item.sellPrice(silver: 4);
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ProjectileType<ThrowingRapierProj>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float throwSpeed = Main.rand.NextFloat(.8f, 1.2f);
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X * throwSpeed, velocity.Y * throwSpeed, ProjectileType<ThrowingRapierProj>(), damage, knockback, player.whoAmI, Main.rand.NextFloat(.1f, .8f));
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(250);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 10);
            recipe.AddIngredient(ItemID.FallenStar);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

    class ThrowingRapierProj : ModProjectile
    {
        public override string Texture => AssetDirectory.Projectile + Name;

        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 62;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 600;
        }

        private const int FALL_TIME = 580;
        private const int MAX_STUCK = 3;
        private readonly Point[] _sticking = new Point[MAX_STUCK];

        public float rotationSpeed
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public float stuckTargetWhoAmI
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public bool IsStickingToTarget = false;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(IsStickingToTarget);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            IsStickingToTarget = reader.ReadBoolean();
        }

        public override void AI()
        {
            if (IsStickingToTarget)
                StuckAI();
            else
                NormalAI();
        }

        public void StuckAI()
        {
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;

            Projectile.localAI[0]++;

            bool hit = Projectile.localAI[0] % 30f == 0f;

            const int aiFactor = 300;

            // Every 30 ticks, the javelin will perform a hit effect
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
            if (Projectile.timeLeft <= FALL_TIME)
            {
                const float velXmult = 0.98f;
                const float velYmult = 0.35f;
                Projectile.velocity.X *= velXmult;
                Projectile.velocity.Y += velYmult;
            }
            Projectile.rotation += (float)rotationSpeed;
        }

        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 6; k++)
            {
                int dust = Dust.NewDust(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustID.Lead, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f);
            }
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            //line from proj center out along blade
            Vector2 line = new Vector2(0, Projectile.width);
            Vector2 rotatedLine = line.RotatedBy(Projectile.rotation - MathHelper.PiOver2);
            if (Collision.CheckAABBvLineCollision(target.position, new Vector2(target.width, target.height), Projectile.Center, Projectile.Center + rotatedLine))
            {
                IsStickingToTarget = true;
                stuckTargetWhoAmI = target.whoAmI; // Set the target whoAmI
                Projectile.velocity =
                    (target.Center - Projectile.Center) *
                    0.75f; // Change velocity based on delta center of targets (difference between entity centers)
                Projectile.netUpdate = true; // netUpdate
                target.AddBuff(BuffType<Buffs.ThrowingRapierDamage>(), 900); // DOT

                Projectile.damage = 0; // Doesn't deal damage anymore

                UpdateStuck(target);
            }
            else
            {
                Projectile.velocity *= -1;
                Vector2 wildBounce = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-20, 20)));
                Projectile.velocity = wildBounce;
                rotationSpeed *= Main.rand.NextFloat(.7f, 1.3f);
            }
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
                    && currentProjectile.ModProjectile is ThrowingRapierProj proj 
                    && proj.IsStickingToTarget 
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

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = Projectile.width / 2;
            height = Projectile.height / 2;
            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.Projectile + Name).Value;

            Main.EntitySpriteDraw(tex, (Projectile.Center - Main.screenPosition), null, lightColor, Projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2), 1, SpriteEffects.None, 0);
            return false;
        }
    }
}
