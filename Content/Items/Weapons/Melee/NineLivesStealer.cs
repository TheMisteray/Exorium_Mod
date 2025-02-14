using ExoriumMod.Core;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExoriumMod.Content.Items.Weapons.Melee
{
    class NineLivesStealer : ModItem
    {
        public override string Texture => AssetDirectory.MeleeWeapon + Name;

        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Instantly kills non-boss creatures with 50 hp or less (100 in expert mode, 150 in master mode) \n" +
                "Heals the player whenever something is killed this way"); */
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 19;
            Item.DamageType = DamageClass.Melee;
            Item.width = 50;
            Item.height = 50;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useStyle = 1;
            Item.knockBack = 3;
            Item.value = 2000;
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            int threshold = 50;
            if (Main.expertMode) threshold = 100;
            if (Main.masterMode) threshold = 150;
            if (!target.boss && target.life <= threshold)
            {
                target.life = 1;
                target.checkDead();
                hit.Damage = threshold;
                hit.Knockback = 0;
                hit.Crit = false;
                target.StrikeNPC(hit, true);
                NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, target.whoAmI, threshold, 0, 0, 1);
                player.HealEffect(10, true);
                SoundEngine.PlaySound(SoundID.Item20, player.position);
            }
        }
    }
}
