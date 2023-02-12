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
            Tooltip.SetDefault("Instantly kills non-boss creatures with 50 hp or less (100 in expert mode) \n" +
                "Heals the player whenever something is killed this way");
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

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (!target.boss && target.life <= ((Main.expertMode) ? 100 : 50))
            {
                target.life = 1;
                target.checkDead();
                target.StrikeNPC(Main.expertMode ? 100 : 50, 0, 0, true);
                NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, target.whoAmI, Main.expertMode ? 100 : 50, 0, 0, 1);
                player.HealEffect(10, true);
                SoundEngine.PlaySound(SoundID.Item20, player.position);
            }
        }
    }
}
