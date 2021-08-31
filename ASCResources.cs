using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Internal;
using Ascension.Items;
using Ascension.Items.Consumables;
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
using System.Linq;
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
        public const string ASSETS_SUBPATH_CONSUMABLES = "Consumables/";

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
        
        [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 31)]
        public static class Delegates
        {
            public static Predicate<NPC> IsSlime { get; private set; }
            public static Predicate<NPC> IsNotSlime { get; private set; }

            internal static void Load()
            {
                IsSlime = (npc) => npc.drippingSlime || npc.FullName.ToUpper().Contains("SLIME");
                IsNotSlime = (npc) => !npc.drippingSlime && !npc.FullName.ToUpper().Contains("SLIME");
            }

            internal static void Unload()
            {
                IsSlime = null;
                IsNotSlime = null;
            }
        }

        [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 24)]
        public static class Items
        {
            public const string TOOLTIP_PARASITESLIMEWEAPON = "\nParasites do not attack slimes.";
        }

        /// <summary>
        /// Contains any information necessary for general systems like armor sets, item sets, etc...
        /// </summary>
        [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 31), Note(Dev.WaitWhatWolf, "I had no idea how to name this class ok")]
        public static class Trademark
        {
            public const float PARASITESLIME_EXP_RANGE = 60f;
            public const int PARASITESLIME_BUFF_DURATION = 80;
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
            public static string GetStandDescription(StandID id)
                => id switch
                {
                    StandID.STAR_PLATINUM => Hooks.Colors.GetColoredTooltipText("Star Platinum", Hooks.Colors.Tooltip_Stand_Title) 
                    + ", a stand which excels at\neverything outside of range.\nMost suited class: " + Hooks.Colors.GetColoredTooltipText("Melee", Hooks.Colors.Tooltip_Class) 
                    + "\n\n" 
                    + Hooks.Colors.GetColoredTooltipText("Good grief.", Hooks.Colors.Tooltip_Quote),
                    StandID.THE_WORLD => "The World",
                    StandID.MAGICIANS_RED => "Magician's Red",
                    StandID.KILLER_QUEEN => Hooks.Colors.GetColoredTooltipText("Killer Queen", Hooks.Colors.Tooltip_Stand_Title) 
                    + " is a stand which focuses primarily on the offensive,\nusing utility for defense rather than tough skin."
                    + "\nMost suited class: " + Hooks.Colors.GetColoredTooltipText("Melee", Hooks.Colors.Tooltip_Class)
                    + "\n\n"
                    + Hooks.Colors.GetColoredTooltipMultilineText("My name is Yoshikage Kira. I'm 33 years old." 
                    + "\nMy house is in the northeast section of Morioh," 
                    + "\nwhere all the villas are, and I am not married." 
                    + "\nI work as an employee for the Kame Yu department stores," 
                    + "\nand I get home every day by 8 PM at the latest."
                    + "\nI don't smoke, but I occasionally drink."
                    + "\nI'm in bed by 11 PM, and make sure I get eight hours of sleep, no matter what."
                    + "\nAfter having a glass of warm milk"
                    + "\nand doing about twenty minutes of stretches before going to bed,"
                    + "\nI usually have no problems sleeping until morning."
                    + "\nJust like a baby, I wake up without any fatigue or stress in the morning."
                    + "\nI was told there were no issues at my last check-up."
                    + "\nI'm trying to explain that I'm a person who wishes to live a very quiet life."
                    + "\nI take care not to trouble myself with any enemies," 
                    + "\nlike winning and losing, that would cause me to lose sleep at night."
                    + "\nThat is how I deal with society, and I know that is what brings me happiness."
                    + "\nAlthough, if I were to fight I wouldn't lose to anyone.", Hooks.Colors.Tooltip_Quote),
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

                player.in_Stand.TryUnlockAbilities(false);

                int toUse = player.in_Stand.UpgradeStand(debugStandName);

                if (toUse == 0 && !create)
                    return -1;

                typeof(AscendedPlayer).GetMethod("OnManifestCall", BindingFlags.NonPublic | BindingFlags.Instance)
                    .CreateDelegate<Action<int>>(player).Invoke(toUse);

                return toUse;
            }

            /// <summary>
            /// This is here so that the player doesn't break when he rolls an unfinished/experimental stand.
            /// </summary>
            public static readonly StandID[] UsableStands = new StandID[]
            {
                StandID.STAR_PLATINUM,
                StandID.KILLER_QUEEN
            };

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
                    id = UsableStands.Random();
                }

                var types = Hooks.Reflection.GetInheritingTypes(typeof(Stand));
                
                foreach(Type type in types)
                {
                    //Constructor hack; When no player is passed only properties are initialized.
                    //This isn't a very optimal approach, but it only triggers once when you enter a world so it's not
                    //the end of the world (no pun intended)
                    Stand tempStand = (Stand)Activator.CreateInstance(type, (AscendedPlayer)null);

                    if (tempStand.ID == id)
                    {
                        player.in_Stand = (Stand)Activator.CreateInstance(type, player);
                        player.in_IsStandUser = true;
                        break;
                    }
                }

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

        [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 24),
            Note(Dev.WaitWhatWolf, "It also contains gores.")]
        public static class Dusts
        {
            internal static void Load()
            {
                Gore_Stand_KillerQueen_Explosion = new(new(1, 4), new(-1f,-1f,1f,1f), new(-0.3f, -2f, 0.3f, -0.7f), new(0.8f, 1f), GoreID.Smoke1, GoreID.Smoke2, GoreID.Smoke3);
                Dust_Stand_KillerQueen_Explosion = new(new(25, 45), new(-1f, -1f, 1f, 1f), 8, 8, Color.White, new(-5f, 5f), new(-5f, 5f), new(0, 100), new(1f, 2f), DustID.MinecartSpark, DustID.SparksMech);
                Dust_Stand_KillerQueen_Bubble = new(new(5, 10), Vector2.Zero, 8, 8, Color.White, 0f, 0f, new(0, 50), new(0.8f, 1f), DustID.BubbleBurst_Blue, DustID.BubbleBurst_Purple);
                pv_Dust_ParasiteSlime_ProjTravel = new DustMaker(new(1, 3), Vector2.Zero, 5, 5, Color.Cyan, 0f, 0f, new(0, 100), new(0.9f, 1f), Event_ParasiteSlime, DustID.t_Slime, DustID.BunnySlime);
                Dust_ParasiteSlime_Explode = new DustMaker(new(20, 25), Vector2.Zero, 5, 5, Color.Cyan, new(-1f, 1f), new(1f, -0.3f), new(0, 100), new(0.8f, 1.4f), Event_ParasiteSlime, DustID.t_Slime, DustID.BunnySlime, DustID.Crimslime);
            }

            public static GoreMaker Gore_Stand_KillerQueen_Explosion { get; private set; }
            public static DustMaker Dust_Stand_KillerQueen_Explosion { get; private set; }
            public static DustMaker Dust_Stand_KillerQueen_Bubble { get; private set; }
            public static DustMaker Dust_ParasiteSlime_Explode { get; private set; }
            public static DustMaker Dust_ParasiteSlime_ProjTravel(Projectile @for) 
                => pv_Dust_ParasiteSlime_ProjTravel with { PosVariation = new(Vector2.Zero, @for.Size) };

            private static DustMaker pv_Dust_ParasiteSlime_ProjTravel;

            private static void Event_ParasiteSlime(Dust dust)
            {
                dust.velocity *= 1.4f;
                dust.noGravity = dust.scale != 1f;
            }
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
            private static RecipeCreator<Item_ParasiteSlimeArrow> RC_ParasiteSlimeArrow => new(100, (r) => r.AddIngredient<Item_ParasiteSlimeSample>().AddIngredient(ItemID.WoodenArrow, 100).AddTile(TileID.Solidifier));

            private static RecipeGroupCreator RGC_DemonFragments => new(GROUP_DEMONFRAGMENTS, CreateRecipeGroup("Demon Fragment", ItemID.ShadowScale, ItemID.TissueSample));
            private static RecipeGroupCreator RGC_DemonBars => new(GROUP_DEMONBARS, CreateRecipeGroup("Demon Bar", ItemID.DemoniteBar, ItemID.CrimtaneBar));
            private static RecipeGroupCreator RGC_GoldBars => new(GROUP_GOLDBARS, CreateRecipeGroup("Gold Bar", ItemID.GoldBar, ItemID.PlatinumBar));
#pragma warning restore IDE0051
        }

        [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08)]
        public static class Textures
        {
            public static Asset<Texture2D> GetTexture(string path) => pv_Textures[path];

            public static void Load(Ascension ascension)
            {
                FieldInfo[] properties = Hooks.Reflection.GetConstants(typeof(Textures));
                foreach(FieldInfo property in properties)
                {
                    if (property.Name.Any(char.IsLower))
                        continue;

                    string val = (string)property.GetValue(null);
                    System.Diagnostics.Debug.WriteLine(val);
                    Asset<Texture2D> texture = ascension.Assets.Request<Texture2D>(val);
                    pv_Textures.Add(val, texture);
                }
            }

            public static void Unload()
            {
                pv_Textures.Clear();
            }

            public const string STAND_ABILITY_STARPLATINUM_BASIC = ASSETS_PATH_UI_ASSETSONLY + "Stand_Ability_StarPlatinum_Punch";
            public const string STAND_ABILITY_STARPLATINUM_ABILITY1 = ASSETS_PATH_UI_ASSETSONLY + "Stand_Ability_StarPlatinum_ORA";
            public const string STAND_ABILITY_STARPLATINUM_ABILITY2 = ASSETS_PATH_UI_ASSETSONLY + "Stand_Ability_StarPlatinum_Receipt";
            public const string STAND_ABILITY_STARPLATINUM_ULTIMATE = ASSETS_PATH_UI_ASSETSONLY + "Stand_Ability_StarPlatinum_TheWorld";
            public const string STAND_PORTRAIT_STARPLATINUM = ASSETS_PATH_UI_ASSETSONLY + "Stand_Portrait_StarPlatinum";

            public const string STAND_ABILITY_KILLERQUEEN_BASIC = ASSETS_PATH_UI_ASSETSONLY + "Stand_Ability_KillerQueen_BombTransmutation";
            public const string STAND_ABILITY_KILLERQUEEN_ABILITY1 = ASSETS_PATH_UI_ASSETSONLY + "Stand_Ability_KillerQueen_StrayCatBombing";
            public const string STAND_PORTRAIT_KILLERQUEEN = ASSETS_PATH_UI_ASSETSONLY + "Stand_Portrait_KillerQueen";
            public const string STAND_MENU_BACKGROUND = ASSETS_PATH_UI_ASSETSONLY + "Stand_Menu_Background";

            private static readonly Dictionary<string, Asset<Texture2D>> pv_Textures = new();
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

            public const string STAND_STAT_DAMAGE = "Stand:Damage";
            public const string STAND_STAT_ATTACKRANGE = "Stand:Atk:Range";
            public const string STAND_STAT_ARMORPEN = "Stand:Atk:Pen";
            public const string STAND_STAT_KNOCKBACK = "Stand:Atk:Knock";
            public const string STAND_STAT_ATTACKSPEED = "Stand:Atk:RPM";
            public const string STAND_STAT_MOVESPEED = "Stand:Atk:MS";
            public const string STAND_STAT_AIRANGE = "Stand:AI:Range";
            public const string STAND_STAT_PROJECTILE_VELOCITY = "Stand:Proj:Vel";
            public const float UMBRAL_CRIT_BASE = 4f;
            public static DamageClass DamageClass_Umbral { get; } = new UmbralDamageClass();
        }

        [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08)]
        public static class Sound
        {
            public const string STAND_INVOKE_STARPLATINUM = ASSETS_PATH_SOUND_CUSTOM + "Stand_Invoke_StarPlatinum";
            public const string STAND_INVOKE_KILLERQUEEN = ASSETS_PATH_SOUND_CUSTOM + "Stand_Invoke_KillerQueen";

            public static int Stand_Invoke_Index_StarPlatinum { get; private set; }
            public static int Stand_Invoke_Index_KillerQueen { get; private set; }

            public static Dictionary<int, AscensionSound> Sounds { get; } = new();

            public static void Load(Ascension ascension)
            {
                //AscensionSound as_StarPlatinum_Invoke;
                ascension.AddSound(SoundType.Custom, STAND_INVOKE_STARPLATINUM);
                ascension.AddSound(SoundType.Custom, STAND_INVOKE_KILLERQUEEN);
                Stand_Invoke_Index_StarPlatinum = SoundLoader.GetSoundSlot(SoundType.Custom, STAND_INVOKE_STARPLATINUM);
                Stand_Invoke_Index_KillerQueen = SoundLoader.GetSoundSlot(SoundType.Custom, STAND_INVOKE_KILLERQUEEN);
                //Sounds.Add(Stand_StarPlatinum_Invoke_Index, as_StarPlatinum_Invoke);
            }

            public static void Unload()
            {
                
            }
        }
    }
}
