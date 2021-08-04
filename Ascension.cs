using Terraria.ModLoader;

namespace Ascension
{
	public sealed class Ascension : Mod
	{
        public override void Load()
        {
            ASCResources.DeathReasons.Load();
            ASCResources.Recipes.Load();
            
        }

        public override void Unload()
        {
            ASCResources.DeathReasons.Unload();
            ASCResources.Recipes.Unload();
        }
    }
}