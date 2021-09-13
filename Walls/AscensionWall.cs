using Ascension.Attributes;
using Ascension.Enums;
using Terraria.ModLoader;

namespace Ascension.Walls
{
    /// <summary>
    ///The base class for most walls in the <see cref="Ascension"/> mod.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/10 11:12:30")]
    public abstract class AscensionWall : ModWall
    {
        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.Walls, this);
    }
}
