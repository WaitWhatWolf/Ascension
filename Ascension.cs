using Terraria.ModLoader;

namespace Ascension
{
	public sealed class Ascension : Mod
	{
        public override void Load()
        {
            ASCResources.DeathReasons.Load();
            ASCResources.Recipes.Load();
            ASCResources.Input.Load(this);
        }

        public override void Unload()
        {
            ASCResources.DeathReasons.Unload();
            ASCResources.Recipes.Unload();
            ASCResources.Input.Unload();
        }
    }
}