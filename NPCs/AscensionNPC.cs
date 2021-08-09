using Ascension.Attributes;
using Ascension.Enums;
using Terraria.ModLoader;

namespace Ascension.NPCs
{
    /// <summary>
    /// The base class for most NPCs in the <see cref="Ascension"/> mod.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 05)]
    public abstract class AscensionNPC : ModNPC
    {
        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.NPCs, this);
    }
}
