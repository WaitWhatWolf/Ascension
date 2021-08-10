using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Interfaces;
using System.Collections.Generic;
using System.Linq;
using static Ascension.ASCResources.Stats;

namespace Ascension.Internal
{
    /// <summary>
    /// Default stacking which is used if <see cref="Stats.Stacking"/> is not set.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 10)]
    public sealed class DefaultStacking : Stacking, ICacheableStacking
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IReadOnlyDictionary<IStat, float> Cached => pv_Results;

        /// <summary>
        /// Returns the calculated value. See <see cref="DefaultStacking"/>'s constant values located in <see cref="WWWResources"/> for more info.
        /// </summary>
        /// <param name="BaseStat"></param>
        /// <returns></returns>
        public override float CalculatedValue(IStat BaseStat)
        {
            if (pv_Used.Contains(BaseStat) && pv_Results.TryGetValue(BaseStat, out float resultVal))
            {
                return resultVal;
            }

            float toReturn = BaseStat.Value;

            IStat[] usableStats = GetStatsByAffections(BaseStat.Affections);

            foreach (IStat overriderCandidate in usableStats)
            {
                if (overriderCandidate.Stacking == STATS_STACKING_PWNER)
                    return overriderCandidate.Value;
                else if (overriderCandidate.Stacking == STATS_STACKING_OVERRIDER)
                {
                    toReturn = overriderCandidate.Value;
                    break;
                }
            }

            float adder = GetStatValuesByStacking(usableStats, STATS_STACKING_ADDITIVE).Sum();
            float multiplier = GetStatValuesByStacking(usableStats, STATS_STACKING_BASEMULT).Sum() + 1f;

            toReturn += adder;
            toReturn *= multiplier;

            foreach (float f in GetStatValuesByStacking(usableStats, STATS_STACKING_TOTALMULT))
            {
                toReturn *= (f + 1f);
            }

            pv_Used.Add(BaseStat);
            pv_Results.Add(BaseStat, toReturn);

            return toReturn;
        }

        /// <summary>
        /// Refreshes an existing stat, effectively removing it's cache. Should be called when a property stat's
        /// value(s) have been changed.
        /// Note: Does not need to be called when a stat is added/removed, as this is taken care of automatically.
        /// </summary>
        /// <param name="stat"></param>
        public void RefreshStatCache(IStat stat)
        {
            pv_Used.Remove(stat);
            pv_Results.Remove(stat);
        }

        /// <summary>
        /// Clears the calculation cache of all stats previously passed through <see cref="CalculatedValue(INyuStat)"/>.
        /// </summary>
        public void ClearStatsCache()
        {
            pv_Used.Clear();
            pv_Results.Clear();
        }

        /// <summary>
        /// Base constructor of the default stacking.
        /// </summary>
        /// <param name="parent"></param>
        public DefaultStacking(EntityStats parent) : base(parent)
        {
            Parent.OnStatAdded += Event_OnStatsUpdated;
            Parent.OnStatRemoved += Event_OnStatsUpdated;
        }

        private void Event_OnStatsUpdated(IStat obj)
        {
            if (obj.Stacking != STATS_STACKING_BASE)
            {
                HashSet<IStat> toReset = new();
                foreach (IStat stat in pv_Used)
                {
                    //In a nutshell, it checks if a stat has been added which would affect any of the existing "calculated" stats,
                    //and removes it to recalculate it the next time a CalculatedValue is requested.
                    if (stat.Affections.Intersect(obj.Affections).Count() > 0)
                    {
                        toReset.Add(stat);
                    }
                }

                foreach (IStat stat in toReset)
                {
                    pv_Used.Remove(stat);
                    pv_Results.Remove(stat);
                }
            }
        }

        private readonly HashSet<IStat> pv_Used = new();
        private readonly Dictionary<IStat, float> pv_Results = new();
    }
}
