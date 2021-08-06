using Ascension.Enums;
using Terraria.ModLoader;

namespace Ascension.Projectiles
{
    /// <summary>
    /// Base class for most projectiles in the <see cref="Ascension"/> mod.
    /// </summary>
    public abstract class AscensionProjectile : ModProjectile
    {
        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.Projectiles, this);
    }
}
