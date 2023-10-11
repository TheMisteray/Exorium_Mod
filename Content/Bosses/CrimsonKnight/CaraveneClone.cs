using ExoriumMod.Core;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Core.Utilities;
using ExoriumMod.Content.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.ModLoader.UI.ModBrowser;

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    internal class CaraveneClone : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + "Caravene";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Image of Red");
        }

        public override void SetDefaults()
        {
            Projectile.width = 140;
            Projectile.height = 240;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
        }

        public bool left
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value ? 1f : 0f;
        }

        private int frameX;

        public override void AI()
        {
            if (Projectile.timeLeft == 260)
            {
                //Swing

            }
            else if (Projectile.timeLeft == 240)
            {
                //Might move to animation calls
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2((Projectile.width * (left ? 1 : -1)), 80), Vector2.Zero, ProjectileType<SwordHitbox>(), Main.expertMode? Projectile.damage/2 : Projectile.damage, 7, Main.myPlayer);
            }
            else if (Projectile.timeLeft < 180 && Projectile.alpha > 0)
            {
                Projectile.alpha-=3;

                if (Projectile.alpha <= 0)
                    Projectile.hostile = false;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(AssetDirectory.CrimsonKnight + Name).Value;

            int ySourceHeight = (int)(Projectile.frameCounter / 10) * 442;
            int xSourceHeight = (int)(frameX * 412);

            if (!left)
            {
                Main.EntitySpriteDraw(tex,
                    Projectile.position,
                    new Rectangle(xSourceHeight, ySourceHeight, 412, 442),
                    new Color(lightColor.R, lightColor.G, lightColor.B, Projectile.alpha),
                    Projectile.rotation,
                    Vector2.Zero,
                    1,
                    SpriteEffects.None,
                    0);
            }
            else
            {
                Main.EntitySpriteDraw(tex,
                    Projectile.position,
                    new Rectangle(xSourceHeight, ySourceHeight, 412, 442),
                    new Color(lightColor.R, lightColor.G, lightColor.B, Projectile.alpha),
                    Projectile.rotation,
                    Vector2.Zero,
                    1,
                    SpriteEffects.FlipHorizontally,
                    0);
            }
            return false;
        }
    }
}
