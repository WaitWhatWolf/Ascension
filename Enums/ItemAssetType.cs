using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascension.Enums
{
    /// <summary>
    /// Used with <see cref="AscensionItem"/> to determine where the image asset(s) is loaded from.
    /// </summary>
    public enum ItemAssetType
    {
        /// <summary>
        /// This will usually crash the game on compile, avoid at all costs.
        /// </summary>
        Undefined = -1,
        Misc = 0,
        Items,
        NPCs,
        Buffs,
        Projectiles,
        Dusts,
        Tiles,
        Placeables,
    }
}
