using Ascension.Enums;
using Ascension.Items;
using Ascension.Players;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Input;
using Ascension.Internal;
using Terraria.Localization;
using Terraria.ID;
using Ascension.Sound;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

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
        /// Float value which would amount to 1f if called every frame. (Assuming the game runs at a smooth 60fps)
        /// </summary>
        public const float FLOAT_PER_FRAME = 1f / 60f;

        /// <summary>
        /// Returns the mouse position in vector value.
        /// </summary>
        public static Vector2 MousePos => new(Main.mouseX, Main.mouseY);

        /// <summary>
        /// Global Assets path.
        /// </summary>
        public const string ASSETS_PATH = "Ascension/Assets/";
        public const string ASSETS_PATH_ASSETSONLY = "Assets/";
        public const string ASSETS_PATH_ITEMS = ASSETS_PATH + "Items/";
        public const string ASSETS_PATH_NPCS = ASSETS_PATH + "NPCs/";
        public const string ASSETS_PATH_ARMORS = ASSETS_PATH + "Armors/";
        public const string ASSETS_PATH_MISC = ASSETS_PATH + "Misc/";
        public const string ASSETS_PATH_PROJECTILES = ASSETS_PATH + "Projectiles/";
        public const string ASSETS_PATH_DEBUFFS = ASSETS_PATH + "Debuffs/";
        public const string ASSETS_PATH_UI = ASSETS_PATH + "UI/";
        public const string ASSETS_PATH_UI_ASSETSONLY = ASSETS_PATH_ASSETSONLY + "UI/";

        public const string ASSETS_PATH_SOUND_CUSTOM = ASSETS_PATH + "Sound/Custom/";

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
                ItemAssetType.Projectiles => ASSETS_PATH_PROJECTILES,
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
            /// <param name="debugStandName">Whether a message shows in chat that shows which stand was manifested & upgraded.</param>
            /// <returns></returns>
            public static int ManifestStand(AscendedPlayer player, StandID id = StandID.NEWBIE, bool debugStandName = true)
            {
                bool create = CreateStand(player, id, debugStandName);
                
                int toUse = player.in_Stand.UpgradeStand(debugStandName);

                if (toUse == 0 && !create)
                    return -1;

                typeof(AscendedPlayer).GetMethod("OnManifestCall", BindingFlags.NonPublic | BindingFlags.Instance)
                    .CreateDelegate<Action<int>>(player).Invoke(toUse);

                return toUse;
            }

            private static bool CreateStand(AscendedPlayer player, StandID id, bool debugStandName)
            {
                if (player.in_IsStandUser)
                    return false;

                string playerName = player.Player.name;
                foreach (var standReference in pv_StandRefernces) //Looks if the player is named a specific name which would fit a stand
                {
                    if (playerName.Contains(standReference.Item1) && playerName.Contains(standReference.Item2))
                    {
                        id = standReference.Item3;
                        break;
                    }
                }

                if (id == StandID.NEWBIE) //Randomizes the stand if none were passed
                {
                    id = (StandID)GlobalRandom.Next(0, (int)StandID.HIEROPHANT_GREEN);
                }

                player.in_Stand = new Stand(player, id);
                player.in_IsStandUser = true;

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
                Item_RedHotChiliPepper redHotChiliPepper = ModContent.GetInstance<Item_RedHotChiliPepper>();

                redHotChiliPepper.CreateRecipe(1)
                    .AddRecipeGroup(RECIPE_GROUP_DEMONBARS, 10)
                    .AddRecipeGroup(RECIPE_GROUP_DEMONFRAGMENTS, 15)
                    .AddTile(TileID.DemonAltar)
                    .Register();
            }

            public static void LoadRecipeGroups()
            {
                RecipeGroup demonFragments = new (() => Language.GetTextValue("LegacyMisc.37") + " Demon Fragment", new int[]
                {
                    ItemID.ShadowScale,
                    ItemID.TissueSample
                });
                
                RecipeGroup demonBars = new (() => Language.GetTextValue("LegacyMisc.37") + " Demonic Bar", new int[]
                {
                    ItemID.DemoniteBar,
                    ItemID.CrimtaneBar
                });

                RecipeGroup.RegisterGroup(RECIPE_GROUP_DEMONFRAGMENTS, demonFragments);
                RecipeGroup.RegisterGroup(RECIPE_GROUP_DEMONBARS, demonBars);
            }

            /// <summary>
            /// Opposite of <see cref="Load"/> lol
            /// </summary>
            public static void Unload() //If you added anything in Load(), you should add an unloading of the same thing here
            {
            }

            public const string RECIPE_GROUP_DEMONFRAGMENTS = "Ascension:DemonFragments";
            public const string RECIPE_GROUP_DEMONBARS = "Ascension:DemonBars";
        }

        public static class Textures
        {
            public static Asset<Texture2D> Stand_Ability_StarPlatinum_Punch { get; private set; }
            public static Asset<Texture2D> Stand_Ability_StarPlatinum_ORA { get; private set; }
            public static Asset<Texture2D> Stand_Portrait_StarPlatinum { get; private set; }

            public static void Load(Ascension ascension)
            {
                Stand_Ability_StarPlatinum_Punch = ascension.Assets.Request<Texture2D>(STAND_ABILITY_STARPLATINUM_PUNCH);
                Stand_Ability_StarPlatinum_ORA = ascension.Assets.Request<Texture2D>(STAND_ABILITY_STARPLATINUM_ORA);
                Stand_Portrait_StarPlatinum = ascension.Assets.Request<Texture2D>(STAND_PORTRAIT_STARPLATINUM);
            }

            public static void Unload()
            {
                Stand_Ability_StarPlatinum_Punch = null;
                Stand_Ability_StarPlatinum_ORA = null;
                Stand_Portrait_StarPlatinum = null;
            }

            private const string STAND_ABILITY_STARPLATINUM_PUNCH = ASSETS_PATH_UI_ASSETSONLY + "Stand_Ability_StarPlatinum_Punch";
            private const string STAND_ABILITY_STARPLATINUM_ORA = ASSETS_PATH_UI_ASSETSONLY + "Stand_Ability_StarPlatinum_ORA";
            private const string STAND_PORTRAIT_STARPLATINUM = ASSETS_PATH_UI_ASSETSONLY + "Stand_Portrait_StarPlatinum";
        }

        public static class Input
        {
            public static ModKeybind Keybind_Stand_Invoke;
            public static ModKeybind Keybind_Stand_Ability_First;
            public static ModKeybind Keybind_Stand_Ability_Second;
            public static ModKeybind Keybind_Stand_Ability_Ultimate;

            /// <summary>
            /// Returns a keybind for a stand ability based on the ability index.
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public static ModKeybind GetStandAbilityKey(int index)
            {
                return index switch
                {
                    1 => Keybind_Stand_Ability_First,
                    2 => Keybind_Stand_Ability_Second,
                    _ => Keybind_Stand_Ability_Ultimate
                };
            }

            public static void Load(Ascension ascension)
            {
                Keybind_Stand_Invoke = KeybindLoader.RegisterKeybind(ascension, KEY_STAND_INVOKE, Keys.Q);
                Keybind_Stand_Ability_First = KeybindLoader.RegisterKeybind(ascension, KEY_STAND_ABILITY1, Keys.Z);
                Keybind_Stand_Ability_Second = KeybindLoader.RegisterKeybind(ascension, KEY_STAND_ABILITY2, Keys.X);
                Keybind_Stand_Ability_Ultimate = KeybindLoader.RegisterKeybind(ascension, KEY_STAND_ABILITYULTIMATE, Keys.R);
            }

            public static void Unload()
            {
                Keybind_Stand_Invoke = null;
            }

            public const string KEY_STAND_INVOKE = "Stand: Invoke";
            public const string KEY_STAND_ABILITY1 = "Stand: Ability 1";
            public const string KEY_STAND_ABILITY2 = "Stand: Ability 2";
            public const string KEY_STAND_ABILITYULTIMATE = "Stand: Ultimate";
        }

        /// <summary>
        /// All stat-related shenanegans.
        /// </summary>
        public static class Stats
        {
            public static void Load()
            {
                DamageClass_Umbral = new UmbralDamageClass();
            }

            public static void Unload()
            {

            }

            public const string STAND_STAT_DAMAGE = "Stand Damage";
            public const string STAND_STAT_ATTACKRANGE = "Stand Attack Range";
            public const string STAND_STAT_ARMORPEN = "Stand ArmorPen";
            public const string STAND_STAT_KNOCKBACK = "Stand Knock";
            public const string STAND_STAT_ATTACKSPEED = "Stand AS";
            public const string STAND_STAT_MOVESPEED = "Stand MS";
            public const string STAND_STAT_AIRANGE = "Stand Range AI";
            public const float UMBRAL_CRIT_BASE = 4f;
            public static UmbralDamageClass DamageClass_Umbral;
        }

        public static class Sound
        {
            public const string STAND_STARPLATINUM_INVOKE = ASSETS_PATH_SOUND_CUSTOM + "Stand_StarPlatinum_Invoke";

            public static int Stand_StarPlatinum_Invoke_Index { get; private set; }

            public static Dictionary<int, AscensionSound> Sounds { get; } = new();

            public static void Load(Ascension ascension)
            {
                //AscensionSound as_StarPlatinum_Invoke;
                ascension.AddSound(SoundType.Custom, STAND_STARPLATINUM_INVOKE);
                Stand_StarPlatinum_Invoke_Index = SoundLoader.GetSoundSlot(SoundType.Custom, STAND_STARPLATINUM_INVOKE);
                //Sounds.Add(Stand_StarPlatinum_Invoke_Index, as_StarPlatinum_Invoke);
            }

            public static void Unload()
            {
                
            }
        }
    }
}
