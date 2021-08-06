using Terraria.ModLoader;

namespace Ascension
{
	public sealed class Ascension : Mod
	{
        public override void AddRecipeGroups()
        {
            ASCResources.Recipes.LoadRecipeGroups();
        }

        public override void AddRecipes()
        {
            ASCResources.Recipes.Load();
        }

        public override void Load()
        {
            ASCResources.DeathReasons.Load();
            ASCResources.Input.Load(this);
            ASCResources.Stats.Load();
        }

        public override void Unload()
        {
            ASCResources.DeathReasons.Unload();
            ASCResources.Recipes.Unload();
            ASCResources.Input.Unload();
            ASCResources.Stats.Unload();
        }
    }
}