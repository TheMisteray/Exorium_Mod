using ExoriumMod.Core;
using ExoriumMod.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;

namespace ExoriumMod.Content.Bosses.Shadowmancer
{
    class ShadowAdd : ModNPC
    {
        public override string Texture => "Terraria/Images/NPC_" + NPCID.Wraith;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Shadow");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Wraith];
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = 22;
            NPC.lifeMax = 12;
            NPC.damage = 25;
            NPC.defense = 0;
            NPC.knockBackResist = 0f;
            NPC.width = 26;
            NPC.height = 48;
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath52;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.Frostburn] = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            AIType = NPCID.Wraith;
            AnimationType = NPCID.Wraith;
        }

        private int counter = 0;
        public float AccelX = Main.rand.NextFloat(-4, 5);

        public override void AI()
        {
            base.AI();
            counter++;
            if (counter < 90)
            {
                NPC.velocity.Y = NPC.ai[3];
                NPC.velocity.X = AccelX;
                NPC.ai[3] *= .99f;
                AccelX *= .99f;
                if (Math.Abs(NPC.velocity.Y) < 1)
                    counter = 90;
                if (Main.netMode == NetmodeID.Server)
                    NPC.netUpdate = true;
            }
        }

        public override void OnKill()
        {
            Vector2 dustSpeed = new Vector2(0, 5);
            for (int i = 0; i < 8; i++)
            {
                Vector2 perturbedDustSpeed = dustSpeed.RotatedBy(MathHelper.ToRadians(Main.rand.Next(0, 361)));
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<Shadow>(), perturbedDustSpeed.X * Main.rand.NextFloat(), perturbedDustSpeed.Y * Main.rand.NextFloat());
            }
        }

        public override bool PreAI()
        {
            if (NPC.ai[2] == -1 && Main.netMode != NetmodeID.MultiplayerClient) //Killed by collective Darkness
            {
                NPC.HitInfo hit = new NPC.HitInfo();
                hit.Damage = 333;
                hit.Crit = true;
                NPC.StrikeNPC(hit);
                if (Main.netMode != NetmodeID.SinglePlayer)
                    NetMessage.SendStrikeNPC(NPC, hit);
                NPC.active = false;
                NPC.life = -1;
            }
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 drawCenter = NPC.Center;
            drawCenter.Y += 2;
            spriteBatch.Draw(Request<Texture2D>(AssetDirectory.Shadowmancer + "ShadowGlow").Value, drawCenter - Main.screenPosition, new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height), Color.White * .4f, NPC.rotation, new Vector2(NPC.width, NPC.height)/2, 1, NPC.spriteDirection == 1? SpriteEffects.FlipHorizontally: SpriteEffects.None, 0);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
            new FlavorTextBestiaryInfoElement("This undead takes the appearance of a humanoid shadow. It feeds on the vitality of living creatures.")
            });
        }
    }
}
