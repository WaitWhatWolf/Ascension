    using Ascension.Attributes;
using Ascension.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.ModLoader;

namespace Ascension.Utility
{
    /// <summary>
    /// Utility class which contains the vast majority of utility methods.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 04)]
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

        public static class InGame
        {
            /// <summary>
            /// Returns a chance which rolls count times. (0-1 scale)
            /// </summary>
            /// <param name="count"></param>
            /// <returns></returns>
            public static float GetChance(int count)
            {
                {
                    float chance = 0;
                    for (int i = 0; i < count; i++)
                    {
                        float newChance = (float)ASCResources.GlobalRandom.NextDouble();
                        if (newChance > chance)
                            chance = newChance;
                    }

                    //Debug.Log(string.Format("Rolled Chance: {0}, Roll count was {1}", V_DEBUG_LAYER_ITEMS, chance, count));
                    return chance;
                }
            }

            /// <summary>
            /// Returns all NPCs within a given range of center.
            /// </summary>
            /// <param name="attacker"></param>
            /// <param name="center"></param>
            /// <param name="within"></param>
            /// <returns></returns>
            public static List<NPC> GetAllWithin(object attacker, Vector2 center, float within)
            {
                List<NPC> toReturn = new();

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];

                    if (npc.CanBeChasedBy(attacker, true))
                    {
                        float between = Vector2.Distance(npc.Center, center);
                        bool inRange = between <= within;

                        if (inRange)
                        {
                            toReturn.Add(npc);
                        }
                    }
                }

                return toReturn;
            }

            /// <summary>
            /// Returns all NPCs.
            /// </summary>
            /// <returns></returns>
            public static List<NPC> GetAll()
            {
                List<NPC> toReturn = new();

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];

                    toReturn.Add(npc);
                }

                return toReturn;
            }
        }

        public static class Random
        {
            /// <summary>
            /// Returns a <see cref="Vector2"/> with each of it's values being a random number between min and max.
            /// </summary>
            /// <param name="min"></param>
            /// <param name="max"></param>
            /// <returns></returns>
            public static Vector2 Range(Vector2 min, Vector2 max)
            {
                return new Vector2(Range(min.X, max.X), Range(min.Y, max.Y));
            }

            public static int Range(int min, int max)
            {
                return ASCResources.GlobalRandom.Next(min, max);
            }

            public static float Range(float min, float max)
            {
                return (float)(ASCResources.GlobalRandom.NextDouble() * (max - min) + min);
            }
        }

        /// <summary>
        /// All text-based utility methods & fields.
        /// </summary>
        public static class Text
        {
            /// <summary>
            /// Returns a formatted string which separates capital letters with a space.
            /// </summary>
            /// <param name="for"></param>
            /// <returns></returns>
            public static string GetFormatClassName(IModType @for)
            {
                string name = @for.GetType().Name;
                int classTypeIndex = name.IndexOf('_'); //This looks if a class starts with "Item_", "Projectile_", "Tile_", etc...
                if (classTypeIndex != -1)
                    name = name[(classTypeIndex + 1)..];

                Match match = pv_Regex_ClassFormat.Match(name);

                if (!match.Success)
                    return name;

                string toReturn = name;

                do
                {
                    match = match.NextMatch();
                    int index = match.Value.Length == 1 ? match.Index : match.Index + 1;
                    toReturn = toReturn.Insert(index, " ");
                }
                while (match.Success);

                return toReturn;
            }

            [Note(Dev.WaitWhatWolf, "The right to left option is required as GetFormatClassName depends on the regex going from right to left.")]
            private static Regex pv_Regex_ClassFormat = new(@"[a-z][A-Z]", RegexOptions.RightToLeft);
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

            /// <summary>
            ///   <para>Moves a point current towards target.</para>
            /// </summary>
            /// <param name="current"></param>
            /// <param name="target"></param>
            /// <param name="maxDistanceDelta"></param>
            public static Vector2 MoveTowards(Vector2 current, Vector2 target, float maxDistanceDelta)
            {
                Vector2 a = target - current;
                float magnitude = Magnitude(a);
                if (!(magnitude <= maxDistanceDelta) && magnitude != 0f)
                {
                    return current + a / magnitude * maxDistanceDelta;
                }
                return target;
            }

            /// <summary>
            /// Returns the magnitude of a vector. (Square root of it's dot product)
            /// </summary>
            /// <param name="original"></param>
            /// <returns></returns>
            public static float Magnitude(Vector2 original)
            {
                return System.MathF.Sqrt(Vector2.Dot(original, original));
            }

            /// <summary>
            /// Returns the deminishing returns of a percent value (0-1).
            /// </summary>
            /// <param name="value01"></param>
            /// <returns></returns>
            public static float DeminishingPercent(float value01)
            {
                return 1f / (1f + value01);
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

            /// <summary>
            /// Color used to refer to a stand title.
            /// </summary>
            public static readonly Color Tooltip_Stand_Title = Color.MediumPurple;

            /// <summary>
            /// Color used to refer to a stand ability.
            /// </summary>
            public static readonly Color Tooltip_Stand_Ability = Tangelo;

            /// <summary>
            /// Color used to refer to a stand ability's cooldown.
            /// </summary>
            public static readonly Color Tooltip_Stand_Ability_Cooldown = Color.PaleVioletRed;

            /// <summary>
            /// Color used for quotes.
            /// </summary>
            public static readonly Color Tooltip_Quote = Color.Crimson;
            /// <summary>
            /// Color used to refer to a damage class.
            /// </summary>
            public static readonly Color Tooltip_Class = Color.DarkRed;
            /// <summary>
            /// Color used to refer to special effects.
            /// </summary>
            public static readonly Color Tooltip_Effect = Color.Gold;
            /// <summary>
            /// Color used to refer to buffs.
            /// </summary>
            public static readonly Color Tooltip_Buff = Color.LimeGreen;
            /// <summary>
            /// Color used to refer to debuffs.
            /// </summary>
            public static readonly Color Tooltip_Debuff = Color.OrangeRed;
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
                if (rarity == ERarity.Unknown)
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

