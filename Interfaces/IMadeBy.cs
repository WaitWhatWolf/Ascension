using Ascension.Attributes;
using Ascension.Enums;

namespace Ascension.Interfaces
{
    /// <summary>
    /// Interface which indicates a dev.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 10)]
    public interface IMadeBy
    {
        /// <summary>
        /// The responsible dev.
        /// </summary>
        Dev Dev { get; }
    }
}
