using Ascension.Enums;

namespace Ascension.Utility
{
    public static class Extensions
    {
        /// <inheritdoc cref="Hooks.Colors.GetColorByRarity(ERarity)"/>
        public static void GetColor(this ERarity rarity) => Hooks.Colors.GetColorByRarity(rarity);
        /// <inheritdoc cref="Hooks.MathF.Truncate(float, int)"/>
        public static float Truncate(this float value, int digits) => Hooks.MathF.Truncate(value, digits);
        /// <inheritdoc cref="Hooks.MathF.Truncate(double, int)"/>
        public static double Truncate(this double value, int digits) => Hooks.MathF.Truncate(value, digits);
    }
}
