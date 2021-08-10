using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Internal;

namespace Ascension.Interfaces
{
    /// <summary>
    /// Stacking used with <see cref="EntityStats"/>.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 10)]
    public interface IStacking
    {
        /// <summary>
        /// Final value that will be returned.
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        float CalculatedValue(IStat stat);
    }
}
