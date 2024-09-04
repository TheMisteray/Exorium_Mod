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
    internal class SwordHitbox : ModProjectile
    {
        public override string Texture => AssetDirectory.Invisible;

        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 300;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 15;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, 900);
        }
    }
}
