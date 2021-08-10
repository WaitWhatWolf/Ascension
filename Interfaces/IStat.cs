using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Internal;

namespace Ascension.Interfaces
{
    /// <summary>
    /// Interface for implementing a stat to be used with an <see cref="IAscensionEntity"/>.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 10)]
    public interface IStat
    {
        /// <summary>
        /// <see cref="float"/> Value returned by this stat.
        /// </summary>
        float Value { get; set; }
        /// <summary>
        /// Which other stats will it interact with.
        /// </summary>
        int[] Affections { get; set; }
        /// <summary>
        /// How the value is calculated by a <see cref="INyuStacking"/>.
        /// </summary>
        int Stacking { get; set; }
        /// <summary>
        /// Invoked when this <see cref="INyuStat"/> is added to <see cref="Stats"/>.
        /// </summary>
        /// <param name="to"></param>
        void OnAdded(EntityStats to);
        /// <summary>
        /// Invoked when this <see cref="INyuStat"/> is removed from <see cref="Stats"/>.
        /// </summary>
        /// <param name="to"></param>
        void OnRemoved(EntityStats to);
    }
}
