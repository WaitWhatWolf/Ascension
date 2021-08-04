using Ascension.Enums;

namespace Ascension.Items.Rarities
{
    /// <summary>
    /// References <see cref="ERarity.Rare"/>.
    /// </summary>
    public sealed class Rarity_Rare : Rarity
    {
        protected override ERarity PR_Rarity { get; } = ERarity.Rare;
    }
}
