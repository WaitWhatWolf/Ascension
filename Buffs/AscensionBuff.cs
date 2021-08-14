using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Utility;
using Terraria;
using Terraria.ModLoader;

namespace Ascension.Buffs
{
    /// <summary>
    /// The base class for most buffs in the <see cref="Ascension"/> mod.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08)]
    public abstract class AscensionBuff : ModBuff
    {
        /// <summary>
        /// Texture path of this item. By default, <see cref="AscensionItem"/> class sets it to <see cref="ASCResources.GetAssetsPath(ItemAssetType, IModType)"/> 
        /// where asset type is <see cref="ItemAssetType.Buffs"/> and <see cref="IModType"/> is <see cref="this"/>;
        /// If <see cref="TextureSubFolder"/> is set, it will also include it as argument for the subfolder.
        /// </summary>
        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.Buffs, TextureSubFolder, this);
        /// <summary>
        /// If this field is overriden, it will be included as a subfolder for <see cref="Texture"/>'s path.
        /// </summary>
        protected virtual string TextureSubFolder { get; } = string.Empty;

        /// <summary>
        /// Sets the <see cref="DisplayNameDefault"/> and <see cref="DescriptionDefault"/>, as well setting all overridable bool values.
        /// </summary>
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(DisplayNameDefault);
            Description.SetDefault(DescriptionDefault);
            Main.debuff[Type] = CountsAsDebuff;
            Main.buffNoSave[Type] = !SaveBuff;
            Main.buffNoTimeDisplay[Type] = !DisplayBuffTimer;
        }

        /// <summary>
        /// The default display name of this item; Points to <see cref="Hooks.Text.GetFormatClassName(IModType)"/> by default.
        /// </summary>
        protected virtual string DisplayNameDefault => Hooks.Text.GetFormatClassName(this);
        /// <summary>
        /// The default tooltip of this item.
        /// </summary>
        protected abstract string DescriptionDefault { get; }
        /// <summary>
        /// Whether this buff is counted as a debuff in <see cref="Main.debuff"/>.
        /// </summary>
        protected virtual bool CountsAsDebuff { get; } = true;
        /// <summary>
        /// Whether this buff is persistent when exiting the game. Applies the opposite value of this to <see cref="Main.buffNoSave"/>.
        /// </summary>
        protected virtual bool SaveBuff { get; } = false;
        /// <summary>
        /// Whether this buff will display it's time left. Applies the opposite value of this to <see cref="Main.buffNoTimeDisplay"/>
        /// </summary>
        protected virtual bool DisplayBuffTimer { get; } = true;
    }
}
