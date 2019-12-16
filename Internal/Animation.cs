using System;
using System.Collections.Generic;
using System.Diagnostics;
using WarWolfWorks_Mod.Interfaces;
using WarWolfWorks_Mod.Internal.Debugging.Exceptions;

namespace WarWolfWorks_Mod.Internal
{
    /// <summary>
    /// Used for multi-sprite animating.
    /// </summary>
    public sealed class Animation : IResetable
    {
        private KeyValuePair<TimeSpan, string>[] Keys;
        private Stopwatch Timer;

        private int CurrentIndex = 0;

        private int HighestIndex
        {
            get
            {
                try
                {
                    int toReturn = 0;

                    for (int i = 0; i < Keys.Length; i++)
                    {
                        if (Keys[i].Key > Keys[toReturn].Key)
                            toReturn = i;
                    }

                    if (toReturn == 0)
                        throw new AnimationKeyNullException(this);

                    return toReturn;
                }
                catch
                {
                    throw new AnimationKeyNullException(this);
                }
            }
        }

        private string CurrentTexture
        {
            get
            {
                try
                {
                Start:
                    for(int i = CurrentIndex + 1; i < Keys.Length; i++)
                    {
                        if (Timer.Elapsed > Keys[i].Key)
                            CurrentIndex = i;
                    }

                    if(CurrentIndex == HighestIndex)
                    {
                        Reset();
                        goto Start;
                    }

                    return Keys[CurrentIndex].Value;
                }
                catch
                {
                    throw new AnimationKeyNullException(this);
                }
            }
        }

        /// <summary>
        /// Starts the animation.
        /// </summary>
        public void Start()
        {
            Timer.Start();
        }

        /// <summary>
        /// Resets this <see cref="Animation"/> to it's state after <see cref="Start"/> was called.
        /// </summary>
        public void Reset()
        {
            Timer.Reset();
            CurrentIndex = 0;
        }

        /// <summary>
        /// Initiates the Animation.
        /// </summary>
        public Animation(params KeyValuePair<TimeSpan,string>[] keys)
        {
            Timer = new Stopwatch();
            Keys = keys;
        }

        /// <summary>
        /// Creates a duplicate animation.
        /// </summary>
        /// <param name="dup"></param>
        public Animation(Animation dup)
        {
            Timer = new Stopwatch();
            Keys = dup.Keys;
        }

        /// <summary>
        /// Returns the Texture name to be played/displayed.
        /// </summary>
        /// <param name="anim"></param>
        public static implicit operator string(Animation anim)
            => anim.CurrentTexture;
    }
}
