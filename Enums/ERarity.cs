using Ascension.Attributes;

namespace Ascension.Enums
{
    /// <summary>
    /// Rarity enum used for various methods, items, etc...
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 04)]
    public enum ERarity
    {
        /// <summary>
        /// This usually generates an exception.
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// Most common rarity.
        /// </summary>
        Common = 0,
        /// <summary>
        /// Slighty less common than <see cref="Common"/>.
        /// </summary>
        Uncommon,
        /// <summary>
        /// Rarer than <see cref="Uncommon"/>.
        /// </summary>
        Rare,
        /// <summary>
        /// Rarer than <see cref="Rare"/>.
        /// </summary>
        VeryRare,
        /// <summary>
        /// Rarer than <see cref="VeryRare"/>.
        /// </summary>
        Legendary,
        /// <summary>
        /// Rarest rarity in this enum.
        /// </summary>
        Chromic
    }
}
