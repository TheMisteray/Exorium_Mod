using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using ExoriumMod.Projectiles;
using ExoriumMod.Dusts;

namespace ExoriumMod.Core
{
    partial class ExoriumPlayer
    {
        public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (shadowCloak && !deadCloak)
            {
                if (Main.rand.NextBool(4) && drawInfo.shadow == 0f)
                {
                    int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, DustType<Shadow>(), player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100, default(Color), 2f);
                    Main.dust[dust].noGravity = true;
                    Main.playerDrawDust.Add(dust);
                }
            }
            if (ritualArrow)
            {
                Texture2D tex = GetTexture(AssetDirectory.Effect + "RitualArrow");
                float scale = 1;
                float disappearRange = 240;
                float shrinkRange = 600;

                //Rotation to point to Shadow Altar, super inelegant way to do this but I couldn't find another way so far. (I'd have to get data from ExoriumWorld to do so from what I can tell)
                Vector2 shadowAltar = new Vector2(ExoriumWorld.shadowAltarCoordsX, ExoriumWorld.shadowAltarCoordsY).ToWorldCoordinates();
                Vector2 toAltar = shadowAltar - player.Center;
                float rotation = toAltar.ToRotation() - MathHelper.ToRadians(45);
                if (toAltar.Length() > disappearRange)
                {
                    if (toAltar.Length() < shrinkRange) //Shrink when close
                        scale = (Math.Abs(toAltar.Length()) - disappearRange) / (shrinkRange - disappearRange);
                    Main.spriteBatch.Draw(tex, new Vector2(player.Center.X - Main.screenPosition.X, player.Center.Y - Main.screenPosition.Y), new Rectangle(0, 0, tex.Width, tex.Height), Color.White, rotation, Vector2.Zero, scale, 0, 0);
                }

            }
        }

        public static readonly PlayerLayer MiscEffectsBack = new PlayerLayer("ExoriumMod", "MiscEffectsBack", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
        });

        public static readonly PlayerLayer MiscEffects = new PlayerLayer("ExoriumMod", "MiscEffects", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("ExoriumMod");
            ExoriumPlayer modPlayer = drawPlayer.GetModPlayer<ExoriumPlayer>();

            if (modPlayer.shield)
            {
                Texture2D tex = GetTexture(AssetDirectory.Effect + "Shield");

                DrawData data = new DrawData(tex, new Vector2(drawPlayer.Center.X - Main.screenPosition.X - tex.Width, drawPlayer.Center.Y - Main.screenPosition.Y - tex.Height), new Rectangle(0, 0, tex.Width, tex.Height), new Color(40, 140, 250), 0, Vector2.Zero, 2, 0, 0);
                Main.playerDrawData.Add(data);
            }
        });

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            MiscEffectsBack.visible = true;
            layers.Insert(0, MiscEffectsBack);
            MiscEffects.visible = true;
            layers.Add(MiscEffects);
        }
    }
}
