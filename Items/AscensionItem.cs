using Ascension.Attributes;
using Ascension.Enums;
using Terraria.ModLoader;
using static Ascension.ASCResources;

namespace Ascension.Items
{
    /// <summary>
    /// The base class for most items in the <see cref="Ascension"/> mod.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 06)]
    public abstract class AscensionItem : ModItem
    {
        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.Items, this);
    }
}
