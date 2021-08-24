using Ascension.Attributes;
using Ascension.Enums;
using Terraria;

namespace Ascension.Internal
{
    /// <summary>
    /// Recipe class used in <see cref="ASCResources.Recipes"/> to automatically create a recipe group.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 24)]
    internal sealed class RecipeGroupCreator
    {
        public readonly string CodeName;
        public readonly RecipeGroup Recipe;

        public RecipeGroupCreator(string codeName, RecipeGroup recipe)
        {
            CodeName = codeName;
            Recipe = recipe;
        }
    }
}
