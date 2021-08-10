using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Internal;

namespace Ascension.Interfaces
{
    /// <summary>
    /// Indicates that an entity is an ascension entity.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 10)]
    public interface IAscensionEntity
    {
        EntityStats Stats { get; }
    }
}
