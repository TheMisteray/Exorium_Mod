using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.GameContent;
using ExoriumMod.Core;
using ExoriumMod.Helpers;
using Terraria.Audio;
using ExoriumMod.Content.Buffs.Minions;
using System;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Effects;
using static Terraria.ModLoader.ModContent;
using Mono.Cecil;

namespace ExoriumMod.Content.Items.Weapons.Summoner.Whips
{
    internal class FlameTounge : ModItem
    {
        public override string Texture => AssetDirectory.SummonerWhip + Name;

        public override void SetStaticDefaults()
        {
            //CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Tooltip.SetDefault("7 summon tag damage\n"
                + "Your summons will focus struck enemies\n" +
                "It flails of it's own accord\n" +
                "Lights targets ablaze and slightly lowers their defense\n" +
                "Your summons amplify this burn effect" + 
                "Hold to control the tounge");
        }

        public override void SetDefaults()
        {
            // Projectile method quickly sets the whip's properties.
            // Mouse over to see its parameters.
            Item.DefaultToWhip(ProjectileType<FlameToungeProjectile>(), 20, 2, 4, 40);

            Item.shootSpeed = 4;
            Item.rare = ItemRarityID.LightRed;
            Item.channel = true;
            Item.UseSound = null;
        }

        public override bool MeleePrefix()
        {
            return true;
        }
    }

    public class FlameToungeProjectile : ModProjectile
    {
        public override string Texture => AssetDirectory.WhipProjectile + Name;
        private const float AimResponsiveness = 0.1f;

        public override void SetStaticDefaults()
        {
            // Projectile makes the projectile use whip collision detection and allows flasks to be applied to it.
            ProjectileID.Sets.IsAWhip[Type] = true;
            DisplayName.SetDefault("Flame Tounge");
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();

            Projectile.WhipSettings.Segments = 30;
            Projectile.WhipSettings.RangeMultiplier = 1.2f;
            Projectile.localNPCHitCooldown = 15; //So can hit multiple times
        }

        private float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public bool Forward
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value ? 1f : 0f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;

            if (Forward) //Something else is increasing ai[0] by 1 every fram and I don't know what, adjusted values to compensate
                Projectile.ai[0] += 1f;
            else
                Projectile.ai[0] -= 3f;

            // Slow turn to cursor
            Vector2 aim = Vector2.Normalize(Main.MouseWorld - player.RotatedRelativePoint(player.MountedCenter, true));
            if (aim.HasNaNs())
            {
                aim = -Vector2.UnitY;
            }
            aim = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), aim, AimResponsiveness));
            aim *= player.HeldItem.shootSpeed;

            if (aim != Projectile.velocity)
            {
                Projectile.netUpdate = true;
            }
            Projectile.velocity = aim;
            player.ChangeDir(Projectile.direction); //Change direction if needed

            Projectile.GetWhipSettings(Projectile, out var timeToFlyOut, out var _, out var _);
            Projectile.Center = Main.GetPlayerArmPosition(Projectile) + Projectile.velocity * (Projectile.ai[0] - 1f);
            Projectile.spriteDirection = ((!(Vector2.Dot(Projectile.velocity, Vector2.UnitX) < 0f)) ? 1 : (-1));
            if (player.channel && !player.noItems && !player.CCed) 
            {
                Projectile.timeLeft = 300;
            }
            if (Projectile.ai[0] >= timeToFlyOut - 2)
            {
                Forward = false;
                Projectile.timeLeft = 10; //This will be overriden by the above next frame, this is to make it so the current swing finishes
            }
            else if (Projectile.ai[0] <= 2)
            {
                Forward = true;
                Projectile.timeLeft = 10; //This will be overriden by the above next frame, this is to make it so the current swing finishes
            }

            player.heldProj = Projectile.whoAmI;
            player.itemAnimation = player.itemAnimationMax - (int)(Projectile.ai[0] / (float)Projectile.MaxUpdates);
            player.itemTime = player.itemAnimation;
            if (Projectile.ai[0] == (float)(int)(timeToFlyOut / 2f))
            {
                Projectile.WhipPointsForCollision.Clear();
                Projectile.FillWhipControlPoints(Projectile, Projectile.WhipPointsForCollision);
                Vector2 vector = Projectile.WhipPointsForCollision[Projectile.WhipPointsForCollision.Count - 1];
                SoundEngine.PlaySound(SoundID.Item20, vector);
            }

            //Dust
            float t = Projectile.ai[0] / timeToFlyOut;
            float num = Utils.GetLerpValue(0.1f, 0.7f, t, clamped: true) * Utils.GetLerpValue(0.9f, 0.7f, t, clamped: true);
            if (!(num > 0.1f) || !(Main.rand.NextFloat() < num))
            {
                return;
            }
            Projectile.WhipPointsForCollision.Clear();
            Projectile.FillWhipControlPoints(Projectile, Projectile.WhipPointsForCollision);
            Rectangle r = Utils.CenteredRectangle(Projectile.WhipPointsForCollision[Projectile.WhipPointsForCollision.Count - 1], new Vector2(20f, 20f));
            Vector2 vector2 = Projectile.WhipPointsForCollision[Projectile.WhipPointsForCollision.Count - 2].DirectionTo(Projectile.WhipPointsForCollision[Projectile.WhipPointsForCollision.Count - 1]).SafeNormalize(Vector2.Zero);
            for (int i = 0; i < 3; i++)
            {
                if (Main.rand.Next(3) != 0)
                {
                    continue;
                }
                if (Main.rand.Next(7) == 0)
                {
                    Dust dust = Dust.NewDustDirect(r.TopLeft(), r.Width, r.Height, DustID.Torch);
                    dust.velocity.X /= 2f;
                    dust.velocity.Y /= 2f;
                    dust.velocity += vector2 * 2f;
                    dust.fadeIn = 1f + Main.rand.NextFloat() * 0.6f;
                    dust.noGravity = true;
                    continue;
                }
                Dust dust2 = Dust.NewDustDirect(r.TopLeft(), r.Width, r.Height, 6, 0f, 0f, 0, default(Color), 1.2f);
                dust2.velocity += vector2 * 2f;
                if (Main.rand.Next(3) != 0)
                {
                    dust2.fadeIn = 0.7f + Main.rand.NextFloat() * 0.9f;
                    dust2.scale = 0.6f;
                    dust2.noGravity = true;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> list = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, list);

            //Vanilla draw removed
            //Main.DrawWhip_WhipBland(Projectile, list);

            SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.instance.LoadProjectile(Type);
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Vector2 pos = list[0];

            SpriteBatch spriteBatch = Main.spriteBatch;

            for (int i = 0; i < list.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 18, 24);
                Vector2 origin = new Vector2(9, 9);
                float scale = 1;

                if (i == 0) //Handle
                {
                    frame.Y = 0;
                    frame.Height = 24;
                }
                else if (i == list.Count - 2) //Head
                {
                    frame.Y = 78;
                    frame.Height = 18;
                }
                else
                {
                    frame.Y = 24;
                    frame.Height = 16;
                }
                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2; // This projectile's sprite faces down, so PiOver2 is used to correct rotation.
                Color color = Color.White;

                if (i != 1)
                    Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);

                pos += diff;
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            target.AddBuff(BuffType<SparklingWhipTag>(), 420);
            target.AddBuff(BuffID.OnFire, 420);
        }
    }
}
