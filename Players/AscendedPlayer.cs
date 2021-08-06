using Ascension;
using Ascension.Enums;
using Ascension.Items;
using Ascension.Players;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Ascension.Players
{
    /// <summary>
    /// Core player class of the <see cref="Ascension"/> mod.
    /// </summary>
    public sealed class AscendedPlayer : ModPlayer
    {
        public float BaseUmbralCrit;
        public bool ConsumedRedHotChiliPepper;
        
        public List<string> DefeatedBosses { get; private set; }

        /// <summary>
        /// Invoked when a new boss is defeated.
        /// </summary>
        public event Action<string> OnNewBossDefeated;

        /// <inheritdoc/>
        public override TagCompound Save()
        {
            return new TagCompound()
            {
                [nameof(StandID)] = !in_Stand ? -1 : (int)in_Stand.ID,
                [nameof(DefeatedBosses)] = DefeatedBosses,
                [nameof(ConsumedRedHotChiliPepper)] = ConsumedRedHotChiliPepper,
            };
        }

        /// <summary>
        /// Loads the stand.
        /// </summary>
        /// <param name="tag"></param>
        public override void Load(TagCompound tag)
        {
            pv_LoadedStandID = (StandID)tag.GetInt(nameof(StandID));
            DefeatedBosses = new(tag.GetList<string>(nameof(DefeatedBosses)));
            ConsumedRedHotChiliPepper = tag.GetBool(nameof(ConsumedRedHotChiliPepper));
        }

        public void AddDefeatedBoss(string name)
        {
            if (DefeatedBosses.Contains(name))
                return;

            DefeatedBosses.Add(name);
            OnNewBossDefeated?.Invoke(name);

            Debug.Log("Defeated Bosses: " + DefeatedBosses.Count);
        }

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            if (!mediumCoreDeath)
                return new [] { new Item(ModContent.ItemType<Item_StandArrow>()) };
            
            return null;
        }

        public override void PreUpdate()
        {
            if (ASCResources.Input.Keybind_Stand_Invoke.JustPressed)
            {
                if (!in_Stand.Active)
                    in_Stand.Invoke();
                else in_Stand.Recall();
            }
        }

        public override void OnEnterWorld(Player player)
        {
            if (pv_LoadedStandID != StandID.NEWBIE && ASCResources.Players.ManifestStand(this, pv_LoadedStandID, false) != -1)
                Debug.Log($"Your will is currently manifested as {in_Stand.Name}");

            //Debug.Log("Defeated Bosses: " + DefeatedBosses.Count);
            //Debug.LogEnumerable(DefeatedBosses);
        }

        public override void Initialize()
        {
            in_Stand = null;
            pv_LoadedStandID = StandID.NEWBIE;
        }

        internal Stand in_Stand = null;
        private StandID pv_LoadedStandID = StandID.NEWBIE;
    }
}
