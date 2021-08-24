    using Ascension.Attributes;
using Ascension.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public static class Reflection
        {
            public static FieldInfo[] GetConstants(Type type)
            {
                FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public |
                     BindingFlags.Static | BindingFlags.FlattenHierarchy);

                return fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToArray();
            }
        }

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
            /// Returns a damage calculated after applying penetration.
            /// </summary>
            /// <param name="damage"></param>
            /// <param name="pen"></param>
            /// <param name="for"></param>
            /// <returns></returns>
            public static int GetDamageWithPen(int damage, int pen, NPC @for)
            {
                return damage + pen >= @for.defense ? @for.defense : pen;
            }

            /// <summary>
            /// Creates a range of dusts.
            /// </summary>
            /// <param name="type">What dust to create.</param>
            /// <param name="amount">The random amount of dust to be created.</param>
            /// <param name="position">The position at which to create the dusts.</param>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <param name="color"></param>
            /// <param name="speedX"></param>
            /// <param name="speedY"></param>
            /// <param name="alpha"></param>
            /// <param name="scale"></param>
            /// <param name="onCreate"></param>
            public static void CreateDust(IntRange amount, Vector2Range position, int width, int height, Color color, FloatRange speedX, FloatRange speedY, IntRange alpha, FloatRange scale, Action<Dust> onCreate = null, params int[] types)
            {
                int max = amount.GetRandom();
                for (int i = 0; i < max; i++)
                {
                    int dust = Dust.NewDust(position.GetRandom(), width, height, Hooks.Collections.Random(types), speedX.GetRandom(), speedY.GetRandom(), alpha.GetRandom(), color, scale.GetRandom());
                    onCreate?.Invoke(Main.dust[dust]);
                }
            }

            public static void CreateDust(int type, IntRange amount, Vector2Range position, int width, int height, FloatRange speedX, FloatRange speedY, IntRange alpha, FloatRange scale, Action<Dust> onCreate = null)
                => CreateDust(amount, position, width, height, default, speedX, speedY, alpha, scale, onCreate, type);
            
            public static void CreateDust(int type, IntRange amount, Vector2Range position, int width, int height, FloatRange speedX, FloatRange speedY, Action<Dust> onCreate = null)
                => CreateDust(amount, position, width, height, default, speedX, speedY, 0, 1f, onCreate, type);

            public static void CreateGore(IntRange amount, Vector2Range position, Vector2Range velocity, FloatRange scale, Action<Gore> onCreate = null, params int[] types)
            {
                int count = amount.GetRandom();
                for (int i = 0; i < count; i++)
                {
                    int gore = Gore.NewGore(position.GetRandom(), velocity.GetRandom(), types.Random(), scale.GetRandom());
                    onCreate?.Invoke(Main.gore[gore]);
                }
            }

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

            public static void ApplyModBuffToAllWithin(int type, object attacker, Vector2 center, float within, int duration)
            {
                foreach (NPC npc in Hooks.InGame.GetAllWithin(attacker, center, within))
                {
                    npc.AddBuff(type, duration);
                }
            }

            public static void ApplyModBuffToAllWithin(int type, object attacker, Vector2 center, float within, int duration, Predicate<NPC> match)
            {
                if (match == null)
                    throw new ArgumentNullException("Cannot pass a null match to ApplyModBuffToAllWithin; Use the method without a Predicate instead.");
                
                foreach (NPC npc in Hooks.InGame.GetAllWithin(attacker, center, within))
                {
                    if (match(npc)) npc.AddBuff(type, duration);
                }
            }

            public static void ApplyModBuffToAllWithin<T>(object attacker, Vector2 center, float within, int duration) where T : ModBuff
            {
                foreach (NPC npc in Hooks.InGame.GetAllWithin(attacker, center, within))
                {
                    npc.AddBuff(ModContent.BuffType<T>(), duration);
                }
            }

            public static void ApplyModBuffToAllWithin<T>(object attacker, Vector2 center, float within, int duration, Predicate<NPC> match) where T : ModBuff
            {
                if (match == null)
                    throw new ArgumentNullException("Cannot pass a null match to ApplyModBuffToAllWithin; Use the method without a Predicate instead.");
                foreach (NPC npc in Hooks.InGame.GetAllWithin(attacker, center, within))
                {
                    if(match(npc)) npc.AddBuff(ModContent.BuffType<T>(), duration);
                }
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
            public static string GetFormatName<T>() => GetFormatName(typeof(T).Name);

            /// <summary>
            /// Returns a formatted string which separates capital letters with a space.
            /// </summary>
            /// <param name="for"></param>
            /// <returns></returns>
            public static string GetFormatName(object obj) => GetFormatName(obj.GetType().Name);

            /// <summary>
            /// Returns a formatted string which separates capital letters with a space.
            /// </summary>
            /// <param name="for"></param>
            /// <returns></returns>
            public static string GetFormatName(string name)
            {
                int classTypeIndex = name.IndexOf('_'); //This looks if a class starts with "Item_", "Projectile_", "Tile_", etc...
                if (classTypeIndex != -1)
                    name = name[(classTypeIndex + 1)..];

                MatchCollection matches = pv_Regex_ClassFormat.Matches(name);

                if (matches.Count < 1)
                    return name;

                string toReturn = name;

                foreach (Match match in matches)
                {
                    toReturn = toReturn.Insert(match.Index + 1, " ");
                }

                return toReturn;
            }

            [Note(Dev.WaitWhatWolf, "The right to left option is required as GetFormatClassName depends on the regex going from right to left.")]
            private static Regex pv_Regex_ClassFormat = new(@"[a-z][A-Z]", RegexOptions.RightToLeft);
        }

        public static class MathF
        {
            /// <summary>
            /// Processes a countdown and returns true if the countdown reached it's destination.
            /// </summary>
            /// <remarks>The countdown is counted down to 0, so negative <paramref name="dest"/> is not allowed.</remarks>
            /// <param name="countdown"></param>
            /// <param name="dest"></param>
            /// <returns></returns>
            public static bool ProcessCountdown(ref float countdown, float dest)
            {
                if (dest < 0f)
                    throw new ArgumentException("dest variable cannot be below 0.");

                countdown -= ASCResources.FLOAT_PER_FRAME;
                if(countdown <= 0)
                {
                    countdown = dest;
                    return true;
                }
                return false;
            }

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

            /// <summary>
            /// Clamps a float value between min and max.
            /// </summary>
            /// <param name="value"></param>
            /// <param name="min"></param>
            /// <param name="max"></param>
            /// <returns></returns>
            public static float Clamp(float value, float min, float max)
            {
                float aMin = Math.Min(min, max);
                float aMax = Math.Max(min, max);

                value = value > aMax ? aMax : value;
                return value < aMin ? aMin : value;
            }

            /// <summary>
            /// Clamps an int value between min and max.
            /// </summary>
            /// <param name="value"></param>
            /// <param name="min"></param>
            /// <param name="max"></param>
            /// <returns></returns>
            public static int Clamp(int value, int min, int max)
            {
                int aMin = Math.Min(min, max);
                int aMax = Math.Max(min, max);

                value = value > aMax ? aMax : value;
                return value < aMin ? aMin : value;
            }

            /// <summary>
            /// Clamps a <see cref="Vector2"/> value between min and max.
            /// </summary>
            /// <param name="value"></param>
            /// <param name="min"></param>
            /// <param name="max"></param>
            /// <returns></returns>
            public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max)
            {
                return new Vector2(Clamp(value.X, min.X, max.X), Clamp(value.Y, min.Y, max.Y));
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
            /// <summary>
            /// Color used to refer to either delays or non-standard countdown.
            /// </summary>
            public static readonly Color Tooltip_Delay = Color.DeepPink;
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
            /// Returns a colored text for item tooltips, which supports multi-line strings.
            /// </summary>
            /// <param name="text">The original text.</param>
            /// <param name="color">Color used for the text.</param>
            /// <returns></returns>
            public static string GetColoredTooltipMultilineText(string text, Color color)
            {
                if (!text.Contains('\n'))
                    return GetColoredTooltipText(text, color);

                string toReturn = string.Empty;
                Regex regex = new(@"(\n|^).*");
                Match match = regex.Match(text);

                while(true)
                {
                    toReturn += $"[c/{color.Hex3()}:{match.Value.Replace("\n", string.Empty)}]";
                    Match nextMatch = match.NextMatch();
                    if (!nextMatch.Success)
                        break;

                    toReturn += '\n';
                    match = nextMatch;
                }

                return toReturn;
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

