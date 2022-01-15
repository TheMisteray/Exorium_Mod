using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExoriumMod.Core
{
    public static class AssetDirectory
    {
        public const string Assets = "ExoriumMod/Assets/";

        public const string Background = Assets + "Backgrounds/";
        public const string MapBackground = Background + "Map/";

        public const string Invisible = Assets + "Invisible";

        public const string Boss = Assets + "Bosses/";
        public const string Shadowmancer = Boss + "Shadowmancer/";
        public const string BlightedSlime = Boss + "BlightedSlime/";

        public const string Buff = Assets + "Buffs/";
        public const string MinionBuff = Buff + "Minions/";

        public const string Dust = Assets + "Dusts/";

        public const string Effect = Assets + "Effects/";

        public const string Gore = Assets + "Gores/";
        public const string TreeGore = Gore + "Trees/";

        public const string Items = Assets + "Items/";

        //Items -------------------------------
        public const string Accessory = Items + "Accessories/";

        public const string Ammo = Items + "Ammo/";

        public const string Armor = Items + "Armor/";

        public const string Consumable = Items + "Consumables/";
        public const string Potion = Consumable + "Potions/";
        public const string SpellScroll = Consumable + "Scrolls/";

        public const string Materials = Items + "Materials/";
        public const string Plant = Materials + "Plants/";
        public const string Metal = Materials + "Metals/";

        public const string Misc = Items + "Misc/";

        public const string Tool = Items + "Tools/";

        public const string Weapons = Items + "Weapons/";
        public const string MagicWeapon = Weapons + "Magic/";
        public const string MeleeWeapon = Weapons + "Melee/";
        public const string RangerWeapon = Weapons + "Ranger/";
        public const string SummonerWeapon = Weapons + "Summoner/";
        //Items ------------------------------

        public const string Liquid = Assets + "Liquids/";

        public const string NPCs = Assets + "NPCs/";
        public const string TownNPC = NPCs + "Town/";
        public const string EnemyNPC = NPCs + "Town/";

        public const string Projectile = Assets + "Projectiles/";
        public const string Minion = Projectile + "Minions/";

        public const string Tile = Assets + "Tiles/";
        public const string Decoration = Tile + "Decoation/";
        public const string TileItem = Tile + "Item/";
        public const string Tree = Tile + "Trees/";

        public const string Wall = Assets + "Walls/";
        public const string WallItem = Wall + "Item/";
    }
}
