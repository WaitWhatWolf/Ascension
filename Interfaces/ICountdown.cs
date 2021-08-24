using Ascension.Attributes;
using Ascension.Enums;

namespace Ascension.Interfaces
{
    /// <summary>
    /// Base class for advanced cooldowns and countdowns.
    /// Usually has a method which should be called every frame.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 23)]
    public interface ICountdown
    {
        /// <summary>
        /// The current countdown.
        /// </summary>
        /// <returns></returns>
        float GetCurrentCountdown();
        /// <summary>
        /// The amount of time that the countdown will be counted down for.
        /// </summary>
        float Countdown { get; }
    }
}
