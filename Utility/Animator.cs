using Ascension.Attributes;
using Ascension.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Utility
{
    /// <summary>
    /// Advanced animation for frame-based animations.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/05 15:56:21")]
    public sealed class Animator
    {
        public class Animation
        {
            public Animation(string name, bool loops, params Frame[] frames)
            {
                Name = name;
                Frames = frames;
                Loops = loops;
                CurrentIndex = -1;
                Speed = 1f;

                Array.Sort(Frames, new FrameComparer());

                TotalPlayback = Frames.Last().Time;
            }

            public string Name { get; }
            public bool Loops { get; }
            public Frame[] Frames { get; }
            /// <summary>
            /// The total duration of the animation.
            /// </summary>
            public float TotalPlayback { get; }
            public float Playback { get; private set; }
            public int CurrentIndex { get; private set; }
            public int IFrameIndex => Frames[CurrentIndex].IFrame;
            protected internal float Speed;

            protected internal bool Update()
            {
                if (CurrentIndex == -1)
                    goto False;

                Playback += ASCResources.FLOAT_PER_FRAME * Speed;

                if (Frames.Length - 1 == CurrentIndex || Playback >= Frames[CurrentIndex + 1].Time)
                {
                    CurrentIndex++;
                    if (CurrentIndex >= Frames.Length)
                    {
                        if(Loops)
                        {
                            Play();
                            return true;
                        }

                    Stop:
                        CurrentIndex = -1;
                        return true;
                    }
                }

            False:
                return false;
            }

            protected internal void Play()
            {
                CurrentIndex = 0;
                Playback = 0f;
            }

            private class FrameComparer : IComparer<Frame>
            {
                public int Compare(Frame x, Frame y)
                {
                    return x.Index.CompareTo(y.Index);
                }
            }
        }

        public struct Frame
        {
            public float Time;
            public int Index;
            public int IFrame;

            public Frame(int index, int frame, float time)
            {
                Index = index;
                IFrame = frame;
                Time = time;
            }
        }

        /// <summary>
        /// Called when an animation finishes playback.
        /// </summary>
        public event Action<Animation, int> OnAnimationEnd;
        
        /// <summary>
        /// Called when an animation starts playback.
        /// </summary>
        public event Action<Animation, int> OnAnimationStart;

        /// <summary>
        /// All animations of this <see cref="Animator"/>.
        /// </summary>
        public Animation[] Animations { get; }

        /// <summary>
        /// Returns an animation with the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Animation GetAnimation(string animationName)
        {
            return Array.Find(Animations, a => a.Name == animationName);
        }

        /// <summary>
        /// Shorthand for Animations[CurrentAnimationIndex].IFrameIndex; Checks for -1 before accessing array;
        /// If -1, returns 0.
        /// </summary>
        public int SafeIFrameIndex => CurrentAnimationIndex == -1 ? 0 : Animations[CurrentAnimationIndex].IFrameIndex;

        /// <summary>
        /// Currently played animation. -1 if none are playing.
        /// </summary>
        public int CurrentAnimationIndex { get; private set; }

        /// <summary>
        /// Animation towards which this <see cref="Animator"/> will return when an animation is complete.
        /// </summary>
        public int DefaultAnimationIndex { get; }

        /// <summary>
        /// Multiplier of the animation playback speed. 1f by default.
        /// </summary>
        public float Speed
        {
            get => pv_Speed;
            set
            {
                foreach (Animation animation in Animations)
                    animation.Speed = value;

                pv_Speed = value;
            }
        }

        /// <summary>
        /// Handles the animator; To be called every frame in an update method.
        /// </summary>
        public void Update()
        {
            if (CurrentAnimationIndex != -1)
            {
                if (Animations[CurrentAnimationIndex].Update())
                {
                    OnAnimationEnd?.Invoke(Animations[CurrentAnimationIndex], CurrentAnimationIndex);
                    if (!Animations[CurrentAnimationIndex].Loops)
                        Play(DefaultAnimationIndex);
                }
            }
        }

        /// <summary>
        /// Attempts to play an animation by name.
        /// </summary>
        /// <param name="animationName">Name of the animation to play.</param>
        /// <returns>True if the animation was successfully started, false if the animation is already playing, null if an animation
        /// under the given name was not found.</returns>
        public bool? Play(string animationName)
        {
            int index = Array.FindIndex(Animations, a => a.Name == animationName);

            if (index == -1)
                return null;

            return Play(index);
        }

        /// <summary>
        /// Attempts to play an animation by index.
        /// </summary>
        /// <param name="index">Index of the animation in the array.</param>
        /// <returns>True if the animation was successfully started, false if the animation is already playing.</returns>
        /// <exception cref="IndexOutOfRangeException"/>
        public bool Play(int index)
        {
            if (index < 0 || index >= Animations.Length)
                throw new IndexOutOfRangeException();

            if (CurrentAnimationIndex == index)
                return false;

            CurrentAnimationIndex = index;
            Animations[index].Play();

            OnAnimationStart?.Invoke(Animations[index], index);

            return true;
        }

        /// <summary>
        /// Restarts the current animation (if one is playing).
        /// </summary>
        /// <returns></returns>
        public void Restart()
        {
            if (CurrentAnimationIndex == -1)
                return;

            Animations[CurrentAnimationIndex].Play();
        }

        /// <summary>
        /// Creates a copy of an existing animator.
        /// </summary>
        /// <param name="other"></param>
        public Animator(Animator other)
        {
            DefaultAnimationIndex = other.DefaultAnimationIndex;
            Animations = new Animation[other.Animations.Length];
            Array.Copy(other.Animations, Animations, other.Animations.Length);
            CurrentAnimationIndex = -1;
            pv_Speed = other.pv_Speed;

            if (DefaultAnimationIndex != -1)
                Play(DefaultAnimationIndex);
        }

        public Animator(int defaultAnimationIndex = -1, params Animation[] animations)
        {
            DefaultAnimationIndex = defaultAnimationIndex;
            Animations = animations;
            CurrentAnimationIndex = -1;
            pv_Speed = 1f;

            if (DefaultAnimationIndex != -1)
                Play(DefaultAnimationIndex);
        }

        public static implicit operator int(Animator animator)
            => animator.SafeIFrameIndex;

        private float pv_Speed;
    }
}
