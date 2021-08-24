using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Interfaces;
using System;

namespace Ascension.Utility
{
    /// <summary>
    /// A countdown which calls a given event every time the countdown is done.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 23)]
    [Note(Dev.WaitWhatWolf, "Maybe do a list of events, easier to check for null")]
    public sealed class EventCountdown : ICountdown
    {
        /// <summary>
        /// The length of this <see cref="EventCountdown"/>.
        /// </summary>
        public float Countdown { get; }

        /// <summary>
        /// Handles the countdown; Must be called every frame in an update method.
        /// </summary>
        public void UpdateCountdown()
        {
            if(Hooks.MathF.ProcessCountdown(ref pv_CurCountdown, Countdown))
            {
                pv_Event();
            }
        }

        /// <inheritdoc/>
        public float GetCurrentCountdown() => pv_CurCountdown;

        /// <summary>
        /// Adds an event to the triggered events.
        /// </summary>
        /// <param name="event"></param>
        public void AddEvent(Action @event) => pv_Event += @event;

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
        public EventCountdown(float countdown, bool startAsTrue = false)
        {
            Countdown = countdown;
            pv_CurCountdown = !startAsTrue ? countdown : 0;
        }

        public EventCountdown(Action @event, float countdown, bool startsAsTrue = false)
        {
            if (@event == null)
                throw new ArgumentNullException("Event passed in an EventCountdown constructor cannot be null.");
        }

        /// <summary>
        /// Returns a <see cref="ReturnCountdown"/> where the countdown is set to the float value.
        /// </summary>
        /// <param name="val"></param>
        public static implicit operator EventCountdown(float val)
            => val;

        private float pv_CurCountdown;

        /// <summary>
        /// The event which is called every time the countdown is done.
        /// </summary>
        private event Action pv_Event;
    }
}
