using Ascension.Enums;
using Terraria.ModLoader;
using static Ascension.ASCResources;

namespace Ascension.Items
{
    /// <summary>
    /// The base class for most items in the <see cref="Ascension"/> mod.
    /// </summary>
    public abstract class AscensionItem : ModItem
    {
        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.Items, this);
    }
}
