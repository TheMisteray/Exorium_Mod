using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Core
{

    internal class ExoriumGlobalProjectile : GlobalProjectile
    {
    }

    internal class GlobalGravestoneProjectile : GlobalProjectile
    {
        public static int[] gravestoneProjectiles = new int[] {
            ProjectileID.Tombstone,
            ProjectileID.GraveMarker,
            ProjectileID.CrossGraveMarker,
            ProjectileID.Headstone,
            ProjectileID.Gravestone,
            ProjectileID.Obelisk,
            ProjectileID.RichGravestone1, ProjectileID.RichGravestone2,ProjectileID.RichGravestone3,
            ProjectileID.RichGravestone4, ProjectileID.RichGravestone5,
        };

        public override bool PreAI(Projectile projectile)
        {
            if (gravestoneProjectiles.Contains(projectile.type))
            {
                foreach (Player player in Main.player)
                {
                    if (player.active && player.HasBuff(BuffType<Content.Buffs.NoGraves>()))
                    {
                        projectile.active = false;
                        return false;
                    }
                }
            }
            return base.PreAI(projectile);
        }
    }
}
