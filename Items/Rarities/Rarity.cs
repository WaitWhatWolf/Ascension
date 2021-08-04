using Ascension.Enums;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace Ascension.Items.Rarities
{
    /// <summary>
    /// The base rarity class for all <see cref="Ascension"/> rarities.
    /// </summary>
    public abstract class Rarity : ModRarity
    {
        /// <inheritdoc/>
        public override sealed int GetPrefixedRarity(int offset, float valueMult) => this.Type;

        /// <summary>
        /// Returns the color based on <see cref="ERarity"/>.
        /// </summary>
        public override Color RarityColor => Hooks.Colors.GetColorByRarity(PR_Rarity);

        /// <summary>
        /// The rarity set for this <see cref="Rarity"/>.
        /// </summary>
        protected abstract ERarity PR_Rarity { get; }
    }
}
