using Ascension.Attributes;
using Ascension.Enums;
using System.Collections.Generic;

namespace Ascension.Interfaces
{
    /// <summary>
    /// An <see cref="IStacking"/> which uses a cache system for it's calculations.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 10)]
    public interface ICacheableStacking : IStacking
    {
        /// <summary>
        /// Returns the cache.
        /// </summary>
        IReadOnlyDictionary<IStat, float> Cached { get; }

        /// <summary>
        /// Removes a specified stat's cache so it can be re-calcuated. Should be called when a property stat's value(s) are changed.
        /// </summary>
        /// <param name="stat"></param>
        void RefreshStatCache(IStat stat);

        /// <summary>
        /// Clears the cache of all previously calculated stats.
        /// </summary>
        void ClearStatsCache();
    }
}
