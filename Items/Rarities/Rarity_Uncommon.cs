using Ascension.Enums;

namespace Ascension.Items.Rarities
{
    /// <summary>
    /// References <see cref="ERarity.Uncommon"/>.
    /// </summary>
    public sealed class Rarity_Uncommon : Rarity
    {
        protected override ERarity PR_Rarity { get; } = ERarity.Uncommon;
    }
}
