using System;
using System.Collections.Generic;

namespace WarWolfWorks_Mod.Internal.Debugging.Exceptions
{
    /// <summary>
    /// Thrown by <see cref="Animation"/> when an <see cref="Animation"/> class was constructed using the default constructor instead of <see cref="Animation.Animation(KeyValuePair{TimeSpan, string}[])"/>.
    /// </summary>
    public sealed class AnimationKeyNullException : Exception
    {
        private string ActMsg;

        /// <summary>
        /// Returns the default message of <see cref="AnimationKeyNullException"/>.
        /// </summary>
        public override string Message => ActMsg;

        /// <summary>
        /// Throws a <see cref="AnimationKeyNullException"/>.
        /// </summary>
        /// <param name="thrower"></param>
        public AnimationKeyNullException(Animation thrower) : base()
        {
            ActMsg = $"{thrower} was instantiated with the default Animation constructor or was passed a null value.";
        }
    }
}
