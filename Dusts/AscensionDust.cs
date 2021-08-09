using Ascension.Attributes;
using Ascension.Enums;
using Terraria.ModLoader;

namespace Ascension.Dusts
{
    /// <summary>
    /// The base class for most dusts in the <see cref="Ascension"/> mod.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08)]
    public abstract class AscensionDust : ModDust
    {
        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.Dusts, this);
    }
}
