using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using ExoriumMod.Content.Dusts;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Content.Bosses.Shadowmancer
{
    class CollectiveDarkness : ModProjectile
    {
        public override string Texture => AssetDirectory.Shadowmancer + Name;

        public override void SetDefaults()
        {
            projectile.width = 90;
            projectile.height = 90;
            projectile.penetrate = -1;
            projectile.timeLeft = 480;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.alpha = 30;
        }

        private float PlayerTarget
        {
            get => projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        private float power
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (projectile.timeLeft == 450)
                {
                    Absorb(.33f); //1 in 3
                    projectile.netUpdate = true;
                }
                else if (projectile.timeLeft == 420)
                {
                    Absorb(.5f); //half remaining
                    projectile.netUpdate = true;
                }
                else if (projectile.timeLeft == 390)
                {
                    Absorb(1f); //all remaining
                    projectile.netUpdate = true;
                }

                if (projectile.timeLeft == 240)
                {
                    Vector2 trajectory = Vector2.Zero;
                    Player playerTarget = Main.player[(int)PlayerTarget];
                    trajectory = playerTarget.Center - projectile.Center;
                    float magnitude = (float)Math.Sqrt(trajectory.X * trajectory.X + trajectory.Y * trajectory.Y);
                    if (magnitude > 0)
                        trajectory *= 5f / magnitude;
                    else
                        trajectory = new Vector2(0f, 5f);

                    projectile.velocity = trajectory;
                    projectile.netUpdate = true;
                }
            }

            //Movement
            projectile.velocity *= 0.99f;
            projectile.rotation += 2;

            Resize();

            //TODO: extra effects at high power.
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.NPCDeath6, projectile.position);
            Vector2 dustSpeed = new Vector2(0, 10 * projectile.scale);
            for (int i = 0; i < (15 * (projectile.scale / 2)); i++)
            {
                Vector2 perturbedDustSpeed = dustSpeed.RotatedBy(MathHelper.ToRadians(Main.rand.Next(0, 361)));
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustType<Shadow>(), perturbedDustSpeed.X * Main.rand.NextFloat(), perturbedDustSpeed.Y * Main.rand.NextFloat());
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                #region Shrapnel
                float rotator = 0;
                float angle = 0;
                Vector2 trajectory = new Vector2(0, 7); //Speed 7 of created projectiles

                if (power >= 10) //Number of created projectiles (this is the rotation between each)
                    rotator = 20;
                else if (power >= 6)
                    rotator = 30;
                else if (power >= 4)
                    rotator = 45;
                else if (power >= 2)
                    rotator = 90;
                else
                    angle = 360; //Don't create projectiles

                while (angle < 360)
                {
                    Vector2 newTrajectory = trajectory.RotatedBy(MathHelper.ToRadians(angle)); //Rotate by angle
                    int p = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, newTrajectory.X, newTrajectory.Y, ProjectileType<CollectiveFragment>(), projectile.damage, 2);
                    Main.projectile[p].netUpdate = true;
                    angle += rotator;
                }
                #endregion

                #region Shadows
                angle = 0;
                rotator = 0;

                if (power >= 8) //Number of created projectiles (this is the rotation between each)
                    rotator = 30;
                else if (power >= 5)
                    rotator = 45;
                else if (power >= 3)
                    rotator = 90;
                else if (power >= 1)
                    rotator = 180;
                else
                    angle = 360; //Don't create projectiles

                while (angle < 360)
                {
                    int p = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, ProjectileType<RotatingShade>(), projectile.damage, 2, projectile.owner, projectile.whoAmI, angle);
                    Main.projectile[p].netUpdate = true;
                    angle += rotator;
                }
                #endregion

            }
            base.Kill(timeLeft);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 dist = new Vector2(projHitbox.Center.X - targetHitbox.Center.X, projHitbox.Center.Y - targetHitbox.Center.Y);
            return dist.Length() < (projectile.width/2 * projectile.scale) + targetHitbox.Width;
        }

        /// <summary>
        /// Destroys Shadows and Mirror entities, powering up the attack
        /// </summary>
        /// <param name="percent">Chance to destroy</param>
        private void Absorb(float percent)
        {
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCType<ShadowAdd>() && Main.rand.NextFloat(1) <= percent)
                {
                    Main.npc[i].ai[2] = -1;
                    Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, 0, 0, ProjectileType<AbsorbedShadow>(), 0, 2, projectile.owner, projectile.whoAmI, 1);
                    //power++ created proj increases power by 1
                }
                else if (Main.npc[i].active && Main.npc[i].type == NPCType<MirrorEntity>() && Main.rand.NextFloat(1) <= percent)
                {
                    Main.npc[i].ai[2] = -1;
                    Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, 0, 0, ProjectileType<AbsorbedShadow>(), 0, 2,  projectile.owner, projectile.whoAmI, 2);
                    //power += created proj increases power by 2
                }
            }
        }

        /// <summary>
        /// Resize the projectile while keeping position
        /// Why does it work like this
        /// </summary>
        private void Resize()
        {
            projectile.position = projectile.Center;
            projectile.scale = 1 + (power * 0.25f);
            projectile.Center = projectile.position;
            if (Main.netMode != NetmodeID.MultiplayerClient)
                projectile.netUpdate = true;
        }
    }

    internal class CollectiveFragment : ModProjectile
    {
        public override string Texture => AssetDirectory.Shadowmancer + Name;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadow Bolt");
        }

        public override void SetDefaults()
        {
            //TODO: set width and height accordingly
            projectile.width = 30;
            projectile.height = 30;
            projectile.penetrate = -1;
            projectile.timeLeft = 420;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.alpha = 30;
        }

        public override void AI()
        {
            projectile.rotation += .1f;
            Vector2 delta = projectile.position - new Vector2(projectile.position.X + Main.rand.NextFloat(-1, 2), projectile.position.Y + Main.rand.NextFloat(-1, 2));
            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Shadow>(), delta.X, delta.Y);
            }
            if (Main.rand.NextBool(5))
            {
                int dust0 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Rainbow>(), delta.X, delta.Y);
                Main.dust[dust0].color = new Color(200, 0, 0);
            }
        }

        public override void Kill(int timeLeft)
        {
            Vector2 dustSpeed = new Vector2(0, 5);
            for (int i = 0; i < 10; i++)
            {
                Vector2 perturbedDustSpeed = dustSpeed.RotatedBy(MathHelper.ToRadians(Main.rand.Next(0, 361)));
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustType<Shadow>(), perturbedDustSpeed.X * Main.rand.NextFloat(), perturbedDustSpeed.Y * Main.rand.NextFloat());
            }
            base.Kill(timeLeft);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = GetTexture(AssetDirectory.Shadowmancer + Name + "_aGlow");
            Main.spriteBatch.Draw(tex, (projectile.Center - Main.screenPosition), null, new Color(40, 0, 0, 0), projectile.velocity.ToRotation(), new Vector2(tex.Width / 2, tex.Height / 2), 2, SpriteEffects.None, 0f);
            return true;
        }
    }

    internal class RotatingShade : ModProjectile
    {
        public override string Texture => "Terraria/NPC_82";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadow");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 26;
            projectile.height = 48;
            projectile.penetrate = -1;
            projectile.timeLeft = 90;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.alpha = 30;
        }

        private float rotation = 0;
        private float angle = 0;
        private Vector2 offset = Vector2.Zero;

        //Hold position of projectile
        private float darkX;
        private float darkY;

        public override void SendExtraAI(System.IO.BinaryWriter writer)
        {
            writer.Write((double)darkX);
            writer.Write((double)darkY);
        }

        public override void ReceiveExtraAI(System.IO.BinaryReader reader)
        {
            darkX = (float)reader.ReadDouble();
            darkY = (float)reader.ReadDouble();
        }

        /// <summary>
        /// array index of dark projectile
        /// </summary>
        private float darkWhoAmI
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        /// <summary>
        /// affects direction the projectile circles in
        /// </summary>
        private float petalAngle
        {
            get => projectile.ai[1] + 45;
        }

        public override void AI()
        {
            if (projectile.timeLeft == 90 && Main.netMode != NetmodeID.MultiplayerClient) // Just spawned
            {
                darkX = Main.projectile[(int)darkWhoAmI].Center.X;
                darkY = Main.projectile[(int)darkWhoAmI].Center.Y;
                projectile.netUpdate = true;
            }
            Vector2 delta = projectile.position - new Vector2(projectile.position.X + Main.rand.NextFloat(-1, 2), projectile.position.Y + Main.rand.NextFloat(-1, 2));
            if (Main.rand.NextBool(6))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Shadow>(), delta.X, delta.Y);
            }

            // Loop frames
            int frameSpeed = 5;
            projectile.frameCounter++;
            if (projectile.frameCounter >= frameSpeed)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }

            //Rotation - Lean in direction currently moving - face direction currently facing
            if (petalAngle > 90 && petalAngle < 270)
            {
                if (angle < 45)
                    projectile.direction = 1;
                else
                    projectile.direction = -1;
            }
            else
            {
                if (angle > 45)
                    projectile.direction = -1;
                else 
                    projectile.direction = 1;
            }
            rotation += projectile.direction;
            if (rotation > 10)
                rotation = 10;
            else if (rotation < -10)
                rotation = -10;
            projectile.rotation = MathHelper.ToRadians(rotation);

            //Positioning
            //Posiitoning - petal from Dark
            float r = 180 * (float)Math.Sin(2 * MathHelper.ToRadians(angle)); //max distance 10 4 petal shape
            Vector2 position = new Vector2(darkX, darkY); //Position of dark
            offset = new Vector2((float)(r * Math.Cos(MathHelper.ToRadians(angle))), (float)(r * Math.Sin(MathHelper.ToRadians(angle))));
            Vector2 rotatedOffset = offset.RotatedBy(MathHelper.ToRadians(petalAngle));
            angle ++;
            projectile.Center = position + rotatedOffset;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.NPCDeath6, projectile.position);
            Vector2 dustSpeed = new Vector2(0, 5);
            for (int i = 0; i < 15; i++)
            {
                Vector2 perturbedDustSpeed = dustSpeed.RotatedBy(MathHelper.ToRadians(Main.rand.Next(0, 361)));
                Dust.NewDust(projectile.position, projectile.width, projectile.height, DustType<Shadow>(), perturbedDustSpeed.X * Main.rand.NextFloat(), perturbedDustSpeed.Y * Main.rand.NextFloat());
            }
            //Bump in randomized direction
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int npc = NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y, NPCType<ShadowAdd>());
                Main.npc[npc].ai[3] = Main.rand.Next(-7, 3);
                Main.npc[npc].netUpdate = true;
            }
            base.Kill(timeLeft);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = GetTexture(AssetDirectory.Shadowmancer + "CollectiveFragment" + "_aGlow");
            Main.spriteBatch.Draw(tex, (projectile.Center - Main.screenPosition), null, new Color(40, 0, 0, 0), projectile.velocity.ToRotation(), new Vector2(tex.Width / 2, tex.Height / 2), 2, SpriteEffects.None, 0f);
            return true;
        }
    }

    internal class AbsorbedShadow : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darkness");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.penetrate = -1;
            projectile.timeLeft = 480;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.alpha = 255;
        }

        private double distance = 0;
        private float angle = 0;
        private float fraction = 0;

        /// <summary>
        /// array index of dark projectile
        /// </summary>
        private float darkWhoAmI
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        /// <summary>
        /// X coord of consuming dark
        /// </summary>
        private float darkX
        {
            get => Main.projectile[(int)darkWhoAmI].Center.X;
        }

        /// <summary>
        /// y coord of consuming dark
        /// </summary>
        private float darkY
        {
            get => Main.projectile[(int)darkWhoAmI].Center.Y;
        }

        /// <summary>
        /// amount of power to increase by
        /// </summary>
        private float powerIncrease
        {
            get => projectile.ai[1];
        }

        public override void AI()
        {
            //Setup values
            Projectile darkOwner = Main.projectile[(int)darkWhoAmI];
            if (projectile.timeLeft == 480) 
            {
                if (Main.netMode == NetmodeID.Server)
                    projectile.netUpdate = true;
                Vector2 darkToThis = darkOwner.Center - projectile.Center;
                angle = (float)Math.Atan2(darkToThis.Y, darkToThis.X);
                angle += (float)Math.PI;
                distance = Math.Sqrt(Math.Pow(projectile.Center.X - darkOwner.Center.X,2) + Math.Pow(projectile.Center.Y - darkOwner.Center.Y,2)); //r for purposes of spiral
                fraction = (float)distance / 120f;
            }

            //Posiitoning - Spin around Dark
            Vector2 position = new Vector2(darkX, darkY); //Position of dark
            position.X += (float)(distance * Math.Cos(angle));
            position.Y += (float)(distance * Math.Sin(angle));
            distance -= fraction;
            angle -= MathHelper.ToRadians(2);
            projectile.Center = position;

            //Kill
            if (distance < 0)
                projectile.timeLeft = 0;

            Vector2 delta = projectile.position - new Vector2(projectile.position.X + Main.rand.NextFloat(-1, 2), projectile.position.Y + Main.rand.NextFloat(-1, 2));
            //Dust
            if (Main.rand.NextBool(5))
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Shadow>(), delta.X, delta.Y);
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustType<Rainbow>(), delta.X, delta.Y, 0, new Color(200, 0, 0), .5f);
            }
        }

        public override void Kill(int timeLeft)
        {
            //Increase power
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Main.projectile[(int)darkWhoAmI].ai[0] += powerIncrease;
                Main.projectile[(int)darkWhoAmI].netUpdate = true;
            }
            base.Kill(timeLeft);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = GetTexture(AssetDirectory.Shadowmancer + "CollectiveFragment" + "_aGlow");
            Main.spriteBatch.Draw(tex, (projectile.Center - Main.screenPosition), null, new Color(40, 0, 0, 0), projectile.velocity.ToRotation(), new Vector2(tex.Width / 2, tex.Height / 2), 1f, SpriteEffects.None, 0f);
            return true;
        }
    }
}
