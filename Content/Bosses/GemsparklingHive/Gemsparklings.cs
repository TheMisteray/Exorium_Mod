using ExoriumMod.Core;
using ExoriumMod.Helpers;
using Microsoft.Xna.Framework;
using System;
using ExoriumMod.Content.Dusts;
using ExoriumMod.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ExoriumMod.Content.Bosses.GemsparklingHive
{
    abstract class Gemsparkling : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.knockBackResist = 0;
            npc.lifeMax = 150;
            npc.HitSound = SoundID.NPCHit5;
            npc.DeathSound = SoundID.NPCDeath7;
            npc.timeLeft = NPC.activeTime * 30;
            npc.noGravity = true;
            npc.noTileCollide = true;
        }

        public float ticker
        {
            get => npc.ai[0];
            set => npc.ai[0] = value;
        }

        public float action
        {
            get => npc.ai[1];
            set => npc.ai[1] = value;
        }
        //0 - move
        //1 - moving attack
        //2 - stationary attack

        public bool retreat
        {
            get => npc.ai[2] == 1f;
            set => npc.ai[2] = value ? 1f : 0f;
        }

        public override void AI()
        {
            #region Targeting
            if (Main.netMode != 1)
            {
                npc.TargetClosest(true);
                npc.netUpdate = true;
            }

            Player player = Main.player[npc.target];
            if (!player.active || player.dead && Main.netMode != NetmodeID.MultiplayerClient || (npc.position - player.position).Length() > 3000)
            {
                npc.TargetClosest(true);
                npc.netUpdate = true;
                player = Main.player[npc.target];
                if (!player.active || player.dead || (npc.position - player.position).Length() > 3000)
                {
                    //Make this retract them instead
                    npc.velocity = new Vector2(0f, 10f);
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                    return;
                }
            }
            #endregion

            if (ticker % 120 == 0)
                MovingAttack();
            if (ticker % 300 == 0)
                StatinaryAttack();

            if (action == 1)
                MovingAttack();
            else if (action == 2)
            {
                StatinaryAttack();
                npc.velocity *= 0.96f;
                return;
            }
            else
                ticker++;

            if (ticker % 30 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                float speed = 10f;
                float inertia = 20f;

                //Movement
                if (player.active)
                {
                    Vector2 direction = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
                    direction *= speed;
                    npc.velocity = (npc.velocity * (inertia - 1) + direction) / inertia;
                }
                else
                    npc.velocity = new Vector2(0f, 10f);
                npc.netUpdate = true;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            int frameSpeed = 10;
            npc.frameCounter++;
            if (npc.frameCounter >= frameSpeed)
            {
                npc.frameCounter = 0;
                npc.frame.Y += frameHeight;
                if (npc.frame.Y >= frameHeight * 3)
                {
                    npc.frame.Y = 0;
                }
            }
        }

        public virtual void StatinaryAttack() { }

        public virtual void MovingAttack() { }
    }

    internal class AmethystGemsparkling : Gemsparkling
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetDefaults()
        {
            npc.damage = 12;
            npc.defDamage = 3;
            npc.width = 23;
            npc.height = 30;
            base.SetDefaults();
        }
    }

    internal class TopazGemsparkling : Gemsparkling
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetDefaults()
        {
            npc.damage = 10;
            npc.defDamage = 9;
            npc.width = 38;
            npc.height = 48;
            base.SetDefaults();
        }
    }

    internal class SapphireGemsparkling : Gemsparkling
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetDefaults()
        {
            npc.damage = 16;
            npc.defDamage = 7;
            npc.width = 38;
            npc.height = 32;
            base.SetDefaults();
        }
    }

    internal class EmeraldGemsparkling : Gemsparkling
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetDefaults()
        {
            npc.damage = 17;
            npc.defDamage = 2;
            npc.width = 22;
            npc.height = 40;
            base.SetDefaults();
        }
    }

    internal class RubyGemsparkling : Gemsparkling
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetDefaults()
        {
            npc.damage = 20;
            npc.defDamage = 0;
            npc.width = 22;
            npc.height = 38;
            base.SetDefaults();
        }
    }

    internal class DiamondGemsparkling : Gemsparkling
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetDefaults()
        {
            npc.damage = 22;
            npc.defDamage = 11;
            npc.width = 23;
            npc.height = 30;
            base.SetDefaults();
        }
    }

    internal class AmberGemsparkling : Gemsparkling
    {
        public override string Texture => AssetDirectory.GemsparklingHive + Name;

        public override void SetDefaults()
        {
            npc.damage = 12;
            npc.defDamage = 3;
            npc.width = 23;
            npc.height = 30;
            base.SetDefaults();
        }
    }
}