﻿using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Content.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using System;
using Luminance.Core.Graphics;

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    class InfernalRift : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 2000;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = false;
        }

        public float towerYMax
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public float towerYMin
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        float progressValue = 0;

        public override void AI()
        {
            if (Projectile.timeLeft == 300)
            {
                if (Main.netMode != NetmodeID.Server && !Filters.Scene["ExoriumMod:InfernalRift"].IsActive())
                {
                    Texture2D heatMap = Request<Texture2D>(AssetDirectory.ShaderMap + "HeatDistortMap", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

                    Filters.Scene.Activate("ExoriumMod:InfernalRift", Projectile.Center).GetShader().UseColor(.05f, .005f, 0).UseTargetPosition(Projectile.Center).UseImage(heatMap).UseProgress(0);
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Core.Systems.WorldDataSystem.FallenTowerRect.Top + 200), Vector2.UnitY * 60, ProjectileType<RiftIndicator>(), 0, 0);
            }

            if (Projectile.timeLeft == 220)
            {
                Projectile.hostile = true;
                SoundEngine.PlaySound(SoundID.Item60);
            }

            if (Main.netMode != NetmodeID.Server && Filters.Scene["ExoriumMod:InfernalRift"].IsActive() && Projectile.timeLeft < 220)
            {
                //Alter progress
                Filters.Scene["ExoriumMod:InfernalRift"].GetShader().UseIntensity(Main.GameUpdateCount * 0.005f).UseProgress(progressValue); //Make use game time for stoppin while paused

                if (Projectile.timeLeft > 110 && progressValue < 1)
                {
                    progressValue = MathHelper.Lerp(progressValue, 1, .1f);
                }
                else if (Projectile.timeLeft <= 31 && progressValue > 0)
                {
                    progressValue -= 1 / 30f;
                }
            }

            if (Projectile.timeLeft == 180)
            {
                //Create Spirits
                float dist = (towerYMin - towerYMax) - 128;
                int count = Main.expertMode? 16: 20;
                if (Main.masterMode) count += 2;
                float interval = dist / count;
                bool[] leftSpirits = new bool[count];
                bool[] rightSpirits = new bool[count];

                //Set where spirits will be
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (Main.rand.NextFloat() < .75f)//left
                        {
                            if (i > 1 && leftSpirits[i - 1] && leftSpirits[i - 2])
                            {
                                leftSpirits[i] = false;
                            }
                            else
                            {
                                leftSpirits[i] = true;
                                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X - 40, towerYMax + interval * i + 192), new Vector2(-1, 0), ProjectileType<RiftSpirit>(), Projectile.damage, 3, Main.myPlayer, -1);
                            }
                        }
                        else
                        {
                            leftSpirits[i] = false;
                        }
                        if (Main.rand.NextFloat() < .75f)//right
                        {
                            if (i > 1 && rightSpirits[i - 1] && rightSpirits[i - 2])
                            {
                                rightSpirits[i] = false;
                            }
                            else
                            {
                                rightSpirits[i] = true;
                                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + 40, towerYMax + interval * i + 192), new Vector2(1, 0), ProjectileType<RiftSpirit>(), Projectile.damage, 3, Main.myPlayer, 1);
                            }
                        }
                        else
                        {
                            rightSpirits[i] = false;
                        }
                    }
                }
            }
        }

        public override bool PreKill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.Server && Filters.Scene["ExoriumMod:InfernalRift"].IsActive())
            {
                Filters.Scene["ExoriumMod:InfernalRift"].Deactivate();
            }
            return base.PreKill(timeLeft);
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.SourceDamage *= 2.2f;
            base.ModifyHitPlayer(target, ref modifiers);
        }
    }

    class RiftSpirit : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + Name;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 40;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.friendly = false;
            Projectile.hostile = true;
        }

        public int direction
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            if (Projectile.velocity.X <= 12)
            {
                Projectile.velocity.X *= 1.02f;
            }

            if (++Projectile.frameCounter >= 60 / Math.Abs(Projectile.velocity.X))
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SolarFlare, Projectile.velocity.X * Main.rand.NextFloat(.25f), 0, 0, default, 1);
                Main.dust[dust].noGravity = true;
            }

            Projectile.spriteDirection = direction;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SolarFlare, Projectile.velocity.X * Main.rand.NextFloat(.25f), 0, 0, default, 1);
                Main.dust[dust].noGravity = true;
            }
        }
    }

    class RiftIndicator : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texIndicator = Request<Texture2D>(AssetDirectory.CrimsonKnight + "RayEffect").Value;
            Main.EntitySpriteDraw(texIndicator, Projectile.Center - Main.screenPosition, null, new Color(200, 0, 0, 0), Projectile.rotation, new Vector2(texIndicator.Width, texIndicator.Height) / 2, .7f, SpriteEffects.None, 0);
            return false;
        }
    }
}
