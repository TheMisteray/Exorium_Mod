using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ReLogic.Content;

namespace ExoriumMod.Core.PlayerDrawEffects
{
    internal class MiscEffectsFront : PlayerDrawLayer
    {
        private Asset<Texture2D> shieldTexture;

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return drawInfo.drawPlayer.GetModPlayer<ExoriumPlayer>().shield;
        }

        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.HandOnAcc);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.drawPlayer.GetModPlayer<ExoriumPlayer>().shield)
            {
                var position = drawInfo.Center - Main.screenPosition;
                position = new Vector2((int)position.X, (int)position.Y); // You'll sometimes want to do this, to avoid quivering.

                if (shieldTexture == null)
                    shieldTexture = ModContent.Request<Texture2D>(AssetDirectory.Effect + "Shield");

                // Queues a drawing of a sprite. Do not use SpriteBatch in drawlayers!
                drawInfo.DrawDataCache.Add(new DrawData(
                    shieldTexture.Value, // The texture to render.
                    position, // Position to render at.
                    null, // Source rectangle.
                    new Color(40, 140, 250), // Color.
                    0f, // Rotation.
                    shieldTexture.Size() * 0.5f, // Origin. Uses the texture's center.
                    2f, // Scale.
                    SpriteEffects.None, // SpriteEffects.
                    0 // 'Layer'. This is always 0 in Terraria.
                ));
            }

        }
    }
}
