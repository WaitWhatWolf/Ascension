using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Interfaces;

namespace Ascension.Utility
{
    /// <summary>
    /// A countdown which has the <see cref="ReturnCountdown.CooldownDone"/> method (to be called every frame, in an update method)
    /// which returns true once when the countdown is done.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 23)]
    public sealed class ReturnCountdown : ICountdown
    {
        /// <summary>
        /// The length of this <see cref="ReturnCountdown"/>.
        /// </summary>
        public float Countdown { get; }

        /// <summary>
        /// Returns true when the countdown is ready, then resets the countdown. (to be called every frame in an update method)
        /// </summary>
        /// <remarks>You can also call the countdown field instead as it has an implicit operator call.</remarks>
        /// <returns></returns>
        public bool CooldownDone() => Hooks.MathF.ProcessCountdown(ref pv_CurCountdown, Countdown);

        /// <inheritdoc/>
        public float GetCurrentCountdown() => pv_CurCountdown;

        public void Reset()
        {
            pv_CurCountdown = Countdown;
        }

        /// <summary>
        /// Creates a new <see cref="ReturnCountdown"/>.
        /// </summary>
        /// <param name="countdown">The length of the countdown.</param>
        /// <param name="startAsTrue">If true, the current countdown will be set to 0, which means that <see cref="CooldownDone"/> 
        /// will return true in the very first frame it is called.</param>
        public ReturnCountdown(float countdown, bool startAsTrue = false)
        {
            Countdown = countdown;
            pv_CurCountdown = !startAsTrue ? countdown : 0;
        }

        /// <summary>
        /// Returns a <see cref="ReturnCountdown"/> where the countdown is set to the float value.
        /// </summary>
        /// <param name="val"></param>
        public static implicit operator ReturnCountdown(float val)
            => new(val);

        /// <summary>
        /// Implicitly calls <see cref="CooldownDone"/>.
        /// </summary>
        /// <param name="countdown"></param>
        public static implicit operator bool(ReturnCountdown countdown)
            => countdown.CooldownDone();

        private float pv_CurCountdown;
    }
}
