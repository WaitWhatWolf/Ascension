using Ascension.Players;

namespace Ascension.Interfaces
{
    /// <summary>
    /// Interface which implements a stand reference.
    /// </summary>
    public interface IStandReferencable
    {
        /// <summary>
        /// The stand reference.
        /// </summary>
        Stand Stand { get; }
    }
}
