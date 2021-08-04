using Ascension.Enums;

namespace Ascension.Items.Rarities
{
    /// <summary>
    /// References <see cref="ERarity.Legendary"/>.
    /// </summary>
    public sealed class Rarity_Legendary : Rarity
    {
        protected override ERarity PR_Rarity { get; } = ERarity.Legendary;
    }
}
