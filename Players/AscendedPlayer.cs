using Ascension;
using Ascension.Enums;
using Ascension.Items;
using Ascension.Players;
using System.Collections.Generic;
using Terraria;
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
        
        /// <inheritdoc/>
        public override TagCompound Save()
        {
            return new TagCompound()
            {
                [nameof(StandID)] = !in_Stand ? -1 : (int)in_Stand.ID
            };
        }

        /// <summary>
        /// Loads the stand.
        /// </summary>
        /// <param name="tag"></param>
        public override void Load(TagCompound tag)
        {
            pv_LoadedStandID = (StandID)tag.GetInt(nameof(StandID));
        }

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            if (!mediumCoreDeath)
                return new [] { new Item(ModContent.ItemType<Item_StandArrow>()) };
            
            return null;
        }

        public override void OnEnterWorld(Player player)
        {
            if (pv_LoadedStandID != StandID.NEWBIE && ASCResources.Players.ManifestStand(this, pv_LoadedStandID, false))
                Debug.Log($"Your will is currently manifested as {in_Stand.Name}");
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
