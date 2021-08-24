using Ascension;
using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Interfaces;
using Ascension.Internal;
using Ascension.Items;
using Ascension.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Ascension.Players
{
    /// <summary>
    /// Core player class of the <see cref="Ascension"/> mod.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 05)]
    public sealed class AscendedPlayer : ModPlayer, IAscensionEntity
    {
        public bool ConsumedRedHotChiliPepper;
        
        public bool[] UnlockedStandAbility { get; private set; } = new bool[4];
        public List<string> DefeatedBosses { get; private set; }

        /// <summary>
        /// Stats of the ascended player.
        /// </summary>
        public EntityStats Stats { get; private set; } 

        /// <summary>
        /// Invoked when a new boss is defeated.
        /// </summary>
        public event Action<string> OnNewBossDefeated;

        /// <summary>
        /// Invoked when the player manifests his stand.
        /// </summary>
        public event Action<Stand, int> OnManifestStand;

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
            if (!in_IsStandUser || Player.dead)
                return;

            if (ASCResources.Input.Keybind_Stand_Invoke.JustPressed)
            {
                if (!in_Stand.Active)
                    in_Stand.Invoke();
                else in_Stand.Recall();
            }

            if(in_IsStandUser)
                in_Stand.Update();
        }

        public override void PostUpdate()
        {
            if (in_IsStandUser)
                in_Stand.UpdateStats();
        }

        public override void OnEnterWorld(Player player)
        {
            Stats = new EntityStats(this);

            if (pv_LoadedStandID != StandID.NEWBIE && ASCResources.Players.ManifestStand(this, pv_LoadedStandID, false) != -1)
            {
                Debug.Log($"Your will is currently manifested as {in_Stand.Name}, Level {in_Stand.Level}");
            }

            //Debug.Log("Defeated Bosses: " + DefeatedBosses.Count);
            //Debug.LogEnumerable(DefeatedBosses);
        }

        public override void Initialize()
        {
            in_Stand = null;
            pv_LoadedStandID = StandID.NEWBIE;
            Stats = null;
        }

        public override void UpdateDead()
        {
            base.UpdateDead();
            if(in_IsStandUser) in_Stand.Recall();
        }

        public override void OnRespawn(Player player)
        {
            base.OnRespawn(player);
            if(in_IsStandUser) in_Stand.Invoke();
        }

        private void OnManifestCall(int val)
        {
            OnManifestStand?.Invoke(in_Stand, val);
        }

        internal Stand in_Stand = null;
        internal bool in_IsStandUser = false;
        private StandID pv_LoadedStandID = StandID.NEWBIE;
    }
}
