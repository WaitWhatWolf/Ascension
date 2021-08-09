using Ascension.Attributes;
using Ascension.Enums;
using Terraria.ModLoader;

namespace Ascension.Buffs
{
    /// <summary>
    /// The base class for most buffs in the <see cref="Ascension"/> mod.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08)]
    public abstract class AscensionBuff : ModBuff
    {
        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.Buffs, this);
    }
}
