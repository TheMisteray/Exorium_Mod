﻿using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using ExoriumMod.Content.Dusts;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    class CaraveneBladeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flametounge Greatsword");
        }

        public override void SetDefaults()
        {
            projectile.width = 42;
            projectile.height = 300;
            projectile.penetrate = -1;
            projectile.timeLeft = 600;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.alpha = 255;
        }

        public float SpawnBehavior
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public bool Enrage
        {
            get => projectile.ai[1] == 1f;
            set => projectile.ai[1] = value ? 1f : 0f;
        }

        public float spawnActionTimer = 600;

        public override void AI()
        {
            if (spawnActionTimer > 0)
            {
                spawnActionTimer -= 10;
                projectile.alpha -= 25;
                projectile.position.Y--;
            }
            else
            {
                projectile.velocity.Y = 14;
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Draw indicator line
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, Enrage ? 600 : 300);
        }
    }
}