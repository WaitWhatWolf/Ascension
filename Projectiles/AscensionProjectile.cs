using Ascension.Attributes;
using Ascension.Enums;
using Terraria.ModLoader;

namespace Ascension.Projectiles
{
    /// <summary>
    /// Base class for most projectiles in the <see cref="Ascension"/> mod.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 05)]
    public abstract class AscensionProjectile : ModProjectile
    {
        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.Projectiles, this);
    }
}
