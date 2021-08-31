using Ascension.Attributes;
using Ascension.Enums;
using Terraria.ModLoader;

namespace Ascension
{
    [CreatedBy(Dev.All, 2021, 08, 04), Note(Dev.WaitWhatWolf, "<3 (nohomo)")]
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
            ASCResources.Delegates.Load();
            ASCResources.DeathReasons.Load();
            ASCResources.Dusts.Load();
            ASCResources.Reflection.Load();
            ASCResources.Textures.Load(this);
            ASCResources.Input.Load(this);
            ASCResources.Stats.Load();
            ASCResources.Sound.Load(this);
        }

        public override void Unload()
        {
            ASCResources.Delegates.Unload();
            ASCResources.DeathReasons.Unload();
            ASCResources.Reflection.Unload();
            ASCResources.Recipes.Unload();
            ASCResources.Textures.Unload();
            ASCResources.Input.Unload();
            ASCResources.Stats.Unload();
            ASCResources.Sound.Unload();
        }

        public Ascension()
        {
            SoundAutoloadingEnabled = false;
        }
    }
}