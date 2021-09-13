using Ascension.Attributes;

namespace Ascension.Enums
{
    /// <summary>
    /// Used with <see cref="AscensionItem"/> to determine where the image asset(s) is loaded from.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 06)]
    public enum ItemAssetType
    {
        /// <summary>
        /// This will usually crash the game on compile, avoid at all costs.
        /// </summary>
        Undefined = -1,
        Misc = 0,
        Items,
        NPCs,
        Buffs,
        Projectiles,
        Dusts,
        Tiles,
        Walls,
        Placeables,
        Biomes,
    }
}
