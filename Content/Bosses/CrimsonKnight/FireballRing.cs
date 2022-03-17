using ExoriumMod.Core;
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
    class FireballRing : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fireball");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.penetrate = -1;
            projectile.timeLeft = 1200;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = true;
        }

        private const float WIDTH = 600;
        private const float HEIGHT = 50;
        Vector2 spawnAxis = Vector2.Zero;

        public float RotationOffset
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public bool Enrage
        {
            get => projectile.ai[1] == 1f;
            set => projectile.ai[1] = value ? 1f : 0f;
        }

        public override void AI()
        {
            if (projectile.timeLeft == 1200)
                spawnAxis = projectile.position;

            spawnAxis.Y += 5;
            Vector2 offsetAxel = new Vector2(WIDTH * (float)Math.Cos(RotationOffset), HEIGHT * (float)Math.Cos(RotationOffset));
            projectile.position = spawnAxis + offsetAxel;

            projectile.rotation += .02f;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, Enrage ? 600 : 300);
        }
    }
}
