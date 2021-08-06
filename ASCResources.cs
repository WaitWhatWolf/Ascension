using Ascension.Enums;
using Ascension.Items;
using Ascension.Players;
using Ascension.Utility;
using Ascension.Players;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Input;

namespace Ascension
{
    /// <summary>
    /// The class which stores all static and const resources of this mod.
    /// </summary>
    public static class ASCResources
    {
        public static readonly Random GlobalRandom = new();
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
        public static Vector2 MousePos => new(Main.mouseX, Main.mouseY);

        /// <summary>
        /// Global Assets path.
        /// </summary>
        public const string ASSETS_PATH = "Ascension/Assets/";
        public const string ASSETS_PATH_ITEMS = ASSETS_PATH + "Items/";
        public const string ASSETS_PATH_NPCS = ASSETS_PATH + "NPCs/";
        public const string ASSETS_PATH_ARMORS = ASSETS_PATH + "Armors/";
        public const string ASSETS_PATH_MISC = ASSETS_PATH + "Misc/";
        public const string ASSETS_PATH_PROJECTILES = ASSETS_PATH + "Projectiles/";
        public const string ASSETS_PATH_DEBUFFS = ASSETS_PATH + "Debuffs/";

        /// <summary>
        /// Returns the full path towards a moded component's texture.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="classType"></param>
        /// <returns></returns>
        public static string GetAssetsPath(ItemAssetType type, IModType caller)
            => type switch
            {
                ItemAssetType.Misc => ASSETS_PATH_MISC,
                ItemAssetType.Items => ASSETS_PATH_ITEMS,
                ItemAssetType.NPCs => ASSETS_PATH_NPCS,
                ItemAssetType.Debuffs => ASSETS_PATH_DEBUFFS,
                ItemAssetType.Armors => ASSETS_PATH_ARMORS,
                ItemAssetType.Projectiles => ASSETS_PATH_ITEMS,
                _ => throw new System.Exception("ItemAssetType was set to ItemAssetType.Undefined; This is not allowed.")
            } + caller.GetType().Name;

        public static class Players
        {
            public static string GetStandName(StandID id)
                => id switch
                {
                    StandID.STAR_PLATINUM => "Star Platinum",
                    StandID.THE_WORLD => "The World",
                    StandID.MAGICIANS_RED => "Magician's Red",
                    _ => string.Empty
                };

            /// <summary>
            /// Manifests a stand for a given player.
            /// </summary>
            /// <param name="player">The player which will manifest the stand.</param>
            /// <param name="id">Which stand to manifest; If left at <see cref="StandID.NEWBIE"/>, it will select a random stand.</param>
            /// <param name="debugStandName">Whether a message shows in chat that shows which stand was manifested.</param>
            /// <returns></returns>
            public static bool ManifestStand(AscendedPlayer player, StandID id = StandID.NEWBIE, bool debugStandName = true)
            {
                if (player.in_Stand)
                    return false;

                string playerName = player.Player.name;
                foreach (var standReference in pv_StandRefernces) //Looks if the player is named a specific name which would fit a stand
                {
                    if(playerName.Contains(standReference.Item1) && playerName.Contains(standReference.Item2))
                    {
                        id = standReference.Item3;
                        break;
                    }
                }

                if(id == StandID.NEWBIE) //Randomizes the stand if none were passed
                {
                    id = (StandID)GlobalRandom.Next(0, (int)StandID.HIEROPHANT_GREEN);
                }

                player.in_Stand = new Stand(player, id);

                if (debugStandName)
                    Debug.Log($"{playerName}'s will manifested as {player.in_Stand.Name}!");

                return true;
            }

            /// <summary>
            /// Used by <see cref="ManifestStand(AscendedPlayer, StandID)"/>.
            /// </summary>
            private static readonly (string, string, StandID)[] pv_StandRefernces = new[]
            {
                ("Kujo", "Jotaro", StandID.STAR_PLATINUM),
                ("Dio", "Brando", StandID.THE_WORLD),
                ("Muhammad", "Avdol", StandID.MAGICIANS_RED),
                ("Kira", "Yoshikage", StandID.KILLER_QUEEN),
                ("Noriaki", "Kakyoin", StandID.HIEROPHANT_GREEN),
            };
        }

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
                PDRV_STANDARROW = new[] { "{0} couldn't handle the arrow's power.", "The power of the arrow ripped {0} to shreds.", "{0} wasn't worthy of the arrow's power." };

                pv_Reasons = new SortedDictionary<string, IEnumerable<string>>();
                FieldInfo[] fields = typeof(DeathReasons).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.CreateInstance);
                foreach(FieldInfo field in fields)
                {
                    if(field.Name.StartsWith("PDRV_"))
                    {
                        string[] val = (string[])field.GetValue(null);
                        if(val != null)
                            pv_Reasons.Add(field.Name.Replace("PDRV_", string.Empty), val);
                    }
                }
            }

            public static void Unload()
            {
                PDRV_STANDARROW = null;
                pv_Reasons = null;
            }

            /// <summary>
            /// Returns a custom death reason based on name.
            /// </summary>
            /// <param name="name">Name of the custom reason to search for.</param>
            /// <param name="args">Arguments to pass for a Format string.</param>
            /// <returns></returns>
            public static PlayerDeathReason GetReason(string name, params object[] args)
            {
                return PlayerDeathReason.ByCustomReason(string.Format(pv_Reasons[name].Random(), args));
            }

#pragma warning disable IDE0052 //Put the PDRV_ variables in here
            /// <summary> Killed by <see cref="Item_StandArrow"/>. </summary>
            private static string[] PDRV_STANDARROW;
#pragma warning restore IDE0052

            private static SortedDictionary<string, IEnumerable<string>> pv_Reasons;
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

        public static class Input
        {
            public static ModKeybind Keybind_Stand_Invoke;

            public static void Load(Ascension ascension)
            {
                Keybind_Stand_Invoke = KeybindLoader.RegisterKeybind(ascension, KEY_STAND_INVOKE, Keys.Q);
            }

            public static void Unload()
            {
                Keybind_Stand_Invoke = null;
            }

            public const string KEY_STAND_INVOKE = "Invoke Stand";
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
