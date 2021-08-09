using Ascension.Attributes;
using System;

namespace Ascension.Enums
{
    /// <summary>
    /// Developers of <see cref="Ascension"/> in an enum value.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 10), Flags]
    public enum Dev
    {
        Unknown,

        /// <summary>Biggest alpha dev in existance.</summary>
        WaitWhatWolf = 2,
        /// <summary>Biggest alpha dev in existance.</summary>
        Mariusz = WaitWhatWolf,

        Adragon = 4,
        Adi = Adragon,

        EbonVoid = 8,
        Chris = EbonVoid,

        All = WaitWhatWolf | Adragon | EbonVoid
    }
}
