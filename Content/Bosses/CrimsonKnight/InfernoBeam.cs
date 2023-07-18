using ExoriumMod.Core;
using ExoriumMod.Helpers;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExoriumMod.Content.Bosses.CrimsonKnight
{
    internal class InfernoBeam : ModProjectile
    {
        public override string Texture => AssetDirectory.CrimsonKnight + Name;

        private const float MAX_CHARGE = 50f;
        //The distance charge particle from the player center
        private const float MOVE_DISTANCE = 60f;

        private const float LIFE_TIME = 180;

        private const float TurnResponsiveness = 0.01f;

        private const float BeamLength = 1600f;

        private const int SoundInterval = 30;

        public float LifeCounter
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        // The actual charge value is stored in the localAI0 field
        public float Charge
        {
            get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }

        public float NPCWhoAmI
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        // Are we at max charge? With c#6 you can simply use => which indicates this is a get only property
        public bool IsAtMaxCharge => Charge == MAX_CHARGE;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
        }
    }
}
