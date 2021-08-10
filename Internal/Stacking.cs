using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Ascension.Internal
{
    /// <summary>
    /// Base class to use to apply to a <see cref="EntityStats.Stacking"/>.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 10)]
    public abstract class Stacking : IStacking
    {
        /// <summary>
        /// The parent stats to be handled.
        /// </summary>
        public EntityStats Parent { get; }

        /// <summary>
        /// What will be used inside <see cref="Stats"/>.
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        public abstract float CalculatedValue(IStat stat);

        /// <summary>
        /// Gets all <see cref="INyuStat"/> with the given affections from <see cref="AllStats"/>.
        /// </summary>
        /// <param name="affections"></param>
        /// <returns></returns>
        public IStat[] GetStatsByAffections(int[] affections) => GetStatsByAffections(AllStats, affections);

        /// <summary>
        /// Gets all <see cref="INyuStat"/> with the given affections from a custom list.
        /// </summary>
        /// <param name="stats"></param>
        /// <param name="affections"></param>
        /// <returns></returns>
        public static IStat[] GetStatsByAffections(IEnumerable<IStat> stats, int[] affections)
        {
            List<IStat> toReturn = new List<IStat>();
            foreach (IStat s in stats)
            {
                if (s.Affections.Intersect(affections).Any()) toReturn.Add(s);
            }

            return toReturn.ToArray();
        }
        /// <summary>
        /// Gets all <see cref="INyuStat"/> with the given stacking from <see cref="AllStats"/>.
        /// </summary>
        /// <param name="stacking"></param>
        /// <returns></returns>
        public float[] GetStatValuesByStacking(int stacking) => GetStatValuesByStacking(AllStats, stacking);
        /// <summary>
        /// Gets all <see cref="INyuStat"/> with the given stacking from a custom list.
        /// </summary>
        /// <param name="stats"></param>
        /// <param name="stacking"></param>
        /// <returns></returns>
        public static float[] GetStatValuesByStacking(IEnumerable<IStat> stats, int stacking)
        {
            List<float> toReturn = new List<float>();
            foreach (IStat s in stats)
            {
                if (s.Stacking == stacking) toReturn.Add(s.Value);
            }

            return toReturn.ToArray();
        }

        /// <summary>
        /// Base constructor of the stacking class.
        /// </summary>
        /// <param name="parent"></param>
        public Stacking(EntityStats parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// All stats of the parent <see cref="Stats"/> class.
        /// </summary>
        protected HashSet<IStat> AllStats => Parent.pv_Stats;
    }
}
