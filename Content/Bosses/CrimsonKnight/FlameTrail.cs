using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using ExoriumMod.Content.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;
using Microsoft.CodeAnalysis.FlowAnalysis;

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    class FlameTrail : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + Name;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 28;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 260;
            Projectile.tileCollide = true;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.ai[0] = 0; //false
        }

        public bool hasTouchedGround
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value ? 1f : 0f;
        }

        public override void AI()
        {
            if (!hasTouchedGround)
                Projectile.velocity.Y = 1;
            else if (Projectile.frame <= 1 && Main.rand.NextBool(60))
                Dust.NewDust(Projectile.BottomLeft, Projectile.width, 1, DustID.SolarFlare, 0, -4, 0, default, Main.rand.NextFloat(.25f, 1));

            Projectile.frameCounter++;

            //Frame loop
            if (Projectile.frameCounter >= 8)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 3;
            }

            if (Projectile.timeLeft < 30) //Fade away and become non damaging
            {
                if (Projectile.alpha > 0)
                    Projectile.alpha -= 9;
                Projectile.hostile = false;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            Vector2 tileBottom = new Vector2(Projectile.position.X + Projectile.width / 2, Projectile.position.Y + Projectile.height);
            if (!Main.tile[tileBottom.ToTileCoordinates().X, tileBottom.ToTileCoordinates().Y].IsActuated)
                return true;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac); ;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            hasTouchedGround = true;
            Projectile.velocity = Vector2.Zero;

            Projectile.position.Y += 1;
            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }
    }

    class FlamePillar : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + Name;

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 300;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 240;
            Projectile.tileCollide = true;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.alpha = 255;
        }
        
        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public bool hasTouchedGround
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value ? 1f : 0f;
        }

        public override void AI()
        {
            if (!hasTouchedGround)
                Projectile.velocity.Y = 1;

            if (Timer == 0)//Move to appropriate location
            {
                Projectile.position += new Vector2(-Projectile.width/2, -Projectile.height/2);
            }
            else if (Timer == 120)
            {
                SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
                Projectile.hostile = true;
            }
            else if (Timer > 120)
            {
                for (int i = 0; i < 3; i++)
                {
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0, -10, 0, default, 2.5f);
                    Main.dust[dust].noGravity = true;
                }
            }

            Timer++;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, 600);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Timer < 120)
            {
                Texture2D tex = Request<Texture2D>(Texture).Value;
                Main.EntitySpriteDraw(tex, Projectile.position - Main.screenPosition + (new Vector2(tex.Width / 2, tex.Height) * 3), new Rectangle(0, 0, tex.Width, tex.Height), Color.Red, 0, new Vector2(tex.Width / 2, tex.Height), ((Timer + 40) % 60) / 60 * 3, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(tex, Projectile.position - Main.screenPosition + (new Vector2(tex.Width / 2, tex.Height) * 3), new Rectangle(0, 0, tex.Width, tex.Height), Color.Red, 0, new Vector2(tex.Width / 2, tex.Height), ((Timer + 20) % 60) / 60 * 3, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(tex, Projectile.position - Main.screenPosition + (new Vector2(tex.Width / 2, tex.Height) * 3), new Rectangle(0, 0, tex.Width, tex.Height), Color.Red, 0, new Vector2(tex.Width / 2, tex.Height), (Timer % 60) / 60 * 3, SpriteEffects.None, 0);
            }
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            if (Timer >= 120) return false;
            fallThrough = false;
            Vector2 tileBottom = new Vector2(Projectile.position.X + Projectile.width / 2, Projectile.position.Y + Projectile.height);
            if (!Main.tile[tileBottom.ToTileCoordinates().X, tileBottom.ToTileCoordinates().Y].IsActuated)
                return true;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac); ;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            hasTouchedGround = true;
            Projectile.velocity = Vector2.Zero;

            Projectile.position.Y += 1;
            return false;
        }
    }

    class LargeFlamePillar : FlamePillar
    {
        public override string Texture => AssetDirectory.CrimsonKnight + "FlamePillar";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CanDistortWater[Type] = false;
            ProjectileID.Sets.CanHitPastShimmer[Type] = true;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 6400;
            base.SetStaticDefaults();
        }

        public override void AI()
        {
            base.AI();
            if (Timer == 120)
            {
                Projectile.height = 1500;
                Projectile.position = new Vector2(Projectile.position.X, Core.Systems.WorldDataSystem.FallenTowerRect.Top);
            }
            else if (Timer > 120)
            {
                for (int i = 0; i < 12; i++)
                {
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0, -10, 0, default, 2.5f);
                    Main.dust[dust].noGravity = true;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Timer < 120) //Draw another Pillar inverse
            {
                Texture2D tex = Request<Texture2D>(Texture).Value;
                Vector2 height = new Vector2(0, Projectile.height);
                Main.EntitySpriteDraw(tex, Projectile.position - Main.screenPosition + (new Vector2(tex.Width / 2, tex.Height) * 3), new Rectangle(0, 0, tex.Width, tex.Height), Color.Red, MathHelper.Pi, new Vector2(tex.Width / 2, tex.Height), ((Timer + 40) % 60) / 60 * 3, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(tex, Projectile.position - Main.screenPosition + (new Vector2(tex.Width / 2, tex.Height) * 3), new Rectangle(0, 0, tex.Width, tex.Height), Color.Red, MathHelper.Pi, new Vector2(tex.Width / 2, tex.Height), ((Timer + 20) % 60) / 60 * 3, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(tex, Projectile.position - Main.screenPosition + (new Vector2(tex.Width / 2, tex.Height) * 3), new Rectangle(0, 0, tex.Width, tex.Height), Color.Red, MathHelper.Pi, new Vector2(tex.Width / 2, tex.Height), (Timer % 60) / 60 * 3, SpriteEffects.None, 0);
            }
            return base.PreDraw(ref lightColor);
        }
    } 
}
