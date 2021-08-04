using Ascension.Enums;

namespace Ascension.Items.Rarities
{
    /// <summary>
    /// References <see cref="ERarity.Common"/>.
    /// </summary>
    public sealed class Rarity_Common : Rarity
    {
        protected override ERarity PR_Rarity { get; } = ERarity.Common;
    }
}
