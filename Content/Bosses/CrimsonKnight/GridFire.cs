using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using ExoriumMod.Content.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    internal class GridFire : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + "FlameBreath";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fireball");
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = false;
        }

        private Vector2 destination;
        private bool check = false;
        public bool shoot = false;

        //draw stuff
        Vector2 oldPos1 = Vector2.Zero;
        Vector2 oldPos2 = Vector2.Zero;
        Vector2 oldPos3 = Vector2.Zero;
        Vector2 oldPos4 = Vector2.Zero;
        Vector2 oldPos5 = Vector2.Zero;
        Vector2 oldPos6 = Vector2.Zero;
        Vector2 oldPos7 = Vector2.Zero;

        public float direction
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public float destinationHelper
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void AI()
        {
            //Find destination
            if (!check)
            {
                //Go to edge of arena
                switch(direction)
                {
                    case 1:
                        destination = new Vector2(ExoriumWorld.FallenTowerRect.Center.X + destinationHelper, ExoriumWorld.FallenTowerRect.Center.Y - (ExoriumWorld.FallenTowerRect.Height/2 - 160));
                        break;
                    case 2:
                        destination = new Vector2(ExoriumWorld.FallenTowerRect.Center.X - (ExoriumWorld.FallenTowerRect.Width / 2 - 80), ExoriumWorld.FallenTowerRect.Center.Y + destinationHelper);
                        break;
                    case 3:
                        destination = new Vector2(ExoriumWorld.FallenTowerRect.Center.X + destinationHelper, ExoriumWorld.FallenTowerRect.Center.Y + (ExoriumWorld.FallenTowerRect.Height / 2 - 96));
                        break;
                    case 4:
                        destination = new Vector2(ExoriumWorld.FallenTowerRect.Center.X + (ExoriumWorld.FallenTowerRect.Width / 2 - 64), ExoriumWorld.FallenTowerRect.Center.Y + destinationHelper);
                        break;
                }

                check = true;
            }

            Projectile.velocity = (destination - Projectile.Center) / 90;
            if (direction == 1 || direction == 3)
                Projectile.velocity.X *= 4.5f;
            else
                Projectile.velocity.Y *= 4.5f;

            if (Projectile.velocity.Length() < 3)
            {
                Projectile.velocity.Normalize();
                Projectile.velocity *= 3;
            }

            if ((Projectile.Center - destination).Length() < 4)
            {
                Projectile.Center = destination;
                if (Main.rand.NextBool(2))
                {
                    int xmod = 0;
                    int ymod = 0;
                    switch(direction)
                    {
                        case 1:
                            ymod = 10;
                            break;
                        case 2:
                            xmod = 10;
                            break;
                        case 3:
                            ymod = -10;
                            break;
                        case 4:
                            xmod = -10;
                            break;
                    }

                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SolarFlare, xmod, ymod, 0, default, 1);
                    Main.dust[dust].noGravity = true;
                }
            }

            //Set draw positions
            if (Projectile.timeLeft % 5 == 0)
            {
                oldPos7 = oldPos6;
                oldPos6 = oldPos5;
                oldPos5 = oldPos4;
                oldPos4 = oldPos3;
                oldPos3 = oldPos2;
                oldPos2 = oldPos1;
                oldPos1 = Projectile.Center;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 600);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.CrimsonKnight + "FlameBreath", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            if ((Projectile.Center - destination).Length() > 4)
            {
                Main.EntitySpriteDraw(tex, oldPos1 - Main.screenPosition, new Rectangle(0, 0, tex.Width, tex.Height / 7), new Color(255, 255, 255, 0), 0, new Vector2(tex.Width/2, tex.Width/2), .33f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(tex, oldPos2 - Main.screenPosition, new Rectangle(0, tex.Height / 7, tex.Width, tex.Height / 7), new Color(255, 255, 255, 0), 0, new Vector2(tex.Width / 2, tex.Width / 2), .33f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(tex, oldPos3 - Main.screenPosition, new Rectangle(0, (tex.Height / 7) * 2, tex.Width, tex.Height / 7), new Color(255, 255, 255, 0), 0, new Vector2(tex.Width / 2, tex.Width / 2), .33f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(tex, oldPos4 - Main.screenPosition, new Rectangle(0, (tex.Height / 7) * 3, tex.Width, tex.Height / 7), new Color(255, 255, 255, 0), 0, new Vector2(tex.Width / 2, tex.Width / 2), .33f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(tex, oldPos5 - Main.screenPosition, new Rectangle(0, (tex.Height / 7) * 4, tex.Width, tex.Height / 7), new Color(255, 255, 255, 0), 0, new Vector2(tex.Width / 2, tex.Width / 2), .33f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(tex, oldPos6 - Main.screenPosition, new Rectangle(0, (tex.Height / 7) * 5, tex.Width, tex.Height / 7), new Color(255, 255, 255, 0), 0, new Vector2(tex.Width / 2, tex.Width / 2), .33f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(tex, oldPos7 - Main.screenPosition, new Rectangle(0, (tex.Height / 7) * 6, tex.Width, tex.Height / 7), new Color(255, 255, 255, 0), 0, new Vector2(tex.Width / 2, tex.Width / 2), .33f, SpriteEffects.None, 0);
            }
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Vector2 speed = Vector2.Zero;
            switch (direction)
            {
                case 1:
                    speed = new Vector2(0, 4);
                    break;
                case 2:
                    speed = new Vector2(4, 0);
                    break;
                case 3:
                    speed = new Vector2(0, -4);
                    break;
                case 4:
                    speed = new Vector2(-4, 0);
                    break;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, speed, ProjectileType<gridShot>(), Projectile.damage, 2, Main.myPlayer);
        }
    }

    internal class gridShot : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + "GridFireball";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fireblast");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
        }

        public override void AI()
        {
            foreach (Projectile p in Main.projectile)
            {
                if (p.type == this.Type && p.Hitbox.Intersects(Projectile.Hitbox) && p.identity != Projectile.identity)
                {
                    p.Kill();
                    p.timeLeft = 0;
                    Projectile.Kill();
                    Projectile.timeLeft = 0;

                    Vector2 midpos = Projectile.Center + ((Projectile.Center - p.Center) / 2);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), midpos, Vector2.Zero, ProjectileType<gridCollision>(), Projectile.damage, 3, Main.myPlayer);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.CrimsonKnight + "GridFireball", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), Projectile.velocity.ToRotation() + MathHelper.PiOver2, tex.Size() / 2, 1, SpriteEffects.None, 0);
            return false;
        }
    }

    internal class gridCollision : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Explosion");
        }

        public override void SetDefaults()
        {
            Projectile.width = 280;
            Projectile.height = 280;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
        }

        public override void AI()
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 rad = new Vector2(0, Main.rand.NextFloat(Projectile.width/20));
                Vector2 shootPoint = rad.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi));
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.SolarFlare, shootPoint, 1, default, 1 + Main.rand.NextFloat(-.5f, .5f));
                dust.noGravity = true;
                dust.color = new Color(184, 58, 24);
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 dist = new Vector2(projHitbox.Center.X - targetHitbox.Center.X, projHitbox.Center.Y - targetHitbox.Center.Y);

            return dist.Length() < ((projHitbox.Width/2) + (targetHitbox.Width/2));
        }
    }
}
