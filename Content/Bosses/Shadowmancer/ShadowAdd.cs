using ExoriumMod.Core;
using ExoriumMod.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ExoriumMod.Content.Bosses.Shadowmancer
{
    class ShadowAdd : ModNPC
    {
        public override string Texture => "Terraria/NPC_82";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadow");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Wraith];
        }

        public override void SetDefaults()
        {
            npc.aiStyle = 22;
            npc.lifeMax = 12;
            npc.damage = 25;
            npc.defense = 0;
            npc.knockBackResist = 0f;
            npc.width = 26;
            npc.height = 48;
            npc.lavaImmune = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath52;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Frostburn] = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            aiType = NPCID.Wraith;
            animationType = NPCID.Wraith;
        }

        private int counter = 0;
        public float AccelX = Main.rand.NextFloat(-4, 5);

        public override void AI()
        {
            base.AI();
            counter++;
            if (counter < 90)
            {
                npc.velocity.Y = npc.ai[3];
                npc.velocity.X = AccelX;
                npc.ai[3] *= .99f;
                AccelX *= .99f;
                if (Math.Abs(npc.velocity.Y) < 1)
                    counter = 90;
                if (Main.netMode == NetmodeID.Server)
                    npc.netUpdate = true;
            }
        }

        public override void NPCLoot()
        {
            Vector2 dustSpeed = new Vector2(0, 5);
            for (int i = 0; i < 8; i++)
            {
                Vector2 perturbedDustSpeed = dustSpeed.RotatedBy(MathHelper.ToRadians(Main.rand.Next(0, 361)));
                Dust.NewDust(npc.position, npc.width, npc.height, DustType<Shadow>(), perturbedDustSpeed.X * Main.rand.NextFloat(), perturbedDustSpeed.Y * Main.rand.NextFloat());
            }
        }

        public override bool PreAI()
        {
            if (npc.ai[2] == -1 && Main.netMode != NetmodeID.MultiplayerClient) //Killed by collective Darkness
            {
                npc.StrikeNPC(333, 0, 0, true);
                if (Main.netMode != NetmodeID.SinglePlayer)
                    NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, npc.whoAmI, 333, 0, 0, 1);
                npc.active = false;
                npc.life = -1;
            }
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Vector2 drawCenter = npc.Center;
            drawCenter.Y += 2;
            spriteBatch.Draw(GetTexture(AssetDirectory.Shadowmancer + "ShadowGlow"), drawCenter - Main.screenPosition, new Rectangle(0, npc.frame.Y, npc.width, npc.height), Color.White * .4f, npc.rotation, new Vector2(npc.width, npc.height)/2, 1, npc.spriteDirection == 1? SpriteEffects.FlipHorizontally: SpriteEffects.None, 0);
        }
    }
}
