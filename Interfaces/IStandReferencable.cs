using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Players;

namespace Ascension.Interfaces
{
    /// <summary>
    /// Interface which implements a stand reference.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08)]
    public interface IStandReferencable
    {
        /// <summary>
        /// The stand reference.
        /// </summary>
        Stand Stand { get; }
    }
}
