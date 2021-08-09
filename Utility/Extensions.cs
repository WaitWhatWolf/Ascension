using Ascension.Attributes;
using Ascension.Enums;
using System.Collections.Generic;

namespace Ascension.Utility
{
    /// <summary>
    /// Utility class which contains all extension methods for <see cref="Hooks"/>.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 04)]
    public static class Extensions
    {
        public static T Random<T>(this IEnumerable<T> enumerable) => Hooks.Collections.Random(enumerable);
        /// <inheritdoc cref="Hooks.Colors.GetColorByRarity(ERarity)"/>
        public static void GetColor(this ERarity rarity) => Hooks.Colors.GetColorByRarity(rarity);
        /// <inheritdoc cref="Hooks.MathF.Truncate(float, int)"/>
        public static float Truncate(this float value, int digits) => Hooks.MathF.Truncate(value, digits);
        /// <inheritdoc cref="Hooks.MathF.Truncate(double, int)"/>
        public static double Truncate(this double value, int digits) => Hooks.MathF.Truncate(value, digits);
    }
}
