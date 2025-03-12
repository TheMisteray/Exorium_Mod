using ExoriumMod.Content.Dusts;
using ExoriumMod.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    internal class CrimsonSlash : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + "SlashIndicator";

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 350;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.alpha = 0;
        }

        public float loopCounter = 0;

        public float Target
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public float chosenRoation
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void AI()
        {
            loopCounter++;
            if (loopCounter >= 20)
                loopCounter = 0;

            Projectile.scale = 1 + (loopCounter * .01f);

            if (Projectile.timeLeft > 231)
            {
                Player p = Main.player[(int)Target];
                Vector2 trajectory = Main.player[(int)Target].Center - Projectile.Center;
                if (trajectory.Length() == 0)
                {
                    trajectory = new Vector2(0, 1);
                }
                trajectory.Normalize();
                trajectory *= 16;
                Projectile.velocity = trajectory;

                if ((Projectile.Center - p.Center).Length() <= 18) //skip to next ai step once near the player
                    Projectile.timeLeft = 231;

                for (int i = 0; i < 2; i++)
                {
                    int dust = Dust.NewDust(Projectile.Center, 16, 16, DustType<Rainbow>(), 0, 0, 0, default, .75f - Main.rand.NextFloat(.25f));
                    Main.dust[dust].color = Color.Red;
                    Main.dust[dust].rotation = Main.rand.NextFloat(MathHelper.Pi * 2);
                }
            }
            else if (Projectile.timeLeft == 230)
            {
                Projectile.rotation = chosenRoation;
                Projectile.velocity = Vector2.Zero;
            }
            else if (Projectile.timeLeft == 1)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 offset = new Vector2(140, 0);
                    offset = offset.RotatedBy(chosenRoation);
                    Vector2 originalOffset = offset;
                    offset.Normalize();
                    Vector2 normalOffset = offset;
                    int projectile = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + originalOffset * 2, -normalOffset * 18, ProjectileType<CrimsonSlashProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    Main.projectile[projectile].rotation = -normalOffset.ToRotation();
                    int projectile2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - originalOffset * 2, normalOffset * 18, ProjectileType<CrimsonSlashProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    Main.projectile[projectile2].rotation = normalOffset.ToRotation();
                }
            }
            else if (Projectile.timeLeft > (Main.expertMode ? 45 : 90))
            {
                Projectile.Center = Main.player[(int)Target].Center;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft <= 230) //manual draw for proper negative space
            {
                Texture2D texIndicator = Request<Texture2D>(AssetDirectory.CrimsonKnight + "SlashIndicator").Value;
                Main.EntitySpriteDraw(texIndicator, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), Projectile.rotation, new Vector2(texIndicator.Width, texIndicator.Height) / 2, 1 + (0.04f * loopCounter), SpriteEffects.None, 0);
            }
            return false;
        }
    }

    internal class CrimsonSlashProjectile : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + "CrimsonSlash";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 0;
            ProjectileID.Sets.TrailCacheLength[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.alpha = 0;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            base.AI();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(Texture).Value;
            //Afterimages
            for (int k = Projectile.oldPos.Length - 1; k >= 0; k--)
            {
                Vector2 pos = Projectile.oldPos[k];

                Main.EntitySpriteDraw(tex, pos - Main.screenPosition + new Vector2(tex.Width/2, tex.Height/2), new Rectangle(0,0,tex.Width,tex.Height), new Color(255 / (k + 1), 255 / (k + 1), 255 / (k + 1), 255 / (k + 1)), Projectile.rotation, new Vector2(-tex.Width / 2, tex.Height / 2), Projectile.scale, SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(tex, Projectile.position - Main.screenPosition + new Vector2(tex.Width / 2, tex.Height / 2), new Rectangle(0, 0, tex.Width, tex.Height), Color.White, Projectile.rotation, new Vector2(-tex.Width / 2, tex.Height / 2), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
