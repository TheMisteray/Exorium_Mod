using Terraria;
using Terraria.ModLoader;
using System.IO;
using Terraria.Graphics.Effects;
using ExoriumMod.Content.Skies;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Graphics;
using Terraria.GameContent.Shaders;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace ExoriumMod.Core.Systems
{
    public class RecipeSystem : ModSystem
    {
        public override void AddRecipeGroups()
        {
            RecipeGroup woodGroup = RecipeGroup.recipeGroups[RecipeGroup.recipeGroupIDs["Wood"]];
            woodGroup.ValidItems.Add(ModContent.ItemType<Content.Items.TileItems.Deadwood>());
        }
    }
}
