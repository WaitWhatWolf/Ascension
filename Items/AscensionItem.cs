using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Utility;
using Terraria.GameContent.Creative;
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
        /// <summary>
        /// Texture path of this item. By default, <see cref="AscensionItem"/> class sets it to <see cref="ASCResources.GetAssetsPath(ItemAssetType, IModType)"/> 
        /// where asset type is <see cref="ItemAssetType.Items"/> and <see cref="IModType"/> is <see cref="this"/>;
        /// If <see cref="TextureSubFolder"/> is set, it will also include it as argument for the subfolder.
        /// </summary>
        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.Items, TextureSubFolder, this);

        /// <summary>
        /// If this field is overriden, it will be included as a subfolder for <see cref="Texture"/>'s path.
        /// </summary>
        protected virtual string TextureSubFolder { get; } = string.Empty;

        /// <summary>
        /// Sets all necessary default values through abstract fields of this class; When overriding, make sure to include "base.SetStaticDefaults();".
        /// </summary>
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(DisplayNameDefault);
            Tooltip.SetDefault(TooltipDefault);
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = JourneyCheatCount;
        }

        /// <summary>
        /// The default display name of this item; Points to <see cref="Hooks.Text.GetFormatClassName(IModType)"/> by default.
        /// </summary>
        protected virtual string DisplayNameDefault => Hooks.Text.GetFormatName(this);
        /// <summary>
        /// The default tooltip of this item.
        /// </summary>
        protected abstract string TooltipDefault { get; }
        /// <summary>
        /// Amount of times this item is required to be cheatable in journey mode.
        /// </summary>
        protected abstract int JourneyCheatCount { get; }
    }
}
