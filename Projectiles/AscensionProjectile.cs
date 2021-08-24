using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Utility;
using Terraria.ModLoader;

namespace Ascension.Projectiles
{
    /// <summary>
    /// Base class for most projectiles in the <see cref="Ascension"/> mod.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 05)]
    public abstract class AscensionProjectile : ModProjectile
    {
        /// <summary>
        /// Texture path of this item. By default, <see cref="AscensionItem"/> class sets it to <see cref="ASCResources.GetAssetsPath(ItemAssetType, IModType)"/> 
        /// where asset type is <see cref="ItemAssetType.Projectiles"/> and <see cref="IModType"/> is <see cref="this"/>;
        /// If <see cref="TextureSubFolder"/> is set, it will also include it as argument for the subfolder.
        /// </summary>
        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.Projectiles, TextureSubFolder, this);

        /// <summary>
        /// If this field is overriden, it will be included as a subfolder for <see cref="Texture"/>'s path.
        /// </summary>
        protected virtual string TextureSubFolder { get; } = string.Empty;

        /// <summary>
        /// Sets the display default to <see cref="DisplayNameDefault"/>.
        /// </summary>
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(DisplayNameDefault);
        }

        /// <summary>
        /// The default display name of this projectile; Points to <see cref="Hooks.Text.GetFormatClassName(IModType)"/> by default.
        /// </summary>
        protected virtual string DisplayNameDefault => Hooks.Text.GetFormatName(this);
    }
}
