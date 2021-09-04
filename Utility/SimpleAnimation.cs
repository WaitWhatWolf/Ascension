using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Interfaces;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Ascension.Utility
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/04 17:03:56")]
    public class SimpleAnimation : ICountdown
    {
        /// <summary>
        /// Creates a simple animation handler.
        /// </summary>
        /// <param name="maxFrames">The amount of frames in the animation.</param>
        /// <param name="onFrameChanged">Called when the animation's frame is updated; Min value is the current frame, Max is the max
        /// frames of the animation.</param>
        /// <param name="speed">Speed of the animation in countdown.</param>
        public SimpleAnimation(int maxFrames, Action<IntRange> onFrameChanged, float speed = ASCResources.FLOAT_PER_FRAME)
        {
            Countdown = speed;
            pv_MaxIndex = maxFrames;
            pv_OnIndexUpdated = onFrameChanged;
        }

        /// <summary>
        /// The speed of the animation.
        /// </summary>
        public float Countdown { get; }

        /// <summary>
        /// Current countdown left to change the animation frame.
        /// </summary>
        /// <returns></returns>
        public float GetCurrentCountdown() => pv_CurCountdown;

        /// <summary>
        /// Handles the countdown; To be called every frame in an update method.
        /// </summary>
        public void UpdateAnimation()
        {
            if(Hooks.MathF.ProcessCountdown(ref pv_CurCountdown, Countdown))
            {
                pv_Index++;
                if (pv_Index >= pv_MaxIndex)
                    pv_Index = 0;

                pv_OnIndexUpdated?.Invoke(new(pv_Index, pv_MaxIndex));
            }
        }

        /// <summary>
        /// Resets the animation index, the countdown, and calls on frame changed event.
        /// </summary>
        public void Reset()
        {
            pv_CurCountdown = Countdown;
            pv_Index = 0;
            pv_OnIndexUpdated?.Invoke(new(pv_Index, pv_MaxIndex));
        }

        private float pv_CurCountdown;
        private int pv_Index;
        private readonly int pv_MaxIndex;
        private readonly Action<IntRange> pv_OnIndexUpdated;
    }
}
