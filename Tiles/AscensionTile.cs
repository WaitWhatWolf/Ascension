using Ascension.Attributes;
using Ascension.Enums;
using Terraria.ModLoader;

namespace Ascension.Tiles
{
    /// <summary>
    /// The base class for most tiles in the <see cref="Ascension"/> mod.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08)]
    public abstract class AscensionTile : ModTile
    {
        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.Tiles, this);
    }
}
