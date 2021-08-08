using Ascension.Enums;
using Terraria.ModLoader;

namespace Ascension.Dusts
{
    /// <summary>
    /// The base class for most dusts in the <see cref="Ascension"/> mod.
    /// </summary>
    public abstract class AscensionDust : ModDust
    {
        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.Dusts, this);
    }
}
