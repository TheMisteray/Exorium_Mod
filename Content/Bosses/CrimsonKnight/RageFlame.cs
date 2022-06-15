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
    class RageFlame : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flames");
        }

        public override void SetDefaults()
        {
            Projectile.width = 1200;
            Projectile.height = 1200;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
        }

        private const float RANGE = 500;

        public override bool CanHitPlayer(Player target)
        {
            return (target.Center - Projectile.Center).Length() < target.width + RANGE;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 600);
        }
    }
}
