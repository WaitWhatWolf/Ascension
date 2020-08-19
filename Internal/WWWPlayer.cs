using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using WarWolfWorks_Mod.Interfaces;
using WarWolfWorks_Mod.Items;
using WarWolfWorks_Mod.UI;

namespace WarWolfWorks_Mod.Internal
{
    /// <summary>
    /// Core Player class of the WarWolfWorks mod.
    /// </summary>
    public sealed class WWWPlayer : ModPlayer
    {
        public static WWWPlayer Instance { get; private set; }

        /// <summary>
        /// All elements which will be called every frame.
        /// </summary>
        public static List<IUpdatable> Updatables { get; } = new List<IUpdatable>();
        /// <summary>
        /// All elements which will be called when the player loads into a world.
        /// </summary>
        public static List<IPostWorldLoadable> PostWorldLoadables { get; } = new List<IPostWorldLoadable>();

        /// <summary>
        /// Stand owned by the player.
        /// </summary>
        public Stand Stand { get; set; }

        /// <summary>
        /// Hotkey for stand interaction.
        /// </summary>
        public static ModHotKey SummonStandKey,
            AbilityKey,
            UltimateKey;

        private StandType LoadedStandID = StandType.NEWBIE;

        /// <summary>
        /// Saves the stand ID, so that it can be re-loaded based on the ID.
        /// </summary>
        /// <returns></returns>
        public override TagCompound Save()
        {
            return new TagCompound()
            {
                ["Stand"] = !Stand ? 0 : (int)Stand.ID
            };
        }

        /// <summary>
        /// Loads the stand.
        /// </summary>
        /// <param name="tag"></param>
        public override void Load(TagCompound tag)
        {
            LoadedStandID = (StandType)tag.GetInt("Stand");
        }

        /// <summary>
        /// Calls <see cref="Menu.OnWorldLoaded(WWWPlayer)"/> on all active menus.
        /// </summary>
        /// <param name="player"></param>
        public override void OnEnterWorld(Player player)
        {
            if (!Stand && false == true)
            {
                Stand = Stand.GetStandByID(LoadedStandID, this);
                if (Stand)
                {
                    Stand.DefineStand(player);
                }
            }

            foreach (IPostWorldLoadable ipwl in PostWorldLoadables)
                ipwl.OnWorldLoaded(this);
        }

        /// <summary>
        /// Gives the player a Stand Arrow when the character is first created.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="mediumcoreDeath"></param>
        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
            if (mediumcoreDeath)
                return;

            Item toAdd = new Item();
            toAdd.SetDefaults(ModContent.ItemType<StandArrow>());
            toAdd.stack = 1;
            items.Add(toAdd);
        }

        /// <summary>
        /// Gives the player a stand. NEWBIE by default, which chooses a random stand.
        /// </summary>
        /// <param name="specific"></param>
        public void ManifestStand(StandType specific = StandType.NEWBIE)
        {
            StandType toUse = specific;
            if(toUse == StandType.NEWBIE)
            {
                Random r = new Random();
                toUse = (StandType)r.Next(1, (int)StandType.THE_WORLD);
            }

            Stand = Stand.GetStandByID(toUse, this);
            Stand.DefineStand(player);
            string standUse = Stand ? Stand.Name : "nothing.";
            Main.NewText($"{player.name} has manifested {standUse}!", 175, 75, 255);
        }

        /// <summary>
        /// Calls each <see cref="IUpdatable"/> in <see cref="Updatables"/> every frame.
        /// </summary>
        public override void PreUpdate()
        {
            foreach (IUpdatable updatable in Updatables)
                updatable.Update();
        }

        /*public WWWPlayer()
        {
            Instance = this;
        }*/

        /// <summary>
        /// Implicit operator which allows use of !player instead of player == null, and vice-versa.
        /// </summary>
        /// <param name="player"></param>
        public static implicit operator bool(WWWPlayer player)
            => player != null;
    }
}
