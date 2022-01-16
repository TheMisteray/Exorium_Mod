using ExoriumMod.Core;
using Terraria;
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
        }

        public override void SetDefaults()
        {
            item.damage = 19;
            item.melee = true;
            item.width = 10;
            item.height = 10;
            item.useTime = 28;
            item.useAnimation = 28;
            item.useStyle = 1;
            item.knockBack = 3;
            item.value = 2000;
            item.rare = 1;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (!target.boss && target.life <= ((Main.expertMode) ? 100 : 50))
            {
                target.life = 1;
                target.checkDead();
                target.StrikeNPC(Main.expertMode ? 100 : 50, 0, 0, true);
                player.HealEffect(Main.expertMode ? 2 : 1, true);
                Main.PlaySound(SoundID.Item20, player.position);
            }
        }
    }
}
