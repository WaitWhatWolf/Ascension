using Ascension.Attributes;
using Ascension.Enums;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Ascension.Internal
{
    /// <summary>
    /// Recipe class used in <see cref="ASCResources.Recipes"/> to automatically create a recipe.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/08/24 5:53:22")]
    public sealed class RecipeCreator<T> where T : ModItem
    {
        public Recipe Recipe;

        public RecipeCreator(int amount, Func<Recipe, Recipe> arg)
        {
            T item = ModContent.GetInstance<T>();
            Recipe = arg(item.CreateRecipe(amount));
        }

        [Obsolete]
        public static implicit operator RecipeCreator<T>(Func<Recipe, Recipe> func)
            => new(1, func);
    }
}
