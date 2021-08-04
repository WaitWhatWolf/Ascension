using Ascension.Enums;

namespace Ascension.Items.Rarities
{
    /// <summary>
    /// References <see cref="ERarity.VeryRare"/>.
    /// </summary>
    public sealed class Rarity_VeryRare : Rarity
    {
        protected override ERarity PR_Rarity { get; } = ERarity.VeryRare;
    }
}
