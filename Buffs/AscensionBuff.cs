using Ascension.Enums;
using Terraria.ModLoader;

namespace Ascension.Buffs
{
    public abstract class AscensionBuff : ModBuff
    {
        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.Buffs, this);
    }
}
