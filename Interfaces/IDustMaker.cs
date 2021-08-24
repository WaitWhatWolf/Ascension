using Ascension.Attributes;
using Ascension.Enums;
using Microsoft.Xna.Framework;

namespace Ascension.Interfaces
{
    /// <summary>
    /// Class for making dust.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/08/24 4:27:36")]
    public interface IDustMaker
    {
        /// <summary>
        /// **Another one bites the dust**
        /// </summary>
        void Create(Vector2 position);
    }
}
