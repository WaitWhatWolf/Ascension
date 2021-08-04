using Ascension.Items;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;

namespace Ascension
{
    /// <summary>
    /// The class which stores all static and const resources of this mod.
    /// </summary>
    public static class ASCResources
    {
        /// <summary>
        /// Timer used to get a consistent timespan for a counter.
        /// </summary>
        // Takes the current framerate, divides 60 by itself to get a float value which increases the lower the framerate
        // (this is here to get the same speed of adding regardless of framerate)
        // then divides it by 60 assuming it will be called once every frame, therefore 60 times a second.
        public static TimeSpan TimespanCounter => TimeSpan.FromSeconds(Math.Max(1, 60 / Terraria.Main.frameRate) / 60);

        /// <summary>
        /// Timer used to get a consistent timespan for a counter; Divided by 60 for an update-friendly timespan.
        /// </summary>
        public static TimeSpan TimespanCounterUF => TimeSpan.FromSeconds(Math.Max(1, 60 / Terraria.Main.frameRate) / 60);

        /// <summary>
        /// Returns the mouse position in vector value.
        /// </summary>
        public static Vector2 MousePos => new Vector2(Main.mouseX, Main.mouseY);

        /// <summary>
        /// Global Assets path.
        /// </summary>
        public const string ASSETS_PATH = "Ascension/Assets/";

        /// <summary>
        /// All possible death reasons; Note that they are all initiated during <see cref="Ascension.Load"/> through <see cref="DeathReasons.Load"/>.
        /// </summary>
        public static class DeathReasons
        {
            /// <summary>
            /// Loads all death reasons.
            /// </summary>
            public static void Load()
            {
                pv_Reasons = new SortedDictionary<string, string>();
                FieldInfo[] fields = typeof(DeathReasons).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.CreateInstance);
                foreach(FieldInfo field in fields)
                {
                    if(field.Name.StartsWith("PDRV_"))
                    {
                        pv_Reasons.Add(field.Name.Replace("PDRV_", string.Empty), field.GetValue(null).ToString());
                    }
                }

                PDRV_STANDARROWKILL = "{0} couldn't handle the arrow's power.";
            }

            public static void Unload()
            {
                PDRV_STANDARROWKILL = null;
            }

            /// <summary>
            /// Returns a custom death reason based on name.
            /// </summary>
            /// <param name="name">Name of the custom reason to search for.</param>
            /// <param name="args">Arguments to pass for a Format string.</param>
            /// <returns></returns>
            public static PlayerDeathReason GetReason(string name, params object[] args)
            {
                return PlayerDeathReason.ByCustomReason(string.Format(pv_Reasons[name], args));
            }

            /// <summary> Killed by <see cref="Item_StandArrow"/>. </summary>
            private static string PDRV_STANDARROWKILL;

            private static SortedDictionary<string, string> pv_Reasons;
        }

        /// <summary>
        /// All recipe-related shenanegans.
        /// </summary>
        public static class Recipes
        {
            /// <summary>
            /// Loads all repices.
            /// </summary>
            public static void Load() //Add any recipe-adding in this method
            {

            }

            /// <summary>
            /// Opposite of <see cref="Load"/> lol
            /// </summary>
            public static void Unload() //If you added anything in Load(), you should add an unloading of the same thing here
            {

            }
        }

        /// <summary>
        /// All stat-related shenanegans.
        /// </summary>
        public static class Stats
        {
            public const float UMBRAL_CRIT_BASE = 4f;
        }
    }
}
