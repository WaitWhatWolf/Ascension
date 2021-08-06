using Ascension.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace Ascension.Utility
{
    /// <summary>
    /// Utility class which contains the vast majority of utility methods.
    /// </summary>
    public static class Hooks
    {
        public static class Collections
        {
            /// <summary>
            /// Returns a random item from an enumerable.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="enumerable"></param>
            /// <returns></returns>
            public static T Random<T>(IEnumerable<T> enumerable)
            {
                T[] array = enumerable.ToArray();
                return array[ASCResources.GlobalRandom.Next(0, array.Length)];
            }
        }

        public static class MathF
        {
            /// <summary>
            /// Cuts a float to digits length.
            /// </summary>
            /// <param name="value"></param>
            /// <param name="digits"></param>
            /// <returns></returns>
            public static float Truncate(float value, int digits)
            {
                double num = Math.Pow(10.0, digits);
                double num2 = Math.Truncate(num * value) / num;
                return (float)num2;
            }

            /// <summary>
            /// Cuts a float to digits length.
            /// </summary>
            /// <param name="value"></param>
            /// <param name="digits"></param>
            /// <returns></returns>
            public static double Truncate(double value, int digits)
            {
                double num = Math.Pow(10.0, digits);
                double num2 = Math.Truncate(num * value) / num;
                return num2;
            }
        }

        /// <summary>
        /// All color utilities.
        /// </summary>
        public static class Colors
        {
            #region Specific colors
            /// <summary>
            /// The best color. (Orange color with a red-ish hue)
            /// </summary>
            public static readonly Color Tangelo = new Color(0.976f, 0.302f, 0f);
            #endregion

            /// <summary>
            /// Returns a colored text for item tooltips.
            /// </summary>
            /// <param name="text">The original text.</param>
            /// <param name="color">Color used for the text.</param>
            /// <returns></returns>
            public static string GetColoredTooltipText(string text, Color color)
            {
                //[c/FF0000:Colors ]
                return $"[c/{color.Hex3()}:{text}]";
            }

            /// <summary>
            /// Returns a color based on <see cref="ERarity"/>.
            /// </summary>
            /// <param name="rarity"></param>
            /// <returns></returns>
            public static Color GetColorByRarity(ERarity rarity)
            {
                if(rarity == ERarity.Unknown)
                    Debug.LogWarning("Passed rarity to Hooks.Colors.GetColorByRarity is ERarity.Unknown; Make sure this is intended.");

                return rarity switch
                {
                    ERarity.Common => Color.AntiqueWhite,
                    ERarity.Uncommon => Color.ForestGreen,
                    ERarity.Rare => Color.CadetBlue,
                    ERarity.VeryRare => Color.MediumPurple,
                    ERarity.Legendary => Color.OrangeRed,
                    ERarity.Chromic => Color.DeepPink,
                    _ => Color.DarkSlateGray
                };
            }
        }
    }
}
