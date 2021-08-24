using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Internal;
using Ascension.Items;
using Ascension.Items.Weapons;
using Ascension.Players;
using Ascension.Sound;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Ascension
{
    /// <summary>
    /// The class which stores all static and const resources of this mod.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 03)]
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
        public const float FLOAT_PER_FRAME = 0.0166666f;

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
        public const string ASSETS_PATH_DUSTS = ASSETS_PATH + "Dusts/";
        public const string ASSETS_PATH_TILES = ASSETS_PATH + "Tiles/";
        public const string ASSETS_PATH_PLACEABLES = ASSETS_PATH + "Placeables/";
        public const string ASSETS_PATH_BUFFS = ASSETS_PATH + "Buffs/";
        public const string ASSETS_PATH_UI = ASSETS_PATH + "UI/";
        public const string ASSETS_PATH_UI_ASSETSONLY = ASSETS_PATH_ASSETSONLY + "UI/";
        public const string ASSETS_PATH_SOUND_CUSTOM = ASSETS_PATH + "Sound/Custom/";

        public const string ASSETS_SUBPATH_MINIONS = "Minions/";
        public const string ASSETS_SUBPATH_WEAPONS = "Weapons/";

        /// <summary>
        /// Returns the full path towards a moded component's texture.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public static string GetAssetsPath(ItemAssetType type, IModType caller)
            => GetAssetsPath(type) + caller.GetType().Name;

        /// <summary>
        /// Returns the full path towards a moded component's texture; If subFolder given is empty, it will simply ignore it.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="subFolder"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public static string GetAssetsPath(ItemAssetType type, string subFolder, IModType caller)
            => !string.IsNullOrEmpty(subFolder) 
            ? GetAssetsPath(type) + subFolder + caller.GetType().Name
            : GetAssetsPath(type, caller);

        private static string GetAssetsPath(ItemAssetType type) 
            => type switch
            {
                ItemAssetType.Misc => ASSETS_PATH_MISC,
                ItemAssetType.Items => ASSETS_PATH_ITEMS,
                ItemAssetType.NPCs => ASSETS_PATH_NPCS,
                ItemAssetType.Buffs => ASSETS_PATH_BUFFS,
                ItemAssetType.Projectiles => ASSETS_PATH_PROJECTILES,
                ItemAssetType.Dusts => ASSETS_PATH_DUSTS,
                ItemAssetType.Placeables => ASSETS_PATH_PLACEABLES,
                ItemAssetType.Tiles => ASSETS_PATH_TILES,
                _ => throw new System.Exception("ItemAssetType was set to ItemAssetType.Undefined; This is not allowed.")
            };
        
        [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 24)]
        public static class Items
        {
            public const string TOOLTIP_PARASITESLIMEWEAPON = "\nParasites do not attack slimes.";
        }

        [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 10)]
        public static class Reflection
        {
            /// <summary>
            /// Pointer to a private field in <see cref="NPC"/> called ignorePlayerInteraction.
            /// </summary>
            public static int IgnorePlayerInteractions
            {
                get => (int)pv_FIignorePlayerInteraction.GetValue(null);
                set => pv_FIignorePlayerInteraction.SetValue(null, value);
            }

            public static void Load()
            {
                pv_FIignorePlayerInteraction = typeof(NPC).GetField("ignorePlayerInteractions", BindingFlags.NonPublic | BindingFlags.Instance);
            }

            public static void Unload()
            {

            }

            private static FieldInfo pv_FIignorePlayerInteraction;
        }

        [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 05)]
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
            
            public static string GetStandDescription(StandID id)
                => id switch
                {
                    StandID.STAR_PLATINUM => "Star Platinum, a stand which excels at\neverything outside of range.\nMost suited class: " + Hooks.Colors.GetColoredTooltipText("Melee", Hooks.Colors.Tooltip_Class) + "\n\n" + Hooks.Colors.GetColoredTooltipText("Good grief.", Hooks.Colors.Tooltip_Quote),
                    StandID.THE_WORLD => "The World",
                    StandID.MAGICIANS_RED => "Magician's Red",
                    _ => string.Empty
                };

            public static Action<Stand> GetStandStatUpdater(StandID id)
                => id switch
                {
                    StandID.STAR_PLATINUM => (s) => 
                    {
                        s.Owner.Player.GetDamage<MeleeDamageClass>() += (0.1f * s.Level);
                        s.Owner.Player.meleeSpeed *= (1.1f * s.Level);
                        s.Owner.Player.GetKnockback<MeleeDamageClass>() += (0.2f * s.Level);
                        s.Owner.Player.GetCritChance<MeleeDamageClass>() += (5 * s.Level);
                        s.Owner.Player.statDefense += (5 * s.Level);
                    }
                    ,
                    _ => throw new Exception("Cannot set stat updater for a undefined stand."),
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

                player.in_Stand.TryUnlockAbilities(false);

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
        [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 04)]
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
        [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 04)]
        public static class Recipes
        {
            /// <summary>
            /// Loads all repices.
            /// </summary>
            internal static void Load() //Add any recipe-adding in this method
            {
                PropertyInfo[] properties = typeof(Recipes).GetProperties(BindingFlags.NonPublic | BindingFlags.Static);

                foreach(PropertyInfo property in properties)
                {
                    if(!property.Name.StartsWith("RC_"))
                        continue;

                    object rc = property.GetValue(null);
                    FieldInfo field = rc.GetType().GetField("Recipe");
                    Recipe recipe = field.GetValue(rc) as Recipe;
                    recipe.Register();
                }
            }

            internal static void LoadRecipeGroups()
            {
                PropertyInfo[] properties = typeof(Recipes).GetProperties(BindingFlags.Static | BindingFlags.NonPublic);

                foreach(PropertyInfo property in properties)
                {
                    if (!property.Name.StartsWith("RGC_"))
                        continue;

                    RecipeGroupCreator rgc = (RecipeGroupCreator)property.GetValue(null);
                    RecipeGroup.RegisterGroup(rgc.CodeName, rgc.Recipe);
                }
            }

            /// <summary>
            /// Opposite of <see cref="Load"/> lol
            /// </summary>
            internal static void Unload() //If you added anything in Load(), you should add an unloading of the same thing here
            {
            }

            private static RecipeGroup CreateRecipeGroup(string name, params int[] IDs)
                => new(() => Language.GetTextValue("LegacyMisc.37") + ' ' + name, IDs);

            public const string GROUP_DEMONFRAGMENTS = "Ascension:DemonFragments";
            public const string GROUP_DEMONBARS = "Ascension:DemonBars";
            public const string GROUP_GOLDBARS = "Ascension:GoldBars";

#pragma warning disable IDE0051
            private static RecipeCreator<Item_RedHotChiliPepper> RC_RedHotChiliPepper => new(1, (r) => r.AddRecipeGroup(GROUP_DEMONBARS, 10)
                    .AddRecipeGroup(GROUP_DEMONFRAGMENTS, 15)
                    .AddTile(TileID.DemonAltar));

            private static RecipeCreator<Item_ParasiteSlimeBar> RC_ParasiteSlimeBar => new(4, (r) => r.AddRecipeGroup(GROUP_GOLDBARS, 4).AddIngredient<Item_ParasiteSlimeSample>(1).AddTile(TileID.Solidifier));
            private static RecipeCreator<Item_ParasiteSlimeJavelin> RC_ParasiteSlime => new(1, (r) => r.AddIngredient<Item_ParasiteSlimeBar>(10).AddTile(TileID.Solidifier));

            private static RecipeGroupCreator RGC_DemonFragments => new(GROUP_DEMONFRAGMENTS, CreateRecipeGroup("Demon Fragment", ItemID.ShadowScale, ItemID.TissueSample));
            private static RecipeGroupCreator RGC_DemonBars => new(GROUP_DEMONBARS, CreateRecipeGroup("Demon Bar", ItemID.DemoniteBar, ItemID.CrimtaneBar));
            private static RecipeGroupCreator RGC_GoldBars => new(GROUP_GOLDBARS, CreateRecipeGroup("Gold Bar", ItemID.GoldBar, ItemID.PlatinumBar));
#pragma warning restore IDE0051
        }

        [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08)]
        public static class Textures
        {
            public static Asset<Texture2D> Stand_Ability_StarPlatinum_Punch { get; private set; }
            public static Asset<Texture2D> Stand_Ability_StarPlatinum_ORA { get; private set; }
            public static Asset<Texture2D> Stand_Ability_StarPlatinum_Receipt { get; private set; }
            public static Asset<Texture2D> Stand_Ability_StarPlatinum_TheWorld { get; private set; }
            public static Asset<Texture2D> Stand_Portrait_StarPlatinum { get; private set; }
            public static Asset<Texture2D> Stand_Menu_Background { get; private set; }

            public static void Load(Ascension ascension)
            {
                Stand_Ability_StarPlatinum_Punch = ascension.Assets.Request<Texture2D>(STAND_ABILITY_STARPLATINUM_PUNCH);
                Stand_Ability_StarPlatinum_ORA = ascension.Assets.Request<Texture2D>(STAND_ABILITY_STARPLATINUM_ORA);
                Stand_Ability_StarPlatinum_Receipt = ascension.Assets.Request<Texture2D>(STAND_ABILITY_STARPLATINUM_RECEIPT);
                Stand_Ability_StarPlatinum_TheWorld = ascension.Assets.Request<Texture2D>(STAND_ABILITY_STARPLATINUM_THEWORLD);
                Stand_Portrait_StarPlatinum = ascension.Assets.Request<Texture2D>(STAND_PORTRAIT_STARPLATINUM);
                Stand_Menu_Background = ascension.Assets.Request<Texture2D>(STAND_MENU_BACKGROUND);
            }

            public static void Unload()
            {
                Stand_Ability_StarPlatinum_Punch = null;
                Stand_Ability_StarPlatinum_ORA = null;
                Stand_Ability_StarPlatinum_Receipt = null;
                Stand_Ability_StarPlatinum_TheWorld = null;
                Stand_Portrait_StarPlatinum = null;
                Stand_Menu_Background = null;
            }

            private const string STAND_ABILITY_STARPLATINUM_PUNCH = ASSETS_PATH_UI_ASSETSONLY + "Stand_Ability_StarPlatinum_Punch";
            private const string STAND_ABILITY_STARPLATINUM_ORA = ASSETS_PATH_UI_ASSETSONLY + "Stand_Ability_StarPlatinum_ORA";
            private const string STAND_ABILITY_STARPLATINUM_RECEIPT = ASSETS_PATH_UI_ASSETSONLY + "Stand_Ability_StarPlatinum_Receipt";
            private const string STAND_ABILITY_STARPLATINUM_THEWORLD = ASSETS_PATH_UI_ASSETSONLY + "Stand_Ability_StarPlatinum_TheWorld";
            private const string STAND_PORTRAIT_STARPLATINUM = ASSETS_PATH_UI_ASSETSONLY + "Stand_Portrait_StarPlatinum";
            private const string STAND_MENU_BACKGROUND = ASSETS_PATH_UI_ASSETSONLY + "Stand_Menu_Background";
        }

        [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 06)]
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
        [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 04)]
        public static class Stats
        {
            #region Stacking
            /// <summary>
            /// Base stacking index of the default stacking calculation. Stats with this stacking will be used as the base value for calculation.
            /// </summary>
            public const int STATS_STACKING_BASE = -1;
            /// <summary>
            /// Overrider stacking index of the default stacking calculation. Stats with this stacking will override the base value with it's own. (useful for weapons or items if they have base stats)
            /// Calculation: Base = Overrider
            /// </summary>
            public const int STATS_STACKING_OVERRIDER = 0;
            /// <summary>
            /// Additive stacking index of the default stacking calculation. Stats with this stacking will add themselves on top of the base value.
            /// Calculation: Base + Value
            /// </summary>
            public const int STATS_STACKING_ADDITIVE = 1;
            /// <summary>
            /// Base Multiplier stacking index of the default stacking calculation. Stats with this stacking will multiply the BASE value by their own.
            /// Calculation: Base * (Value + 1)
            /// </summary>
            public const int STATS_STACKING_BASEMULT = 2;
            /// <summary>
            /// Total Multiplier stacking index of the default stacking calculation. Stats with this stacking will multiply the TOTAL value by their own.
            /// Calculation: (Base + All Value Calculations) * (Value + 1)
            /// </summary>
            public const int STATS_STACKING_TOTALMULT = 3;
            /// <summary>
            /// Pwner stacking index of the default stacking calculation. Stats with this stacking will ignore any calculation and return their own value.
            /// Calculation: (Base + All Value Calculation) = Value
            /// </summary>
            public const int STATS_STACKING_PWNER = 42;
            #endregion

            public static void Load()
            {

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
            public static DamageClass DamageClass_Umbral { get; } = new UmbralDamageClass();
        }

        [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08)]
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
