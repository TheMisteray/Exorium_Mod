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
            Projectile.width = 220;
            Projectile.height = 400;
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

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.CrimsonKnight + "Caravene_Hitbox").Value;
            Main.spriteBatch.Draw(tex,
            new Rectangle((int)(Projectile.position.X - Main.screenPosition.X), (int)(Projectile.position.Y - Main.screenPosition.Y), 0, 0),
            new Rectangle(0, 0, Projectile.width, Projectile.height),
            Color.Red,
            0,
            Vector2.Zero,
            SpriteEffects.FlipHorizontally,
            0);
            return base.PreDraw(ref lightColor);
        }
    }
}
